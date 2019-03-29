using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;
using xPlug.BusinessService;

namespace ExpenseManager.ExpenseMgt.Reports
{
    public partial class FrmTransactionItemsReport : UserControl
    {
        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindGridWithDefaultList();
                LoadAccountsHead();

            }
        }
        protected void DdlAccountsHeadSelectedChanged(object sender, EventArgs e)
        {
            if(int.Parse(ddlAccountsHead.SelectedValue) < 1)
            {
                ErrorDisplay1.ShowError("Invalid selection!");
                return;
            }
            if (!LoadExpenseItemsByAccountHead(int.Parse(ddlAccountsHead.SelectedValue)))
           {
           }
        }
        protected void BtnRefreshClick(object sender, EventArgs e)
        {
            ddlAccountsHead.SelectedIndex = 0;

           if(!LoadAllExpenseItems())
           {
           }
        }
        #endregion

        #region Page Helpers
        private void BindGridWithDefaultList()
        {
            dgExpenseItem.DataSource = new List<ExpenseItem>();
            dgExpenseItem.DataBind();
        }
        private bool LoadAllExpenseItems()
        {
            try
            {
                var expenseItemList = ServiceProvider.Instance().GetExpenseItemServices().GetExpenseItems();

                if (!expenseItemList.Any())
                {
                    ErrorDisplay1.ShowError("Expense Item list is empty.");
                    dgExpenseItem.DataSource = new List<ExpenseItem>();
                    dgExpenseItem.DataBind();
                    return false;
                }

                dgExpenseItem.DataSource = expenseItemList;
                dgExpenseItem.DataBind();
                return true;
            }
            catch (Exception ex)
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }
        private void LoadAccountsHead()
        {
            try
            {
                var filteredAccountsHeadsList = ServiceProvider.Instance().GetAccountsHeadServices().GetFilteredAccountsHeads();

                if (!filteredAccountsHeadsList.Any())
                {
                    ErrorDisplay1.ShowError("Accounts Head list is empty or session has expired.");
                    ddlAccountsHead.DataSource = new List<AccountsHead>();
                    ddlAccountsHead.DataBind();
                    ddlAccountsHead.Items.Insert(0, new ListItem("--List is empty--", "0"));
                    ddlAccountsHead.SelectedIndex = 0;
                    return;
                }

                ddlAccountsHead.DataSource = filteredAccountsHeadsList;
                ddlAccountsHead.DataValueField = "AccountsHeadId";
                ddlAccountsHead.DataTextField = "Title";
                ddlAccountsHead.DataBind();
                ddlAccountsHead.Items.Insert(0, new ListItem("---Select an Accounts Head---", "0"));
                ddlAccountsHead.SelectedIndex = 0;
            }

            catch (Exception ex)
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }
        }
        private bool LoadExpenseItemsByAccountHead(int accountsHeadId)
        {
            try
            {
                var expenseItemsList = ServiceProvider.Instance().GetExpenseItemServices().GetOrderedExpenseItemsByAccountsHeadId(accountsHeadId);

                if (expenseItemsList == null || !expenseItemsList.Any())
                {
                    ErrorDisplay1.ShowError("Expense Item list is empty.");
                    dgExpenseItem.DataSource = new List<ExpenseItem>();
                    dgExpenseItem.DataBind();
                    return false;
                }
                
                dgExpenseItem.DataSource = expenseItemsList;
                dgExpenseItem.DataBind();
                return true;
            }
            catch (Exception ex)
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }
        #endregion
    }
}