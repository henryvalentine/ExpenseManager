<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FrmManageApprover.ascx.cs" Inherits="ExpenseManager.ExpenseMgt.FrmManageApprover" %>

  <%@ Register TagPrefix="uc2" TagName="ErrorDisplay_1" Src="~/CoreFramework/ErrorControl/ErrorDisplay.ascx" %>
<%@ Register src="../CoreFramework/AlertControl/ConfirmAlertBox.ascx" tagname="ConfirmAlertBox" tagprefix="uc1" %>
<link href="App_Themes/Default/ControlStyles.css" rel="stylesheet" type="text/css" />

<div class="dvContainer">
    
    <h2>Manage Transaction Approver Delegates</h2>
     <uc1:ConfirmAlertBox ID="ConfirmAlertBox1" runat="server" />
    <div style="width: 100%;" class="gridDiv">
        <table style="width: 100%">
             <tbody>
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr class="divBackground">
                                    <td style="width: 80%" class="tdpadd2">
                                        <div style="width: 28%; font-size: 1em; font-weight: bolder">
                                            <label class="label">Users in the Executive Officer Role</label> 
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%">
                            <asp:DataGrid ID="dgPortalUsers" runat="server" AutoGenerateColumns="False" CellPadding="1" CellSpacing="2"  DataKeyField="PortalUserId"  GridLines="None" ShowFooter="True" Width="100%"
									  OnItemCommand="DgPortalUsersItemCommand">
										<FooterStyle CssClass="gridFooter" />
										<AlternatingItemStyle CssClass="alternatingRowStyle" />
										<ItemStyle CssClass="gridRowItem" />
										<HeaderStyle CssClass="gridHeader" />
										<Columns>
											<asp:TemplateColumn HeaderText="S/No.">
												<HeaderStyle HorizontalAlign="Center" Width="4%" CssClass="tdpadtop" />
												<ItemStyle CssClass="lDisplay" HorizontalAlign="center" />
												<ItemTemplate>
													<asp:Label ID="lblSNo" runat="server" CssClass="xPlugTextAll" 
													   Text="<%# ((dgPortalUsers.PageSize*dgPortalUsers.CurrentPageIndex) + Container.ItemIndex + 1)%>">
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="First Name" >
												<HeaderStyle HorizontalAlign="left" Width="25%" CssClass="tdpadtop" />
												<ItemStyle HorizontalAlign="left" />
												<ItemTemplate>
												  <asp:label ID="lblFirstName" runat="server"  CssClass="lDisplay" 
														Text='<%#(DataBinder.Eval(Container.DataItem, "firstName")) %>' >
													</asp:label>
												</ItemTemplate>
											</asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Last Name" >
												<HeaderStyle HorizontalAlign="left" Width="25%" CssClass="tdpadtop" />
												<ItemStyle HorizontalAlign="left" />
												<ItemTemplate>
												  <asp:label ID="lblFLastName" runat="server"  CssClass="lDisplay" 
														Text='<%# (DataBinder.Eval(Container.DataItem, "lastName"))%>' >
													</asp:label>
												</ItemTemplate>
											</asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="User Name" >
												<HeaderStyle HorizontalAlign="left" Width="15%" CssClass="tdpadtop" />
												<ItemStyle HorizontalAlign="left" />
												<ItemTemplate>
													<asp:Label ID="lblUserName" runat="server" CssClass="lDisplay" Text='<%# (DataBinder.Eval(Container.DataItem, "userName")) %>' >
													</asp:Label>
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
                                       <asp:TemplateColumn HeaderText="Set As Approver" >
                                        <HeaderStyle HorizontalAlign="center" Width="10%" CssClass="tdpadtop" />
                                        <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgAuthtorize" runat="server" AlternateText="Authtorize" CommandArgument="1" CommandName="Authtorize" ImageUrl="~/App_Themes/Default/Images/btn_edit_new.gif" OnClientClick="javascript:return confirm('Are you sure you want to delegate this user to approve Transactions?')" CausesValidation="False" ToolTip="Authtorize" style="cursor:hand" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
             </tbody>
        </table>
    </div>
</div>
 
 