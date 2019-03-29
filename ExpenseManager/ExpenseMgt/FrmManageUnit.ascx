<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FrmManageUnit.ascx.cs" Inherits="ExpenseManager.ExpenseMgt.FrmManageUnit" %>
<%@ Register src="../CoreFramework/AlertControl/ConfirmAlertBox.ascx" tagname="ConfirmAlertBox" tagprefix="uc1" %>
<link href="App_Themes/Default/ControlStyles.css" rel="stylesheet" type="text/css" />
<%@ Register TagPrefix="uc2" TagName="ErrorDisplay_1" Src="~/CoreFramework/ErrorControl/ErrorDisplay.ascx" %>


<div class="dvContainer">
    

 <h2 id="hTitle"> Manage Units </h2>
	 <uc1:ConfirmAlertBox ID="ConfirmAlertBox1" runat="server" />
	 <div> <asp:Button ID="btnPopUp" runat="server" Style="display: none" /></div>
     <ajaxToolkit:ModalPopupExtender ID="mpeProcessUnit" BackgroundCssClass="popupBackground"  TargetControlID="btnPopUp" CancelControlID="btnClose" PopupControlID="dvNewUnit" RepositionMode="RepositionOnWindowResizeAndScroll" runat="server"></ajaxToolkit:ModalPopupExtender>
     <div class="single-form-display" style="width:25%; display: none; border: 0 groove transparent; border-radius: 5px;"  runat="server" id="dvNewUnit">
        <fieldset>
			<legend id="lgTitle" runat="server" style=""></legend>
            <div id="divErrorDispManageUnit" style="width: 98%"><uc2:ErrorDisplay_1 ID="ErrorDispManageUnit" runat="server"/></div>
            <table style="width:100%; padding: 2px; border: none" runat="server" id="tbNewUnit">
                <tr>
                     <td colspan="2">
					    <div><i>Unit</i><asp:RequiredFieldValidator ID="ReqUnit" ValidationGroup="valManageUnit"  runat="server" ErrorMessage="* Required" ControlToValidate="txtName" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> </asp:RequiredFieldValidator></div>
					    <div> <asp:TextBox style="width:98%" ID="txtName" runat="server" CssClass="text-box" ></asp:TextBox></div>
				    </td>
                </tr>
                 <tr>
                     <td colspan="2">
					    <div><i>Department</i><asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="valManageUnit"  runat="server" ErrorMessage="* Required" ControlToValidate="txtName" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> </asp:RequiredFieldValidator></div>
					    <div><asp:DropDownList style="" ID="ddlDepartment" runat="server"  ></asp:DropDownList></div>
				    </td>
                </tr>
               <tr>
                <td style="width: 25%">
				    <div><asp:CheckBox ID="chkUnit" CssClass="customNewCheckbox" runat="server" Text="Active?" Width="85px"  /></div>
                </td>
			</tr>
            <tr>
                <td style="text-align: right">
					 <asp:Button ID="btnProcessUnit" runat="server" Text="Submit" CssClass="customButton" ValidationGroup="valManageUnit"  CommandArgument="1" onclick="BtnProcessUnitClick" Width="97px" />&nbsp;
                     <input type="button" id="btnClose" style="width : 95px" value="Close" class="customButton" />
				 
            </td>
            </tr>
        </table>
      </fieldset>
    </div>
    <div runat="server" id="divReport" class="gridDiv">
       <table border="0" cellspacing="0" cellpadding="2" width="100%" runat="server" id="tbExpenseType">
          <tbody>
             <tr>
			    <td style="width: 100%">
				   <table style="width: 100%; border: none; padding: 0px">
					   <tr class="divBackground">
						  <td style="width: 90%" class="tdpadd">
						   
						        <label class="label">Registered Units</label>
						   
						</td>
						<td style="width: 10%" class="tdpadd">
						    							   <asp:Button ID="btnAddUnit" runat="server" Text="Add New Unit" CssClass="customButton" onclick="BtnAddNewUnitClick"  />
												</td>
					</tr>
				</table>
			</td>
		 </tr>
         <tr>
            <td style="width: 100%">
                <asp:DataGrid ID="dgUnits" runat="server" AutoGenerateColumns="False" CellPadding="1" OnEditCommand="DgUnitsEditCommand" OnDeleteCommand="DgUnitsDeleteCommand" CellSpacing="1" GridLines="none" CssClass="xPlugTextAll_x"  DataKeyField="UnitId" ShowFooter="True" Width="100%">
                    <FooterStyle CssClass="gridFooter" />
                    <AlternatingItemStyle CssClass="alternatingRowStyle"/>
                    <ItemStyle CssClass="gridRowItem" />
                    <HeaderStyle CssClass="gridHeader" />
                    <Columns>
                        <asp:TemplateColumn HeaderText="S/No.">
                            <HeaderStyle HorizontalAlign="center" Width="5%" CssClass="tdpadtop" />
                            <ItemStyle CssClass="xPlugTextAll_x" HorizontalAlign="center" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblSNo" runat="server" CssClass="lDisplay" Text="<%# ((dgUnits.PageSize*dgUnits.CurrentPageIndex) + Container.ItemIndex + 1)%>">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Unit" >
                            <HeaderStyle HorizontalAlign="left" Width="12%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:label ID="lblUnit" runat="server"  CssClass="lDisplay" Text='<%# (DataBinder.Eval(Container.DataItem,"Name")) %>' >
                                </asp:label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Department" >
                            <HeaderStyle HorizontalAlign="left" Width="12%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:label ID="lblDepartment" runat="server"  CssClass="lDisplay" Text='<%# (DataBinder.Eval(Container.DataItem,"Department.Name")) %>' >
                                </asp:label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Status" >
                            <HeaderStyle HorizontalAlign="center" Width="8%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server"  CssClass="lDisplay" Text='<%#  ((DataBinder.Eval(Container.DataItem,"Status")).ToString() == "1")? "Active" : "Inactive"%>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Edit">
                            <HeaderStyle HorizontalAlign="center" Width="8%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" AlternateText="Edit"  CommandArgument="1" CommandName="Edit" ImageUrl="~/App_Themes/Default/Images/btn_edit_new.gif" ToolTip="Edit" style="cursor:hand" />
                            </ItemTemplate>
                         </asp:TemplateColumn>
                         <asp:TemplateColumn HeaderText="Delete" >
                            <HeaderStyle HorizontalAlign="center" Width="2%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:ImageButton ID="imgDelete" runat="server" AlternateText="Delete" OnClientClick="javascript:return confirm('Are you sure you want to delete this item from the list?')" CausesValidation="False" CommandArgument="1" CommandName="Delete" ImageUrl="~/App_Themes/Default/Images/btn_delete_new.gif" ToolTip="Delete" style="cursor:hand" />
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
</div>