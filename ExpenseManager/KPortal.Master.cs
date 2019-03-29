using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExpenseManager.CoreFramework;
using ExpenseManager.CoreFramework.GridPager;


namespace ExpenseManager
{
    public partial class KPortal : System.Web.UI.MasterPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");

            if (!Page.IsPostBack)
            {
                try
                {

                    string username = HttpContext.Current.User.Identity.Name;
                    portaluser mUser = new PortalServiceManager().GetPortalUser(username);

                    if (mUser != null)
                    {
                        if (mUser.PortalUserId < 1 || mUser.UserId < 1)
                        {
                            Response.Redirect("~/Login.aspx");

                        }
                    }
                    else
                    {
                        Response.Redirect("~/Login.aspx");
                    }
                    Session["myCurrentUser.kPortal"] = mUser.PortalUserId;
                }
                catch
                {
                    Response.Redirect("~/Login.aspx");
                }
            }

        }
        protected void topMenu_MenuItemDataBound(object sender, MenuEventArgs e)
        {
            try
            {
                var currentNode = e.Item.DataItem as SiteMapNode;
                if (currentNode == null) { return; }
                var selectedNode = SiteMap.CurrentNode;
                if (selectedNode == null) { return; }

                if (SiteMap.CurrentNode != null && SiteMap.CurrentNode.IsDescendantOf(currentNode))
                {
                    e.Item.Text = String.Format("<span class='dynamicSelected'>{0}</span>", e.Item.Text);
                }
                if (currentNode == selectedNode)
                {
                    e.Item.Text = String.Format("<span class='dynamicSelected'>{0}</span>", e.Item.Text);
                }
                if (selectedNode.IsDescendantOf(currentNode))
                {
                    e.Item.Text = String.Format("<span class='dynamicSelected'>{0}</span>", e.Item.Text);
                }
            }
            catch (Exception){}
            
            

            
            
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {


            if (Request.ServerVariables["http_user_agent"].IndexOf("Safari", StringComparison.CurrentCultureIgnoreCase) != -1)
                Page.ClientTarget = "uplevel";
        }
        protected void setRef(object sender, EventArgs e)
        {
            if (NavigationMenu.SelectedItem != null) Session["lastTopMenuItemDataPath"] = NavigationMenu.SelectedItem.DataPath;

        }

        protected void getRef(object sender, EventArgs e)
        {
            string ldp = Session["lastTopMenuItemDataPath"] == null ? "" : Session["lastTopMenuItemDataPath"].ToString();
            if (ldp != null) foreach (MenuItem m in NavigationMenu.Items) m.Selected = (m.DataPath == ldp); //&& Menu2.Items.Count != 0
        }
        protected override void AddedControl(Control control, int index)
        {

            if (Request.ServerVariables["http_user_agent"].IndexOf("Safari", StringComparison.CurrentCultureIgnoreCase) != -1)
                this.Page.ClientTarget = "uplevel";

            base.AddedControl(control, index);
        }

    }
}
