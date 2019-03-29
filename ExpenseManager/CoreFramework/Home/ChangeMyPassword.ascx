<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangeMyPassword.ascx.cs" Inherits="ExpenseManager.CoreFramework.Home.ChangeMyPassword" %>
<%@ Register TagPrefix="uc1" TagName="ErrorDisplay" Src="~/CoreFramework/ErrorControl/ErrorDisplay.ascx" %>
<link href="../../App_Themes/Default/cbtControlStyles.css" rel="stylesheet" type="text/css" />
<link href="../../App_Themes/Default/ControlStyles.css" rel="stylesheet" type="text/css" />
<div style="width: 500px;margin-left: auto; margin-right: auto">
<div class="dvContainer" style="margin-left: auto; margin-right: auto;">
   <%-- <h2 style="">Change Password</h2>--%>
     <div><asp:Panel ID="Panel2" runat="server" Width="98%">
			<uc1:ErrorDisplay ID="ErrorDisplay1" runat="server" /></asp:Panel>
	 </div>
	 <div style="width:100%;margin-top: 30px;" runat="server" id="detailDiv">
		<fieldset  style="">
			<legend style="">Change Password</legend>
            <table id="tbUserInfo" style="width:100%; padding: 3px" runat="server">
					<tr>
						<td style="width:100%">
							Current Password <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCurrentPassword" ErrorMessage="" ValidationGroup="regValidation" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass">* Required</asp:RequiredFieldValidator>
							<asp:TextBox ID="txtCurrentPassword" runat="server" CssClass="text-box" ReadOnly="False" TextMode="Password" ValidationGroup="regValidation"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td style="width:100%">
							New Password <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNewPassword" ErrorMessage="" ValidationGroup="regValidation" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass">* Required</asp:RequiredFieldValidator>
							<asp:TextBox ID="txtNewPassword" runat="server" CssClass="text-box" ReadOnly="False" TextMode="Password" ValidationGroup="regValidation"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td>
							Confirm New Password <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtConfirmNewPassword" ErrorMessage="" ValidationGroup="regValidation" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass">* Required</asp:RequiredFieldValidator> <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="txtNewPassword" ControlToValidate="txtConfirmNewPassword" Display="Dynamic" ErrorMessage="" ValidationGroup="regValidation" CssClass="lDisplay" SetFocusOnError="True">* Password and Confirmation Password must match</asp:CompareValidator>
							<asp:TextBox ID="txtConfirmNewPassword" runat="server" CssClass="text-box" ReadOnly="false" TextMode="Password" ValidationGroup="regValidation"></asp:TextBox>
						</td>
					</tr>
				    <tr><td style="height: 5px"></td></tr>
					<tr>
						<td style="width:100%; text-align: right; vertical-align: top" colspan="2">
							<asp:Button ID="btnSubmit" runat="server" Text="Update" CssClass="customButton" ValidationGroup="regValidation"  CommandArgument="1" OnClick="btnSubmit_Click" />
						</td>
				</tr>
			</table>
		</fieldset>
	</div>
</div>
</div>