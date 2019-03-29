using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExpenseManager.CoreFramework.AlertControl;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;
using xPlug.BusinessService;

namespace ExpenseManager.ExpenseMgt
{
    public partial class FrmBeneficiaryType : UserControl
    {
        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoaBeneficiaryTypes();

            }

        }
        protected void BtnProcessBeneficiaryTypeClick(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateControls())
                {
                    return;
                }

                switch (int.Parse(btnProcessBeneficiaryType.CommandArgument))
                {
                    case 1:
                        if (!AddBeneficiaryType())
                        {
                            return;
                        }
                        break;

                    case 2:
                        if (!UpdateBeneficiaryType())
                        {
                            return;
                        }

                        break;

                    default:
                        ConfirmAlertBox1.ShowMessage("Invalid process call!", ConfirmAlertBox.PopupMessageType.Error);
                        mpeProcessBeneficiaryType.Show();
                        break;
                }

                if (!LoaBeneficiaryTypes())
                {
                    return;
                }

                ConfirmAlertBox1.ShowSuccessAlert("Beneficiary Type information was processed successfully.");

            }

            catch (Exception ex)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered.Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }

        }
        protected void DgBeneficiaryTypesEditCommand(object source, DataGridCommandEventArgs e)
        {
            txtName.Text = string.Empty;
            try
            {
                dgBeneficiaryTypes.SelectedIndex = e.Item.ItemIndex;

                var id = (DataCheck.IsNumeric(dgBeneficiaryTypes.DataKeys[e.Item.ItemIndex].ToString()))
                             ? int.Parse(dgBeneficiaryTypes.DataKeys[e.Item.ItemIndex].ToString())
                             : 0;

                if (id < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid Selection", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var beneficiaryType = ServiceProvider.Instance().GetBeneficiaryTypeServices().GetBeneficiaryType(id);
                if (beneficiaryType == null)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid selection!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }
                if (beneficiaryType.BeneficiaryTypeId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid selection!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                txtName.Text = beneficiaryType.Name;
                chkBeneficiaryType.Checked = beneficiaryType.Status == 1;
                btnProcessBeneficiaryType.CommandArgument = "2";
                btnProcessBeneficiaryType.Text = "Update";
                mpeProcessBeneficiaryType.Show();
                Session["_beneficiaryType"] = beneficiaryType;
            }

            catch (Exception ex)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }
        }
        protected void BtnAddNewBeneficiaryTypeClick(object sender, EventArgs e)
        {
            ClearControls();
            btnProcessBeneficiaryType.CommandArgument = "1";
            btnProcessBeneficiaryType.Text = "Submit";
            mpeProcessBeneficiaryType.Show();
        }
        #endregion

        #region Page Helpers
        private bool LoaBeneficiaryTypes()
        {
            try
            {
                var beneficiaryTypeList = ServiceProvider.Instance().GetBeneficiaryTypeServices().GetAllBeneficiaryTypes();

                if (beneficiaryTypeList == null || !beneficiaryTypeList.Any())
                {
                    dgBeneficiaryTypes.DataSource = new List<BeneficiaryType>();
                    dgBeneficiaryTypes.DataBind();
                    return false;
                }

                dgBeneficiaryTypes.DataSource = beneficiaryTypeList;
                dgBeneficiaryTypes.DataBind();
                return true;
            }
            catch (Exception ex)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                throw;
            }
        }
        private bool AddBeneficiaryType()
        {
            try
            {

                var newBeneficiaryType = new BeneficiaryType
                                            {
                                                Name = txtName.Text.Trim(),
                                                Status = chkBeneficiaryType.Checked ? 1 : 0
                                            };

                var k = ServiceProvider.Instance().GetBeneficiaryTypeServices().AddBeneficiaryTypeCheckDuplicate(newBeneficiaryType);

                if (k < 1)
                {
                    if (k == -3)
                    {
                        ConfirmAlertBox1.ShowMessage("Beneficiary Type Information already exists.", ConfirmAlertBox.PopupMessageType.Error);
                        return false;
                    }
                    ConfirmAlertBox1.ShowMessage("Beneficiary Type Information could not be added.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                ConfirmAlertBox1.ShowSuccessAlert("Beneficiary Type Information was added to successfully.");
                return true;
            }
            catch (Exception ex)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }

        }
        private bool UpdateBeneficiaryType()
        {
            try
            {
                if (Session["_beneficiaryType"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Beneficiary Type list is empty or session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    mpeProcessBeneficiaryType.Show();
                    return false;
                }

                var beneficiaryType = Session["_beneficiaryType"] as BeneficiaryType;

                if (beneficiaryType == null || beneficiaryType.BeneficiaryTypeId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid selection!", ConfirmAlertBox.PopupMessageType.Error);
                    mpeProcessBeneficiaryType.Show();
                    return false;
                }

                beneficiaryType.Name = txtName.Text.Trim();
                beneficiaryType.Status = chkBeneficiaryType.Checked ? 1 : 0;
                var k = ServiceProvider.Instance().GetBeneficiaryTypeServices().UpdateBeneficiaryTypeCheckDuplicate(beneficiaryType);
                if (k < 1)
                {
                    if (k == -3)
                    {
                        ConfirmAlertBox1.ShowMessage("Beneficiary Type already exists.", ConfirmAlertBox.PopupMessageType.Error);
                        txtName.Focus();
                        mpeProcessBeneficiaryType.Show();
                    }

                    else
                    {
                        ConfirmAlertBox1.ShowMessage("Beneficiary Type information could not be updated.", ConfirmAlertBox.PopupMessageType.Error);
                        mpeProcessBeneficiaryType.Show();
                    }

                }

                return true;
            }
            catch (Exception ex)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }
        private bool ValidateControls()
        {
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                ConfirmAlertBox1.ShowMessage("Please supply an Beneficiary Type.", ConfirmAlertBox.PopupMessageType.Error);
                txtName.Focus();
                mpeProcessBeneficiaryType.Show();
                return false;
            }

            return true;
        }
        private void ClearControls()
        {
            txtName.Text = string.Empty;
           chkBeneficiaryType.Checked = false;
        }
        #endregion
    }
}