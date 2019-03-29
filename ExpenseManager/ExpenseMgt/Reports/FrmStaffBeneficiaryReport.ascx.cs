using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;
using xPlug.BusinessService;

namespace ExpenseManager.ExpenseMgt.Reports
{
    public partial class FrmStaffBeneficiaryReport : UserControl
    {
        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDepartments();
                dgBeneficiaries.DataSource = new List<BeneficiaryService>();
                dgBeneficiaries.DataBind();
            }

        }
       protected void DdlDepartmentIndexChanged(object sender, EventArgs e)
       {
           if(int.Parse(ddlDepartment.SelectedValue) < 1)
           {
               ErrorDisplay1.ShowError("Please select a Department!");
               return;
           }
           var staffBeneficiaries =
               ServiceProvider.Instance().GetStaffBeneficiaryServices().GetOrderedStaffBeneficiariesByDepartmentId(int.Parse(ddlDepartment.SelectedValue));

           if (staffBeneficiaries == null || !staffBeneficiaries.Any())
           {
               ErrorDisplay1.ShowError("No record found!");
               return;
           }

           dgBeneficiaries.DataSource = staffBeneficiaries;
           dgBeneficiaries.DataBind();
       }
       protected void BtnRefreshClick(object sender, EventArgs e)
       {
           if(!LoadBeneficiaries())
           {
               
           }
       }
        #endregion

        #region Page Helper
        private bool LoadBeneficiaries()
        {
            try
            {
                var beneficiariesList = ServiceProvider.Instance().GetStaffBeneficiaryServices().GetStaffBeneficiaries();

                if (beneficiariesList == null || !beneficiariesList.Any())
                {
                    ErrorDisplay1.ShowError("Beneficiary list is empty");
                    return false;

                }

                dgBeneficiaries.DataSource = beneficiariesList;
                Session["_beneficiariesList"] = beneficiariesList;
                dgBeneficiaries.DataBind();
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                return false;
            }
        }
        private void LoadDepartments()
        {
            try
            {
                var beneficiariesList = ServiceProvider.Instance().GetStaffBeneficiaryServices().GetOrderedStaffDepartments();

                if (beneficiariesList == null || !beneficiariesList.Any())
                {
                    ErrorDisplay1.ShowError("Department list is empty");
                    ddlDepartment.DataSource = new List<Department>();
                    ddlDepartment.Items.Insert(0, new ListItem("--List is empty--", "0"));
                    ddlDepartment.SelectedIndex = 0;
                    return;
                }

                ddlDepartment.DataSource = beneficiariesList;
                ddlDepartment.DataTextField = "Name";
                ddlDepartment.DataValueField = "DepartmentId";
                ddlDepartment.DataBind();
                ddlDepartment.Items.Insert(0, new ListItem("--Select Department--", "0"));
                ddlDepartment.SelectedIndex = 0;
                
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                
            }
        }
        #endregion
    }
}