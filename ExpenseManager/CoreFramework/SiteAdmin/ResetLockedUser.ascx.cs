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
using ExpenseManager.CoreFramework.AlertControl;
using XPLUG.WEBTOOLS;

namespace ExpenseManager.CoreFramework
{
    public partial class ResetLockedUser : UserControl
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
        
            protected void DgPortalUsersDeleteCommand(Object source, DataGridCommandEventArgs e)
            {
                ErrorDisplay1.ClearError();
                //if (!Page.User.IsInRole("PortalAdmin"))
                //{
                //    ErrorDisplay1.ShowError("Sorry: You are not authorized to delete a user");
                //    return;
                //}
                try
                {
                    dgPortalUsers.SelectedIndex = e.Item.ItemIndex;
                    var portaluserId = (DataCheck.IsNumeric(dgPortalUsers.DataKeys[e.Item.ItemIndex].ToString())) ? long.Parse(dgPortalUsers.DataKeys[e.Item.ItemIndex].ToString()) : 0;
                    if (portaluserId < 1)
                    {
                        ErrorDisplay1.ShowError("Invalid Selection");
                        return;
                    }

                    int userId = int.Parse(((HiddenField)dgPortalUsers.SelectedItem.FindControl("hndUId")).Value.ToString(CultureInfo.InvariantCulture));
                    if (userId < 1)
                    {
                        ErrorDisplay1.ShowError("No User Item Selected");
                        return;
                    }
                    string userName = ((Label)dgPortalUsers.SelectedItem.FindControl("lblUserName")).Text;

                    if (userName.Length < 2)
                    {
                        ErrorDisplay1.ShowError("Invalid UserName!");
                        return;
                    }
                   
                    var mUser = Membership.GetUser(userName, false);
                    if (mUser == null)
                    {
                        ErrorDisplay1.ShowError("Username not found!");
                        return;
                    }
                    //if (HttpContext.Current.User.Identity.Name.ToLower() == mName.ToLower())
                    //{
                    //    ErrorDisplay1.ShowError("You cannot delete the current user!");
                    //    return;
                    //}

                    var ret = new PortalServiceManager().UpdateLockedUser(userId);

                    if (!ret)
                    {
                        ErrorDisplay1.ShowError("Unsuccessful Reset Operation!");
                        return;
                    }


                    string newPassword;
                    try
                    {
                        newPassword = mUser.ResetPassword();
                    }
                    catch (MembershipPasswordException ex)
                    {
                        ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                        ErrorDisplay1.ShowError("Invalid password answer. Please re-enter and try again.");
                        return;
                    }

                    var body = string.Format("Password reset. Your new password is: {0}", Server.HtmlEncode(newPassword));

                   if(Mailsender(mUser.Email,"ExpenseManager : User Credential Reset.", body))
                   {
                       BindUsersList();
                       ErrorDisplay1.ShowSuccess("User Record has been Reset");
                   }

                }
                catch (Exception ex)
                {
                    ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                    ErrorDisplay1.ShowError(ex.Message);
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
                                ErrorDisplay1.ShowError("A notification could not be sent to the User.");
                                return;
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
        #endregion
        #region Helper Methods
            #region ROLE SETTING
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
            #endregion
            #region USER
                protected void BindUsersList()
                {
                    try
                    {
                        var mlist = new PortalServiceManager().GetLockedPortalUser();
                        if(mlist == null)
                        {
                            Session["_userList"] = new List<portaluser>();
                            dgPortalUsers.DataSource =new List<portaluser>();
                            dgPortalUsers.DataBind();
                            ErrorDisplay1.ShowError("No Locked User Found");
                            return;
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