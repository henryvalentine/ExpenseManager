<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="frmManageCategoriesOfAssets.ascx.cs" Inherits="ExpenseManager.AssetManagement.FrmManageCategoriesOfAssets" %>
<%@ Register TagPrefix="uc2" TagName="ErrorDisplay_1_1" Src="~/CoreFramework/ErrorControl/ErrorDisplay.ascx" %>
<style type="text/css">
        .customNewCheckbox
       {
           text-align: center;
            float: right;
            margin-left: 30%;
            width: auto;
            top: 10px;
        }
        
    </style>

<div class="dvContainer">
    <h2 style="font-family: 'OCR A Extended', arial; border-bottom-color: #038103">
        Manage Asset Categories</h2>
     <div style="padding-bottom: 10px"><asp:Panel ID="Panel1" runat="server" Width="98%"><uc2:ErrorDisplay_1_1 ID="ErrorDisplay1" runat="server" /></asp:Panel></div>
    <div> <asp:Button ID="btnPopUp" runat="server" Style="display: none" /></div>
    <ajaxToolkit:ModalPopupExtender runat="server" ID="mpeProcessAssetCategory" BackgroundCssClass="popupBackground" TargetControlID="btnPopUp" PopupControlID="dvProcessAssetCategories" CancelControlID="btnCloseAssetCategories" RepositionMode="RepositionOnWindowResizeAndScroll"/>
    <div id="dvProcessAssetCategories" class="single-form-display" style="width:35%; border: 0 groove transparent; border-radius: 5px; Display: none">
        <div style="padding-bottom: 10px; width: 98%"><uc2:ErrorDisplay_1_1 ID="ErrorDisplayProcessAssetCategory" runat="server" /></div>
        <fieldset style="border-radius: 5px; border: 1px groove #038103">
            <legend style="color: #038103; font-family: 'OCR A Extended', arial;" runat="server" id="lgAssetCategoryTitle">Create a new Asset Category</legend>
           <table style="width: 97%; padding: 3px; border: none">
              <tr>
                  <td>
                    <div><i>Name</i><asp:RequiredFieldValidator ID="ReqName" runat="server" ValidationGroup="valManageAssetCategory" ErrorMessage="*Required" ControlToValidate="txtName" SetFocusOnError="True" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpName" runat="server" ControlToValidate="txtName" Display="Dynamic" ErrorMessage="* Invalid Entry" Font-Size="8pt" ForeColor="Red" SetFocusOnError="True" ValidationExpression="<%$ AppSettings:NameValidationExpression4 %>" ValidationGroup="valManageAssetCategory"></asp:RegularExpressionValidator> </div>
                      <asp:TextBox runat="server" Width="100%" ID="txtName" CssClass="text-box"></asp:TextBox>
                 </td>
             </tr>
             <tr>
                <td>
                    <div><i>Code</i><asp:RequiredFieldValidator ID="ReqCode" ValidationGroup="valManageAssetCategory" runat="server" ErrorMessage="*Required" ControlToValidate="txtCode" SetFocusOnError="True" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator></div>
                    <asp:TextBox runat="server" Width="100%" ID="txtCode" CssClass="text-box"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 40%">
                                <asp:CheckBox ID="chkAssetCategory" Text="Active?" CssClass="customNewCheckbox" runat="server" />
                            </td>
                            <td style="width: 60%">
                                <div style="width: 215px; margin-left: 7%">
                                    <asp:Button runat="server" id="btnProcessAssetCategory" CommandArgument="1"  Text="Submit" CssClass="customButton" OnClick="BtnprocessAssetCategoryClick" Width="119px" ValidationGroup="valManageAssetCategory" />&nbsp;
                                    <input type="button" id="btnCloseAssetCategories" style="width: 88px" value="Close" class="customButton"/>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
     </fieldset>
    </div>
    <div id="dvAssetCategoriesList" style="width: 100%; " class="gridDiv">
        <table style="width: 100%">
             <tbody>
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 80%" class="divBackground">
                                        <div style="width: 28%; font-size: 1em; font-weight: bolder">
                                            <legend style="color: #038103; font-family: 'OCR A Extended', arial;">Registered Asset Categories</legend> 
                                        </div>
                                    </td>
                                    <td style="width: 20%" class="divBackground">
                                        <div style="margin-left: 20%; width: 208px">
                                            <asp:Button ID="btnAddAssetNewCategory" runat="server" Text="Add New Asset Category" CssClass="customButton" Width="183px" OnClick="BtnAddNewAssetCategoryClick"  />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%">
                            <asp:DataGrid runat="server" ID="dgAssetCategories" AutoGenerateColumns="False" CellPadding="1" CellSpacing="1" GridLines="none" CssClass="xPlugTextAll_x" ShowFooter="True" DataKeyField="AssetCategoryId" OnEditCommand="DgAssetCategoriesEditCommand">
                                <FooterStyle CssClass="gridFooter" />
                                <AlternatingItemStyle CssClass="alternatingRowStyle"  />
                                <ItemStyle CssClass="gridRowItem" />
                                <HeaderStyle CssClass="gridHeader" />  
                                <Columns>
                                    <asp:TemplateColumn HeaderText="S/No.">
                                        <HeaderStyle HorizontalAlign="Left" Width="1%" />
                                        <ItemStyle CssClass="xPlugTextAll_x" HorizontalAlign="Left" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblSNo" runat="server" CssClass="xPlugTextAll_x" Text="<%# ((dgAssetCategories.PageSize*dgAssetCategories.CurrentPageIndex) + Container.ItemIndex + 1)%>"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Category" >
                                        <HeaderStyle HorizontalAlign="left" Width="12%" />
                                        <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:label ID="lblCategoryTitle" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Name")) %>' ></asp:label>
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
                                    <asp:TemplateColumn HeaderText="Edit" >
                                        <HeaderStyle HorizontalAlign="center" Width="3%" />
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