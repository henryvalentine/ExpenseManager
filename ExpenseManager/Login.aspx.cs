using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ExpenseManager.CoreFramework;

namespace ExpenseManager
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)

            {
              
                var qString = Request.QueryString[".rdnOut"];
                if (string.IsNullOrEmpty(qString)){return;}
                if (qString.Length < 1){return;}
                var id = int.Parse(qString);
                if (id < 1){return;}
                
                if (id == 1)
                {
                   
                    FormsAuthentication.SignOut();
                    Session.Abandon();

                  var cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "")
                                      {Expires = DateTime.Now.AddYears(-1)};
                    Response.Cookies.Add(cookie1);

                    var cookie2 = new HttpCookie("ASP.NET_SessionId", "") {Expires = DateTime.Now.AddYears(-1)};
                    Response.Cookies.Add(cookie2);
                    string inQty = ".rdnOut=0";
                   Response.Redirect("Login.aspx");
                    
                }

             
               
            }
        }
    }

}

