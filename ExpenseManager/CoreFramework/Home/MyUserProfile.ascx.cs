using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPLUG.WEBTOOLS;

namespace ExpenseManager.CoreFramework.Home
{
    public partial class MyUserProfile : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                LoadProfile();

            }
        }
        #region Events
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            if (!ValidateControls()) { return; }
            SaveData();
        }
        #endregion
        #region Helper Methods

        private void SaveData()
        {
            try
            {

                if (Session["defaultUserProfile"] == null)
                {
                    ErrorDisplay1.ShowError("Session Expired! Please try again soon");
                    return;
                }

                portaluser mUser;
                try
                {
                    mUser = (portaluser)Session["defaultUserProfile"];
                    if (mUser == null)
                    {
                        ErrorDisplay1.ShowError("Session Expired! Please try again soon");
                        return;

                    }

                }
                catch
                {
                    ErrorDisplay1.ShowError("Session Expired! Please try again soon");
                    return;
                }

                mUser.DateRegistered = XPLUG.WEBTOOLS.DateMap.GetLocalDate();
                mUser.Designation = this.txtDesignation.Text.Trim();
                mUser.FirstName = this.txtFirstName.Text.Trim();
                mUser.LastName = this.txtLastName.Text.Trim();
                mUser.MobileNumber = txtMobileNumber.Text.Trim();
                var k = new PortalServiceManager().UpdatePortalUser(mUser);
                if (k < 1)
                {
                    if (k == -3)
                    {
                        ErrorDisplay1.ShowError("The user name already exists.");
                        return;
                    }

                    if (k == -4)
                    {
                        ErrorDisplay1.ShowError("A user with the same mobile number already exists.");
                        return;
                    }

                    if (k == -5)
                    {
                        ErrorDisplay1.ShowError("The user information already exists.");
                        return;
                    }
                }

                MembershipUser myuser = Membership.GetUser(HttpContext.Current.User.Identity.Name.ToString(CultureInfo.InvariantCulture));
                if (myuser != null)
                {
                    myuser.Email = this.txtEmail.Text;
                    Membership.UpdateUser(myuser);
                }

                Session["defaultUserProfile"] = mUser;
                ErrorDisplay1.ShowSuccess("Profile Information Was Updated");
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay1.ShowError(ex.Message);
                return;
            }
        }
        private bool ValidateControls()
        {
            if (this.txtFirstName.Text.Trim() == "")
            {
                ErrorDisplay1.ShowError("First Name is required");
                return false;
            }
            if (this.txtLastName.Text.Trim() == "")
            {
                ErrorDisplay1.ShowError("Last Name is required");
                return false;
            }
            if (this.txtMobileNumber.Text.Trim() == "")
            {
                ErrorDisplay1.ShowError("Mobile Number is required");
                return false;
            }
            if (this.txtDesignation.Text == "")
            {
                ErrorDisplay1.ShowError("Specify the designation");
                return false;
            }
            if (this.txtEmail.Text.Trim() == "")
            {
                ErrorDisplay1.ShowError("Specify email address");
                return false;
            }
            return true;
        }
        private void LoadProfile()
        {
            try
            {
                string username = HttpContext.Current.User.Identity.Name.ToString(CultureInfo.InvariantCulture);
                portaluser mUser = new PortalServiceManager().GetPortalUser(username);
                if (mUser != null)
                {
                    if (mUser.PortalUserId > 0)
                    {
                        Session["defaultUserProfile"] = mUser;
                        this.txtDesignation.Text = mUser.Designation;
                        try
                        {
                            var membershipUser = Membership.GetUser(username);
                            if (membershipUser != null)
                                this.txtEmail.Text = membershipUser.Email;
                        }
                        catch { }
                        this.txtFirstName.Text = mUser.FirstName;
                        this.txtLastName.Text = mUser.LastName;
                        this.txtMobileNumber.Text = mUser.MobileNumber;
                        this.txtSex.Text = Enum.Parse(typeof(kPortal.Common.EnumControl.Enums.Sex), mUser.SexId.ToString(CultureInfo.InvariantCulture)).ToString();

                    }
                }

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay1.ShowError(ex.Message);
                return;
            }
        }

        protected void ClearControl()
        {
            ErrorDisplay1.ClearControls(this);
        }
        #endregion
    }
}