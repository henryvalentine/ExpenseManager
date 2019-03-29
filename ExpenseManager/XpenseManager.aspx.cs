using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ExpenseManager
{
    public partial class XpenseManager : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            LoadControl();
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void LoadControl()
        {
            if (this.Master == null) { return; }
            var cph = this.Master.FindControl("MainContent") as ContentPlaceHolder;


            //search for the controlID from the queryString
            var specialId = Request.QueryString[".rndChecker"];
            if (specialId != null)
            {
                if (!string.IsNullOrEmpty(specialId))
                {
                    if (specialId.Length > 0)
                    {
                        var id = int.Parse(specialId);
                        try
                        {

                            var uc = new UserControl();
                            switch (id)
                            {
                                case 1:
                                    uc = this.LoadControl("~/CoreFramework/Home/ChangeMyPassword.ascx") as UserControl;
                                    break;
                                case 2:
                                    uc = this.LoadControl("~/CoreFramework/Home/MyUserProfile.ascx") as UserControl;
                                    break;
                            }
                            if ((cph != null) && (uc != null))
                            {
                                cph.Controls.Add(uc);
                                return;
                            }
                        }
                        catch
                        {
                            return;
                        }

                    }
                }
            }

            var tabId = Request.QueryString["tabId"];
            var tabParentId = Request.QueryString["tabParentId"];

            if (string.IsNullOrEmpty(tabId)) { return; }
            if (string.IsNullOrEmpty(tabParentId)) { return; }

            var currentTabId = int.Parse(tabId);
            var currentParent = int.Parse(tabParentId);

            var currentNode = GetSelectedNode(currentTabId.ToString(CultureInfo.InvariantCulture));
            if (currentNode == null) { return; }

            //Verify current node by parent id
            var tabParent = currentNode["tabParentId"];
            if (string.IsNullOrEmpty(tabParent)) { return; }
            if (int.Parse(tabParent) != currentParent) { return; }

            //Verify if this user can access module(s) in this page  
            var roles = new string[currentNode.Roles.Count];
            currentNode.Roles.CopyTo(roles, 0);
            var moduleSource = currentNode["moduleSource"];
            if (!roles.Any(item => User.IsInRole(item)))
            {
                moduleSource = "~/CoreFramework/ErrorControl/ModuleAccessDenied.ascx";
            }

            //Get the module source

            if (string.IsNullOrEmpty(moduleSource))
            {
                return;
            }

            try
            {
                var uc = this.LoadControl(moduleSource) as UserControl;
                if ((cph != null) && (uc != null))
                {
                    cph.Controls.Add(uc);
                }
            }
            catch (Exception ex)
            {

            }

        }
        private SiteMapNode GetSelectedNode(string nodeKey)
        {
            try
            {
                if (SiteMap.Providers["MySqlSiteMapProvider"] == null) { return null; }
                var myMap = SiteMap.Providers["MySqlSiteMapProvider"].RootNode.Url;
                if (myMap == null) { return null; }
                var mNodes = SiteMap.RootNode.GetAllNodes();
                if (mNodes.Count < 1) { return null; }
                var currentNode = mNodes.Cast<SiteMapNode>().FirstOrDefault(item => item.Key == nodeKey);
                if (currentNode == null) { return null; }
                if (string.IsNullOrEmpty(currentNode.Key))
                {
                    return null;
                }
                return currentNode;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}