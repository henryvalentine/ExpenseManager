<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="frmManageCategoriesOfExpenses.ascx.cs" Inherits="ExpenseManager.ExpenseMgt.FrmManageCategoriesOfExpenses1" %>
<%@ Register TagPrefix="uc2" TagName="ErrorDisplay_1" Src="~/CoreFramework/ErrorControl/ErrorDisplay.ascx" %>
<%@ Register src="../CoreFramework/AlertControl/ConfirmAlertBox.ascx" tagname="ConfirmAlertBox" tagprefix="uc1" %>
<link href="App_Themes/Default/ControlStyles.css" rel="stylesheet" type="text/css" />

<div class="dvContainer">
    
    <h2>
        Manage Expense Categories</h2>
     <uc1:ConfirmAlertBox ID="ConfirmAlertBox1" runat="server" />
    <div> <asp:Button ID="btnPopUp" runat="server" Style="display: none" /></div>
    <ajaxToolkit:ModalPopupExtender runat="server" ID="mpeProcessExpenseCategory" BackgroundCssClass="popupBackground"  TargetControlID="btnPopUp" PopupControlID="dvProcessExpenseCategories" CancelControlID="btnCloseProcessCategories" RepositionMode="RepositionOnWindowResizeAndScroll"/>
    <div id="dvProcessExpenseCategories" class="single-form-display" style="width:25%; border: 0 groove transparent; border-radius: 5px; display: none">
        <div style="padding-bottom: 10px; width: 98%"><uc2:ErrorDisplay_1 ID="ErrorDisplayProcessExpenseCategory" runat="server" /></div>
        <fieldset style="border-radius: 5px;">
            <legend style="" runat="server" id="lgCategoryTitle">
                Create new Expense Category</legend>
           <table style="width: 97%; padding: 3px; border: none">
                <tr>
                  <td>
                   Title<asp:RequiredFieldValidator ID="ReqTitle" runat="server" ValidationGroup="valManageExpenseCategory" ErrorMessage="*Required" ControlToValidate="txtTitle" SetFocusOnError="True" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpTitle" runat="server" ControlToValidate="txtTitle" Display="Dynamic" ErrorMessage="* Invalid Entry" Font-Size="8pt" ForeColor="Red" SetFocusOnError="True" ValidationExpression="<%$ AppSettings:NameValidationExpression4 %>" ValidationGroup="valManageExpenseCategory"></asp:RegularExpressionValidator> 
                      <asp:TextBox runat="server" Width="100%" ID="txtTitle" class="text-box"></asp:TextBox>
                 </td>
            </tr>
            <tr>
                <td>
                    <div><i>Code</i><asp:RequiredFieldValidator ID="ReqCode" ValidationGroup="valManageExpenseCategory" runat="server" ErrorMessage="*Required" ControlToValidate="txtCode" SetFocusOnError="True" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator></div>
                    <asp:TextBox runat="server" Width="100%" ID="txtCode" CssClass="text-box"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 100%" class="tdpa">
                                <asp:CheckBox ID="chkCategory" Text="Active?" CssClass="customNewCheckbox" runat="server" />
                            </td>
                           
                        </tr>
                        <tr>
                          <td style="width: 60%; text-align: right">
                                <div style="">
                                    <asp:Button runat="server" id="btnProcessCategory" CommandArgument="1"  Text="Submit" class="customButton" OnClick="BtnprocessCategoryClick"  ValidationGroup="valManageExpenseCategory" />&nbsp;
                                    <input type="button" id="btnCloseProcessCategories" value="Close" class="customButton"/>
                                </div>
                            </td>   
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
     </fieldset>
    </div>
    <div id="dvCategoriesList" style="width: 100%;" class="gridDiv">
        <table style="width: 100%">
             <tbody>
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr class="divBackground">
                                    <td style="width: 80%" class="tdpadd">
                                        <div style="width: 28%; font-size: 1em; font-weight: bolder">
                                            <label class="label">Registered Expense Categories</label> 
                                        </div>
                                    </td>
                                    <td style="width: 20%" class="tdpadd">
                                        <div style="margin-left: 20%; width: 208px">
                                            <asp:Button ID="btnAddNewCategory" runat="server" Text="Add New Expense Category" CssClass="customButton" Width="183px" OnClick="BtnAddNewCategoryClick"  />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%">
                            <asp:DataGrid runat="server" ID="dgExpCatCollections" AutoGenerateColumns="False" CellPadding="1" CellSpacing="1" GridLines="none" CssClass="xPlugTextAll_x" ShowFooter="True" DataKeyField="ExpenseCategoryId" OnEditCommand="DgExpCatCollectionsEditCommand">
                                <FooterStyle CssClass="gridFooter" />
                                <AlternatingItemStyle CssClass="alternatingRowStyle" />
                                <ItemStyle CssClass="gridRowItem" />
                                <HeaderStyle CssClass="gridHeader" />  
                                <Columns>
                                    <asp:TemplateColumn HeaderText="S/No.">
                                        <HeaderStyle HorizontalAlign="center" Width="1%" CssClass="tdpadtop" />
                                        <ItemStyle CssClass="xPlugTextAll_x" HorizontalAlign="center" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblSNo" runat="server" CssClass="xPlugTextAll_x" Text="<%# ((dgExpCatCollections.PageSize*dgExpCatCollections.CurrentPageIndex) + Container.ItemIndex + 1)%>"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Category" >
                                        <HeaderStyle HorizontalAlign="left" Width="22%" CssClass="tdpadtop"/>
                                        <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:label ID="lblCategoryTitle" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Title")) %>' ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Code" >
                                        <HeaderStyle HorizontalAlign="left" Width="16%" CssClass="tdpadtop" />
                                        <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:label ID="lblCode" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Code")) %>' ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Status" >
                                        <HeaderStyle HorizontalAlign="left" Width="5%" CssClass="tdpadtop" />
                                        <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:label ID="lblStatus" runat="server"  CssClass="xPlugTextAll_x" Text='<%# ((DataBinder.Eval(Container.DataItem,"Status")).ToString() == "1") ? "Active" : "Inactive" %>' ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Edit" >
                                        <HeaderStyle HorizontalAlign="center" Width="1%" CssClass="tdpadtop" />
                                        <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgEdit" runat="server" AlternateText="Edit" CommandArgument="1" CommandName="Edit" ImageUrl="~/App_Themes/Default/Images/btn_edit_new.gif" ToolTip="Edit" style="cursor:hand" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <%--<asp:TemplateColumn HeaderText="Delete" >
                                        <HeaderStyle HorizontalAlign="center" Width="3%" />
                                        <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgDelete" runat="server" AlternateText="Delete" OnClientClick="javascript:return confirm('Are you sure you want to delete this item from the list?')" CausesValidation="False" CommandArgument="1" CommandName="Delete" ImageUrl="~/App_Themes/Default/Images/btn_delete_new.gif" ToolTip="Delete" style="cursor:hand" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>--%>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
             </tbody>
        </table>
    </div>
</div>