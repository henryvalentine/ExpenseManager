using System;
using System.Web;
using System.Web.Security;

namespace ExpenseManager.CoreFramework.Home
{
    public partial class ChangeMyPassword : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity.Name.Length < 2)
            {
                ErrorDisplay1.ShowError("Only registered users can change password!");
                detailDiv.Visible = false;
                return;
            }
            else
            {

                detailDiv.Visible = true;
            }
            if (!IsPostBack)
            {
               
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Page.Validate("regValidation");
            if (!Page.IsValid)
            {
                ErrorDisplay1.ShowError("Please provider all required information");
                return;
            }
            MembershipUser memUser = Membership.GetUser(HttpContext.Current.User.Identity.Name.Trim());
            if (memUser == null)
            {
                ErrorDisplay1.ShowError("User Profile cannot be obtained! Please try again soon");
                return;
            }
            if (!memUser.ChangePassword(this.txtCurrentPassword.Text, this.txtNewPassword.Text))
            {
                ErrorDisplay1.ShowError("Password Was Not Modified! Please check old password and try again");
                return;
            }
            ErrorDisplay1.ShowSuccess("Password Was Changed Successfully");
        }
    }
}