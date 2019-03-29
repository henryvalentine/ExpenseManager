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
using xPlug.BusinessService;
using xPlug.BusinessService.CustomizedASPBusinessService;

namespace ExpenseManager.ExpenseMgt
{
    public partial class FrmManageTransactions : UserControl
    {
        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["_transaction"] = null;
                Session["_transactionItems"] = null;
                Session["_transactionItem"] = null;
                Session["_expenseItems"] = null;
                divEditTransaction.Visible = false;
                btnCancel.CommandArgument = "0";
                dvContents.Visible = false;
                BindControlsWithDefaultLists();
                dvFirstStage.Visible = true;
                //LoadTransactionTypes();
            }

        }
        protected void BtnSubmitTransactionsClick(object sender, EventArgs e)
        {
            try
            {
                if (!AddExpenseTransaction())
                {
                    return;
                }

                Session["_transactionItems"] = null;
                Session["_transaction"] = null;
                Session["_expenseItems"] = null;
                btnCancel.Style.Add("margin-left", "118%");
                lgTitle.InnerHtml = "Create New Transaction";
                lnkAddTransactionItem.Visible = false;
                txtDescription.Enabled = true;
                txtTransactionTitle.Enabled = true;
                divCreateTransaction.Visible = true;
                divEditTransaction.Visible = false;
                btnCancel.Visible = true;
                btnAddItem.Visible = false;
                ClearTransactionControls();
                if (!SendNotification())
                {
                    ConfirmAlertBox1.ShowMessage("Your Transaction request was submitted successfully pending approval, but a notification could not be sent to the Approve.\n Approval of your Transaction request may be delayed.", ConfirmAlertBox.PopupMessageType.Information);
                }
                else
                {
                    ConfirmAlertBox1.ShowSuccessAlert("Your Transaction request was submitted successfully pending approval");
                }
                
            }

            catch (Exception ex)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }
        }
        protected void grdViewTransMain_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Session["_transactionItems"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var transactionItems = Session["_transactionItems"] as List<TransactionItem>;

                if (transactionItems == null || !transactionItems.Any())
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                int expenseItemId = int.Parse(grdViewTransMain.DataKeys[e.Row.RowIndex].Value.ToString());
                if (expenseItemId < 0)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }
                var grdViewTranItem = (GridView)e.Row.FindControl("grdViewTransItems");
                grdViewTranItem.DataSource = transactionItems.FindAll(m => m.ExpensenseItemId == expenseItemId).OrderBy(m => m.ExpenseItem.Title).ToList();
                grdViewTranItem.DataBind();
            }
        }
        protected void grdViewTransItems_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                if (Session["_transactionItems"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired", ConfirmAlertBox.PopupMessageType.Error);
                    var url = Context.Request.Url;
                    Response.Redirect("~/Login.aspx", true);
                    return;
                }
                if (Session["_expenseItems"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var transactionItems = Session["_transactionItems"] as List<TransactionItem>;

                var expenseItems = Session["_expenseItems"] as List<ExpenseItem>;

                if (transactionItems == null || !transactionItems.Any())
                {
                    ConfirmAlertBox1.ShowMessage("Expense Transaction list is empty or session has expired", ConfirmAlertBox.PopupMessageType.Error);
                    Response.Redirect("~/Login.aspx", true);
                    return;
                }

                if (index < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid Record Selection!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var transactionItem = transactionItems.Find(m => m.TempId == index);

                if (transactionItem == null || transactionItem.TempId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid record selection.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                if (expenseItems == null || !expenseItems.Any())
                {
                    ConfirmAlertBox1.ShowMessage("Expense Item list is empty or session has expired", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                if (Session["_transaction"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired!", ConfirmAlertBox.PopupMessageType.Error);
                    Response.Redirect("~/Login.aspx", true);
                    return;
                }

                var transaction = Session["_transaction"] as ExpenseTransaction;

                if (transaction == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired!", ConfirmAlertBox.PopupMessageType.Error);
                    Response.Redirect("~/Login.aspx", true);
                    return;
                }

                ccdAccountHead.SelectedValue = transactionItem.ExpenseItem.AccountsHeadId.ToString(CultureInfo.InvariantCulture);
                ccdExpenseItem.SelectedValue = transactionItem.ExpensenseItemId.ToString(CultureInfo.InvariantCulture);
                ddlExpenseType.SelectedValue = transactionItem.ExpenseTypeId.ToString(CultureInfo.InvariantCulture);
                txtQuantity.Text = transactionItem.RequestedQuantity.ToString(CultureInfo.InvariantCulture);
                txtUnitPrice.Text = transactionItem.RequestedUnitPrice.ToString(CultureInfo.InvariantCulture);
                txtItemDescription.Text = transactionItem.Description;
                btnProcessExpenseTransactions.Text = "Update";
                btnProcessExpenseTransactions.CommandArgument = "2";
                lgTitle.InnerText = "Update Transaction Item";
                mpeExpenseItemsPopup.CancelControlID = "btnClose";
                mpeExpenseItemsPopup.PopupControlID = "dvCreateTransaction";
                mpeExpenseItemsPopup.Show();
                Session["_transactionItem"] = transactionItem;

                grdViewTransMain.DataSource = expenseItems;
                grdViewTransMain.DataBind();
                LoadTransactionsFooter();
                ConfirmAlertBox1.ShowSuccessAlert("Transaction information was successfully Updated.");
            }
            if (e.CommandName == "DeleteRow")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                if (Session["_transactionItems"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                if (Session["_expenseItems"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var transactionItems = Session["_transactionItems"] as List<TransactionItem>;

                var expenseItems = Session["_expenseItems"] as List<ExpenseItem>;

                if (transactionItems == null || !transactionItems.Any())
                {
                    ConfirmAlertBox1.ShowMessage("Expense Transaction list is empty or session has expired", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                if (expenseItems == null || !expenseItems.Any())
                {
                    ConfirmAlertBox1.ShowMessage("Expense Item list is empty or session has expired", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                if (index < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid Record Selection!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var transactionItem = transactionItems.Find(m => m.TempId == index);

                if (transactionItem == null || transactionItem.TempId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid record selection.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                transactionItems.Remove(transactionItem);

                if (transactionItems.Count(m => m.ExpensenseItemId == transactionItem.ExpensenseItemId) < 1)
                {
                    expenseItems.Remove(expenseItems.Find(ex => ex.ExpenseItemId == transactionItem.ExpensenseItemId));
                    Session["_expenseItems"] = null;
                    Session["_expenseItems"] = expenseItems;
                }

                grdViewTransMain.DataSource = expenseItems;
                grdViewTransMain.DataBind();
                Session["_transactionItems"] = null;
                Session["_transactionItems"] = transactionItems;
                LoadTransactionsFooter();
                ConfirmAlertBox1.ShowSuccessAlert("Transaction information was successfully deleted from the pending Transaction Item list.");
            }
        }

        //protected void DgExpenseTransactionDeleteCommand(object source, DataGridCommandEventArgs e)
        //{
        //    try
        //    {
        //        if (Session["_transactionItems"] == null)
        //        {
        //            ConfirmAlertBox1.ShowMessage("Session has expired", ConfirmAlertBox.PopupMessageType.Error);
        //            return;
        //        }

        //        var transactionItems = Session["_transactionItems"] as List<TransactionItem>;

        //        if (transactionItems == null || !transactionItems.Any())
        //        {
        //            ConfirmAlertBox1.ShowMessage("Expense Transaction list is empty or session has expired", ConfirmAlertBox.PopupMessageType.Error);
        //            return;
        //        }

        //        dgExpenseItem.SelectedIndex = e.Item.ItemIndex;

        //        long id = (DataCheck.IsNumeric(dgExpenseItem.DataKeys[e.Item.ItemIndex].ToString())) ? int.Parse(dgExpenseItem.DataKeys[e.Item.ItemIndex].ToString()) : 0;

        //        if (id < 1)
        //        {
        //            ConfirmAlertBox1.ShowMessage("Invalid Record Selection!", ConfirmAlertBox.PopupMessageType.Error);
        //            return;
        //        }

        //        var transactionItem = transactionItems.Find(m => m.TempId == id);

        //        if (transactionItem == null || transactionItem.TempId < 1)
        //        {
        //            ConfirmAlertBox1.ShowMessage("Invalid record selection.", ConfirmAlertBox.PopupMessageType.Error);
        //            return;
        //        }

        //        transactionItems.Remove(transactionItem);
        //        dgExpenseItem.DataSource = transactionItems.OrderBy(m => m.ExpenseItem.Title).ToList();
        //        dgExpenseItem.DataBind();
        //        Session["_transactionItems"] = null;
        //        Session["_transactionItems"] = transactionItems;
        //        LoadTransactionsFooter();
        //        ConfirmAlertBox1.ShowSuccessAlert("Transaction information was successfully deleted from the pending Transaction Item list.");
        //    }

        //    catch
        //    {
        //        ConfirmAlertBox1.ShowMessage("The delete process could not be completed.", ConfirmAlertBox.PopupMessageType.Error);
        //    }
        //}
        //protected void DgExpenseTransactionEditCommand(object source, DataGridCommandEventArgs e)
        //{
        //    try
        //    {
        //        if (Session["_transactionItems"] == null)
        //        {
        //            ConfirmAlertBox1.ShowMessage("Session has expired", ConfirmAlertBox.PopupMessageType.Error);
        //            Response.Redirect("~/Login.aspx", true);
        //            return;
        //        }

        //        var transactionItems = Session["_transactionItems"] as List<TransactionItem>;

        //        if (transactionItems == null || !transactionItems.Any())
        //        {
        //            ConfirmAlertBox1.ShowMessage("Expense Transaction list is empty or session has expired", ConfirmAlertBox.PopupMessageType.Error);
        //            Response.Redirect("~/Login.aspx", true);
        //            return;
        //        }

        //        dgExpenseItem.SelectedIndex = e.Item.ItemIndex;

        //        long id = (DataCheck.IsNumeric(dgExpenseItem.DataKeys[e.Item.ItemIndex].ToString())) ? int.Parse(dgExpenseItem.DataKeys[e.Item.ItemIndex].ToString()) : 0;

        //        if (id < 1)
        //        {
        //            ConfirmAlertBox1.ShowMessage("Invalid Record Selection!", ConfirmAlertBox.PopupMessageType.Error);
        //            return;
        //        }

        //        var transactionItem = transactionItems.Find(m => m.TempId == id);

        //        if (transactionItem == null || transactionItem.TempId < 1)
        //        {
        //            ConfirmAlertBox1.ShowMessage("Invalid record selection.", ConfirmAlertBox.PopupMessageType.Error);
        //            return;
        //        }

        //        if (Session["_transaction"] == null)
        //        {
        //            ConfirmAlertBox1.ShowMessage("Session has expired!", ConfirmAlertBox.PopupMessageType.Error);
        //            Response.Redirect("~/Login.aspx", true);
        //            return;
        //        }
                
        //        var transaction = Session["_transaction"] as ExpenseTransaction;

        //        if (transaction == null)
        //        {
        //            ConfirmAlertBox1.ShowMessage("Session has expired!", ConfirmAlertBox.PopupMessageType.Error);
        //            Response.Redirect("~/Login.aspx", true);
        //            return;
        //        }

        //        ccdAccountHead.SelectedValue = transactionItem.ExpenseItem.AccountsHeadId.ToString(CultureInfo.InvariantCulture);
        //        ccdExpenseItem.SelectedValue = transactionItem.ExpensenseItemId.ToString(CultureInfo.InvariantCulture);
        //        ddlExpenseType.SelectedValue = transactionItem.ExpenseTypeId.ToString(CultureInfo.InvariantCulture);
        //        txtQuantity.Text = transactionItem.RequestedQuantity.ToString(CultureInfo.InvariantCulture);
        //        txtUnitPrice.Text = transactionItem.RequestedUnitPrice.ToString(CultureInfo.InvariantCulture);
        //        txtItemDescription.Text = transactionItem.Description;
        //        btnProcessExpenseTransactions.Text = "Update";
        //        btnProcessExpenseTransactions.CommandArgument = "2";
        //        lgTitle.InnerText = "Update Transaction Item";
        //        mpeExpenseItemsPopup.CancelControlID = "btnClose";
        //        mpeExpenseItemsPopup.PopupControlID = "dvCreateTransaction";
        //        mpeExpenseItemsPopup.Show();
        //        Session["_transactionItem"] = transactionItem;
        //    }
        //    catch
        //    {
        //        ConfirmAlertBox1.ShowMessage("The Edit process could not be initiated. Please try again.", ConfirmAlertBox.PopupMessageType.Error);
        //    }
        //}
        protected void DdlBeneficiariesSelectedIndexChanged(object sender, EventArgs e)
        {
            var userId = int.Parse(new PortalServiceManager().GetUserIdByUsername(HttpContext.Current.User.Identity.Name).ToString(CultureInfo.InvariantCulture));
            if (userId < 1)
            {
                ConfirmAlertBox1.ShowMessage("You do not have the privilege to perform this function. Please contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
                return;
            }
            var user = ServiceProvider.Instance().GetStaffUserServices().GetStaffUserByMembershipId(userId);
            if (user.StaffUserId < 1)
            {
                ConfirmAlertBox1.ShowMessage("You do not have the privilege to perform this function. Please contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
                return;
            }

            btnCancel.Text = "Cancel";
            lblAffirm.InnerText = "0";
            try
            {
                try
                {

                    if (int.Parse(ddlBeneficiaries.SelectedValue) < 1)
                    {
                        ConfirmAlertBox1.ShowMessage("Please select a Beneficiary", ConfirmAlertBox.PopupMessageType.Error);
                        return;
                    }
                }
                catch
                {
                    ConfirmAlertBox1.ShowMessage("Invalid selection!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                if (!GetBeneficiaryInfo(int.Parse(ddlBeneficiaries.SelectedValue)))
                {
                    ConfirmAlertBox1.ShowMessage("The Beneficiary information could not be retrieved. Please try again later or contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                btnCancel.Style.Add("margin-left", "118%");
                ClearControls();
                dvFirstStage.Visible = false;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        protected void BtnSelectNewBeneficiaryClick(object sender, EventArgs e)
        {
            dvContents.Visible = false;
            ClearControls();
            ddlBeneficiaryType.SelectedIndex = 0;
            ccdBeneficiaryType.SelectedValue = "0";
            ddlBeneficiaries.SelectedIndex = 0;
            ccdBeneficiaries.SelectedValue = "0";
            dvFirstStage.Visible = true;
        }
        protected void BtnProcessExpenseTransactionsClick(object sender, EventArgs e)
        {
            try
            {

                if (!ValidateTransactionItemsControls())
                {
                    return;
                }


                switch (int.Parse(btnProcessExpenseTransactions.CommandArgument))
                {
                    case 1:
                        if (!AddTransactionItem())
                        {
                            return;
                        }

                        ConfirmAlertBox1.ShowSuccessAlert("Transaction Item was added successfully.");

                        break;

                    case 2:
                            if (!UpdateTransactionItem())
                            {
                                return;
                            }

                            ConfirmAlertBox1.ShowSuccessAlert("Transaction Item was updated successfully.");
                           
                        break;

                    default:
                        ConfirmAlertBox1.ShowMessage("Invalid Process call!", ConfirmAlertBox.PopupMessageType.Error);
                        break;
                }

            }
            catch
            {
                ConfirmAlertBox1.ShowMessage("The process could not be completed.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        protected void BtnCancelTransactionsClick(object sender, EventArgs e)
        {
            Session["_transaction"] = null;
            Session["_transactionItems"] = null;
            dvContents.Visible = false;
            ClearControls();
            ddlBeneficiaryType.SelectedIndex = 0;
            ccdBeneficiaryType.SelectedValue = "0";
            ddlBeneficiaries.SelectedIndex = 0;
            ccdBeneficiaries.SelectedValue = "0";
            dvFirstStage.Visible = true;
        }
        protected void BtnCreateNewTransactionClick(object sender, EventArgs e)
        {
           if(!ValidateTransactionControls())
           {
               return;
           }
           
           var transaction = new ExpenseTransaction();

           switch (int.Parse(btnCreateNewTransaction.CommandArgument))
            {
                case 1:
                    //Add New
                    transaction = new ExpenseTransaction
                                     {
                                        Status = 0,
                                        BeneficiaryTypeId = int.Parse(ddlBeneficiaryType.SelectedValue),
                                        BeneficiaryId = int.Parse(ddlBeneficiaries.SelectedValue),
                                        Description = txtDescription.Text.Trim(),
                                        ExpenseTitle = txtTransactionTitle.Text.Trim(),
                                     };
                    
                    ConfirmAlertBox1.ShowSuccessAlert("New Transaction successfully created. Use the <b>Add Transaction Item</b> link to add Items to the Transaction.");
                    break;

                case 2:
                    //Update
                    if(Session["_transaction"] == null)
                    {
                        ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                        mpeExpenseItemsPopup.Hide();
                        return;
                    }

                     transaction = Session["_transaction"] as ExpenseTransaction;

                    if (transaction == null || string.IsNullOrEmpty(transaction.ExpenseTitle))
                    {
                        ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                        mpeExpenseItemsPopup.Hide();
                        return;
                    }
                   
                    if (transaction.ExpenseTitle == txtTransactionTitle.Text.Trim() && transaction.Description == txtDescription.Text.Trim())
                    {
                        ConfirmAlertBox1.ShowSuccessAlert("Transaction details was not modified.");
                    }
                     else
                     {
                         transaction.Description = txtDescription.Text.Trim();
                         transaction.ExpenseTitle = txtTransactionTitle.Text.Trim();
                         ConfirmAlertBox1.ShowSuccessAlert("Transaction details was successfully modified.");
                     }
                    break;

                default:
                    ConfirmAlertBox1.ShowMessage("Process could not be completed.", ConfirmAlertBox.PopupMessageType.Error);
                    break;

            }
            Session["_transaction"] = null;
            Session["_transaction"] = transaction;
            lnkAddTransactionItem.Text = "Add Transaction Item";
            lnkAddTransactionItem.Visible = true;
            txtDescription.Enabled = false;
            txtTransactionTitle.Enabled = false;
            divCreateTransaction.Visible = false;
            divEditTransaction.Visible = true;

        }
        protected void LnkEditTransactionDetailsClick(object sender, EventArgs e)
        {
            btnCreateNewTransaction.CommandArgument = "2";
            lgTitle.InnerHtml = "Modify Transaction Details";
            lnkAddTransactionItem.Visible = false;
            txtDescription.Enabled = true;
            txtTransactionTitle.Enabled = true;
            divCreateTransaction.Visible = true;
            divEditTransaction.Visible = false;
        }
        protected void LnkAddTransactionItemClick(object sender, EventArgs e)
        {
            ClearTransactionItemsControls();
            btnProcessExpenseTransactions.CommandArgument = "1";
            mpeExpenseItemsPopup.PopupControlID = "dvCreateTransaction";
            btnProcessExpenseTransactions.Text = "Add Item";
            mpeExpenseItemsPopup.CancelControlID = "btnClose";
            lgTitle.InnerText = "Add new Transaction Item";
            mpeExpenseItemsPopup.Show();
        }
        #endregion

        #region Page Helpers
        private void LoadTransactionTypes()
        {
            try
            {
                var transactionTypesList = ServiceProvider.Instance().GetExpenseTypeServices().GetActiveExpenseTypes();

                if (!transactionTypesList.Any())
                {
                    ConfirmAlertBox1.ShowMessage("Expense Type List is empty!", ConfirmAlertBox.PopupMessageType.Error);
                    ddlExpenseType.DataSource = new List<ExpenseType>();
                    ddlExpenseType.DataBind();
                    ddlExpenseType.Items.Insert(0, new ListItem("-- List is empty --", "0"));
                    ddlExpenseType.SelectedIndex = 0;
                    return;
                }

                ddlExpenseType.DataSource = transactionTypesList;
                ddlExpenseType.DataTextField = "Name";
                ddlExpenseType.DataValueField = "ExpenseTypeId";
                ddlExpenseType.DataBind();
                ddlExpenseType.Items.Insert(0, new ListItem("--Select Expense Type--", "0"));
                ddlExpenseType.SelectedIndex = 0;
                Session["_expenseTypesList"] = transactionTypesList;
                
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                
            }
        }
        private void BindControlsWithDefaultLists()
        {
            grdViewTransMain.DataSource = new List<ExpenseItem>();
            grdViewTransMain.DataBind();
        }
        private bool AddExpenseTransaction()
        {
            try
            {
                if (Session["_transaction"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                var transaction = Session["_transaction"] as ExpenseTransaction;

                if (transaction == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                if (Session["_transactionItems"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                var transactionItems = Session["_transactionItems"] as List<TransactionItem>;

                if (transactionItems == null || !transactionItems.Any())
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                //transaction.TotalTransactionAmount = transactionItems.Sum(m => m.RequestedQuantity * m.RequestedUnitPrice);
                transaction.TransactionDate = DateMap.GetLocalDate();
                transaction.TransactionTime = DateMap.GetLocalTime();
                transaction.ApproverId = 0;

                transaction.RegisteredById = int.Parse(new PortalServiceManager().GetUserIdByUsername(HttpContext.Current.User.Identity.Name).ToString(CultureInfo.InvariantCulture));

                var k = ServiceProvider.Instance().GetTransactionItemServices().AddTransactionItems(transactionItems, transaction);

                if (k < 1)
                {
                    if (k == -3)
                    {
                        ConfirmAlertBox1.ShowMessage("A similar pending Transaction has already been requested today. \nPlease go to the <b>Pending/Rejected Transactions</b> page to edit as required.", ConfirmAlertBox.PopupMessageType.Error);
                        return false;
                    }

                    ConfirmAlertBox1.ShowMessage("Transaction information could not be added.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                Session["_transactionItems"] = null;
                Session["_transaction"] = null;
                grdViewTransMain.DataSource = new List<ExpenseItem>();
                grdViewTransMain.DataBind();
                return true;
            }

            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }
        }
        private bool AddToExpenseItem(ExpenseItem expenseItem, out List<ExpenseItem> expenseItems)
        {
            var expItems = new List<ExpenseItem>();
            try
            {
                if ((expenseItem == null) || (expenseItem.ExpenseItemId < 1))
                {
                    expenseItems = null;
                    return false;
                }
                if (Session["_expenseItems"] == null)
                {
                    expItems.Add(expenseItem);
                }
                else if (Session["_expenseItems"] != null)
                {
                    expItems = Session["_expenseItems"] as List<ExpenseItem>;
                    if (expItems == null)
                    {
                        ConfirmAlertBox1.ShowMessage("Session has expired!", ConfirmAlertBox.PopupMessageType.Error);
                        expenseItems = null;
                        return false;
                    }
                    if (!expItems.Any())
                    {
                        ConfirmAlertBox1.ShowMessage("Session has expired!", ConfirmAlertBox.PopupMessageType.Error);
                        expenseItems = null;
                        return false;
                    }
                    if (!expItems.Exists(p => p.ExpenseItemId == expenseItem.ExpenseItemId))
                    {
                        expItems.Add(expenseItem);
                    }
                }
            }
            catch (Exception ex)
            {
                ConfirmAlertBox1.ShowMessage("Transaction Item could not be added. The process failed.", ConfirmAlertBox.PopupMessageType.Error);
                expenseItems = null;
                return false;
            }
            Session["_expenseItems"] = expItems;
            expenseItems = expItems;
            return true;
        }
        private bool AddTransactionItem()
        {
            try
            {
                var expenseItemId = int.Parse(ddlExpenseItem.SelectedValue);
                var expenseTypeId = int.Parse(ddlExpenseType.SelectedValue);
                var expenseItem = ServiceProvider.Instance().GetExpenseItemServices().GetExpenseItem(expenseItemId);
                if (expenseItem == null || expenseItem.ExpenseItemId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Error retrieving Expense Item information. Please try again or contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }
                List<ExpenseItem> expenseItemList;
                if (!AddToExpenseItem(expenseItem, out expenseItemList))
                {
                    return false;
                }

                List<TransactionItem> transactionItems;
                int tempId;
                if (Session["_transactionItems"] == null)
                {
                    transactionItems = new List<TransactionItem>();
                    tempId = 1;
                }
                else
                {
                    transactionItems = Session["_transactionItems"] as List<TransactionItem>;

                    if (transactionItems == null || !transactionItems.Any())
                    {
                        transactionItems = new List<TransactionItem>();
                        tempId = 1;
                    }
                    else
                    {
                        var orderByIds = transactionItems.OrderByDescending(m => m.TempId);
                        var checkDuplicateItem = transactionItems.Find(m => m.Description.ToLower().Replace(" ", string.Empty) == txtDescription.Text.Trim().ToLower().Replace(" ", string.Empty));
                        if (checkDuplicateItem == null || checkDuplicateItem.ExpensenseItemId == 0)
                        {
                            tempId = orderByIds.ElementAt(0).TempId + 1;
                        }
                        else
                        {
                            ErrorDisplayManageTransaction.ShowError("Please state a <b>Different Purpose</b> for selecting this Expense Item again.");
                            ddlExpenseItem.Focus();
                            mpeExpenseItemsPopup.Show();
                            return false;
                        }
                    }
                }

                var newTransactionItem = new TransactionItem
                {
                    ExpenseItem = expenseItem,
                    TempId = tempId,
                    Status = 0,
                    AccountsHeadTitle = ddlAccountHead.SelectedItem.Text,
                    ExpensenseItemId = expenseItemId,
                    ExpenseTypeId = expenseTypeId,
                    Description = txtItemDescription.Text.Trim(),
                    ExpenseCategoryId = expenseItem.ExpenseCategoryId,
                    RequestedUnitPrice = double.Parse(txtUnitPrice.Text.Trim()),
                    RequestedQuantity = int.Parse(txtQuantity.Text.Trim()),
                };

                transactionItems.Add(newTransactionItem);

                foreach (var item in transactionItems)
                {
                    item.TotalPrice = item.RequestedQuantity * item.RequestedUnitPrice;
                }

                Session["_transactionItems"] = transactionItems;

                grdViewTransMain.DataSource = expenseItemList;
                grdViewTransMain.DataBind();

                //dgExpenseItem.DataSource = transactionItems.OrderBy(m => m.ExpenseItem.Title);
                //dgExpenseItem.DataBind();

                btnAddItem.Visible = true;
                btnCancel.Style.Add("margin-left", "-30%");
                LoadTransactionsFooter();
                return true;

            }
            catch
            {
                ConfirmAlertBox1.ShowMessage("Transaction Item could not be added. The process failed.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }
        }
        private bool UpdateTransactionItem()
        {
            try
            {
                if (Session["_transactionItems"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                var transactionItems = Session["_transactionItems"] as List<TransactionItem>;

                if (transactionItems == null || !transactionItems.Any())
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                if (Session["_transactionItem"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                var transactionItem = Session["_transactionItem"] as TransactionItem;

                if (transactionItem == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                var transactionItemToEdit = transactionItems.Find(m => m.ExpensenseItemId == transactionItem.ExpensenseItemId);

                if (transactionItemToEdit == null || transactionItemToEdit.ExpensenseItemId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid selection!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                var expenseItem = ServiceProvider.Instance().GetExpenseItemServices().GetExpenseItem(int.Parse(ddlExpenseItem.SelectedValue));

                if (expenseItem == null || expenseItem.ExpenseItemId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid selection!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }
                List<ExpenseItem> expenseItemList;
                if (!AddToExpenseItem(expenseItem, out expenseItemList))
                {
                    return false;
                }
                transactionItemToEdit.ExpensenseItemId = expenseItem.ExpenseItemId;
                transactionItem.Description = txtItemDescription.Text.Trim();
                transactionItemToEdit.ExpenseTypeId = int.Parse(ddlExpenseType.SelectedValue);
                transactionItemToEdit.ExpenseCategoryId = expenseItem.ExpenseCategoryId;
                transactionItemToEdit.RequestedUnitPrice = int.Parse(txtUnitPrice.Text.Trim());
                transactionItemToEdit.RequestedQuantity = int.Parse(txtQuantity.Text.Trim());
                transactionItemToEdit.ExpenseItem.Title = expenseItem.Title;
                transactionItemToEdit.TotalPrice = transactionItemToEdit.RequestedQuantity * transactionItemToEdit.RequestedUnitPrice;
                Session["_transactionItems"] = null;
                Session["_transactionItems"] = transactionItems;
                grdViewTransMain.DataSource = expenseItemList; //transactionItems.OrderBy(m => m.ExpenseItem.Title).ToList();
                grdViewTransMain.DataBind();
                LoadTransactionsFooter();
                return true;
            }
            catch
            {
                ConfirmAlertBox1.ShowMessage("The Edit process could not be completed. Please try again.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }
        }
        private void LoadTransactionsFooter()
        {
            //try
            //{
            //    if (dgExpenseItem.Items.Count > 0)
            //    {
            //        double subTotalQuantity = 0;
            //        double subTotalUnitPrice = 0;
            //        double subTotalPrice = 0;

            //        for (var i = 0; i < dgExpenseItem.Items.Count; i++)
            //        {
            //            subTotalQuantity += DataCheck.IsNumeric(((Label)dgExpenseItem.Items[i].FindControl("lblQuantity")).Text)
            //                          ? int.Parse(((Label)dgExpenseItem.Items[i].FindControl("lblQuantity")).Text) : 0;
            //            subTotalUnitPrice += DataCheck.IsNumeric(((Label)dgExpenseItem.Items[i].FindControl("lblUnitPrice")).Text)
            //                          ? double.Parse(((Label)dgExpenseItem.Items[i].FindControl("lblUnitPrice")).Text) : 0;
            //            subTotalPrice += DataCheck.IsNumeric(((Label)dgExpenseItem.Items[i].FindControl("lblSubTotalPrice")).Text)
            //                          ? double.Parse(((Label)dgExpenseItem.Items[i].FindControl("lblSubTotalPrice")).Text) : 0;
            //        }
                    
            //        foreach (var item in dgExpenseItem.Controls[0].Controls)
            //        {
            //            if (item.GetType() == typeof(DataGridItem))
            //            {
            //                var itmType = ((DataGridItem)item).ItemType;
            //                if (itmType == ListItemType.Footer)
            //                {
            //                    if (((DataGridItem)item).FindControl("lblTotalQuantity") != null)
            //                    {
            //                        ((Label)((DataGridItem)item).FindControl("lblTotalQuantity")).Text =  subTotalQuantity.ToString(CultureInfo.InvariantCulture);
            //                    }

            //                    if (((DataGridItem)item).FindControl("lblTotalUnitPrice") != null)
            //                    {
            //                        ((Label)((DataGridItem)item).FindControl("lblTotalUnitPrice")).Text = "N" + NumberMap.GroupToDigits(subTotalUnitPrice.ToString(CultureInfo.InvariantCulture));
            //                    }

            //                    if (((DataGridItem)item).FindControl("lblTotalPrice") != null)
            //                    {
            //                        ((Label)((DataGridItem)item).FindControl("lblTotalPrice")).Text = "N" + NumberMap.GroupToDigits(subTotalPrice.ToString(CultureInfo.InvariantCulture));
            //                    }

            //                }
            //            }
            //        }
            //    }
            //}
            //catch
            //{
            //    ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            //}
        }
        private bool ValidateTransactionItemsControls()
        {
            try
            {
                if (int.Parse(ddlExpenseItem.SelectedValue) < 1)
                {
                    ErrorDisplayManageTransaction.ShowError("Please select Expense Item.");
                    ddlExpenseItem.Focus();
                    return false;
                }

                if (int.Parse(ddlExpenseType.SelectedValue) < 1)
                {
                    ErrorDisplayManageTransaction.ShowError("Please select Expense an Expense Type.");
                    ddlExpenseType.Focus();
                    return false;
                }


                if (string.IsNullOrEmpty(txtQuantity.Text.Trim()))
                {
                    ErrorDisplayManageTransaction.ShowError("Please supply the Item quantity.");
                    txtQuantity.Focus();
                    return false;
                }


                if (string.IsNullOrEmpty(txtItemDescription.Text.Trim()))
                {
                    ErrorDisplayManageTransaction.ShowError("Please supply a Description for the Transaction Item.");
                    txtItemDescription.Focus();
                    return false;
                }
               
                if (string.IsNullOrEmpty(txtUnitPrice.Text.Trim()))
                {
                    ErrorDisplayManageTransaction.ShowError("Please supply the Price per item.");
                    txtUnitPrice.Focus();
                    return false;
                }

                if (!DataCheck.IsNumeric(txtUnitPrice.Text.Trim()))
                {
                    ErrorDisplayManageTransaction.ShowError("Invalid entry!");
                    txtUnitPrice.Focus();
                    return false;
                }

                if (!DataCheck.IsNumeric(txtQuantity.Text.Trim()))
                {
                    ErrorDisplayManageTransaction.ShowError("Invalid entry!");
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
        private bool ValidateTransactionControls()
        {
            try
            {
                if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
                {
                    ConfirmAlertBox1.ShowMessage("Please supply a description for the Transaction.", ConfirmAlertBox.PopupMessageType.Error);
                    txtDescription.Focus();
                    return false;
                }

                if (string.IsNullOrEmpty(txtTransactionTitle.Text.Trim()))
                {
                    ConfirmAlertBox1.ShowMessage("Please supply a Title for the Transaction.", ConfirmAlertBox.PopupMessageType.Error);
                    txtTransactionTitle.Focus();
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
        private void ClearTransactionControls()
        {
            txtTransactionTitle.Text = string.Empty;
            txtDescription.Text = string.Empty;
            lgTitle.InnerHtml = "Create New Transaction";
            ddlAccountHead.SelectedIndex = 0;
        }
        private void ClearTransactionItemsControls()
        {
            txtUnitPrice.Text = string.Empty;
            txtQuantity.Text = string.Empty;
            lgTitle.InnerHtml = "Add New Transaction Item";
            LoadTransactionTypes();
            txtItemDescription.Text = string.Empty;
            ddlExpenseItem.SelectedIndex = 0;
            ccdExpenseItem.SelectedValue = "0";
            ddlExpenseType.SelectedIndex = 0;
            ccdAccountHead.SelectedValue = "0";
        }
        private void ClearControls()
        {
            grdViewTransMain.DataSource = new List<ExpenseItem>();
            grdViewTransMain.DataBind();
            txtDescription.Text = string.Empty;
            txtDescription.Enabled = true;
            lnkAddTransactionItem.Visible = false;
            txtTransactionTitle.Text = string.Empty;
            txtTransactionTitle.Enabled = true;
            lnkAddTransactionItem.Visible = false;
            divCreateTransaction.Visible = true;
            divEditTransaction.Visible = false;
            Session["_transaction"] = null;
            Session["_transactionItems"] = null;
            Session["_transactionItem"] = null;
            Session["_expenseItems"] = null;
        }
        private bool GetBeneficiaryInfo(int beneficiaryId)
        {
            try
            {
                if (beneficiaryId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid selection", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }
                var clientBeneficiary = ServiceProvider.Instance().GetBeneficiaryServices().GetBeneficiary(beneficiaryId);

                if (clientBeneficiary == null || clientBeneficiary.BeneficiaryId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Beneficiary Information could not be retrieved.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }
                dvContents.Visible = true;
                dvBeneficiaryInfo.Visible = true;
                btnAddItem.Visible = false;
                lblCompany.InnerText = clientBeneficiary.CompanyName;
                lblDateRegistered.InnerText = clientBeneficiary.DateRegistered;
                lblTimeRegistered.InnerText = clientBeneficiary.TimeRegistered;
                lblEmail.InnerText = clientBeneficiary.Email;
                lblFullName.InnerText = clientBeneficiary.FullName;
                lblSecondGSMNO.InnerText = !string.IsNullOrEmpty(clientBeneficiary.GSMNO2) ? clientBeneficiary.GSMNO2 : "Not available.";
                lblSex.InnerText = (clientBeneficiary.Sex == 1) ? "Male" : "Female";
                lblFirstGSMNO.InnerText = clientBeneficiary.GSMNO1;
                Session["_clientBeneficiary"] = clientBeneficiary;
                Session["_beneficiaryId"] = clientBeneficiary.BeneficiaryId;
                return true;
                
            }
            catch (Exception ex)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again later or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                ErrorManager.LogApplicationError(ex.StackTrace,ex.Source,ex.Message);
                return false;
            }

        }
        private bool SendNotification()
        {
            try
            {
                var user = Membership.GetUser();

                if (user == null)
                {
                    ConfirmAlertBox1.ShowMessage("Your user details could not be retrieved. Your Transaction request notification cannot be sent.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                const string subject = "Transaction request Notification!";

                var message = "One ore more Transaction request(s) from " + user.UserName + " are waiting for your attention.";

                 var delegateService = new ApprovalDelegateService();

                 var approverEmail = delegateService.GetApproverEmail();

                 if (string.IsNullOrEmpty(approverEmail))
                {
                    ConfirmAlertBox1.ShowMessage("The Approver information could not be retrieved. A notification could not be sent for your Transaction request.\n Approval of your request may be delayed.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }
                
                if (!Mailsender(approverEmail, subject, message))
                {
                    ConfirmAlertBox1.ShowMessage("A notification could not be sent for your Transaction request due to some technical issues.\n Approval of your request may be delayed.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                return true;
              
            }
            catch (Exception)
            {
                ConfirmAlertBox1.ShowMessage("Your Transactions request notification could not be sent. Approval of your request might be delayed.", ConfirmAlertBox.PopupMessageType.Error);
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
       

    }
}