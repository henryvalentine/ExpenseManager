<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SiteMapMgt.ascx.cs" Inherits="ExpenseManager.CoreFramework.PortalAdmin.SiteMapMgt" %>
<%@ Register TagPrefix="uc1" TagName="ErrorDisplay" Src="~/CoreFramework/ErrorControl/ErrorDisplay.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="kPortal.Common.EnumControl" Assembly="kPortal.Common" %>
<link href="../../App_Themes/Default/cbtControlStyles.css" rel="stylesheet" type="text/css" />

<div class="dvContainer">
    <ajaxToolkit:ModalPopupExtender ID="mpeDisplayJobDetails" runat="server"  BackgroundCssClass="popupBackground"   CancelControlID="btnCancel" DropShadow="false" PopupControlID="detailDiv" TargetControlID="btnShowJobDetails" ></ajaxToolkit:ModalPopupExtender>
      <asp:Button ID="btnShowJobDetails" runat="server" Style="display: none" Text="Show PopUp" />
    <h2>Manage Portal Tabs</h2>
     <div style="padding-bottom: 10px"><asp:Panel ID="Panel2" runat="server" Width="98%">
			<uc1:ErrorDisplay ID="ErrorDisplay1" runat="server" /></asp:Panel>
	 </div>		
	<div class="single-form-display" style="width:50%;  border: 0 groove transparent; border-radius: 5px; display: none" id="detailDiv">
		<fieldset>
			<legend>Portal Tab Settings</legend>
			<table id="tbUserInfo" style="width:100%; padding: 3px" runat="server">
			    <tr>
			        <td style="width:100%" colspan="2">
			            <div><asp:Panel ID="Panel1" runat="server" Width="98%">
			                <uc1:ErrorDisplay ID="ErrorDisplay2" runat="server" /></asp:Panel>
	                    </div>
			        </td>
			       </tr>
					<tr>
						<td style="width:50%" class="tdpad">
                           Tab Parent <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="regValidation" runat="server" ErrorMessage="* Required" ControlToValidate="ddlTabParent" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator><asp:CompareValidator ID="CompareValidator3" runat="server" ErrorMessage="" ValidationGroup="regValidation" ControlToValidate="ddlTabParent" CssClass="errorClass" ValueToCompare="1" Operator="GreaterThanEqual">* Invalid Selection</asp:CompareValidator>
							 <asp:DropDownList ID="ddlTabParent" runat="server" CssClass="ddl-box" ValidationGroup="regValidation" OnSelectedIndexChanged="DdlTabParentSelectedIndexChanged" AutoPostBack="True">
							</asp:DropDownList>
						
						</td>
                        <td style="width:50%" class="tdpad">
						   Tab Name <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="regValidation" runat="server" ErrorMessage="* Required" ControlToValidate="txtTabName" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator>
						    <asp:TextBox ID="txtTabName" runat="server" CssClass="text-box" ValidationGroup="regValidation"></asp:TextBox>   
					    </td>
					</tr>
				    <tr>
						<td style="width:50%" class="tdpad">
							Tab Type <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="regValidation" runat="server" ErrorMessage="* Required" ControlToValidate="ddlTabType" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator><asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="" ValidationGroup="regValidation" ControlToValidate="ddlTabType" CssClass="errorClass" ValueToCompare="1" Operator="GreaterThanEqual">* Invalid Selection</asp:CompareValidator>
							<cc1:DropDownListEnum runat="server" FixNames="true" EnumType="kPortal.Common.EnumControl.Enum.TabType" SetDefaultSelectValue="true" UseXmlEnumNames="false" ID="ddlTabType" CssClass="ddl-box" ValidationGroup="regValidation">
							</cc1:DropDownListEnum>
						
						</td>
                        <td style="width:50%" class="tdpad">
							Tab Order <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="regValidation" runat="server" ErrorMessage="* Required" ControlToValidate="ddlTabOrder" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator><asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="" ValidationGroup="regValidation" ControlToValidate="ddlTabOrder" CssClass="errorClass" ValueToCompare="1" Operator="GreaterThanEqual">* Invalid Selection</asp:CompareValidator>
							 <asp:DropDownList ID="ddlTabOrder" runat="server" CssClass="ddl-box" Width="100%" ValidationGroup="regValidation">
							</asp:DropDownList>
						
						</td>
					</tr>
					<tr>
						<td style="width:100%" colspan="2" class="tdpad">
							Description
							<asp:TextBox ID="txtDescription" runat="server" ReadOnly="false" Height="51px" Width="98%" TextMode="MultiLine"></asp:TextBox>
						</td>
					</tr>
					 <tr>
						<td style="width:100%" colspan="2" class="tdpad">
							URL
							<asp:TextBox ID="txtLink" runat="server"  ReadOnly="false" Height="51px" Width="98%" TextMode="MultiLine"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td style="width:100%" colspan="2" class="tdpad">
							Roles
							<div style="width: 100%; background-color: #f1f1f1; height: auto">
								<asp:CheckBoxList  ID="chkRoles" CssClass="checkBoxListWrap" runat="server"  Width="100%" TextAlign="Right" CellPadding="-1" CellSpacing="-1" RepeatLayout="Table"  RepeatDirection="Horizontal" RepeatColumns="3"></asp:CheckBoxList>
							</div>
						</td>
					</tr>
					<tr><td style="height: 5px" colspan="2"></td></tr>
					<tr>
						<td style="width:100%; text-align: right; vertical-align: top" colspan="2">
							<asp:Button ID="btnSubmit" runat="server" Text="Add New Tab" Width="140px" CssClass="customButton" ValidationGroup="regValidation"  CommandArgument="1" OnClick="BtnSubmitClick" />
							 &nbsp;<asp:Button id="btnCancel" Text="Close" class="customButton" style="width: 100px;"   runat="server"></asp:Button>
						</td>
				</tr>
			</table>
		</fieldset>
	</div>
	<div style="width: 100%;" class="gridDiv">
	       <table style="width: 100%">
                <tr>
                    <td style="width: 100%">
                        <table style="width: 100%">
                           <tr class="divBackground">
                            <td style="width: 80%" class="tdpadd">
                                
                                    <label class="label">Site Map Tab List</label>
					           
                            </td>
                            <td style="width: 20%" class="tdpadd">
                                <div style="margin-left: 37%; width: 208px">
						          <asp:Button ID="btnAddItem" runat="server" Text="Add New Tab" CssClass="customButton" Width="140px" Height="30px" onclick="BtnAddItemClick" />
					           </div>
                           </td>
				       </tr>
                    </table>
                    </td>
                </tr>
                <tr>
                   <td colspan="2" style="width: 100%"></td>
                </tr>
				<tr>
					<td style="width: 100%" colspan="2">
						<asp:DataGrid ID="dgPortalSiteMap" runat="server" AutoGenerateColumns="False" CellPadding="2" CellSpacing="1"  DataKeyField="ID" GridLines="None" BorderColor="Aqua" ShowFooter="True" Width="100%"
						OnDeleteCommand="DgPortalSiteMapDeleteCommand" OnEditCommand="DgPortalSiteMapEditCommand">
						<FooterStyle CssClass="gridFooter" />
						<AlternatingItemStyle CssClass="alternatingRowStyle" />
						<ItemStyle CssClass="gridRowItem" />
						<HeaderStyle CssClass="gridHeader" />
						<Columns>
							<asp:TemplateColumn HeaderText="No.">
								<HeaderStyle HorizontalAlign="Left" Width="4%" CssClass="tdpadtop"/>
								<ItemStyle CssClass="lDisplay" HorizontalAlign="center" />
								<ItemTemplate>
									<asp:Label ID="lblSNo" runat="server" CssClass="lDisplay" 
										Text="<%# ((dgPortalSiteMap.PageSize*dgPortalSiteMap.CurrentPageIndex) + Container.ItemIndex + 1)%>"></asp:Label>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Tab" >
								<HeaderStyle HorizontalAlign="left" Width="20%" CssClass="tdpadtop" />
								<ItemStyle HorizontalAlign="left" />
								<ItemTemplate>
									<asp:linkbutton ID="lblTitle" runat="server" CssClass="linkStyle" ForeColor="green" 
										Text='<%# (DataBinder.Eval(Container.DataItem,"Title")) %>' CommandName="Edit" CommandArgument='<%# (DataBinder.Eval(Container.DataItem,"ID")) %>' CausesValidation="false" ></asp:linkbutton>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Description" >
								<HeaderStyle HorizontalAlign="left" Width="20%" CssClass="tdpadtop" />
								<ItemStyle HorizontalAlign="left" />
								<ItemTemplate>
									<asp:label ID="lblDescription" runat="server"  CssClass="lDisplay" 
										ToolTip='<%# (DataBinder.Eval(Container.DataItem,"url")) %>'  Text='<%# (DataBinder.Eval(Container.DataItem,"Description")) %>' ></asp:label>
								</ItemTemplate>
							</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Link"  Visible="false">
								<HeaderStyle HorizontalAlign="left" Width="5%" />
								<ItemStyle HorizontalAlign="left" />
								<ItemTemplate>
									<asp:label ID="lblLink" runat="server"  CssClass="lDisplay" 
										Text='<%# (DataBinder.Eval(Container.DataItem,"url")) %>' ></asp:label>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Parent ID" >
								<HeaderStyle HorizontalAlign="Center" Width="5%" />
								<ItemStyle HorizontalAlign="Center" />
								<ItemTemplate>
									<asp:label ID="lblParent" runat="server"  CssClass="lDisplay" 
										Text='<%# (DataBinder.Eval(Container.DataItem,"Parent")) %>' ></asp:label>
								</ItemTemplate>
							</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Order" >
								<HeaderStyle HorizontalAlign="Center" Width="5%" />
								<ItemStyle HorizontalAlign="Center" />
								<ItemTemplate>
									<asp:label ID="lblTabOrder" runat="server"  CssClass="lDisplay" 
										Text='<%# (DataBinder.Eval(Container.DataItem,"TabOrder")) %>' ></asp:label>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="TabType" Visible="false" >
								<HeaderStyle HorizontalAlign="left" Width="0%" />
								<ItemStyle HorizontalAlign="left" />
								<ItemTemplate>
									<asp:label ID="lblTabType" runat="server"  CssClass="lDisplay"
										Text='<%# (DataBinder.Eval(Container.DataItem,"TabType")) %>' ></asp:label>
								</ItemTemplate>
							</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Roles" >
								<HeaderStyle HorizontalAlign="left" Width="40%" />
								<ItemStyle HorizontalAlign="left" />
								<ItemTemplate>
									<asp:label ID="lblRoles" runat="server"  CssClass="lDisplay" 
										Text='<%# (DataBinder.Eval(Container.DataItem,"Roles")) %>' ></asp:label>
								</ItemTemplate>
							</asp:TemplateColumn>
											  
							<asp:TemplateColumn HeaderText="Action">
								<HeaderStyle HorizontalAlign="center" Width="3%" />
								<ItemStyle HorizontalAlign="center" />
								<ItemTemplate>
									<table style="width: 100%; text-align: center; vertical-align: top; padding: 0px">
										<tr>
											<td style="width: 100%; text-align: center;">
												<asp:ImageButton ID="imgDelete" runat="server" OnClientClick="javascript:return confirm('Are you sure you want to delete this item from the list?')" AlternateText="Delete SiteMap Item"
													CausesValidation="False" CommandArgument="1" CommandName="Delete" ImageUrl="~/App_Themes/Default/Images/btn_delete_new.gif" ToolTip="Delete SiteMap Item" style="cursor: pointer" />
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
	