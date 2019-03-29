using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExpenseManager.CoreFramework;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;
using xPlug.BusinessService;

namespace ExpenseManager.ExpenseMgt
{
    public partial class FrmCompleteStaffTransactionPayment : UserControl
    {
        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session["_expenseTransactionHistoryList"] = null;
                Session["_staffTransactionPaymentList"] = null;
                Session["_staffExpenseTransactions"] = null;
                Session["_staffExpenseTransactionPayment"] = null;
                Session["_approvedTransactionList"] = null;
                divBeneficiary.Visible = true;
                divPaymentTrack.Visible = false;
                LoadBeneficiaries();
            }

        }
        protected void BtnSubmitClick(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateTransactionPaymentControl())
                {
                    return;
                }

                var k = UpdateTransactionPayment();
                if (k < 1)
                {
                    ErrorDisplay1.ShowError("The Transaction Payment information could not be Updated.");
                    return;
                }

                if (!AddTransactionPaymentHistory(k))
                {
                    ErrorDisplay1.ShowError("The Transaction Payment information could not be Updated.");
                    return;
                }

                ErrorDisplay1.ShowSuccess("The Transaction Payment information was updated successfully.");

                if (!LoadTransactionPayments())
                {
                }

            }

            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
            }

        }
        protected void DdlBeneficiariesIndexChanged(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            try
            {
                if (int.Parse(ddlBeneficiaries.SelectedValue) < 1)
                {
                    ErrorDisplay1.ShowError("Please select a StaffBeneficiary");
                    return;
                }

                if (!LoadTransactionPayments())
                {
                    return;
                }

               // LoadExpenseTransactions();
                divBeneficiary.Visible = false;
                divPaymentTrack.Visible = true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
            }
        }
        protected void DgBeneficiaryPaymentTrackCommand(object source, DataGridCommandEventArgs e)
        {

            try
            {
                ClearControls();
                
                if (Session["_staffTransactionPaymentList"] == null)
                {
                    ErrorDisplay1.ShowError("Expense Transaction Payment list is empty or session has expired.");
                    return;
                }

                var expenseTransactionPaymentList = Session["_staffTransactionPaymentList"] as List<StaffExpenseTransactionPayment>;

                if (expenseTransactionPaymentList == null || !expenseTransactionPaymentList.Any())
                {
                    ErrorDisplay1.ShowError("Expense Transaction list is empty or session has expired.");
                    return;
                }

                dgBeneficiaryPaymentTrack.SelectedIndex = e.Item.ItemIndex;

                long id = (DataCheck.IsNumeric(dgBeneficiaryPaymentTrack.DataKeys[e.Item.ItemIndex].ToString()))
                              ? long.Parse(dgBeneficiaryPaymentTrack.DataKeys[e.Item.ItemIndex].ToString())
                              : 0;

                if (id < 1)
                {
                    ErrorDisplay1.ShowError("Invalid Record Selection!");
                    return;
                }

                var expenseTransactionPayment = expenseTransactionPaymentList.Find(m => m.StaffExpenseTransactionPaymentId == id);

                if (expenseTransactionPayment == null || expenseTransactionPayment.StaffExpenseTransactionPaymentId < 1)
                {
                    ErrorDisplay1.ShowError("Invalid record selection.");
                    return;
                }

                if (expenseTransactionPayment.Balance <= 0)
                {
                    ErrorDisplay1.ShowError("Transaction Payment has been completed");
                    btnSubmit.Enabled = false;
                }

                else
                {

                    var expenseTransaction = ServiceProvider.Instance().GetStaffExpenseTransactionServices().GetStaffExpenseTransaction(expenseTransactionPayment.StaffExpenseTransactionId);
                    if (expenseTransaction == null)
                    {
                        ErrorDisplay1.ShowError("Invalid record selection.");
                        return;
                    }

                    if (expenseTransaction.StaffExpenseTransactionId < 1)
                    {
                        ErrorDisplay1.ShowError("Invalid record selection.");
                        return;
                    }

                    var approvedTransactions = ServiceProvider.Instance().GetStaffExpenseApprovalServices().GetStaffExpenseApprovalsByStaffExpenseTransactionId(expenseTransaction.StaffExpenseTransactionId);

                    if (approvedTransactions == null || !approvedTransactions.Any())
                    {
                        ErrorDisplay1.ShowError("Expense Transaction Payment list is empty or session has expired.");
                        return;
                    }

                    var expenseApproval = approvedTransactions.First();

                    var expensePaymentHistoryList = ServiceProvider.Instance().GetStaffExpenseTransactionPaymentHistoryServices().GetStaffRecentExpenseTransactionPaymentHistoriesByStaffExpenseTransactionId(expenseTransaction.StaffExpenseTransactionId, expenseTransactionPayment);

                    mpeSelectDateRangePopup.TargetControlID = btnPopUp.ID;
                    mpeSelectDateRangePopup.CancelControlID = btnReset.ID;
                    mpeSelectDateRangePopup.PopupControlID = dvExpensePayment.ID;
                    txtExpenseCategory.Text = expenseTransaction.ExpenseCategory.Title;
                    txtExpenseItem.Text = expenseTransaction.ExpenseItem.Title;
                    txtTitle.Text = expenseTransactionPayment.StaffExpenseTransaction.ExpenseTitle;
                    txtApprovedQuantity.Text = expenseApproval.ApprovedQuantity.ToString(CultureInfo.InvariantCulture);
                    txtApprovedUnitPrice.Text = expenseApproval.ApprovedUnitAmmount.ToString(CultureInfo.InvariantCulture);
                    txtApprovedTotalAmount.Text = expenseApproval.ApprovedTotalAmmount.ToString(CultureInfo.InvariantCulture);
                    txtBalance.Text = expenseTransactionPayment.Balance.ToString(CultureInfo.InvariantCulture);
                    if (!expensePaymentHistoryList.Any())
                    {
                        txtPaymentComment.Text = string.Empty;
                    }
                    else
                    {
                        var expensePaymentHistory = expensePaymentHistoryList.Find(m => m.StaffExpenseTransactionId == expenseTransactionPayment.StaffExpenseTransactionId);
                        txtPaymentComment.Text = expensePaymentHistory.Comment;
                    }
                    txtApprovedBy.Text = new PortalServiceManager().GetPortalUserById(expenseApproval.ApprovedById).UserName;
                    txtApprovalDateTime.Text = expenseApproval.DateApproved + "  " + expenseApproval.TimeApproved;
                    lgTransactionTitle.InnerText = expenseTransactionPayment.StaffExpenseTransaction.ExpenseTitle;
                    btnSubmit.Text = "Update Payment";
                    iBalance.InnerHtml = "Old Balance";
                    mpeSelectDateRangePopup.Show();
                    Session["_staffExpenseTransactionPayment"] = expenseTransactionPayment;
                }

            }

            catch (Exception ex)
            {
                ErrorDisplayCompletePayment.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                mpeSelectDateRangePopup.Show();
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }


        }
        protected void LnkSelectNewBeneficiaryClick(object sender, EventArgs e)
        {
            ddlBeneficiaries.SelectedIndex = 0;
            divPaymentTrack.Visible = false;
            divBeneficiary.Visible = true;
        }
        #endregion

        #region Page Helpers
        private bool LoadTransactionPayments()
        {

            try
            {
                var expenseTransactionPaymentList = ServiceProvider.Instance().GetStaffExpenseTransactionPaymentServices().GetStaffBeneficiaryUncompletedExpenseTransactionPayments(int.Parse(ddlBeneficiaries.SelectedValue));

                if (!expenseTransactionPaymentList.Any())
                {
                    ErrorDisplay1.ShowError(" There are no uncompleted Transaction Payments for this Staff Beneficiary");
                    dgBeneficiaryPaymentTrack.DataSource = new List<StaffExpenseTransactionPayment>();
                    dgBeneficiaryPaymentTrack.DataBind();
                    hBeneficiary.InnerHtml = ddlBeneficiaries.SelectedItem.Text;
                    divPaymentTrack.Visible = true;
                    return false;
                }

                dgBeneficiaryPaymentTrack.DataSource = expenseTransactionPaymentList;
                dgBeneficiaryPaymentTrack.DataBind();
                hBeneficiary.InnerHtml = ddlBeneficiaries.SelectedItem.Text;
                Session["_staffTransactionPaymentList"] = expenseTransactionPaymentList;
                return true;

            }

            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                return false;
            }
        }
        private void LoadBeneficiaries()
        {
            try
            {
                ddlBeneficiaries.DataSource = new List<StaffBeneficiary>();
                ddlBeneficiaries.DataBind();
                ddlBeneficiaries.Items.Insert(0, new ListItem("--List is empty--", "0"));
                ddlBeneficiaries.SelectedIndex = 0;

                var filteredBeneficiaries = ServiceProvider.Instance().GetStaffBeneficiaryServices().GetStaffBeneficiariesWithUnCompletedTransactionPayments();

                if (filteredBeneficiaries == null)
                {
                    ErrorDisplay1.ShowError("The Beneficiary list is empty!");
                    return;
                }

                if (!filteredBeneficiaries.Any())
                {
                    ErrorDisplay1.ShowError("The Beneficiary list is empty!");
                    return;
                }

                ddlBeneficiaries.DataSource = filteredBeneficiaries;
                ddlBeneficiaries.DataValueField = "StaffBeneficiaryId";
                ddlBeneficiaries.DataTextField = "FullName";
                ddlBeneficiaries.DataBind();
                ddlBeneficiaries.Items.Insert(0, new ListItem("--Select a Beneficiary--", "0"));
                ddlBeneficiaries.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
            }
        }
        private bool AddTransactionPaymentHistory(long transactionPaymentId)
        {
            try
            {
                if (Session["_staffExpenseTransactionPayment"] == null)
                {
                    ErrorDisplay1.ShowError("Expense Transaction list is empty or session has expired.");
                    return false;
                }

                var expenseTransactionPayment = Session["_staffExpenseTransactionPayment"] as StaffExpenseTransactionPayment;

                if (expenseTransactionPayment == null || expenseTransactionPayment.StaffExpenseTransactionId < 1)
                {
                    ErrorDisplay1.ShowError("Expense Transaction list is empty or session has expired.");
                    return false;
                }

                var newOrModifiedTransactioPaymentHistory = new StaffExpenseTransactionPaymentHistory
                                                                {
                                                                    StaffExpenseTransactionId = expenseTransactionPayment.StaffExpenseTransactionId,
                                                                    StaffExpenseTransactionPaymentId = transactionPaymentId,
                                                                    PaymentDate = DateMap.GetLocalDate(),
                                                                    PaymentTime = DateMap.GetLocalTime(),
                                                                    Status = 1,
                                                                    AmountPaid = double.Parse(txtAmountPaid.Value.Trim()),
                                                                    PaidById = new PortalServiceManager().GetUserIdByUsername(HttpContext.Current.User.Identity.Name),
                                                                    Comment = txtPaymentComment.Text.Trim(),
                                                                };

                var k = ServiceProvider.Instance().GetStaffExpenseTransactionPaymentHistoryServices().AddStaffExpenseTransactionPaymentHistory(newOrModifiedTransactioPaymentHistory);

                if (k < 1)
                {
                    ErrorDisplay1.ShowError("The Transaction Payment Information could not be Updated");
                    return false;
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
        private long UpdateTransactionPayment()
        {
            ErrorDisplay1.ClearError();
            try
            {
                if (Session["_staffExpenseTransactionPayment"] == null)
                {
                    ErrorDisplay1.ShowError("Expense Transaction Payment list is empty or session has expired.");
                    return 0;
                }

                var expenseTransactionPayment = Session["_staffExpenseTransactionPayment"] as StaffExpenseTransactionPayment;

                if (expenseTransactionPayment == null || expenseTransactionPayment.StaffExpenseTransactionId < 1)
                {
                    ErrorDisplayCompletePayment.ShowError("Invalid record selection.");
                    mpeSelectDateRangePopup.Show();
                    return 0;
                }

                expenseTransactionPayment.Balance = expenseTransactionPayment.Balance - double.Parse(txtAmountPaid.Value.Trim());
                expenseTransactionPayment.TotalAmountPayable = double.Parse(txtApprovedTotalAmount.Text.Trim());
                expenseTransactionPayment.StaffExpenseTransactionId = expenseTransactionPayment.StaffExpenseTransactionId;
                expenseTransactionPayment.LastPaymentDate = DateMap.GetLocalDate();
                expenseTransactionPayment.LastPaymentTime = DateMap.GetLocalTime();
                expenseTransactionPayment.Status = 1;
                expenseTransactionPayment.AmountPaid = double.Parse(txtAmountPaid.Value.Trim()) + expenseTransactionPayment.AmountPaid;

                var k = ServiceProvider.Instance().GetStaffExpenseTransactionPaymentServices().UpdateStaffExpenseTransactionPayment(expenseTransactionPayment);

                if (k < 1)
                {
                    ErrorDisplay1.ShowError("The Transaction information could not be updated");
                    return 0;
                }

                ErrorDisplay1.ShowSuccess("Expense Transaction Information was successfully updated");
                return k;

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                return 0;
            }
        }
        private bool ValidateTransactionPaymentControl()
        {
            ErrorDisplay1.ClearError();
            ErrorDisplayCompletePayment.ClearError();

            try
            {
                if (string.IsNullOrEmpty(txtAmountPaid.Value.Trim()))
                {
                    ErrorDisplayCompletePayment.ShowError("Please supply the amount to be paid.");
                    txtAmountPaid.Focus();
                    mpeSelectDateRangePopup.Show();
                    return false;
                }

                if (txtAmountPaid.Value.Trim().Contains('-') || txtAmountPaid.Value.Trim().Contains('+') || txtAmountPaid.Value.Trim().Contains('*') || txtAmountPaid.Value.Trim().Contains('/'))
                {
                    ErrorDisplayCompletePayment.ShowError("Invalid entry!");
                    txtAmountPaid.Focus();
                    mpeSelectDateRangePopup.Show();
                    return false;
                }

                if (double.Parse(txtAmountPaid.Value.Trim()) > double.Parse(txtBalance.Text.Trim()))
                {
                    ErrorDisplayCompletePayment.ShowError("Please enter an amount less than or equal to the Expense Transaction balance.");
                    mpeSelectDateRangePopup.Show();
                    return false;
                }

                if (!DataCheck.IsNumeric(txtAmountPaid.Value.Trim()))
                {
                    ErrorDisplayCompletePayment.ShowError("Invalid entry!");
                    txtAmountPaid.Focus();
                    mpeSelectDateRangePopup.Show();
                    return false;
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
        private void ClearControls()
        {
            ErrorDisplay1.ClearError();
            ErrorDisplayCompletePayment.ClearError();
            txtTitle.Text = string.Empty;
            txtExpenseCategory.Text = string.Empty;
            txtExpenseItem.Text = string.Empty;
            txtApprovalDateTime.Text = string.Empty;
            txtApprovedUnitPrice.Text = string.Empty;
            txtApprovalDateTime.Text = string.Empty;
            txtApprovedBy.Text = string.Empty;
            txtApprovedQuantity.Text = string.Empty;
            txtApprovedTotalAmount.Text = string.Empty;
            txtAmountPaid.Value = string.Empty;
        }
        #endregion
    }
}