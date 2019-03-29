using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ExpenseManager.CoreFramework.AlertControl
{
    public partial class ConfirmAlertBox : UserControl
    {

        public event Action<bool> AlertResponse;
       
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }
        public void ShowSuccessAlert(string message)
        {
            lblSuccessHeader.Text = "Success";
            ltrSuccessMessagePopupImage.Text = "<img src='" +
            Page.ResolveUrl("~/Images/AlertIcons/success.png") + "' alt='' />";
            lblSuccessMessage.Text = message;
            mpeSuccessMessage.Show();
        }
       

        public void ShowMessage(string message, PopupMessageType popupMessageType)
        {
            if( popupMessageType ==  PopupMessageType.Confirm)
            {
                 btnOk.Visible =true ;
                 btnNo.Visible = true;
                 btnNo.Text = "No";
            
            }
            else
            {
                btnOk.Visible =false ;
                btnNo.Text = "Ok";
                btnNo.Visible = true;
               
            }
           
            ShowPopupMessage(message, popupMessageType);
        }
        
        protected void SuccessButtonAction(object sender, EventArgs e)
        {
            mpeSuccessMessage.Hide();
            
        }
        protected void GetUserAction(object sender, EventArgs e)
        {
            var but = sender as LinkButton;
            if(but == null){return;}
            var id = int.Parse(but.CommandArgument);
            if (id == 1)
            {
                AlertResponse(true);
            }
           
        }

        /// <summary>
        /// Details: Change modal popup image according to PopupMessageType
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageType"></param>
        private void ShowPopupMessage(string message, PopupMessageType messageType)
        {
            switch (messageType)
            {
                case PopupMessageType.Error:
                    lblMessagePopupHeading.Text = "Error";
                    //Render image in literal control
                    ltrMessagePopupImage.Text = "<img src='" +
                      Page.ResolveUrl("~/Images/AlertIcons/Delete.png") + "' alt='' />";
                    break;
                case PopupMessageType.Confirm:
                    lblMessagePopupHeading.Text = "Information";
                    ltrMessagePopupImage.Text = "<img src='" +
                      Page.ResolveUrl("~/Images/AlertIcons/confirm.png") + "' alt='' />";
                    break;
                case PopupMessageType.Warning:
                    lblMessagePopupHeading.Text = "Warning";
                    ltrMessagePopupImage.Text = "<img src='" +
                      Page.ResolveUrl("~/Images/AlertIcons/Warning.png") + "' alt='' />";
                    break;
                case PopupMessageType.Success:
                    lblMessagePopupHeading.Text = "Success";
                    ltrMessagePopupImage.Text = "<img src='" +
                      Page.ResolveUrl("~/Images/AlertIcons/imgSuccess.png") + "' alt='' />";
                    break;
                case PopupMessageType.Information:
                     lblMessagePopupHeading.Text = "Information";
                    ltrMessagePopupImage.Text = "<img src='" +
                      Page.ResolveUrl("~/Images/AlertIcons/Info.png") + "' alt='' />";
                    break;
                default:
                    lblMessagePopupHeading.Text = "Information";
                    ltrMessagePopupImage.Text = "<img src='" +
                      Page.ResolveUrl("~/Images/AlertIcons/Info.png") + "' alt='' />";
                    break;
            }

            lblMessagePopupText.Text = message;
            mpeMessagePopup.Show();
        }

        /// <summary>
        /// Message type enum
        /// </summary>
        public enum PopupMessageType
        {
            Error,
            Confirm,
            Warning,
            Success,
            Information
        }
    }
}