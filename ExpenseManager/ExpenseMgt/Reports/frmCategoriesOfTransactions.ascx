<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="frmCategoriesOfTransactions.ascx.cs" Inherits="ExpenseManager.ExpenseMgt.Reports.FrmCategoriesOfTransactions" %>
<%@ Register TagPrefix="uc2" TagName="ErrorDisplay_1" Src="~/CoreFramework/ErrorControl/ErrorDisplay.ascx" %>
    
    <h2 style=" font-weight: normal; font-family: 'OCR A Extended', arial; border-bottom-color: #038103; margin-top: 1.3%">
        Expense Categories</h2>
     <div style="padding-bottom: 5px"><asp:Panel ID="Panel1" runat="server" Width="98%"><uc2:ErrorDisplay_1 ID="ErrorDisplay1" runat="server" /></asp:Panel></div>
    <div> <asp:Button ID="btnPopUp" runat="server" Style="display: none" /></div>
    <div id="dvCategoriesList" style="width: 99%; padding: 1px; " class="gridDiv">
        <table style="width: 100%">
             <tbody>
                    <tr>
                        <td style="width: 100%" class="divBackground" >
                            <div style="width: 28%; font-weight: bolder">
                                <legend style="color: #038103">Registered Expense Categories</legend> 
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%">
                            <asp:DataGrid runat="server" ID="dgExpCatCollections" AutoGenerateColumns="False" CellPadding="1" CellSpacing="1" GridLines="none" CssClass="xPlugTextAll_x" ShowFooter="True" DataKeyField="ExpenseCategoryId" >
                                <FooterStyle CssClass="gridFooter" />
                                <AlternatingItemStyle CssClass="alternatingRowStyle"  />
                                <ItemStyle CssClass="gridRowItem" />
                                <HeaderStyle CssClass="gridHeader" />  
                                <Columns>
                                    <asp:TemplateColumn HeaderText="S/No.">
                                        <HeaderStyle HorizontalAlign="center" Width="1%" />
                                        <ItemStyle CssClass="xPlugTextAll_x" HorizontalAlign="center" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblSNo" runat="server" CssClass="xPlugTextAll_x" Text="<%# ((dgExpCatCollections.PageSize*dgExpCatCollections.CurrentPageIndex) + Container.ItemIndex + 1)%>"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Category" >
                                        <HeaderStyle HorizontalAlign="left" Width="12%" />
                                        <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:label ID="lblCategoryTitle" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Title")) %>' ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Code" >
                                        <HeaderStyle HorizontalAlign="left" Width="10%" />
                                        <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:label ID="lblCode" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Code")) %>' ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Status" >
                                        <HeaderStyle HorizontalAlign="left" Width="5%" />
                                        <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:label ID="lblStatus" runat="server"  CssClass="xPlugTextAll_x" Text='<%# ((DataBinder.Eval(Container.DataItem,"Status")).ToString() == "1") ? "Active" : "Inactive" %>' ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
             </tbody>
        </table>
    </div>