using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExpenseManager.CoreFramework.AlertControl;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;
using xPlug.BusinessService;

namespace ExpenseManager.ExpenseMgt
{
    public partial class FrmManageCategoriesOfExpenses1 : UserControl
    {
        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadExpenseCategories();

            }

        }
        protected void BtnprocessCategoryClick(object sender, EventArgs e)
        {
            
            try
            {
                if (!ValidateControls())
                {
                    return;
                }

                switch (int.Parse(btnProcessCategory.CommandArgument))
                {
                    case 1:
                        if (!AddExpenseCategory())
                        {
                            return;
                        }
                        break;

                    case 2:
                        if (!UpdateExpenseCategory())
                        {
                            return;
                        }
                       
                        break;

                    default:
                        ErrorDisplayProcessExpenseCategory.ShowError("Invalid process call!");
                        mpeProcessExpenseCategory.Show();
                        break;
                }


                if (!LoadExpenseCategories())
                {
                    return;
                }

                ConfirmAlertBox1.ShowSuccessAlert("Expense Category information was processed successfully.");

            }

            catch (Exception ex)
            {
                ErrorDisplayProcessExpenseCategory.ShowError("An unknown error was encountered. Please try again soon or contact the Admin. Please try again soon or contact the Admin.");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }


        }
        protected void DgExpCatCollectionsEditCommand(object source, DataGridCommandEventArgs e)
        {
            ErrorDisplayProcessExpenseCategory.ClearError();
            
            txtTitle.Text = string.Empty;
            try
            {
                if (Session["_expenseCategoriesList"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Expense Category list is empty or session has expired!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var expenseaCategoriesList = Session["_expenseCategoriesList"] as List<ExpenseCategory>;

                if (expenseaCategoriesList == null || !expenseaCategoriesList.Any())
                {
                    ConfirmAlertBox1.ShowMessage("Expense Category list is empty or session has expired!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }


                dgExpCatCollections.SelectedIndex = e.Item.ItemIndex;

                var id = (DataCheck.IsNumeric(dgExpCatCollections.DataKeys[e.Item.ItemIndex].ToString()))
                             ? long.Parse(dgExpCatCollections.DataKeys[e.Item.ItemIndex].ToString())
                             : 0;

                if (id < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid Selection", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var expenseCategory = expenseaCategoriesList.Find(m => m.ExpenseCategoryId == id);
                if (expenseCategory == null)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid selection!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }
                if (expenseCategory.ExpenseCategoryId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid selection!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                txtTitle.Text = expenseCategory.Title;
                txtCode.Text = expenseCategory.Code;
                chkCategory.Checked = expenseCategory.Status == 1;
                btnProcessCategory.CommandArgument = "2";
                btnProcessCategory.Text = "Update";
                mpeProcessExpenseCategory.Show();
                Session["_expenseCategory"] = expenseCategory;
            }

            catch (Exception ex)
            {
                ErrorDisplayProcessExpenseCategory.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }
        }
        protected void BtnAddNewCategoryClick(object sender, EventArgs e)
        {
            
            ClearControls();
            btnProcessCategory.CommandArgument = "1";
            btnProcessCategory.Text = "Submit";
            mpeProcessExpenseCategory.Show();
        }
        #endregion

        #region Page Helpers
        private bool LoadExpenseCategories()
        {
            try
            {

                var expenseCategoriesList = ServiceProvider.Instance().GetExpenseCategoryServices().GetOrderedExpenseCategories();

                if (expenseCategoriesList == null)
                {
                    ConfirmAlertBox1.ShowMessage("Expense Category list is empty!", ConfirmAlertBox.PopupMessageType.Error);
                    dgExpCatCollections.DataSource = new List<ExpenseCategory>();
                    dgExpCatCollections.DataBind();
                    return false;
                }

                if (!expenseCategoriesList.Any())
                {
                    ConfirmAlertBox1.ShowMessage("Expense Category list is empty!", ConfirmAlertBox.PopupMessageType.Error);
                    dgExpCatCollections.DataSource = new List<ExpenseCategory>();
                    dgExpCatCollections.DataBind();
                    return false;
                }
                dgExpCatCollections.DataSource = expenseCategoriesList;
                dgExpCatCollections.DataBind();
                Session["_expenseCategoriesList"] = expenseCategoriesList;
                return true;
            }
            catch (Exception ex)
            {
                ErrorDisplayProcessExpenseCategory.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                throw;
            }
        }
        private bool AddExpenseCategory()
        {
            
            try
            {

                var newxpenseCategory = new ExpenseCategory
                                            {
                                                Title = txtTitle.Text.Trim(),
                                                Code = txtCode.Text.Trim(),
                                                Status = chkCategory.Checked ? 1 : 0
                                            };

                var k = ServiceProvider.Instance().GetExpenseCategoryServices().AddExpenseCategoryCheckDuplicate(newxpenseCategory);

                if (k < 1)
                {
                    if (k == -3)
                    {
                        ConfirmAlertBox1.ShowMessage("Expense Category Information already exists.", ConfirmAlertBox.PopupMessageType.Error);
                        return false;
                    }
                    ConfirmAlertBox1.ShowMessage("Expense Category Information could not be added.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                ConfirmAlertBox1.ShowSuccessAlert("ExpenseCategory Information was added to successfully.");
                return true;
            }
            catch (Exception ex)
            {
                ErrorDisplayProcessExpenseCategory.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }

        }
        private bool UpdateExpenseCategory()
        {
            try
            {
                if (Session["_expenseCategory"] == null)
                {
                    ErrorDisplayProcessExpenseCategory.ShowError("Expense Category list is empty or session has expired.");
                    mpeProcessExpenseCategory.Show();
                    return false;
                }

                var expenseCategory = Session["_expenseCategory"] as ExpenseCategory;

                if (expenseCategory == null || expenseCategory.ExpenseCategoryId < 1)
                {
                    ErrorDisplayProcessExpenseCategory.ShowError("Invalid selection!");
                    mpeProcessExpenseCategory.Show();
                    return false;
                }
                
                expenseCategory.Title = txtTitle.Text.Trim();
                expenseCategory.Code = txtCode.Text.Trim();
                expenseCategory.Status = chkCategory.Checked ? 1 : 0;
                var k = ServiceProvider.Instance().GetExpenseCategoryServices().UpdateExpenseCategoryCheckDuplicate(expenseCategory);
                if (k < 1)
                {
                    if (k == -3)
                    {
                        ErrorDisplayProcessExpenseCategory.ShowError("Expense Category already exists.");
                        txtTitle.Focus();
                        mpeProcessExpenseCategory.Show();
                    }


                    else
                    {
                        ErrorDisplayProcessExpenseCategory.ShowError("Expense Category information could not be updated.");
                        mpeProcessExpenseCategory.Show();
                    }

                }

                return true;
            }
            catch (Exception ex)
            {
                ErrorDisplayProcessExpenseCategory.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }
        private bool ValidateControls()
        {
            
            ErrorDisplayProcessExpenseCategory.ClearError();
            if (string.IsNullOrEmpty(txtTitle.Text.Trim()))
            {
                ErrorDisplayProcessExpenseCategory.ShowError("Please supply an Expense Category.");
                txtTitle.Focus();
                mpeProcessExpenseCategory.Show();
                return false;
            }

            if (string.IsNullOrEmpty(txtCode.Text.Trim()))
            {
                ErrorDisplayProcessExpenseCategory.ShowError("Please supply a code for the category.");
                txtCode.Focus();
                mpeProcessExpenseCategory.Show();
                return false;
            }

            if (!DataCheck.IsNumeric(txtCode.Text.Trim()))
            {
                ErrorDisplayProcessExpenseCategory.ShowError("Invalid entry!");
                txtCode.Focus();
                mpeProcessExpenseCategory.Show();
                return false;
            }

            return true;
        }
        private void ClearControls()
        {
            ErrorDisplayProcessExpenseCategory.ClearError();
            txtTitle.Text = string.Empty;
            txtCode.Text = string.Empty;
            chkCategory.Checked = false;
        }
        #endregion
    }
}