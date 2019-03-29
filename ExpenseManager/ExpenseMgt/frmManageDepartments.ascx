<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="frmManageDepartments.ascx.cs" Inherits="ExpenseManager.ExpenseMgt.FrmManageDepartments" %>
<%@ Register src="../CoreFramework/AlertControl/ConfirmAlertBox.ascx" tagname="ConfirmAlertBox" tagprefix="uc1" %>
<link href="App_Themes/Default/ControlStyles.css" rel="stylesheet" type="text/css" />
<%@ Register TagPrefix="uc2" TagName="ErrorDisplay_1" Src="~/CoreFramework/ErrorControl/ErrorDisplay.ascx" %>


<div class="dvContainer">
 <h2 id="hTitle"> Manage Departments </h2>
	 <uc1:ConfirmAlertBox ID="ConfirmAlertBox1" runat="server" />
	 <div> <asp:Button ID="btnPopUp" runat="server" Style="display: none" /></div>
     <ajaxToolkit:ModalPopupExtender ID="mpeProcessDepartment" BackgroundCssClass="popupBackground"  TargetControlID="btnPopUp" CancelControlID="btnClose" PopupControlID="dvNewDepartment" RepositionMode="RepositionOnWindowResizeAndScroll" runat="server"></ajaxToolkit:ModalPopupExtender>
     <div class="single-form-display" style="width:25%; display: none; border: 0 groove transparent; border-radius: 5px;"  runat="server" id="dvNewDepartment">
       <div id="divErrDepartments" style="width: 98%"><uc2:ErrorDisplay_1 ID="ErrorDisplayProcessDepartment" runat="server" /></div>
        <fieldset>
			<legend id="lgTitle" runat="server" style=""></legend>
            <table style="width:100%; padding: 2px; border: none" runat="server" id="tbNewDepartment">
                <tr>
                     <td colspan="2">
					    <div><i>Name</i><asp:RequiredFieldValidator ID="ReqDepartment" ValidationGroup="valManageDepartment"  runat="server" ErrorMessage="* Required" ControlToValidate="txtName" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> </asp:RequiredFieldValidator></div>
					    <div> <asp:TextBox style="width:98%" ID="txtName" runat="server" CssClass="text-box" ></asp:TextBox></div>
				    </td>
                </tr>
               <tr>
                    <td style="width: 25%">
				        <div><asp:CheckBox ID="chkDepartment" CssClass="customNewCheckbox" runat="server" 
                                Text="Active?" Width="85px"  /></div>
                    </td>
               
			</tr>
            <tr>
             
                <td style="width: 100%; text-align: right">
              <asp:Button ID="btnProcessDepartment" runat="server" Text="Submit" CssClass="customButton" ValidationGroup="valManageDepartment"  CommandArgument="1" onclick="BtnProcessDepartmentClick" Width="97px" />      
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
			    <td width="100%">
				   <table style="width: 100%; border: none; padding: 0px">
					   <tr class="divBackground">
						  <td style="width: 90%" class="tdpadd" >
						     <div style="width: 67%; font-size: 1em; font-weight: bolder" >
						        <label class="label">Registered Departments</label>
						     </div> 
						</td>
						<td style="width: 10%" class="tdpadd">
						    <div style="margin-left: 5%; width: 179px;" >
							   <asp:Button ID="btnAddItem" runat="server" Text="Add New Department" CssClass="customButton" onclick="BtnAddNewDepartmentClick" Width="165px" />
							</div>
						</td>
					</tr>
				</table>
			</td>
		 </tr>
        <tr>
            <td style="width: 100%">
                <asp:DataGrid ID="dgDepartments" runat="server" AutoGenerateColumns="False" CellPadding="1" OnEditCommand="DgDepartmentsEditCommand" OnDeleteCommand="DgDepartmentsDeleteCommand" CellSpacing="1" GridLines="none" CssClass="xPlugTextAll_x"  DataKeyField="DepartmentId" ShowFooter="True" Width="100%">
                    <FooterStyle CssClass="gridFooter" />
                    <AlternatingItemStyle CssClass="alternatingRowStyle"/>
                    <ItemStyle CssClass="gridRowItem" />
                    <HeaderStyle CssClass="gridHeader" />
                    <Columns>
                        <asp:TemplateColumn HeaderText="S/No.">
                            <HeaderStyle HorizontalAlign="center" Width="5%" CssClass="tdpadtop" />
                            <ItemStyle CssClass="xPlugTextAll_x" HorizontalAlign="center" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblSNo" runat="server" CssClass="xPlugTextAll_x" Text="<%# ((dgDepartments.PageSize*dgDepartments.CurrentPageIndex) + Container.ItemIndex + 1)%>">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Department" >
                            <HeaderStyle HorizontalAlign="left" Width="22%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:label ID="lblDepartment" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Name")) %>' >
                                </asp:label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Status" >
                            <HeaderStyle HorizontalAlign="center" Width="8%" CssClass="tdpadtop"/>
                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server"  CssClass="xPlugTextAll_x" Text='<%#  ((DataBinder.Eval(Container.DataItem,"Status")).ToString() == "1")? "Active" : "Inactive"%>' ></asp:Label>
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
                            <HeaderStyle HorizontalAlign="center" Width="2%" CssClass="tdpadtop"/>
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