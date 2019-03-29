<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserRoleMgt.ascx.cs" Inherits="ExpenseManager.CoreFramework.PortalAdmin.UserRoleMgt" %>
<%@ Register TagPrefix="uc1" TagName="ErrorDisplay" Src="~/CoreFramework/ErrorControl/ErrorDisplay.ascx" %>
<link href="../../App_Themes/Default/cbtControlStyles.css" rel="stylesheet" type="text/css" />

<div class="dvContainer">
     <asp:Button ID="btnShowJobDetails" runat="server" Style="display: none" Text="Show PopUp" />
    <h2 style="">Manage Portal Roles</h2>
     <div><asp:Panel ID="Panel2" runat="server" Width="98%">
			<uc1:ErrorDisplay ID="ErrorDisplay1" runat="server" /></asp:Panel>
	 </div>	
	 <div class="single-form-display" style="width:30%;  border: 0 groove transparent; border-radius: 5px; display: none"  id="detailDiv">
         <fieldset >
			<legend >Role Details</legend>
			<table id="tbUserInfo" style="width:100%; padding: 3px" runat="server">
			       <tr>
			           <td style="width:100%">
			               <div><asp:Panel ID="Panel1" runat="server" Width="98%">
			                    <uc1:ErrorDisplay ID="ErrorDisplay2" runat="server" /></asp:Panel>
	                     </div>
			           </td>
			       </tr>
					<tr>
					<td  class="tdpad">
						Role <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="regValidation" runat="server" ErrorMessage="* Required" ControlToValidate="txtRoleName" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator>
						<asp:TextBox ID="txtRoleName" runat="server"  ValidationGroup="regValidation"></asp:TextBox>
					</td>
					</tr>
					<tr><td style="height: 5px"></td></tr>
					<tr>
						<td style="width:100%; text-align: right; vertical-align: top" colspan="2">
							<asp:Button ID="btnSubmit" runat="server" Text="Add New Role" CssClass="customButton" ValidationGroup="regValidation"  CommandArgument="1" OnClick="BtnSubmitClick" />
							 <input type="button" id="btnCancel" value="Close" class="customButton"  />
						</td>
				</tr>
			</table>
		</fieldset>
	</div>	
    <div>
        <table>
           <tr>
                <td width="100%" colspan="2">
                        <ajaxToolkit:ModalPopupExtender ID="mpeDisplayJobDetails" runat="server"  BackgroundCssClass="popupBackground"   CancelControlID="btnCancel" DropShadow="false" PopupControlID="detailDiv" TargetControlID="btnShowJobDetails" >
                    </ajaxToolkit:ModalPopupExtender>
                </td>
                </tr>
        </table>
   </div>
	<div  style="width: 100%; " class="gridDiv">
	        <table style="width: 100%; border: none;padding:0;">
			  <tr>
				  <td style="width: 90%; text-align: left; vertical-align: top">
					 <table style="width: 100%; border: none; padding: 0px" >
						  <tbody>
						  <tr>
							<td width="100%">
								<table style="width: 100%; border: none; padding: 0px">
									<tr class="divBackground">
										<td  style="width: 90%; text-align: left; vertical-align: middle" class="tdpadd" ><label class="label">Portal Roles</label></td>
										<td  style="width: 10%; text-align: center; vertical-align: middle;" class="tdpadd">
											<asp:Button ID="btnAddItem" runat="server" Text="Add Role" CssClass="customButton" Height="30px" Width="180px" onclick="BtnAddItemClick" />
										</td>
									</tr>
								</table>
							  </td>
						  </tr>
						  <tr>
							<td>
							  <table style="width: 100%; text-align: left; vertical-align: top">
								<tbody>
								<tr>
								  <td style="width: 100%" colspan="2">
									<asp:DataGrid ID="dgPortalRole" runat="server" AutoGenerateColumns="False" CellSpacing="1"  CellPadding="0" DataKeyField="ID" GridLines="None" ShowFooter="True" Width="100%"
											OnDeleteCommand="DgPortalRoleDeleteCommand" OnEditCommand="DgPortalRoleEditCommand">
											<FooterStyle CssClass="gridFooter" />
											<AlternatingItemStyle CssClass="alternatingRowStyle" />
											<ItemStyle CssClass="gridRowItem" />
											<HeaderStyle CssClass="gridHeader" />
											<Columns>
												<asp:TemplateColumn HeaderText="No.">
													<HeaderStyle HorizontalAlign="center"  Width="5%" CssClass="tdpadtop"  />
													<ItemStyle HorizontalAlign="center"  />
													<ItemTemplate>
														<asp:Label ID="lblSNo" runat="server" CssClass="lDisplay" 
															Text="<%# ((dgPortalRole.PageSize*dgPortalRole.CurrentPageIndex) + Container.ItemIndex + 1)%>">
														</asp:Label>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Role Name" >
													<HeaderStyle HorizontalAlign="left" Width="50%" CssClass="tdpadtop" />
													<ItemStyle HorizontalAlign="left" />
													<ItemTemplate>
														<asp:linkbutton ID="lblRole" runat="server" ForeColor="green"  CssClass="linkStyle" 
															Text='<%# (DataBinder.Eval(Container.DataItem,"Name")) %>' CommandName="Edit" CommandArgument='<%# (DataBinder.Eval(Container.DataItem,"ID")) %>' CausesValidation="false" >
														</asp:linkbutton>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Action">
													<HeaderStyle HorizontalAlign="center" Width="3%" CssClass="tdpadtop" />
													<ItemStyle HorizontalAlign="center" />
													<ItemTemplate>
														<table style="width: 100%; text-align: center; vertical-align: middle; padding: 0px">
															<tr>
																<td style="width: 100%; text-align: center;">                                                                    
																	<asp:ImageButton ID="imgDelete" runat="server" OnClientClick="javascript:return confirm('Are you sure you want to delete this item from the list?')" AlternateText="Delete Role Item"
																		CausesValidation="False" CommandArgument="1" CommandName="Delete" ImageUrl="~/App_Themes/Default/Images/btn_delete_new.gif" ToolTip="Delete Portal Role" style="cursor:hand" />
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
								<tr >
									<td width="100%">
									</td>
								</tr>
								</tbody>
							</table>
							</td>
						  </tr>
						
						 </tbody>
						</table>
				  </td>
			   </tr>
		  </table>
		  
	   </div>
</div>
	