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
    public partial class FrmApproveExpenseTransaction : UserControl
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
                Session["_idArrays"] = null;
                Session["_transaction"] = null;
                Session["_transactionItem"] = null;
                Session["_transactionItems"] = null;
                btnBackNav.CommandArgument = "0";
                LoadTransactions();
                BindDefaultExpenseItem();
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
            var expTransactionList = ServiceProvider.Instance().GetExpenseTransactionServices().GetPendingExpenseTransactionsByCurrentDate();
            
            if (expTransactionList == null || !expTransactionList.Any())
            {
                return;
            }

            
            Session["_expTransactionListToApprove"] = null;
            Session["_expTransactionListToApprove"] = expTransactionList;
           
            Limit = int.Parse(ddlLimit.SelectedValue);
            FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_expTransactionListToApprove", Navigation.None, Limit, LoadMethod);
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

                        var expTransactionsByDate = ServiceProvider.Instance().GetExpenseTransactionServices().GetPendingTransactionsByDateRange(startDate, endDate);
                        if (!expTransactionsByDate.Any())
                        {
                            ConfirmAlertBox1.ShowMessage("No record found.", ConfirmAlertBox.PopupMessageType.Error);
                            return;
                        }

                        Session["_expTransactionListToApprove"] = null;
                        Session["_expTransactionListToApprove"] = expTransactionsByDate;
                       
                        Limit = int.Parse(ddlLimit.SelectedValue);
                        FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_expTransactionListToApprove", Navigation.None, Limit, LoadMethod);
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

                if (string.IsNullOrEmpty(txtStart.Text.Trim()) || string.IsNullOrEmpty(txtEndDate.Text.Trim()))
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

                        if (approvedLabel.Text == "Pending")
                        {
                            approvedLabel.Style.Add("color", "maroon");
                            approvedLabel.Style.Add("font-weight", "bold");
                            //approvedDeleteImage.Style.Add("cursor", "hand");
                        }
                       
                    }

                }
            }
            catch
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
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

        #region Approved Transaction
        #region Events
        protected void BtnProcessExpenseTransactionsClick(object sender, EventArgs e)
        {
            if (!ValidateTransactionItemsControls())
            {
                return;
            }

            if (!UpdateTransactionItem())
            {
                return;
            }

            ConfirmAlertBox1.ShowSuccessAlert("Transaction Item was successfully updated");
        }
        protected void DgExpenseTransactionEditCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                if (Session["_transactionItems"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var transactionItems = Session["_transactionItems"] as List<TransactionItem>;

                if (transactionItems == null || !transactionItems.Any())
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                dgExpenseItem.SelectedIndex = e.Item.ItemIndex;

                var id = (DataCheck.IsNumeric(dgExpenseItem.DataKeys[e.Item.ItemIndex].ToString())) ? int.Parse(dgExpenseItem.DataKeys[e.Item.ItemIndex].ToString()) : 0;

                if (id < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid Record Selection!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var transactionItem = transactionItems.Find(m => m.TransactionItemId == id);

                if (transactionItem == null || transactionItem.TransactionItemId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid selection.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                textApprouvedQuantity.Value = string.Empty;
                textApprovedUnitPrice.Value = string.Empty;
                btnUpdateTransactionItem.Text = "Update";
                ddlExpenseItem.SelectedValue = transactionItem.ExpenseItem.ExpenseItemId.ToString(CultureInfo.InvariantCulture);
                ddlExpenseType.SelectedValue = transactionItem.ExpenseTypeId.ToString(CultureInfo.InvariantCulture);
                txtQuantity.Text = transactionItem.RequestedQuantity.ToString(CultureInfo.InvariantCulture);
                txtUnitPrice.Text = transactionItem.RequestedUnitPrice.ToString(CultureInfo.InvariantCulture);
                txtTotalRequestedPrice.Value = transactionItem.TotalPrice.ToString(CultureInfo.InvariantCulture);
                btnUpdateTransactionItem.Text = "Update";
                mpeExpenseItemsPopup.Show();
                Session["_transactionItem"] = transactionItem;
            }
            catch
            {
                ConfirmAlertBox1.ShowMessage("The Edit process could not be initiated. Please try again.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        protected void BtnApproveTransactionClick(object sender, EventArgs e)
        {
            if (!ValidateApprovalControls())
            {
                return;
            }
            
            if(radApprove.Checked)
            {
                if (Session["_idArrays"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Please select at least one item on the grid or click the <b>Select All</b> checkbox.", ConfirmAlertBox.PopupMessageType.Error);
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

                var updatedTransactionItemList = (from int id in idArrays select transactionItems.Find(m => m.TransactionItemId == id)).ToList();
                //var rejectedItemList = (from int id in idArrays select transactionItems.Find(m => m.TransactionItemId != id)).ToList();

                if (!updatedTransactionItemList.Any())
                {
                    ConfirmAlertBox1.ShowMessage("An unknown error was encountered. The process call was invalid.", ConfirmAlertBox.PopupMessageType.Error);
                    return; 
                }

                if (!UpdateTransactionAndItems(updatedTransactionItemList))
                {
                    return;
                }
                
            }

            else
            {
                if (!UpdateTransaction())
                {
                    return;
                }
            }

            if (!SendNotification())
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
            SetTransactionsStyle();
            Session["_idArrays"] = null;
            Session["_transaction"] = null;
            Session["_transactionItem"] = null;
            Session["_transactionItems"] = null;
            dgExpenseItem.DataSource = new List<TransactionItem>();
            dgExpenseItem.DataBind();
            dvUserEntries.Visible = true;
            dvTransactionItems.Visible = false;
            dvUserEntries.Visible = true;
            ConfirmAlertBox1.ShowSuccessAlert("Your decision on the Transaction was successfully submitted and a notification was sent to the requester.");
                
        }
        protected void BtnBackNavClick(object sender, EventArgs e)
        {
              dgExpenseItem.DataSource = new List<TransactionItem>();
              dgExpenseItem.DataBind();
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
              SetTransactionsStyle();
              dvUserEntries.Visible = true;
              dvTransactionItems.Visible = false;
              dvUserEntries.Visible = true;
              Session["_transaction"] = null;
              Session["_transactionItem"] = null;
              Session["_transactionItems"] = null;
        }
        #endregion
        
        #region Helpers
        private void BindDefaultExpenseItem()
        {
            ddlExpenseType.DataSource = new List<ExpenseType>();
            ddlExpenseType.DataBind();
            ddlExpenseType.Items.Insert(0, new ListItem("-- List is empty --", "0"));
            ddlExpenseType.SelectedIndex = 0;
        }
        private void LoadTransactionItems(ExpenseTransaction transaction)
        {
            try
            {
                radApprove.Checked = false;
                radReject.Checked = false;
                radVoid.Checked = false;
                txtApproverComment.Text = string.Empty;
                var dictCollection = ServiceProvider.Instance().GetTransactionItemServices().GetTransactionItemsByExpenseTransaction(transaction.ExpenseTransactionId);

                if (dictCollection == null || !dictCollection.Any())
                {
                    ConfirmAlertBox1.ShowMessage("Transaction Items could not be retrieved.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var transactionItems = dictCollection.ElementAtOrDefault(0).Key.ToList();
                if (!transactionItems.Any())
                {
                    dgExpenseItem.DataSource = new List<TransactionItem>();
                    ddlExpenseItem.DataBind();
                    ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again or contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var expenseItems = dictCollection.ElementAtOrDefault(0).Value.ToList();
                
                if (!expenseItems.Any())
                {
                    ddlExpenseItem.DataSource = new List<ExpenseItem>();
                    ddlExpenseItem.DataBind();
                    ddlExpenseItem.Items.Insert(0, new ListItem("--List is empty--", "0"));
                    ddlExpenseItem.SelectedIndex = 0;
                    ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again or contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                ddlExpenseItem.DataSource = expenseItems;
                ddlExpenseItem.DataTextField = "Title";
                ddlExpenseItem.DataValueField = "ExpenseItemId";
                ddlExpenseItem.DataBind();
                ddlExpenseItem.Items.Insert(0, new ListItem("--Select Expense Item--", "0"));
                ddlExpenseItem.SelectedIndex = 0;

                if (ddlExpenseType.Items.Count == 1 || ddlExpenseType.Items.Count == 0)
                {
                    LoadTransactionTypes();
                }

                lblTransactionTitle.Text = transaction.ExpenseTitle;
                lblRequestedAmmount.InnerText = "N" + NumberMap.GroupToDigits(transaction.TotalTransactionAmount.ToString(CultureInfo.InvariantCulture));
                lblTransactionRquestDate.InnerText = transaction.TransactionDate;
                dgExpenseItem.DataSource = transactionItems;
                dgExpenseItem.DataBind();
                //SetTransactionsStyle();
                LoadTransactionsFooter();
                Session["_transactionItems"] = transactionItems;
                Session["_transaction"] = transaction;
                dvTransactionItems.Visible = true;
                dvUserEntries.Visible = false;

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        private void LoadTransactionTypes()
        {
            try
            {
                var transactionTypesList = ServiceProvider.Instance().GetExpenseTypeServices().GetActiveExpenseTypes();

                if (!transactionTypesList.Any())
                {
                    ConfirmAlertBox1.ShowMessage("The Expense Types List is empty!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                ddlExpenseType.DataSource = transactionTypesList;
                ddlExpenseType.DataTextField = "Name";
                ddlExpenseType.DataValueField = "ExpenseTypeId";
                ddlExpenseType.DataBind();
                ddlExpenseType.Items.Insert(0, new ListItem("--Select Expense Type--", "0"));
                ddlExpenseType.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);

            }
        }
        private bool UpdateTransactionItem()
        {
            try
            {
                if (Session["_transactionItems"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                var transactionItems = Session["_transactionItems"] as List<TransactionItem>;

                if (transactionItems == null || !transactionItems.Any())
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                if (string.IsNullOrEmpty(textApprouvedQuantity.Value.Trim()) && string.IsNullOrEmpty(textApprovedUnitPrice.Value.Trim()))
                {
                    ConfirmAlertBox1.ShowSuccessAlert("No changes was made.");
                    return true;
                }

                if (Session["_transactionItem"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                var transactionItem = Session["_transactionItem"] as TransactionItem;

                if (transactionItem == null || transactionItem.TransactionItemId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                var newTransactionItem = transactionItems.Find(m => m.TransactionItemId == transactionItem.TransactionItemId);

                if (newTransactionItem == null || newTransactionItem.TransactionItemId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                if (!string.IsNullOrEmpty(textApprovedUnitPrice.Value.Trim()) || double.Parse(textApprouvedQuantity.Value.Trim()) > 0)
                {
                    if (!string.IsNullOrEmpty(textApprouvedQuantity.Value.Trim()) && int.Parse(textApprouvedQuantity.Value.Trim()) > 0)
                    {
                        newTransactionItem.ApprovedQuantity = int.Parse(textApprouvedQuantity.Value.Trim());
                    }

                    if (!string.IsNullOrEmpty(textApprovedUnitPrice.Value.Trim()) && double.Parse(textApprovedUnitPrice.Value.Trim()) > 0)
                    {
                        newTransactionItem.ApprovedUnitPrice = double.Parse(textApprovedUnitPrice.Value.Replace(",", string.Empty).Replace(".", string.Empty).Trim());
                        
                    }

                    newTransactionItem.ApprovedTotalPrice = newTransactionItem.ApprovedQuantity * newTransactionItem.ApprovedUnitPrice;
                }

                if (string.IsNullOrEmpty(textApprovedUnitPrice.Value.Trim()) && double.Parse(textApprovedUnitPrice.Value.Trim()) < 1)
                {
                    newTransactionItem.ApprovedTotalPrice = 0;
                }

                transactionItems.Remove(transactionItem);
                transactionItems.Add(newTransactionItem);
                dgExpenseItem.DataSource = transactionItems.OrderBy(m => m.ExpenseItem.Title);
                dgExpenseItem.DataBind();
                LoadTransactionsFooter();
                Session["_transactionItems"] = transactionItems;
                Session["_transactionItem"] = null;
                return true;

            }
            catch
            {
                ConfirmAlertBox1.ShowMessage("The Edit process could not be completed. Please try again.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }
        }
        private bool UpdateTransactionAndItems(List<TransactionItem> updatedTransactionItemList)
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
                transaction.DateApproved = DateMap.GetLocalDate();
                transaction.TimeApproved = DateMap.GetLocalTime();
                transaction.ApproverId = int.Parse(new PortalServiceManager().GetUserIdByUsername(HttpContext.Current.User.Identity.Name).ToString(CultureInfo.InvariantCulture));
                
                if(radApprove.Checked)
                {
                    transaction.Status = 1;
                }

                if (!ServiceProvider.Instance().GetTransactionItemServices().UpdateTransactionAndItems(transaction, updatedTransactionItemList))
                {
                    ConfirmAlertBox1.ShowMessage("The Approval process could not be completed. Please try again.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                return true;

            }
            catch
            {
                ConfirmAlertBox1.ShowMessage("The Approval process could not be completed. Please try again.", ConfirmAlertBox.PopupMessageType.Error);
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
                
                transaction.ApproverComment = txtApproverComment.Text.Trim();
                transaction.DateApproved = DateMap.GetLocalDate();
                transaction.TimeApproved = DateMap.GetLocalTime();
                transaction.ApproverId = int.Parse(new PortalServiceManager().GetUserIdByUsername(HttpContext.Current.User.Identity.Name).ToString(CultureInfo.InvariantCulture));
                transaction.TotalApprovedAmount = 0;
               
                if(radReject.Checked)
                {
                    transaction.Status = 2;
                }

                if (radVoid.Checked)
                {
                    transaction.Status = 3;
                }

                if(!ServiceProvider.Instance().GetExpenseTransactionServices().UpdateExpenseTransaction(transaction))
                {
                    ConfirmAlertBox1.ShowMessage("The process could not be completed. Please try again.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                return true;

            }
            catch
            {
                ConfirmAlertBox1.ShowMessage("The required process failed. Please try again.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }
        }
        private void LoadTransactionsFooter()
        {
            try
            {
                if (dgExpenseItem.Items.Count > 0)
                {
                    double subTotalRequestedUnitPrice = 0;
                    double subRequestedTotalPrice = 0;
                    double subApprovedTotalUnitPrice = 0;
                    double subTotalApprovedPrice = 0;

                    for (var i = 0; i < dgExpenseItem.Items.Count; i++)
                    {
                        subTotalRequestedUnitPrice += DataCheck.IsNumeric(((Label)dgExpenseItem.Items[i].FindControl("lblUnitPrice")).Text)
                                      ? double.Parse(((Label)dgExpenseItem.Items[i].FindControl("lblUnitPrice")).Text) : 0;
                        subRequestedTotalPrice += DataCheck.IsNumeric(((Label)dgExpenseItem.Items[i].FindControl("lblRequestedTotalPrice")).Text)
                                      ? double.Parse(((Label)dgExpenseItem.Items[i].FindControl("lblRequestedTotalPrice")).Text) : 0;
                        subApprovedTotalUnitPrice += DataCheck.IsNumeric(((Label)dgExpenseItem.Items[i].FindControl("lblApprovedUnitPrice")).Text)
                                      ? double.Parse(((Label)dgExpenseItem.Items[i].FindControl("lblApprovedUnitPrice")).Text) : 0;
                        subTotalApprovedPrice += DataCheck.IsNumeric(((Label)dgExpenseItem.Items[i].FindControl("lblApprovedTotalPrice")).Text)
                                      ? double.Parse(((Label)dgExpenseItem.Items[i].FindControl("lblApprovedTotalPrice")).Text) : 0;
                    }

                    //lblUnitPrice lblTotalUnitPriceRequested
                    //lblRequestedTotalPrice lblTotalPriceRequested
                    //lblApprovedUnitPrice lblTotalUnitPriceApproved 
                    //lblApprovedTotalPrice lblTotalPriceApproved

                    foreach (var item in dgExpenseItem.Controls[0].Controls)
                    {
                        if (item.GetType() == typeof(DataGridItem))
                        {
                            var itmType = ((DataGridItem)item).ItemType;
                            if (itmType == ListItemType.Footer)
                            {
                                if (((DataGridItem)item).FindControl("lblTotalUnitPriceRequested") != null)
                                {
                                    ((Label)((DataGridItem)item).FindControl("lblTotalUnitPriceRequested")).Text = "N" + NumberMap.GroupToDigits(subTotalRequestedUnitPrice.ToString(CultureInfo.InvariantCulture));
                                }

                                if (((DataGridItem)item).FindControl("lblTotalPriceRequested") != null)
                                {
                                    ((Label)((DataGridItem)item).FindControl("lblTotalPriceRequested")).Text = "N" + NumberMap.GroupToDigits(subRequestedTotalPrice.ToString(CultureInfo.InvariantCulture));
                                }

                                if (((DataGridItem)item).FindControl("lblTotalUnitPriceApproved") != null)
                                {
                                    ((Label)((DataGridItem)item).FindControl("lblTotalUnitPriceApproved")).Text = "N" + NumberMap.GroupToDigits(subApprovedTotalUnitPrice.ToString(CultureInfo.InvariantCulture));
                                }

                                if (((DataGridItem)item).FindControl("lblTotalPriceApproved") != null)
                                {
                                    ((Label)((DataGridItem)item).FindControl("lblTotalPriceApproved")).Text = "N" + NumberMap.GroupToDigits(subTotalApprovedPrice.ToString(CultureInfo.InvariantCulture));
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
        private bool ValidateTransactionItemsControls()
        {
            try
            {
                if (int.Parse(ddlExpenseItem.SelectedValue) < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Please select Expense Item.", ConfirmAlertBox.PopupMessageType.Error);
                    ddlExpenseItem.Focus();
                    return false;
                }

                if (int.Parse(ddlExpenseType.SelectedValue) < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Please select Expense an Expense Type.", ConfirmAlertBox.PopupMessageType.Error);
                    ddlExpenseType.Focus();
                    return false;
                }


                if (string.IsNullOrEmpty(txtQuantity.Text.Trim()))
                {
                    ConfirmAlertBox1.ShowMessage("Please supply the Item quantity.", ConfirmAlertBox.PopupMessageType.Error);
                    txtQuantity.Focus();
                    return false;
                }

                if (string.IsNullOrEmpty(txtUnitPrice.Text.Trim()))
                {
                    ConfirmAlertBox1.ShowMessage("Please supply the Price per item.", ConfirmAlertBox.PopupMessageType.Error);
                    txtUnitPrice.Focus();
                    return false;
                }
                
               
                if (!DataCheck.IsNumeric(txtUnitPrice.Text.Trim()))
                {
                    ConfirmAlertBox1.ShowMessage("Invalid entry!", ConfirmAlertBox.PopupMessageType.Error);
                    txtUnitPrice.Focus();
                    return false;
                }

                if (!DataCheck.IsNumeric(txtQuantity.Text.Trim()))
                {
                    ConfirmAlertBox1.ShowMessage("Invalid entry!", ConfirmAlertBox.PopupMessageType.Error);
                    txtQuantity.Focus();
                    return false;
                }

                if (!string.IsNullOrEmpty(textApprouvedQuantity.Value.Trim()))
                {
                    if (!DataCheck.IsNumeric(textApprouvedQuantity.Value.Trim()))
                    {
                        ConfirmAlertBox1.ShowMessage("Invalid entry!", ConfirmAlertBox.PopupMessageType.Error);
                        txtQuantity.Focus();
                        return false;
                    }
                   
                }

                if (!string.IsNullOrEmpty(textApprovedUnitPrice.Value.Trim()))
                {
                    if (!DataCheck.IsNumeric(textApprovedUnitPrice.Value.Replace(",", string.Empty).Replace(".", string.Empty).Trim()))
                    {
                        ConfirmAlertBox1.ShowMessage("Invalid entry!", ConfirmAlertBox.PopupMessageType.Error);
                        txtQuantity.Focus();
                        return false;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
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

            if (!radApprove.Checked && !radReject.Checked && !radVoid.Checked)
            {
                ConfirmAlertBox1.ShowMessage("Please provide a decision.", ConfirmAlertBox.PopupMessageType.Error);
                radApprove.Focus();
                return false;
            }
            return true;
        }

        private bool SendNotification()
        {
            try
            {
                if (Session["_transaction"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired. A notification of your decision on the Transaction request cannot be sent.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                var transaction = Session["_transaction"] as ExpenseTransaction;

                if (transaction == null || transaction.ExpenseTransactionId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired. A notification of your decision on the Transaction request cannot be sent.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                var user = Membership.GetUser(transaction.RegisteredById);

                if (user == null)
                {
                    ConfirmAlertBox1.ShowMessage("The Transaction requester's details could not be retrieved. A notification of your decision on the Transaction request cannot be sent.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                const string subject = "Transaction Approval Notification!";

                var message = "Your Transaction request: " + transaction.ExpenseTitle + " has been attended to.";

                if (!Mailsender(user.Email, subject, message))
                {
                    ConfirmAlertBox1.ShowMessage("The mailing process failed.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                ConfirmAlertBox1.ShowSuccessAlert("A notification of your decision on the Transaction was successfully sent to the requester.");
                
                return true;
            }
            catch (Exception)
            {
                ConfirmAlertBox1.ShowMessage("A notification of your decision on the Transaction request could not be sent.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }
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
                            ConfirmAlertBox1.ShowMessage("A notification could not be sent for your Transaction request due to some technical issues.\n Approval of your request may be delayed.",
                                ConfirmAlertBox.PopupMessageType.Error);
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
                FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_expTransactionListToApprove", Navigation.Sorting, Limit, LoadMethod);
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
                //listNav3.Attributes.Add("class", "disabled");
                //listNav4.Attributes.Add("class", "disabled");
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
                FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_expTransactionListToApprove", Navigation.Next, Limit, LoadMethod);
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
                FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_expTransactionListToApprove", Navigation.First, Limit, LoadMethod);
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
                FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_expTransactionListToApprove", Navigation.Last, Limit, LoadMethod);
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
                FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_expTransactionListToApprove", Navigation.Previous, Limit, LoadMethod);
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
                FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_expTransactionListToApprove", Navigation.None, Limit, LoadMethod);
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
