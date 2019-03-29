using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExpenseManager.CoreFramework;
using ExpenseManager.CoreFramework.AlertControl;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;
using xPlug.BusinessObject.CustomizedASPBusinessObject.Enum;
using xPlug.BusinessService;

namespace ExpenseManager.ExpenseMgt.Reports
{
    public partial class FrmTransactionReport : UserControl
    {
        protected int Offset = 5;
        protected int DataCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dgUserInitiatedTransactions.DataSource = new List<ExpenseTransaction>();
                dgUserInitiatedTransactions.DataBind();
                LoadYearOptions();
                LoadFilterOptions();
                LoadTimeFrameOptions();
                LoadMonthOptions();
                LoadDepartments();
                LoadWeekOptions();
                hTitle.InnerText = "Transaction Report";
                dvUserEntries.Visible = true;
            }
        }
        #region Page Events
        protected void DgUserInitiatedTransactionsItemCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
               
                dgUserInitiatedTransactions.SelectedIndex = e.Item.ItemIndex;

                long id = (DataCheck.IsNumeric(dgUserInitiatedTransactions.DataKeys[e.Item.ItemIndex].ToString()))
                    ? long.Parse(dgUserInitiatedTransactions.DataKeys[e.Item.ItemIndex].ToString())
                    : 0;

                if (id < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid Record Selection!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var transaction =
                    ServiceProvider.Instance().GetExpenseTransactionServices().GetExpenseTransaction(id);

                if (transaction == null || transaction.ExpenseTransactionId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid record selection.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }
                
                if (e.CommandName == "viewComment")
                {
                    if (transaction.Status == 0)
                    {
                        ConfirmAlertBox1.ShowMessage("Transaction is still pending.",
                            ConfirmAlertBox.PopupMessageType.Error);
                        return;
                    }

                    txtRejComment.Text = string.Empty;
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
        protected void BtnGenerateReportchClick(object sender, EventArgs e)
        {
            if (! ValidateControls())
            {
                ConfirmAlertBox1.ShowMessage("No Record Found!", ConfirmAlertBox.PopupMessageType.Error);
                dgUserInitiatedTransactions.DataSource = new List<ExpenseTransaction>();
                dgUserInitiatedTransactions.DataBind();
                return; 
            }
            
            var collection = GetTransactionsByDate();
            if (collection == null || !collection.Any())
            {
                ConfirmAlertBox1.ShowMessage("No Record Found!", ConfirmAlertBox.PopupMessageType.Error);
                dgUserInitiatedTransactions.DataSource = new List<ExpenseTransaction>();
                dgUserInitiatedTransactions.DataBind();
                return;
            }

            dgUserInitiatedTransactions.DataSource = collection;
            dgUserInitiatedTransactions.DataBind();
            hDepartment.InnerText = ddlDepartment.SelectedItem.Text;
            SetTransactionsStyle();
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
        private void LoadFilterOptions()
        {
            var options = DataArray.ConvertEnumToArrayList(typeof(FilterOptions));
            ddlFilterOption.DataSource = options;
            ddlFilterOption.DataValueField = "ID";
            ddlFilterOption.DataTextField = "Name";
            ddlFilterOption.DataBind();
            ddlFilterOption.Items.Insert(0, new ListItem(" -- Select Status -- ", "-1"));
            ddlFilterOption.Items.Insert(1, new ListItem("All", "0"));
            ddlFilterOption.SelectedIndex = -1;
        }
        private void LoadYearOptions()
        {
            var yrOptions = new List<string>();
            for (long i = 2010; i < DateTime.Now.Year; i++)
            {
                var yr = i + 1;
                yrOptions.Add(yr.ToString(CultureInfo.InvariantCulture));
            }
            
            ddlYear.DataSource = yrOptions.OrderByDescending(m => m);
            ddlYear.DataBind();
            ddlYear.Items.Insert(0, new ListItem(" -- Select Year -- ", "0"));
            ddlYear.SelectedIndex = 0;
        }
        private void LoadTimeFrameOptions()
        {
            var options = DataArray.ConvertEnumToArrayList(typeof(TimeFrame));
            ddlPeriod.DataSource = options;
            ddlPeriod.DataValueField = "ID";
            ddlPeriod.DataTextField = "Name";
            ddlPeriod.DataBind();
            ddlPeriod.Items.Insert(0, new ListItem(" -- Select Time Frame -- ", "0"));
            ddlPeriod.SelectedIndex = 0;
        }
        private void LoadMonthOptions()
        {
            var options = DataArray.ConvertEnumToArrayList(typeof(MonthList));
            ddlMonth.DataSource = options;
            ddlMonth.DataValueField = "ID";
            ddlMonth.DataTextField = "Name";
            ddlMonth.DataBind();
            ddlMonth.Items.Insert(0, new ListItem(" -- Select Month -- ", "0"));
            ddlMonth.SelectedIndex = 0;
            
        }
        private void LoadDepartments()
        {
            try
            {
                var departmentList = ServiceProvider.Instance().GetDepartmentServices().GetActiveOrderedDepartments();

                if (departmentList == null || !departmentList.Any())
                {
                    ddlDepartment.DataSource = new List<Department>();
                    ddlDepartment.Items.Insert(0, new ListItem("--List is empty--", "0"));
                    ddlDepartment.SelectedIndex = 0;
                    return;
                }

                ddlDepartment.DataSource = departmentList;
                ddlDepartment.DataTextField = "Name";
                ddlDepartment.DataValueField = "DepartmentId";
                ddlDepartment.DataBind();
                ddlDepartment.Items.Insert(0, new ListItem("--Select Department--", "0"));
                ddlDepartment.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }

        }
        private void LoadWeekOptions()
        {
            var weeks = DataArray.ConvertEnumToArrayList(typeof(WeekOptions));
            ddlWeekly.DataSource = weeks;
            ddlWeekly.DataValueField = "ID";
            ddlWeekly.DataTextField = "Name";
            ddlWeekly.DataBind();
            ddlWeekly.Items.Insert(0, new ListItem("--Select a Week--", "0"));
            ddlWeekly.SelectedIndex = 0;
        }
        private IEnumerable<ExpenseTransaction> GetTransactionsByDate()
        {
            try
            {
                var status = int.Parse(ddlFilterOption.SelectedValue);
                
                if (status < 0)
                {
                    ConfirmAlertBox1.ShowMessage("Please select a Status!", ConfirmAlertBox.PopupMessageType.Error);
                    return null;
                }

                var deptmnt = int.Parse(ddlDepartment.SelectedValue);

                if (deptmnt < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Please select a Department!", ConfirmAlertBox.PopupMessageType.Error);
                    return null;
                }

                var period = int.Parse(ddlPeriod.SelectedValue);

                if (period < 1)
                {
                    return null;
                }

                List<ExpenseTransaction> transactionList;
               

                if (period == 1)
                {
                    if (int.Parse(ddlYear.SelectedValue) < 1)
                    {
                        ConfirmAlertBox1.ShowMessage("Please select a Year!", ConfirmAlertBox.PopupMessageType.Error);
                        return null;
                    }

                    var yrVal = ddlYear.SelectedItem.Text;
                    
                    if (int.Parse(ddlMonth.SelectedValue) < 1)
                    {
                        ConfirmAlertBox1.ShowMessage("Please select a Month!", ConfirmAlertBox.PopupMessageType.Error);
                        return null;
                    }
                    string monthVal;
                    var month = int.Parse(ddlMonth.SelectedValue);

                    if (month < 10)
                    {
                        monthVal = "0" + month.ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        monthVal = month.ToString(CultureInfo.InvariantCulture);
                    }
                    
                    transactionList =
                        ServiceProvider.Instance()
                            .GetExpenseTransactionServices()
                            .GetMonthlyTransactions(status, deptmnt, yrVal, monthVal);

                    if (transactionList == null || !transactionList.Any())
                    {
                        ConfirmAlertBox1.ShowMessage("No Record Found!", ConfirmAlertBox.PopupMessageType.Error);
                        return null;
                    }
                   
                    return transactionList;

                }

                if (period == 2)
                {
                    if (int.Parse(ddlYear.SelectedValue) < 1)
                    {
                        ConfirmAlertBox1.ShowMessage("Please select a Year!", ConfirmAlertBox.PopupMessageType.Error);
                        return null;
                    }

                    var yrVal = ddlYear.SelectedItem.Text;
                    
                    string monthVal;

                    if (int.Parse(ddlMonth.SelectedValue) < 1)
                    {
                        ConfirmAlertBox1.ShowMessage("Please select a Month!", ConfirmAlertBox.PopupMessageType.Error);
                        return null;
                    }

                    if (int.Parse(ddlWeekly.SelectedValue) < 1)
                    {
                        ConfirmAlertBox1.ShowMessage("Please select a Week!", ConfirmAlertBox.PopupMessageType.Error);
                        return null;
                    }
                    var month = int.Parse(ddlMonth.SelectedValue);

                    if (month < 10)
                    {
                        monthVal = "0" + month.ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        monthVal = month.ToString(CultureInfo.InvariantCulture);
                    }

                    int weeklyVal = int.Parse(ddlWeekly.SelectedValue);

                    transactionList =
                        ServiceProvider.Instance()
                            .GetExpenseTransactionServices()
                            .GetWeeklyTransactions(status, deptmnt, yrVal, monthVal, weeklyVal);

                    if (transactionList == null || !transactionList.Any())
                    {
                        ConfirmAlertBox1.ShowMessage("No Record Found!", ConfirmAlertBox.PopupMessageType.Error);
                        return null;
                    }
                   
                    return transactionList;

                }
                
                if (period == 3)
                {
                    var startDateString = DateMap.ReverseToServerDate(txtStart.Text.Trim());
                    var startDate = DateTime.Parse(startDateString);
                    var endDateString = DateMap.ReverseToServerDate(txtEndDate.Text.Trim());
                    var endDate = DateTime.Parse(endDateString);
                    if (endDate < startDate || startDate > endDate)
                    {
                        ConfirmAlertBox1.ShowMessage("The 'From' date must not be LATER than the 'To' date.", ConfirmAlertBox.PopupMessageType.Error);
                        return null;
                    }

                    transactionList = ServiceProvider.Instance().GetExpenseTransactionServices().GetTransactionsByDateRange(startDateString, endDateString, deptmnt, status);

                    if (!transactionList.Any())
                    {
                        ConfirmAlertBox1.ShowMessage("No record found!", ConfirmAlertBox.PopupMessageType.Error);
                        return new List<ExpenseTransaction>();
                    }

                    return transactionList;
                }

                return new List<ExpenseTransaction>();

            }
            catch (Exception ex)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again or contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }

        }
        private bool ValidateControls()
        {
            try
            {
                var period = int.Parse(ddlPeriod.SelectedValue);
                
                if (period < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Please Select a Period.", ConfirmAlertBox.PopupMessageType.Error);
                    return false; 
                }

                if (period == 3)
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

                    if (!DateMap.IsEarlyDate(DateMap.ReverseToServerDate(txtStart.Text.Trim()), DateMap.ReverseToServerDate(txtEndDate.Text.Trim())))
                    {
                        ConfirmAlertBox1.ShowMessage("The 'From' date should not be LATER than the 'To' date.", ConfirmAlertBox.PopupMessageType.Error);
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
        #region Events
        #endregion

        #region Helpers
        //private void LoadTransactionItems(ExpenseTransaction transaction)
        //{
        //    try
        //    {
        //        var transactionItems = ServiceProvider.Instance().GetTransactionItemServices().GetTransactionItemsByExpenseTransaction(transaction.ExpenseTransactionId);
                
        //        var nestedItem =
        //            ServiceProvider.Instance()
        //                .GetTransactionItemServices()
        //                .GetItemsByDateRange(transaction.ExpenseTransactionId);

        //        if (transactionItems == null || !transactionItems.Any())
        //        {
        //            ConfirmAlertBox1.ShowMessage("Transaction Items could not be retrieved.", ConfirmAlertBox.PopupMessageType.Error);
        //            return;
        //        }

        //        if (string.IsNullOrEmpty(lblRequestedAmmount.InnerText))
        //        {
        //            lblRequestedAmmount.InnerText = "N" + NumberMap.GroupToDigits(transaction.TotalTransactionAmount.ToString(CultureInfo.InvariantCulture));
        //        }

        //        if (transaction.TotalApprovedAmount > 0)
        //        {
        //            lblApprovedTotalAmount.InnerText = "N" + NumberMap.GroupToDigits(transaction.TotalApprovedAmount.ToString(CultureInfo.InvariantCulture));
        //        }
        //        else
        //        {
        //            lblApprovedTotalAmount.InnerText = NumberMap.GroupToDigits(0.ToString(CultureInfo.InvariantCulture));
        //        }

        //        lblTransactionTitle.Text = transaction.ExpenseTitle;
        //        dgExpenseItem.DataSource = transactionItems.ElementAtOrDefault(0).Key.ToList();
        //        dgExpenseItem.DataBind();
        //        LoadTransactionsFooter();
        //        dvTransactionItems.Visible = true;
        //        dvReports.Visible = false;
                
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
        //        ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                
        //    }
        //}
        //private void LoadTransactionsFooter()
        //{
        //    try
        //    {
        //        if (dgExpenseItem.Items.Count > 0)
        //        {
        //            int subTotalQuantity = 0;
        //            int subtotalApprovedQuantity = 0;
        //            double subTotalUnitPrice = 0;
        //            double subTotalApprovedPrice = 0;

        //            for (var i = 0; i < dgExpenseItem.Items.Count; i++)
        //            {
        //                subTotalQuantity += DataCheck.IsNumeric(((Label)dgExpenseItem.Items[i].FindControl("lblQuantity")).Text)
        //                              ? int.Parse(((Label)dgExpenseItem.Items[i].FindControl("lblQuantity")).Text) : 0;
        //                subtotalApprovedQuantity += DataCheck.IsNumeric(((Label)dgExpenseItem.Items[i].FindControl("lblApprovedQuantity")).Text)
        //                              ? int.Parse(((Label)dgExpenseItem.Items[i].FindControl("lblApprovedQuantity")).Text) : 0;
        //                subTotalUnitPrice += DataCheck.IsNumeric(((Label)dgExpenseItem.Items[i].FindControl("lblUnitPrice")).Text)
        //                              ? double.Parse(((Label)dgExpenseItem.Items[i].FindControl("lblUnitPrice")).Text) : 0;
        //                subTotalApprovedPrice += DataCheck.IsNumeric(((Label)dgExpenseItem.Items[i].FindControl("lblApprovedUnitPrice")).Text)
        //                              ? double.Parse(((Label)dgExpenseItem.Items[i].FindControl("lblApprovedUnitPrice")).Text) : 0;
        //            }

        //            foreach (var item in dgExpenseItem.Controls[0].Controls)
        //            {
        //                if (item.GetType() == typeof(DataGridItem))
        //                {
        //                    var itmType = ((DataGridItem)item).ItemType;
        //                    if (itmType == ListItemType.Footer)
        //                    {
        //                        if (((DataGridItem)item).FindControl("lblTotalQuantity") != null)
        //                        {
        //                            ((Label)((DataGridItem)item).FindControl("lblTotalQuantity")).Text = subTotalQuantity.ToString(CultureInfo.InvariantCulture);
        //                        }

        //                        if (((DataGridItem)item).FindControl("lblTotalApprovedQuantity") != null)
        //                        {
        //                            ((Label)((DataGridItem)item).FindControl("lblTotalApprovedQuantity")).Text = subtotalApprovedQuantity.ToString(CultureInfo.InvariantCulture);
        //                        }

        //                        if (((DataGridItem)item).FindControl("lblTotalUnitPrice") != null)
        //                        {
        //                            ((Label)((DataGridItem)item).FindControl("lblTotalUnitPrice")).Text = "N" + NumberMap.GroupToDigits(subTotalUnitPrice.ToString(CultureInfo.InvariantCulture));
        //                        }

        //                        if (((DataGridItem)item).FindControl("lblTotalApprovedUnitPrice") != null)
        //                        {
        //                            ((Label)((DataGridItem)item).FindControl("lblTotalApprovedUnitPrice")).Text = "N" + NumberMap.GroupToDigits(subTotalApprovedPrice.ToString(CultureInfo.InvariantCulture));
        //                        }

        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
        //    }
        //}
        #endregion
        #endregion
       
    }
}