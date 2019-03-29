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
    public partial class FrmAmendRejectedTransaction : UserControl
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
                BindDefaultData();
                hTitle.InnerText = "Manage Pending/Rejected Transactions";
                LoadFilterOptions();
                LoadTransactions();
                btnSubmit.CommandArgument = "0";
            }
        }

        #region Page Events
        protected void BtnDateFilterClick(object sender, EventArgs e)
        {
            if (int.Parse(ddlAmendFilterOption.SelectedValue) < 0)
            {
                ConfirmAlertBox1.ShowMessage("Please select a FILTER OPTION!", ConfirmAlertBox.PopupMessageType.Error);
                return;

            }
            
            if (!ValidateDateControls())
            {
                return;
            }

            if (!GetTransactionsByDate())
            {
                return;
            }
            SetTransactionsStyle();
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

                if (e.CommandName == "Delete")
                {
                    if (!DeleteUserInitiatedTransaction(transaction))
                    {
                        return;
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

                    ConfirmAlertBox1.ShowSuccessAlert("Transaction information was successfully deleted.");
                }

            }
            catch
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin. Please try again soon or contact your site Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        #endregion

        #region Page Helpers
        private void BindDefaultData()
        {
            lblRequestedAmmount.InnerText = string.Empty;
            lblApprovedTotalAmount.InnerText = string.Empty;
            dvUserEntries.Visible = true;
            dvTransactionItems.Visible = false;
            btnSubmit.CommandArgument = "0";
            Session["_transaction"] = null;
            Session["_flag"] = null;
            Session["_id"] = null;
            dgExpenseItem.DataSource = new List<ExpenseItem>();
            dgExpenseItem.DataBind();
        }
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
                expTransactionList = ServiceProvider.Instance().GetExpenseTransactionServices().GetPendingAndRejectedTransactionsByDate(DateMap.GetLocalDate());
            }
            else
            {
                var userId = new PortalServiceManager().GetUserIdByUsername(HttpContext.Current.User.Identity.Name);
                if (userId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Your user details could not be retrieved.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                expTransactionList = ServiceProvider.Instance().GetExpenseTransactionServices().GetPortalUserRejectedTransactionsByDate(userId, DateMap.GetLocalDate());
            }  

            if (expTransactionList == null || !expTransactionList.Any())
            {
                return;
            }

            Session["_rejectedOrPendingExpTransactionList"] = null;
            Session["_rejectedOrPendingExpTransactionList"] = expTransactionList;
           
            Limit = int.Parse(ddlLimit.SelectedValue);
            FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_rejectedOrPendingExpTransactionList", Navigation.None, Limit, LoadMethod);
            SetTransactionsStyle();
            
        }
        private void LoadFilterOptions()
        {
            try
            {
                var options = DataArray.ConvertEnumToArrayList(typeof(PendingRejectedFilterOptions));
                ddlAmendFilterOption.DataSource = options;
                ddlAmendFilterOption.DataValueField = "ID";
                ddlAmendFilterOption.DataTextField = "Name";
                ddlAmendFilterOption.DataBind();
                ddlAmendFilterOption.Items.Insert(0, new ListItem(" -- Select Status -- ", "-1"));
                ddlAmendFilterOption.Items.Insert(1, new ListItem("All", "0"));
                ddlAmendFilterOption.SelectedIndex = -1;
            }
            catch (Exception)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        private bool GetTransactionsByDate()
        {
            try
            {
                var status = int.Parse(ddlAmendFilterOption.SelectedValue);
                
                dgUserInitiatedTransactions.DataSource = new List<ExpenseTransaction>();
                
                dgUserInitiatedTransactions.DataBind();

                var expTransactionsByDate = new List<ExpenseTransaction>();
                var startDateString = DateMap.ReverseToServerDate(txtStart.Text.Trim());
                var startDate = DateTime.Parse(startDateString);
                var endDateString = DateMap.ReverseToServerDate(txtEndDate.Text.Trim());
                var endDate = DateTime.Parse(endDateString);
                if (endDate < startDate || startDate > endDate)
                {
                    ConfirmAlertBox1.ShowMessage("The 'From' date must not be LATER than the 'To' date.", ConfirmAlertBox.PopupMessageType.Error);
                    return false; 
                }

                if (HttpContext.Current.User.IsInRole("PortalAdmin"))
                {

                    if (status == 0)
                    {
                      expTransactionsByDate = ServiceProvider.Instance().GetExpenseTransactionServices().GetPendingAndRejectedTransactionsByDateRange(startDate, endDate);
                        
                    }

                    if (status == 1)
                    {
                        expTransactionsByDate = ServiceProvider.Instance().GetExpenseTransactionServices().GetPendingTransactionsByDateRange(startDate, endDate);
                    }

                    if (status == 2)
                    {
                        expTransactionsByDate = ServiceProvider.Instance().GetExpenseTransactionServices().GetRejectedTransactionsByDateRange(startDate, endDate);
                    }
                    
                }

                else
                {
                    var userId = new PortalServiceManager().GetUserIdByUsername(HttpContext.Current.User.Identity.Name);
                    if (userId < 1)
                    {
                        ConfirmAlertBox1.ShowMessage("Your user details could not be retrieved.", ConfirmAlertBox.PopupMessageType.Error);
                        return false;
                    }

                    if (status == 0)
                    {
                        expTransactionsByDate = ServiceProvider.Instance().GetExpenseTransactionServices().GetPortalUserPendingAndRejectedTransactionsByDateRange(userId, startDate, endDate);
                    }

                    if (status == 1)
                    {
                        expTransactionsByDate = ServiceProvider.Instance().GetExpenseTransactionServices().GetPortalUserPendingTransactionsByDateRange(userId, startDate, endDate);
                    }

                    if (status == 2)
                    {
                        expTransactionsByDate = ServiceProvider.Instance().GetExpenseTransactionServices().GetPortalUserRejectedTransactionsByDateRange(userId, startDate, endDate);
                    }
                    
                }  

                if (!expTransactionsByDate.Any())
                {
                    ConfirmAlertBox1.ShowMessage("No record found.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }
                Session["_rejectedOrPendingExpTransactionList"] = null;
                Session["_rejectedOrPendingExpTransactionList"] = expTransactionsByDate;
               
                Limit = int.Parse(ddlLimit.SelectedValue);
                FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_rejectedOrPendingExpTransactionList", Navigation.None, Limit, LoadMethod);
                SetTransactionsStyle();
                return true;
            }
            catch (Exception ex)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
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
        private bool DeleteUserInitiatedTransaction(ExpenseTransaction transaction)
        {
            try
            {

                if (transaction == null || transaction.ExpenseTransactionId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid record selection.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                var loggedOnUserId = new PortalServiceManager().GetUserIdByUsername(HttpContext.Current.User.Identity.Name);

                if (loggedOnUserId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Your User details could not be retrieved. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                if (transaction.RegisteredById != loggedOnUserId)
                {
                    ConfirmAlertBox1.ShowMessage("You can only modify Transactions initiated by you.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }


                if (transaction.Status == 1)
                {
                    ConfirmAlertBox1.ShowMessage("An approved Transaction cannot be deleted.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                if (transaction.Status == 2)
                {
                    ConfirmAlertBox1.ShowMessage("A rejected Transaction cannot be deleted. Please make amendments if applicable.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                if (!ServiceProvider.Instance().GetTransactionItemServices().DeleteTransactionAndItems(transaction.ExpenseTransactionId))
                {
                    ConfirmAlertBox1.ShowMessage("Transaction information could not be deleted. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
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
                        var approverCommentLink = ((LinkButton)dgUserInitiatedTransactions.Items[i].FindControl("lnkApproverComment"));
                       
                        if (approvedLabel.Text == "Pending")
                        {
                            approvedLabel.Style.Add("color", "maroon");
                            approvedLabel.Style.Add("font-weight", "bold");
                            approverCommentLink.Enabled = false;
                        }

                        if (approvedLabel.Text == "Rejected")
                        {
                            approvedLabel.Style.Add("color", "red");
                            approvedLabel.Style.Add("font-weight", "bold");
                            approverCommentLink.Enabled = true;
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
        protected void DgTransactionItemDeleteCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                var loggedOnUserId = new PortalServiceManager().GetUserIdByUsername(HttpContext.Current.User.Identity.Name);

                if (loggedOnUserId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Your User details could not be retrieved. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                if (Session["_transaction"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var transaction = Session["_transaction"] as ExpenseTransaction;

                if (transaction == null || transaction.ExpenseTransactionId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                if (transaction.RegisteredById != loggedOnUserId)
                {
                    ConfirmAlertBox1.ShowMessage("You can only delete the items of a Transaction initiated by you.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                if (transaction.Status == 2)
                {
                    ConfirmAlertBox1.ShowMessage("The item(s) of a Rejected Transaction cannot be deleted.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                dgExpenseItem.SelectedIndex = e.Item.ItemIndex;

                var id = (DataCheck.IsNumeric(dgExpenseItem.DataKeys[e.Item.ItemIndex].ToString())) ? int.Parse(dgExpenseItem.DataKeys[e.Item.ItemIndex].ToString()) : 0;

                if (id < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid Record Selection!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                if (dgExpenseItem.Items.Count == 1)
                {
                    lblMessage.InnerHtml =
                            "<br/>You are about to delete the last Item in the Transaction request. \nIf you wish to continue, both the Item and the requested Transaction will be Deleted. \n<b>DO YOU WISH TO CONTINUE?</b>";
                    mpeExpenseItemsPopup.PopupControlID = "dvConfirmation";
                    mpeExpenseItemsPopup.CancelControlID = "btnNO";
                    mpeExpenseItemsPopup.Show();
                    Session["_id"] = id;
                    return;
                }

                if (transaction.Status == 2)
                {
                    Session["_flag"] = 1;
                    transaction.Status = 0;
                }
                
                if (!ServiceProvider.Instance().GetTransactionItemServices().ModifyPendingTransactionAndItem(transaction, id))
                {
                    ConfirmAlertBox1.ShowMessage("Transaction Item could not be deleted. Please try again soon or contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }
                ConfirmAlertBox1.ShowSuccessAlert("Transaction Item was successfully deleted");
                Sendmail();
                LoadTransactionItems(transaction);
            }

            catch
            {
                ConfirmAlertBox1.ShowMessage("The delete process could not be completed. Please try again or contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        protected void DgTransactionItemEditCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                var loggedOnUserId = new PortalServiceManager().GetUserIdByUsername(HttpContext.Current.User.Identity.Name);

                if (loggedOnUserId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Your User details could not be retrieved. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                if (Session["_transaction"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var transaction = Session["_transaction"] as ExpenseTransaction;
               
                if (transaction == null || transaction.ExpenseTransactionId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                if (transaction.RegisteredById != loggedOnUserId)
                {
                    ConfirmAlertBox1.ShowMessage("Sorry, You can only modify the items of a Transaction initiated by you.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                if (transaction.Status == 1 || transaction.Status == 3)
                {
                    ConfirmAlertBox1.ShowMessage("The items of an Approved, or Voided  Transaction cannot be modified.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }
                
                dgExpenseItem.SelectedIndex = e.Item.ItemIndex;

                var id = (DataCheck.IsNumeric(dgExpenseItem.DataKeys[e.Item.ItemIndex].ToString())) ? int.Parse(dgExpenseItem.DataKeys[e.Item.ItemIndex].ToString()) : 0;

                if (id < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid Record Selection!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var transactionItem = ServiceProvider.Instance().GetTransactionItemServices().GetTransactionItem(id);

                if (transactionItem == null || transactionItem.TransactionItemId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid selection.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                if (transaction.Status == 0)
                {
                    btnUpdateTransactionItem.Text = "Update";
                }

                if (transaction.Status == 2)
                {
                    btnUpdateTransactionItem.Text = "Amend";
                }

                ddlExpenseItem.SelectedValue = transactionItem.ExpenseItem.ExpenseItemId.ToString(CultureInfo.InvariantCulture);
                ddlExpenseType.SelectedValue = transactionItem.ExpenseTypeId.ToString(CultureInfo.InvariantCulture);
                txtQuantity.Text = transactionItem.RequestedQuantity.ToString(CultureInfo.InvariantCulture);
                txtUnitPrice.Text = transactionItem.RequestedUnitPrice.ToString(CultureInfo.InvariantCulture);
                btnUpdateTransactionItem.Text = "Update";
                txtItemDescription.Text = transactionItem.Description;
                mpeExpenseItemsPopup.CancelControlID = "btnCancelUpdateTransactionItem";
                mpeExpenseItemsPopup.PopupControlID = "dvModifyTransactionItem";
                mpeExpenseItemsPopup.Show();
                Session["_transactionItem"] = transactionItem;
            }
            catch
            {
                ConfirmAlertBox1.ShowMessage("The Edit process could not be initiated. Please try again.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        protected void BtnDeleletTransactionClick(object sender, EventArgs e)
        {
            if (Session["_transaction"] == null)
            {
                ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                return;
            }

            var transaction = Session["_transaction"] as ExpenseTransaction;

            if (transaction == null || string.IsNullOrEmpty(transaction.ExpenseTitle))
            {
                ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                return;
            }

            if (Session["_id"] == null)
            {
                ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                return;
            }

            var id = (int)Session["_id"];

            if (id < 1)
            {
                ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                return;
            }

            if (!ServiceProvider.Instance().GetTransactionItemServices().DeleteTransactionAndItem(id))
            {
                ConfirmAlertBox1.ShowMessage("Process could not be completed. Please try again or contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
                return;
            }

            if (!string.IsNullOrEmpty(txtStart.Text.Trim()) && !string.IsNullOrEmpty(txtEndDate.Text.Trim()) && int.Parse(ddlAmendFilterOption.SelectedValue) > -1)
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

            BindDefaultData();
            hTitle.InnerText = "Manage Pending/Rejected Transactions";
            ConfirmAlertBox1.ShowSuccessAlert("Process successfully completed.");
        }
        protected void BtnBackNavClick(object sender, EventArgs e)
         {
             if (!string.IsNullOrEmpty(txtStart.Text.Trim()) && !string.IsNullOrEmpty(txtEndDate.Text.Trim()) && int.Parse(ddlAmendFilterOption.SelectedValue) > -1)
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

             BindDefaultData();
         }
        #endregion

        #region Helpers
        private void LoadTransactionItems(ExpenseTransaction transaction)
        {
            try
            {
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
                    ConfirmAlertBox1.ShowMessage("The Transaction Item list could not be retrieved. Please try again or contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
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

                if (string.IsNullOrEmpty(lblRequestedAmmount.InnerText))
                {
                    lblRequestedAmmount.InnerText = "N" + NumberMap.GroupToDigits(transaction.TotalTransactionAmount.ToString(CultureInfo.InvariantCulture));
                }

                if (transaction.TotalApprovedAmount > 0)
                {
                    lblApprovedTotalAmount.InnerText = "N" + NumberMap.GroupToDigits(transaction.TotalApprovedAmount.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    lblApprovedTotalAmount.InnerText = NumberMap.GroupToDigits(0.ToString(CultureInfo.InvariantCulture));
                }

                lblTransactionTitle.Text = transaction.ExpenseTitle;
                dgExpenseItem.DataSource = transactionItems;
                dgExpenseItem.DataBind();
                SetTransactionsStyle();
                LoadTransactionsFooter();
                Session["_transaction"] = transaction;

                if (ddlExpenseType.Items.Count == 1 || ddlExpenseType.Items.Count == 0)
                {
                    LoadTransactionTypes();
                }

                hTitle.InnerText = " Amend Items for Rejected Transaction";
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
                if (Session["_transaction"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

               var transaction = Session["_transaction"] as ExpenseTransaction;

               if (transaction == null || transaction.ExpenseTransactionId < 1)
               {
                   ConfirmAlertBox1.ShowMessage("Session has expired!", ConfirmAlertBox.PopupMessageType.Error);
                   return false;
               }
                
                if (Session["_transactionItem"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                var transactionItemToUpdate = Session["_transactionItem"] as TransactionItem;

                if (transactionItemToUpdate == null || transactionItemToUpdate.TransactionItemId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                var expenseItem = ServiceProvider.Instance().GetExpenseItemServices().GetExpenseItem(int.Parse(ddlExpenseItem.SelectedValue));

                if (expenseItem == null || expenseItem.ExpenseItemId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid selection!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                transactionItemToUpdate.ExpensenseItemId = expenseItem.ExpenseItemId;
                transactionItemToUpdate.ExpenseTypeId = int.Parse(ddlExpenseType.SelectedValue);
                transactionItemToUpdate.ExpenseCategoryId = expenseItem.ExpenseCategoryId;
                transactionItemToUpdate.RequestedUnitPrice = int.Parse(txtUnitPrice.Text.Trim());
                transactionItemToUpdate.RequestedQuantity = int.Parse(txtQuantity.Text.Trim());
                transactionItemToUpdate.ExpenseItem.Title = expenseItem.Title;
                transactionItemToUpdate.Description = txtItemDescription.Text.Trim();
                if (transactionItemToUpdate.ExpenseTransactionId < 1 || transactionItemToUpdate.TransactionItemId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Transaction Item information could not be updated. Please try again.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }
                
                if(transaction.Status == 2)
                {
                    transaction.Status = 0;
                    Session["_flag"] = 1;
                }
                
                if (!SubmitAmendedTransactionItem(transaction, transactionItemToUpdate))
                {
                    ConfirmAlertBox1.ShowMessage("Transaction Item information could not be updated. Please try again.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }
                
                return true;

            }
            catch
            {
                ConfirmAlertBox1.ShowMessage("The Edit process could not be completed. Please try again.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }
        }
        private bool SubmitAmendedTransactionItem(ExpenseTransaction transaction, TransactionItem transactionItem)
        {
            try
            {
                if (transaction == null || transaction.ExpenseTransactionId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }
                
                if (transactionItem == null || transactionItem.TransactionItemId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                if(!ServiceProvider.Instance().GetTransactionItemServices().UpdatePendingTransactionAndItem(transaction, transactionItem))
                {
                    ConfirmAlertBox1.ShowMessage("Transaction could not be updated. Please try again soon or contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                Sendmail();
                LoadTransactionItems(transaction);
                return true;
            }
            catch (Exception)
            {

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

                if (string.IsNullOrEmpty(txtItemDescription.Text.Trim()))
                {
                    ConfirmAlertBox1.ShowMessage("Please supply the Item's Description.", ConfirmAlertBox.PopupMessageType.Error);
                    txtItemDescription.Focus();
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

                return true;
            }
            catch (Exception)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }
        }
        private void Sendmail()
        {
            try
            {
                var argument = int.Parse(btnSubmit.CommandArgument);

                if (argument < 1)
                {
                    if (Session["_flag"] == null)
                    {
                        return;
                    }

                    var flag = (int)Session["_flag"];
                    
                    if(flag < 1)
                    {
                        return;
                    }

                    var user = Membership.GetUser();

                    if (user == null)
                    {
                        ConfirmAlertBox1.ShowMessage(
                            "Your user details could not be retrieved. Your Transaction request notification cannot be sent.",
                            ConfirmAlertBox.PopupMessageType.Error);
                        return;
                    }

                    const string subject = "Transaction request Notification!";

                    var message = "A modified Transaction request from " + user.UserName +
                                    " is waiting for your attention.";

                    var delegateService = new ApprovalDelegateService();

                    var approverEmail = delegateService.GetApproverEmail();

                    if (string.IsNullOrEmpty(approverEmail))
                    {
                        ConfirmAlertBox1.ShowMessage(
                            "The Approver information could not be retrieved. A notification could not be sent for your Transaction request.\n Approval of your request may be delayed.",
                            ConfirmAlertBox.PopupMessageType.Error);
                        return;
                    }

                    if (!Mailsender(approverEmail, subject, message))
                    {
                        ConfirmAlertBox1.ShowMessage(
                            "A notification could not be sent for your Transaction request due to some technical issues.\n Approval of your request may be delayed.",
                            ConfirmAlertBox.PopupMessageType.Error);
                        return;
                    }

                    ConfirmAlertBox1.ShowSuccessAlert("A notification for your Transaction request has been sent.");

                    btnSubmit.CommandArgument = "1";
                    
                }
               
            }
            catch (Exception)
            {
                ConfirmAlertBox1.ShowMessage(
                                   "The Approver information could not be retrieved. A notification could not be sent for your Transaction request.\n Approval of your request may be delayed.",
                                   ConfirmAlertBox.PopupMessageType.Error);
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
                FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_rejectedOrPendingExpTransactionList", Navigation.Sorting, Limit, LoadMethod);
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
                FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_rejectedOrPendingExpTransactionList", Navigation.Next, Limit, LoadMethod);
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
                FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_rejectedOrPendingExpTransactionList", Navigation.First, Limit, LoadMethod);
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
                FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_rejectedOrPendingExpTransactionList", Navigation.Last, Limit, LoadMethod);
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
                FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_rejectedOrPendingExpTransactionList", Navigation.Previous, Limit, LoadMethod);
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
                FillRepeater<ExpenseTransaction>(dgUserInitiatedTransactions, "_rejectedOrPendingExpTransactionList", Navigation.None, Limit, LoadMethod);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }


        }
        private void GetPageLimits()
        {
            var intVal = new List<int>();
            for (var i = 20; i <= 200; i+=30)
            {
                if(i == 20)
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
           if(!string.IsNullOrEmpty(txtStart.Text.Trim()) && !string.IsNullOrEmpty(txtEndDate.Text.Trim()))
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