using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExpenseManager.CoreFramework;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;
using xPlug.BusinessService;

namespace ExpenseManager.ExpenseMgt.Reports
{
    public partial class FrmStaffBeneficiaryTransactionReport : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["_expTransactions"] = null;
                hTitle.InnerText = "Staff Beneficiaries' Transactions Report";
                dvStaffVouchers.Visible = false;
                lnkCloseVouchersSection.Visible = false;

                if (!LoadFilteredPortalUsers())
                {
         
                }
                if (!LoadDepartments())
                {
                }

            }
        }

        #region Transaction Report
        #region Events
        protected void LnkFilterByApprovedClick(object sender, EventArgs e)
        {
            try
            {
                ErrorDisplay1.ClearError();
                var expTransactions = ServiceProvider.Instance().GetStaffExpenseTransactionServices().GetStaffApprovedOrVoidedExpenseTransactions();

                if (!expTransactions.Any())
                {
                    ErrorDisplay1.ShowError("Approved Transaction list is empty.");
                    dgExpenseTransaction.DataSource = new List<StaffExpenseTransaction>();
                    dgExpenseTransaction.DataBind();
                    return;
                }

                dgExpenseTransaction.DataSource = expTransactions;
                dgExpenseTransaction.DataBind();
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
            }
        }
        protected void LnkFilterByUnApprovedClick(object sender, EventArgs e)
        {
            try
            {
                ErrorDisplay1.ClearError();
                var expTransactions = ServiceProvider.Instance().GetStaffExpenseTransactionServices().GetUnApprovedctiveExpenseTransactions();

                if (!expTransactions.Any())
                {
                    ErrorDisplay1.ShowError("Unapproved Expense Transaction list is empty.");
                    dgExpenseTransaction.DataSource = new List<StaffExpenseTransaction>();
                    dgExpenseTransaction.DataBind();
                    return;
                }

                dgExpenseTransaction.DataSource = expTransactions;
                dgExpenseTransaction.DataBind();
                Session["_expTransactions"] = expTransactions;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
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
        protected void DdlDepartmentIndexChanged(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();

            try
            {
                ddlPortalUser.SelectedIndex = 0;
                txtEndDate.Text = string.Empty;
                txtStart.Text = string.Empty;
                if (int.Parse(ddlDepartment.SelectedValue) < 1)
                {
                    ErrorDisplay1.ShowError("Please select a Department.");
                    dgExpenseTransaction.DataSource = new List<StaffExpenseTransaction>();
                    dgExpenseTransaction.DataBind();
                    return;
                }

                if (!LoadTransactionsByDepartment())
                {
                }

            }
            catch
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
            }
        }
        protected void DdlPortalUserIndexChanged(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();

            try
            {
                ddlDepartment.SelectedIndex = 0;
                txtEndDate.Text = string.Empty;
                txtStart.Text = string.Empty;
                if (int.Parse(ddlPortalUser.SelectedValue) < 1)
                {
                    ErrorDisplay1.ShowError("Please select a User.");
                    dgExpenseTransaction.DataSource = new List<StaffExpenseTransaction>();
                    dgExpenseTransaction.DataBind();
                    return;
                }

                if (!LoadTransactionsByPortalUser(int.Parse(ddlPortalUser.SelectedValue)))
                {
                }
            }
            catch
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
            }
        }
        protected void BtnRefreshClick(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            ddlDepartment.SelectedIndex = 0;
            ddlPortalUser.SelectedIndex = 0;
            txtEndDate.Text = string.Empty;
            txtEndDate.Text = string.Empty;
            if (!LoadAllTransactions())
            {
            }

            //lblFilterReport.InnerHtml = "All Transaction Payments";
        }
        #endregion

        #region Helpers
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
        private bool LoadFilteredPortalUsers()
        {
            try
            {
                ErrorDisplay1.ClearError();

                dgExpenseTransaction.DataSource = new List<StaffExpenseTransaction>();
                dgExpenseTransaction.DataBind();

                ddlPortalUser.DataSource = new List<portaluser>();
                ddlPortalUser.Items.Insert(0, new ListItem("--List is empty--", "0"));
                ddlPortalUser.SelectedIndex = 0;

                ddlStaffPortalUsersVouchers.DataSource = new List<portaluser>();
                ddlStaffPortalUsersVouchers.Items.Insert(0, new ListItem("--List is empty--", "0"));
                ddlStaffPortalUsersVouchers.SelectedIndex = 0;


                var portalUsersWithRegisteredTransactions = ServiceProvider.Instance().GetStaffExpenseTransactionServices().GetFilteredPortalUsers();
                
                if (portalUsersWithRegisteredTransactions == null || !portalUsersWithRegisteredTransactions.Any())
                {
                    ErrorDisplay1.ShowError("Portal User Transaction list is empty.");
                    return false;
                }
                var idsForUsersWithUnApprovedTransactions = portalUsersWithRegisteredTransactions.ElementAt(0).Key.FindAll(m => m > 0).ToList();
                var idsForUsersWithApprovedTransactions = portalUsersWithRegisteredTransactions.ElementAt(0).Value.FindAll(m => m > 0).ToList();
                var portalUsers = new PortalServiceManager().GetPortalUserList();
                if (portalUsers == null || !portalUsers.Any())
                {
                    return false;
                }


                var portalUsersWithUnApprovedTransactions = new List<portaluser>();

                var portalUsersWithApprovedTransactions = new List<portaluser>();


                foreach (var userId in idsForUsersWithUnApprovedTransactions.Where(portaluserId => portalUsersWithUnApprovedTransactions.All(m => m.UserId != portaluserId)))
                {
                    portalUsersWithUnApprovedTransactions.Add(portalUsers.Find(m => m.UserId == userId));
                }

                foreach (var userId in idsForUsersWithApprovedTransactions.Where(portaluserId => portalUsersWithApprovedTransactions.All(m => m.UserId != portaluserId)))
                {
                    portalUsersWithApprovedTransactions.Add(portalUsers.Find(m => m.UserId == userId));
                }

                if (portalUsersWithUnApprovedTransactions.Any())
                {
                    ddlPortalUser.DataSource = portalUsersWithUnApprovedTransactions;
                    ddlPortalUser.DataTextField = "UserName";
                    ddlPortalUser.DataValueField = "UserId";
                    ddlPortalUser.DataBind();
                    ddlPortalUser.Items.Insert(0, new ListItem("--Select a User--", "0"));
                    ddlPortalUser.SelectedIndex = 0;
                }

                if (portalUsersWithApprovedTransactions.Any())
                {
                    ddlStaffPortalUsersVouchers.DataSource = portalUsersWithApprovedTransactions;
                    ddlStaffPortalUsersVouchers.DataTextField = "UserName";
                    ddlStaffPortalUsersVouchers.DataValueField = "UserId";
                    ddlStaffPortalUsersVouchers.DataBind();
                    ddlStaffPortalUsersVouchers.Items.Insert(0, new ListItem("--Select a User--", "0"));
                    ddlStaffPortalUsersVouchers.SelectedIndex = 0;
                }
                
                dgExpenseTransaction.DataSource = new List<StaffExpenseTransaction>();
                dgExpenseTransaction.DataBind();
                SetApprovedTransactionStyle();
                Session["_filteredStaffPortalUserList"] = portalUsersWithUnApprovedTransactions;
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                return false;
            }

        }
        private bool LoadDepartments()
        {
            try
            {
                ErrorDisplay1.ClearError();

                var filteredDepartmentsList = ServiceProvider.Instance().GetDepartmentServices().GetFilteredDepartments();
                
                if (filteredDepartmentsList == null)
                {
                    ErrorDisplay1.ShowError("Department list is empty");
                    ddlDepartment.DataSource = new List<Department>();
                    ddlDepartment.Items.Insert(0, new ListItem("--List is empty--", "0"));
                    ddlDepartment.SelectedIndex = 0;

                    ddlStaffDepartmentVouchers.DataSource = new List<Department>();
                    ddlStaffDepartmentVouchers.Items.Insert(0, new ListItem("--List is empty--", "0"));
                    ddlStaffDepartmentVouchers.SelectedIndex = 0;
                    return false;
                }

                if (!filteredDepartmentsList.Any())
                {
                    ErrorDisplay1.ShowError("Department list is empty");
                    ddlDepartment.DataSource = new List<Department>();
                    ddlDepartment.Items.Insert(0, new ListItem("--List is empty--", "0"));
                    ddlDepartment.SelectedIndex = 0;

                    ddlStaffDepartmentVouchers.DataSource = new List<Department>();
                    ddlStaffDepartmentVouchers.Items.Insert(0, new ListItem("--List is empty--", "0"));
                    ddlStaffDepartmentVouchers.SelectedIndex = 0;
                    return false;
                }


                var departmentsWithUnApprovedTransactions = filteredDepartmentsList.ElementAt(0).Key.FindAll(m => m.DepartmentId > 0).ToList();
                var departmentsWithApprovedTransactions = filteredDepartmentsList.ElementAt(0).Value.FindAll(m => m.DepartmentId > 0).ToList();

                if (departmentsWithUnApprovedTransactions.Any())
                {
                    ddlDepartment.DataSource = departmentsWithUnApprovedTransactions;
                    ddlDepartment.DataTextField = "Name";
                    ddlDepartment.DataValueField = "DepartmentId";
                    ddlDepartment.DataBind();
                    ddlDepartment.Items.Insert(0, new ListItem("--Select Department--", "0"));
                    ddlDepartment.SelectedIndex = 0;

                }

                if (departmentsWithApprovedTransactions.Any())
                {
                    ddlStaffDepartmentVouchers.DataSource = departmentsWithApprovedTransactions;
                    ddlStaffDepartmentVouchers.DataTextField = "Name";
                    ddlStaffDepartmentVouchers.DataValueField = "DepartmentId";
                    ddlStaffDepartmentVouchers.DataBind();
                    ddlStaffDepartmentVouchers.Items.Insert(0, new ListItem("--Select Department--", "0"));
                    ddlStaffDepartmentVouchers.SelectedIndex = 0;
                   
                }
               
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                return false;
            }

        }
        private bool LoadTransactionsByDepartment()
        {
            try
            {
                var departmentTransactions = ServiceProvider.Instance().GetStaffExpenseTransactionServices().GetTransactionsByDepartmentId(int.Parse(ddlDepartment.SelectedValue));

                if (departmentTransactions == null || !departmentTransactions.Any())
                {
                    ErrorDisplay1.ShowError("Expense Transaction list is empty or session has expired.");
                    dgExpenseTransaction.DataSource = new List<StaffExpenseTransaction>();
                    dgExpenseTransaction.DataBind();
                    return false;
                }

                var accountsHead = ServiceProvider.Instance().GetAccountsHeadServices().GetAccountsHeads();

                if (accountsHead == null || !accountsHead.Any())
                {
                    ErrorDisplay1.ShowError("Expense Transaction list is empty.");
                    dgExpenseTransaction.DataSource = new List<ExpenseTransaction>();
                    dgExpenseTransaction.DataBind();
                    return false;
                }
                Parallel.ForEach(departmentTransactions, item =>
                {
                    item.AccountsHead = accountsHead.Find(m => m.AccountsHeadId == item.ExpenseItem.AccountsHeadId).Title;
                });


                dgExpenseTransaction.DataSource = departmentTransactions;
                dgExpenseTransaction.DataBind();
                Session["_expTransactions"] = departmentTransactions;
                return true;
            }
            catch (Exception ex)
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }
        private bool LoadTransactionsByPortalUser(int portalUserId)
        {
            try
            {
                var transactionsList = ServiceProvider.Instance().GetStaffExpenseTransactionServices().GetTransactionsByPortalUser(portalUserId);

                if (!transactionsList.Any())
                {
                    ErrorDisplay1.ShowError("Transaction list is empty.");
                    dgExpenseTransaction.DataSource = new List<StaffExpenseTransaction>();
                    dgExpenseTransaction.DataBind();
                    return false;
                }

                var accountsHead = ServiceProvider.Instance().GetAccountsHeadServices().GetAccountsHeads();

                if (accountsHead == null || !accountsHead.Any())
                {
                    ErrorDisplay1.ShowError("Expense Transaction list is empty.");
                    dgExpenseTransaction.DataSource = new List<ExpenseTransaction>();
                    dgExpenseTransaction.DataBind();
                    return false;
                }
                Parallel.ForEach(transactionsList, item =>
                {
                    item.AccountsHead = accountsHead.Find(m => m.AccountsHeadId == item.ExpenseItem.AccountsHeadId).Title;
                });


                dgExpenseTransaction.DataSource = transactionsList;
                dgExpenseTransaction.DataBind();
                Session["_expTransactions"] = transactionsList;
                return true;
            }
            catch (Exception ex)
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }
        private bool GetTransactionsByDate()
        {

            ErrorDisplay1.ClearError();
            try
            {
                var expTransactionsByDate = new List<StaffExpenseTransaction>();

                dgExpenseTransaction.DataSource = new List<StaffExpenseTransaction>();
                dgExpenseTransaction.DataBind();

                if (int.Parse(ddlDepartment.SelectedValue) < 1 && int.Parse(ddlPortalUser.SelectedValue) < 1)
                {
                    if (!string.IsNullOrEmpty(txtStart.Text.Trim()) && !string.IsNullOrEmpty(txtEndDate.Text.Trim()))
                    {
                        var startDateString = DateMap.ReverseToServerDate(txtStart.Text.Trim());
                        var startDate = DateTime.Parse(startDateString);
                        var endDateString = DateMap.ReverseToServerDate(txtEndDate.Text.Trim());
                        var endDate = DateTime.Parse(endDateString);
                        if (endDate < startDate || startDate > endDate)
                        {
                            ErrorDisplay1.ShowError("The 'From' date must not be LATER than the 'To' date.");
                            dgExpenseTransaction.DataSource = new List<StaffExpenseTransaction>();
                            dgExpenseTransaction.DataBind();
                            return false;
                        }

                        expTransactionsByDate = ServiceProvider.Instance().GetStaffExpenseTransactionServices().GetExpenseTransactionsByDateRange(startDate, endDate);
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(txtEndDate.Text.Trim()) && string.IsNullOrEmpty(txtStart.Text.Trim()))
                        {
                            expTransactionsByDate = ServiceProvider.Instance().GetStaffExpenseTransactionServices().GetExpenseTransactionsByDate(DateMap.ReverseToServerDate(txtEndDate.Text.Trim()));

                        }

                        if (string.IsNullOrEmpty(txtEndDate.Text.Trim()) && !string.IsNullOrEmpty(txtStart.Text.Trim()))
                        {
                            expTransactionsByDate = ServiceProvider.Instance().GetStaffExpenseTransactionServices().GetExpenseTransactionsByDate(DateMap.ReverseToServerDate(txtStart.Text.Trim()));
                        }
                    }

                }

                var portalUserId = int.Parse(ddlPortalUser.SelectedValue);
                var departmentId = int.Parse(ddlDepartment.SelectedValue);

                if (departmentId > 0 || portalUserId > 0)
                {

                    if (!string.IsNullOrEmpty(txtStart.Text.Trim()) && !string.IsNullOrEmpty(txtEndDate.Text.Trim()))
                    {
                        var startDateString = DateMap.ReverseToServerDate(txtStart.Text.Trim());
                        var startDate = DateTime.Parse(startDateString);
                        var endDateString = DateMap.ReverseToServerDate(txtEndDate.Text.Trim());
                        var endDate = DateTime.Parse(endDateString);

                        if (endDate < startDate || startDate > endDate)
                        {
                            ErrorDisplay1.ShowError("The 'From' date must not be soon than the 'To' date.");
                            dgExpenseTransaction.DataSource = new List<StaffExpenseTransaction>();
                            dgExpenseTransaction.DataBind();
                            return false;
                        }

                        expTransactionsByDate = ServiceProvider.Instance().GetStaffExpenseTransactionServices().GetExpenseTransactionsByDateRange(startDate, endDate);
                    }

                    else
                    {
                        if (string.IsNullOrEmpty(txtStart.Text.Trim()) && !string.IsNullOrEmpty(txtEndDate.Text.Trim()))
                        {
                            expTransactionsByDate = ServiceProvider.Instance().GetStaffExpenseTransactionServices().GetExpenseTransactionsByDate(DateMap.ReverseToServerDate(txtEndDate.Text.Trim()));
                        }

                        if (!string.IsNullOrEmpty(txtStart.Text.Trim()) && string.IsNullOrEmpty(txtEndDate.Text.Trim()))
                        {
                            var date = DateMap.ReverseToServerDate(txtStart.Text.Trim());
                            expTransactionsByDate = ServiceProvider.Instance().GetStaffExpenseTransactionServices().GetExpenseTransactionsByDate(date);
                        }
                    }

                }

                var accountsHead = ServiceProvider.Instance().GetAccountsHeadServices().GetAccountsHeads();

                if (accountsHead == null || !accountsHead.Any())
                {
                    ErrorDisplay1.ShowError("Expense Transaction list is empty.");
                    dgExpenseTransaction.DataSource = new List<ExpenseTransaction>();
                    dgExpenseTransaction.DataBind();
                    return false;
                }
                Parallel.ForEach(expTransactionsByDate, item =>
                {
                    item.AccountsHead = accountsHead.Find(m => m.AccountsHeadId == item.ExpenseItem.AccountsHeadId).Title;
                });

                if (!expTransactionsByDate.Any())
                {
                    ErrorDisplay1.ShowError("No record found.");
                    return false;
                }

                dgExpenseTransaction.DataSource = expTransactionsByDate;
                dgExpenseTransaction.DataBind();
                SetApprovedTransactionStyle();
                Session["_expTransactions"] = expTransactionsByDate;
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


                if (!string.IsNullOrEmpty(DateMap.ReverseToServerDate(txtEndDate.Text.Trim())))
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
        private void SetApprovedTransactionStyle()
        {
            try
            {
                if (dgExpenseTransaction.Items.Count > 0)
                {

                    for (var i = 0; i < dgExpenseTransaction.Items.Count; i++)
                    {
                        var approvedLabel = ((Label)dgExpenseTransaction.Items[i].FindControl("lblStatus"));
                        if (approvedLabel.Text == "Approved")
                        {
                            approvedLabel.Style.Add("color", "darkcyan");
                            approvedLabel.Style.Add("font-weight", "bold");
                        }

                        if (approvedLabel.Text == "Pending")
                        {
                            approvedLabel.Style.Add("color", "maroon");
                            approvedLabel.Style.Add("font-weight", "bold");
                        }

                        if (approvedLabel.Text == "Rejected")
                        {
                            approvedLabel.Style.Add("color", "red");
                            approvedLabel.Style.Add("font-weight", "bold");
                        }

                        if (approvedLabel.Text == "Voided")
                        {
                            approvedLabel.Style.Add("color", "dodgerblue");
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
        private bool LoadAllTransactions()
        {
            try
            {
                var allTransactionsList = ServiceProvider.Instance().GetStaffExpenseTransactionServices().GetAllStaffExpenseTransactions();


                if (allTransactionsList == null || !allTransactionsList.Any())
                {
                    ErrorDisplay1.ShowError("Transaction list is empty or session has expired.");
                    dgExpenseTransaction.DataSource = new List<StaffExpenseTransaction>();
                    dgExpenseTransaction.DataBind();
                    return false;
                }

                var accountsHead = ServiceProvider.Instance().GetAccountsHeadServices().GetAccountsHeads();

                if (accountsHead == null || !accountsHead.Any())
                {
                    ErrorDisplay1.ShowError("Expense Transaction list is empty.");
                    dgExpenseTransaction.DataSource = new List<ExpenseTransaction>();
                    dgExpenseTransaction.DataBind();
                    return false;
                }
                Parallel.ForEach(allTransactionsList, item =>
                {
                    item.AccountsHead = accountsHead.Find(m => m.AccountsHeadId == item.ExpenseItem.AccountsHeadId).Title;
                });

                dgExpenseTransaction.DataSource = allTransactionsList;
                dgExpenseTransaction.DataBind();
                SetApprovedTransactionStyle();
                Session["_expTransactions"] = allTransactionsList;
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
        #endregion

        #region Transaction Vouchers
        #region Events
        protected void LnkCloseVouchersSectionClick(object sender, EventArgs e)
        {
            hTitle.InnerText = "Staff Beneficiaries' Transactions Report";
            dvStaffVouchers.Visible = false;
            lnkViewPrintVouchers.Visible = true;
            lnkCloseVouchersSection.Visible = false;
            tblTransactionsReport.Visible = true;
        }
        protected void LnkViewPrintVouchersClick(object sender, EventArgs e)
        {
            hTitle.InnerText = "Staff Beneficiaries' Transaction Vouchers";
            dvStaffVouchers.Visible = true;
            lnkViewPrintVouchers.Visible = false;
            lnkCloseVouchersSection.Visible = true;
            tblTransactionsReport.Visible = false;
        }
        #endregion
        #region Helpers
        //All page Helpers implemented with jQuery via Webservice
        #endregion
        #endregion
    }
}