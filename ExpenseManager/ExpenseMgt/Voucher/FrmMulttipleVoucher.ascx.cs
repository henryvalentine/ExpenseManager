using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExpenseManager.CoreFramework.AlertControl;
using xPlug.BusinessObject;
using xPlug.BusinessObject.CustomizedASPBusinessObject.Enum;
using xPlug.BusinessService;
using XPLUG.WEBTOOLS;

namespace ExpenseManager.ExpenseMgt.Reports
{
    public partial class FrmMulttipleVoucher : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dgVouchers.DataSource = new List<ExpenseTransactionPaymentHistory>();
                dgVouchers.DataBind();
                LoadFilterOptions();
                LoadDepartments();
            }
        }

        private void LoadFilterOptions()
        {
            var options = DataArray.ConvertEnumToArrayList(typeof(ApprovedVoidedFilterOptions));
            ddlMultiVoucherFilterOption.DataSource = options;
            ddlMultiVoucherFilterOption.DataValueField = "ID";
            ddlMultiVoucherFilterOption.DataTextField = "Name";
            ddlMultiVoucherFilterOption.DataBind();
            ddlMultiVoucherFilterOption.Items.Insert(0, new ListItem(" -- Select Status -- ", "-1"));
            ddlMultiVoucherFilterOption.Items.Insert(1, new ListItem("All", "0"));
            ddlMultiVoucherFilterOption.SelectedIndex = -1;
        }

        protected void BtnMultiVoucherDateFilterClick(object sender, EventArgs e)
        {
            var option = int.Parse(ddlMultiVoucherFilterOption.SelectedValue);

            if (option < 0)
            {
                ConfirmAlertBox1.ShowMessage("Please select a filter Status.", ConfirmAlertBox.PopupMessageType.Error);
                return;
            }

            var startDate = txtMultiVoucherStartDate.Text.Trim();
            var endDate = txtMultiVoucherEndDate.Text.Trim();

            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
            {
                ConfirmAlertBox1.ShowMessage("Please enter a date range.", ConfirmAlertBox.PopupMessageType.Error);
                return;
            }

            if (!ValidateVouchersByDateArguments(startDate, endDate))
            {
                return;
            }

            if (!GetTVouchersByDateRange(startDate, endDate))
            {
                dgVouchers.DataSource = new List<ExpenseTransactionPaymentHistory>();
                dgVouchers.DataBind();
            }
          
        }

        private bool GetTVouchersByDateRange(string startDate, string endDate)
        {
            try
            {
                var status = int.Parse(ddlMultiVoucherFilterOption.SelectedValue);
                
                if (status == 0)
                {
                    if (GetTransactionPamentsByDateRange(startDate, endDate)) return true;
                    {
                        return false;
                    }
                }

                if (status == 1)
                {
                    return GetApprovedTransactionPaymentVouchersByDateRange(startDate, endDate);
                }

                if (status == 2)
                {
                    return GetVoidedTransactionPaymentVouchersByDateRange(startDate, endDate);
                }

                return false;
            }
            catch (Exception)
            {
               return false;
            }
        }

        public bool GetTransactionPamentsByDateRange(string startDate, string endDate)
        {
            if (!ValidateVouchersByDateArguments(startDate, endDate))
            {
                return false;
            }
            
            try
            {
                var start = DateTime.Parse(DateMap.ReverseToServerDate(startDate.Trim()));
                var end = DateTime.Parse(DateMap.ReverseToServerDate(endDate.Trim()));
                var dept = int.Parse(ddlDepartmentVoucher.SelectedValue);
                if (dept < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Please select a Department!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }
                if (end < start || start > end)
                {
                    ConfirmAlertBox1.ShowMessage("The START date must be earlier than the END DATE!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                var paymentHistoryList =
                    ServiceProvider.Instance()
                        .GetExpenseTransactionPaymentHistoryServices()
                        .GetTransactionPaymentsByDateRange(start, end, dept);
                
                if (paymentHistoryList == null || !paymentHistoryList.Any())
                {
                    ConfirmAlertBox1.ShowMessage("No record found!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                dgVouchers.DataSource = paymentHistoryList;
                dgVouchers.DataBind();
                return true;
            }
            
            catch (Exception ex)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again or contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }

        }
       
        public bool GetApprovedTransactionPaymentVouchersByDateRange(string startDate, string endDate)
        {
            if (!ValidateVouchersByDateArguments(startDate, endDate))
            {
                return false;
            }
            
            try
            {
                var start = DateTime.Parse(DateMap.ReverseToServerDate(startDate.Trim()));
                var end = DateTime.Parse(DateMap.ReverseToServerDate(endDate.Trim()));
                var dept = int.Parse(ddlDepartmentVoucher.SelectedValue);
                if (dept < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Please select a Department!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                if (end < start || start > end)
                {
                    ConfirmAlertBox1.ShowMessage("The START date must be earlier than the END DATE!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                var paymenthistoryObjList =
                    ServiceProvider.Instance()
                        .GetExpenseTransactionPaymentHistoryServices()
                        .GetApprovedTransactionPaymentsByDateRange(start, end, dept);
               
                if (paymenthistoryObjList == null || !paymenthistoryObjList.Any())
                {
                    ConfirmAlertBox1.ShowMessage("No record found!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                dgVouchers.DataSource = paymenthistoryObjList;
                dgVouchers.DataBind();
                return true;
            }
            catch (Exception ex)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again or contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }

        }

        public bool GetVoidedTransactionPaymentVouchersByDateRange(string startDate, string endDate)
        {
            if (!ValidateVouchersByDateArguments(startDate, endDate))
            {
                return false;
            }

            try
            {
                var start = DateTime.Parse(DateMap.ReverseToServerDate(startDate.Trim()));
                var end = DateTime.Parse(DateMap.ReverseToServerDate(endDate.Trim()));
                var dept = int.Parse(ddlDepartmentVoucher.SelectedValue);
                if (dept < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Please select a Department!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }
                if (end < start || start > end)
                {
                    ConfirmAlertBox1.ShowMessage("The START date must be earlier than the END DATE!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                var paymentHistoryObjList =
                    ServiceProvider.Instance()
                        .GetExpenseTransactionPaymentHistoryServices()
                        .GetVoidedTransactionPaymentsByDateRange(start, end, dept);
               
                if (paymentHistoryObjList == null || !paymentHistoryObjList.Any())
                {
                    ConfirmAlertBox1.ShowMessage("No record found!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                dgVouchers.DataSource = paymentHistoryObjList;
                dgVouchers.DataBind();
                return true;
            }
            catch (Exception ex)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again or contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
                ErrorManager.LogApplicationError(ex.StackTrace,ex.Source,ex.Message);
                return false;
            }

        }

        private bool ValidateVouchersByDateArguments(string startDate, string endDate)
        {
            try
            {
                if (string.IsNullOrEmpty(startDate) && string.IsNullOrEmpty(endDate))
                {
                    ConfirmAlertBox1.ShowMessage("Please enter a date range.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(startDate))
                {
                    ConfirmAlertBox1.ShowMessage("Please enter a date range.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                if (!string.IsNullOrEmpty(startDate))
                {
                    if (!DateMap.IsDate(DateMap.ReverseToServerDate(startDate)))
                    {
                        ConfirmAlertBox1.ShowMessage("Please enter a valid date.", ConfirmAlertBox.PopupMessageType.Error);
                        return false;
                    }
                }


                if (!string.IsNullOrEmpty(DateMap.ReverseToServerDate(endDate)))
                {
                    if (!DateMap.IsDate(DateMap.ReverseToServerDate(endDate)))
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

        private void LoadDepartments()
        {
            try
            {
                var departmentList = ServiceProvider.Instance().GetDepartmentServices().GetActiveOrderedDepartments();

                if (departmentList == null || !departmentList.Any())
                {
                    ddlDepartmentVoucher.DataSource = new List<Department>();
                    ddlDepartmentVoucher.Items.Insert(0, new ListItem("--List is empty--", "0"));
                    ddlDepartmentVoucher.SelectedIndex = 0;
                    return;
                }

                ddlDepartmentVoucher.DataSource = departmentList;
                ddlDepartmentVoucher.DataTextField = "Name";
                ddlDepartmentVoucher.DataValueField = "DepartmentId";
                ddlDepartmentVoucher.DataBind();
                ddlDepartmentVoucher.Items.Insert(0, new ListItem("--Select Department--", "0"));
                ddlDepartmentVoucher.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }

        }

    }
}