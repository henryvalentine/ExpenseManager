using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPLUG.WEBTOOLS;

namespace ExpenseManager.CoreFramework
{
    public partial class UserProfile : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                Session["_userList"] = null;
                Session["_selectedUser"] = null;
                BindUsersList();
            }
        }

        #region Events
            protected void BtnAddItemClick(Object sender, EventArgs e)
            {
                ErrorDisplay1.ClearError();
                ErrorDisplay1.ClearControls(tbUserInfo);
                ErrorDisplay2.ClearError();
                BindRoleList();

                //Enable controls 
                txtPassword.Enabled = true;
                txtConfirmPassword.Enabled = true;
                txtUserName.Enabled = true;
                reqPassword.Enabled = true;
                reqConfirmPassword.Enabled = true;
                chkActive.Checked = false;

                btnSubmit.CommandArgument = "1"; // Add Item
                btnSubmit.Text = "Add New User";
                mpeDisplayJobDetails.Show();

            }

            protected void DgPortalUsersEditCommand(Object source, DataGridCommandEventArgs e)
            {
                if (!(Page.User.IsInRole("PortalAdmin") || Page.User.IsInRole("SiteAdmin")))
                {
                    ErrorDisplay1.ShowError("Sorry: You do not have access to this module!");
                   return;
                }
                ErrorDisplay1.ClearError();
                dgPortalUsers.SelectedIndex = e.Item.ItemIndex;
                var userId = (DataCheck.IsNumeric(dgPortalUsers.DataKeys[e.Item.ItemIndex].ToString())) ? long.Parse(dgPortalUsers.DataKeys[e.Item.ItemIndex].ToString()) : 0;
                if (userId < 1)
                {
                    ErrorDisplay1.ShowError("Invalid Selection");
                    return; 
                }
               
                Session["_selectedUser"] = null;
               if(!BindUSer(userId))
               {
                   return;
               }

                btnSubmit.CommandArgument = "2"; // Update Item
                btnSubmit.Text = "Update User";
                mpeDisplayJobDetails.Show();
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
                    var userId = (DataCheck.IsNumeric(dgPortalUsers.DataKeys[e.Item.ItemIndex].ToString())) ? long.Parse(dgPortalUsers.DataKeys[e.Item.ItemIndex].ToString()) : 0;
                    if (userId < 1)
                    {
                        ErrorDisplay1.ShowError("Invalid Selection");
                        return;
                    }
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
                    //new PortalServiceManager().DeletePortalUser(userId);
                    BindUsersList();
                    ErrorDisplay1.ShowSuccess("User Record Was Deleted");
                }
                catch (Exception ex)
                {
                    ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                    ErrorDisplay1.ShowError(ex.Message);
                }
            }
            protected void BtnSubmitClick(object sender, EventArgs e)
            {
                ErrorDisplay1.ClearError();
                Page.Validate("regValidation");
                if (!Page.IsValid)
                {
                    ErrorDisplay2.ShowError("Please enter all required fields");
                    mpeDisplayJobDetails.Show();
                    return;
                }
                
           
           
                try
                {
               
                    if (ValidateRole() < 1)
                    {
                        ErrorDisplay2.ShowError("You must select at least one role");
                        mpeDisplayJobDetails.Show();
                        return;
                    }

                    switch (int.Parse(btnSubmit.CommandArgument))
                    {
                        case 1: //Add
                            if (!ValidateControls())
                            {
                                mpeDisplayJobDetails.Show();
                                return;
                            }
                            SaveData();
                            break;
                        case 2: //Update
                            if(!ValidateControlsForUpdate())
                            {
                                mpeDisplayJobDetails.Show();
                                return;
                            }
                            
                            if ( Session["_selectedUser"] == null)
                            {
                                ErrorDisplay2.ShowError("Session Has Expired!");
                                mpeDisplayJobDetails.Show();
                                return;
                            }
                            try
                            {
                                 var user = (portaluser) Session["_selectedUser"];
                                if(user == null)
                                {
                                    ErrorDisplay2.ShowError("Session Has Expired!");
                                    mpeDisplayJobDetails.Show();
                                    return;
                                }
                                if (user.UserId < 1)
                                {
                                    ErrorDisplay2.ShowError("Session Has Expired!");
                                    mpeDisplayJobDetails.Show();
                                    return;
                                }
                                if (!(new PortalServiceManager()).CheckUniqueEmailNo(user.UserId, txtEmail.Text.Trim()))
                                {
                                    ErrorDisplay2.ShowError("Supplied Email address has been used by another User");
                                    mpeDisplayJobDetails.Show();
                                    return;
                                }
                                if (!(new PortalServiceManager()).CheckUniqueMobileNo(user.PortalUserId, txtMobileNumber.Text.Trim()))
                                {
                                    ErrorDisplay2.ShowError("Supplied Mobile Number already been used by another User");
                                    mpeDisplayJobDetails.Show();
                                    return;
                                } 

                                if (!UpdateData(user))
                                {
                                    mpeDisplayJobDetails.Show();
                                    return; 
                                }
                            }
                            catch(Exception)
                            {
                                ErrorDisplay2.ShowError("Session Has Expired!");
                                mpeDisplayJobDetails.Show();
                                return;
                            }
                           
                            // mpeDisplayJobDetails.Show();
                            ErrorDisplay1.ShowSuccess("User Information Was Updated");
                            btnSubmit.Text = "Add New User";
                            btnSubmit.CommandArgument = "1";
                            mpeDisplayJobDetails.Hide();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                }

            }
        #endregion
        #region Helper Methods
       
        private void SaveData()
        {
            try
            {
                var memUser = Membership.GetUser(txtUserName.Text.Trim());
                
                if (memUser != null)
                {
                    if (memUser.UserName.Length > 1)
                    {
                        ErrorDisplay2.ShowError("User Already Exist!");
                        mpeDisplayJobDetails.Show();
                        return;
                    }
                }
                
                var memUser2 = Membership.CreateUser(txtUserName.Text.Trim(), txtPassword.Text.Trim(), txtEmail.Text.Trim());
                
                if (memUser2.UserName == "")
                {
                    ErrorDisplay2.ShowError("Process Failed! User Information was not registered");
                    mpeDisplayJobDetails.Show();
                    return;
                }

                memUser2.IsApproved = chkActive.Checked;
                Membership.UpdateUser(memUser2);
                var userId = new PortalServiceManager().GetUserIdByUsername(txtUserName.Text.Trim());
                if (userId < 1)
                {
                    Membership.DeleteUser(txtUserName.Text.Trim());
                    ErrorDisplay2.ShowError("Process Failed! Please try again soon");
                    mpeDisplayJobDetails.Show();
                    return;
                }

                var mUser = new portaluser
                                {
                                    DateRegistered = DateMap.GetLocalDate(),
                                    Designation = txtDesignation.Text.Trim(),
                                    FirstName = txtFirstName.Text.Trim(),
                                    LastName = txtLastName.Text.Trim(),
                                    MobileNumber = txtMobileNumber.Text.Trim(),
                                    SexId = int.Parse(ddlSex.SelectedValue),
                                    TimeRegistered = DateTime.Now.ToString("hh:mm:ss"),
                                    UserName = txtUserName.Text.Trim(),
                                    UserId = userId,
                                    Status = chkActive.Checked
                                };
                
                var k = (new PortalServiceManager()).AddPortalUser(mUser);

                if (k < 1)
                {
                    Membership.DeleteUser(txtUserName.Text.Trim());
                    ErrorDisplay2.ShowError("User Information Was Not Saved!");
                    mpeDisplayJobDetails.Show();
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
                catch (Exception)
                {
                    Membership.DeleteUser(txtUserName.Text.Trim());
                    (new PortalServiceManager()).DeletePortalUser(k);
                    ErrorDisplay2.ShowError("Process Failed! Please try again soon");
                    return;
                }

                BindUsersList();

                ErrorDisplay2.ClearControls(tbUserInfo);
                ErrorDisplay1.ClearError();
                ErrorDisplay1.ShowSuccess("User Information Was Saved");
                mpeDisplayJobDetails.Hide();
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay2.ShowError(ex.Message);
                mpeDisplayJobDetails.Show();
            }
        }
        private bool UpdateData(portaluser user)
        {
            try
            {
                var memUser = Membership.GetUser(user.UserName);
                if (memUser == null)
                {
                    ErrorDisplay2.ShowError("Session Has Expired!");
                    return false;
                    
                }
                if (memUser.Email.Length < 5)
                {
                    ErrorDisplay2.ShowError("Session Has Expired!");
                    return false;

                }

                memUser.Email = txtEmail.Text.Trim();
                memUser.IsApproved = chkActive.Checked;
                Membership.UpdateUser(memUser);

                user.Designation = txtDesignation.Text.Trim();
                user.FirstName = txtFirstName.Text.Trim();
                user.LastName = txtLastName.Text.Trim();
                user.MobileNumber = txtMobileNumber.Text.Trim();
                user.SexId = int.Parse(ddlSex.SelectedValue);
                user.Status = chkActive.Checked;

                var k = (new PortalServiceManager()).UpdatePortalUser(user);

                if (k < 1)
                {
                    Membership.DeleteUser(txtUserName.Text.Trim());
                    ErrorDisplay2.ShowError("User Information Was Not Updated!");
                    return false;
                }
              
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
                        if(item.Selected)
                        {
                             Roles.AddUserToRole(txtUserName.Text.Trim(), item.Value.Trim());
                        }
                    }
                }
                catch(Exception)
                {
                    Membership.DeleteUser(txtUserName.Text.Trim());
                    (new PortalServiceManager()).DeletePortalUser(user.UserId);
                    ErrorDisplay2.ShowError("Process Failed! Please try again soon");
                    return false;
                }
                BindUsersList();
              
                ErrorDisplay2.ClearControls(tbUserInfo);
                ErrorDisplay1.ClearError();
                ErrorDisplay1.ShowSuccess("User record was updated successfully.");
                return true;
                
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay2.ShowError(ex.Message);
                mpeDisplayJobDetails.Show();
                return false;
            }
        }
        private bool ValidateControls()
        {

            if (txtFirstName.Text.Trim() == "")
            {
                ErrorDisplay2.ShowError("First Name is required");
                return false;
            }
            if (txtLastName.Text.Trim() == "")
            {
                ErrorDisplay2.ShowError("Last Name is required");
                return false;
            }
            if (int.Parse(ddlSex.SelectedValue) < 1)
            {
                ErrorDisplay2.ShowError("Select sex");
                return false;
            }
            if (txtMobileNumber.Text.Trim() == "")
            {
                ErrorDisplay2.ShowError("Mobile Number is required");
                return false;
            }
            if (txtDesignation.Text == "")
            {
                ErrorDisplay2.ShowError("Specify the designation");
                return false;
            }
            if (txtUserName.Text.Trim() == "")
            {
                ErrorDisplay2.ShowError("Specify the login username");
                return false;
            }
            if (txtPassword.Text.Trim() == "")
            {
                ErrorDisplay2.ShowError("Specify the login password");
                return false;
            }
            if (txtConfirmPassword.Text.Length < 8)
            {
                ErrorDisplay2.ShowError("Invalid password length. Password must be atleast 8 character length");
                return false;
            }
            if (txtConfirmPassword.Text.Trim() == "")
            {
                ErrorDisplay2.ShowError("Confirm login password");
                return false;
            }
            if (!txtConfirmPassword.Text.ToUpper().Trim().Equals(txtPassword.Text.ToUpper().Trim()))
            {
                ErrorDisplay2.ShowError("Password and Confirm password not equal");
                return false;
            }
            if (txtEmail.Text.Trim() == "")
            {
                ErrorDisplay2.ShowError("Specify email address");
                return false;
            }
            return true;
        }
        private bool ValidateControlsForUpdate()
        {

            if (txtFirstName.Text.Trim() == "")
            {
                ErrorDisplay2.ShowError("First Name is required");
                return false;
            }
            if (txtLastName.Text.Trim() == "")
            {
                ErrorDisplay2.ShowError("Last Name is required");
                return false;
            }
            if (int.Parse(ddlSex.SelectedValue) < 1)
            {
                ErrorDisplay2.ShowError("Select sex");
                return false;
            }
            if (txtMobileNumber.Text.Trim() == "")
            {
                ErrorDisplay2.ShowError("Mobile Number is required");
                return false;
            }
            if (txtDesignation.Text == "")
            {
                ErrorDisplay2.ShowError("Specify the designation");
                return false;
            }
            if (txtEmail.Text.Trim() == "")
            {
                ErrorDisplay2.ShowError("Specify email address");
                return false;
            }
              
            return true;
        }
        
            #region ROLE SETTING
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
            #region USER
                protected bool BindUSer(long portalUserId)
                {
                    ErrorDisplay1.ClearError();
                    ErrorDisplay2.ClearError();
                    if (Session["_userList"] == null)
                    {
                        ErrorDisplay1.ShowError("Invalid Selection");
                        return false;
                    }
                    try
                    {
                        var mlist = (List<portaluser>)Session["_userList"];
                        if (mlist == null)
                        {
                            ErrorDisplay1.ShowError("Invalid Selection");
                            return false;
                        }
                        if (mlist.Count < 1)
                        {
                            ErrorDisplay1.ShowError("Invalid Selection");
                            return false;
                        }

                        var user = mlist.Find(m => m.UserId == portalUserId);
                        if(user == null)
                        {
                            ErrorDisplay1.ShowError("Invalid Selection");
                            return false;
                        }
                        if (user.UserId < 1)
                        {
                            ErrorDisplay1.ShowError("Invalid Selection");
                            return false;
                        }

                        if (Page.User.Identity.Name.ToLower() != "sadegboyega")
                        {
                            if (user.UserName.ToLower() == "admin" || user.UserName.ToLower() == "sadegboyega")
                            {
                                ErrorDisplay1.ShowError("Sorry: You cannot edit this user information");
                                return false;
                            }
                            if (Roles.GetRolesForUser(user.UserName).Contains("SiteAdmin") && !Page.User.IsInRole("PortalAdmin"))
                            {
                                ErrorDisplay1.ShowError("Sorry: You are not authorized to edit this user information");
                                return false;
                            }
                        }

                        if (Page.User.Identity.Name.ToLower() == "sadegboyega" && !Page.User.IsInRole("SiteAdmin"))
                        {
                            ErrorDisplay1.ShowError("Sorry: You are not authorized to edit this user information");
                            return false;
                        }

                        if (user.UserName.ToLower() == "admin")
                        {
                            ErrorDisplay1.ShowError("Sorry: You cannot edit this user information");
                            return false;
                        }
                        
                        Session["_selectedUser"] = user;
                        MembershipUser mUser = Membership.GetUser(user.UserName);
                        if (mUser == null)
                        {
                            ErrorDisplay1.ShowError("Invalid Selection");
                            return false;
                        }
                       
                        txtEmail.Text = mUser.Email;
                        txtUserName.Text = user.UserName;
                        txtDesignation.Text = user.Designation;
                        txtFirstName.Text = user.FirstName;
                        txtLastName.Text = user.LastName;
                        txtMobileNumber.Text = user.MobileNumber;
                        txtPassword.Enabled = false;
                        txtConfirmPassword.Text = "";
                        txtConfirmPassword.Enabled = false;
                        txtUserName.Enabled = false;
                        reqPassword.Enabled = false;
                        reqConfirmPassword.Enabled = false;
                        chkActive.Checked = user.Status;
                        BindRoleList();
                        CheckRoles(user.UserName);
                        ddlSex.SelectedValue = user.SexId.ToString(CultureInfo.InvariantCulture);
                        return true;

                    }
                    catch(Exception)
                    {
                        return false;
                    }
                }
                protected void BindUsersList()
                {
                    try
                    {
                        var mlist = new PortalServiceManager().GetActivePortalUser();
                        if(mlist == null)
                        {
                            Session["_userList"] = new List<portaluser>();
                            dgPortalUsers.DataSource =new List<portaluser>();
                            dgPortalUsers.DataBind();
                        }

                        Session["_userList"] = mlist;
                        dgPortalUsers.DataSource = mlist;
                        dgPortalUsers.DataBind();
                    }
                    catch(Exception)
                    {
                        dgPortalUsers.DataSource = new List<portaluser>();
                        dgPortalUsers.DataBind(); 
                    }
                   
                    
                }
                protected void ReBindUsersList()
                {
                    try
                    {
                        if(Session["_userList"] == null)
                        {
                            BindUsersList();
                            return;
                        }

                        var mlist = (List<portaluser>) Session["_userList"];
                        if(mlist == null)
                        {
                            BindUsersList();
                            return;
                        }

                        dgPortalUsers.DataSource = mlist;
                        dgPortalUsers.DataBind();
                    }
                    catch (Exception)
                    {
                        dgPortalUsers.DataSource = new List<portaluser>();
                        dgPortalUsers.DataBind();
                    }


                }
            #endregion
        #endregion
    }
}