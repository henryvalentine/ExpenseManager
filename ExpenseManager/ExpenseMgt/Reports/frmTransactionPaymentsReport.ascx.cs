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
    public partial class FrmTransactionPaymentsReport : UserControl
    {
        protected int Offset = 5;
        protected int DataCount = 0;
        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Limit = 20;
                GetPageLimits();
                ddlLimit.SelectedValue = "20";
                dvView.Visible = false;
                dvAllContent.Visible = true;
                hTitle.InnerHtml = "Transaction Payments";
                LoadTimeFrameOptions();
                LoadFilterOptions();
                LoadYearOptions();
                LoadYearOptions();
                LoadMonthOptions();
                LoadDepartments();
                LoadWeekOptions();
                GetCurrentTransactionPayments();

            }
        }
        protected void DgAllTransactionPaymentsCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                dgAllTransactionPayments.SelectedIndex = e.Item.ItemIndex;

                long id = (DataCheck.IsNumeric(dgAllTransactionPayments.DataKeys[e.Item.ItemIndex].ToString())) ? long.Parse(dgAllTransactionPayments.DataKeys[e.Item.ItemIndex].ToString()) : 0;

                if (id < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid Record Selection!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var expenseTransactionPayment = ServiceProvider.Instance().GetExpenseTransactionPaymentServices().GetExpenseTransactionPayment(id);

                if (expenseTransactionPayment == null || expenseTransactionPayment.ExpenseTransactionId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid record selection.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }
                
                LoadTransactionPaymentHistory(expenseTransactionPayment.ExpenseTransactionId);
                //dvTransactionPayments.Visible = false;
                //dvTransactionPaymentHistory.Visible = true;
                dvView.Visible = true;
                dvAllContent.Visible = false;
            }

            catch
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }


        }
        protected void DgPaymentHistoryCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                dgPaymentHistory.SelectedIndex = e.Item.ItemIndex;

                long id = (DataCheck.IsNumeric(dgPaymentHistory.DataKeys[e.Item.ItemIndex].ToString())) ? long.Parse(dgPaymentHistory.DataKeys[e.Item.ItemIndex].ToString()) : 0;

                if (id < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid Record Selection!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var transactionHistory = ServiceProvider.Instance().GetExpenseTransactionPaymentHistoryServices().GetExpenseTransactionPaymentHistory(id);

                if (transactionHistory == null || transactionHistory.ExpenseTransactionPaymentHistoryId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid selection", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                if(transactionHistory.PaymentModeId == 1)
                {
                     trCashPayment.Visible = true;
                     trChequePayment.Visible = false;
                     var user = new PortalServiceManager().GetPortalUserById(transactionHistory.PaidById);
                     lblCashPaidBy.InnerText = user.FirstName + " " + user.LastName;
                    lblCashAmount.InnerText = "N" + NumberMap.GroupToDigits(transactionHistory.AmountPaid.ToString(CultureInfo.InvariantCulture));
                    lblCashDatePaid.InnerText = transactionHistory.PaymentDate;
                    lblCashTimePaid.InnerText = transactionHistory.PaymentTime;
                   

                }

                if(transactionHistory.PaymentModeId == 2)
                {
                     trCashPayment.Visible = false;
                     trChequePayment.Visible = true;
                     var user = new PortalServiceManager().GetPortalUserById(transactionHistory.PaidById);
                     lblChequePaidBy.InnerText = user.FirstName + " " + user.LastName;
                    lblChequeAmount.InnerText = "N" + NumberMap.GroupToDigits(transactionHistory.AmountPaid.ToString(CultureInfo.InvariantCulture));
                    lblChequeTimePaid.InnerText = transactionHistory.PaymentTime;
                    lblChequeDatePaid.InnerText = transactionHistory.PaymentDate;
                    var cheque = ServiceProvider.Instance().GetChequeServices().GetChequesByExpenseTransactionPaymentHistoryId(transactionHistory.ExpenseTransactionPaymentHistoryId);
                    if (cheque != null)
                    {
                        var defaultElement = cheque.ElementAtOrDefault(0);

                        if (defaultElement != null)
                        {
                            lblChequeNumber.InnerText = defaultElement.ChequeNo;
                            lblBank.InnerText = defaultElement.Bank.BankName;
                        }
                    }
                    

                }
               
                lgTransactionTitle.InnerHtml = transactionHistory.ExpenseTransaction.ExpenseTitle;
                txttHistoryComment.Value = transactionHistory.Comment;
                mpePaymentCommentPopup.PopupControlID = "dvTransactionComment";
                mpePaymentCommentPopup.CancelControlID = "btnCloseDetails";
                mpePaymentCommentPopup.Show();
                

            }
            catch
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        protected void BtnBackNavClick(object sender, EventArgs e)
        {
            hTitle.InnerHtml = "Transaction Payments";
            dvView.Visible = false;
            dvAllContent.Visible = true;
        }
        protected void BtnGeneratePaymentReportchClick(object sender, EventArgs e)
        {
            if (!ValidateControls())
            {
                ConfirmAlertBox1.ShowMessage("No Record Found!", ConfirmAlertBox.PopupMessageType.Error);
                dgAllTransactionPayments.DataSource = new List<ExpenseTransaction>();
                dgAllTransactionPayments.DataBind();
                return;
            }

            var collection = GetTransactionPaymentsByDate();
            if (collection == null || !collection.Any())
            {
                ConfirmAlertBox1.ShowMessage("No Record Found!", ConfirmAlertBox.PopupMessageType.Error);
                dgAllTransactionPayments.DataSource = new List<ExpenseTransaction>();
                dgAllTransactionPayments.DataBind();
                return;
            }
            dgAllTransactionPayments.DataSource = collection;
            dgAllTransactionPayments.DataBind();
            hDepartment.InnerText = ddlPaymentDepartment.SelectedItem.Text;
            LoadTransactionsFooter();
            SetTransactionStyle();
        }
        #endregion

        #region Page Event Helpers
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
        private void LoadTransactionPaymentFooter()
        {
            try
            {
                if (dgAllTransactionPayments.Items.Count > 0)
                {
                    double expAmountPayableTotal = 0;
                    double expAmountPaidTotal = 0;
                    double totalBalance = 0;
                    for (var i = 0; i < dgAllTransactionPayments.Items.Count; i++)
                    {
                        expAmountPayableTotal += DataCheck.IsNumeric(((Label)dgAllTransactionPayments.Items[i].FindControl("lblTotalAmountPaid")).Text)
                                      ? double.Parse(((Label)dgAllTransactionPayments.Items[i].FindControl("lblTotalAmountPaid")).Text)
                                      : 0;
                        expAmountPaidTotal += DataCheck.IsNumeric(((Label)dgAllTransactionPayments.Items[i].FindControl("lblAmountPaid")).Text)
                                     ? double.Parse(((Label)dgAllTransactionPayments.Items[i].FindControl("lblAmountPaid")).Text)
                                     : 0;
                        totalBalance += DataCheck.IsNumeric(((Label)dgAllTransactionPayments.Items[i].FindControl("lblBalance")).Text)
                                     ? double.Parse(((Label)dgAllTransactionPayments.Items[i].FindControl("lblBalance")).Text)
                                     : 0;

                    }

                    foreach (var item in dgAllTransactionPayments.Controls[0].Controls)
                    {
                        if (item.GetType() == typeof(DataGridItem))
                        {
                            var itmType = ((DataGridItem)item).ItemType;
                            if (itmType == ListItemType.Footer)
                            {
                                if (((DataGridItem)item).FindControl("lblTotalAmountPaidFooter") != null)
                                {
                                    ((Label)((DataGridItem)item).FindControl("lblTotalAmountPaidFooter")).Text = "N" + NumberMap.GroupToDigits(expAmountPayableTotal.ToString(CultureInfo.InvariantCulture));
                                }
                                if (((DataGridItem)item).FindControl("lblAmountPaidFooter") != null)
                                {
                                    ((Label)((DataGridItem)item).FindControl("lblAmountPaidFooter")).Text = "N" + NumberMap.GroupToDigits(expAmountPaidTotal.ToString(CultureInfo.InvariantCulture));
                                }
                                if (((DataGridItem)item).FindControl("lblBalanceFooter") != null)
                                {
                                    ((Label)((DataGridItem)item).FindControl("lblBalanceFooter")).Text = "N" + NumberMap.GroupToDigits(totalBalance.ToString(CultureInfo.InvariantCulture));
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        private void LoadTransactionPaymentHistory(long expenseTransactionId)
        {
            try
            {
                dgPaymentHistory.DataSource = new List<ExpenseTransactionPaymentHistory>();
                dgPaymentHistory.DataBind();

                var paymentsHistoryList = ServiceProvider.Instance().GetExpenseTransactionPaymentHistoryServices().GetExpenseTransactionPaymentHistoriesByExpenseTransactionId(expenseTransactionId);

                if (paymentsHistoryList == null || !paymentsHistoryList.Any())
                {
                    ConfirmAlertBox1.ShowMessage("There is no Payment log for the selected Transaction.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                dgPaymentHistory.DataSource = paymentsHistoryList;
                dgPaymentHistory.DataBind();
                LoadTransactionPaymentHistoryFooter();
                var transaction = paymentsHistoryList.ElementAtOrDefault(0);
                if (transaction != null)
                {
                    hTitle.InnerHtml = transaction.ExpenseTransaction.ExpenseTitle;
                    lblHistoryTitle.InnerHtml = transaction.ExpenseTransaction.ExpenseTitle;
                }

                dvView.Visible = true;
                dvAllContent.Visible = false;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        private void LoadTransactionPaymentHistoryFooter()
        {
            try
            {
                if (dgPaymentHistory.Items.Count > 0)
                {
                    double expAmountPayableTotal = 0;
                    for (var i = 0; i < dgPaymentHistory.Items.Count; i++)
                    {
                        expAmountPayableTotal += DataCheck.IsNumeric(((Label)dgPaymentHistory.Items[i].FindControl("lblHistoryAmountPaid")).Text)
                                      ? double.Parse(((Label)dgPaymentHistory.Items[i].FindControl("lblHistoryAmountPaid")).Text)
                                      : 0;

                    }

                    foreach (var item in dgPaymentHistory.Controls[0].Controls)
                    {
                        if (item.GetType() == typeof(DataGridItem))
                        {
                            var itmType = ((DataGridItem)item).ItemType;
                            if (itmType == ListItemType.Footer)
                            {
                                if (((DataGridItem)item).FindControl("lblHistoryAmountPaidFooter") != null)
                                {
                                    ((Label)((DataGridItem)item).FindControl("lblHistoryAmountPaidFooter")).Text = "N" + NumberMap.GroupToDigits(expAmountPayableTotal.ToString(CultureInfo.InvariantCulture));
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        private void LoadFilterOptions()
        {
            var options = DataArray.ConvertEnumToArrayList(typeof(ApprovedVoidedFilterOptions));
            ddlPaymentFilterOption.DataSource = options;
            ddlPaymentFilterOption.DataValueField = "ID";
            ddlPaymentFilterOption.DataTextField = "Name";
            ddlPaymentFilterOption.DataBind();
            ddlPaymentFilterOption.Items.Insert(0, new ListItem(" -- Select Status -- ", "-1"));
            ddlPaymentFilterOption.Items.Insert(1, new ListItem("All", "0"));
            ddlPaymentFilterOption.SelectedIndex = -1;
        }
        private void LoadYearOptions()
        {
            var yrOptions = new List<string>();
            for (long i = 2010; i < DateTime.Now.Year; i++)
            {
                var yr = i + 1;
                yrOptions.Add(yr.ToString(CultureInfo.InvariantCulture));
            }

            ddlPaymentYear.DataSource = yrOptions.OrderByDescending(m => m);
            ddlPaymentYear.DataBind();
            ddlPaymentYear.Items.Insert(0, new ListItem(" -- Select Year -- ", "0"));
            ddlPaymentYear.SelectedIndex = 0;
        }
        private void LoadTimeFrameOptions()
        {
            var options = DataArray.ConvertEnumToArrayList(typeof(TimeFrame));
            ddlPaymentPeriod.DataSource = options;
            ddlPaymentPeriod.DataValueField = "ID";
            ddlPaymentPeriod.DataTextField = "Name";
            ddlPaymentPeriod.DataBind();
            ddlPaymentPeriod.Items.Insert(0, new ListItem(" -- Select Time Frame -- ", "0"));
            ddlPaymentPeriod.SelectedIndex = 0;
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
                    ddlPaymentDepartment.DataSource = new List<Department>();
                    ddlPaymentDepartment.Items.Insert(0, new ListItem("--List is empty--", "0"));
                    ddlPaymentDepartment.SelectedIndex = 0;
                    return;
                }

                ddlPaymentDepartment.DataSource = departmentList;
                ddlPaymentDepartment.DataTextField = "Name";
                ddlPaymentDepartment.DataValueField = "DepartmentId";
                ddlPaymentDepartment.DataBind();
                ddlPaymentDepartment.Items.Insert(0, new ListItem("--Select Department--", "0"));
                ddlPaymentDepartment.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }

        }
        private void LoadWeekOptions()
        {
            var weeks = DataArray.ConvertEnumToArrayList(typeof(WeekOptions));
            ddlPaymentWeekly.DataSource = weeks;
            ddlPaymentWeekly.DataValueField = "ID";
            ddlPaymentWeekly.DataTextField = "Name";
            ddlPaymentWeekly.DataBind();
            ddlPaymentWeekly.Items.Insert(0, new ListItem("--Select a Week--", "0"));
            ddlPaymentWeekly.SelectedIndex = 0;
        }
        private IEnumerable<ExpenseTransactionPayment> GetTransactionPaymentsByDate()
        {
            try
            {
                var status = int.Parse(ddlPaymentFilterOption.SelectedValue);

                if (status < 0)
                {
                    ConfirmAlertBox1.ShowMessage("Please select a Status!", ConfirmAlertBox.PopupMessageType.Error);
                    return null;
                }

                var deptmnt = int.Parse(ddlPaymentDepartment.SelectedValue);

                if (deptmnt < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Please select a Department!", ConfirmAlertBox.PopupMessageType.Error);
                    return null;
                }

                var period = int.Parse(ddlPaymentPeriod.SelectedValue);

                if (period < 1)
                {
                    return null;
                }

                List<ExpenseTransactionPayment> transactionList;


                if (period == 1)
                {
                    if (int.Parse(ddlPaymentYear.SelectedValue) < 1)
                    {
                        ConfirmAlertBox1.ShowMessage("Please select a Year!", ConfirmAlertBox.PopupMessageType.Error);
                        return null;
                    }

                    var yrVal = ddlPaymentYear.SelectedItem.Text;

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
                            .GetExpenseTransactionPaymentServices()
                            .GetMonthlyTransactionPayments(status, deptmnt, yrVal, monthVal);

                    if (transactionList == null || !transactionList.Any())
                    {
                        ConfirmAlertBox1.ShowMessage("No Record Found!", ConfirmAlertBox.PopupMessageType.Error);
                        return null;
                    }

                    return transactionList;

                }

                if (period == 2)
                {
                    if (int.Parse(ddlPaymentYear.SelectedValue) < 1)
                    {
                        ConfirmAlertBox1.ShowMessage("Please select a Year!", ConfirmAlertBox.PopupMessageType.Error);
                        return null;
                    }

                    var yrVal = ddlPaymentYear.SelectedItem.Text;

                    string monthVal;

                    if (int.Parse(ddlMonth.SelectedValue) < 1)
                    {
                        ConfirmAlertBox1.ShowMessage("Please select a Month!", ConfirmAlertBox.PopupMessageType.Error);
                        return null;
                    }

                    if (int.Parse(ddlPaymentWeekly.SelectedValue) < 1)
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

                    int weeklyVal = int.Parse(ddlPaymentWeekly.SelectedValue);

                    transactionList =
                        ServiceProvider.Instance()
                            .GetExpenseTransactionPaymentServices()
                            .GetWeeklyTransactionPayments(status, deptmnt, yrVal, monthVal, weeklyVal);

                    if (transactionList == null || !transactionList.Any())
                    {
                        ConfirmAlertBox1.ShowMessage("No Record Found!", ConfirmAlertBox.PopupMessageType.Error);
                        return null;
                    }

                    return transactionList;

                }

                if (period == 3)
                {
                    var startDateString = DateMap.ReverseToServerDate(txtPaymentStart.Text.Trim());
                    var startDate = DateTime.Parse(startDateString);
                    var endDateString = DateMap.ReverseToServerDate(txtPaymentEndDate.Text.Trim());
                    var endDate = DateTime.Parse(endDateString);
                    if (endDate < startDate || startDate > endDate)
                    {
                        ConfirmAlertBox1.ShowMessage("The 'From' date must not be LATER than the 'To' date.", ConfirmAlertBox.PopupMessageType.Error);
                        return null;
                    }

                    transactionList = ServiceProvider.Instance().GetExpenseTransactionPaymentServices().GetTransactionPaymentsByDateRange(startDateString, endDateString, deptmnt, status);

                    if (!transactionList.Any())
                    {
                        ConfirmAlertBox1.ShowMessage("No record found!", ConfirmAlertBox.PopupMessageType.Error);
                        return new List<ExpenseTransactionPayment>();
                    }

                    return transactionList;
                }

                return new List<ExpenseTransactionPayment>();

            }
            catch (Exception ex)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again or contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransactionPayment>();
            }

        }
        private bool ValidateControls()
        {
            try
            {
                var period = int.Parse(ddlPaymentPeriod.SelectedValue);

                if (period < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Please Select a Period.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                if (period == 3)
                {
                    if (string.IsNullOrEmpty(txtPaymentStart.Text.Trim()) && string.IsNullOrEmpty(txtPaymentEndDate.Text.Trim()))
                    {
                        ConfirmAlertBox1.ShowMessage("Please enter a date range.", ConfirmAlertBox.PopupMessageType.Error);
                        return false;
                    }

                    if (string.IsNullOrEmpty(txtPaymentStart.Text.Trim()) || string.IsNullOrEmpty(txtPaymentStart.Text.Trim()))
                    {
                        ConfirmAlertBox1.ShowMessage("Please enter a date range.", ConfirmAlertBox.PopupMessageType.Error);
                        return false;
                    }

                    if (!string.IsNullOrEmpty(txtPaymentStart.Text.Trim()))
                    {
                        if (!DateMap.IsDate(DateMap.ReverseToServerDate(txtPaymentStart.Text.Trim())))
                        {
                            ConfirmAlertBox1.ShowMessage("Please enter a valid date.", ConfirmAlertBox.PopupMessageType.Error);
                            return false;
                        }
                    }


                    if (!string.IsNullOrEmpty(DateMap.ReverseToServerDate(txtPaymentEndDate.Text.Trim())))
                    {
                        if (!DateMap.IsDate(DateMap.ReverseToServerDate(txtPaymentEndDate.Text.Trim())))
                        {
                            ConfirmAlertBox1.ShowMessage("Please enter a valid date.", ConfirmAlertBox.PopupMessageType.Error);
                            return false;
                        }
                    }

                    if (!DateMap.IsEarlyDate(DateMap.ReverseToServerDate(txtPaymentStart.Text.Trim()), DateMap.ReverseToServerDate(txtPaymentEndDate.Text.Trim())))
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
        private void GetCurrentTransactionPayments()
        {
            try
            {
                dgAllTransactionPayments.DataSource = new List<ExpenseTransactionPayment>();
                dgAllTransactionPayments.DataBind();

                var transactionPaymentsByDate = ServiceProvider.Instance().GetExpenseTransactionPaymentServices().GetCurrentTransactionPayments(DateMap.GetLocalDate());

                if (!transactionPaymentsByDate.Any())
                {
                    return;
                }

                Session["_expTransactionPaymentReportList"] = null;
                Session["_expTransactionPaymentReportList"] = transactionPaymentsByDate;
               
                Limit = int.Parse(ddlLimit.SelectedValue);
                FillRepeater<ExpenseTransactionPayment>(dgAllTransactionPayments, "_expTransactionPaymentReportList", Navigation.None, Limit, LoadMethod);
                LoadTransactionPaymentFooter();
                SetTransactionStyle();
            
            }
            catch (Exception ex)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            
            }

        }
        private void SetTransactionStyle()
        {
            try
            {
                if (dgAllTransactionPayments.Items.Count > 0)
                {
                    for (var i = 0; i < dgAllTransactionPayments.Items.Count; i++)
                    {
                        var approvedLabel = ((Label)dgAllTransactionPayments.Items[i].FindControl("lblTransactionStatus"));
                        var linkViewHistory = ((LinkButton)dgAllTransactionPayments.Items[i].FindControl("lblViewHistory"));
                        if (approvedLabel.Text == "Completed")
                        {
                            approvedLabel.Style.Add("color", "darkcyan");
                            approvedLabel.Style.Add("font-weight", "bold");
                        }

                        if (approvedLabel.Text == "Uncompleted")
                        {
                            approvedLabel.Style.Add("color", "maroon");
                            approvedLabel.Style.Add("font-weight", "bold");
                        }

                        linkViewHistory.Style.Add("color", "rgba(35, 6, 35, 0.65)");
                    }

                }
            }
            catch
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        private void LoadTransactionsFooter()
        {
            try
            {
                if (dgAllTransactionPayments.Items.Count > 0)
                {
                    double totalBalance = 0;
                    double totalAmountPaid = 0;
                    double totalAmountPayable = 0;
                   // lblTotalAmountPaid lblAmountPaid  lblBalance  lblTotalAmountPaidFooter lblAmountPaidFooter lblBalanceFooter
                    for (var i = 0; i < dgAllTransactionPayments.Items.Count; i++)
                    {
                        totalBalance += DataCheck.IsNumeric(((Label)dgAllTransactionPayments.Items[i].FindControl("lblBalance")).Text)
                                      ? double.Parse(((Label)dgAllTransactionPayments.Items[i].FindControl("lblBalance")).Text.Replace(",",string.Empty)) : 0;
                        totalAmountPaid += DataCheck.IsNumeric(((Label)dgAllTransactionPayments.Items[i].FindControl("lblAmountPaid")).Text)
                                      ? double.Parse(((Label)dgAllTransactionPayments.Items[i].FindControl("lblAmountPaid")).Text.Replace(",", string.Empty)) : 0;
                        totalAmountPayable += DataCheck.IsNumeric(((Label)dgAllTransactionPayments.Items[i].FindControl("lblTotalAmountPaid")).Text)
                                      ? double.Parse(((Label)dgAllTransactionPayments.Items[i].FindControl("lblTotalAmountPaid")).Text.Replace(",", string.Empty)) : 0;
                    }

                    foreach (var item in dgAllTransactionPayments.Controls[0].Controls)
                    {
                        if (item.GetType() == typeof(DataGridItem))
                        {
                            var itmType = ((DataGridItem)item).ItemType;
                            if (itmType == ListItemType.Footer)
                            {
                                if (((DataGridItem)item).FindControl("lblBalanceFooter") != null)
                                {
                                    ((Label)((DataGridItem)item).FindControl("lblBalanceFooter")).Text = "N" + NumberMap.GroupToDigits(totalBalance.ToString(CultureInfo.InvariantCulture));
                                }

                                if (((DataGridItem)item).FindControl("lblAmountPaidFooter") != null)
                                {
                                    ((Label)((DataGridItem)item).FindControl("lblAmountPaidFooter")).Text = "N" + NumberMap.GroupToDigits(totalAmountPaid.ToString(CultureInfo.InvariantCulture));
                                }

                                if (((DataGridItem)item).FindControl("lblTotalAmountPaidFooter") != null)
                                {
                                    ((Label)((DataGridItem)item).FindControl("lblTotalAmountPaidFooter")).Text = "N" + NumberMap.GroupToDigits(totalAmountPayable.ToString(CultureInfo.InvariantCulture));
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
        #endregion

        #region Pagination
        protected void FirstLink(object sender, EventArgs e)
        {
            try
            {
                var senderLinkArgument = int.Parse(((LinkButton)sender).CommandArgument);
                NowViewing = senderLinkArgument;
                FillRepeater<ExpenseTransactionPayment>(dgAllTransactionPayments, "_expTransactionPaymentReportList", Navigation.Sorting, Limit, LoadMethod);
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
                if (sesslist != null)
                {
                    DataCount = sesslist.Count;
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
                        //lblCurrentPage1.Text = "Page : " + (NowViewing + 1).ToString(CultureInfo.InvariantCulture) + " of " + objPds.PageCount.ToString(CultureInfo.InvariantCulture);
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
                        //SetTransactionStyle();
                        LoadTransactionsFooter();
                    
                    }
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
                FillRepeater<ExpenseTransactionPayment>(dgAllTransactionPayments, "_expTransactionPaymentReportList", Navigation.Next, Limit, LoadMethod);
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
                FillRepeater<ExpenseTransactionPayment>(dgAllTransactionPayments, "_expTransactionPaymentReportList", Navigation.First, Limit, LoadMethod);
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
                FillRepeater<ExpenseTransactionPayment>(dgAllTransactionPayments, "_expTransactionPaymentReportList", Navigation.Last, Limit, LoadMethod);
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
                FillRepeater<ExpenseTransactionPayment>(dgAllTransactionPayments, "_expTransactionPaymentReportList", Navigation.Previous, Limit, LoadMethod);
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
                FillRepeater<ExpenseTransactionPayment>(dgAllTransactionPayments, "_expTransactionPaymentReportList", Navigation.None, Limit, LoadMethod);
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
        /* Method to hold all content from the Db*/
        protected bool LoadMethod()
        {
           if(!string.IsNullOrEmpty(txtPaymentStart.Text.Trim()) && !string.IsNullOrEmpty(txtPaymentEndDate.Text.Trim()) && int.Parse(ddlPaymentFilterOption.SelectedValue) > -1)
            {
                
            }

            if (string.IsNullOrEmpty(txtPaymentStart.Text.Trim()) && string.IsNullOrEmpty(txtPaymentEndDate.Text.Trim()))
            {
                GetCurrentTransactionPayments();
            }
            return true;
        }
        #endregion
        #endregion
    }
}