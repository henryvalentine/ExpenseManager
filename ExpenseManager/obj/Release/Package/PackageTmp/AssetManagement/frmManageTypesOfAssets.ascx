<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="frmManageTypesOfAssets.ascx.cs" Inherits="ExpenseManager.AssetManagement.FrmManageTypesOfAssets" %>
<%@ Register TagPrefix="uc2" TagName="ErrorDisplay_1_1" Src="~/CoreFramework/ErrorControl/ErrorDisplay.ascx" %>

            
    
    <div class="dvContainer">
        <h2>Manage Asset Types</h2>
     <div style="padding-bottom: 10px"><asp:Panel ID="Panel1" runat="server" Width="98%"><uc2:ErrorDisplay_1_1 ID="ErrorDisplay1" runat="server" /></asp:Panel></div>
    <div> <asp:Button ID="btnPopUp" runat="server" Style="display: none" /></div>
    <ajaxToolkit:ModalPopupExtender runat="server" ID="mpeProcessAssetType" BackgroundCssClass="popupBackground" TargetControlID="btnPopUp" PopupControlID="dvProcessAssetType" CancelControlID="btnCloseProcessAssetType" RepositionMode="RepositionOnWindowResizeAndScroll"/>
    <div id="dvProcessAssetType" class="single-form-display" style="width:35%; display: none; border: 0 groove transparent; border-radius: 5px; ">
        <asp:Panel ID="Panel2" runat="server" Width="98%"><uc2:ErrorDisplay_1_1 ID="ErrorDisplayProcessAssetType" runat="server" /></asp:Panel>
        <fieldset>
            <legend runat="server" id="lgAssetTypeTitle">Create a new Asset Type</legend>
           <table style="width: 97%; padding: 3px; border: none">
            <tr>
                <td class="tdpad">
                     <asp:RequiredFieldValidator ID="ReqAssetCategory" ValidationGroup="valManageAssetType" runat="server" ErrorMessage="*Required" ControlToValidate="ddlAssetCategory" SetFocusOnError="True" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator> <asp:CompareValidator ID="CompareValAssetCategory" runat="server" ErrorMessage="* Invalid Selection" ValidationGroup="valManageAssetType" ValueToCompare="1" ControlToValidate="ddlAssetCategory" Operator="GreaterThanEqual" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:CompareValidator>
                    <asp:DropDownList runat="server" ID="ddlAssetCategory"/>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    
                </td>
            </tr>
            <tr>
                <td class="tdpad">
                    Asset Type<asp:RequiredFieldValidator ID="ReqAssetType" runat="server" ValidationGroup="valManageAssetType" ErrorMessage="*Required" ControlToValidate="txtAssetType" SetFocusOnError="True" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" Width="100%" ID="txtAssetType" CssClass="text-box"></asp:TextBox>
                </td>
            </tr>
             <tr>
                <td colspan="2">
                    
                </td>
            </tr>
            <tr>
                <td>
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 40%" class="tdpad">
                                <asp:CheckBox ID="chkAssetType" Text="Active?" CssClass="customNewCheckbox" runat="server" />
                            </td>
                           
                        </tr>
                        <tr>
                          <td style="width: 60%;text-align: right">
                               
                                    <asp:Button runat="server" ID="btnSubmitAssetType" CommandArgument="1" Text="Submit" CssClass="customButton" OnClick="BtnSubmitAssetTypeClick" ValidationGroup="valManageAssetType" />&nbsp;
                                    <input type="button" id="btnCloseProcessAssetType" style="" value="Close" class="customButton"/>
                                
                            </td>   
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
     </fieldset>
    </div>
    <div id="dvAssetTypesList" style="width: 100%">
    <table style="width: 100%" class="divBackground2">
         <tr>
            <td class="tdpad"style="width: 13%"><label class="label2">Retrieve Asset Types by Asset Category: </label></td>
            <td class="tdpad" style="width: 15%;text-align: left"><asp:DropDownList runat="server" ID="ddlAssetCategories" Width="60%" AutoPostBack="True" OnSelectedIndexChanged="DdlAssetCategoriesSelectedChanged"/></td>
            <td class="tdpad" style="width: 20%"></td>
        </tr>
    </table> 
    <div  style="width: 100%; " class="gridDiv">
       <table style="width: 100%">
         <tr class="divBackground">
            <td style="width: 80%" class="tdpadd">
                <div style="width: 100%; font-size: 1em; font-weight: bolder">
                    <label class="label">Registered Asset Types</label>
                </div>
             </td>
             <td style="width: 20%" class="tdpadd">
                <div style="margin-left: 20%; width: 208px">
                    <asp:Button ID="btnAddNewAssetType" runat="server" Text="Add New Asset Type" CssClass="customButton" Width="183px" onclick="BtnAddNewAssetTypeClick"  />
                </div>
              </td>
            </tr>
            <tr>
                <td style="width: 100%" colspan="2">
                    <asp:DataGrid runat="server" ID="dgAssetTypes" AutoGenerateColumns="False" CellPadding="1" CellSpacing="1" 
                    GridLines="none" CssClass="xPlugTextAll_x" ShowFooter="True" Width="100%" DataKeyField="AssetTypeId" OnEditCommand="DgAssetTypesEditCommand">
                        <FooterStyle CssClass="gridFooter" />
                        <AlternatingItemStyle CssClass="alternatingRowStyle"  />
                        <ItemStyle CssClass="gridRowItem" />
                        <HeaderStyle CssClass="gridHeader" />  
                        <Columns>
                            <asp:TemplateColumn HeaderText="S/No.">
                                <HeaderStyle HorizontalAlign="center" Width="1%" CssClass="tdpadtop" />
                                <ItemStyle CssClass="xPlugTextAll_x" HorizontalAlign="center" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblSNo" runat="server" CssClass="xPlugTextAll_x" Text="<%# ((dgAssetTypes.PageSize*dgAssetTypes.CurrentPageIndex) + Container.ItemIndex + 1)%>"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Asset Type" >
                                <HeaderStyle HorizontalAlign="left" Width="12%" CssClass="tdpadtop" />
                                <ItemStyle HorizontalAlign="left" VerticalAlign="Top"  />
                                <ItemTemplate>
                                    <asp:label ID="lbldgAssetTypeName" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Name")) %>' ></asp:label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Asset Category" >
                                <HeaderStyle HorizontalAlign="left" Width="6%" CssClass="tdpadtop" />
                                <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:label ID="lblAssetCategory" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"AssetCategory.Name")) %>' ></asp:label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Code" >
                                <HeaderStyle HorizontalAlign="left" Width="4%" CssClass="tdpadtop" />
                                <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:label ID="lblCode" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Code")) %>' ></asp:label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Status" >
                                <HeaderStyle HorizontalAlign="left" Width="2%" CssClass="tdpadtop" />
                                <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:label ID="lblStatus" runat="server"  CssClass="xPlugTextAll_x" Text='<%# ((DataBinder.Eval(Container.DataItem,"Status")).ToString() == "1") ? "Active" : "Inactive" %>' ></asp:label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Edit" >
                                <HeaderStyle HorizontalAlign="center" Width="2%" CssClass="tdpadtop" />
                                <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgEdit" runat="server" AlternateText="Edit" CommandArgument="1" CommandName="Edit" ImageUrl="~/App_Themes/Default/Images/btn_edit_new.gif" ToolTip="Edit" style="cursor:hand" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
        </table>
     </div>
  </div>
</div>