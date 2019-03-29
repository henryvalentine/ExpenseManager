<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResetLockedUser.ascx.cs" Inherits="ExpenseManager.CoreFramework.ResetLockedUser" %>
<%@ Register TagPrefix="uc1" TagName="ErrorDisplay" Src="~/CoreFramework/ErrorControl/ErrorDisplay.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="kPortal.Common.EnumControl" Assembly="kPortal.Common" %>
<link href="~/App_Themes/Default/ControlStyles.css" rel="stylesheet" type="text/css" />

<div class="dvContainer">
 <h2> Manage Locked Users</h2>
    <div style="width: 98%" class="aligncenter" ><uc1:ErrorDisplay ID="ErrorDisplay1" runat="server" /></div>
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
										<td class="tdpadd2" style="width: 90%"><label class="label">Locked Users</label></td>
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
									 <asp:DataGrid ID="dgPortalUsers" runat="server" AutoGenerateColumns="False" CellPadding="1" CellSpacing="2"  DataKeyField="portalUserId"  GridLines="None" ShowFooter="True" Width="100%" OnDeleteCommand="DgPortalUsersDeleteCommand" >
										<FooterStyle CssClass="gridFooter" />
										<AlternatingItemStyle CssClass="alternatingRowStyle" />
										<ItemStyle CssClass="gridRowItem" />
										<HeaderStyle CssClass="gridHeader" />
										<Columns>
											<asp:TemplateColumn HeaderText="No.">
												<HeaderStyle HorizontalAlign="Left" Width="5%" CssClass="tdpadtop" />
												<ItemStyle CssClass="lDisplay" HorizontalAlign="center" />
												<ItemTemplate>
													<asp:Label ID="lblSNo" runat="server" CssClass="xPlugTextAll" Text="<%# ((dgPortalUsers.PageSize*dgPortalUsers.CurrentPageIndex) + Container.ItemIndex + 1)%>"></asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="User Name" >
												<HeaderStyle HorizontalAlign="left" Width="15%" CssClass="tdpadtop" />
												<ItemStyle HorizontalAlign="left" />
												<ItemTemplate>
                                                    <asp:label ID="lblUserName" runat="server"  CssClass="lDisplay" Text='<%# (DataBinder.Eval(Container.DataItem, "userName")) %>' ></asp:label>
                                                    <asp:HiddenField ID="hndUId" Value='<%# (DataBinder.Eval(Container.DataItem, "userId")) %>' runat="server"/>
												</ItemTemplate>
											</asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Full Name" >
												<HeaderStyle HorizontalAlign="left" Width="25%" CssClass="tdpadtop" />
												<ItemStyle HorizontalAlign="left" />
												<ItemTemplate>
												  <asp:label ID="lblFullName" runat="server"  CssClass="lDisplay" Text='<%# (DataBinder.Eval(Container.DataItem, "lastName")) + " " + (DataBinder.Eval(Container.DataItem, "firstName")) %>' ></asp:label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Actions">
												<HeaderStyle HorizontalAlign="center" Width="5%" CssClass="tdpadtop" />
												<ItemStyle HorizontalAlign="center" />
												<ItemTemplate>
													<table align="center" cellpadding="0" cellspacing="0" width="100%">
														<tr>
														   <td width="100%" align="center">
																<asp:ImageButton ID="imgDelete" runat="server" AlternateText="Reset Portal User" OnClientClick="javascript:return confirm('Are you sure you want to Reset the User?')"
																	CausesValidation="False" CommandArgument="1" CommandName="Delete" ImageUrl="~/App_Themes/Default/Images/reset_icon.gif" ToolTip="Reset Portal User" style="cursor:hand" />
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
