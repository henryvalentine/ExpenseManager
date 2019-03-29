using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SwiftHRMPortal.CoreFramework.AlertControl;
using kPortal.Common.EnumControl.Enums;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;
using xPlug.BusinessService;

namespace ExpenseManager.ExpenseMgt
{
    public partial class FrmManageBeneficiaries : UserControl
    {
        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["_beneficiary"] = null;
                Session["_beneficiariesList"] = null;
                LoadSex();
                if (!LoadBeneficiaries())
                {
                    ConfirmAlertBox1.ShowMessage("Beneficiary list is empty", ConfirmAlertBox.PopupMessageType.Error);
                }

            }

        }
        protected void BtnSubmitClick(object sender, EventArgs e)
        {
            try
            {

                if (!ValidatePage())
                {
                    return;
                }

                switch (int.Parse(btnSubmit.CommandArgument))
                {
                    case 1:
                        if (!AddBeneficiary())
                        {
                            return;
                        }
                        break;

                    case 2:
                        if (!UpdateBeneficiary())
                        {
                            return;
                        }



                        break;

                    default:
                        ConfirmAlertBox1.ShowMessage("Invalid process call!", ConfirmAlertBox.PopupMessageType.Error);
                        mpeSelectDateRangePopup.Show();
                        break;
                }

                if (!LoadBeneficiaries())
                {
                    return;
                }
                ConfirmAlertBox1.ShowSuccessAlert2("The Beneficiary information was processed successfully.");
            }

            catch (Exception ex)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }
        }
        protected void DgBeneficiariesEditCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                if (Session["_beneficiariesList"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Beneficiary list is empty or session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var beneficiariesList = Session["_beneficiariesList"] as List<Beneficiary>;

                if (beneficiariesList == null || !beneficiariesList.Any())
                {
                    ConfirmAlertBox1.ShowMessage("Beneficiary list is empty or session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                dgBeneficiaries.SelectedIndex = e.Item.ItemIndex;

                var id = (DataCheck.IsNumeric(dgBeneficiaries.DataKeys[e.Item.ItemIndex].ToString())) ? long.Parse(dgBeneficiaries.DataKeys[e.Item.ItemIndex].ToString()) : 0;

                if (id < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid Selection", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var beneficiary = beneficiariesList.Find(m => m.BeneficiaryId == id);
                if (beneficiary == null)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid selection!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }
                if (beneficiary.BeneficiaryId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid selection!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                txtFullName.Text = string.Empty;
                txtCompanyName.Text = string.Empty;
                txtPhone1.Text = string.Empty;
                txtPhone2.Text = string.Empty;
                txtEmail.Text = string.Empty;
                ddlSex.SelectedValue = "0";
                chkBeneficiary.Checked = false;

                txtFullName.Text = beneficiary.FullName;
                txtCompanyName.Text = beneficiary.CompanyName;
                txtPhone1.Text = beneficiary.GSMNO1;
                txtPhone2.Text = beneficiary.GSMNO2;
                txtEmail.Text = beneficiary.Email;
                ddlSex.SelectedValue = beneficiary.Sex.ToString(CultureInfo.InvariantCulture);
                chkBeneficiary.Checked = beneficiary.Status == 1;
                btnSubmit.CommandArgument = "2";
                btnSubmit.Text = "Update";
                lgTitle.InnerHtml = "Update Beneficiary";
                mpeSelectDateRangePopup.Show();
                Session["_beneficiary"] = beneficiary;
            }
            catch
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        protected void BtnAddItemClick(object sender, EventArgs e)
        {
            chkBeneficiary.Checked = false;
            ClearControls();
            btnSubmit.CommandArgument = "1";
            btnSubmit.Text = "Submit";
            lgTitle.InnerHtml = "Create a New Beneficiary";
            mpeSelectDateRangePopup.Show();
        }
        protected void BtnSearchClick(object sender, EventArgs e)
        {
            var beneficiaries = ServiceProvider.Instance().GetBeneficiaryServices().GeteBeneficiaryBySearchText(txtSearch.Text.Trim());

            if (beneficiaries == null || !beneficiaries.Any())
            {
                ConfirmAlertBox1.ShowMessage("No record found", ConfirmAlertBox.PopupMessageType.Error);
                return;
            }

            dgBeneficiaries.DataSource = beneficiaries;
            dgBeneficiaries.DataBind();
        }
        #endregion

        #region Page Helpers
        private void LoadSex()
        {
            try
            {
                var status = DataArray.ConvertEnumToArrayList(typeof(Sex));
                ddlSex.DataSource = status;
                ddlSex.DataValueField = "ID";
                ddlSex.DataTextField = "Name";
                ddlSex.DataBind();
                ddlSex.Items.Insert(0, new ListItem(" -- Select Sex -- ", "0"));
                ddlSex.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("Sex could not be loaded", ConfirmAlertBox.PopupMessageType.Error);

            }
        }
        private void LoadBeneficiaryType()
        {
            try
            {
                var beneficiaryTypes =
                    ServiceProvider.Instance().GetBeneficiaryTypeServices().GetActiveBeneficiaryTypes();
                if (beneficiaryTypes == null || !beneficiaryTypes.Any())
                {
                    ddlBeneficiaryType.DataSource = new List<BeneficiaryType>();
                    ddlBeneficiaryType.DataBind();
                    ddlBeneficiaryType.Items.Insert(0, new ListItem(" -- List is empty -- ", "0"));
                    ddlBeneficiaryType.SelectedIndex = 0;
                }

                ddlBeneficiaryType.DataSource = beneficiaryTypes;
                ddlBeneficiaryType.DataValueField = "BeneficiaryTypeId";
                ddlBeneficiaryType.DataTextField = "Name";
                ddlBeneficiaryType.DataBind();
                ddlBeneficiaryType.Items.Insert(0, new ListItem(" -- Select Beneficiary Type -- ", "0"));
                ddlBeneficiaryType.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please contact the Administrator", ConfirmAlertBox.PopupMessageType.Error);

            }
        }
        private bool ValidatePage()
        {
            try
            {
                if (string.IsNullOrEmpty(txtFullName.Text.Trim()))
                {
                    ConfirmAlertBox1.Attributes.CssStyle.Add("z-index", "20000001");
                    ConfirmAlertBox1.ShowMessage("Please supply Beneficiary's Full Name.", ConfirmAlertBox.PopupMessageType.Error);
                    txtFullName.Focus();
                    mpeSelectDateRangePopup.Show();
                    return false;
                }
                if (string.IsNullOrEmpty(txtEmail.Text.Trim()))
                {
                    ConfirmAlertBox1.Attributes.CssStyle.Add("z-index", "20000001");
                    ConfirmAlertBox1.ShowMessage("Please enter Email Address.", ConfirmAlertBox.PopupMessageType.Error);
                    txtEmail.Focus();
                    mpeSelectDateRangePopup.Show();
                    return false;
                }
                if (string.IsNullOrEmpty(txtPhone1.Text.Trim()))
                {
                    ConfirmAlertBox1.Attributes.CssStyle.Add("z-index", "20000001");
                    ConfirmAlertBox1.ShowMessage("Please supply Beneficiary's GSM Phone Number.", ConfirmAlertBox.PopupMessageType.Error);
                    txtPhone1.Focus();
                    mpeSelectDateRangePopup.Show();
                    return false;
                }

                if ((txtPhone1.Text.Trim().Length != 11))
                {
                    ConfirmAlertBox1.Attributes.CssStyle.Add("z-index", "20000001");
                    ConfirmAlertBox1.ShowMessage("Incorrect GSM number format!", ConfirmAlertBox.PopupMessageType.Error);
                    txtPhone1.Focus();
                    mpeSelectDateRangePopup.Show();
                    return false;
                }

                if (int.Parse(ddlSex.SelectedValue) < 1)
                {
                    ConfirmAlertBox1.Attributes.CssStyle.Add("z-index", "20000001");
                    ConfirmAlertBox1.ShowMessage("Invalid Sex option selected.", ConfirmAlertBox.PopupMessageType.Error);
                    ddlSex.Focus();
                    mpeSelectDateRangePopup.Show();
                    return false;
                }

                if (!string.IsNullOrEmpty(txtPhone2.Text.Trim()))
                {
                    if (txtPhone2.Text.Trim().Length != 11)
                    {
                        ConfirmAlertBox1.Attributes.CssStyle.Add("z-index", "20000001");
                        ConfirmAlertBox1.ShowMessage("Incorrect GSM number format!", ConfirmAlertBox.PopupMessageType.Error);
                        txtPhone2.Focus();
                        mpeSelectDateRangePopup.Show();
                        return false;
                    }

                    if (!DataCheck.IsNumeric(txtPhone2.Text.Trim()))
                    {
                        ConfirmAlertBox1.Attributes.CssStyle.Add("z-index", "20000001");
                        ConfirmAlertBox1.ShowMessage("Invalid entry!", ConfirmAlertBox.PopupMessageType.Error);
                        txtPhone2.Focus();
                        mpeSelectDateRangePopup.Show();
                        return false;
                    }
                    return true;
                }



                if (!DataCheck.IsNumeric(txtPhone1.Text.Trim()))
                {
                    ConfirmAlertBox1.Attributes.CssStyle.Add("z-index", "20000001");
                    ConfirmAlertBox1.ShowMessage("Invalid entry!", ConfirmAlertBox.PopupMessageType.Error);
                    txtPhone1.Focus();
                    mpeSelectDateRangePopup.Show();
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }

        }
        private bool AddBeneficiary()
        {
            try
            {
                var newBeneficiary = new Beneficiary
                {
                    FullName = txtFullName.Text.Trim(),
                    CompanyName = txtCompanyName.Text.Trim(),
                    GSMNO1 = txtPhone1.Text,
                    GSMNO2 = txtPhone2.Text,
                    Email = txtEmail.Text,
                    Sex = int.Parse(ddlSex.SelectedValue),
                    Status = chkBeneficiary.Checked ? 1 : 0,
                    DateRegistered = DateMap.GetLocalDate(),
                    TimeRegistered = DateMap.GetLocalTime()

                };

                var k = ServiceProvider.Instance().GetBeneficiaryServices().AddBeneficiaryCheckDuplicate(newBeneficiary);
                if (k < 1)
                {
                    if (k == -3)
                    {
                        ConfirmAlertBox1.Attributes.CssStyle.Add("z-index", "20000001");
                        ConfirmAlertBox1.ShowMessage("The Beneficiary information already exists.", ConfirmAlertBox.PopupMessageType.Error);
                        mpeSelectDateRangePopup.Show();
                        return false;
                    }

                    if (k == -4)
                    {
                        ConfirmAlertBox1.Attributes.CssStyle.Add("z-index", "20000001");
                        ConfirmAlertBox1.ShowMessage("Another Beneficiary with similar email already exists.", ConfirmAlertBox.PopupMessageType.Error);
                        mpeSelectDateRangePopup.Show();
                        return false;
                    }
                    if (k == -5)
                    {
                        ConfirmAlertBox1.Attributes.CssStyle.Add("z-index", "20000001");
                        ConfirmAlertBox1.ShowMessage("Another Beneficiary with similar phone number already exists.", ConfirmAlertBox.PopupMessageType.Error);
                        mpeSelectDateRangePopup.Show();
                        return false;
                    }
                    //if (k == -6)
                    //{
                    //    ConfirmAlertBox1.Attributes.CssStyle.Add("z-index", "20000001");
                    //    ConfirmAlertBox1.ShowMessage("Another Beneficiary with similar second phone number already exists.", ConfirmAlertBox.PopupMessageType.Error);
                    //    mpeSelectDateRangePopup.Show();
                    //    return false;
                    //}

                    ConfirmAlertBox1.Attributes.CssStyle.Add("z-index", "20000001");
                    ConfirmAlertBox1.ShowMessage("The Beneficiary Information could not be Added.", ConfirmAlertBox.PopupMessageType.Error);
                    mpeSelectDateRangePopup.Show();
                    return false;
                }
                ClearControls();
                return true;
            }


            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }


        }
        private bool UpdateBeneficiary()
        {
            try
            {
                if (Session["_beneficiary"] == null)
                {
                    ConfirmAlertBox1.Attributes.CssStyle.Add("z-index", "20000001");
                    ConfirmAlertBox1.ShowMessage("Beneficiary list is empty or session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    mpeSelectDateRangePopup.Show();
                    return false;
                }

                var beneficiary = Session["_beneficiary"] as Beneficiary;
                if (beneficiary == null || beneficiary.BeneficiaryId < 1)
                {
                    ConfirmAlertBox1.Attributes.CssStyle.Add("z-index", "20000001");
                    ConfirmAlertBox1.ShowMessage("Invalid selection!", ConfirmAlertBox.PopupMessageType.Error);
                    mpeSelectDateRangePopup.Show();
                    return false;
                }

                beneficiary.FullName = txtFullName.Text.Trim();
                beneficiary.CompanyName = txtCompanyName.Text.Trim();
                beneficiary.GSMNO1 = txtPhone1.Text.Trim();
                beneficiary.GSMNO2 = txtPhone2.Text.Trim();
                beneficiary.Email = txtEmail.Text.Trim();
                beneficiary.Sex = int.Parse(ddlSex.SelectedValue);
                beneficiary.Status = chkBeneficiary.Checked ? 1 : 0;
                var k = ServiceProvider.Instance().GetBeneficiaryServices().UpdateBeneficiaryCheckDuplicate(beneficiary);
                if (k < 1)
                {
                    if (k == -3)
                    {
                        ConfirmAlertBox1.Attributes.CssStyle.Add("z-index", "20000001");
                        ConfirmAlertBox1.ShowMessage("The Beneficiary information already exists.", ConfirmAlertBox.PopupMessageType.Error);
                        mpeSelectDateRangePopup.Show();
                        return false;
                    }
                    if (k == -4)
                    {
                        ConfirmAlertBox1.Attributes.CssStyle.Add("z-index", "20000001");
                        ConfirmAlertBox1.ShowMessage("Another Beneficiary with similar email already exists.", ConfirmAlertBox.PopupMessageType.Error);
                        mpeSelectDateRangePopup.Show();
                        return false;
                    }

                    if (k == -5)
                    {
                        ConfirmAlertBox1.Attributes.CssStyle.Add("z-index", "20000001");
                        ConfirmAlertBox1.ShowMessage("Another Beneficiary with similar phone number already exists.", ConfirmAlertBox.PopupMessageType.Error);
                        mpeSelectDateRangePopup.Show();
                        return false;
                    }

                    //if (k == -6)
                    //{
                    //    ConfirmAlertBox1.Attributes.CssStyle.Add("z-index", "20000001");
                    //    ConfirmAlertBox1.ShowMessage("Another Beneficiary with similar phone number 2 already exists.", ConfirmAlertBox.PopupMessageType.Error);
                    //    mpeSelectDateRangePopup.Show();
                    //    return false;
                    //}

                    ConfirmAlertBox1.Attributes.CssStyle.Add("z-index", "20000001");
                    ConfirmAlertBox1.ShowMessage("The Beneficiary Information could not be modified!", ConfirmAlertBox.PopupMessageType.Error);
                    mpeSelectDateRangePopup.Show();
                    return false;

                }
                txtFullName.Text = string.Empty;
                txtCompanyName.Text = string.Empty;
                txtPhone1.Text = string.Empty;
                txtPhone2.Text = string.Empty;
                txtEmail.Text = string.Empty;
                ddlSex.SelectedValue = "0";
                chkBeneficiary.Checked = false;
                return true;

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }
        }
        private bool LoadBeneficiaries()
        {
            try
            {
                var beneficiariesList = ServiceProvider.Instance().GetBeneficiaryServices().GetAllBeneficiaries();

                if (beneficiariesList == null || !beneficiariesList.Any())
                {
                    ConfirmAlertBox1.ShowMessage("Beneficiary list is empty", ConfirmAlertBox.PopupMessageType.Error);
                    dgBeneficiaries.DataSource = new List<Beneficiary>();
                    dgBeneficiaries.DataBind();
                    return false;

                }

                dgBeneficiaries.DataSource = beneficiariesList;
                dgBeneficiaries.DataBind();
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }
        }
        private void ClearControls()
        {
            txtFullName.Text = string.Empty;
            txtCompanyName.Text = string.Empty;
            txtPhone1.Text = string.Empty;
            txtPhone1.Text = string.Empty;
            txtPhone2.Text = string.Empty;
            chkBeneficiary.Checked = false;
            txtEmail.Text = string.Empty;
            ddlSex.SelectedIndex = 0;
            ddlDepartment.SelectedIndex = 0;
            ddlUnit.SelectedIndex = 0;
            ddlBeneficiaryType.SelectedIndex = 0;
        }
        #endregion
    }
}