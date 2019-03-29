using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExpenseManager.CoreFramework;
using kPortal.Common.EnumControl.Enums;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;
using xPlug.BusinessObject.CustomizedASPBusinessObject.Enum;
using xPlug.BusinessService;

namespace ExpenseManager.ExpenseMgt
{
    public partial class FrmManageBeneficiary : UserControl
    {
        protected int Offset = 5;
        protected int DataCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Limit = 20;
                GetPageLimits();
                ddlLimit.SelectedValue = "20";
                Session["_beneficiary"] = null;
                LoadSex();
                LoadBeneficiaryTypes();
                LoadBeneficiaries();
            }

        }

        #region Create Beneficiary
        #region Events
        protected void BtnSubmitClick(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            try
            {
                ErrorDisplayBeneficiary.ClearError();
                if (!ValidateBeneficiaryRegistrationControls())
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
                        ErrorDisplay1.ShowSuccess("Beneficiary information was added successfully."); 
                       break;

                    case 2:
                        if (!UpdateBeneficiary())
                        {
                            return;
                        }
                        ErrorDisplay1.ShowSuccess("The Beneficiary information was successfully Updated."); 
                        break;
                    default:
                        ErrorDisplay1.ShowError("Invalid process call!");
                        mpeRegisterBeneficiaryInfo.Show();
                        break;
                }

                LoadBeneficiaries();

            }

            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
            }
        }
        protected void DgBeneficiariesCommand(object source, DataGridCommandEventArgs e)
        {
            ClearControls();
            ErrorDisplay1.ClearError();
            ErrorDisplayPortalUser.ClearError();
            try
            {
                dgBeneficiaries.SelectedIndex = e.Item.ItemIndex;

                var id = (DataCheck.IsNumeric(dgBeneficiaries.DataKeys[e.Item.ItemIndex].ToString())) ? int.Parse(dgBeneficiaries.DataKeys[e.Item.ItemIndex].ToString()) : 0;

                if (id < 1)
                {
                    ErrorDisplay1.ShowError("Invalid Selection");
                    return;
                }

                var beneficiary = ServiceProvider.Instance().GetBeneficiaryServices().GetBeneficiary(id);

                if (beneficiary == null || beneficiary.BeneficiaryId < 1)
                {
                    ErrorDisplay1.ShowError("Invalid selection!");
                    return;
                }

                if (e.CommandName == "Edit")
                {
                    txtFullName.Text = beneficiary.FullName;
                    txtPhone1.Text = beneficiary.GSMNO1;
                    txtPhone2.Text = beneficiary.GSMNO2;
                    ddlBeneficiaryType.SelectedValue =
                        beneficiary.BeneficiaryTypeId.ToString(CultureInfo.InvariantCulture);
                    txtEmail.Text = beneficiary.Email;
                    ddlSex.SelectedValue = beneficiary.Sex.ToString(CultureInfo.InvariantCulture);
                    chkBeneficiary.Checked = beneficiary.Status == 1;
                    btnSubmit.CommandArgument = "2";
                    btnSubmit.Text = "Update";
                    lgTitle.InnerHtml = "Update Beneficiary";
                    if (int.Parse(ddlBeneficiaryType.SelectedValue) > 0 && int.Parse(ddlBeneficiaryType.SelectedValue) != 1)
                    {
                        ReqDepartment.Enabled = false;
                        CompareValDepartment.Enabled = false;
                        txtCompanyName.Enabled = true;
                        txtCompanyName.Text = beneficiary.CompanyName;
                        ReqtxtCompanyName.Enabled = true;
                        ReqUnit.Enabled = false;
                        CompareValUnit.Enabled = false;
                        //ddlDepartment.SelectedValue = "8";
                        //ddlUnit.SelectedValue = "14";
                        ddlDepartment.Enabled = false;
                        ddlUnit.Enabled = false;
                        mpeRegisterBeneficiaryInfo.Show();
                    }

                    if (int.Parse(ddlBeneficiaryType.SelectedValue) == 1)
                    {
                        ccdDepartment.SelectedValue = beneficiary.DepartmentId.ToString(CultureInfo.InvariantCulture);
                        ccdUnit.SelectedValue = beneficiary.UnitId.ToString(CultureInfo.InvariantCulture);
                        txtCompanyName.Enabled = false;
                        ReqtxtCompanyName.Enabled = false;
                        ReqDepartment.Enabled = true;
                        CompareValDepartment.Enabled = true;
                        ReqUnit.Enabled = true;
                        CompareValUnit.Enabled = true;
                        ddlDepartment.Enabled = true;
                        ddlUnit.Enabled = true;
                        mpeRegisterBeneficiaryInfo.Show();
                    }
                    mpeRegisterBeneficiaryInfo.PopupControlID = "dvNewBeneficiary";
                    mpeRegisterBeneficiaryInfo.CancelControlID = "btnClose";
                    mpeRegisterBeneficiaryInfo.Show();
                    Session["_beneficiary"] = beneficiary;
                }

                if(e.CommandName == "Register")
                {
                    if (beneficiary.BeneficiaryTypeId == 2)
                    {
                        ErrorDisplay1.ShowError("Only Staff Members can be registered as users!");
                        return;
                    }

                    var staffUser = ServiceProvider.Instance().GetStaffUserServices().GetStaffUsersByBeneficiaryId(beneficiary.BeneficiaryId);

                    if(staffUser.Any())
                    {
                        ErrorDisplay1.ShowError("Beneficiary is already a Portal User.");
                        return;
                    }

                    var userName = Membership.GetUserNameByEmail(beneficiary.Email);

                      if(!string.IsNullOrEmpty(userName))
                      {
                          var portalUserId = new PortalServiceManager().GetPortalUserIdByUserName(userName);

                          if (portalUserId > 1)
                          {
                              mpeRegisterBeneficiaryInfo.CancelControlID = "btnNO";
                              mpeRegisterBeneficiaryInfo.PopupControlID = "dvConfirmation";
                              lblMessage.InnerHtml = "<br/>This Beneficiary already has a Portal account. Add to User list?";
                              mpeRegisterBeneficiaryInfo.Show();
                              Session["_portalUserId"] = portalUserId;
                              Session["_beneficiaryId"] = beneficiary.BeneficiaryId;
                              return;
                          }
                      }
                  
                    txtPassword.Text = string.Empty;
                    txtConfirmPassword.Text = string.Empty;
                    txtUserName.Text = string.Empty;
                    txtFirstName.Text = string.Empty;
                    txtLastName.Text = string.Empty;
                    ddlPortalUserSex.SelectedValue = beneficiary.Sex.ToString(CultureInfo.InvariantCulture);
                    txtMobileNumber.Text = beneficiary.GSMNO1;
                    txtPortalUserEmail.Text = beneficiary.Email;
                    BindRoleList();
                    mpeRegisterBeneficiaryInfo.PopupControlID = "detailDiv";
                    mpeRegisterBeneficiaryInfo.CancelControlID = "btnCancelProfile";
                    mpeRegisterBeneficiaryInfo.Show();
                    Session["_beneficiaryId"] = beneficiary.BeneficiaryId;
                }
            }
            catch
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
            }
        }
        protected void BtnAddNewBeneficiaryClick(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            ErrorDisplayBeneficiary.ClearError();
            ClearControls();
            txtCompanyName.Text = string.Empty;
            txtPhone1.Text = string.Empty;
            btnSubmit.CommandArgument = "1";
            btnSubmit.Text = "Submit";
            lgTitle.InnerHtml = "Create a New Beneficiary";
            mpeRegisterBeneficiaryInfo.PopupControlID = "dvNewBeneficiary";
            mpeRegisterBeneficiaryInfo.CancelControlID = "btnClose";
            mpeRegisterBeneficiaryInfo.Show();
        }
        protected void DdlBeneficiaryTypeIndexChanged(object sender, EventArgs e)
        {
            ErrorDisplayBeneficiary.ClearError();
            ErrorDisplay1.ClearError();
            if (int.Parse(ddlBeneficiaryType.SelectedValue) < 1)
            {
                ddlDepartment.Enabled = false;
                ddlUnit.Enabled = false;
                ErrorDisplay1.ShowError("Please select Beneficiary Type.");
                ddlBeneficiaryType.Focus();
                ddlDepartment.Enabled = false;
                ddlUnit.Enabled = false;
                mpeRegisterBeneficiaryInfo.Show();
                return;
            }

            if (int.Parse(ddlBeneficiaryType.SelectedValue) == 1)
            {
                ReqDepartment.Enabled = true;
                CompareValDepartment.Enabled = true;
                ReqUnit.Enabled = true;
                txtCompanyName.Enabled = false;
                ReqtxtCompanyName.Enabled = false;
                CompareValUnit.Enabled = true;
                ddlDepartment.Enabled = true;
                ddlUnit.Enabled = true;
                mpeRegisterBeneficiaryInfo.Show();
            }

            else
            {
                ReqDepartment.Enabled = false;
                CompareValDepartment.Enabled = false;
                ReqUnit.Enabled = false;
                CompareValUnit.Enabled = false;
                txtCompanyName.Enabled = true;
                ReqtxtCompanyName.Enabled = true;
                //ddlDepartment.SelectedValue = "8";
                //ddlUnit.SelectedValue = "14";
                ddlDepartment.Enabled = false;
                ddlUnit.Enabled = false;
                mpeRegisterBeneficiaryInfo.Show();
            }
        }
        protected void BtnOkClick(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            ErrorDisplayBeneficiary.ClearError();
            if(Session["_portalUserId"] == null || Session["_beneficiaryId"] == null)
            {
                ErrorDisplay1.ShowError("Session has expired.");
                return;
            }

           var portalUserId = (long)Session["_portalUserId"];
           var beneficiaryId = (int)Session["_beneficiaryId"];

           if (portalUserId < 1 || beneficiaryId < 1)
           {
               ErrorDisplay1.ShowError("Session has expired.");
               return;
           }

            var newStaffUser = new StaffUser
                                {
                                    BeneficiaryId = beneficiaryId,
                                    PortalUserId = portalUserId,
                                    Status = 1
                                };

            var k2 = ServiceProvider.Instance().GetStaffUserServices().AddStaffUser(newStaffUser);

            if (k2 < 1)
            {
                ErrorDisplay1.ShowError("Beneficiary could not be added to the Staff User list.\n Please try again soon or contact the Admin.");
                return;
            }

            ErrorDisplay1.ShowSuccess("Beneficiary was successfully created as a User.");                   

        }
        #endregion

        #region Page Helpers
        private void LoadSex()
        {
            ErrorDisplay1.ClearError();
            ErrorDisplayPortalUser.ClearError();
            try
            {
                var status = DataArray.ConvertEnumToArrayList(typeof(Sex));
                ddlSex.DataSource = status;
                ddlSex.DataValueField = "ID";
                ddlSex.DataTextField = "Name";
                ddlSex.DataBind();
                ddlSex.Items.Insert(0, new ListItem(" -- Select Sex -- ", "0"));
                ddlSex.SelectedIndex = 0;

                ddlPortalUserSex.DataSource = status;
                ddlPortalUserSex.DataValueField = "ID";
                ddlPortalUserSex.DataTextField = "Name";
                ddlPortalUserSex.DataBind();
                ddlPortalUserSex.Items.Insert(0, new ListItem(" -- Select Sex -- ", "0"));
                ddlPortalUserSex.SelectedIndex = 0;
                
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay1.ShowError("Sex could not be loaded");
               
            }
        }
        private void LoadBeneficiaryTypes()
        {
            ErrorDisplay1.ClearError();
            ErrorDisplayPortalUser.ClearError();
            try
            {
                var beneficiaryTypeList = ServiceProvider.Instance().GetBeneficiaryTypeServices().GetActiveBeneficiaryTypes();

                if (beneficiaryTypeList == null || !beneficiaryTypeList.Any())
                {
                    ddlBeneficiaryType.DataSource = new List<BeneficiaryType>();
                    ddlBeneficiaryType.DataBind();
                    ddlBeneficiaryType.Items.Insert(0, new ListItem(" -- List is empty -- ", "0"));
                    ddlBeneficiaryType.SelectedIndex = 0;
                    return;
                }

                ddlBeneficiaryType.DataSource = beneficiaryTypeList;
                ddlBeneficiaryType.DataValueField = "BeneficiaryTypeId";
                ddlBeneficiaryType.DataTextField = "Name";
                ddlBeneficiaryType.DataBind();
                ddlBeneficiaryType.Items.Insert(0, new ListItem(" -- Select Beneficiary Type -- ", "0"));
                ddlBeneficiaryType.SelectedIndex = 0;
               
            }
            catch (Exception ex)
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                throw;
            }
        }
        private bool ValidateBeneficiaryRegistrationControls()
        {
            ErrorDisplay1.ClearError();
            ErrorDisplayBeneficiary.ClearError();
            try
            {
                if (int.Parse(ddlBeneficiaryType.SelectedValue) < 1)
                {
                    ErrorDisplayBeneficiary.ShowError("Please select Beneficiary Type.");
                    ddlBeneficiaryType.Focus();
                    mpeRegisterBeneficiaryInfo.Show();
                    return false;
                }

                if (string.IsNullOrEmpty(txtFullName.Text.Trim()))
                {
                    ErrorDisplayBeneficiary.ShowError("Please supply Beneficiary's Full Name.");
                    txtFullName.Focus();
                    mpeRegisterBeneficiaryInfo.Show();
                    return false;
                }
                
                if (string.IsNullOrEmpty(txtEmail.Text.Trim()))
                {
                    ErrorDisplayBeneficiary.ShowError("Please enter Email Address.");
                    txtEmail.Focus();
                    mpeRegisterBeneficiaryInfo.Show();
                    return false;
                }
                if (string.IsNullOrEmpty(txtPhone1.Text.Trim()))
                {
                    ErrorDisplayBeneficiary.ShowError("Please supply Beneficiary's GSM Phone Number.");
                    txtPhone1.Focus();
                    mpeRegisterBeneficiaryInfo.Show();
                    return false;
                }

                if (DataCheck.IsNumeric(txtFullName.Text.Trim()))
                {
                    ErrorDisplayBeneficiary.ShowError("Invalid name supplied!");
                    txtFullName.Focus();
                    mpeRegisterBeneficiaryInfo.Show();
                    return false;
                }

                if (!DataCheck.IsNumeric(txtPhone1.Text.Trim()))
                {
                    ErrorDisplayBeneficiary.ShowError("Invalid GSM number format!");
                    txtPhone1.Focus();
                    mpeRegisterBeneficiaryInfo.Show();
                    return false;
                }

                if ((txtPhone1.Text.Trim().Length != 11))
                {
                    ErrorDisplayBeneficiary.ShowError("Incomplete GSM number!");
                    txtPhone1.Focus();
                    mpeRegisterBeneficiaryInfo.Show();
                    return false;
                }

                if (!string.IsNullOrEmpty(txtPhone2.Text.Trim()))
                {
                    if (txtPhone2.Text.Trim().Length != 11)
                    {
                        ErrorDisplayBeneficiary.ShowError("Incorrect GSM number format!");
                        txtPhone2.Focus();
                        mpeRegisterBeneficiaryInfo.Show();
                        return false;
                    }

                    if (!DataCheck.IsNumeric(txtPhone2.Text.Trim()))
                    {
                        ErrorDisplayBeneficiary.ShowError("Invalid entry!");
                        txtPhone2.Focus();
                        mpeRegisterBeneficiaryInfo.Show();
                        return false;
                    }
                    return true;
                }

                if (int.Parse(ddlSex.SelectedValue) < 1)
                {
                    ErrorDisplayBeneficiary.ShowError("Invalid Sex option selected.");
                    ddlSex.Focus();
                    mpeRegisterBeneficiaryInfo.Show();
                    return false;
                }

                if (int.Parse(ddlBeneficiaryType.SelectedValue) == 1)
                {
                    if (int.Parse(ddlDepartment.SelectedValue) < 1)
                    {
                        ErrorDisplayBeneficiary.ShowError("Invalid Department selected!");
                        ddlDepartment.Focus();
                        mpeRegisterBeneficiaryInfo.Show();
                        return false;
                    }

                    if (int.Parse(ddlUnit.SelectedValue) < 1)
                    {
                        ErrorDisplayBeneficiary.ShowError("Invalid Unit selected!");
                        ddlUnit.Focus();
                        mpeRegisterBeneficiaryInfo.Show();
                        return false;
                    }
                }

                if (int.Parse(ddlBeneficiaryType.SelectedValue) > 0 && int.Parse(ddlBeneficiaryType.SelectedValue) != 1)
                {
                    if(string.IsNullOrEmpty(txtCompanyName.Text.Trim()))
                    {
                        ErrorDisplayBeneficiary.ShowError("Please enter Beneficiary's Company Name.");
                        txtCompanyName.Focus();
                        mpeRegisterBeneficiaryInfo.Show();
                        return false;
                    }
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
        private bool AddBeneficiary()
        {
            ErrorDisplay1.ClearError();
            ErrorDisplayBeneficiary.ClearError();
            try
            {
                var departmentId = 0;
                var unitId = 0;
                var companyName = string.Empty;
                if(int.Parse(ddlBeneficiaryType.SelectedValue) == 1)
                {
                    departmentId = int.Parse(ddlDepartment.SelectedValue);
                    unitId = int.Parse(ddlUnit.SelectedValue);
                    companyName = " ";
                }
                if(int.Parse(ddlBeneficiaryType.SelectedValue) == 2)
                {
                    departmentId = 8;
                    unitId = 14;
                    companyName = txtCompanyName.Text.Trim();
                }
                

                
                var newBeneficiary = new Beneficiary
                                         {
                                             CompanyName = companyName,
                                            BeneficiaryTypeId = int.Parse(ddlBeneficiaryType.SelectedValue),
                                            FullName = txtFullName.Text.Trim(),
                                            DepartmentId = departmentId,
                                            UnitId = unitId,
                                            GSMNO1 = txtPhone1.Text.Trim(),
                                            GSMNO2 = txtPhone2.Text.Trim(),
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
                        ErrorDisplayBeneficiary.ShowError("The Beneficiary information already exists.");
                        mpeRegisterBeneficiaryInfo.Show();
                        return false;
                    }

                    if (k == -4)
                    {
                        ErrorDisplayBeneficiary.ShowError("Another Beneficiary with similar phone number already exists.");
                        mpeRegisterBeneficiaryInfo.Show();
                        return false;
                    }
                    if (k == -5)
                    {
                        ErrorDisplayBeneficiary.ShowError("Another Beneficiary with similar email already exists.");
                        mpeRegisterBeneficiaryInfo.Show();
                        return false;
                    }

                    ErrorDisplayBeneficiary.ShowError("The Beneficiary Information could not be Added.");
                    mpeRegisterBeneficiaryInfo.Show();
                    return false;
                }
                ClearControls();
                return true;
            }


            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                return false;
            }


        }
        private bool UpdateBeneficiary()
        {
            ErrorDisplay1.ClearError();
            ErrorDisplayBeneficiary.ClearError();
            try
            {
                if (Session["_beneficiary"] == null)
                {
                    ErrorDisplayBeneficiary.ShowError("Beneficiary list is empty or session has expired.");
                    mpeRegisterBeneficiaryInfo.Show();
                    return false;
                }

                var beneficiary = Session["_beneficiary"]  as Beneficiary;

                if (beneficiary == null || beneficiary.BeneficiaryId < 1)
                {
                    ErrorDisplayBeneficiary.ShowError("Invalid selection!");
                    mpeRegisterBeneficiaryInfo.Show();
                    return false;
                }

                int departmentId = 0;
                int unitId = 0;
                if (int.Parse(ddlBeneficiaryType.SelectedValue) == 1)
                {
                    departmentId = int.Parse(ddlDepartment.SelectedValue);
                    unitId = int.Parse(ddlUnit.SelectedValue);
                }
                if (int.Parse(ddlBeneficiaryType.SelectedValue) == 2)
                {
                    departmentId = 8;
                    unitId = 14;
                }
                beneficiary.CompanyName = txtCompanyName.Text.Trim();
                beneficiary.FullName = txtFullName.Text.Trim();
                beneficiary.DepartmentId = departmentId;
                beneficiary.UnitId = unitId;
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
                        ErrorDisplayBeneficiary.ShowError("The Beneficiary information already exists.");
                        mpeRegisterBeneficiaryInfo.Show();
                        return false;
                    }
                    if (k == -4)
                    {
                        ErrorDisplayBeneficiary.ShowError("Another Beneficiary with similar phone number already exists.");
                        mpeRegisterBeneficiaryInfo.Show();
                        return false;
                    }

                    if (k == -5)
                    {
                        ErrorDisplayBeneficiary.ShowError("Another Beneficiary with similar email already exists.");
                        mpeRegisterBeneficiaryInfo.Show();
                        return false;
                    }
                    ErrorDisplayBeneficiary.ShowError("The Beneficiary Information could not be modified!");
                    mpeRegisterBeneficiaryInfo.Show();
                    return false;
                   
                }

                var staffUsers = ServiceProvider.Instance().GetStaffUserServices().GetStaffUsersByBeneficiaryId(beneficiary.BeneficiaryId);

                if(staffUsers != null && staffUsers.Any())
                {
                    staffUsers[0].Status = chkActive.Checked ? 1 : 0;

                    if (!ServiceProvider.Instance().GetStaffUserServices().UpdateStaffUser(staffUsers[0]))
                    {
                        ErrorDisplay1.ShowError("Beneficiary's Profile was created successfully but could not be registered as a User.");
                    }
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
        private void LoadBeneficiaries()
        {
            ErrorDisplayPortalUser.ClearError();
            try
            {
                var beneficiariesList = ServiceProvider.Instance().GetBeneficiaryServices().GetAllBeneficiaries();

                if (beneficiariesList == null || !beneficiariesList.Any())
                {
                    dgBeneficiaries.DataSource = new List<Beneficiary>();
                    dgBeneficiaries.DataBind();
                    return;
                }

                //dgBeneficiaries.DataSource = beneficiariesList;
                //dgBeneficiaries.DataBind();
                Session["_BeneficiaryList"] = beneficiariesList;
                FillRepeater<Beneficiary>(dgBeneficiaries, "_BeneficiaryList", Navigation.None, Limit, LoadMethod);
           
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
               
            }
        }
        private void ClearControls()
        {
            ErrorDisplay1.ClearError();
            ErrorDisplayBeneficiary.ClearError();
            txtFullName.Text = string.Empty;
            ddlBeneficiaryType.SelectedIndex = 0;
            ccdDepartment.SelectedValue = "0";
            ReqPhone1.Text = string.Empty;
            ccdUnit.SelectedValue = "0";
            txtPhone2.Text = string.Empty;
            chkBeneficiary.Checked = false;
            txtEmail.Text = string.Empty;
            ddlSex.SelectedIndex = 0;

        }
        #endregion
         #endregion

        #region Create Portal User Profile
        #region Event
        protected void BtnCreatePortalProfileClick(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            ErrorDisplayPortalUser.ClearError();
            try
            {

                if (ValidateRole() < 1)
                {
                    ErrorDisplayPortalUser.ShowError("You must select at least one role");
                    mpeRegisterBeneficiaryInfo.Show();
                    return;
                }



                if (!ValidateBeneficiaryPortalProfileControls())
                {
                    return;
                }
                SaveData();
               

            }
            catch (Exception ex)
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }

        }
        #endregion
        
        #region Helpers
        private void SaveData()
        {
            ErrorDisplayPortalUser.ClearError();
            try
            {
                if(Session["_beneficiaryId"] == null)
                {
                    ErrorDisplayPortalUser.ShowError("Beneficiary list is empty or session has expired.");
                    mpeRegisterBeneficiaryInfo.Show();
                    return;
                }
                
                var beneficiaryId = (int)Session["_beneficiaryId"];

                if (beneficiaryId < 1 )
                {
                    ErrorDisplayPortalUser.ShowError("Beneficiary list is empty or session has expired.");
                    mpeRegisterBeneficiaryInfo.Show();
                    return;
                }

                var memUser = Membership.GetUser(txtUserName.Text.Trim());

                if (memUser != null)
                {
                    if (memUser.UserName.Length > 1)
                    {
                        ErrorDisplayPortalUser.ShowError("User Already Exist!");
                        mpeRegisterBeneficiaryInfo.Show();
                        return;
                    }

                    if (memUser.ProviderUserKey != null)
                    {
                        ErrorDisplayPortalUser.ShowError("User Already Exists!");
                        mpeRegisterBeneficiaryInfo.Show();
                        return;
                    }
                }

                var userName = Membership.GetUserNameByEmail(txtPortalUserEmail.Text.Trim());

                if(!string.IsNullOrEmpty(userName))
                {
                    ErrorDisplayPortalUser.ShowError("This Email is already in use by another User.");
                    txtPortalUserEmail.Focus();
                    mpeRegisterBeneficiaryInfo.Show();
                    return;
                    //Membership.DeleteUser(userName);
                }


                var memUser2 = Membership.CreateUser(txtUserName.Text.Trim(), txtPassword.Text.Trim(), txtPortalUserEmail.Text.Trim());

                if (string.IsNullOrEmpty(memUser2.UserName))
                {
                    ErrorDisplayPortalUser.ShowError("Process Failed! User Information was not registered");
                    mpeRegisterBeneficiaryInfo.Show();
                    return;
                }

                memUser2.IsApproved = chkActive.Checked;
                Membership.UpdateUser(memUser2);
                var userId = new PortalServiceManager().GetUserIdByUsername(txtUserName.Text.Trim());
                if (userId < 1)
                {
                    Membership.DeleteUser(txtUserName.Text.Trim());
                    ErrorDisplayPortalUser.ShowError("Process Failed! Please try again soon");
                    mpeRegisterBeneficiaryInfo.Show();
                    return;
                }

                var password = txtPassword.Text.Trim();
                var username = txtUserName.Text.Trim();
                var email = txtPortalUserEmail.Text.Trim();
                var mUser = new portaluser
                                {
                                    DateRegistered = DateMap.GetLocalDate(),
                                    Designation = txtDesignation.Text.Trim(),
                                    FirstName = txtFirstName.Text.Trim(),
                                    LastName = txtLastName.Text.Trim(),
                                    MobileNumber = txtMobileNumber.Text.Trim(),
                                    SexId = int.Parse(ddlPortalUserSex.SelectedValue),
                                    TimeRegistered = DateTime.Now.ToString("hh:mm:ss"),
                                    UserName = txtUserName.Text.Trim(),
                                    UserId = userId,
                                    Status = chkActive.Checked
                                };

                var k = (new PortalServiceManager()).AddPortalUser(mUser);

                if (k < 1)
                {
                    Membership.DeleteUser(txtUserName.Text.Trim());
                    ErrorDisplayPortalUser.ShowError("User Information Was Not Saved!");
                    mpeRegisterBeneficiaryInfo.Show();
                    return;

                }

                //Add Roles
                string[] mRoles = Roles.GetRolesForUser(txtUserName.Text.Trim());

                if (mRoles != null)
                {
                    if (mRoles.Length > 0)
                    {
                        Roles.RemoveUserFromRoles(txtUserName.Text.Trim(), mRoles);
                    }

                }

                try
                {
                    foreach (ListItem item in chkRoles.Items)
                    {
                        if (item.Selected)
                        {
                            Roles.AddUserToRole(txtUserName.Text.Trim(), item.Value.Trim());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Membership.DeleteUser(txtUserName.Text.Trim());
                    (new PortalServiceManager()).DeletePortalUser(k);
                    ErrorDisplayPortalUser.ShowError("Process Failed! Please try again soon or contact the Admin.");
                    ErrorManager.LogApplicationError(ex.StackTrace,ex.Source,ex.Message);
                    return;
                }

                var newStaffUser = new StaffUser
                                       {
                                           BeneficiaryId = beneficiaryId,
                                           PortalUserId = k,
                                           Status =  chkActive.Checked ? 1 : 0
                                       };

                var k2 = ServiceProvider.Instance().GetStaffUserServices().AddStaffUser(newStaffUser);
                if(k2 < 1)
                {
                    ErrorDisplay1.ShowSuccess("Beneficiary's User Profile was created successfully but could not be registered as a Staff Beneficiary.");
                    return;
                }
                 
                const string subject = "User Profile Created";
                var body = "A user profile has been created for you on the Expense Manager Portal with the following details:"+ "\n"+ "Username: " +
                    username + "\n"+ " Password: " + password;

                if (!Mailsender(email, subject, body))
                {
                    ErrorDisplay1.ShowSuccess("A notification could not be sent to the User. Please consider sending the user a notification personally.");
                    return;
                }

                ErrorDisplay1.ShowSuccess("Beneficiary's User Profile was created successfully and a notification was sent to the user.");
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplayPortalUser.ShowError(ex.Message);
                mpeRegisterBeneficiaryInfo.Show();
            }
        }
        private bool Mailsender(string to, string subject, string body)
        {
            try
            {
                var emailUtility = new ExpensemanagerEmailSenderUtility();
                var config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                var settings = (System.Net.Configuration.MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");

                if (settings != null)
                {
                    var fromAddress = new MailAddress(settings.Smtp.From);
                    ThreadPool.QueueUserWorkItem(s =>
                    {
                        if (!emailUtility.SendMail(fromAddress, to, subject, body, settings.Smtp.Network.UserName, settings.Smtp.Network.Password))
                        {
                            ErrorDisplay1.ShowError("A notification could not be sent for your Transaction request due to some technical issues.\n Approval of your request may be delayed.");

                        }

                    });
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                ErrorDisplay1.ShowError("Your Transactions request notification could not be sent. Approval of your request might be delayed.");
                return false;
            }
        }
        protected void BindRoleList()
        {
            ErrorDisplay1.ClearError();
            try
            {
                string[] mRoles = Roles.GetAllRoles();
                var mRoleList = new List<string>();
                if (mRoles.Length > 0)
                {
                    foreach (string mString in mRoles)
                    {
                        if (!Page.User.IsInRole("PortalAdmin"))
                        {
                            if (mString != "PortalAdmin" && mString != "SiteAdmin")
                            {
                                mRoleList.Add(mString);
                            }
                        }
                        else
                        {
                            if (mString != "PortalAdmin")
                            {
                                mRoleList.Add(mString);
                            }

                        }
                    }
                    if (mRoleList.Count > 0)
                    {
                        chkRoles.DataSource = mRoleList;
                        chkRoles.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay1.ShowError(ex.Message);
            }

        }
        private bool ValidateBeneficiaryPortalProfileControls()
        {
            ErrorDisplayBeneficiary.ClearError();
            ErrorDisplay1.ClearError();
            try
            {
                if (string.IsNullOrEmpty(txtFirstName.Text.Trim()))
                {
                    ErrorDisplayPortalUser.ShowError("Please supply Beneficiary's First Name.");
                    txtFirstName.Focus();
                    mpeRegisterBeneficiaryInfo.Show();
                    return false;
                }

                if (DataCheck.IsNumeric(txtFirstName.Text.Trim()))
                {
                    ErrorDisplayPortalUser.ShowError("Invalid First Name!");
                    txtFirstName.Focus();
                    mpeRegisterBeneficiaryInfo.Show();
                    return false;
                }

                if (string.IsNullOrEmpty(txtLastName.Text.Trim()))
                {
                    ErrorDisplayPortalUser.ShowError("Please supply Beneficiary's First Last Name.");
                    txtLastName.Focus();
                    mpeRegisterBeneficiaryInfo.Show();
                    return false;
                }

                if (DataCheck.IsNumeric(txtLastName.Text.Trim()))
                {
                    ErrorDisplayPortalUser.ShowError("Invalid Last Name!");
                    txtLastName.Focus();
                    mpeRegisterBeneficiaryInfo.Show();
                    return false;
                }

                if (string.IsNullOrEmpty(txtPortalUserEmail.Text.Trim()))
                {
                    ErrorDisplayPortalUser.ShowError("Please enter Email Address.");
                    txtPortalUserEmail.Focus();
                    mpeRegisterBeneficiaryInfo.Show();
                    return false;
                }

                if (string.IsNullOrEmpty(txtDesignation.Text.Trim()))
                {
                    ErrorDisplayPortalUser.ShowError("Please enter User's Designation.");
                    txtDesignation.Focus();
                    mpeRegisterBeneficiaryInfo.Show();
                    return false;
                }

                if (string.IsNullOrEmpty(txtMobileNumber.Text.Trim()))
                {
                    ErrorDisplayPortalUser.ShowError("Please supply Beneficiary's GSM Phone Number.");
                    txtMobileNumber.Focus();
                    mpeRegisterBeneficiaryInfo.Show();
                    return false;
                }

                if ((txtMobileNumber.Text.Trim().Length != 11))
                {
                    ErrorDisplayPortalUser.ShowError("Incorrect GSM number format!");
                    txtMobileNumber.Focus();
                    mpeRegisterBeneficiaryInfo.Show();
                    return false;
                }

                if (!DataCheck.IsNumeric(txtMobileNumber.Text.Trim()))
                {
                    ErrorDisplayPortalUser.ShowError("Invalid entry!");
                    txtMobileNumber.Focus();
                    mpeRegisterBeneficiaryInfo.Show();
                    return false;
                }

                if (DataCheck.IsNumeric(txtFirstName.Text.Trim()) || DataCheck.IsNumeric(txtLastName.Text.Trim()))
                {
                    ErrorDisplayPortalUser.ShowError("Invalid entry!");
                    txtFirstName.Focus();
                    mpeRegisterBeneficiaryInfo.Show();
                    return false;
                }

                if (int.Parse(ddlPortalUserSex.SelectedValue) < 1)
                {
                    ErrorDisplayPortalUser.ShowError("Invalid Sex option selected.");
                    ddlPortalUserSex.Focus();
                    mpeRegisterBeneficiaryInfo.Show();
                    return false;
                }

                if (txtUserName.Text.Trim() == "")
                {
                    ErrorDisplayPortalUser.ShowError("Specify the login username");
                    txtUserName.Focus();
                    mpeRegisterBeneficiaryInfo.Show();
                    return false;
                }
                if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
                {
                    ErrorDisplayPortalUser.ShowError("Please specify User's login password");
                    txtPassword.Focus();
                    mpeRegisterBeneficiaryInfo.Show();
                    return false;
                }

                if (string.IsNullOrEmpty(txtConfirmPassword.Text.Trim()))
                {
                    ErrorDisplayPortalUser.ShowError("Please Confirm the login password");
                    txtConfirmPassword.Focus();
                    mpeRegisterBeneficiaryInfo.Show();
                    return false;
                }

                if (txtPassword.Text.Length < 8)
                {
                    ErrorDisplayPortalUser.ShowError("Invalid password length. Password must be at least 8 characters in length");
                    txtPassword.Focus();
                    mpeRegisterBeneficiaryInfo.Show();
                    return false;
                }

                if (txtConfirmPassword.Text.Length < 8)
                {
                    ErrorDisplayPortalUser.ShowError("Invalid password length. Password must be at least 8 characters in length");
                    txtConfirmPassword.Focus();
                    mpeRegisterBeneficiaryInfo.Show();
                    return false;
                }

                if (!txtConfirmPassword.Text.ToUpper().Trim().Equals(txtPassword.Text.ToUpper().Trim()))
                {
                    ErrorDisplayPortalUser.ShowError("The Passwords do not match");
                    txtConfirmPassword.Focus();
                    mpeRegisterBeneficiaryInfo.Show();
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
        protected int ValidateRole()
        {
            int k = 0;
            for (int i = 0; i < chkRoles.Items.Count; i++)
            {
                if (chkRoles.Items[i].Selected)
                    k += 1;
            }
            return k;
        }

        #endregion
        #endregion
        
        #region Pagination
        protected void FirstLink(object sender, EventArgs e)
        {

            ErrorDisplay1.ClearError();
            try
            {
                var senderLinkArgument = int.Parse(((LinkButton)sender).CommandArgument);
                NowViewing = (int)senderLinkArgument;
                FillRepeater<Beneficiary>(dgBeneficiaries, "_BeneficiaryList", Navigation.Sorting, Limit, LoadMethod);
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
                var obj = ViewState["_NowViewing"];
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

            ErrorDisplay1.ClearError();
            try
            {
                FillRepeater<Beneficiary>(dgBeneficiaries, "_BeneficiaryList", Navigation.Next, Limit, LoadMethod);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
              
            }
        }
        protected void LbtnFirstClick(object sender, EventArgs e)
        {

            ErrorDisplay1.ClearError();
            try
            {
                FillRepeater<Beneficiary>(dgBeneficiaries, "_BeneficiaryList", Navigation.First, Limit, LoadMethod);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            
            }
        }
        protected void LbtnLastClick(object sender, EventArgs e)
        {

            ErrorDisplay1.ClearError();
            try
            {
                FillRepeater<Beneficiary>(dgBeneficiaries, "_BeneficiaryList", Navigation.Last, Limit, LoadMethod);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
              
            }
        }
        protected void LbtnPrevClick(object sender, EventArgs e)
        {

            ErrorDisplay1.ClearError();
            try
            {
                FillRepeater<Beneficiary>(dgBeneficiaries, "_BeneficiaryList", Navigation.Previous, Limit, LoadMethod);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
             
            }
        }
        protected void OnLimitChange(object sender, EventArgs e)
        {

            ErrorDisplay1.ClearError();
            try
            {
                //ShowLink();
                Limit = int.Parse(ddlLimit.SelectedValue);
                FillRepeater<Beneficiary>(dgBeneficiaries, "_BeneficiaryList", Navigation.None, Limit, LoadMethod);
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
            var beneficiariesList = ServiceProvider.Instance().GetBeneficiaryServices().GetAllBeneficiaries();

            if (beneficiariesList == null || !beneficiariesList.Any())
            {
                dgBeneficiaries.DataSource = new List<Beneficiary>();
                dgBeneficiaries.DataBind();
                return false;

            }
            Session["_BeneficiaryList"] = beneficiariesList;
            return true;
        }
        #endregion
        #endregion

    }
}