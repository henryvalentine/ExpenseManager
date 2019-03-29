<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageUser.ascx.cs" Inherits="ExpenseManager.CoreFramework.SiteAdmin.ManageUser" %>
<%@ Register TagPrefix="uc1" TagName="ErrorDisplay" Src="~/CoreFramework/ErrorControl/ErrorDisplay.ascx" %>
<link href="~/App_Themes/Default/ControlStyles.css" rel="stylesheet" type="text/css" />

<div class="dvContainer">
 <h2> Manage Portal Users</h2>
    <div style="padding-bottom: 0px; width: 98%"><uc1:ErrorDisplay ID="ErrorDisplay1" runat="server" /></div>
     <ajaxToolkit:ModalPopupExtender ID="mpeDisplayJobDetails" runat="server"  BackgroundCssClass="popupBackground"   CancelControlID="btnCancel" DropShadow="false" PopupControlID="detailDiv" TargetControlID="btnShowJobDetails" ></ajaxToolkit:ModalPopupExtender>
     <input type="button" id="btnShowJobDetails" runat="server" style="display: none"/>
	 <div class="single-form-display" style="width:35%; height: auto; border: 0 groove transparent; border-radius: 5px; display: none" runat="server" id="detailDiv">
		<fieldset>
			<legend>User Details</legend>
			<table id="tbUserInfo" style="width:100%; padding: 3px" runat="server">
					<tr>
					<td class="tdpad">User Name
                          <asp:TextBox ID="txtUserName" runat="server" CssClass="text-box" ReadOnly="True" ValidationGroup="regValidation"></asp:TextBox>
					</td>
					</tr>
					<tr>
						<td class="tdpad" style="">
							Email <asp:RequiredFieldValidator ID="req1" runat="server" ControlToValidate="txtEmail" ErrorMessage="" ValidationGroup="regValidation">* Required</asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegExpr1" runat="server"  ControlToValidate="txtEmail" ErrorMessage=""  ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"  ValidationGroup="regValidation">* Invalid Email</asp:RegularExpressionValidator>
						<asp:TextBox ID="txtEmail" runat="server" CssClass="text-box" ReadOnly="false" ></asp:TextBox>
						</td>
					</tr>
					 <tr>
						<td class="tdpad" style="">
							Date Registered
							<asp:TextBox ID="txtCreationDate" runat="server" CssClass="text-box" ReadOnly="True" ></asp:TextBox>
						</td>
					</tr>
                    <tr class="clear">
                        <td></td>
                    </tr>
                    <tr>
                       <td colspan="2" style="width:100%; padding-bottom: 2px; margin-bottom: 5px; border-bottom: solid 1px #999; text-shadow: 0 0 2px #ccc;">Roles</td>
                    </tr>
                    <tr>
                        <td style="width:100%; padding-top: 8px" colspan="2">
						   <div style="width: 100%; background-color: #f1f1f1; height: auto">
								<asp:CheckBoxList  ID="chkRoles" CssClass="checkBoxListWrap" runat="server"  Width="100%" TextAlign="Right" CellPadding="-1" CellSpacing="-1" RepeatLayout="Table"  RepeatDirection="Horizontal" RepeatColumns="2"></asp:CheckBoxList>
							</div>
						</td>
                    </tr>

		
				
					<tr>
						<td style="width:100%" >
								<asp:CheckBox ID="chkActive" CssClass="checkBoxListWrap" runat="server" Text="Active?" TextAlign="Left" />
						</td>
					</tr>
					<tr><td style="height: 5px"></td></tr>
					<tr>
						<td style="width:100%; text-align: right; vertical-align: top" colspan="2">
							<asp:Button ID="btnSubmit" runat="server" Text="Update" CssClass="customButton" ValidationGroup="regValidation"  CommandArgument="1" OnClick="BtnUpdateRecordClick" />
							 &nbsp;&nbsp;&nbsp;<input id="btnCancel" value="Close" class="customButton" style="width: 50px" />
						</td>
				</tr>
			</table>
		</fieldset>
	</div>	
    <div>
   </div>
    <div  style="width: 100%;" runat="server" id="listDV" class="gridDiv">
	    <table style="width: 100%">
			  <tr class="divBackground">
				  <td style="width:100%" class="tdpadd2" >
						    <label class="label"> Registered Portal Users</label>   
				  </td>
		          <tr>
					<td style="width: 100%" colspan="2">
						<asp:DataGrid ID="dgPortalUsers" runat="server" AutoGenerateColumns="False" CellPadding="1" CellSpacing="2"  DataKeyField="UserName"  GridLines="None" ShowFooter="True" Width="100%"
						OnDeleteCommand="DgPortalUsersDeleteCommand" OnEditCommand="DgPortalUsersEditCommand">
						<FooterStyle CssClass="gridFooter" />
						<AlternatingItemStyle CssClass="alternatingRowStyle" />
						<ItemStyle CssClass="gridRowItem" />
						<HeaderStyle CssClass="gridHeader" />
						<Columns>
							<asp:TemplateColumn HeaderText="No.">
								<HeaderStyle HorizontalAlign="Left" Width="4%" CssClass="tdpadtop" />
								<ItemStyle CssClass="lDisplay" HorizontalAlign="center" />
								<ItemTemplate>
									<asp:Label ID="lblSNo" runat="server" CssClass="xPlugTextAll" 
										Text="<%# ((dgPortalUsers.PageSize*dgPortalUsers.CurrentPageIndex) + Container.ItemIndex + 1)%>">
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="User Name" >
								<HeaderStyle HorizontalAlign="left" Width="20%" CssClass="tdpadtop" />
								<ItemStyle HorizontalAlign="left" />
								<ItemTemplate>
									<asp:linkbutton ID="lblUserName" runat="server" CssClass="linkStyle" ForeColor="green"
										Text='<%# (DataBinder.Eval(Container.DataItem, "UserName")) %>' CommandName="Edit" CommandArgument='<%# (DataBinder.Eval(Container.DataItem,"UserName")) %>' CausesValidation="false" >
									</asp:linkbutton>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Email" >
								<HeaderStyle HorizontalAlign="left" Width="15%" CssClass="tdpadtop" />
								<ItemStyle HorizontalAlign="left" />
								<ItemTemplate>
									<asp:label ID="lblEmail" runat="server"  CssClass="lDisplay" 
										Text='<%# (DataBinder.Eval(Container.DataItem, "Email")) %>' >
									</asp:label>
								</ItemTemplate>
							</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Creation Date" >
								<HeaderStyle HorizontalAlign="left" Width="20%" CssClass="tdpadtop" />
								<ItemStyle HorizontalAlign="left" />
								<ItemTemplate>
									<asp:label ID="lblCreationDate" runat="server" CssClass="lDisplay"
										Text='<%# (DataBinder.Eval(Container.DataItem, "CreationDate")) %>' >
									</asp:label>
								</ItemTemplate>
							</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Active" >
								<HeaderStyle HorizontalAlign="center" Width="8%"  CssClass="tdpadtop"/>
								<ItemStyle HorizontalAlign="center" />
								<ItemTemplate>
									<asp:label ID="lblApproved" runat="server"  CssClass="lDisplay" 
										Text='<%# (DataBinder.Eval(Container.DataItem, "IsApproved")) %>' >
									</asp:label>
								</ItemTemplate>
							</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Last Login" >
								<HeaderStyle HorizontalAlign="left" Width="20%" />
								<ItemStyle HorizontalAlign="left" />
								<ItemTemplate>
									<asp:label ID="lblLastLogin" runat="server"  CssClass="lDisplay" 
										Text='<%# (DataBinder.Eval(Container.DataItem, "LastLoginDate")) %>' >
									</asp:label>
								</ItemTemplate>
							</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="User Roles" >
								<HeaderStyle HorizontalAlign="left" Width="15%" CssClass="tdpadtop" />
								<ItemStyle HorizontalAlign="left" />
								<ItemTemplate>
									<asp:label ID="lblUserRoles" runat="server" CssClass="lDisplay"
										Text='<%# GetRoles(DataBinder.Eval(Container.DataItem, "UserName").ToString()) %>' >
									</asp:label>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Delete">
								<HeaderStyle HorizontalAlign="center" Width="3%" CssClass="tdpadtop" />
								<ItemStyle HorizontalAlign="center" />
								<ItemTemplate>
									<table align="center" cellpadding="0" cellspacing="0" width="100%">
										<tr>
											<td width="100%" align="center">
												<asp:ImageButton ID="imgDelete" runat="server" AlternateText="Delete Portal User" OnClientClick="javascript:return confirm('Are you sure you want to delete this item from the list?')"
													CausesValidation="False" CommandArgument="1" CommandName="Delete" ImageUrl="~/App_Themes/Default/Images/btn_delete_new.gif" ToolTip="Delete Portal User" style="cursor:hand" />
											</td>
										</tr>
									</table>
								</ItemTemplate>
							</asp:TemplateColumn>
							</Columns>
						<PagerStyle HorizontalAlign="Right" Mode="NumericPages" />
					</asp:DataGrid>
					</td>
				</tr>
			</table>	
	   </div>
</div>
	 