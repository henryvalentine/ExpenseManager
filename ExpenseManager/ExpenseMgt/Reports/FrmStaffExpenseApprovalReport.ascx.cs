using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExpenseManager.CoreFramework;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;
using xPlug.BusinessService;

namespace ExpenseManager.ExpenseMgt.Reports
{
    public partial class FrmStaffExpenseApprovalReport : UserControl
    {
        #region Methods
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                dgApprovedTransactions.DataSource = new List<StaffExpenseApproval>();
                dgApprovedTransactions.DataBind();

                if(!LoadPortalUsers())
                {
                    
                }
            }
        }

        protected void BtnDateFilterClick(object sender, EventArgs e)
        {
            if (!ValidateControls())
            {
                return;
            }

            if (!GetTransactionsByDate())
            {
            }

        }

        protected void BtnRefreshClick(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            txtEndDate.Text = string.Empty;
            txtEndDate.Text = string.Empty;
            if (!LoadAllTransactions())
            {
            }
        }
        #endregion

        #region Helpers

        private bool GetTransactionsByDate()
        {

            ErrorDisplay1.ClearError();
            try
            {
                dgApprovedTransactions.DataSource = new List<StaffExpenseApproval>();
                dgApprovedTransactions.DataBind();

                var approvedTransactionsByDate = new List<StaffExpenseApproval>();

                if (!string.IsNullOrEmpty(txtStart.Text.Trim()) && !string.IsNullOrEmpty(txtEndDate.Text.Trim()))
                {
                    var startDateString = DateMap.ReverseToServerDate(txtStart.Text.Trim());
                    var startDate = DateTime.Parse(startDateString);
                    var endDateString = DateMap.ReverseToServerDate(txtEndDate.Text.Trim());
                    var endDate = DateTime.Parse(endDateString);
                    approvedTransactionsByDate = ServiceProvider.Instance().GetStaffExpenseApprovalServices().GetStaffExpenseApprovalsDateRange(startDate, endDate);
                }

                else
                {
                    if (!string.IsNullOrEmpty(txtEndDate.Text.Trim()) && string.IsNullOrEmpty(txtStart.Text.Trim()))
                    {
                        approvedTransactionsByDate = ServiceProvider.Instance().GetStaffExpenseApprovalServices().GetStaffExpenseApprovalsDate(DateMap.ReverseToServerDate(txtEndDate.Text.Trim()));
                    }

                    if (string.IsNullOrEmpty(txtEndDate.Text.Trim()) && !string.IsNullOrEmpty(txtStart.Text.Trim()))
                    {
                        approvedTransactionsByDate =
                            ServiceProvider.Instance().GetStaffExpenseApprovalServices().GetStaffExpenseApprovalsDate(DateMap.ReverseToServerDate(txtStart.Text.Trim()));
                    }


                    if (!approvedTransactionsByDate.Any())
                    {
                        ErrorDisplay1.ShowError("No record found.");
                        return false;
                    }
                }

                if (Session["_userList"] == null)
                {
                    ErrorDisplay1.ShowError("Transaction Approver list is empty or session has expired.");
                    return false;
                }

                var userList = Session["_userList"] as List<portaluser>;

                if (userList == null || !userList.Any())
                {
                    ErrorDisplay1.ShowError("Transaction Approver list is empty or session has expired.");
                    return false;
                }

                foreach (var expenseTransaction in approvedTransactionsByDate)
                {
                    expenseTransaction.ApprovedBy = userList.Find(m => m.UserId == expenseTransaction.ApprovedById).FirstName + " " + userList.Find(m => m.UserId == expenseTransaction.ApprovedById).LastName;
                }

                dgApprovedTransactions.DataSource = approvedTransactionsByDate;
                dgApprovedTransactions.DataBind();
                SetApprovedTransactionStyle();
                return true;

            }
            catch (Exception ex)
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }

        }

        private bool ValidateControls()
        {
            try
            {
                if (string.IsNullOrEmpty(txtStart.Text.Trim()) && string.IsNullOrEmpty(txtEndDate.Text.Trim()))
                {
                    ErrorDisplay1.ShowError("Please enter at least a date.");
                    return false;
                }

                if (!string.IsNullOrEmpty(txtStart.Text.Trim()))
                {
                    if (!DateMap.IsDate(DateMap.ReverseToServerDate(txtStart.Text.Trim())))
                    {
                        ErrorDisplay1.ShowError("Please enter a valid date.");
                        return false;
                    }
                }


                if (!string.IsNullOrEmpty(txtEndDate.Text.Trim()))
                {
                    if (!DateMap.IsDate(DateMap.ReverseToServerDate(txtEndDate.Text.Trim())))
                    {
                        ErrorDisplay1.ShowError("Please enter a valid date.");
                        return false;
                    }
                }

                return true;
            }
            catch
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                return false;
            }
        }

        private bool LoadPortalUsers()
        {
            var userList = new PortalServiceManager().GetPortalUserList();
            if (userList == null || !userList.Any())
            {
                return false;
            }

            Session["_userList"] = userList;
            return true;
        }

        private bool LoadAllTransactions()
        {
            try
            {

                var allApprovedTransactions = ServiceProvider.Instance().GetStaffExpenseApprovalServices().GetAllStaffExpenseApprovals();

                if (!allApprovedTransactions.Any())
                {
                    ErrorDisplay1.ShowError("Approved Transaction list is empty.");
                    dgApprovedTransactions.DataSource = new List<StaffExpenseApproval>();
                    dgApprovedTransactions.DataBind();
                    return false;
                }

                if (Session["_userList"] == null)
                {
                    ErrorDisplay1.ShowError("Transaction Approver list is empty or session has expired.");
                    return false;
                }

                var userList = Session["_userList"] as List<portaluser>;

                if (userList == null || !userList.Any())
                {
                    ErrorDisplay1.ShowError("Transaction Approver list is empty or session has expired.");
                    return false;
                }

                foreach (var expenseTransaction in allApprovedTransactions)
                {
                    expenseTransaction.ApprovedBy = userList.Find(m => m.UserId == expenseTransaction.ApprovedById).FirstName + " " + userList.Find(m => m.UserId == expenseTransaction.ApprovedById).LastName;
                }

                dgApprovedTransactions.DataSource = allApprovedTransactions;
                dgApprovedTransactions.DataBind();
                SetApprovedTransactionStyle();
                Session["_allApprovedTransactions"] = allApprovedTransactions;
                return true;
            }
            catch (Exception)
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                return false;
            }
        }

        private void SetApprovedTransactionStyle()
        {
            try
            {
                if (dgApprovedTransactions.Items.Count > 0)
                {

                    for (var i = 0; i < dgApprovedTransactions.Items.Count; i++)
                    {
                        var approvedLabel = ((Label)dgApprovedTransactions.Items[i].FindControl("lblStatus"));
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
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
            }
        }
        #endregion 
    }
}