<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FrmBeneficiaryType.ascx.cs" Inherits="ExpenseManager.ExpenseMgt.FrmBeneficiaryType" %>
<%@ Register src="../CoreFramework/AlertControl/ConfirmAlertBox.ascx" tagname="ConfirmAlertBox" tagprefix="uc1" %>
<link href="App_Themes/Default/ControlStyles.css" rel="stylesheet" type="text/css" />

<div class="dvContainer">
     
    <h2>Beneficiary Types</h2>
    <div> <asp:Button ID="btnPopUp" runat="server" Style="display: none" />
        <uc1:ConfirmAlertBox ID="ConfirmAlertBox1" runat="server" />
     </div>
    <ajaxToolkit:ModalPopupExtender runat="server" ID="mpeProcessBeneficiaryType" BackgroundCssClass="popupBackground"  TargetControlID="btnPopUp" PopupControlID="dvProcessExpenseBeneficiaryTypes" CancelControlID="btnCloseProcessBeneficiaryTypes" RepositionMode="RepositionOnWindowResizeAndScroll"/>
    <div id="dvProcessExpenseBeneficiaryTypes" class="single-form-display" style="width:30%; border: 0 groove transparent; border-radius: 5px; display: none">
        <fieldset>
           <legend runat="server" id="lgBeneficiaryTypeTitle"> Create new Beneficiary Type</legend>
           <table style="width: 97%; padding: 3px; border: none">
              <tr>
                 <td>
                  Beneficiary Type<asp:RequiredFieldValidator ID="ReqTitle" runat="server" ValidationGroup="valManageBeneficiaryType" ErrorMessage="*Required" ControlToValidate="txtName" SetFocusOnError="True" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" Width="100%" ID="txtName" class="text-box"></asp:TextBox>
               </td>
            </tr>
            <tr>
                <td>
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 40%">
                                <asp:CheckBox ID="chkBeneficiaryType" Text="Active?" CssClass="customNewCheckbox" runat="server" />
                            </td>
                            
                        </tr>
                        <tr>
                         <td style="text-align: right">
                                
                                    <asp:Button runat="server" id="btnProcessBeneficiaryType" CommandArgument="1"  Text="Submit" class="customButton" OnClick="BtnProcessBeneficiaryTypeClick" Width="119px" ValidationGroup="valManageBeneficiaryType" />&nbsp;
                                    <input type="button" id="btnCloseProcessBeneficiaryTypes" style="width: 88px" value="Close" class="customButton"/>
                                
                            </td>   
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
     </fieldset>
    </div>
    <div id="dvBeneficiaryTypesList" style="width: 100%;" class="gridDiv">
        <table style="width: 100%">
             <tbody>
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr class="divBackground">
                                    <td style="width: 80%" class="tdpadd">
                                       
                                            <label class="label">Registered BeneficiaryTypes</label> 
                                       
                                    </td>
                                    <td style="width: 10%" class="tdpadd">
                                        
                                            <asp:Button ID="btnAddNewBeneficiaryType" runat="server" Text="Add New Beneficiary Type" CssClass="customButton" Width="183px" OnClick="BtnAddNewBeneficiaryTypeClick"  />
                                        
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%">
                            <asp:DataGrid runat="server" ID="dgBeneficiaryTypes" AutoGenerateColumns="False" CellPadding="1" CellSpacing="1" GridLines="none" CssClass="xPlugTextAll_x" ShowFooter="True" DataKeyField="BeneficiaryTypeId" OnEditCommand="DgBeneficiaryTypesEditCommand">
                                <FooterStyle CssClass="gridFooter" />
                                <AlternatingItemStyle CssClass="alternatingRowStyle"  />
                                <ItemStyle CssClass="gridRowItem" />
                                <HeaderStyle CssClass="gridHeader" />  
                                <Columns>
                                    <asp:TemplateColumn HeaderText="S/No.">
                                        <HeaderStyle HorizontalAlign="center" Width="1%" CssClass="tdpadtop" />
                                        <ItemStyle CssClass="xPlugTextAll_x" HorizontalAlign="center" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblSNo" runat="server" CssClass="xPlugTextAll_x" Text="<%# ((dgBeneficiaryTypes.PageSize*dgBeneficiaryTypes.CurrentPageIndex) + Container.ItemIndex + 1)%>"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Beneficiary Type" >
                                        <HeaderStyle HorizontalAlign="left" Width="22%" CssClass="tdpadtop"  />
                                        <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:label ID="lblBeneficiaryType" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Name")) %>' ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Status" >
                                        <HeaderStyle HorizontalAlign="left" Width="5%" CssClass="tdpadtop"  />
                                        <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:label ID="lblStatus" runat="server"  CssClass="xPlugTextAll_x" Text='<%# ((DataBinder.Eval(Container.DataItem,"Status")).ToString() == "1") ? "Active" : "Inactive" %>' ></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Edit" >
                                        <HeaderStyle HorizontalAlign="center" Width="1%" CssClass="tdpadtop"  />
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