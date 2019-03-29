<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="frmManageExpenseItems.ascx.cs" Inherits="ExpenseManager.ExpenseMgt.FrmManageTransactionItems1" %>
<%@ Register TagPrefix="uc2" TagName="ErrorDisplay" Src="~/CoreFramework/ErrorControl/ErrorDisplay.ascx" %>
 <%@ Register src="../CoreFramework/AlertControl/ConfirmAlertBox.ascx" tagname="ConfirmAlertBox" tagprefix="uc1" %>
 <link href="App_Themes/Default/ControlStyles.css" rel="stylesheet" type="text/css" />

<div class="dvContainer">
     <h2 id="hTitle">Manage Expense Items</h2>
     <uc1:ConfirmAlertBox ID="ConfirmAlertBox1" runat="server" />
    <div> <asp:Button ID="btnPopUp" runat="server" Style="display: none" /></div>
     <ajaxToolkit:ModalPopupExtender ID="mpeExpenseItemsPopup" BackgroundCssClass="popupBackground"  TargetControlID="btnPopUp" CancelControlID="btnClose" PopupControlID="dvExpensePayment" RepositionMode="RepositionOnWindowResizeAndScroll" runat="server"></ajaxToolkit:ModalPopupExtender>
     <div class="single-form-display" style="width:35%; border: 0 groove transparent; border-radius: 5px; display: none"  runat="server" id="dvExpensePayment">
        <div style="width: 98%"><uc2:ErrorDisplay ID="ErrorDisplayProcessTransactionItems" runat="server" /></div>
        <fieldset >
			<legend runat="server" id="lgTitleScope" style="">Create New Expense Item</legend>
            <table style="width:100%; padding: 3px; border: none">
                 <tr>
                      <td style="width:100%" class="tdpad">
						Select Expense Category <asp:RequiredFieldValidator ID="ReqExpenseCategory" ValidationGroup="valExpenseItems" runat="server" ErrorMessage="* Required" ControlToValidate="ddlExpenseCategory" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator> <asp:CompareValidator ID="cmpCustomer" ValidationGroup="valExpenseItems" runat="server" ErrorMessage="* Invalid Selection" ValueToCompare="1" ControlToValidate="ddlExpenseCategory" Operator="GreaterThanEqual" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:CompareValidator>
						     <asp:DropDownList ID="ddlExpenseCategory"  runat="server"></asp:DropDownList>
						    <ajaxToolkit:cascadingdropdown ID="ccdExpenseCategory" runat="server" TargetControlID="ddlExpenseCategory" ParentControlID="" Category="ExpenseCategoryId" LoadingText="Loading Expense Categories. Please wait ..."  PromptText="--- Select Expense Category ---" PromptValue="0" EmptyValue="0" EmptyText="-- List is empty --" SelectedValue="0" ServiceMethod="LoadExpenseCategoriesService" ServicePath="~/expenseManagerStructuredServices.asmx"></ajaxToolkit:cascadingdropdown>
                      </td> 
                    </tr>
                    <tr>
                      <td style="width:100%" class="tdpad">
						Select Accounts Head <asp:RequiredFieldValidator ID="ReqAccountsHead" ValidationGroup="valExpenseItems" runat="server" ErrorMessage="* Required" ControlToValidate="ddlAccountsHead" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator> <asp:CompareValidator ID="CompareValidator1" ValidationGroup="valExpenseItems" runat="server" ErrorMessage="* Invalid Selection" ValueToCompare="1" ControlToValidate="ddlAccountsHead" Operator="GreaterThanEqual" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:CompareValidator>
						   <asp:DropDownList ID="ddlAccountsHead"  runat="server" ></asp:DropDownList>
                             <ajaxToolkit:cascadingdropdown ID="ccdAccountsHead" runat="server" TargetControlID="ddlAccountsHead" ParentControlID="ddlExpenseCategory" Category="AccountsHeadId" LoadingText="Loading Accounts Head. Please wait ..."  PromptText="--- Select Accounts Head ---" PromptValue="0" EmptyValue="0" EmptyText="-- List is empty --" SelectedValue="0" ServiceMethod="LoadAccountsHeadList" ServicePath="~/expenseManagerStructuredServices.asmx"></ajaxToolkit:cascadingdropdown>
                        
					</td>
                  </tr>
                  <tr>
                    <td style="width:100%" class="tdpad">
					    Item<asp:RequiredFieldValidator ID="ReqTitle" ValidationGroup="valExpenseItems"  runat="server" ErrorMessage="* Required" ControlToValidate="txtTitle" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator> 
					     <asp:TextBox ID="txtTitle" Width="100%" runat="server" CssClass="text-box"></asp:TextBox>
				    </td>
                </tr>
                <tr>
                    <td style="width:100%" class="tdpad">
					   Description <asp:RequiredFieldValidator ID="ReqDescription" ValidationGroup="valExpenseItems"  runat="server" ErrorMessage="* Required" ControlToValidate="txtDescription" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> </asp:RequiredFieldValidator>
					    <asp:TextBox ID="txtDescription" Width="100%" runat="server" CssClass="text-box" TextMode="MultiLine"></asp:TextBox>
				    </td>
                </tr>
                <tr>
				  <td style="width:100%">
				    
					          <asp:CheckBox runat="server" CssClass="customNewCheckbox" ID="chkActivateTransactionItem" Text="Active?"/>
				            </td>
 
			 </tr>
             <tr>
                  <td style="width:60%; text-align: right">
				 <asp:Button ID="btnProcessTransactionItems" runat="server" Text="Submit" OnClick="BtnProcessTransactionItemsClick"  CssClass="customButton" CommandArgument="1" ValidationGroup="valExpenseItems" Width="86px"/>&nbsp; 
				<asp:Button ID="btnClose" CssClass="customButton" runat="server" Text="Close" Width="86px"/> 
                              
              </td>
             </tr>
         </table>
       </fieldset>
    </div>  
    <div runat="server" id="divReport" >
        <table style="width: 100%; border: none; padding: 0px">
			<tr class="divBackground2 ">
			    
                <td class="tdpad" style="width: 30%">
                   <label class="label2"> Retrieve Expense Items by an Accounts Head:</label> 
                </td>
                <td class="tdpad" style="width: 38%">
                   <asp:DropDownList runat="server" ID="ddlAccountsHeads" AutoPostBack="True" OnSelectedIndexChanged="DdlAccountsHeadsSelectedChanged"/> 
                </td>
                  <td class="tdpad" style="width: 35%">
                  </td>

                <%--<td class="tdpad" style="width: 20%">
                 <asp:LinkButton ID="lnkShowAll" Text="Retrieve All Expense Items" CssClass="linkStyle" ForeColor="green" OnClick="LnkShowAllClick" runat="server" Width="219px"></asp:LinkButton>  
                         
                      
                    
                </td>--%>
            </tr>
    </table>
    <div  style="width: 100%;" class="gridDiv">
     <table  style="width: 100%" runat="server" id="tbExpenseType">
         <tr class="divBackground">
			<td  style="width: 90%" class="tdpadd">
				
					<label class="label">Registered Expense Items</label> 
				
			</td>
			<td style="width: 10%" class="tdpadd">
				
					<asp:Button ID="btnAddNewExpenseItem" runat="server" Text="Add New Expense Item" CssClass="customButton" onclick="BtnAddNewExpenseItemClick" Width="163px" />
                
			</td>
		 </tr>
         <tr>
            <td width="100%"  colspan="2">
                <asp:DataGrid ID="dgExpenseItem" runat="server" AutoGenerateColumns="False" CellPadding="1" OnEditCommand="DgExpenseItemEditCommand" CellSpacing="1" GridLines="none" CssClass="xPlugTextAll_x"  DataKeyField="ExpenseItemId" ShowFooter="True" Width="100%">
                    <FooterStyle CssClass="gridFooter" />
                    <AlternatingItemStyle CssClass="alternatingRowStyle"/>
                    <ItemStyle CssClass="gridRowItem" />
                    <HeaderStyle CssClass="gridHeader" />
                    <Columns>
                        <asp:TemplateColumn HeaderText="S/No.">
                            <HeaderStyle HorizontalAlign="center" Width="1%" CssClass="tdpadtop" />
                            <ItemStyle CssClass="xPlugTextAll_x" HorizontalAlign="center" VerticalAlign="Top" />
                            <ItemTemplate>
	                            <asp:Label ID="lblSNo" runat="server" CssClass="xPlugTextAll">
		                            <%# (NowViewing*Limit)+(Container.ItemIndex + 1) %>
	                            </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Expense Item" >
                            <HeaderStyle HorizontalAlign="left" Width="15%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:label ID="lblTitle" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Title")) %>' >
                                </asp:label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Expense Category" >
                            <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblExpenseCategoryTitle" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"ExpenseCategory.Title")) %>' >
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Accounts Head" >
                            <HeaderStyle HorizontalAlign="left" Width="12%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblAccountsHeadTitle" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"AccountsHead.Title")) %>' >
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Description" >
                            <HeaderStyle HorizontalAlign="left" Width="20%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:label ID="lblDescription" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Description")) %>' >
                                </asp:label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Code" >
                            <HeaderStyle HorizontalAlign="left" Width="5%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblCode" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Code")) %>' >
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Status" >
                        <HeaderStyle HorizontalAlign="Left" Width="2%" CssClass="tdpadtop" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" runat="server"  CssClass="xPlugTextAll_x" Text='<%#  ((DataBinder.Eval(Container.DataItem,"Status")).ToString() == "1")? "Active" : "Inactive"%>' >
                            </asp:Label>
                        </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Edit" >
                                <HeaderStyle HorizontalAlign="center" Width="2%" CssClass="tdpadtop"  />
                                <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                                <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" AlternateText="Edit" CommandArgument="1" CommandName="Edit" ImageUrl="~/App_Themes/Default/Images/btn_edit_new.gif" ToolTip="Edit" style="cursor:hand" />
                                    </ItemTemplate>
                           </asp:TemplateColumn>
                       </Columns>
                    <PagerStyle HorizontalAlign="Right" Mode="NumericPages" />
                </asp:DataGrid>   
                <table style="width: 100%" class="gridFoot">
                        <tr>
                        <td style="width: 35%">
		                <span style="float: left; color: rgb(105, 105, 105)">Total Records: <%= DataCount %></span>
                        </td>
                        <td style="width: 10%">
                            <span style=" font-weight: bold; color: rgb(105, 105, 105)"><asp:Label ID="lblCurrentPage1" runat="server"></asp:Label></span>  
                        </td>
                        <td style="width: 5%">
                                <span><label style=" font-weight: bold; color: rgb(105, 105, 105)">Navigation:</label>&nbsp;</span>  
                        </td>
                        <td style="width: 20%">
                        <span id="pagingdiv1" runat="server" style="text-align: right; margin-left: 5%" >
                                
                            <span class="paginationn" style="display: inline; width: auto; float: left">
                                    
                            <ul>
                                <li id="listNav1" runat="server">&nbsp;</li>         
                                <li id="listNav2" runat="server"> <asp:LinkButton ID="lblnFirst" runat="server" Text=" first " OnClick="LbtnFirstClick" ></asp:LinkButton></li>  
                                <li id="listNav3" runat="server"> <asp:LinkButton   ID="lblnPrev" runat="server" Text=" previous " OnClick="LbtnPrevClick"></asp:LinkButton> </li>         
                                <li id="listNav4" runat="server"><asp:LinkButton ID="lblnNext" runat="server" Text=" next " OnClick="LbtnNextClick"></asp:LinkButton></li> 
                                <li id="listNav5" runat="server"><asp:LinkButton ID="lblnLast" runat="server" Text="last " OnClick="LbtnLastClick"></asp:LinkButton></li> 
                            </ul>                 
                        </span>               
                        </span> 
                        </td>
                        <td style="width: 30%">
                        <span style=" font-weight: bold; margin-left: 30%"><span>Items Per Page&nbsp;&nbsp;</span><asp:DropDownList 
                                CssClass="span1" runat="server" AutoPostBack="True"  OnSelectedIndexChanged="OnLimitChange" ID="ddlLimit" Width="163px"/></span>  
                        </td>
                       </tr>
                     </table>   
              </td>
           </tr>
        </table>
     </div>
   </div>
</div>