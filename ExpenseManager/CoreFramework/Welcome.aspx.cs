using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace ExpenseManager.CoreFramework
{
    public partial class Welcome : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             if (HttpContext.Current.User.Identity.Name == null || HttpContext.Current.User.Identity.Name.ToString(CultureInfo.InvariantCulture) == "") 
             {
                 Response.Redirect("~/CoreFramework/Login.aspx");
             }
              if (!IsPostBack)
              {
                 if(Master != null && (Label) Master.FindControl("lblTitle") != null) 
                 {
                    ((Label) Master.FindControl("lblTitle")).Text = "Welcome : " + HttpContext.Current.User.Identity.Name;
                 }
              }
    
        }
    }
}


