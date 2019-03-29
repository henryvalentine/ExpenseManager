using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExpenseManager.CoreFramework.AlertControl;
using xPlug.BusinessObject;
using xPlug.BusinessService;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject.CustomizedASPBusinessObject.Enum;

namespace ExpenseManager.ExpenseMgt.Reports
{
    public partial class FrmVouchers : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                LoadFilterOptions();
                LoadDepartments();
            }
        }

        private void LoadFilterOptions()
        {
            var options = DataArray.ConvertEnumToArrayList(typeof(ApprovedVoidedFilterOptions));
            ddlVoucherFilterOption.DataSource = options;
            ddlVoucherFilterOption.DataValueField = "ID";
            ddlVoucherFilterOption.DataTextField = "Name";
            ddlVoucherFilterOption.DataBind();
            ddlVoucherFilterOption.Items.Insert(0, new ListItem(" -- Select Status -- ", "-1"));
            ddlVoucherFilterOption.Items.Insert(1, new ListItem("All", "0"));
            ddlVoucherFilterOption.SelectedIndex = -1;
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