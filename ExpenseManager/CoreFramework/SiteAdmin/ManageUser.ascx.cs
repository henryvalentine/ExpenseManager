using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPLUG.WEBTOOLS;

namespace ExpenseManager.CoreFramework.SiteAdmin
{
    public partial class ManageUser : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindUsersList();
            }
        }

        #region Page Events
        protected void DgPortalUsersEditCommand(Object source, DataGridCommandEventArgs e)
        {
            if (!(Page.User.IsInRole("PortalAdmin") || Page.User.IsInRole("SiteAdmin")))
            {
                ErrorDisplay1.ShowError("Sorry: You do not have access to this module!");
                return;
            }
            ErrorDisplay1.ClearError();
            dgPortalUsers.SelectedIndex = e.Item.ItemIndex;
            string userName = dgPortalUsers.DataKeys[e.Item.ItemIndex].ToString();

            if (Page.User.Identity.Name.ToLower() != "sadegboyega")
            {
                if (userName.ToLower() == "admin" || userName.ToLower() == "sadegboyega")
                {
                    ErrorDisplay1.ShowError("Sorry: You cannot edit this user information");
                    return;
                }
                if (Roles.GetRolesForUser(userName).Contains("SiteAdmin") && !Page.User.IsInRole("PortalAdmin"))
                {
                    ErrorDisplay1.ShowError("Sorry: You are not authorized to edit this user information");
                    return;
                }
            }
            if (Page.User.Identity.Name.ToLower() == "sadegboyega" && !Page.User.IsInRole("SiteAdmin"))
            {
                ErrorDisplay1.ShowError("Sorry: You are not authorized to edit this user information");
                return;
            }
            if (userName.ToLower() == "admin")
            {
                ErrorDisplay1.ShowError("Sorry: You cannot edit this user information");
                return;
            }

            BindUSer(userName);
        }

        protected void DgPortalUsersDeleteCommand(Object source, DataGridCommandEventArgs e)
        {
            ErrorDisplay1.ClearError();
            if (!Page.User.IsInRole("PortalAdmin"))
            {
                ErrorDisplay1.ShowError("Sorry: You are not authorized to delete a user");
                return;
            }
            try
            {
                dgPortalUsers.SelectedIndex = e.Item.ItemIndex;
                string mName = ((LinkButton)dgPortalUsers.SelectedItem.FindControl("lblUserName")).Text;
                if (mName.Length < 2)
                {
                    ErrorDisplay1.ShowError("No User Item Selected");
                    return;
                }
                if (HttpContext.Current.User.Identity.Name.ToLower() == mName.ToLower())
                {
                    ErrorDisplay1.ShowError("You cannot delete the current user!");
                    return;
                }
                var mUser = Membership.GetUser(mName, false);
                if (mUser == null)
                {
                    ErrorDisplay1.ShowError("Username not found!");
                    return;
                }
                mUser.IsApproved = false;
                Membership.UpdateUser(mUser);
                //Membership.DeleteUser(mName);
                BindUsersList();
                ErrorDisplay1.ShowSuccess("User Record was Deleted successfully.");
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay1.ShowError(ex.Message);
            }
        }

        protected void BtnUpdateRecordClick(Object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            if (txtEmail.Text == "")
            {
                ErrorDisplay1.ShowError("Email Address is required");
                return;
            }
            int k = ValidateRole();
            if (k < 1)
            {
                ErrorDisplay1.ShowError("User must belong to at least a role");
                return;
            }

            var mUser = Membership.GetUser(txtUserName.Text.Trim());
            if (mUser == null)
            {
                ErrorDisplay1.ShowError("Fatal Error occurred! Please try again soon");
                return;
            }
            mUser.Email = txtEmail.Text.Trim();
            mUser.IsApproved = chkActive.Checked;
            Membership.UpdateUser(mUser);
            string[] mRoles = Roles.GetRolesForUser(txtUserName.Text.Trim());

            if (mRoles != null)
            {
                if (mRoles.Length > 0)
                {
                    Roles.RemoveUserFromRoles(txtUserName.Text.Trim(), mRoles);
                }

            }

            var mNewRoles = new string[k];
            int h = 0;
            for (int i = 0; i < chkRoles.Items.Count; i++)
            {
                if (chkRoles.Items[i].Selected)
                {
                    mNewRoles[h] = chkRoles.Items[i].Value.Trim();
                    h += 1;
                }
            }
            if (mNewRoles[0].Length > 0)
            {
                Roles.AddUserToRoles(txtUserName.Text.Trim(), mNewRoles);
                BindUsersList();
                HideTables();
                listDV.Visible = true;
                ErrorDisplay1.ShowSuccess("User Information was updated successfully.");
            }
            else
            {
                ErrorDisplay1.ShowError("Fatal Error occurred! User information could not be updated");
            }

        }

        protected void BtnCancelClick(Object sender, EventArgs e)
        {
            HideTables();
            listDV.Visible = true;
        }

        #endregion
        #region Helper Methods
        protected void HideTables()
        {
            detailDiv.Visible = false;
            listDV.Visible = false;
        }
        protected void BindUsersList()
        {
            MembershipUserCollection userList = Membership.GetAllUsers();

            var approvedUser = new MembershipUserCollection();

            foreach (MembershipUser user in userList)
            {
                if(user.IsApproved && !user.IsLockedOut)
                {
                    approvedUser.Add(user);
                }
            }

            dgPortalUsers.DataSource = approvedUser;
            dgPortalUsers.DataBind();
            listDV.Visible = true;
        }
        protected void BindRoleList()
        {
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
        public string GetRoles(string userName)
        {
            try
            {
                string myRoles = String.Empty;
                string[] mRoles = Roles.GetRolesForUser(userName);
                if (mRoles.Length > 0)
                {
                    myRoles = mRoles.Aggregate(myRoles, (current, mRole) => current + ("; " + mRole));
                }
                else
                {
                    return "";
                }
                if (myRoles.StartsWith(";"))
                {
                    myRoles = myRoles.Substring(1);
                }
                if (myRoles.EndsWith(";"))
                {
                    myRoles = myRoles.Substring(0, myRoles.Length - 1);
                }
                return myRoles;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return "";
            }
        }
        protected void BindUSer(string userName)
        {
            if (userName.Length < 2)
            {
                ErrorDisplay1.ShowError("Process Error / Invalid Request");
                return;
            }
            var mUser = Membership.GetUser(userName);
            if (mUser == null)
            {
                ErrorDisplay1.ShowError("Process Error / Invalid Request");
                return;
            }
            txtCreationDate.Text = mUser.CreationDate.ToString("dd/MM/yyyy");
            txtEmail.Text = mUser.Email;
            txtUserName.Text = userName;
            chkActive.Checked = mUser.IsApproved;
            BindRoleList();
            CheckRoles(userName);
            mpeDisplayJobDetails.Show();
        }
        protected void CheckRoles(string userName)
        {
            string[] mRoles = Roles.GetRolesForUser(userName);
            if (mRoles.Length > 0)
            {
                foreach (string mRole in mRoles)
                {
                    for (int i = 0; i < chkRoles.Items.Count; i++)
                    {
                        if (chkRoles.Items[i].Value == mRole)
                        {
                            chkRoles.Items[i].Selected = true;
                            break;
                        }
                    }
                }
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
    }
}