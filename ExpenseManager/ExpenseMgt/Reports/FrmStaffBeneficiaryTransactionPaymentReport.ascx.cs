using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExpenseManager.CoreFramework;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;
using xPlug.BusinessService;

namespace ExpenseManager.ExpenseMgt.Reports
{
    public partial class FrmStaffBeneficiaryTransactionPaymentReport : UserControl
    {
        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session["_paymentHistoryList"] = null;
                Session["_expensePaymentsList"] = null;
                dvTransactionPayments.Visible = true;
                btnRefresh.CommandArgument = "0";
                dvTransactionPaymentHistory.Visible = false;
                hTitle.InnerHtml = "Transaction Payments for Staff Beneficiaries";
                lblFilterReport.InnerHtml = "All Transaction Payments";
                BindGridWithDefaultList();

            }
        }
        protected void DgAllTransactionPaymentsCommand(object source, DataGridCommandEventArgs e)
        {
            ErrorDisplay1.ClearError();
            try
            {
                if (Session["_expensePaymentsList"] == null)
                {
                    ErrorDisplay1.ShowError("Expense Transaction Payment list is empty or session has expired.");
                    return;
                }

                var expenseTransactionPaymentList = Session["_expensePaymentsList"] as List<StaffExpenseTransactionPayment>;

                if (expenseTransactionPaymentList == null || !expenseTransactionPaymentList.Any())
                {
                    ErrorDisplay1.ShowError("Expense Transaction Payment list is empty or session has expired.");
                    return;
                }

                dgAllTransactionPayments.SelectedIndex = e.Item.ItemIndex;

                long id = (DataCheck.IsNumeric(dgAllTransactionPayments.DataKeys[e.Item.ItemIndex].ToString())) ? long.Parse(dgAllTransactionPayments.DataKeys[e.Item.ItemIndex].ToString()) : 0;

                if (id < 1)
                {
                    ErrorDisplay1.ShowError("Invalid Record Selection!");
                    return;
                }

                var expenseTransactionPayment = expenseTransactionPaymentList.Find(m => m.StaffExpenseTransactionPaymentId == id);

                if (expenseTransactionPayment == null || expenseTransactionPayment.StaffExpenseTransactionId < 1)
                {
                    ErrorDisplay1.ShowError("Invalid record selection.");
                    return;
                }

                if (e.CommandName == "ViewHistory")
                {
                    LoadTransactionPaymentHistory(expenseTransactionPayment.StaffExpenseTransactionId);
                    dvTransactionPayments.Visible = false;
                    dvTransactionPaymentHistory.Visible = true;
                }

            }

            catch
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
            }


        }
        protected void DgPaymentHistoryCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                if (Session["_paymentHistoryList"] == null)
                {
                    ErrorDisplay1.ShowError("Transaction Payment History is empty or session has expired.");
                    return;
                }

                var expenseTransactionPaymentHistoryList = Session["_paymentHistoryList"] as List<StaffExpenseTransactionPaymentHistory>;

                if (expenseTransactionPaymentHistoryList == null || !expenseTransactionPaymentHistoryList.Any())
                {
                    ErrorDisplay1.ShowError("Transaction Payment History is empty or session has expired.");
                    return;
                }

                dgPaymentHistory.SelectedIndex = e.Item.ItemIndex;

                long id = (DataCheck.IsNumeric(dgPaymentHistory.DataKeys[e.Item.ItemIndex].ToString())) ? long.Parse(dgPaymentHistory.DataKeys[e.Item.ItemIndex].ToString()) : 0;

                if (id < 1)
                {
                    ErrorDisplay1.ShowError("Invalid Record Selection!");
                    return;
                }

                var transactionHistory =
                    expenseTransactionPaymentHistoryList.Find(m => m.StaffExpenseTransactionPaymentHistoryId == id);

                if (transactionHistory == null || transactionHistory.StaffExpenseTransactionPaymentHistoryId < 1)
                {
                    ErrorDisplay1.ShowError("Invalid selection");
                    return;
                }

                if (transactionHistory.Comment == null)
                {
                    ErrorDisplay1.ShowError("There is no comment for the selected transaction");
                }

                else
                {
                    lgTransactionTitle.InnerHtml = transactionHistory.StaffExpenseTransaction.ExpenseTitle;
                    txtHistoryComment.Text = transactionHistory.Comment;
                    mpePaymentCommentPopup.PopupControlID = dvTransactionComment.ID;
                    mpePaymentCommentPopup.CancelControlID = btnCloseComment.ID;
                    mpePaymentCommentPopup.Show();
                }

            }
            catch
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
            }
        }
        protected void BtnBackNavClick(object sender, EventArgs e)
        {
            hTitle.InnerHtml = "Transaction Payments";
            dvTransactionPaymentHistory.Visible = false;
            dvTransactionPayments.Visible = true;
        }
        protected void LnkUncompletedClick(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            txtEndDate.Text = string.Empty;
            txtStart.Text = string.Empty;
            btnRefresh.CommandArgument = "2";
            if (!GetUncompletedPayments())
            {
                lblFilterReport.InnerHtml = "No record found";
                return;
            }
            LoadTransactionPaymentFooter();
        }
        protected void LnkCompletedClick(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            btnRefresh.CommandArgument = "1";
            txtEndDate.Text = string.Empty;
            txtStart.Text = string.Empty;
            if (!GetcompletedPayments())
            {
                lblFilterReport.InnerHtml = "No record found";
                return;
            }

            LoadTransactionPaymentFooter();
        }
        protected void BtnRefreshClick(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            txtEndDate.Text = string.Empty;
            txtStart.Text = string.Empty;
            if (!LoadAllTransactionPayments())
            {
                return;
            }
            LoadTransactionPaymentFooter();
            lblFilterReport.InnerHtml = "All Transaction Payments";
        }
        protected void BtnDateFilterClick(object sender, EventArgs e)
        {
            if (!ValidateControls())
            {
                dgAllTransactionPayments.DataSource = new List<StaffExpenseTransactionPayment>();
                dgAllTransactionPayments.DataBind();
                lblFilterReport.InnerHtml = "No record found";
                return;
            }

            if (!GetTransactionPaymentsByDate())
            {
                dgAllTransactionPayments.DataSource = new List<StaffExpenseTransactionPayment>();
                dgAllTransactionPayments.DataBind();
                lblFilterReport.InnerHtml = "No record found";
                return;
            }
            LoadTransactionPaymentFooter();
        }
        #endregion

        #region Page Event Helpers
        private void BindGridWithDefaultList()
        {
            dgAllTransactionPayments.DataSource = new List<StaffExpenseTransactionPayment>();
            dgAllTransactionPayments.DataBind();
        }
        private bool GetUncompletedPayments()
        {
            try
            {
                var uncompletedList = ServiceProvider.Instance().GetStaffExpenseTransactionPaymentServices().GetStaffUnCompletedExpenseTransactionPayments();

                if (!uncompletedList.Any())
                {
                    ErrorDisplay1.ShowError("Uncompleted Payment list is empty.");
                    dgAllTransactionPayments.DataSource = new List<StaffExpenseTransactionPayment>();
                    dgAllTransactionPayments.DataBind();
                    return false;
                }
                var itemCount = uncompletedList.Count;
                lblFilterReport.InnerHtml = itemCount + " " + " Uncompleted Payment(s) found!";
                dgAllTransactionPayments.DataSource = uncompletedList;
                dgAllTransactionPayments.DataBind();
                Session["_uncompletedPaymentsList"] = uncompletedList;
                Session["_expensePaymentsList"] = null;
                Session["_expensePaymentsList"] = uncompletedList;
                LoadTransactionPaymentFooter();
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
        private bool GetcompletedPayments()
        {
            try
            {
                var completedList = ServiceProvider.Instance().GetStaffExpenseTransactionPaymentServices().GetStaffCompletedExpenseTransactionPayments();

                if (!completedList.Any())
                {
                    ErrorDisplay1.ShowError("Completed payment list is empty.");
                    lblFilterReport.InnerHtml = "No record found";
                    dgAllTransactionPayments.DataSource = new List<StaffExpenseTransactionPayment>();
                    dgAllTransactionPayments.DataBind();
                    return false;
                }

                var itemCount = completedList.Count;
                lblFilterReport.InnerHtml = itemCount + " " + " Completed Payment(s) found!";

                dgAllTransactionPayments.DataSource = completedList;
                dgAllTransactionPayments.DataBind();
                Session["_completedPaymentsList"] = completedList;
                Session["_expensePaymentsList"] = null;
                Session["_expensePaymentsList"] = completedList;
                LoadTransactionPaymentFooter();
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
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
            }
        }
        private void LoadTransactionPaymentHistory(long expenseTransactionId)
        {
            try
            {

                dgPaymentHistory.DataSource = new List<StaffExpenseTransactionPaymentHistory>();
                dgPaymentHistory.DataBind();
                dvTransactionPayments.Visible = false;
                dvTransactionPaymentHistory.Visible = true;

                var paymentsHistoryList = ServiceProvider.Instance().GetStaffExpenseTransactionPaymentHistoryServices().GetStaffExpenseTransactionPaymentHistoriesByStaffExpenseTransactionId(expenseTransactionId);

                if (paymentsHistoryList == null)
                {
                    ErrorDisplay1.ShowError("There is no Payment log for the selected Transaction.");
                    return;
                }

                if (!paymentsHistoryList.Any())
                {
                    ErrorDisplay1.ShowError("There is no Payment log for the selected Transaction.");
                    return;
                }

                foreach (var transactionHistory in paymentsHistoryList)
                {
                    transactionHistory.PaidBy = (new PortalServiceManager().GetPortalUserById(transactionHistory.PaidById)).UserName;
                }

                dgPaymentHistory.DataSource = paymentsHistoryList;
                dgPaymentHistory.DataBind();
                LoadTransactionPaymentHistoryFooter();
                var transaction = paymentsHistoryList.ElementAt(0);
                hTitle.InnerHtml = transaction.StaffExpenseTransaction.ExpenseTitle;
                lblHistoryTitle.InnerHtml = transaction.StaffExpenseTransaction.ExpenseTitle;
                Session["_paymentHistoryList"] = paymentsHistoryList;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
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
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
            }
        }
        private bool GetTransactionPaymentsByDate()
        {

            ErrorDisplay1.ClearError();
            try
            {
                string date;
                var transactionPaymentsByDate = new List<StaffExpenseTransactionPayment>();
                dgAllTransactionPayments.DataSource = new List<StaffExpenseTransactionPayment>();
                dgAllTransactionPayments.DataBind();
                lblFilterReport.InnerHtml = "Data list is empty.";

                if (btnRefresh.CommandArgument == "2")
                {
                    if (Session["_uncompletedPaymentsList"] == null)
                    {
                        ErrorDisplay1.ShowError("Uncompleted Transaction Payment list is empty or session has expired.");
                        return false;
                    }

                    var uncompletedExpTransactionPayments = (List<StaffExpenseTransactionPayment>)Session["_uncompletedPaymentsList"];

                    if (!uncompletedExpTransactionPayments.Any())
                    {
                        ErrorDisplay1.ShowError("Uncompleted Transaction Payment list list is empty or session has expired.");
                        return false;
                    }

                    transactionPaymentsByDate = new List<StaffExpenseTransactionPayment>();

                    if (!string.IsNullOrEmpty(txtStart.Text.Trim()) && !string.IsNullOrEmpty(txtEndDate.Text.Trim()))
                    {
                        var startDateString = DateMap.ReverseToServerDate(txtStart.Text.Trim());
                        var startDate = DateTime.Parse(startDateString);
                        var endDateString = DateMap.ReverseToServerDate(txtEndDate.Text.Trim());
                        var endDate = DateTime.Parse(endDateString);

                        transactionPaymentsByDate = ServiceProvider.Instance().GetStaffExpenseTransactionPaymentServices().FilterStaffExpenseTransactionPaymentsByDateRange(uncompletedExpTransactionPayments, startDate, endDate);
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(txtEndDate.Text.Trim()) && string.IsNullOrEmpty(txtStart.Text.Trim()))
                        {
                            date = DateMap.ReverseToServerDate(txtEndDate.Text.Trim());
                            transactionPaymentsByDate = ServiceProvider.Instance().GetStaffExpenseTransactionPaymentServices().FilterStaffExpenseTransactionPaymentsByLastPaymentDate(uncompletedExpTransactionPayments, date);
                        }

                        if (string.IsNullOrEmpty(txtEndDate.Text.Trim()) && !string.IsNullOrEmpty(txtStart.Text.Trim()))
                        {
                            date = DateMap.ReverseToServerDate(txtStart.Text.Trim());
                            transactionPaymentsByDate = ServiceProvider.Instance().GetStaffExpenseTransactionPaymentServices().FilterStaffExpenseTransactionPaymentsByLastPaymentDate(uncompletedExpTransactionPayments, date);

                        }
                    }

                    if (!transactionPaymentsByDate.Any())
                    {
                        ErrorDisplay1.ShowError("No record found.");
                        return false;
                    }

                }

                if (btnRefresh.CommandArgument == "1")
                {

                    if (Session["_completedPaymentsList"] == null)
                    {
                        ErrorDisplay1.ShowError("Uncompleted Transaction Payment list list is empty or session has expired.");
                        return false;
                    }

                    var completedPaymentsList = (List<StaffExpenseTransactionPayment>)Session["_completedPaymentsList"];

                    if (!completedPaymentsList.Any())
                    {
                        ErrorDisplay1.ShowError("Completed Transaction Payment list is empty or session has expired.");
                        return false;
                    }

                    transactionPaymentsByDate = new List<StaffExpenseTransactionPayment>();

                    if (!string.IsNullOrEmpty(txtStart.Text.Trim()) && !string.IsNullOrEmpty(txtEndDate.Text.Trim()))
                    {
                        var startDateString = DateMap.ReverseToServerDate(txtStart.Text.Trim());
                        var startDate = DateTime.Parse(startDateString);
                        var endDateString = DateMap.ReverseToServerDate(txtEndDate.Text.Trim());
                        var endDate = DateTime.Parse(endDateString);
                        transactionPaymentsByDate = ServiceProvider.Instance().GetStaffExpenseTransactionPaymentServices().FilterStaffExpenseTransactionPaymentsByDateRange(completedPaymentsList, startDate, endDate);

                    }

                    else
                    {
                        if (string.IsNullOrEmpty(txtStart.Text.Trim()) && !string.IsNullOrEmpty(txtEndDate.Text.Trim()))
                        {
                            date = DateMap.ReverseToServerDate(txtEndDate.Text.Trim());
                            transactionPaymentsByDate = ServiceProvider.Instance().GetStaffExpenseTransactionPaymentServices().FilterStaffExpenseTransactionPaymentsByLastPaymentDate(completedPaymentsList, date);
                        }

                        if (!string.IsNullOrEmpty(txtStart.Text.Trim()) && string.IsNullOrEmpty(txtEndDate.Text.Trim()))
                        {
                            date = DateMap.ReverseToServerDate(txtStart.Text.Trim());
                            transactionPaymentsByDate = ServiceProvider.Instance().GetStaffExpenseTransactionPaymentServices().FilterStaffExpenseTransactionPaymentsByLastPaymentDate(completedPaymentsList, date);
                        }
                    }

                }

                if (btnRefresh.CommandArgument == "0")
                {

                    var allExpensePaymentsList = ServiceProvider.Instance().GetStaffExpenseTransactionPaymentServices().GetStaffExpenseTransactionPayments();

                    if (!allExpensePaymentsList.Any())
                    {
                        ErrorDisplay1.ShowError("Transaction Payment list is empty or session has expired.");
                        return false;
                    }


                    if (!string.IsNullOrEmpty(txtStart.Text.Trim()) && !string.IsNullOrEmpty(txtEndDate.Text.Trim()))
                    {
                        var startDateString = DateMap.ReverseToServerDate(txtStart.Text.Trim());
                        var startDate = DateTime.Parse(startDateString);
                        var endDateString = DateMap.ReverseToServerDate(txtEndDate.Text.Trim());
                        var endDate = DateTime.Parse(endDateString);

                        transactionPaymentsByDate = ServiceProvider.Instance().GetStaffExpenseTransactionPaymentServices().FilterStaffExpenseTransactionPaymentsByDateRange(startDate, endDate);
                    }

                    else
                    {
                        if (string.IsNullOrEmpty(txtStart.Text.Trim()) && !string.IsNullOrEmpty(txtEndDate.Text.Trim()))
                        {
                            date = DateMap.ReverseToServerDate(txtEndDate.Text.Trim());
                            transactionPaymentsByDate = ServiceProvider.Instance().GetStaffExpenseTransactionPaymentServices().FilterStaffExpenseTransactionPaymentsByLastPaymentDate(date);

                        }

                        if (!string.IsNullOrEmpty(txtStart.Text.Trim()) && string.IsNullOrEmpty(txtEndDate.Text.Trim()))
                        {
                            date = DateMap.ReverseToServerDate(txtStart.Text.Trim());
                            transactionPaymentsByDate = ServiceProvider.Instance().GetStaffExpenseTransactionPaymentServices().FilterStaffExpenseTransactionPaymentsByLastPaymentDate(date);
                        }
                    }

                }

                if (!transactionPaymentsByDate.Any())
                {
                    ErrorDisplay1.ShowError("No record found.");
                    return false;
                }

                dgAllTransactionPayments.DataSource = transactionPaymentsByDate;
                dgAllTransactionPayments.DataBind();
                SetApprovedTransactionStyle();
                SetApprovedTransactionStyle();
                Session["_expensePaymentsList"] = transactionPaymentsByDate;
                var itemCount = transactionPaymentsByDate.Count;
                lblFilterReport.InnerHtml = itemCount + " " + " item(s) found!";
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

                if (!string.IsNullOrEmpty(txtStart.Text.Trim()) && !string.IsNullOrEmpty(txtEndDate.Text.Trim()))
                {

                    if (!DateMap.IsEarlyDate(DateMap.ReverseToServerDate(txtStart.Text.Trim()), DateMap.ReverseToServerDate(txtEndDate.Text.Trim())))
                    {
                        ErrorDisplay1.ShowError("The 'From' date should not be LATER than the 'To' date.");
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
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
            }
        }
        private bool LoadAllTransactionPayments()
        {
            try
            {
                dgAllTransactionPayments.DataSource = new List<StaffExpenseTransactionPayment>();
                dgAllTransactionPayments.DataBind();

                var allTransactionsList = ServiceProvider.Instance().GetStaffExpenseTransactionPaymentServices().GetStaffOrderedExpenseTransactionPayments();

                if (!allTransactionsList.Any())
                {
                    ErrorDisplay1.ShowError("Transaction Payment list is empty.");
                    dgAllTransactionPayments.DataSource = new List<StaffExpenseTransactionPayment>();
                    dgAllTransactionPayments.DataBind();
                    return false;
                }

                dgAllTransactionPayments.DataSource = allTransactionsList;
                dgAllTransactionPayments.DataBind();
                LoadTransactionPaymentFooter();
                SetApprovedTransactionStyle();
                Session["_expensePaymentsList"] = null;
                Session["_expensePaymentsList"] = allTransactionsList;
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