<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfirmAlertBox.ascx.cs" Inherits="ExpenseManager.CoreFramework.AlertControl.ConfirmAlertBox" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="CoreFramework/AlertControl/AlertBoxStyle.css" rel="stylesheet" type="text/css" />
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
     <ContentTemplate>
         <asp:Button runat="server" ID="btnMessagePopupTargetButton" Style="display: none;" />
          <cc1:ModalPopupExtender  ID="mpeMessagePopup" runat="server" PopupControlID="pnlMessageBox" TargetControlID="btnMessagePopupTargetButton" CancelControlID="btnCancel"  BackgroundCssClass="MessageBoxPopupBackground"></cc1:ModalPopupExtender>
          <ajaxToolkit:ModalPopupExtender  ID="mpeSuccessMessage" runat="server" PopupControlID="pnlSuccessBox" TargetControlID="btnMessagePopupTargetButton" CancelControlID="lnkCloseSuccess" BackgroundCssClass="MessageBoxPopupBackground"></ajaxToolkit:ModalPopupExtender>
       
                 <asp:Panel runat="server" ID="pnlMessageBox" BackColor="White" Width="420" Style="display: none; border-radius: 4px;
                   -moz-border-radius: 4px; -webkit-border-radius: 4px; -ms-border-radius: 4px; -o-border-radius: 4px;   border: 2px solid #959596;">
                <div class="popupHeader" style="width: 417px;">
                    <asp:Label ID="lblMessagePopupHeading" Text="Information" runat="server"></asp:Label>
                    <asp:LinkButton ID="btnCancel" runat="server" Style="float: right; margin-right: 5px;">X</asp:LinkButton>
                </div>
                <div style="max-height: 500px; width: 420px; overflow: hidden;">
                    <div style="float:left; width:380px; margin:20px;">
                        <table style="padding: 0; border-spacing: 0; border-collapse: collapse; width: 100%;">
                            <tr>
                                <td style="text-align: left; vertical-align: top; width: 11%;">
                                    <asp:Literal runat="server" ID="ltrMessagePopupImage"></asp:Literal>
                                </td>
                                <td style="width: 2%;">
                                </td>
                                <td style="text-align: left; vertical-align: top; width: 87%;">
                                    <p style="margin: 0px; padding: 0px; color: #b94a48;">
                                        <asp:Label runat="server" ID="lblMessagePopupText"></asp:Label>
                                    </p>
                                </td>
                            </tr>
                           
                            <tr>
                                <td style="text-align: right; vertical-align: top;" colspan="3">
                                     <br/>
                                    <div style="width: auto; float: right;">
                                       <span><asp:LinkButton ID="btnOk" runat="server" CssClass="popup_button" Text="Yes" CommandArgument="1" OnClick="GetUserAction"></asp:LinkButton></span> 
                                       <span> <asp:LinkButton ID="btnNo" runat="server" CssClass="popup_button" Text="No" CommandArgument="0" OnClick="GetUserAction"></asp:LinkButton></span>
                                       
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </asp:Panel>

                <asp:Panel runat="server" ID="pnlSuccessBox" BackColor="White" Width="420" Style="display: none; border-radius: 4px;
                   -moz-border-radius: 4px; -webkit-border-radius: 4px; -ms-border-radius: 4px; -o-border-radius: 4px; border: 2px solid #959596;">
                <div id="Div1" class="popupHeader2" runat="server" style="width: 417px;">
                    <asp:Label ID="lblSuccessHeader" Text="Information" runat="server"></asp:Label>
                    <asp:LinkButton ID="lnkCloseSuccess" runat="server" Style="float: right; margin-right: 5px;">X</asp:LinkButton>
                </div>
                <div style="max-height: 500px; width: 420px; overflow: hidden;">
                    <div style="float:left; width:380px; margin:20px;">
                        <table style="padding: 0; border-spacing: 0; border-collapse: collapse; width: 100%;">
                            <tr>
                                <td style="text-align: left; vertical-align: top; width: 11%;">
                                    <asp:Literal runat="server" ID="ltrSuccessMessagePopupImage"></asp:Literal>
                                </td>
                                <td style="width: 2%;">
                                </td>
                                <td style="text-align: left; vertical-align: top; width: 87%;">
                                    <p style="margin: 0px; padding: 0px; color: #23530E;">
                                        <asp:Label runat="server" ID="lblSuccessMessage"></asp:Label>
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; vertical-align: top;" colspan="3">
                                     <br/>
                                    <div style="width: auto; float: right;">
                                       <span><asp:LinkButton ID="btnSuccessOk" runat="server" CssClass="popup_button2" Text="OK" OnClick="SuccessButtonAction"></asp:LinkButton></span> 
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </asp:Panel>

            
             
   </ContentTemplate>
</asp:UpdatePanel>
