using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExpenseManager.CoreFramework;
using ExpenseManager.CoreFramework.AlertControl;
using xPlug.BusinessManager;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;
using xPlug.BusinessObject.CustomizedASPBusinessObject.Enum;
using xPlug.BusinessService;

namespace ExpenseManager.ExpenseMgt
{
    public partial class FrmViewTransaction : UserControl
    {
        protected int Offset = 5;
        protected int DataCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Limit = 20;
                GetPageLimits();
                ddlLimit.SelectedValue = "20";
                LoadTransactions();
                LoadFilterOptions();
                hTitle.InnerText = "Approved And Voided Transactions";
                lblRequestedAmmount.InnerText = string.Empty;
                dvTransactionItems.Visible = false;
                dvUserEntries.Visible = true;
                btnBackNav.CommandArgument = "0";
            }
        }
        #region Page Events
        protected void BtnDateFilterClick(object sender, EventArgs e)
        {
            if (int.Parse(ddlFilterOption.SelectedValue) < 0)
            {
                ConfirmAlertBox1.ShowMessage("Please select a FILTER OPTION!", ConfirmAlertBox.PopupMessageType.Error);
                return;

            }
            
            if (!ValidateDateControls())
            {
                return;
            }

            GetTransactionsByDate();

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

                if (e.CommandName == "viewDetails")
                {
                    LoadTransactionItems(transaction);
                    return;
                }

                if (e.CommandName == "viewComment")
                {
                    if (transaction.Status == 0)
                    {
                        ConfirmAlertBox1.ShowMessage("Transaction is still pending.", ConfirmAlertBox.PopupMessageType.Error);
                        return;
                    }

                    txtRejComment.Text = transaction.ApproverComment;
                    lgCommentTitle.InnerText = transaction.ExpenseTitle;
                    mpeExpenseItemsPopup.CancelControlID = "closerejection";
                    mpeExpenseItemsPopup.PopupControlID = "dvRejection";
                    mpeExpenseItemsPopup.Show();
                }

            }
            catch
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin. Please try again soon or contact your site Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
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
            List<ExpenseTransaction> expTransactionList;
            if (HttpContext.Current.User.IsInRole("PortalAdmin"))
            {
                expTransactionList = ServiceProvider.Instance().GetExpenseTransactionServices().GetCurrentApprovedOrVoidedExpenseTransactions();
            }
            else
            {
                var userId = new PortalServiceManager().GetUserIdByUsername(HttpContext.Current.User.Identity.Name);
                if (userId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Your user details could not be retrieved.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }
                expTransactionList = ServiceProvider.Instance().GetExpenseTransactionServices().GetPortalUserCurrentApprovedOrVoidedExpenseTransactions(userId);
            }

            if (expTransactionList == null || !expTransactionList.Any())
            {
                return;
            }

            Session["_approvedOrVoidedExpTransactionList"] = null;
            Session["_approvedOrVoidedExpTransactionList"] = expTransactionList;
           
            Limit = int.Parse(ddlLimit.SelectedValue);
            FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_approvedOrVoidedExpTransactionList", Navigation.None, Limit, LoadMethod);
            SetTransactionsStyle();
        }
        private void LoadFilterOptions()
        {
            var options = DataArray.ConvertEnumToArrayList(typeof(ApprovedVoidedFilterOptions));
            ddlFilterOption.DataSource = options;
            ddlFilterOption.DataValueField = "ID";
            ddlFilterOption.DataTextField = "Name";
            ddlFilterOption.DataBind();
            ddlFilterOption.Items.Insert(0, new ListItem(" -- Select Status -- ", "-1"));
            ddlFilterOption.Items.Insert(1, new ListItem("All", "0"));
            ddlFilterOption.SelectedIndex = -1;
           
        }
        private void GetTransactionsByDate()
        {
            try
            {
                dgUserInitiatedTransactions.DataSource = new List<ExpenseTransaction>();
                dgUserInitiatedTransactions.DataBind();
                
                var expTransactionsByDate = new List<ExpenseTransaction>();
                var status = int.Parse(ddlFilterOption.SelectedValue);
                var startDate =  DateTime.ParseExact(txtStart.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var endDate = DateTime.ParseExact(txtEndDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (endDate < startDate || startDate > endDate)
                {
                    ConfirmAlertBox1.ShowMessage("The 'From' date must not be LATER than the 'To' date.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                if (HttpContext.Current.User.IsInRole("PortalAdmin"))
                {
                    if (status == 0)
                    {
                        expTransactionsByDate = ServiceProvider.Instance().GetExpenseTransactionServices().GetApprovedOrVoidedExpenseTransactionsByDateRange(startDate, endDate);
                    }

                    if (status == 1)
                    {
                        expTransactionsByDate = ServiceProvider.Instance().GetExpenseTransactionServices().GetApprovedTransactionsByDateRange(startDate, endDate);
                    }

                    if (status == 2)
                    {
                        expTransactionsByDate = ServiceProvider.Instance().GetExpenseTransactionServices().GetVoidedTransactionsByDateRange(startDate, endDate);
                    }
                    
                }
                else
                {
                    var userId = new PortalServiceManager().GetUserIdByUsername(HttpContext.Current.User.Identity.Name);
                    
                    if (userId < 1)
                    {
                        ConfirmAlertBox1.ShowMessage("Your user details could not be retrieved.", ConfirmAlertBox.PopupMessageType.Error);
                        return;
                    }

                    if (status == 0)
                    {
                        expTransactionsByDate = ServiceProvider.Instance().GetExpenseTransactionServices().GetPortalUserApprovedOrVoidedExpenseTransactionsByDateRange(userId, startDate, endDate);

                    }
                    
                    if (status == 1)
                    {
                        expTransactionsByDate = ServiceProvider.Instance().GetExpenseTransactionServices().GetPortalUserApprovedTransactionsByDateRange(userId, startDate, endDate);
                    }

                    if (status == 2)
                    {
                        expTransactionsByDate = ServiceProvider.Instance().GetExpenseTransactionServices().GetPortalUserVoidedTransactionsByDateRange(userId, startDate, endDate);
                    }

                }

                if (!expTransactionsByDate.Any())
                {
                    ConfirmAlertBox1.ShowMessage("No record found.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }
                Session["_approvedOrVoidedExpTransactionList"] = null;
                Session["_approvedOrVoidedExpTransactionList"] = expTransactionsByDate;
              
                Limit = int.Parse(ddlLimit.SelectedValue);
                FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_approvedOrVoidedExpTransactionList", Navigation.None, Limit, LoadMethod);
                SetTransactionsStyle();
                
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
                        var approvedLabel = ((Label)dgUserInitiatedTransactions.Items[i].FindControl("lblApprovalStatus"));

                        if (approvedLabel.Text == "Approved")
                        {
                            approvedLabel.Style.Add("color", "darkcyan");
                            approvedLabel.Style.Add("font-weight", "bold");

                        }

                        if (approvedLabel.Text == "Voided")
                        {
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

        #region View Transaction Details
        #region Event
        protected void BtnBackNavClick(object sender, EventArgs e)
        {
            dgExpenseItem.DataSource = new List<TransactionItem>();
            dgExpenseItem.DataBind();
            lblApprovedTotalAmount.InnerText = string.Empty;
            lblRequestedAmmount.InnerText = string.Empty;
            lblTransactionTitle.Text = string.Empty;
            dvTransactionItems.Visible = false;
            hTitle.InnerText = "Approved And Voided Transactions";
            dvUserEntries.Visible = true;
        }
        #endregion

        #region Helpers
        private void ClearItemsControls()
        {
            dgExpenseItem.DataSource = new List<TransactionItem>();
            dgExpenseItem.DataBind();
            lblApprovedTotalAmount.InnerText = string.Empty;
            lblRequestedAmmount.InnerText = string.Empty;
            lblTransactionTitle.Text = string.Empty;
        }
        private void LoadTransactionItems(ExpenseTransaction transaction)
        {

            try
            {
                ClearItemsControls();

                var transactionItems = ServiceProvider.Instance().GetTransactionItemServices().GetTransactionItemsByExpenseTransaction(transaction.ExpenseTransactionId);

                if (transactionItems == null || !transactionItems.Any())
                {
                    ConfirmAlertBox1.ShowMessage("Transaction Items could not be retrieved.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }
                
                if(string.IsNullOrEmpty(lblRequestedAmmount.InnerText))
                {
                    lblRequestedAmmount.InnerText = "N" + NumberMap.GroupToDigits(transaction.TotalTransactionAmount.ToString(CultureInfo.InvariantCulture));
                }

                if(transaction.TotalApprovedAmount > 0)
                {
                    lblApprovedTotalAmount.InnerText = "N" + NumberMap.GroupToDigits(transaction.TotalApprovedAmount.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    lblApprovedTotalAmount.InnerText = NumberMap.GroupToDigits(0.ToString(CultureInfo.InvariantCulture));
                }

                lblTransactionTitle.Text = transaction.ExpenseTitle;
                dgExpenseItem.DataSource = transactionItems.ElementAtOrDefault(0).Key.ToList();
                dgExpenseItem.DataBind();
                SetTransactionsStyle();
                LoadTransactionsFooter();
                hTitle.InnerText = "Requested Transaction Items";
                dvTransactionItems.Visible = true;
                dvUserEntries.Visible = false;
                
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
               
            }
        }
        private void LoadTransactionsFooter()
        {
            try
            {
                if (dgExpenseItem.Items.Count > 0)
                {
                    //int subTotalQuantity = 0;
                    //int subtotalApprovedQuantity = 0;
                    double subTotalUnitPrice = 0;
                    double subTotalApprovedPrice = 0;

                    for (var i = 0; i < dgExpenseItem.Items.Count; i++)
                    {
                        //subTotalQuantity += DataCheck.IsNumeric(((Label)dgExpenseItem.Items[i].FindControl("lblQuantity")).Text)
                        //              ? int.Parse(((Label)dgExpenseItem.Items[i].FindControl("lblQuantity")).Text) : 0;
                        //subtotalApprovedQuantity += DataCheck.IsNumeric(((Label)dgExpenseItem.Items[i].FindControl("lblApprovedQuantity")).Text)
                        //              ? int.Parse(((Label)dgExpenseItem.Items[i].FindControl("lblApprovedQuantity")).Text) : 0;
                        subTotalUnitPrice += DataCheck.IsNumeric(((Label)dgExpenseItem.Items[i].FindControl("lblUnitPrice")).Text)
                                      ? double.Parse(((Label)dgExpenseItem.Items[i].FindControl("lblUnitPrice")).Text) : 0;
                        subTotalApprovedPrice += DataCheck.IsNumeric(((Label)dgExpenseItem.Items[i].FindControl("lblApprovedUnitPrice")).Text)
                                      ? double.Parse(((Label)dgExpenseItem.Items[i].FindControl("lblApprovedUnitPrice")).Text) : 0;
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
                                //    ((Label)((DataGridItem)item).FindControl("lblTotalApprovedQuantity")).Text = subtotalApprovedQuantity.ToString(CultureInfo.InvariantCulture);
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
        #endregion
        #endregion

        #region Pagination
        protected void FirstLink(object sender, EventArgs e)
        {
            try
            {
                var senderLinkArgument = int.Parse(((LinkButton)sender).CommandArgument);
                NowViewing = (int)senderLinkArgument;
                FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_approvedOrVoidedExpTransactionList", Navigation.Sorting, Limit, LoadMethod);
                SetTransactionsStyle();
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
                
                SetTransactionsStyle();
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
                FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_approvedOrVoidedExpTransactionList", Navigation.Next, Limit, LoadMethod);
             
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
                FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_approvedOrVoidedExpTransactionList", Navigation.First, Limit, LoadMethod);
                
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
                FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_approvedOrVoidedExpTransactionList", Navigation.Last, Limit, LoadMethod);
               
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
                FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_approvedOrVoidedExpTransactionList", Navigation.Previous, Limit, LoadMethod);
               
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
                FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_approvedOrVoidedExpTransactionList", Navigation.None, Limit, LoadMethod);
             
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