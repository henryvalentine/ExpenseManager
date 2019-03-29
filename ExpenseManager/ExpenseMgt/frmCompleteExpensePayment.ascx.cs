using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExpenseManager.CoreFramework;
using ExpenseManager.CoreFramework.AlertControl;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;
using xPlug.BusinessObject.CustomizedASPBusinessObject.Enum;
using xPlug.BusinessService;

namespace ExpenseManager.ExpenseMgt
{
    public partial class FrmCompleteExpensePayment : UserControl
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
                Session["_expenseTransactionHistoryList"] = null;
                Session["_expenseTransactionPaymentList"] = null;
                Session["_approvedTransactionList"] = null;
                divBeneficiary.Visible = true;
                divPaymentTrack.Visible = false;
                LoadBeneficiaries();
            }
        }
        protected void BtnUpdatePaymentClick(object sender, EventArgs e)
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
                    ConfirmAlertBox1.ShowMessage("The Transaction Payment information could not be Updated.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                if (!AddTransactionPaymentHistory(k))
                {
                    ConfirmAlertBox1.ShowMessage("The Transaction Payment information could not be Updated.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                //var popup = "<script language='javascript' ID='updatePaymentVoucher'>" + "window.open('ExpenseMgt/Voucher/VoucherManager.aspx?data=" + HttpUtility.UrlEncode(k.ToString(CultureInfo.InvariantCulture)) + "','newwindow', 'top=0,left=50,width=900,height=650,dependant = no, alwaysRaised = no, menubar=yes, resizable=no ,scrollbars=yes, toolbar=no,status=no');</script>";

                //ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "updatePaymentVoucher", popup, false);

                if (int.Parse(ddlPaymentMode.SelectedValue) == 3)
                {
                    var transactionPayment = ServiceProvider.Instance().GetExpenseTransactionPaymentServices().GetUncompltedTransactionPayment(k);

                    if (transactionPayment != null && transactionPayment.ExpenseTransactionPaymentId > 1 && transactionPayment.Balance < transactionPayment.TotalAmountPayable && transactionPayment.Balance > 0)
                    {
                        UpdateWithChqueOnly(transactionPayment);
                        return;
                    }
                }

                if (!LoadTransactionPayments())
                {
                    ddlPaymentMode.SelectedIndex = 0;
                    return;
                }
                Session["_expenseTransactionPayment"] = null;
                btnSubmit.CommandArgument = "0";
                btnUpdatePayment.CommandArgument = "0";
                ConfirmAlertBox1.ShowSuccessAlert("Transaction Payment Information was successfully updated");
            }

            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }

        }
        protected void DdlBeneficiariesIndexChanged(object sender, EventArgs e)
        {
            try
            {
                btnUpdatePayment.CommandArgument = "0";
                if (int.Parse(ddlBeneficiaries.SelectedValue) < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Please select a Beneficiary", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }
                LoadPaymentModes();
                LoadBank();
                if (!LoadTransactionPayments())
                {
                    return;
                }
                
                divBeneficiary.Visible = false;
                divPaymentTrack.Visible = true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        protected void DgBeneficiaryPaymentTrackCommand(object source, DataGridCommandEventArgs e)
        {
            ErrorDisplayCashPayment.ClearError();
            ErrorDispChequePayment.ClearError();
            try
            {
                if(int.Parse(ddlPaymentMode.SelectedValue) <  1)
                {
                    ConfirmAlertBox1.ShowMessage("Please select a Payment mode.", ConfirmAlertBox.PopupMessageType.Error);
                    return;  
                }
                    ClearControls();
                    dgBeneficiaryPaymentTrack.SelectedIndex = e.Item.ItemIndex;

                    long id = (DataCheck.IsNumeric(dgBeneficiaryPaymentTrack.DataKeys[e.Item.ItemIndex].ToString()))
                                  ? long.Parse(dgBeneficiaryPaymentTrack.DataKeys[e.Item.ItemIndex].ToString())
                                  : 0;

                    if (id < 1)
                    {
                        ConfirmAlertBox1.ShowMessage("Invalid Record Selection!", ConfirmAlertBox.PopupMessageType.Error);
                        return;
                    }

                    var expenseTransactionPayment = ServiceProvider.Instance().GetExpenseTransactionPaymentServices().GetExpenseTransactionPayment(id);

                    if (expenseTransactionPayment == null || expenseTransactionPayment.ExpenseTransactionPaymentId < 1)
                    {
                        ConfirmAlertBox1.ShowMessage("Invalid record selection.", ConfirmAlertBox.PopupMessageType.Error);
                        return;
                    }

                    if (expenseTransactionPayment.Balance <= 0)
                    {
                        ConfirmAlertBox1.ShowMessage("Transaction Payment has been completed", ConfirmAlertBox.PopupMessageType.Error);
                        btnSubmit.Enabled = false;
                    }

                    else
                    {
                        var expenseTransaction = expenseTransactionPayment.ExpenseTransaction;

                        if (expenseTransaction == null || expenseTransaction.ExpenseTransactionId < 1)
                        {
                            ConfirmAlertBox1.ShowMessage("Invalid record selection.", ConfirmAlertBox.PopupMessageType.Error);
                            return;
                        }
                        if (int.Parse(ddlPaymentMode.SelectedValue) == 1 || int.Parse(ddlPaymentMode.SelectedValue) == 3)
                        {
                            txtTotalPayableAmount.Text = expenseTransaction.TotalApprovedAmount.ToString(CultureInfo.InvariantCulture);
                            txtOldBalance.Text = expenseTransactionPayment.Balance.ToString(CultureInfo.InvariantCulture);
                            lgUpdatePayment.InnerText = expenseTransactionPayment.ExpenseTransaction.ExpenseTitle;
                            btnUpdatePayment.Text = "Update";
                            btnUpdatePayment.CommandArgument = "1";
                            btnSubmit.CommandArgument = "1";
                            mpeSelectDateRangePopup.CancelControlID = btnReset.ID;
                            mpeSelectDateRangePopup.PopupControlID = dvExpensePayment.ID;
                            mpeSelectDateRangePopup.Show();
                            Session["_expenseTransactionPayment"] = expenseTransactionPayment;
                            return;
                        }

                        if (int.Parse(ddlPaymentMode.SelectedValue) == 2)
                        {
                            txtChequeTotalPayableAmount.Text = expenseTransaction.TotalApprovedAmount.ToString(CultureInfo.InvariantCulture);
                            txtOldChequeBalance.Text = expenseTransactionPayment.Balance.ToString(CultureInfo.InvariantCulture);
                            lgChequeUpdate.InnerText = expenseTransactionPayment.ExpenseTransaction.ExpenseTitle;
                            btnSubmit.Text = "Update";
                            btnUpdatePayment.CommandArgument = "2";
                            btnSubmit.CommandArgument = "2";
                            ErrorDispChequePayment.ClearError();
                            mpeSelectDateRangePopup.CancelControlID = btnChequeClose.ID;
                            mpeSelectDateRangePopup.PopupControlID = dvChequePayment.ID;
                            mpeSelectDateRangePopup.Show();
                            Session["_expenseTransactionPayment"] = expenseTransactionPayment;
                        }
                }
             }
            catch (Exception ex)
            {
                ErrorDispChequePayment.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                mpeSelectDateRangePopup.Show();
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }
        }
        protected void BtnSelectNewBeneficiaryClick(object sender, EventArgs e)
        {
            LoadBeneficiaries();
            Session["_expenseTransactionPayment"] = null;
            btnSubmit.CommandArgument = "0";
            btnUpdatePayment.CommandArgument = "0";
            divPaymentTrack.Visible = false;
            divBeneficiary.Visible = true;
        }
        protected void BtnChequeCloseClick(object sender, EventArgs e)
        {
            Session["_expenseTransactionPayment"] = null;
            btnSubmit.CommandArgument = "0";
            btnUpdatePayment.CommandArgument = "0";
            LoadTransactionPayments();
        }
        #endregion

        #region Page Helpers
        private bool LoadTransactionPayments()
        {

            try
            {
                var expenseTransactionPaymentList = ServiceProvider.Instance().GetExpenseTransactionPaymentServices().GetBeneficiaryUncompletedExpenseTransactionPayments(int.Parse(ddlBeneficiaries.SelectedValue));

                if (!expenseTransactionPaymentList.Any())
                {
                    ConfirmAlertBox1.ShowMessage("There are no uncompleted Transaction Payments for this Beneficiary", ConfirmAlertBox.PopupMessageType.Error);
                    dgBeneficiaryPaymentTrack.DataSource = new List<ExpenseTransactionPayment>();
                    dgBeneficiaryPaymentTrack.DataBind();
                    hBeneficiary.InnerHtml = ddlBeneficiaries.SelectedItem.Text;
                    divPaymentTrack.Visible = false;
                    divBeneficiary.Visible = true;
                    return false;
                }
                
                Session["_expTransactionPaymentList"] = null;
                Session["_expTransactionPaymentList"] = expenseTransactionPaymentList;
              
                Limit = int.Parse(ddlLimit.SelectedValue);
                FillRepeater<ExpenseTransactionPayment>(dgBeneficiaryPaymentTrack, "_expTransactionPaymentList", Navigation.None, Limit, LoadMethod);
                hBeneficiary.InnerHtml = ddlBeneficiaries.SelectedItem.Text;
                return true;

            }

            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }
        }
        private void LoadBeneficiaries()
        {
            try
            {
                ddlBeneficiaries.DataSource = new List<Beneficiary>();
                ddlBeneficiaries.DataBind();
                ddlBeneficiaries.Items.Insert(0, new ListItem("-- List is empty --", "0"));
                ddlBeneficiaries.SelectedIndex = 0;

                var filteredBeneficiaries = ServiceProvider.Instance().GetBeneficiaryServices().GetBeneficiariesWithUnCompletedTransactionPayments();


                if (filteredBeneficiaries == null || !filteredBeneficiaries.Any())
                {
                    ConfirmAlertBox1.ShowMessage("The Beneficiary list is empty!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }
                
                ddlBeneficiaries.DataSource = filteredBeneficiaries;
                ddlBeneficiaries.DataValueField = "BeneficiaryId";
                ddlBeneficiaries.DataTextField = "FullName";
                ddlBeneficiaries.DataBind();
                ddlBeneficiaries.Items.Insert(0, new ListItem("-- Select a Beneficiary --", "0"));
                ddlBeneficiaries.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        private void LoadPaymentModes()
        {
            try
            {
                var paymentModes = ServiceProvider.Instance().GetPaymentModeServices().GetPaymentModes();
                if (paymentModes == null || paymentModes.Any())
                {
                    ddlPaymentMode.DataSource = new List<PaymentMode>();
                    ddlPaymentMode.DataBind();
                    ddlPaymentMode.Items.Insert(0, new ListItem(" -- List is empty -- ", "0"));
                    ddlPaymentMode.SelectedIndex = 0;
                }
                ddlPaymentMode.DataSource = paymentModes;
                ddlPaymentMode.DataValueField = "PaymentModeId";
                ddlPaymentMode.DataTextField = "Name";
                ddlPaymentMode.DataBind();
                ddlPaymentMode.Items.Insert(0, new ListItem(" -- Select Payment Mode -- ", "0"));
                ddlPaymentMode.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("Payment Modes could not be loaded", ConfirmAlertBox.PopupMessageType.Error);

            }
        }
        private void LoadBank()
        {
            try
            {
                var banks = ServiceProvider.Instance().GetBankServices().GetBanks();
                if (banks == null || !banks.Any())
                {
                    ddChequeBank.DataSource = new List<Bank>();
                    ddChequeBank.DataBind();
                    ddChequeBank.Items.Insert(0, new ListItem("-- List is empty --", "0"));
                    ddChequeBank.SelectedIndex = 0;
                    return;
                }
                ddChequeBank.DataSource = banks;
                ddChequeBank.DataTextField = "BankName";
                ddChequeBank.DataValueField = "BankId";
                ddChequeBank.DataBind();
                ddChequeBank.Items.Insert(0, new ListItem("-- Select Bank --", "0"));
                ddChequeBank.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("Bank List could not be retrieved. Please try again soon or contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        private bool AddTransactionPaymentHistory(long transactionPaymentId)
        {
            try
            {
                if (Session["_expenseTransactionPayment"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    
                    return false;
                }
                var expenseTransactionPayment = Session["_expenseTransactionPayment"] as ExpenseTransactionPayment;
                
                if (expenseTransactionPayment == null || expenseTransactionPayment.ExpenseTransactionId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                   
                    return false;
                }
                var expenseTransaction = expenseTransactionPayment.ExpenseTransaction;
                ExpenseTransactionPaymentHistory newOrModifiedExpenseTransactioPaymentHistory;
                long paymentHistoryId = 0;
                
                    int paymentModeId;
                    switch (int.Parse(btnUpdatePayment.CommandArgument))
                    {
                        case 1:
                            paymentModeId = 1;
                            newOrModifiedExpenseTransactioPaymentHistory = new ExpenseTransactionPaymentHistory
                                                                        {
                                                                            BeneficiaryId = expenseTransaction.BeneficiaryId,
                                                                            PaymentModeId = paymentModeId,
                                                                            ExpenseTransactionId = expenseTransaction.ExpenseTransactionId,
                                                                            ExpenseTransactionPaymentId = transactionPaymentId,
                                                                            PaymentDate = DateMap.GetLocalDate(),
                                                                            PaymentTime = DateMap.GetLocalTime(),
                                                                            Status = 1,
                                                                            AmountPaid = double.Parse(txtUpdateAmount.Value.Trim().Replace(",", string.Empty)),
                                                                            PaidById = new PortalServiceManager().GetUserIdByUsername(HttpContext.Current.User.Identity.Name),
                                                                            Comment = txtUpdatePaymentComment.Text.Trim(),
                                                                        };

                            paymentHistoryId = ServiceProvider.Instance().GetExpenseTransactionPaymentHistoryServices().AddTransactionPaymentHistoryAndPcv(newOrModifiedExpenseTransactioPaymentHistory);

                            if (paymentHistoryId < 1)
                            {
                                ErrorDisplayCashPayment.ShowError("The Transaction Payment Information could not be submitted");
                                mpeSelectDateRangePopup.Show();
                                return false;
                            }
                            break;

                        case 2:

                            if (!updateFileUploadControl.HasFile)
                            {
                                ConfirmAlertBox1.ShowMessage("Select the path to the cheque scanned copy", ConfirmAlertBox.PopupMessageType.Warning);
                                return false;
                            }

                            var imgBytes = FileUploader();

                            if (imgBytes == null)
                            {
                                ErrorDispChequePayment.ShowError("The Cheque information could not be submitted. Payment was not made.");
                                mpeSelectDateRangePopup.Show();
                                return false;
                            }
                            
                            paymentModeId = 2;
                            newOrModifiedExpenseTransactioPaymentHistory = new ExpenseTransactionPaymentHistory
                                                                        {
                                                                            BeneficiaryId = expenseTransaction.BeneficiaryId,
                                                                            PaymentModeId = paymentModeId,
                                                                            ExpenseTransactionId = expenseTransaction.ExpenseTransactionId,
                                                                            ExpenseTransactionPaymentId = transactionPaymentId,
                                                                            PaymentDate = DateMap.GetLocalDate(),
                                                                            PaymentTime = DateMap.GetLocalTime(),
                                                                            Status = 1,
                                                                            AmountPaid = double.Parse(txtChequeAmountToPay.Value.Trim().Replace(",", string.Empty)),
                                                                            PaidById = new PortalServiceManager().GetUserIdByUsername(HttpContext.Current.User.Identity.Name),
                                                                            Comment = txUpdateChequePaymentChequeComment.Text.Trim(),
                                                                        };


                    paymentHistoryId = ServiceProvider.Instance().GetExpenseTransactionPaymentHistoryServices().AddTransactionPaymentHistoryAndPcv(newOrModifiedExpenseTransactioPaymentHistory);

                    if (paymentHistoryId < 1)
                    {
                        ErrorDispChequePayment.ShowError("The Transaction Payment Information could not be submitted");
                        mpeSelectDateRangePopup.Show();
                        return false;
                    }

                    var newChequeInfo = new Cheque
                                            {
                                                Amount = double.Parse(txtChequeAmountToPay.Value.Trim()),
                                                ChequeNo = txtChequNo.Value.Trim(),
                                                ExpenseTransactionPaymentHistoryId = paymentHistoryId,
                                                BankId = int.Parse(ddChequeBank.Value),
                                                ScannedCopy = imgBytes
                                            };
                    var x = ServiceProvider.Instance().GetChequeServices().AddCheque(newChequeInfo);
                    if (x < 1)
                    {
                        ErrorDispChequePayment.ShowError("The Cheque information could not be submitted. Payment was not made");
                        mpeSelectDateRangePopup.Show();
                        if (!ServiceProvider.Instance().GetExpenseTransactionPaymentServices().DeleteExpenseTransactionPayment(transactionPaymentId))
                        {
                            return false;
                        }

                        if (!ServiceProvider.Instance().GetExpenseTransactionPaymentHistoryServices().DeleteExpenseTransactionPaymentHistory(paymentModeId))
                        {
                            return false;
                        }

                        return false;
                    }

                            break;

                        default:
                            ErrorDispChequePayment.ShowError("The Cheque information could not be submitted. Payment was not made.");
                            break;
                    }

                Session["_paymentHistoryId"] = paymentHistoryId;
                Session["_expenseTransactionPayment"] = null;
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again or contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }
        }
        private long UpdateTransactionPayment()
        {
            try
            {
                if (Session["_expenseTransactionPayment"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return 0;
                }

                var expenseTransactionPayment = Session["_expenseTransactionPayment"] as ExpenseTransactionPayment;

                if (expenseTransactionPayment == null || expenseTransactionPayment.ExpenseTransactionId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid selection!", ConfirmAlertBox.PopupMessageType.Error);
                    return 0;
                }

                long k = 0;

                if (int.Parse(btnUpdatePayment.CommandArgument) == 1)
                {
                    var balance = expenseTransactionPayment.Balance - double.Parse(txtUpdateAmount.Value.Trim().Replace(",", string.Empty));

                    if (balance.ToString(CultureInfo.InvariantCulture).Trim().Contains("-") || balance < 0)
                    {
                        ErrorDisplayCashPayment.ShowError("Please supply an amount less than or equal to the Transaction Old Balance.");
                        txtUpdateAmount.Focus();
                        mpeSelectDateRangePopup.Show();
                        return 0;
                    }

                    var status = 0;

                    if (balance > 0 && balance < expenseTransactionPayment.ExpenseTransaction.TotalApprovedAmount)
                    {
                        status = 0;
                    }
                    else if (balance.Equals(0.0))
                    {
                        status = 1;
                    }

                    //if (balance.Equals(expenseTransactionPayment.ExpenseTransaction.TotalApprovedAmount))
                    //{
                    //    status = 1;
                    //}

                    expenseTransactionPayment.Balance = balance;
                    expenseTransactionPayment.ExpenseTransactionId = expenseTransactionPayment.ExpenseTransactionId;
                    expenseTransactionPayment.LastPaymentDate = DateMap.GetLocalDate();
                    expenseTransactionPayment.LastPaymentTime = DateMap.GetLocalTime();
                    expenseTransactionPayment.Status = status;
                    expenseTransactionPayment.AmountPaid = double.Parse(txtUpdateAmount.Value.Trim()) + expenseTransactionPayment.AmountPaid;

                    k = ServiceProvider.Instance().GetExpenseTransactionPaymentServices().UpdateTransactionPayment(expenseTransactionPayment);

                    if (k < 1)
                    {
                        ConfirmAlertBox1.ShowMessage("The Transaction information could not be updated", ConfirmAlertBox.PopupMessageType.Error);
                        return 0;
                    }
                }

                if (int.Parse(btnUpdatePayment.CommandArgument) == 2)
                {
                    if (!updateFileUploadControl.HasFile)
                    {
                        ErrorDispChequePayment.ShowError("Please select a scanned copy of the cheque.");
                        updateFileUploadControl.Focus();
                        mpeSelectDateRangePopup.Show();
                        return 0;
                    }

                    var imgBytes = FileUploader();

                    if (imgBytes == null)
                    {
                        ErrorDispChequePayment.ShowError("The Cheque information could not be submitted. Payment was not made.");
                        mpeSelectDateRangePopup.Show();
                        return 0;
                    }

                    var chequBalance = expenseTransactionPayment.Balance - double.Parse(txtChequeAmountToPay.Value.Trim().Replace(",", string.Empty));

                    if (chequBalance.ToString(CultureInfo.InvariantCulture).Trim().Contains("-") || chequBalance < 0)
                    {
                        ErrorDispChequePayment.ShowError("Please supply an amount less than or equal to the Transaction Old Balance.");
                        txtChequeAmountToPay.Focus();
                        mpeSelectDateRangePopup.Show();
                        return 0;
                    }

                    var status = 0;

                    if (chequBalance > 0 && chequBalance < expenseTransactionPayment.ExpenseTransaction.TotalApprovedAmount)
                    {
                        status = 0;
                    }
                    else if (chequBalance.Equals(0.0))
                    {
                        status = 1;
                    }

                    //if (chequBalance.Equals(expenseTransactionPayment.ExpenseTransaction.TotalApprovedAmount))
                    //{
                    //    status = 1;
                    //}
                    expenseTransactionPayment.Balance = chequBalance;
                    expenseTransactionPayment.ExpenseTransactionId = expenseTransactionPayment.ExpenseTransactionId;
                    expenseTransactionPayment.LastPaymentDate = DateMap.GetLocalDate();
                    expenseTransactionPayment.LastPaymentTime = DateMap.GetLocalTime();
                    expenseTransactionPayment.Status = status;
                    expenseTransactionPayment.AmountPaid = double.Parse(txtChequeAmountToPay.Value.Trim()) + expenseTransactionPayment.AmountPaid;

                    k = ServiceProvider.Instance().GetExpenseTransactionPaymentServices().UpdateTransactionPayment(expenseTransactionPayment);

                    if (k < 1)
                    {
                        ConfirmAlertBox1.ShowMessage("The Transaction information could not be updated", ConfirmAlertBox.PopupMessageType.Error);
                        return 0;
                    }
                }
                Session["_expenseTransactionPayment"] = expenseTransactionPayment;
                return k;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                return 0;
            }
        }
        private void UpdateWithChqueOnly(ExpenseTransactionPayment transactionPayment)
        {
            try
            {
                var expenseTransaction = transactionPayment.ExpenseTransaction;
                txtChequeTotalPayableAmount.Text = expenseTransaction.TotalApprovedAmount.ToString(CultureInfo.InvariantCulture);
                txtOldChequeBalance.Text = transactionPayment.Balance.ToString(CultureInfo.InvariantCulture);
                lgChequeUpdate.InnerText = transactionPayment.ExpenseTransaction.ExpenseTitle;
                btnSubmit.Text = "Update";
                btnSubmit.CommandArgument = "2";
                btnUpdatePayment.CommandArgument = "2";
                ErrorDispChequePayment.ClearError();
                txtChequeAmountToPay.Value = String.Empty;
                ddChequeBank.SelectedIndex = 0;
                txtUpdatePaymentComment.Text = string.Empty;
                txtChequNo.Value = String.Empty;
                mpeSelectDateRangePopup.CancelControlID = btnChequeClose.ID;
                mpeSelectDateRangePopup.PopupControlID = dvChequePayment.ID;
                mpeSelectDateRangePopup.Show();
                Session["_expenseTransactionPayment"] = transactionPayment;
                ErrorDispChequePayment.ShowSuccess("Transaction Payment Information was successfully updated. \nPlease kindly continue the update by providing the needed cheque information, <b>OR</b> click the <b>Close</b> button to make updates later.");
               
            }
            catch (Exception)
            {
                ConfirmAlertBox1.ShowMessage("Transaction cash Payment Information was successfully updated but an error was encountered while trying to initialize the cheque payment process. \nPlease try again or contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        private bool ValidateTransactionPaymentControl()
        {
            try
            {
                if (Session["_expenseTransactionPayment"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                var expenseTransactionPayment = Session["_expenseTransactionPayment"] as ExpenseTransactionPayment;

                if (expenseTransactionPayment == null || expenseTransactionPayment.ExpenseTransactionId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                if (int.Parse(btnSubmit.CommandArgument) == 1)
                {
                    if (string.IsNullOrEmpty(txtUpdateAmount.Value.Trim()))
                    {
                        ErrorDisplayCashPayment.ShowError("Please supply the amount to be paid.");
                        txtUpdateAmount.Focus();
                        mpeSelectDateRangePopup.Show();
                        return false;
                    }

                    if (txtUpdateAmount.Value.Trim().Contains('-') || txtUpdateAmount.Value.Trim().Contains('+') || txtUpdateAmount.Value.Trim().Contains('*') || txtUpdateAmount.Value.Trim().Contains('/'))
                    {
                        ErrorDisplayCashPayment.ShowError("Invalid entry!");
                        txtUpdateAmount.Focus();
                        mpeSelectDateRangePopup.Show();
                        return false;
                    }

                    if (double.Parse(txtUpdateAmount.Value.Trim()) > expenseTransactionPayment.Balance)
                    {
                        ErrorDisplayCashPayment.ShowError("Please enter an amount less than or equal to the Balance.");
                        mpeSelectDateRangePopup.Show();
                        return false;
                    }

                    if (!DataCheck.IsNumeric(txtUpdateAmount.Value.Trim()))
                    {
                        ErrorDisplayCashPayment.ShowError("Invalid entry!");
                        txtUpdateAmount.Focus();
                        mpeSelectDateRangePopup.Show();
                        return false;
                    }
                }

                if (int.Parse(btnSubmit.CommandArgument) == 2)
                {
                    if (string.IsNullOrEmpty(txtChequeAmountToPay.Value.Trim()))
                    {
                        ErrorDispChequePayment.ShowError("Please supply Cheque amount.");
                        txtChequeAmountToPay.Focus();
                        mpeSelectDateRangePopup.Show();
                        return false;
                    }
                    if (string.IsNullOrEmpty(txUpdateChequePaymentChequeComment.Text.Trim()))
                    {
                        ErrorDispChequePayment.ShowError("Please a comment .");
                        txUpdateChequePaymentChequeComment.Focus();
                        mpeSelectDateRangePopup.Show();
                        return false;
                    }

                    if (txtChequeAmountToPay.Value.Trim().Contains('-') || txtChequeAmountToPay.Value.Trim().Contains('+') || txtChequeAmountToPay.Value.Trim().Contains('*') || txtChequeAmountToPay.Value.Trim().Contains('/'))
                    {
                        ErrorDispChequePayment.ShowError("Invalid entry!");
                        txtChequeAmountToPay.Focus();
                        mpeSelectDateRangePopup.Show();
                        return false;
                    }

                    if (double.Parse(txtChequeAmountToPay.Value.Trim()) > expenseTransactionPayment.Balance)
                    {
                        ErrorDispChequePayment.ShowError("Please enter an amount less than or equal to the Transaction Balance.");
                        txtChequeAmountToPay.Focus();
                        mpeSelectDateRangePopup.Show();
                        return false;
                    }

                    if (!DataCheck.IsNumeric(txtChequeAmountToPay.Value.Trim()))
                    {
                        ErrorDispChequePayment.ShowError("Invalid entry!");
                        txtChequeAmountToPay.Focus();
                        mpeSelectDateRangePopup.Show();
                        return false;
                    }

                    if (string.IsNullOrEmpty(txtChequNo.Value.Trim()))
                    {
                        ErrorDispChequePayment.ShowError("Please supply Cheque Number.");
                        txtChequNo.Focus();
                        mpeSelectDateRangePopup.Show();
                        return false;
                    }

                    if (int.Parse(ddChequeBank.Value) < 1)
                    {
                        ErrorDispChequePayment.ShowError("Please select a bank.");
                        ddChequeBank.Focus();
                        mpeSelectDateRangePopup.Show();
                        return false;
                    }
                }

                return true;
            }
            catch (Exception)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }

        }
        private void ClearControls()
        {
            ddChequeBank.SelectedIndex = 0;
            txtUpdatePaymentComment.Text = string.Empty;
            txtChequNo.Value = string.Empty;
            txtUpdateAmount.Value = string.Empty; 
            txtChequeAmountToPay.Value = string.Empty;
            txtOldBalance.Text = string.Empty;
            txtTotalPayableAmount.Text = string.Empty;
            txtChequeTotalPayableAmount.Text = string.Empty;
            txtOldChequeBalance.Text = string.Empty;
            txtUpdatePaymentComment.Text = string.Empty;
            txUpdateChequePaymentChequeComment.Text = string.Empty;
        }
        private byte[] FileUploader()
        {

            if (!updateFileUploadControl.HasFile)
            {
                ConfirmAlertBox1.ShowMessage("Select the path to the cheque scanned copy", ConfirmAlertBox.PopupMessageType.Warning);
                return null;
            }

            var response = UploadManager2.UploadFile(ref updateFileUploadControl, UploadManager.ExtensionType.Scanned_Document, false, false);
            
            if (response == null)
            {
                ConfirmAlertBox1.ShowMessage("Cheque copy upload failed.", ConfirmAlertBox.PopupMessageType.Error);
                return null;
            }
            if (!response.Succeed)
            {
                ConfirmAlertBox1.ShowMessage(response.Message, ConfirmAlertBox.PopupMessageType.Error);
                return null;
            }

            return response.UploadedData;

        }
        #endregion

        #region Pagination
        protected void FirstLink(object sender, EventArgs e)
        {
            try
            {
                var senderLinkArgument = int.Parse(((LinkButton)sender).CommandArgument);
                NowViewing = (int)senderLinkArgument;
                FillRepeater<ExpenseTransactionPayment>(dgBeneficiaryPaymentTrack, "_expTransactionPaymentList", Navigation.Sorting, Limit, LoadMethod);
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
                DataCount = sesslist.Count;
                if (sesslist != null)
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
                    lblCurrentPage1.Text = "Page : " + (NowViewing + 1).ToString(CultureInfo.InvariantCulture) + " of " + objPds.PageCount.ToString(CultureInfo.InvariantCulture);
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
                FillRepeater<ExpenseTransactionPayment>(dgBeneficiaryPaymentTrack, "_expTransactionPaymentList", Navigation.Next, Limit, LoadMethod);
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
                FillRepeater<ExpenseTransactionPayment>(dgBeneficiaryPaymentTrack, "_expTransactionPaymentList", Navigation.First, Limit, LoadMethod);
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
                FillRepeater<ExpenseTransactionPayment>(dgBeneficiaryPaymentTrack, "_expTransactionPaymentList", Navigation.Last, Limit, LoadMethod);
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
                FillRepeater<ExpenseTransactionPayment>(dgBeneficiaryPaymentTrack, "_expTransactionPaymentList", Navigation.Previous, Limit, LoadMethod);
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
                FillRepeater<ExpenseTransactionPayment>(dgBeneficiaryPaymentTrack, "_expTransactionPaymentList", Navigation.None, Limit, LoadMethod);
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
        /* Method to hold all content frm the Db*/
        protected bool LoadMethod()
        {
            LoadTransactionPayments();
            return true;
        }
        #endregion
        #endregion
    }
}