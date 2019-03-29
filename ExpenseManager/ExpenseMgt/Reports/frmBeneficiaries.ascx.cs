using System;
using System.Collections.Generic;
using System.Linq;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;
using xPlug.BusinessService;

namespace ExpenseManager.ExpenseMgt.Reports
{
    public partial class FrmBeneficiaries : System.Web.UI.UserControl
    {
        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dgBeneficiaries.DataSource = new List<Beneficiary>();
                dgBeneficiaries.DataBind();
            }

        }
        protected void BtnSearchClick(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            var beneficiaries = ServiceProvider.Instance().GetBeneficiaryServices().GeteBeneficiaryBySearchText(txtSearch.Text.Trim());
           
            if(beneficiaries == null || !beneficiaries.Any())
            {
                ErrorDisplay1.ShowError("No record found");
                return;
            }

            dgBeneficiaries.DataSource = beneficiaries;
            dgBeneficiaries.DataBind();
        }
       protected void BtnRefreshClick(object sender, EventArgs e)
       {
           ErrorDisplay1.ClearError();
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
                var beneficiariesList = ServiceProvider.Instance().GetBeneficiaryServices().GetBeneficiaries();

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
        #endregion
    }
}