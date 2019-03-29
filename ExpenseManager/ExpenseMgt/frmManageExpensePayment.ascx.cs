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
    public partial class FrmManageExpensePayment : UserControl
    {
        protected int Offset = 5;
        protected int DataCount = 0;
        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Limit = 15;
                GetPageLimits();
                ddlLimit.SelectedValue = "15";
                LoadApprovedUnpaidTransactions();
                LoadPaymentModes();
                LoadTransactionPayments();
                LoadBank();
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

                long k = 0;

                switch (int.Parse(btnSubmit.CommandArgument))
                {
                    case 1:
                        k = AddTransactionPayment();
                        break;

                    case 2:
                        k =  UpdateTransactionPayment();
                        break;

                    default:
                         ConfirmAlertBox1.ShowMessage("The Transaction Payment information could not be processed. Please try again or contact the Administrator", ConfirmAlertBox.PopupMessageType.Error);
                        break;     
                }

                if (k < 1)
                {
                    ConfirmAlertBox1.ShowMessage("The Transaction Payment information could not be processed. Please try again or contact the Administrator", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                if (!AddTransactionPaymentHistory(k))
                {
                    ConfirmAlertBox1.ShowMessage("The Transaction Payment information could not be Updated.", ConfirmAlertBox.PopupMessageType.Error);

                    if(!ServiceProvider.Instance().GetExpenseTransactionPaymentServices().DeleteExpenseTransactionPayment(k))
                    {
                        ConfirmAlertBox1.ShowMessage("An unknown error was encountered. The Transaction Payment information could not be Updated.", ConfirmAlertBox.PopupMessageType.Error);
                    }
                    return;
                }

                //var popup = "<script language='javascript' ID='paymentVoucherManager'>" + "window.open('ExpenseMgt/Voucher/VoucherManager.aspx?data=" + HttpUtility.UrlEncode(k.ToString(CultureInfo.InvariantCulture)) + "','newwindow', 'top=0,left=50,width=900,height=650,dependant = no, alwaysRaised = no, menubar=yes, resizable=no ,scrollbars=yes, toolbar=no,status=no');</script>";

                //ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "paymentVoucherManager", popup, false);

                if (int.Parse(ddlPaymentMode.SelectedValue) == 3)
                {
                    var transactionPayment = ServiceProvider.Instance().GetExpenseTransactionPaymentServices().GetUncompltedTransactionPayment(k);

                    if (transactionPayment != null && transactionPayment.ExpenseTransactionPaymentId > 1 && transactionPayment.Balance < transactionPayment.TotalAmountPayable && transactionPayment.Balance > 0)
                    {
                        UpdateWithChqueOnly(transactionPayment);
                       
                        return;
                    }
                }

                LoadApprovedUnpaidTransactions();
                if (!LoadTransactionPayments())
                {
                    return;
                }
                Session["_expenseTransaction"] = null;
                Session["_expenseTransactionPayment"] = null;
                btnSubmit.Text = "Submit";
                iChequeAmountBalance.InnerText = "Total Approved Amount";
                btnSubmit.CommandArgument = "0";
                btnChequeSubmit.CommandArgument = "0";
                ConfirmAlertBox1.ShowSuccessAlert("Transaction Payment Information was successfully updated");
            }

            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }

        }
        protected void BtnSearchByDateClick(object sender, EventArgs e)
        {
            if(!ValidateDateControls())
            {
                return;
            }

            GetTransactionsByDate();
        }
        protected void BtnGetAllTransactions(object sender, EventArgs e)
        {
            GetAllApprovedUnpaidExpenseTransactions();
        }
        protected void BtnContinueClick(object sender, EventArgs e)
        {
            try
            {
                ErrorDispChequePayment.ClearError();
                if (long.Parse(ddlExpenseTransactions.SelectedValue) < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Please select a Transaction", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                if (int.Parse(ddlPaymentMode.SelectedValue) < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Please select a <b>PAYMENT MODE.</b>", ConfirmAlertBox.PopupMessageType.Error);
                    return; 
                }
                ddlChequeDepartment.Enabled = true;
                btnChequeSubmit.CommandArgument = "0";
                btnSubmit.CommandArgument = "0";
                GetTransactionInfo(long.Parse(ddlExpenseTransactions.SelectedValue));

                }
                catch (Exception ex)
                {
                    ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                    ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                }
        }
        protected void BtnChequeCloseClick(object sender, EventArgs e)
        {
            LoadApprovedUnpaidTransactions();
            LoadTransactionPayments();
        }
        #endregion

        #region Page Helpers
        private void LoadApprovedUnpaidTransactions()
        {
            try
            {
                var beneficiaryApprovedUnpaidTransactions = ServiceProvider.Instance().GetExpenseTransactionServices().GetCurrentApprovedUnpaidExpenseTransactions(DateMap.GetLocalDate());

                if (beneficiaryApprovedUnpaidTransactions == null || !beneficiaryApprovedUnpaidTransactions.Any())
                {
                    ddlExpenseTransactions.DataSource = new List<ExpenseTransaction>();
                    ddlExpenseTransactions.DataBind();
                    ddlExpenseTransactions.Items.Insert(0, new ListItem("-- List is empty --", "0"));
                    ddlExpenseTransactions.SelectedIndex = 0;
                    return;
                }
                ddlExpenseTransactions.DataSource = beneficiaryApprovedUnpaidTransactions;
                ddlExpenseTransactions.DataTextField = "ExpenseTitle";
                ddlExpenseTransactions.DataValueField = "ExpenseTransactionId";
                ddlExpenseTransactions.DataBind();
                ddlExpenseTransactions.Items.Insert(0, new ListItem("-- Select Transaction --", "0"));
                ddlExpenseTransactions.SelectedIndex = 0;
               
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("Unpaid Transaction list could not be retrieved. Please try again soon or contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
              
            }
        }
        private void LoadDepartments()
        {
            try
            {
                var departments = ServiceProvider.Instance().GetDepartmentServices().GetDepartments();

                if (departments == null || !departments.Any())
                {
                    ddlDepartment.DataSource = new List<Department>();
                    ddlChequeDepartment.DataSource = new List<Department>();
                    ddlDepartment.DataBind();
                    ddlChequeDepartment.DataBind();
                    ddlDepartment.Items.Insert(0, new ListItem("-- List is empty --", "0"));
                    ddlDepartment.SelectedIndex = 0;
                    ddlChequeDepartment.Items.Insert(0, new ListItem("-- List is empty --", "0"));
                    ddlChequeDepartment.SelectedIndex = 0;
                    return;
                }
                ddlDepartment.DataSource = departments;
                ddlDepartment.DataTextField = "Name";
                ddlDepartment.DataValueField = "DepartmentId";
                ddlDepartment.DataBind();
                ddlDepartment.Items.Insert(0, new ListItem("-- Select Department --", "0"));
                ddlDepartment.SelectedIndex = 0;

                ddlChequeDepartment.DataSource = departments;
                ddlChequeDepartment.DataTextField = "Name";
                ddlChequeDepartment.DataValueField = "DepartmentId";
                ddlChequeDepartment.DataBind();
                ddlChequeDepartment.Items.Insert(0, new ListItem("-- Select Department --", "0"));
                ddlChequeDepartment.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("Unpaid Transaction list could not be retrieved. Please try again soon or contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);

            }
        }
        private void GetTransactionInfo(long expenseTransactionId)
        {
            try
            {
               

                var expenseTransaction = ServiceProvider.Instance().GetExpenseTransactionServices().GetExpenseTransactionInfo(expenseTransactionId);
                
                if(expenseTransaction == null || expenseTransaction.ExpenseTransactionId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Expense Transaction details could not be retrieved. Please try again.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                LoadDepartments();
                Session["_expenseTransaction"] = null;

                ClearControls();
                if (int.Parse(ddlPaymentMode.SelectedValue) == 1 || int.Parse(ddlPaymentMode.SelectedValue) == 3)
                {
                    txtApprovedQuantity.Text = expenseTransaction.TotalApprovedQuantity.ToString(CultureInfo.InvariantCulture);
                    txtApprovedDate.Text = expenseTransaction.DateApproved;
                    txtApprovalTime.Text = expenseTransaction.TimeApproved;
                    txtApprovedTotalAmount.Text = expenseTransaction.TotalApprovedAmount.ToString(CultureInfo.InvariantCulture);
                    if (expenseTransaction.BeneficiaryTypeId != 1)
                    {
                        divDeptOwner.Visible = true;
                        reqDdlDepartment.Enabled = true;
                    }
                    else
                    {
                        divDeptOwner.Visible = false;
                        reqDdlDepartment.Enabled = false;
                    }
                    mpeSelectDateRangePopup.CancelControlID = "btnReset";
                    mpeSelectDateRangePopup.PopupControlID = "dvExpensePayment";
                    btnChequeSubmit.CommandArgument = "1";//To help determine that payment at this time is for cash only
                    btnSubmit.CommandArgument = "1";
                    mpeSelectDateRangePopup.Show();
                    Session["_expenseTransaction"] = expenseTransaction;
                    return;
                }

                if (int.Parse(ddlPaymentMode.SelectedValue) == 2)
                {
                    txtChequeApprovedDate.Text = expenseTransaction.DateApproved.ToString(CultureInfo.InvariantCulture);
                    txtChequeApprovedTime.Text = expenseTransaction.TimeApproved;
                    txtChequeApprovedTotalAmount.Text = expenseTransaction.TotalApprovedAmount.ToString(CultureInfo.InvariantCulture);
                    txtChequeApprovedTotalQuantity.Text = expenseTransaction.TotalApprovedQuantity.ToString(CultureInfo.InvariantCulture);
                    if (expenseTransaction.BeneficiaryTypeId != 1)
                    {
                        pnlChequeDepartment.Visible = true;
                        reqFieldChequeDepart.Enabled = true;
                    }
                    else
                    {
                        pnlChequeDepartment.Visible = false;
                        reqFieldChequeDepart.Enabled = false;
                    }
                    mpeSelectDateRangePopup.CancelControlID = "closePop";
                    iChequeAmountBalance.InnerText = "Total Approved Amount";
                    btnChequeSubmit.Text = "Submit";
                    btnSubmit.CommandArgument = "1";
                    btnChequeSubmit.CommandArgument = "2";//To help determine that payment at this time is for Cheque only
                    mpeSelectDateRangePopup.PopupControlID = "dvChequePayment";
                    mpeSelectDateRangePopup.Show();
                    Session["_expenseTransaction"] = expenseTransaction;
                }
               
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        private void LoadBank()
        {
            try
            {
                var banks = ServiceProvider.Instance().GetBankServices().GetBanks();
                if (banks == null || !banks.Any())
                {
                    ddBank.DataSource = new List<Bank>();
                    ddBank.DataBind();
                    ddBank.Items.Insert(0, new ListItem("-- List is empty --", "0"));
                    ddBank.SelectedIndex = 0;
                    return;
                }
                ddBank.DataSource = banks;
                ddBank.DataTextField = "BankName";
                ddBank.DataValueField = "BankId";
                ddBank.DataBind();
                ddBank.Items.Insert(0, new ListItem("-- Select Bank --", "0"));
                ddBank.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("Bank List could not be retrieved. Please try again soon or contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        private void LoadPaymentModes()
        {
            try
            {
                var paymentModes = ServiceProvider.Instance().GetPaymentModeServices().GetPaymentModes();
                if(paymentModes == null || paymentModes.Any())
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
        private void ClearControls()
        {
            ddBank.SelectedIndex = 0;
            txtPaymentComment.Text = string.Empty;
            txtChequNo.Value = string.Empty;
            txtAmountPaid.Value = string.Empty; txtChequeAmount.Value = string.Empty;
            txtApprovalTime.Text = string.Empty;
            txtApprovedDate.Text = string.Empty;
            txtApprovedQuantity.Text = string.Empty;
            txtApprovedTotalAmount.Text = string.Empty;
            txtChequNo.Value = string.Empty;
            txtChequeAmount.Value = string.Empty;
            txtChequeApprovedDate.Text = string.Empty;
            txtChequeApprovedTime.Text = string.Empty;
            txtChequeApprovedTotalAmount.Text = string.Empty;
            txtChequeApprovedTotalQuantity.Text = string.Empty;
            txtChequeComment.Text = string.Empty;
            txtPaymentComment.Text = string.Empty;
        }
        private bool LoadTransactionPayments()
        {
            try
            {
                var transactionDate = DateMap.GetLocalDate();
                var expenseTransactionPaymentList = ServiceProvider.Instance().GetExpenseTransactionPaymentServices().GetCurrentTransactionPayments(transactionDate);
                if (!expenseTransactionPaymentList.Any())
                {
                    dgExpenseTransactionPayment.DataSource = new List<ExpenseTransactionPayment>();
                    dgExpenseTransactionPayment.DataBind();
                    return false;
                }

                Session["_expTransactionUpdatePaymentList"] = null;
                Session["_expTransactionUpdatePaymentList"] = expenseTransactionPaymentList;
                Limit = int.Parse(ddlLimit.SelectedValue);
                FillRepeater<ExpenseTransactionPayment>(dgExpenseTransactionPayment, "_expTransactionUpdatePaymentList", Navigation.None, Limit, LoadMethod);
                LoadTransactionPaymentFooter();
                return true;


            }

            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }
        }
        private void GetTransactionsByDate()
        {
            try
            {
                ddlExpenseTransactions.DataSource = new List<ExpenseTransaction>();
                ddlExpenseTransactions.DataBind();
                ddlExpenseTransactions.Items.Insert(0, new ListItem("-- List is empty --", "0"));
                ddlExpenseTransactions.SelectedIndex = 0;

                if (!string.IsNullOrEmpty(txtStart.Text.Trim()) && !string.IsNullOrEmpty(txtEndDate.Text.Trim()))
                {
                    var startDate = DateTime.Parse(DateMap.ReverseToServerDate(txtStart.Text.Trim()));
                    var endDate = DateTime.Parse(DateMap.ReverseToServerDate(txtEndDate.Text.Trim()));
                    if (endDate < startDate || startDate > endDate)
                    {
                        ConfirmAlertBox1.ShowMessage("The <b>From</b> date must not be <b>LATER</b> than the <b>To</b> date.", ConfirmAlertBox.PopupMessageType.Error);
                        ddlExpenseTransactions.SelectedIndex = -1;
                        ddlExpenseTransactions.DataSource = new List<ExpenseTransaction>();
                        ddlExpenseTransactions.DataBind();
                        ddlExpenseTransactions.Items.Insert(0, new ListItem("-- List is empty --", "0"));
                        ddlExpenseTransactions.SelectedIndex = 0;
                        return;
                    }

                    var expTransactionsByDate = ServiceProvider.Instance().GetExpenseTransactionServices().GetApprovedUnpaidExpenseTransactionsByDateRange(startDate, endDate);
                    
                    if (!expTransactionsByDate.Any())
                    {
                        ConfirmAlertBox1.ShowMessage("No record found.", ConfirmAlertBox.PopupMessageType.Error);
                        return;
                    }

                    ddlExpenseTransactions.DataSource = expTransactionsByDate;
                    ddlExpenseTransactions.DataTextField = "ExpenseTitle";
                    ddlExpenseTransactions.DataValueField = "ExpenseTransactionId";
                    ddlExpenseTransactions.DataBind();
                    ddlExpenseTransactions.Items.Insert(0, new ListItem("-- Select Transaction --", "0"));
                    ddlExpenseTransactions.SelectedIndex = 0;
                    
                }
                
            }
            catch (Exception ex)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                
            }

        }
        private long UpdateTransactionPayment()
        {
            try
            {
                if (Session["_expenseTransactionPayment"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Expense Transaction Payment list is empty or session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return 0;
                }

                var expenseTransactionPayment = Session["_expenseTransactionPayment"] as ExpenseTransactionPayment;

                if (expenseTransactionPayment == null || expenseTransactionPayment.ExpenseTransactionId < 1)
                {
                    ErrorDispChequePayment.ShowError("Invalid record selection.");
                    mpeSelectDateRangePopup.Show();
                    return 0;
                }

                long k = 0;

                if (int.Parse(btnChequeSubmit.CommandArgument) == 1)
                {
                    if (double.Parse(txtAmountPaid.Value.Trim()) < 1)
                    {
                        ErrorDisplayExpensePayment.ShowError("Please supply a valid amount to be Paid.");
                        txtAmountPaid.Focus();
                        mpeSelectDateRangePopup.Show();
                        return 0;
                    }
                    
                    var balance = expenseTransactionPayment.Balance - double.Parse(txtAmountPaid.Value.Trim());

                    if (balance.ToString(CultureInfo.InvariantCulture).Trim().Contains("-") || balance < 0)
                    {
                        ErrorDisplayExpensePayment.ShowError("Please supply an amount less than or equal to the Transaction Old Balance.");
                        txtAmountPaid.Focus();
                        mpeSelectDateRangePopup.Show();
                        return 0;
                    }


                    var status = 0;

                    if (balance > 0 && balance < expenseTransactionPayment.ExpenseTransaction.TotalApprovedAmount)
                    {
                        status = 0;
                    }

                    if (balance.Equals(expenseTransactionPayment.ExpenseTransaction.TotalApprovedAmount))
                    {
                        status = 1;
                    }

                    expenseTransactionPayment.Balance = balance;
                    expenseTransactionPayment.ExpenseTransactionId = expenseTransactionPayment.ExpenseTransactionId;
                    expenseTransactionPayment.LastPaymentDate = DateMap.GetLocalDate();
                    expenseTransactionPayment.LastPaymentTime = DateMap.GetLocalTime();
                    expenseTransactionPayment.Status = status;
                    expenseTransactionPayment.AmountPaid = double.Parse(txtAmountPaid.Value.Trim()) + expenseTransactionPayment.AmountPaid;

                    k = ServiceProvider.Instance().GetExpenseTransactionPaymentServices().UpdateTransactionPayment(expenseTransactionPayment);

                    if (k < 1)
                    {
                        ConfirmAlertBox1.ShowMessage("The Transaction information could not be updated", ConfirmAlertBox.PopupMessageType.Error);
                        return 0;
                    }

                }

                if (int.Parse(btnChequeSubmit.CommandArgument) == 2)
                {
                    
                    if (double.Parse(txtChequeAmount.Value.Trim()) < 1)
                    {
                        ErrorDisplayExpensePayment.ShowError("Please supply a valid Cheque amount to be Paid.");
                        txtChequeAmount.Focus();
                        mpeSelectDateRangePopup.Show();
                        return 0;
                    }
                    
                    var chequBalance = expenseTransactionPayment.Balance - double.Parse(txtChequeAmount.Value.Trim());

                    if (chequBalance.ToString(CultureInfo.InvariantCulture).Trim().Contains("-") || chequBalance < 0)
                    {
                        ErrorDispChequePayment.ShowError("Please supply an amount less than or equal to the Transaction Old Balance.");
                        txtChequeAmount.Focus();
                        mpeSelectDateRangePopup.Show();
                        return 0;
                    }

                    var status = 0;

                    if (chequBalance > 0 && chequBalance < expenseTransactionPayment.ExpenseTransaction.TotalApprovedAmount)
                    {
                        status = 0;
                    }

                    if (chequBalance.Equals(expenseTransactionPayment.ExpenseTransaction.TotalApprovedAmount))
                    {
                        status = 1;
                    }

                    expenseTransactionPayment.Balance = chequBalance;
                    expenseTransactionPayment.ExpenseTransactionId = expenseTransactionPayment.ExpenseTransactionId;
                    expenseTransactionPayment.LastPaymentDate = DateMap.GetLocalDate();
                    expenseTransactionPayment.LastPaymentTime = DateMap.GetLocalTime();
                    expenseTransactionPayment.Status = status;
                    expenseTransactionPayment.AmountPaid = double.Parse(txtChequeAmount.Value.Trim()) + expenseTransactionPayment.AmountPaid;

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
        private long AddTransactionPayment()
        {
            try
            {
                if (Session["_expenseTransaction"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return 0;
                }

                var expenseTransaction = Session["_expenseTransaction"] as ExpenseTransaction;

                if (expenseTransaction == null || expenseTransaction.ExpenseTransactionId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return 0;
                }

                var newExpenseTransactionPayment = new ExpenseTransactionPayment();

                if (int.Parse(btnChequeSubmit.CommandArgument) == 1)
                {

                    if (double.Parse(txtAmountPaid.Value.Trim()) < 1)
                    {
                        ErrorDisplayExpensePayment.ShowError("Please supply a valid Cheque amount to be Paid.");
                        txtAmountPaid.Focus();
                        mpeSelectDateRangePopup.Show();
                        return 0;
                    }
                    
                    var balance = expenseTransaction.TotalApprovedAmount - double.Parse(txtAmountPaid.Value.Trim().Replace(",", string.Empty));

                    if (balance.ToString(CultureInfo.InvariantCulture).Trim().Contains("-") || balance < 0)
                    {
                        ErrorDisplayExpensePayment.ShowError("Please supply an amount less than or equal to the Transaction total amount.");
                        txtAmountPaid.Focus();
                        mpeSelectDateRangePopup.Show();
                        return 0;
                    }

                    int status = 0;

                    if (balance > 0 && balance < expenseTransaction.TotalApprovedAmount)
                    {
                        status = 0;
                    }
                    else if (balance.Equals(0.0))
                    {
                        status = 1;
                    }
                    //if (balance.Equals(expenseTransaction.TotalApprovedAmount))
                    //{
                    //    status = 1;
                    //}

                    var department = 8;

                    if (expenseTransaction.BeneficiaryTypeId == 1)
                    {
                        department = expenseTransaction.Beneficiary.DepartmentId;
                    }
                    else if (expenseTransaction.BeneficiaryTypeId != 1)
                    {
                        department = int.Parse(ddlDepartment.SelectedValue);
                    }
                    
                    newExpenseTransactionPayment = new ExpenseTransactionPayment
                                                    {
                                                        BeneficiaryId = expenseTransaction.BeneficiaryId,
                                                        Balance = balance,
                                                        TotalAmountPayable = expenseTransaction.TotalApprovedAmount,
                                                        ExpenseTransactionId = expenseTransaction.ExpenseTransactionId,
                                                        LastPaymentDate = DateMap.GetLocalDate(),
                                                        LastPaymentTime = DateMap.GetLocalTime(),
                                                        Status = status,
                                                        DepartmentId = department,
                                                        AmountPaid = double.Parse(txtAmountPaid.Value.Trim().Replace(",", string.Empty)),
                                                    }; 
                }

                if (int.Parse(btnChequeSubmit.CommandArgument) == 2)
                {

                    if (double.Parse(txtChequeAmount.Value.Trim()) < 1)
                    {
                        ErrorDisplayExpensePayment.ShowError("Please supply a valid Cheque amount to be Paid.");
                        txtChequeAmount.Focus();
                        mpeSelectDateRangePopup.Show();
                        return 0;
                    }
                    
                    var chequeBalance = expenseTransaction.TotalApprovedAmount - double.Parse(txtChequeAmount.Value.Trim().Replace(",", string.Empty));

                    if (chequeBalance.ToString(CultureInfo.InvariantCulture).Trim().Contains("-") || chequeBalance < 0)
                    {
                        ErrorDispChequePayment.ShowError("Please supply an amount less than or equal to the Transaction total amount.");
                        txtChequeAmount.Focus();
                        mpeSelectDateRangePopup.Show();
                        return 0;
                    }

                    var status = 0;

                    if (chequeBalance > 0 && chequeBalance < expenseTransaction.TotalApprovedAmount)
                    {
                        status = 0;
                    }
                    else if (chequeBalance.Equals(0.0))
                    {
                        status = 1;
                    }

                    //if (chequeBalance.Equals(expenseTransaction.TotalApprovedAmount))
                    //{
                        
                    //}

                    var department = 8;

                    if (expenseTransaction.BeneficiaryTypeId == 1)
                    {
                        department = expenseTransaction.Beneficiary.DepartmentId;
                    }
                    else if(expenseTransaction.BeneficiaryTypeId != 1)
                    {
                        department = int.Parse(ddlChequeDepartment.SelectedValue);
                    }
                    
                    newExpenseTransactionPayment = new ExpenseTransactionPayment
                                                        {
                                                            BeneficiaryId = expenseTransaction.BeneficiaryId,
                                                            Balance = chequeBalance,
                                                            TotalAmountPayable = expenseTransaction.TotalApprovedAmount,
                                                            ExpenseTransactionId = expenseTransaction.ExpenseTransactionId,
                                                            LastPaymentDate = DateMap.GetLocalDate(),
                                                            LastPaymentTime = DateMap.GetLocalTime(),
                                                            Status = status,
                                                            DepartmentId = department,
                                                            AmountPaid = double.Parse(txtChequeAmount.Value.Trim().Replace(",", string.Empty)),
                                                        };
                }

                var k = ServiceProvider.Instance().GetExpenseTransactionPaymentServices().AddExpenseTransactionPayment(newExpenseTransactionPayment);

                if (k < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Transaction payment could not be made.", ConfirmAlertBox.PopupMessageType.Error);
                    return 0;
                }
                Session["_expenseTransaction"] = expenseTransaction;
                return k;
            }

            catch (Exception)
            {
                ConfirmAlertBox1.ShowMessage("Process Failed: The Transaction Payment could not be made.", ConfirmAlertBox.PopupMessageType.Error);
                return 0;
            }
        }
        private bool AddTransactionPaymentHistory(long transactionPaymentId)
        {
            try
            {
                if (Session["_expenseTransaction"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);

                    return false;
                }
                var expenseTransaction = Session["_expenseTransaction"] as ExpenseTransaction;

                if (expenseTransaction == null || expenseTransaction.ExpenseTransactionId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);

                    return false;
                }

                ExpenseTransactionPaymentHistory newOrModifiedExpenseTransactioPaymentHistory;
                long paymentHistoryId = 0;
                int paymentModeId;
                switch (int.Parse(btnChequeSubmit.CommandArgument))
                {
                    case 1://Pay with Cash only
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
                                                                            AmountPaid = double.Parse(txtAmountPaid.Value.Trim()),
                                                                            PaidById = new PortalServiceManager().GetUserIdByUsername(HttpContext.Current.User.Identity.Name),
                                                                            Comment = txtPaymentComment.Text.Trim(),
                                                                        };

                        paymentHistoryId = ServiceProvider.Instance().GetExpenseTransactionPaymentHistoryServices().AddTransactionPaymentHistoryAndPcv(newOrModifiedExpenseTransactioPaymentHistory);

                        if (paymentHistoryId < 1)
                        {
                            ErrorDisplayExpensePayment.ShowError("The Transaction Payment Information could not be submitted");
                            mpeSelectDateRangePopup.Show();
                            return false;
                        }
                        break;

                    case 2: //Pay with Cheque only
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
                                                                            AmountPaid = double.Parse(txtChequeAmount.Value.Trim()),
                                                                            PaidById = new PortalServiceManager().GetUserIdByUsername(HttpContext.Current.User.Identity.Name),
                                                                            Comment = txtChequeComment.Text.Trim(),
                                                                        };


                        paymentHistoryId = ServiceProvider.Instance().GetExpenseTransactionPaymentHistoryServices().AddTransactionPaymentHistoryAndPcv(newOrModifiedExpenseTransactioPaymentHistory);

                        if (paymentHistoryId < 1)
                        {
                            ErrorDispChequePayment.ShowError("The Transaction Payment Information could not be submitted");
                            mpeSelectDateRangePopup.Show();
                            return false;
                        }

                    var imgBytes = FileUploader();

                    if (imgBytes == null)
                    {
                        ErrorDispChequePayment.ShowError("The Cheque information could not be submitted. Payment was not made");
                        mpeSelectDateRangePopup.Show();
                        if (!ServiceProvider.Instance().GetExpenseTransactionPaymentHistoryServices().DeleteExpenseTransactionPaymentHistory(paymentHistoryId))
                        {
                            return false;
                        }

                        if (!ServiceProvider.Instance().GetExpenseTransactionPaymentServices().DeleteExpenseTransactionPayment(transactionPaymentId))
                        {
                            return false;
                        }

                        return false;
                    }

                    var newChequeInfo = new Cheque
                                            {
                                                Amount = double.Parse(txtChequeAmount.Value.Trim()),
                                                ChequeNo = txtChequNo.Value.Trim(),
                                                ExpenseTransactionPaymentHistoryId = paymentHistoryId,
                                                BankId = int.Parse(ddBank.Value),
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

                        if (!ServiceProvider.Instance().GetExpenseTransactionPaymentHistoryServices().DeleteExpenseTransactionPaymentHistory(paymentHistoryId))
                        {
                            return false;
                        }
                        
                        return false;
                    }
                       break;

                    default:
                       ConfirmAlertBox1.ShowMessage("The Cheque information could not be submitted. Payment was not made", ConfirmAlertBox.PopupMessageType.Error);
                       break;
                }

                if(paymentHistoryId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("The Transaction Payment Information could not be submitted. Please try again or contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                //Session["_paymentHistoryId"] = paymentHistoryId;
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again or contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }
        }
        private bool ValidateTransactionPaymentControl()
        {
            try
            {
                if (Session["_expenseTransaction"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                var expenseTransaction = Session["_expenseTransaction"] as ExpenseTransaction;

                if (expenseTransaction == null || expenseTransaction.ExpenseTransactionId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                if (int.Parse(btnChequeSubmit.CommandArgument) == 1)
                {
                    if (string.IsNullOrEmpty(txtAmountPaid.Value.Trim()))
                    {
                        ErrorDisplayExpensePayment.ShowError("Please supply the amount to be paid.");
                        txtAmountPaid.Focus();
                        mpeSelectDateRangePopup.Show();
                        return false;
                    }

                    if (txtAmountPaid.Value.Trim().Contains('-') || txtAmountPaid.Value.Trim().Contains('+') || txtAmountPaid.Value.Trim().Contains('*') || txtAmountPaid.Value.Trim().Contains('/'))
                    {
                        ErrorDisplayExpensePayment.ShowError("Invalid entry!");
                        txtAmountPaid.Focus();
                        mpeSelectDateRangePopup.Show();
                        return false;
                    }

                    if (double.Parse(txtAmountPaid.Value.Trim()) > expenseTransaction.TotalApprovedAmount)
                    {
                        ErrorDisplayExpensePayment.ShowError("Please enter an amount less than or equal to the Approved Total Amount.");
                        mpeSelectDateRangePopup.Show();
                        return false;
                    }

                    if (!DataCheck.IsNumeric(txtAmountPaid.Value.Trim()))
                    {
                        ErrorDisplayExpensePayment.ShowError("Invalid entry!");
                        txtAmountPaid.Focus();
                        mpeSelectDateRangePopup.Show();
                        return false;
                    }
                    if (expenseTransaction.Beneficiary.BeneficiaryTypeId != 1)
                    {
                        if (int.Parse(ddlDepartment.SelectedValue) < 1)  
                        {
                            ErrorDisplayExpensePayment.ShowError("Please select a Department.");
                            ddlDepartment.Focus();
                            mpeSelectDateRangePopup.Show();
                            return false;
                        }
                    }
                }

                if (int.Parse(btnChequeSubmit.CommandArgument) == 2)
                {
                    
                    if (string.IsNullOrEmpty(txtChequeAmount.Value.Trim()))
                    {
                        ErrorDispChequePayment.ShowError("Please supply Cheque amount.");
                        txtChequeAmount.Focus();
                        mpeSelectDateRangePopup.Show();
                        return false;
                    }
                    if (string.IsNullOrEmpty(txtChequeComment.Text.Trim()))
                    {
                        ErrorDispChequePayment.ShowError("Please a supply comment.");
                        txtChequeComment.Focus();
                        mpeSelectDateRangePopup.Show();
                        return false;
                    }
                    
                    if (txtChequeAmount.Value.Trim().Contains('-') || txtChequeAmount.Value.Trim().Contains('+') || txtChequeAmount.Value.Trim().Contains('*') || txtChequeAmount.Value.Trim().Contains('/'))
                    {
                        ErrorDispChequePayment.ShowError("Invalid entry!");
                        txtChequeAmount.Focus();
                        mpeSelectDateRangePopup.Show();
                        return false;
                    }

                    if (int.Parse(btnSubmit.CommandArgument) == 1)
                    {
                        if (double.Parse(txtChequeAmount.Value.Trim()) > expenseTransaction.TotalApprovedAmount)
                        {
                            ErrorDispChequePayment.ShowError("Please enter an amount less than or equal to the Transaction Total Amount.");
                            txtChequeAmount.Focus();
                            mpeSelectDateRangePopup.Show();
                            return false;
                        }
                    }

                    if (int.Parse(btnSubmit.CommandArgument) == 2)
                    {
                       if (Session["_expenseTransactionPayment"] == null)
                        {
                            ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                            return false;
                        }

                        var newTransactionPayment = Session["_expenseTransactionPayment"] as ExpenseTransactionPayment;

                        if (newTransactionPayment == null || newTransactionPayment.ExpenseTransactionPaymentId < 1)
                        {
                            ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                            return false;
                        }
 
                        if (double.Parse(txtChequeAmount.Value.Trim()) > newTransactionPayment.Balance)
                        {
                            ErrorDispChequePayment.ShowError("Please enter an amount less than or equal to the Transaction Balance.");
                            txtChequeAmount.Focus();
                            mpeSelectDateRangePopup.Show();
                            return false;
                        }
                    }
                    
                    if (!DataCheck.IsNumeric(txtChequeAmount.Value.Trim()))
                    {
                        ErrorDispChequePayment.ShowError("Invalid entry!");
                        txtAmountPaid.Focus();
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

                    if (int.Parse(ddBank.Value) < 1)
                    {
                        ErrorDispChequePayment.ShowError("Please select a bank.");
                        ddBank.Focus();
                        mpeSelectDateRangePopup.Show();
                        return false;
                    }

                    if (expenseTransaction.Beneficiary.BeneficiaryTypeId != 1)
                    {
                        if (int.Parse(ddlChequeDepartment.SelectedValue) < 1)
                        {
                            ErrorDisplayExpensePayment.ShowError("Please select a Department.");
                            ddlChequeDepartment.Focus();
                            mpeSelectDateRangePopup.Show();
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }

        }
        private void UpdateWithChqueOnly(ExpenseTransactionPayment transactionPayment)
        {
            try
            {
                ErrorDispChequePayment.ClearError();
                if (Session["_expenseTransaction"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);

                    return;
                }

                var expenseTransaction = Session["_expenseTransaction"] as ExpenseTransaction;

                if (expenseTransaction == null || expenseTransaction.ExpenseTransactionId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);

                    return;
                }
                txtChequeApprovedDate.Text = expenseTransaction.DateApproved;
                txtChequeApprovedTime.Text = expenseTransaction.TimeApproved;
                txtChequeApprovedTotalQuantity.Text = expenseTransaction.TotalApprovedQuantity.ToString(CultureInfo.InvariantCulture);
                txtChequeApprovedTotalAmount.Text = transactionPayment.Balance.ToString(CultureInfo.InvariantCulture);
                ddlChequeDepartment.SelectedValue =
                    transactionPayment.DepartmentId.ToString(CultureInfo.InvariantCulture);
                ddlChequeDepartment.Enabled = false;
                //lgChequeUpdate.InnerText = transactionPayment.ExpenseTransaction.ExpenseTitle;
                btnChequeSubmit.Text = "Update";
                iChequeAmountBalance.InnerText = "Old Balance";
                ErrorDispChequePayment.ClearError();
                txtChequeAmount.Value = String.Empty;
                ddBank.SelectedIndex = 0;
                txtChequeComment.Text = string.Empty;
                txtChequNo.Value = String.Empty;
                btnSubmit.CommandArgument = "2";//Updating Payment
                btnChequeSubmit.CommandArgument = "2";//To help determine that payment is done by Cheque
                ErrorDispChequePayment.ClearError();
                mpeSelectDateRangePopup.CancelControlID = btnChequeClose.ID;
                mpeSelectDateRangePopup.PopupControlID = dvChequePayment.ID;
                mpeSelectDateRangePopup.Show();
                Session["_expenseTransactionPayment"] = null;
                Session["_expenseTransactionPayment"] = transactionPayment;
                ErrorDispChequePayment.ShowSuccess("Transaction Payment Information was successfully updated. \nPlease kindly continue the update by providing the needed cheque information, <b>OR</b> click the <b>Close</b> button to make updates later.");
            }
            catch (Exception)
            {
                ConfirmAlertBox1.ShowMessage("Transaction cash Payment Information was successfully updated but an error was encountered while trying to initialize the cheque payment process. \nPlease try again or contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        private void LoadTransactionPaymentFooter()
        {
            try
            {
                if (dgExpenseTransactionPayment.Items.Count > 0)
                {
                    double expAmountPayableTotal = 0;
                    double expAmountPaidTotal = 0;
                    double totalBalance = 0;
                    for (var i = 0; i < dgExpenseTransactionPayment.Items.Count; i++)
                    {
                        expAmountPayableTotal += DataCheck.IsNumeric(((Label)dgExpenseTransactionPayment.Items[i].FindControl("lblTotalAmountPayable")).Text)
                                      ? double.Parse(((Label)dgExpenseTransactionPayment.Items[i].FindControl("lblTotalAmountPayable")).Text)
                                      : 0;
                        expAmountPaidTotal += DataCheck.IsNumeric(((Label)dgExpenseTransactionPayment.Items[i].FindControl("lblAmountPaid")).Text)
                                     ? double.Parse(((Label)dgExpenseTransactionPayment.Items[i].FindControl("lblAmountPaid")).Text)
                                     : 0;
                        totalBalance += DataCheck.IsNumeric(((Label)dgExpenseTransactionPayment.Items[i].FindControl("lblBalance")).Text)
                                     ? double.Parse(((Label)dgExpenseTransactionPayment.Items[i].FindControl("lblBalance")).Text)
                                     : 0;

                    }

                    foreach (var item in dgExpenseTransactionPayment.Controls[0].Controls)
                    {
                        if (item.GetType() == typeof(DataGridItem))
                        {
                            var itmType = ((DataGridItem)item).ItemType;
                            if (itmType == ListItemType.Footer)
                            {
                                if (((DataGridItem)item).FindControl("lblTotalAmountPayableTotal") != null)
                                {
                                    ((Label)((DataGridItem)item).FindControl("lblTotalAmountPayableTotal")).Text = "N" + NumberMap.GroupToDigits(expAmountPayableTotal.ToString(CultureInfo.InvariantCulture));
                                }
                                if (((DataGridItem)item).FindControl("lblAmountPaidTotal") != null)
                                {
                                    ((Label)((DataGridItem)item).FindControl("lblAmountPaidTotal")).Text = "N" + NumberMap.GroupToDigits(expAmountPaidTotal.ToString(CultureInfo.InvariantCulture));
                                }
                                if (((DataGridItem)item).FindControl("lblBalanceTotal") != null)
                                {
                                    ((Label)((DataGridItem)item).FindControl("lblBalanceTotal")).Text = "N" + NumberMap.GroupToDigits(totalBalance.ToString(CultureInfo.InvariantCulture));
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
        private bool ValidateDateControls()
        {
            try
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

                return true;
            }
            catch
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }
        }
        private byte[] FileUploader()
        {

            if (!fldChequeCopy.HasFile)
            {
                ErrorDispChequePayment.ShowError("Select the path to the cheque scanned copy");
                return null;
            }

            var response = UploadManager2.UploadFile(ref fldChequeCopy, UploadManager.ExtensionType.Scanned_Document, false, false);
            if (response == null)
            {
                ErrorDispChequePayment.ShowError("Cheque copy upload failed.");
                return null;
            }
            if (!response.Succeed)
            {
                ErrorDispChequePayment.ShowError(response.Message);
                return null;
            }

            return response.UploadedData;
            
        }
        private void GetAllApprovedUnpaidExpenseTransactions()
        {
            try
            {
                var beneficiaryApprovedUnpaidTransactions = ServiceProvider.Instance().GetExpenseTransactionServices().GetAllApprovedUnpaidExpenseTransactions();

                if (beneficiaryApprovedUnpaidTransactions == null || !beneficiaryApprovedUnpaidTransactions.Any())
                {
                    ConfirmAlertBox1.ShowMessage("No record found.", ConfirmAlertBox.PopupMessageType.Error);
                    ddlExpenseTransactions.DataSource = new List<ExpenseTransaction>();
                    ddlExpenseTransactions.DataBind();
                    ddlExpenseTransactions.Items.Insert(0, new ListItem("-- List is empty --", "0"));
                    ddlExpenseTransactions.SelectedIndex = 0;
                    return;
                }
                ddlExpenseTransactions.DataSource = beneficiaryApprovedUnpaidTransactions;
                ddlExpenseTransactions.DataTextField = "ExpenseTitle";
                ddlExpenseTransactions.DataValueField = "ExpenseTransactionId";
                ddlExpenseTransactions.DataBind();
                ddlExpenseTransactions.Items.Insert(0, new ListItem("-- Select Transaction --", "0"));
                ddlExpenseTransactions.SelectedIndex = 0;
               
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("Unpaid Transaction list could not be retrieved. Please try again soon or contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
              
            }
        }
        #endregion

        #region Pagination
        protected void FirstLink(object sender, EventArgs e)
        {
            try
            {
                var senderLinkArgument = int.Parse(((LinkButton)sender).CommandArgument);
                NowViewing = (int)senderLinkArgument;
                FillRepeater<ExpenseTransactionPayment>(dgExpenseTransactionPayment, "_expTransactionUpdatePaymentList", Navigation.Sorting, Limit, LoadMethod);
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
                FillRepeater<ExpenseTransactionPayment>(dgExpenseTransactionPayment, "_expTransactionUpdatePaymentList", Navigation.Next, Limit, LoadMethod);
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
                FillRepeater<ExpenseTransactionPayment>(dgExpenseTransactionPayment, "_expTransactionUpdatePaymentList", Navigation.First, Limit, LoadMethod);
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
                FillRepeater<ExpenseTransactionPayment>(dgExpenseTransactionPayment, "_expTransactionUpdatePaymentList", Navigation.Last, Limit, LoadMethod);
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
                FillRepeater<ExpenseTransactionPayment>(dgExpenseTransactionPayment, "_expTransactionUpdatePaymentList", Navigation.Previous, Limit, LoadMethod);
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
                FillRepeater<ExpenseTransactionPayment>(dgExpenseTransactionPayment, "_expTransactionUpdatePaymentList", Navigation.None, Limit, LoadMethod);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }
        }
        private void GetPageLimits()
        {
            var intVal = new List<int>();
            for (var i = 1; i <= 20; i++)
            {
                intVal.Add(i);
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