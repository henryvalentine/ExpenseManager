<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserProfile.ascx.cs" Inherits="ExpenseManager.CoreFramework.UserProfile" %>
<%@ Register TagPrefix="uc1" TagName="ErrorDisplay" Src="~/CoreFramework/ErrorControl/ErrorDisplay.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="kPortal.Common.EnumControl" Assembly="kPortal.Common" %>
<link href="~/App_Themes/Default/ControlStyles.css" rel="stylesheet" type="text/css" />

<div class="dvContainer">
 <h2> Manage Portal Users Profiles</h2>
    <div style="width: 98%" class="aligncenter" ><uc1:ErrorDisplay ID="ErrorDisplay1" runat="server" /></div>
     <input type="button" id="Button1" runat="server" style="display: none"/>
	  <div style="width:50%; display: none; border-radius: 5px; border: 0 groove transparent;" class="single-form-display" id="detailDiv">
         <fieldset>
			<legend>Portal User Profile</legend>
			   <table id="tbUserInfo" style="width:100%; padding: 4px" runat="server">
			        <tr>
			           <td style="width:100%" colspan="2">
			               <div><asp:Panel ID="Panel1" runat="server" Width="100%"><uc1:ErrorDisplay ID="ErrorDisplay2" runat="server" /></asp:Panel>
	                     </div>
			           </td>
			       </tr>
					<tr>
						<td style="width:50%" class="tdpad">
							First Name <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFirstName" ErrorMessage="" ValidationGroup="regValidation" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass">* Required</asp:RequiredFieldValidator>
							<asp:TextBox ID="txtFirstName" runat="server" CssClass="text-box" ReadOnly="False" ValidationGroup="regValidation" ></asp:TextBox>
						</td>
                        <td style="width:50%" class="tdpad">
							Last Name <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLastName" ErrorMessage="" ValidationGroup="regValidation" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass">* Required</asp:RequiredFieldValidator>
							<asp:TextBox ID="txtLastName" runat="server" CssClass="text-box" ReadOnly="False" ValidationGroup="regValidation"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td style="width:50%" class="tdpad">
							 Sex 
							 <cc1:DropDownListEnum ID="ddlSex" runat="server" Width="100%" CssClass="ddl-box" EnumType="kPortal.Common.EnumControl.Enums.Sex" SetDefaultSelectValue="true" FixNames="true" UseXmlEnumNames="false" />
						</td>
                        <td style="width:50%" class="tdpad">
							Mobile Number: <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtMobileNumber" ErrorMessage="" ValidationGroup="regValidation" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass">* Required</asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"  ControlToValidate="txtMobileNumber" ErrorMessage=""  ValidationExpression="<% $ AppSettings:GSMValidationExpression %>"  ValidationGroup="regValidation" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass">* Invalid Mobile Number</asp:RegularExpressionValidator>
							<asp:TextBox ID="txtMobileNumber" MaxLength="11" runat="server" CssClass="text-box" ReadOnly="false" ></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td style="width:50%" class="tdpad">
							Email <asp:RequiredFieldValidator ID="req1" runat="server" ControlToValidate="txtEmail" ErrorMessage="" ValidationGroup="regValidation" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass">* Required</asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegExpr1" runat="server"  ControlToValidate="txtEmail" ErrorMessage=""  ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"  ValidationGroup="regValidation" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass">* Invalid Email</asp:RegularExpressionValidator>
							<asp:TextBox ID="txtEmail" runat="server" CssClass="text-box" ReadOnly="false" ></asp:TextBox>
						</td>
                        <td style="width:50%" class="tdpad">
							Designation
							<asp:TextBox ID="txtDesignation" runat="server" CssClass="text-box" ReadOnly="False"></asp:TextBox>
						</td>
					</tr>
					 <tr>
						 <td colspan="2" style="width:100%; padding-bottom: 2px; margin-bottom: 5px; color: #666; font-weight: bold; border-bottom: solid 1px #ccc; ">Login Information</td>
					</tr>
					
					<tr>
						<td style="width:50%;" class="tdpad">
							User Name <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtUserName" ErrorMessage="" ValidationGroup="regValidation" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass">* Required</asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"  ControlToValidate="txtUserName" ErrorMessage=""  ValidationExpression="^[a-zA-Z1-9'.]{6,20}$"  ValidationGroup="regValidation">* Invalid User Name</asp:RegularExpressionValidator>
							<asp:TextBox ID="txtUserName" runat="server" Width="98%" ReadOnly="false" ></asp:TextBox>
						</td>
                        <td style="width:50%;" class="tdpad">
							
								<asp:CheckBox ID="chkActive" runat="server" Text="Active?" TextAlign="Left" />
							
						</td>
					</tr>
					<tr>
						<td style="width:50%" class="tdpad">
							Password <asp:RequiredFieldValidator ID="reqPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="" ValidationGroup="regValidation" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass">* Required</asp:RequiredFieldValidator>
							<asp:TextBox ID="txtPassword" runat="server" CssClass="text-box" ReadOnly="False" TextMode="Password" ValidationGroup="regValidation"></asp:TextBox>
						</td>
                        <td style="width:50%" class="tdpad">
							Confirm Password<asp:RequiredFieldValidator ID="reqConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword" ErrorMessage="" ValidationGroup="regValidation">* Required</asp:RequiredFieldValidator><asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword" Display="Dynamic" ErrorMessage="" ValidationGroup="regValidation" CssClass="lDisplay" SetFocusOnError="True">* Password and Confirmation Password must match</asp:CompareValidator>
							<asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="text-box" ReadOnly="False" TextMode="Password" ValidationGroup="regValidation"></asp:TextBox>
						</td>
					</tr>
                     <tr>
						 <td colspan="2" style="width:100%; padding-bottom: 2px; margin-bottom: 5px; border-bottom: solid 1px #999; text-shadow: 0 0 2px #ccc;">Identified User Role(s)</td>
					</tr>
                    <tr>
						<td style="width:100%; padding-top: 8px" colspan="2">
						   <div style="width: 100%; background-color: #f1f1f1; height: auto">
								<asp:CheckBoxList  ID="chkRoles" CssClass="checkBoxListWrap" runat="server"  Width="100%" TextAlign="Right" CellPadding="-1" CellSpacing="-1" RepeatLayout="Table"  RepeatDirection="Horizontal" RepeatColumns="3"></asp:CheckBoxList>
							</div>
						</td>
                    </tr>
					<tr><td style="height: 5px"></td></tr>
					<tr>
						<td style="width:100%; text-align: right; vertical-align: top" colspan="2">
							<asp:Button ID="btnSubmit" runat="server" Text="Create User" CssClass="customButton" Width="130px"  ValidationGroup="regValidation"  CommandArgument="1" OnClick="BtnSubmitClick" />
					            &nbsp;<input id="btnCancel" type="button" value="Close" class="customButton"/>
                    </td>
				</tr>
			</table>
		</fieldset>
	</div>
    <ajaxToolkit:ModalPopupExtender ID="mpeDisplayJobDetails" runat="server"  BackgroundCssClass="popupBackground"   CancelControlID="btnCancel" DropShadow="false" PopupControlID="detailDiv" TargetControlID="btnShowJobDetails" >
    </ajaxToolkit:ModalPopupExtender>
    <asp:Button ID="btnShowJobDetails" runat="server" Style="display: none" Text="Show PopUp" />
    <div runat="server" id="listDV" style="width: 100%;" class="gridDiv">
        <table style="width: 100%; border: none; padding: 0px">
		    <tr>
				  <td style="width: 100%; text-align: left; vertical-align: top">
					 <table style="width: 100%; border: none; padding: 0px" >
						<tbody>
						  <tr>
							<td style="width: 100%">
								<table style="width: 100%; border: none; padding: 0px">
									<tr class="divBackground" >
										<td class="tdpadd" style="width: 90%"><label class="label">Registered Users</label></td>
                                        <td class="tdpadd">
									    <asp:Button ID="btnAddItem" runat="server" Text="Add New User" Width="130px" CssClass="customButton" onclick="BtnAddItemClick" />
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
								  <td style="border: 0px solid #d42e12; width: 100%">
									 <asp:DataGrid ID="dgPortalUsers" runat="server" AutoGenerateColumns="False" CellPadding="1" CellSpacing="2"  DataKeyField="portalUserId"  GridLines="None" ShowFooter="True" Width="100%"
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
												<HeaderStyle HorizontalAlign="left" Width="15%" CssClass="tdpadtop" />
												<ItemStyle HorizontalAlign="left" />
												<ItemTemplate>
													<asp:linkbutton ID="lblUserName" runat="server" CssClass="lDisplay"
														Text='<%# (DataBinder.Eval(Container.DataItem, "userName")) %>' CommandName="Edit" CommandArgument='<%# (DataBinder.Eval(Container.DataItem,"userName")) %>' CausesValidation="false" >
													</asp:linkbutton>
												</ItemTemplate>
											</asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Full Name" >
												<HeaderStyle HorizontalAlign="left" Width="25%" CssClass="tdpadtop" />
												<ItemStyle HorizontalAlign="left" />
												<ItemTemplate>
												  <asp:label ID="lblFullName" runat="server"  CssClass="lDisplay" 
														Text='<%# (DataBinder.Eval(Container.DataItem, "lastName")) + " " + (DataBinder.Eval(Container.DataItem, "firstName")) %>' >
													</asp:label>
												</ItemTemplate>
											</asp:TemplateColumn>
                                             <asp:TemplateColumn HeaderText="Status" >
												<HeaderStyle HorizontalAlign="center" Width="8%"  CssClass="tdpadtop"/>
												<ItemStyle HorizontalAlign="center" />
												<ItemTemplate>
												  <asp:label ID="lblApproved" runat="server"  CssClass="lDisplay" 
														Text='<%# bool.Parse((DataBinder.Eval(Container.DataItem, "status")).ToString())? "Active" : "In-Active" %>' >
													</asp:label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Delete">
												<HeaderStyle HorizontalAlign="center" Width="5%" CssClass="tdpadtop" />
												<ItemStyle HorizontalAlign="center" />
												<ItemTemplate>
													<table align="center" cellpadding="0" cellspacing="0" width="100%">
														<tr>
														   <td width="100%" align="center">
																<asp:ImageButton ID="imgDelete" runat="server" AlternateText="Delete Portal User" OnClientClick="javascript:return confirm('Are you sure you want to delete this item from the list?')"
																	CausesValidation="False" CommandArgument="1" CommandName="Delete" ImageUrl="~/App_Themes/Default/Images/btn_delete_new.gif"
																	ToolTip="Delete Portal User" style="cursor:hand" />
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
