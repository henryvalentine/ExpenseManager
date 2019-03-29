using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExpenseManager.CoreFramework;
using ExpenseManager.CoreFramework.AlertControl;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject.CustomizedASPBusinessObject;
using xPlug.BusinessService.CustomizedASPBusinessService;

namespace ExpenseManager.ExpenseMgt
{
    public partial class FrmManageApprover : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           if(!IsPostBack)
           {
               LoadserList();
           }
        }

        protected void DgPortalUsersItemCommand(object source, DataGridCommandEventArgs e)
        {
            dgPortalUsers.SelectedIndex = e.Item.ItemIndex;

            var id = (DataCheck.IsNumeric(dgPortalUsers.DataKeys[e.Item.ItemIndex].ToString())) ? int.Parse(dgPortalUsers.DataKeys[e.Item.ItemIndex].ToString()) : 0;

            if (id < 1)
            {
                ConfirmAlertBox1.ShowMessage("Invalid Record Selection!", ConfirmAlertBox.PopupMessageType.Error);
                return;
            }

            var portalUser = new PortalServiceManager().GetPortalUserById(id);

            if (portalUser == null || portalUser.PortalUserId < 1)
            {
                ConfirmAlertBox1.ShowMessage("Invalid record selection.", ConfirmAlertBox.PopupMessageType.Error);
                return;
            }

            var user = Membership.GetUser(portalUser.UserId);

            if (user == null)
            {
                ConfirmAlertBox1.ShowMessage("Invalid record selection.", ConfirmAlertBox.PopupMessageType.Error);
                return;
            }

            var status = (portalUser.Status) ? 1 : 0;
            var email = user.Email;
                                   
            var delegateService = new ApprovalDelegateService();
            
            if (!delegateService.AddDelegateApprover(portalUser.PortalUserId, status, email))
            {
               ConfirmAlertBox1.ShowMessage("User could not be added as an Approver. Please try again later or contact the Administrator.", ConfirmAlertBox.PopupMessageType.Error);
               return;
            }

            ConfirmAlertBox1.ShowSuccessAlert("User was successfully assigned the Approver role.");
          
        }

        private void LoadserList()
        {
            try
            {
                var mlist = new PortalServiceManager().GetPortalUserList();
                if (mlist == null)
                {
                    dgPortalUsers.DataSource = new List<portaluser>();
                    dgPortalUsers.DataBind();
                }

                var userList = Membership.GetAllUsers();

                if (userList == null || userList.Count == 0)
                {
                    ConfirmAlertBox1.ShowMessage("Executive Officer list is empty.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

               var executiveOfficerRoleList = (from MembershipUser member in userList where Roles.IsUserInRole(member.UserName, "ExecutiveOfficer") select member).ToList();

               if (!executiveOfficerRoleList.Any())
                {
                    ConfirmAlertBox1.ShowMessage("The Transaction Approver list could not be retrieved. Your Transaction request notification cannot be sent.\n Approval of your request may be delayed.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var executiveOfficerList = new List<portaluser>();

                foreach (MembershipUser executive in executiveOfficerRoleList)
                {
                    if (mlist != null)
                    {
                        var executiveOfficer = mlist.Find(m => executive.ProviderUserKey != null && m.UserId == (int) executive.ProviderUserKey);
                        if (executiveOfficer == null || executiveOfficer.PortalUserId < 1)
                        {
                           continue;
                        }
                        executiveOfficerList.Add(executiveOfficer);
                    }
                }

                dgPortalUsers.DataSource = executiveOfficerList;
                dgPortalUsers.DataBind();
            }
            catch (Exception)
            {
                dgPortalUsers.DataSource = new List<portaluser>();
                dgPortalUsers.DataBind();
            }


        }
    }
}