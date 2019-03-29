using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;
using xPlug.BusinessService;

namespace ExpenseManager.ExpenseMgt.Reports
{
    public partial class FrmCategoriesOfTransactions : UserControl
    {
        #region Page Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
               if(! LoadExpenseCategories())
               {
               }

            }

        }
        #endregion

        #region Page Helper
        private bool LoadExpenseCategories()
        {
            try
            {

                var expenseCategoriesList =
                    ServiceProvider.Instance().GetExpenseCategoryServices().GetExpenseCategories();

                if (expenseCategoriesList == null || !expenseCategoriesList.Any())
                {
                    ErrorDisplay1.ShowError("Expense Category list is empty!");
                    dgExpCatCollections.DataSource = new List<ExpenseCategory>();
                    dgExpCatCollections.DataBind();
                    return false;
                }
                
                expenseCategoriesList = expenseCategoriesList.OrderBy(m => m.Title).ToList();
                dgExpCatCollections.DataSource = expenseCategoriesList;
                dgExpCatCollections.DataBind();
                Session["_expenseCategoriesList"] = expenseCategoriesList;
                return true;
            }
            catch (Exception ex)
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                throw;
            }
        }
        #endregion
    }
}