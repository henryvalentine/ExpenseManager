using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ExpenseManager.CoreFramework;
using ExpenseManager.CoreFramework.AlertControl;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;
using xPlug.BusinessObject.CustomizedASPBusinessObject.Enum;
using xPlug.BusinessService;
using xPlug.BusinessService.CustomizedASPBusinessService;

namespace ExpenseManager.ExpenseMgt
{
    public partial class FrmVoidTransaction : UserControl
    {
        protected int Offset = 5;
        protected int DataCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckUserApproverPrivilege())
                {
                    ConfirmAlertBox1.ShowMessage("Sorry. You do not have the privilege to perform this operation.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }
                
                Limit = 20;
                GetPageLimits();
                ddlLimit.SelectedValue = "20";
                LoadTransactions();
                dvTransactionItems.Visible = false;
                dvUserEntries.Visible = true;
            }
        }
        #region Page Events
        protected void BtnDateFilterClick(object sender, EventArgs e)
        {
            if (!ValidateDateControls())
            {
                return;
            }

            GetTransactionsByDate();

            //SetTransactionsStyle();
        }
        protected void DgUserInitiatedTransactionsItemCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {

                dgUserInitiatedTransactions.SelectedIndex = e.Item.ItemIndex;

                long id = (DataCheck.IsNumeric(dgUserInitiatedTransactions.DataKeys[e.Item.ItemIndex].ToString())) ? long.Parse(dgUserInitiatedTransactions.DataKeys[e.Item.ItemIndex].ToString()) : 0;

                if (id < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid Record Selection!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var transaction = ServiceProvider.Instance().GetExpenseTransactionServices().GetExpenseTransaction(id);

                if (transaction == null || transaction.ExpenseTransactionId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid record selection.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                LoadTransactionItems(transaction);

            }
            catch
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin. Please try again soon or contact your site Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        private bool CheckUserApproverPrivilege()
        {
            var portalUserId = int.Parse(new PortalServiceManager().GetUserIdByUsername(HttpContext.Current.User.Identity.Name).ToString(CultureInfo.InvariantCulture));

            var delegateService = new ApprovalDelegateService();

            if (!delegateService.CheckDelegate(portalUserId))
            {
                Response.Redirect("XpenseManager.aspx?tabParentId=1&tabId=35&tabtype=1&tabOrder=1");
                return false;
            }
            return true;
        }
        #endregion

        #region Page Helpers
        public string GetUserFullName(int userId)
        {
            try
            {
                var user = new PortalServiceManager().GetPortalUserById(userId);
                return user.FirstName + " " + user.LastName;
            }
            catch (Exception)
            {
                return "N/A";
            }
        }
        private void LoadTransactions()
        {
            dgUserInitiatedTransactions.DataSource = new List<ExpenseTransaction>();
            dgUserInitiatedTransactions.DataBind();
            var expTransactionList = ServiceProvider.Instance().GetExpenseTransactionServices().GetApprovedTransactionsByDate(DateMap.GetLocalDate());

            if (expTransactionList == null || !expTransactionList.Any())
            {
                return;
            }

            dgUserInitiatedTransactions.DataSource = expTransactionList;
            dgUserInitiatedTransactions.DataBind();
            SetTransactionsStyle();
        }
        private void GetTransactionsByDate()
        {
            try
            {
                dgUserInitiatedTransactions.DataSource = new List<ExpenseTransaction>();
                dgUserInitiatedTransactions.DataBind();

                if (!string.IsNullOrEmpty(txtStart.Text.Trim()) && !string.IsNullOrEmpty(txtEndDate.Text.Trim()))
                {
                    var startDateString = DateMap.ReverseToServerDate(txtStart.Text.Trim());
                    var startDate = DateTime.Parse(startDateString);
                    var endDateString = DateMap.ReverseToServerDate(txtEndDate.Text.Trim());
                    var endDate = DateTime.Parse(endDateString);
                    if (endDate < startDate || startDate > endDate)
                    {
                        ConfirmAlertBox1.ShowMessage("The 'From' date must not be LATER than the 'TO' date.", ConfirmAlertBox.PopupMessageType.Error);
                        dgUserInitiatedTransactions.DataSource = new List<ExpenseTransaction>();
                        dgUserInitiatedTransactions.DataBind();
                        return;
                    }

                    var expTransactionsByDate = ServiceProvider.Instance().GetExpenseTransactionServices().GetApprovedTransactionsByDateRange(startDate, endDate);
                    if (!expTransactionsByDate.Any())
                    {
                        ConfirmAlertBox1.ShowMessage("No record found.", ConfirmAlertBox.PopupMessageType.Error);
                        return;
                    }

                    Session["_expTransactionListToVoid"] = null;
                    Session["_expTransactionListToVoid"] = expTransactionsByDate;
                    Limit = int.Parse(ddlLimit.SelectedValue);
                    FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_expTransactionListToVoid", Navigation.None, Limit, LoadMethod);
                    SetTransactionsStyle();

                }

            }
            catch (Exception ex)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

            }

        }
        private bool ValidateDateControls()
        {
            try
            {
                if (string.IsNullOrEmpty(txtStart.Text.Trim()) && string.IsNullOrEmpty(txtEndDate.Text.Trim()))
                {
                    ConfirmAlertBox1.ShowMessage("Please enter a date range.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                if (string.IsNullOrEmpty(txtStart.Text.Trim()) || string.IsNullOrEmpty(txtStart.Text.Trim()))
                {
                    ConfirmAlertBox1.ShowMessage("Please enter a date range.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                if (!string.IsNullOrEmpty(txtStart.Text.Trim()))
                {
                    if (!DateMap.IsDate(DateMap.ReverseToServerDate(txtStart.Text.Trim())))
                    {
                        ConfirmAlertBox1.ShowMessage("Please enter a valid date.", ConfirmAlertBox.PopupMessageType.Error);
                        return false;
                    }
                }


                if (!string.IsNullOrEmpty(DateMap.ReverseToServerDate(txtEndDate.Text.Trim())))
                {
                    if (!DateMap.IsDate(DateMap.ReverseToServerDate(txtEndDate.Text.Trim())))
                    {
                        ConfirmAlertBox1.ShowMessage("Please enter a valid date.", ConfirmAlertBox.PopupMessageType.Error);
                        return false;
                    }
                }

                return true;
            }
            catch
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }
        }
        private void SetTransactionsStyle()
        {
            try
            {
                if (dgUserInitiatedTransactions.Items.Count > 0)
                {
                    for (var i = 0; i < dgUserInitiatedTransactions.Items.Count; i++)
                    {
                        var approvedLabel = ((Label)dgUserInitiatedTransactions.Items[i].FindControl("lblUserTransactionStatus"));

                        //var approvedDeleteImage = ((ImageButton)dgUserInitiatedTransactions.Items[i].FindControl("imgDelete"));

                        if (approvedLabel.Text == "Approved")
                        {
                            approvedLabel.Style.Add("color", "darkcyan");
                            approvedLabel.Style.Add("font-weight", "bold");

                        }

                    }

                }
            }
            catch
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        #endregion

        #region Void Transaction
        #region Events
        protected void BtnVoidTransactionClick(object sender, EventArgs e)
        {
            if (!ValidateApprovalControls())
            {
                return;
            }

            const string subject = "Transaction Approval Notification!";

            string message = "";

             var transaction = Session["_transaction"] as ExpenseTransaction;

            if (transaction == null || transaction.ExpenseTransactionId < 1)
            {
                ConfirmAlertBox1.ShowMessage("Session has expired. A notification of your decision on the Transaction request cannot be sent.", ConfirmAlertBox.PopupMessageType.Error);
                return;
            }

            if (rdVoidItems.Checked)
            {
                if (Session["_idArrays"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Please <b>select at least one item</b> on the grid or click the <b>Select All</b> checkbox.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var idArrays = Session["_idArrays"] as List<int>;

                if (idArrays == null || !idArrays.Any())
                {
                    ConfirmAlertBox1.ShowMessage("Please use the <b>checkboxes to select at least one item on the grid or click the <b>Select All</b> checkbox.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }
               
                if (Session["_transactionItems"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var transactionItems = Session["_transactionItems"] as List<TransactionItem>;

                if (transactionItems == null || !transactionItems.Any())
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var voidedTransactionItemList = (from int id in idArrays select transactionItems.Find(m => m.TransactionItemId == id)).ToList();
              
                if (!voidedTransactionItemList.Any())
                {
                    ConfirmAlertBox1.ShowMessage("An unknown error was encountered. The process call was invalid.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                if (voidedTransactionItemList.Count == transactionItems.Count)
                {
                    transaction.Status = 3;
                }

                if (!UpdateTransactionAndItems(voidedTransactionItemList))
                {
                    return;
                }

                message = "One or more Items of your Transaction request: " + transaction.ExpenseTitle +
                          " has been VOIDED.";
            }

            if (rdVoidTransaction.Checked)
            {

                if (!UpdateTransaction())
                {

                    return;
                }

                message = "Your Transaction request: " + transaction.ExpenseTitle + " was VOIDED.";
            }
            
            if (Session["_transaction"] == null)
            {
                ConfirmAlertBox1.ShowMessage("Session has expired. A notification of your decision on the Transaction request cannot be sent.", ConfirmAlertBox.PopupMessageType.Error);
                return;
            }
                
            var user = Membership.GetUser(transaction.RegisteredById);

            if (user == null)
            {
                ConfirmAlertBox1.ShowMessage("The Transaction requester's details could not be retrieved. A notification of your decision on the Transaction request cannot be sent.", ConfirmAlertBox.PopupMessageType.Error);
                return;
            }
           

            if (!Mailsender(user.Email, subject, message))
            {
                ConfirmAlertBox1.ShowMessage("A notification of your decision on the Transaction request could not be sent.", ConfirmAlertBox.PopupMessageType.Error);

            }

            if (!string.IsNullOrEmpty(txtStart.Text.Trim()) && !string.IsNullOrEmpty(txtEndDate.Text.Trim()))
            {
                if (ValidateDateControls())
                {
                    GetTransactionsByDate();

                }
            }
            else
            {
                LoadTransactions();
            }
            Session["_transaction"] = null;
            SetTransactionsStyle();
            dvUserEntries.Visible = true;
            dgExpenseItem.DataSource = new List<TransactionItem>();
            dgExpenseItem.DataBind();
            dvTransactionItems.Visible = false;
            dvUserEntries.Visible = true;
            ConfirmAlertBox1.ShowSuccessAlert("Your decision on the Transaction was successfully submitted and a notification was successfully sent to the requester.");

        }
        protected void BtnBackNavClick(object sender, EventArgs e)
        {
            Session["_transaction"] = null;
            SetTransactionsStyle();
            dvUserEntries.Visible = true;
            dgExpenseItem.DataSource = new List<TransactionItem>();
            dgExpenseItem.DataBind();
            dvTransactionItems.Visible = false;
            dvUserEntries.Visible = true;
        } 
        #endregion

        #region Helpers
        private void LoadTransactionItems(ExpenseTransaction transaction)
        {
            rdVoidTransaction.Checked = false;
            rdVoidItems.Checked = false;
            
            try
            {
                var transactionItems = ServiceProvider.Instance().GetTransactionItemServices().GetTransactionItemsByExpenseTransaction(transaction.ExpenseTransactionId);

                if (transactionItems == null || !transactionItems.Any())
                {
                    ConfirmAlertBox1.ShowMessage("Transaction Items could not be retrieved.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var dSource = transactionItems.ElementAt(0).Key.ToList();

                lblTransactionTitle.Text = transaction.ExpenseTitle;
                lblRequestedAmmount.InnerText = "N" + NumberMap.GroupToDigits(transaction.TotalTransactionAmount.ToString(CultureInfo.InvariantCulture));
                lblApprovedTotalAmount.InnerText = "N" + NumberMap.GroupToDigits(transaction.TotalApprovedAmount.ToString(CultureInfo.InvariantCulture));
                dgExpenseItem.DataSource = dSource;
                dgExpenseItem.DataBind(); 
                LoadTransactionsFooter();
                Session["_transaction"] = transaction;
                Session["_transactionItems"] = dSource;
                dvTransactionItems.Visible = true;
                dvUserEntries.Visible = false;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }

        //public string SetId(object id)
        //{
        //    return "chkSlct" + id;
        //}

        private bool UpdateTransactionAndItems(List<TransactionItem> voidedTransactionItemList)
        {
            try
            {
                if (Session["_transaction"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                var transaction = Session["_transaction"] as ExpenseTransaction;

                if (transaction == null || transaction.ExpenseTransactionId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                transaction.ApproverComment = txtApproverComment.Text.Trim();
                //transaction.DateApproved = DateMap.GetLocalDate();
                //transaction.TimeApproved = DateMap.GetLocalTime();
                transaction.ApproverId = int.Parse(new PortalServiceManager().GetUserIdByUsername(HttpContext.Current.User.Identity.Name).ToString(CultureInfo.InvariantCulture));

                if (!ServiceProvider.Instance().GetTransactionItemServices().UpdateVoidedTransactionItemsAndTransaction(transaction, voidedTransactionItemList))
                {
                    ConfirmAlertBox1.ShowMessage("The Voiding process could not be completed. Please try again.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                return true;

            }
            catch
            {
                ConfirmAlertBox1.ShowMessage("The Voiding process could not be completed. Please try again.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }
        }
        private bool UpdateTransaction()
        {
            try
            {
                if (Session["_transaction"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                var transaction = Session["_transaction"] as ExpenseTransaction;

                if (transaction == null || transaction.ExpenseTransactionId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }
                
                if (rdVoidTransaction.Checked)
                {
                    transaction.Status = 3;
                }

                if (!ServiceProvider.Instance().GetExpenseTransactionServices().UpdateExpenseTransaction(transaction))
                {
                    ConfirmAlertBox1.ShowMessage("The Approval process could not be completed. Please try again.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                return true;

            }
            catch
            {
                ConfirmAlertBox1.ShowMessage("The void process could not be completed. Please try again.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }
        }
        private void LoadTransactionsFooter()
        {
            try
            {
                if (dgExpenseItem.Items.Count > 0)
                {
                    //int subTotalQuantity = 0;
                    //int subTotalApprovedQuantity = 0;
                    double subTotalUnitPrice = 0;
                    double subTotalApprovedPrice = 0;

                    for (var i = 0; i < dgExpenseItem.Items.Count; i++)
                    {
                        //subTotalQuantity += DataCheck.IsNumeric(((Label)dgExpenseItem.Items[i].FindControl("lblQuantity")).Text)
                        //              ? int.Parse(((Label)dgExpenseItem.Items[i].FindControl("lblQuantity")).Text) : 0;
                        //subTotalApprovedQuantity += DataCheck.IsNumeric(((Label)dgExpenseItem.Items[i].FindControl("lblApprovedQuantity")).Text)
                        //              ? int.Parse(((Label)dgExpenseItem.Items[i].FindControl("lblApprovedQuantity")).Text) : 0;
                        subTotalUnitPrice += DataCheck.IsNumeric(((Label)dgExpenseItem.Items[i].FindControl("lblUnitPrice")).Text)
                                      ? float.Parse(((Label)dgExpenseItem.Items[i].FindControl("lblUnitPrice")).Text) : 0;
                        subTotalApprovedPrice += DataCheck.IsNumeric(((Label)dgExpenseItem.Items[i].FindControl("lblApprovedUnitPrice")).Text)
                                      ? float.Parse(((Label)dgExpenseItem.Items[i].FindControl("lblApprovedUnitPrice")).Text) : 0;
                    }

                    foreach (var item in dgExpenseItem.Controls[0].Controls)
                    {
                        if (item.GetType() == typeof(DataGridItem))
                        {
                            var itmType = ((DataGridItem)item).ItemType;
                            if (itmType == ListItemType.Footer)
                            {
                                //if (((DataGridItem)item).FindControl("lblTotalQuantity") != null)
                                //{
                                //    ((Label)((DataGridItem)item).FindControl("lblTotalQuantity")).Text = subTotalQuantity.ToString(CultureInfo.InvariantCulture);
                                //}

                                //if (((DataGridItem)item).FindControl("lblTotalApprovedQuantity") != null)
                                //{
                                //    ((Label)((DataGridItem)item).FindControl("lblTotalApprovedQuantity")).Text = subTotalApprovedQuantity.ToString(CultureInfo.InvariantCulture);
                                //}

                                if (((DataGridItem)item).FindControl("lblTotalUnitPrice") != null)
                                {
                                    ((Label)((DataGridItem)item).FindControl("lblTotalUnitPrice")).Text = "N" + NumberMap.GroupToDigits(subTotalUnitPrice.ToString(CultureInfo.InvariantCulture));
                                }

                                if (((DataGridItem)item).FindControl("lblTotalApprovedUnitPrice") != null)
                                {
                                    ((Label)((DataGridItem)item).FindControl("lblTotalApprovedUnitPrice")).Text = "N" + NumberMap.GroupToDigits(subTotalApprovedPrice.ToString(CultureInfo.InvariantCulture));
                                }

                            }
                        }
                    }
                }
            }
            catch
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        private bool ValidateApprovalControls()
        {
            if (string.IsNullOrEmpty(txtApproverComment.Text.Trim()))
            {
                ConfirmAlertBox1.ShowMessage("Please supply a comment", ConfirmAlertBox.PopupMessageType.Error);
                txtApproverComment.Focus();
                return false;
            }

            if (!rdVoidTransaction.Checked && !rdVoidItems.Checked)
            {
                ConfirmAlertBox1.ShowMessage("Please check one of the VOID Options.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }

            return true;
        }
        private bool Mailsender(string to, string subject, string body)
        {
            try
            {
                var emailUtility = new ExpensemanagerEmailSenderUtility();
                var config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                var settings = (System.Net.Configuration.MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");

                if (settings != null)
                {
                    var fromAddress = new MailAddress(settings.Smtp.From);
                    ThreadPool.QueueUserWorkItem(s =>
                    {
                        if (!emailUtility.SendMail(fromAddress, to, subject, body, settings.Smtp.Network.UserName, settings.Smtp.Network.Password))
                        {
                            ConfirmAlertBox1.ShowMessage("A notification could not be sent for your Transaction request due to some technical issues.\n Approval of your request may be delayed.", ConfirmAlertBox.PopupMessageType.Error);
                            return;
                        }

                    });
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                ConfirmAlertBox1.ShowMessage("Your Transactions request notification could not be sent. Approval of your request might be delayed.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }
        }
        #endregion

        #endregion


        #region Pagination
        protected void FirstLink(object sender, EventArgs e)
        {
            try
            {
                var senderLinkArgument = int.Parse(((LinkButton)sender).CommandArgument);
                NowViewing = (int)senderLinkArgument;
                FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_expTransactionListToVoid", Navigation.Sorting, Limit, LoadMethod);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

            }

        }
       protected void FillRepeater<T>(DataGrid dg, string str, Navigation navigation, int val, Func<bool> loadMethod)
        {
            try
            {
                if (Session[str] == null)
                {
                    loadMethod();
                }
                var sessionValue = Session[str];
                var sesslist = (sessionValue as IList<T>);
                DataCount = sesslist.Count;
                if (sesslist != null)
                {
                    var objPds = new PagedDataSource { DataSource = sesslist.ToList(), AllowPaging = true, PageSize = val };
                    switch (navigation)
                    {
                        case Navigation.Next:
                            if (NowViewing < objPds.PageCount - 1)
                                NowViewing++;
                            break;
                        case Navigation.Previous:
                            if (NowViewing > 0)
                                NowViewing--;
                            break;
                        case Navigation.Last:
                            NowViewing = objPds.PageCount - 1;
                            break;
                        case Navigation.Sorting:
                            break;
                        default:
                            NowViewing = 0;
                            break;
                    }
                    objPds.CurrentPageIndex = NowViewing;
                    lblCurrentPage1.Text = "Page : " + (NowViewing + 1).ToString(CultureInfo.InvariantCulture) + " of " + objPds.PageCount.ToString(CultureInfo.InvariantCulture);
                    lblnPrev.Enabled = !objPds.IsFirstPage;
                    lblnNext.Enabled = !objPds.IsLastPage;
                    lblnFirst.Enabled = !objPds.IsFirstPage;
                    lblnLast.Enabled = !objPds.IsLastPage;
                    ActivateList();
                    if (objPds.IsFirstPage)
                    {
                        listNav2.Attributes.Add("class", "active");
                        listNav3.Attributes.Add("class", "active");
                    }
                    if (objPds.IsLastPage)
                    {
                        listNav4.Attributes.Add("class", "active");
                        listNav5.Attributes.Add("class", "active");
                    }
                    dg.DataSource = objPds;
                    dg.DataBind();
                    
                }
                
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

            }
        }
        public int NowViewing
        {
            get
            {
                object obj = ViewState["_NowViewing"];
                if (obj == null)
                    return 0;

                return (int)obj;
            }
            set
            {
                ViewState["_NowViewing"] = value;
            }
        }
        protected void ActivateList()
        {
            try
            {
                listNav2.Attributes.Add("class", "disabled");
                listNav3.Attributes.Add("class", "disabled");
                listNav4.Attributes.Add("class", "disabled");
                listNav5.Attributes.Add("class", "disabled");
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

            }
        }
        protected void LbtnNextClick(object sender, EventArgs e)
        {

            try
            {
                FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_expTransactionListToVoid", Navigation.Next, Limit, LoadMethod);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

            }
        }
        protected void LbtnFirstClick(object sender, EventArgs e)
        {

            try
            {
                FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_expTransactionListToVoid", Navigation.First, Limit, LoadMethod);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

            }
        }
        protected void LbtnLastClick(object sender, EventArgs e)
        {

            try
            {
                FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_expTransactionListToVoid", Navigation.Last, Limit, LoadMethod);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

            }
        }
        protected void LbtnPrevClick(object sender, EventArgs e)
        {

            try
            {
                FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_expTransactionListToVoid", Navigation.Previous, Limit, LoadMethod);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

            }
        }
        protected void OnLimitChange(object sender, EventArgs e)
        {

            try
            {
                Limit = int.Parse(ddlLimit.SelectedValue);
                FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_expTransactionListToVoid", Navigation.None, Limit, LoadMethod);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }


        }
        private void GetPageLimits()
        {
            var intVal = new List<int>();
            for (var i = 20; i <= 200; i += 30)
            {
                if (i == 20)
                {
                    intVal.Add(i);
                }
                if (i == 50)
                {
                    intVal.Add(i);
                }
                if (i == 80)
                {
                    i = 100;
                    intVal.Add(i);
                }

                if (i == 130)
                {
                    i = 150;
                    intVal.Add(i);
                }

                if (i == 150)
                {
                    i = 200;
                    intVal.Add(i);
                }
            }
            ddlLimit.DataSource = intVal;
            ddlLimit.DataBind();
        }
        public int Limit
        {
            get
            {
                object obj = Session["_limit"];
                if (obj == null)
                {
                    Session["_limit"] = int.Parse(ddlLimit.SelectedValue);
                    return (int)Session["_limit"];
                }

                return (int)obj;
            }
            set
            {
                Session["_limit"] = value;
            }
        }
        #region PageEventMethod
        /* Method to hold all content frm the Db*/
        protected bool LoadMethod()
        {
            if (!string.IsNullOrEmpty(txtStart.Text.Trim()) && !string.IsNullOrEmpty(txtEndDate.Text.Trim()))
            {
                GetTransactionsByDate();
            }

            if (string.IsNullOrEmpty(txtStart.Text.Trim()) && string.IsNullOrEmpty(txtEndDate.Text.Trim()))
            {
                LoadTransactions();
            }
            return true;
        }
        #endregion
        #endregion
    }
}