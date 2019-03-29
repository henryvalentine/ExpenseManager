using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using ExpenseManager.CoreFramework.AlertControl;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;
using xPlug.BusinessService;

namespace ExpenseManager.ExpenseMgt
{
    public partial class FrmManageTypesOfExpenses1 : System.Web.UI.UserControl
    {


        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;
            if (!LoadTransactionTypes())
            {
            }
        }
        protected void BtnSubmitClick(object sender, EventArgs e)
        {
            
            try
            {
                if (!ValidateControls())
                {
                    return;
                }

                switch (int.Parse(btnSubmit.CommandArgument))
                {
                    case 1:
                        if (!AddTransactionType())
                        {

                            return;
                        }
                        
                        break;

                    case 2:
                        if (!UpdateTransactionType())
                        {
                            return;
                        }
                       
                        break;

                    default:
                       ConfirmAlertBox1.ShowMessage("Invalid Process call!", ConfirmAlertBox.PopupMessageType.Error);
                        break;
                }
                
                if (!LoadTransactionTypes())
                {
                }

                ConfirmAlertBox1.ShowSuccessAlert("The Expense Type information was processed successfully.");
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
               ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        protected void DgTypesOfExpenseTransactionsEditCommand(object source, DataGridCommandEventArgs e)
        {
            
            ErrorDisplayTransactionType.ClearError();
            chkTransactionType.Checked = false;
            txtTransactionType.Text = string.Empty;
            try
            {
                if (Session["_transactionTypesList"] == null)
                {
                   ConfirmAlertBox1.ShowMessage("Expense Types list is empty or session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }
                var transactionTypesList = (List<ExpenseType>)Session["_transactionTypesList"];

                if (transactionTypesList == null)
                {
                   ConfirmAlertBox1.ShowMessage("Expense Types list is empty or session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                if (!transactionTypesList.Any())
                {
                   ConfirmAlertBox1.ShowMessage("Expense Types list is empty or session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                dgTypesOfExpenseTransactions.SelectedIndex = e.Item.ItemIndex;

                long id = (DataCheck.IsNumeric(dgTypesOfExpenseTransactions.DataKeys[e.Item.ItemIndex].ToString())) ? long.Parse(dgTypesOfExpenseTransactions.DataKeys[e.Item.ItemIndex].ToString()) : 0;

                if (id < 1)
                {
                   ConfirmAlertBox1.ShowMessage("Invalid Record Selection!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var expenseType = transactionTypesList.Find(m => m.ExpenseTypeId == id);

                if (expenseType == null)
                {
                   ConfirmAlertBox1.ShowMessage("Invalid record selection.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                if (expenseType.ExpenseTypeId < 1)
                {
                   ConfirmAlertBox1.ShowMessage("Invalid record selection.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }
                chkTransactionType.Checked = false;
                txtTransactionType.Text = string.Empty;
                txtTransactionType.Text = expenseType.Name;
                btnSubmit.CommandArgument = "2";
                btnSubmit.Text = "Update";
                chkTransactionType.Checked = expenseType.Status == 1;
                lgTitle.InnerHtml = "Update Expense Type";
                mpeProcessTypesOfExpensesPopup.Show();
                Session["_expenseType"] = expenseType;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
               ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        protected void DgTypesOfExpenseTransactionsDeleteCommand(object source, DataGridCommandEventArgs e)
        {
            
            try
            {
                if (Session["_transactionTypesList"] == null)
                {
                   ConfirmAlertBox1.ShowMessage("Expense Types list is empty or session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }
                var transactionTypesList = (List<ExpenseType>)Session["_transactionTypesList"];

                if (transactionTypesList == null)
                {
                   ConfirmAlertBox1.ShowMessage("Expense Types list is empty or session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                if (!transactionTypesList.Any())
                {
                   ConfirmAlertBox1.ShowMessage("Expense Types list is empty or session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                dgTypesOfExpenseTransactions.SelectedIndex = e.Item.ItemIndex;

                long id = (DataCheck.IsNumeric(dgTypesOfExpenseTransactions.DataKeys[e.Item.ItemIndex].ToString())) ? long.Parse(dgTypesOfExpenseTransactions.DataKeys[e.Item.ItemIndex].ToString()) : 0;

                if (id < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid Record Selection!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var expenseType = transactionTypesList.Find(m => m.ExpenseTypeId == id);

                if (expenseType == null)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid record selection.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                if (expenseType.ExpenseTypeId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid record selection.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                if (!ServiceProvider.Instance().GetExpenseTypeServices().DeleteExpenseType(expenseType.ExpenseTypeId))
                {
                    ConfirmAlertBox1.ShowMessage("The Expense Type information could not be deleted!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                ConfirmAlertBox1.ShowSuccessAlert("Expense Type information was deleted successfully.");
                if (!LoadTransactionTypes())
                {
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        protected void BtnAddNewTransactionTypeClick(object sender, EventArgs e)
        {
            
            ErrorDisplayTransactionType.ClearError();
            txtTransactionType.Text = string.Empty;
            chkTransactionType.Checked = false;
            btnSubmit.CommandArgument = "1";
            btnSubmit.Text = "Submit";
            lgTitle.InnerHtml = "Create New Expense Type";
            mpeProcessTypesOfExpensesPopup.Show();
        }
        #endregion

        #region Page Helpers
        private bool LoadTransactionTypes()
        {
            try
            {
                var transactionTypesList = ServiceProvider.Instance().GetExpenseTypeServices().GetExpenseTypes();

                if (!transactionTypesList.Any())
                {
                    ConfirmAlertBox1.ShowMessage("The Expense Types List is empty!", ConfirmAlertBox.PopupMessageType.Error);
                    dgTypesOfExpenseTransactions.DataSource = new List<ExpenseType>();
                    dgTypesOfExpenseTransactions.DataBind();
                    return false;
                }

                dgTypesOfExpenseTransactions.DataSource = transactionTypesList.OrderBy(m => m.Name);
                dgTypesOfExpenseTransactions.DataBind();
                Session["_transactionTypesList"] = transactionTypesList;
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }
        }
        private bool AddTransactionType()
        {
            try
            {
                var transactionType = new ExpenseType
                                        {
                                            Name = txtTransactionType.Text.Trim(),
                                            Status = chkTransactionType.Checked ? 1 : 0
                                        };

                var k = ServiceProvider.Instance().GetExpenseTypeServices().AddExpenseTypeCheckDuplicate(transactionType);

                if (k < 1)
                {
                    if (k == -3)
                    {
                        ErrorDisplayTransactionType.ShowError("Expense Type information already exists!");
                        txtTransactionType.Focus();
                        mpeProcessTypesOfExpensesPopup.Show();
                        return false;
                    }

                    ErrorDisplayTransactionType.ShowError("The Expense Type information Could not be added.");
                    txtTransactionType.Focus();
                    mpeProcessTypesOfExpensesPopup.Show();
                    return false;
                }


                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }
        }
        private bool UpdateTransactionType()
        {
            try
            {
                if (Session["_expenseType"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Expense Types list is empty or session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }
                var expenseType = (ExpenseType)Session["_expenseType"];

                if (expenseType == null)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid record selection.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                if (expenseType.ExpenseTypeId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid record selection.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                expenseType.Name = txtTransactionType.Text.Trim();
                expenseType.Status = chkTransactionType.Checked ? 1 : 0;

                var k = ServiceProvider.Instance().GetExpenseTypeServices().UpdateExpenseTypeCheckDuplicate(expenseType);
                if (k < 1)
                {
                    if (k == -3)
                    {
                        ErrorDisplayTransactionType.ShowError("Expense Type information already exists!");
                        txtTransactionType.Focus();
                        mpeProcessTypesOfExpensesPopup.Show();
                        return false;
                    }

                    ErrorDisplayTransactionType.ShowError("The Expense Type information Could not be updated.");
                    txtTransactionType.Focus();
                    mpeProcessTypesOfExpensesPopup.Show();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }
        }
        private bool ValidateControls()
        {
            try
            {
                if (string.IsNullOrEmpty(txtTransactionType.Text.Trim()))
                {
                    ConfirmAlertBox1.ShowMessage("Please supply an Expense Type.", ConfirmAlertBox.PopupMessageType.Error);
                    txtTransactionType.Focus();
                    mpeProcessTypesOfExpensesPopup.Show();
                    return false;
                }
                if (DataCheck.IsNumeric(txtTransactionType.Text.Trim()))
                {
                    ConfirmAlertBox1.ShowMessage("Invalid entry!", ConfirmAlertBox.PopupMessageType.Error);
                    txtTransactionType.Focus();
                    mpeProcessTypesOfExpensesPopup.Show();
                    return false;
                }
                return true;
            }
            catch 
            {

                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }
        }
        #endregion
    }
}