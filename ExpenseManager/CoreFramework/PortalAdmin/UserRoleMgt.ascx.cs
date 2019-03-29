using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using kPortal.CoreUtilities;
using DataCheck = XPLUG.WEBTOOLS.DataCheck;
using ErrorManager = XPLUG.WEBTOOLS.ErrorManager;

namespace ExpenseManager.CoreFramework.PortalAdmin
{
    public partial class UserRoleMgt : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

           if (!IsPostBack)
            {
                Session["mRoleList"] = null;
                ViewState["mRoleName"] = "";
                ViewState["mRecordID"] = 0;
                btnSubmit.CommandArgument = "1"; // Add Item;
                btnSubmit.Text = "Add New Role";
                HideTables();
                BindRoleItems();
            }
        }

        #region Page Events
        protected void DgPortalRoleDeleteCommand(Object source, DataGridCommandEventArgs e)
        {
            ErrorDisplay1.ClearError();
            try
            {
                dgPortalRole.SelectedIndex = e.Item.ItemIndex;
                int id = (DataCheck.IsNumeric(dgPortalRole.DataKeys[e.Item.ItemIndex].ToString())) ? int.Parse(dgPortalRole.DataKeys[e.Item.ItemIndex].ToString()) : 0;
                string roleName = ((LinkButton)dgPortalRole.SelectedItem.FindControl("lblRole")).Text;
                if (roleName.Length == 0)
                {
                    ErrorDisplay1.ShowError("Process Error! Please try again later");
                    return;
                }

                //Check if the role name is PortalAdmin, then terminate the process
                if (roleName.ToLower() == "PortalAdmin".ToLower())
                {
                    ErrorDisplay1.ShowError("Sorry, you can not delete this role");
                    return;
                }
                //Check to see that the current user has right to delete role
                if (!Page.User.IsInRole("PortalAdmin"))
                {
                    ErrorDisplay1.ShowError("Sorry, you are not authorized to delete a role");
                }

                //If role has registered users, remove the users from the role.
                try
                {
                    if (Roles.GetUsersInRole(roleName).Length > 0)
                    {
                        string[] mUsersInRole = Roles.GetUsersInRole(roleName);
                        Roles.RemoveUsersFromRole(mUsersInRole, roleName);
                    }

                }
                catch (Exception ex)
                {
                    ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                    ErrorDisplay1.ShowError(ex.Message);
                    return;
                }
                //'Now delete the role!
                if (Roles.DeleteRole(roleName, false))
                {
                    BindRoleItems();
                    ErrorDisplay1.ShowSuccess("Role Item Was Removed");
                }
                else
                {
                    ErrorDisplay1.ShowError("Role Item could not be removed");
                }

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay1.ShowError(ex.Message);
                return;
            }

        }
        protected void DgPortalRoleEditCommand(Object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            ErrorDisplay1.ClearError();
            try
            {
                dgPortalRole.SelectedIndex = e.Item.ItemIndex;
                int id = (DataCheck.IsNumeric(dgPortalRole.DataKeys[e.Item.ItemIndex].ToString())) ? int.Parse(dgPortalRole.DataKeys[e.Item.ItemIndex].ToString()) : 0;
                if (id < 1)
                {
                    ErrorDisplay1.ShowError("Invalid Item Selection");
                    return;
                }
                ErrorDisplay1.ClearControls(tbUserInfo);
                txtRoleName.Text = ((LinkButton)dgPortalRole.SelectedItem.FindControl("lblRole")).Text;
                ViewState["mRoleName"] = ((LinkButton)dgPortalRole.SelectedItem.FindControl("lblRole")).Text;
                ViewState["mRecordID"] = id;
                HideTables();
                btnSubmit.CommandArgument = "2"; //Update
                btnSubmit.Text = "Update Role";
                mpeDisplayJobDetails.Show();
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay1.ShowError(ex.Message);
            }
        }
        protected void BtnAddItemClick(Object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            ErrorDisplay2.ClearError();
            ErrorDisplay1.ClearControls(tbUserInfo);
            btnSubmit.CommandArgument = "1";
            btnSubmit.Text = "Add Role";
            mpeDisplayJobDetails.Show();
        }
        protected void BtnCancelClick(Object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
        }
        protected void BtnSubmitClick(Object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            try
            {
                if (txtRoleName.Text == "")
                {
                    ErrorDisplay1.ShowError("Role name is required");
                    return;
                }

                switch (int.Parse(btnSubmit.CommandArgument))
                {
                    case 1: //Add
                        var roles = Roles.GetAllRoles();
                        if (roles == null || roles.Length < 1)
                        {
                            ErrorDisplay2.ShowError("The Portal Roles could not be retrieved. Please try again soon.");
                            mpeDisplayJobDetails.Show();
                            return;
                        }

                        if (roles.Any(role => role.ToLower().Replace(" ", string.Empty.Trim()) == txtRoleName.Text.Trim().ToLower().Replace(" ", string.Empty.Trim())))
                        {
                            ErrorDisplay2.ShowError("This Portal Role already exist");
                            mpeDisplayJobDetails.Show();
                            return;
                        }
                        
                        Roles.CreateRole(txtRoleName.Text.Trim());
                        //HideTables();
                        BindRoleItems();
                       
                        ErrorDisplay1.ShowSuccess("Portal Role Was Added Successfully");
                        //this.listDV.Visible = true;
                        mpeDisplayJobDetails.Hide();
                        break;
                    case 2: //Update
                        if (Roles.DeleteRole(ViewState["mRoleName"].ToString().Trim(), false))
                        {
                            Roles.CreateRole(txtRoleName.Text.Trim());
                            //HideTables();
                            BindRoleItems();
                            
                            ErrorDisplay1.ShowSuccess("Portal Role Was Updated Successfully");
                            //this.listDV.Visible = true;
                            btnSubmit.Text = "Add New Role";
                            mpeDisplayJobDetails.Hide();
                        }
                        else
                        {
                            ErrorDisplay2.ShowError("Error Occurred! Please try again later");
                            mpeDisplayJobDetails.Show();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay1.ShowError(ex.Message);
            }
        }

        #endregion
        #region Helper Methods
        protected void HideTables()
        {
           //this.listDV.Visible = false;
        }
        protected void BindRoleItems()
        {
            var mList = new List<NameAndValueManager>();

            string[] mRoles = Roles.GetAllRoles();
            if (mRoles == null)
            {
                ErrorDisplay1.ShowError("No Portal Role Was Defined!");
                return;
            }
            if (mRoles.Length == 0)
            {
                ErrorDisplay1.ShowError("No Portal Role Was Defined!");
                return;
            }
            try
            {
                int i = 1;
                foreach (string mRole in mRoles)
                {
                    var mItem = new NameAndValueManager { ID = i, Name = mRole };
                    mList.Add(mItem);
                    i += 1;
                }
                if (mList.Count == 0)
                {
                    ErrorDisplay1.ShowError("No Portal Role Was Defined!");
                }
                Session["mRoleList"] = mList;
                dgPortalRole.DataSource = mList;
                dgPortalRole.DataBind();
                //this.listDV.Visible = true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay1.ShowError(ex.Message);
            }
        }

        #endregion
    }
}