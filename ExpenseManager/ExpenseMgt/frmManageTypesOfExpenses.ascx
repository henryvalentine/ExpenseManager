<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="frmManageTypesOfExpenses.ascx.cs" Inherits="ExpenseManager.ExpenseMgt.FrmManageTypesOfExpenses1" %>
<%@ Register TagPrefix="uc2" TagName="ErrorDisplay_1" Src="~/CoreFramework/ErrorControl/ErrorDisplay.ascx" %>
<%@ Register src="../CoreFramework/AlertControl/ConfirmAlertBox.ascx" tagname="ConfirmAlertBox" tagprefix="uc1" %>
<link href="~/App_Themes/Default/ControlStyles.css" rel="stylesheet" type="text/css" />

 
<div class="dvContainer">
    <div> <asp:Button ID="btnPopUp" runat="server" Style="display: none" /></div>
    <h2>Manage Expense Types</h2>
    <uc1:ConfirmAlertBox ID="ConfirmAlertBox1" runat="server" />
   
    <ajaxToolkit:ModalPopupExtender ID="mpeProcessTypesOfExpensesPopup" BackgroundCssClass="popupBackground"  TargetControlID="btnPopUp" CancelControlID="btnReset" PopupControlID="dvProcessTransactionTypes" RepositionMode="RepositionOnWindowResizeAndScroll" runat="server"></ajaxToolkit:ModalPopupExtender>
     <div runat="server" id="divReport" style="width: 100%;" class="gridDiv">
        <table border="0" cellspacing="0" cellpadding="2" width="100%" runat="server" id="tbExpenseType">
            <tbody>
               <tr>
                  <td width="100%" colspan="2">
				     <table style="border-style: none; border-color: inherit; border-width: medium; width: 100%; padding: 0px; height: 38px;">
					    <tr class="divBackground">
						   <td style="width: 85%" class="tdpadd">
						     
						        &nbsp;<label class="label">Registered Expense Types</label>
                           
                         </td>
                         <td style="width: 15%" class="tdpadd">
						   <div>
						     <asp:Button runat="server" ID="btnAddNewTransactionType" CssClass="customButton" Text="Add New Expense Type" Width="100%" OnClick="BtnAddNewTransactionTypeClick"/>  
						   </div>
                         </td>
					    </tr>
				     </table>
				 </td>
			   </tr>
             <tr>
                <td width="100%" align="left">
                    <asp:DataGrid ID="dgTypesOfExpenseTransactions" runat="server" AutoGenerateColumns="False" CellPadding="1" OnEditCommand="DgTypesOfExpenseTransactionsEditCommand" OnDeleteCommand="DgTypesOfExpenseTransactionsDeleteCommand" CellSpacing="1" GridLines="none" CssClass="xPlugTextAll_x"  DataKeyField="ExpenseTypeId" ShowFooter="True" Width="100%">
                        <FooterStyle CssClass="gridFooter" />
                         <AlternatingItemStyle CssClass="alternatingRowStyle"  />
                         <ItemStyle CssClass="gridRowItem" />
                          <HeaderStyle CssClass="gridHeader" />
                            <Columns>
                                <asp:TemplateColumn HeaderText="S/No.">
                                    <HeaderStyle HorizontalAlign="center" Width="2%" CssClass="tdpadtop" />
                                    <ItemStyle CssClass="xPlugTextAll_x" HorizontalAlign="center" VerticalAlign="Top" />
                                    <ItemTemplate>     
                                        <asp:Label ID="lblSNo" runat="server" CssClass="xPlugTextAll_x" Text="<%# ((dgTypesOfExpenseTransactions.PageSize*dgTypesOfExpenseTransactions.CurrentPageIndex) + Container.ItemIndex + 1)%>">
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Expense Type" >
                                    <HeaderStyle HorizontalAlign="left" Width="16%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Name")) %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Status" >
                                    <HeaderStyle HorizontalAlign="center" Width="5%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" CssClass="xPlugTextAll_x" Text='<%#  ((DataBinder.Eval(Container.DataItem,"Status")).ToString() == "1")? "Active" : "Inactive"%>' ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Edit" >
                            <HeaderStyle HorizontalAlign="center" Width="2%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" AlternateText="Edit" CommandArgument="1" CommandName="Edit" ImageUrl="~/App_Themes/Default/Images/btn_edit_new.gif" ToolTip="Edit" style="cursor:hand" />
                            </ItemTemplate>
                         </asp:TemplateColumn>
                         <asp:TemplateColumn HeaderText="Delete" >
                                <HeaderStyle HorizontalAlign="center" Width="1%" CssClass="tdpadtop" />
                                <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgDelete" runat="server" AlternateText="Delete" OnClientClick="javascript:return confirm('Are you sure you want to delete this item from the list?')" 
                                            CausesValidation="False" CommandArgument="1" CommandName="Delete" ImageUrl="~/App_Themes/Default/Images/btn_delete_new.gif" ToolTip="Delete" style="cursor:hand" />
                                </ItemTemplate>
                         </asp:TemplateColumn>
                               </Columns>
                            <PagerStyle HorizontalAlign="Right" Mode="NumericPages" />
                        </asp:DataGrid>   
                      </td>
                   </tr>
               </tbody>
            </table>
         </div>
        <div class="single-form-display" style="width:30%; border: 0 groove transparent; border-radius: 5px; display: none" runat="server" id="dvProcessTransactionTypes" >
        <asp:Panel ID="PanelTransactionType" runat="server" Width="98%"><uc2:ErrorDisplay_1 ID="ErrorDisplayTransactionType" runat="server" /></asp:Panel>
        <fieldset style="">
		    <legend style="" runat="server" id="lgTitle">Create New Expense Type</legend>
            <table style="width:100%; margin-top: -1%; margin-bottom: 2%; padding: 2px; border: none" runat="server" id="tblCreateTransaction">
                    <tr>
                        <td class="tdpad">
                            <div><i>Expense Type</i><asp:RequiredFieldValidator ValidationGroup="valTransactionType" ID="ReqApprovedTotalAmount"  runat="server" ErrorMessage="* Required" ControlToValidate="txtTransactionType" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator><%--<asp:RegularExpressionValidator ID="RegularExpAmountPaid" runat="server" ControlToValidate="txtTransactionType" Display="Dynamic" ErrorMessage="* Invalid Entry" Font-Size="8pt" ForeColor="Red" SetFocusOnError="True" ValidationExpression="<%$ AppSettings:NameValidationExpression4 %>" ValidationGroup="valTransactionType"></asp:RegularExpressionValidator>--%> </div>
					        <div> <asp:TextBox ID="txtTransactionType" runat="server" Width="99%" CssClass="text-box"></asp:TextBox></div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            
                        </td>
                    </tr>
                    <tr>
                    <td colspan="2" class="tdpad">
                        
                                    <asp:CheckBox runat="server" ID="chkTransactionType" Text="Active?" 
                                        CssClass="customNewCheckbox" Width="155%"/>
                            </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                               
                                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="BtnSubmitClick"  CssClass="customButton" CommandArgument="1" ValidationGroup="valTransactionType" Width="110px" />&nbsp;
					                <input type="button" id="btnReset" runat="server" style="width: 80px; " value="Close" class="customButton"  /> 
				              
                                </td>
                </tr>
             </table>
        </fieldset>
    </div>	
</div>  