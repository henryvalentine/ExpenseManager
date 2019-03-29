using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using kPortal.CoreUtilities;
using DataCheck = XPLUG.WEBTOOLS.DataCheck;
using ErrorManager = XPLUG.WEBTOOLS.ErrorManager;
using DateMap = XPLUG.WEBTOOLS.DateMap;

namespace ExpenseManager.CoreFramework.PortalAdmin
{
    public partial class SiteMapMgt : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                Session["_mlist"] = null;
                this.btnSubmit.CommandArgument = "1"; // Add Item;
                ViewState["mRecordID"] = 0;
                //HideMainDvs();
                BindSitemMapItemList();
                var xy = Request.Url.PathAndQuery;
                if (!string.IsNullOrEmpty(xy))
                {
                     Session["_thisPath"] = xy;
                }
               
            }
        }

        #region Page Events
        protected void DgPortalSiteMapDeleteCommand(Object source, DataGridCommandEventArgs e)
        {
            ErrorDisplay1.ClearError();
            try
            {
                dgPortalSiteMap.SelectedIndex = e.Item.ItemIndex;
                int id = (DataCheck.IsNumeric(dgPortalSiteMap.DataKeys[e.Item.ItemIndex].ToString())) ? int.Parse(dgPortalSiteMap.DataKeys[e.Item.ItemIndex].ToString()) : 0;
                if (id == 1)
                {
                    ErrorDisplay1.ShowError("The Root Tab Item Cannot Be Deleted");
                    return;
                }

                RemoveItem(id);
                Reset();
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }
        }

        protected void DgPortalSiteMapEditCommand(Object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            ErrorDisplay1.ClearError();
            try
            {
                dgPortalSiteMap.SelectedIndex = e.Item.ItemIndex;
                int id = (DataCheck.IsNumeric(dgPortalSiteMap.DataKeys[e.Item.ItemIndex].ToString())) ? int.Parse(dgPortalSiteMap.DataKeys[e.Item.ItemIndex].ToString()) : 0;
                if (id == 1)
                {
                    ErrorDisplay1.ShowError("The Root Tab Item Cannot be edited");
                    return;
                }

                ErrorDisplay1.ClearControls(tbUserInfo);
                txtLink.Text = ((Label)dgPortalSiteMap.SelectedItem.FindControl("lblLink")).Text;
                txtDescription.Text = ((Label)dgPortalSiteMap.SelectedItem.FindControl("lblDescription")).Text;
                txtTabName.Text = ((LinkButton)dgPortalSiteMap.SelectedItem.FindControl("lblTitle")).Text;
                ViewState["mRecordID"] = id;
                BindSiteMapItems();
                BindRoleList();

                string mRoles = ((Label)dgPortalSiteMap.SelectedItem.FindControl("lblRoles")).Text;
                CheckRoles(mRoles);

                int k = (DataCheck.IsNumeric(((Label)dgPortalSiteMap.SelectedItem.FindControl("lblParent")).Text)) ? int.Parse(((Label)dgPortalSiteMap.SelectedItem.FindControl("lblParent")).Text) : 0;
                if (k > 0)
                {
                    ddlTabParent.SelectedValue = k.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    ddlTabParent.SelectedIndex = 0;
                }

                int wx = (DataCheck.IsNumeric(((Label)dgPortalSiteMap.SelectedItem.FindControl("lblTabType")).Text)) ? int.Parse(((Label)dgPortalSiteMap.SelectedItem.FindControl("lblTabType")).Text) : 0;
                if (wx > 0)
                {
                    ddlTabType.SelectedValue = wx.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    ddlTabType.SelectedIndex = 0;
                }

                int px = (DataCheck.IsNumeric(((Label)dgPortalSiteMap.SelectedItem.FindControl("lblTabOrder")).Text)) ? int.Parse(((Label)dgPortalSiteMap.SelectedItem.FindControl("lblTabOrder")).Text) : 0;
                if (px > 0)
                {
                    BindOrders();
                    if (ddlTabOrder.Items.Count > 0)
                    {
                        ddlTabOrder.SelectedValue = px.ToString(CultureInfo.InvariantCulture);
                    }
                }

                //HideMainDvs();
                //this.tbUserInfo.Visible = true;
                //this.detailDiv.Visible = true;
                
                btnSubmit.CommandArgument = "2"; // Update Item
                btnSubmit.Text = "Update";
                mpeDisplayJobDetails.Show();
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }
        }

        protected void BtnAddItemClick(Object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            ErrorDisplay1.ClearControls(tbUserInfo);
            BindSiteMapItems();
            BindRoleList();
            BindOrders();
            //HideMainDvs();
            btnSubmit.CommandArgument = "1"; // Add Item
            btnSubmit.Text = "Add New Tab";
            mpeDisplayJobDetails.Show();
            //this.tbUserInfo.Visible = true;
            //this.detailDiv.Visible = true;

        }

        protected void BtnCancelClick(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            //HideMainDvs();
            //this.listDV.Visible = true;
        }
        protected void BtnSubmitClick(Object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            try
            {
                if (ddlTabParent.SelectedIndex == 0)
                {
                    ErrorDisplay1.ShowError("Please select parent tab");
                    return;
                }

                if (txtTabName.Text == "")
                {
                    ErrorDisplay1.ShowError("Tab name is required");
                    return;
                }

                if (ddlTabType.SelectedIndex == 0)
                {
                    ErrorDisplay1.ShowError("Please select tab type");
                    return;
                }


                if (ValidateRole() < 1)
                {
                    ErrorDisplay1.ShowError("You must select at least one role");
                    return;
                }


                string mRoles = String.Empty;
                for (int i = 0; i < chkRoles.Items.Count; i++)
                {
                    if (chkRoles.Items[i].Selected)
                        mRoles += ";" + chkRoles.Items[i].Value;
                }

                if (mRoles.Length > 0)
                {
                    if (mRoles.StartsWith(";"))
                    {
                        mRoles = mRoles.Substring(1);
                    }
                    if (mRoles.EndsWith(";"))
                    {
                        mRoles = mRoles.Substring(0, mRoles.Length - 1);
                    }
                }
                else
                {
                    ErrorDisplay1.ShowError("Tab name is required");
                    return;
                }


                var mInfo = new sitemap
                                {
                                    DateCreated = DateMap.GetLocalDate(),
                                    Description = txtDescription.Text,
                                    Parent = (DataCheck.IsNumeric(ddlTabParent.SelectedValue.ToString(CultureInfo.InvariantCulture)))? int.Parse(ddlTabParent.SelectedValue.ToString(CultureInfo.InvariantCulture)): 0,
                                    Roles = mRoles,
                                    Title = txtTabName.Text,
                                    TabType = (DataCheck.IsNumeric(ddlTabType.SelectedValue.ToString(CultureInfo.InvariantCulture)))? int.Parse(ddlTabType.SelectedValue.ToString(CultureInfo.InvariantCulture)): 0,
                                    TabOrder = (DataCheck.IsNumeric(ddlTabOrder.SelectedValue.ToString(CultureInfo.InvariantCulture)))? int.Parse(ddlTabOrder.SelectedValue.ToString(CultureInfo.InvariantCulture)): 0,
                                    Url = txtLink.Text
                                };


                switch (int.Parse(btnSubmit.CommandArgument))
                {
                    case 1: //Add
                        long k = (new PortalServiceManager()).AddSiteMap(mInfo);
                        if (k < 1)
                        {
                            if (k == -1)
                            {
                                ErrorDisplay2.ShowError("Duplicate Tab Name! This Tab Name is already registered");
                                mpeDisplayJobDetails.Show();
                                return;
                            }
                            ErrorDisplay2.ShowError("Item was not be added");
                            mpeDisplayJobDetails.Show();
                            return;
                        }
                        Reset();
                        //HideMainDvs();
                        BindSitemMapItemList();
                        //this.listDV.Visible = true;
                        ErrorDisplay1.ShowSuccess("Item Was Added Successfully");
                        mpeDisplayJobDetails.Hide();
                        break;
                    case 2: //Update
                        int id = (DataCheck.IsNumeric(ViewState["mRecordID"].ToString())) ? int.Parse(ViewState["mRecordID"].ToString()) : 0;
                        if (id < 1)
                        {
                            ErrorDisplay2.ShowError("Process Validation Failed!");
                            mpeDisplayJobDetails.Show();
                            return;
                        }
                        mInfo.ID = id;
                        int flag = 0;
                        if (!(new PortalServiceManager()).UpdateSiteMap(mInfo, ref flag))
                        {
                            if (flag == -1)
                            {
                                ErrorDisplay2.ShowError("Duplicate Tab Name! This Tab Name is already registered");
                                mpeDisplayJobDetails.Show();
                                return;
                            }
                            ErrorDisplay2.ShowError("Update failed!"); 
                            mpeDisplayJobDetails.Show();
                            return;
                        }
                        Reset();
                        BindSitemMapItemList();
                        btnSubmit.Text = "Add New Tab";
                        mpeDisplayJobDetails.Hide();
                        ErrorDisplay1.ShowSuccess("Item Was Updated Successfully");
                        break;
                }

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }
        }
        private void Reset()
        {
            bool error = false;
            try
            {
                HttpRuntime.UnloadAppDomain();
            }
            catch
            {
                error = true;
            }
            if (!error)
            {
                if (!string.IsNullOrEmpty(Session["_thisPath"].ToString()))
                {
                    HttpContext.Current.Response.Redirect(Session["_thisPath"].ToString());
                }
               
            }
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Request.PhysicalApplicationPath != null)
                {
                    string configPath = System.IO.Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "\\web.config");
                    try
                    {
                        System.IO.File.SetLastWriteTimeUtc(configPath, DateTime.UtcNow);
                    }
                    catch(Exception)
                    {
                        
                    }
                }
            }

        }
        protected void DdlTabParentSelectedIndexChanged(Object sender, EventArgs e)
        {
            ArrayList mArry = GetTabOrders(int.Parse(ddlTabParent.SelectedValue.ToString(CultureInfo.InvariantCulture)));
            if (mArry == null)
            {
                ddlTabOrder.Items.Insert(0, new ListItem("1", "1"));
                ddlTabOrder.SelectedIndex = 0;
                mpeDisplayJobDetails.Show();
                return;
            }
            try
            {
                ddlTabOrder.DataSource = mArry;
                ddlTabOrder.DataTextField = "Name";
                ddlTabOrder.DataValueField = "ID";
                ddlTabOrder.DataBind();
                ddlTabOrder.SelectedIndex = 0;
                mpeDisplayJobDetails.Show();
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay2.ShowError(ex.Message);
                mpeDisplayJobDetails.Show();
            }
        }
        #endregion

        #region Class Helpers

        protected void HideMainDvs()
        {
            //this.detailDiv.Visible = false;
            //this.listDV.Visible = false;

        }
        protected void BindOrders()
        {
            var mArry = new ArrayList();
            for (int i = 1; i < 16; i++)
            {
                var mVal = new NameAndValueManager { ID = i, Name = i.ToString(CultureInfo.InvariantCulture) };
                mArry.Add(mVal);
            }
            ddlTabOrder.DataSource = mArry;
            ddlTabOrder.DataTextField = "Name";
            ddlTabOrder.DataValueField = "ID";
            ddlTabOrder.DataBind();
        }

        protected void BindSitemMapItemList()
        {
            try
            {
                ErrorDisplay1.ClearError();
                List<sitemap> mList = (new PortalServiceManager()).GetSiteMapList();
                if (mList == null || mList.Count == 0)
                {
                    dgPortalSiteMap.DataSource = new List<sitemap>();
                    dgPortalSiteMap.DataBind();
                }
                if (mList != null)
                {
                    mList[0].Title = "ROOT";
                    mList[0].Description = "ROOT";
                    dgPortalSiteMap.DataSource = mList;
                }
                dgPortalSiteMap.DataBind();
                //HideMainDvs();
                //this.listDV.Visible = true;
                Session["_mlist"] = mList;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }

        }

        protected void BindSiteMapItems()
        {

            try
            {
                ErrorDisplay1.ClearError();
                List<sitemap> mList = (new PortalServiceManager()).GetSiteMapList();
                if (mList == null || mList.Count == 0)
                {
                    dgPortalSiteMap.DataSource = new List<sitemap>();
                    dgPortalSiteMap.DataBind();
                }
                if (mList != null)
                {
                    mList[0].Title = "ROOT";
                    ddlTabParent.DataSource = mList;
                }
                ddlTabParent.DataTextField = "Title";
                ddlTabParent.DataValueField = "ID";
                ddlTabParent.DataBind();
                ddlTabParent.Items.Insert(0, (new ListItem("-- Please Select --", "0")));
                ddlTabParent.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }
        }

        protected void BindRoleList()
        {
            try
            {
                chkRoles.DataSource = Roles.GetAllRoles();
                chkRoles.DataBind();
                chkRoles.Items.Insert(chkRoles.Items.Count, new ListItem("All Users", "*"));
                for (int i = 0; i < chkRoles.Items.Count; i++)
                {
                    if (chkRoles.Items[i].Text == "PortalAdmin")
                    {
                        chkRoles.Items[i].Selected = true;
                        chkRoles.Items[i].Enabled = false;
                    }
                    else
                    {
                        chkRoles.Items[i].Enabled = true;
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }

        }

        public string GetRoles(string userName)
        {
            try
            {
                string myRoles = String.Empty;
                string[] mRoles = Roles.GetRolesForUser(userName);
                if (mRoles.Length > 0)
                    myRoles = mRoles.Aggregate(myRoles, (current, mRole) => current + ("; " + mRole));
                if (myRoles.StartsWith(";"))
                    myRoles = myRoles.Substring(1);
                if (myRoles.EndsWith(";"))
                    myRoles = myRoles.Substring(0, myRoles.Length - 1);

                return myRoles;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return "";
            }
        }

        protected void CheckRoles(string mxRole)
        {

            try
            {
                if (mxRole.Length == 0)
                    return;
                mxRole = mxRole.Trim();
                if (mxRole.IndexOf(";", StringComparison.Ordinal) > 0)
                {
                    string[] mRoles = mxRole.Split(';');
                    if (mRoles.Length > 0)
                        foreach (string mRole in mRoles)
                        {
                            for (int i = 0; i < chkRoles.Items.Count; i++)
                            {
                                if (chkRoles.Items[i].Value == mRole)
                                {
                                    chkRoles.Items[i].Selected = true;
                                    break;
                                }
                                if (mRole == "*")
                                {
                                    chkRoles.Items[chkRoles.Items.Count - 1].Selected = true;
                                    break;
                                }
                            }
                        }

                }
                else if (mxRole[0].ToString(CultureInfo.InvariantCulture) == "*")
                {
                    chkRoles.Items[chkRoles.Items.Count - 1].Selected = true;
                }
                else
                {
                    for (int j = 0; j < chkRoles.Items.Count; j++)
                    {
                        if (chkRoles.Items[j].Value == mxRole[0].ToString(CultureInfo.InvariantCulture))
                        {
                            chkRoles.Items[j].Selected = true;
                            break;
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
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

        protected void RemoveItem(int id)
        {
            try
            {
                if (id > 0)
                    if (!(new PortalServiceManager()).DeleteSiteMap(id))
                    {
                        ErrorDisplay1.ShowError("Process Failed!");
                        return;
                    }
                BindSitemMapItemList();
                ErrorDisplay1.ShowSuccess("Portal SiteMap Item Was Removed");
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }

        }

        protected ArrayList GetTabOrders(int parentId)
        {
            var mArry = new ArrayList();
            int maxId = 0;
            maxId = parentId > 0 ? (new PortalServiceManager()).GetMaxTabOrder(parentId) : 0;
            try
            {
                for (int i = maxId + 1; i < 16; i++)
                {
                    var mValue = new NameAndValueManager { ID = i, Name = i.ToString(CultureInfo.InvariantCulture) };
                    mArry.Add(mValue);
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
            return mArry;
        }

        #endregion
    }
}