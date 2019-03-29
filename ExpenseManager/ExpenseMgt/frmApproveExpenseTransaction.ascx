<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="frmApproveExpenseTransaction.ascx.cs" Inherits="ExpenseManager.ExpenseMgt.FrmApproveExpenseTransaction" %>
<%@ Register src="../CoreFramework/AlertControl/ConfirmAlertBox.ascx" tagname="ConfirmAlertBox" tagprefix="uc1" %>
<link href="App_Themes/Default/ControlStyles.css" rel="stylesheet" type="text/css" />
 
  <div class="dvContainer">
    <uc1:ConfirmAlertBox ID="ConfirmAlertBox1" runat="server" />
    <h2>Approve, Reject, or Void Transaction Requests</h2>
    <div> <asp:Button ID="btnPopUp" runat="server" Style="display: none; visibility: visible" /></div>
      <div class="single-form-display" style="width:40%; border: 0 groove transparent; border-radius: 5px; display: none" id="dvModifyTransactionItem" >
        <fieldset style="border-radius: 5px; border: 1px groove">
            <legend runat="server" id="Legend1" style="">Modify Transaction Item</legend>
            <table style="width:100%; padding: 3px; border: none" runat="server" id="Table1">
                 <tr>
                    <td>
                        <table style="width:100%">
                            <tr>
                                <td style="width:50%" class="tdpad">
						            <div><i>Expense Item</i> <asp:RequiredFieldValidator ID="ReqExpenseItem" runat="server" ErrorMessage="*Required" ControlToValidate="ddlExpenseItem"  ValidationGroup="valTransactions" SetFocusOnError="True" Display="Dynamic"  CssClass="errorClass"></asp:RequiredFieldValidator> <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="* Invalid Selection" ValueToCompare="1" ControlToValidate="ddlExpenseItem" Operator="GreaterThanEqual"  ValidationGroup="valTransactions" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" ></asp:CompareValidator></div>
						            <div style="width:100%"> <asp:DropDownList ID="ddlExpenseItem" Enabled="False" CssClass="text-box"  Width="100%" runat="server"></asp:DropDownList> </div>
                                </td>
                                <td style="width:50%" class="tdpad">
                                  <div><i>&nbsp;Expense Type</i> <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*Required" ControlToValidate="ddlExpenseType"  ValidationGroup="valTransactions" SetFocusOnError="True" Display="Dynamic"  CssClass="errorClass"></asp:RequiredFieldValidator> <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="* Invalid Selection" ValueToCompare="1" ControlToValidate="ddlExpenseType" Operator="GreaterThanEqual"  ValidationGroup="valTransactions" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" ></asp:CompareValidator></div>
						          <div style="width:100%"> <asp:DropDownList ID="ddlExpenseType" CssClass="text-box" Enabled="False" runat="server"></asp:DropDownList> </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:50%" class="tdpad">
					                <div><i>&nbsp;Requested Quantity</i><asp:RequiredFieldValidator ID="RequiredFieldValidator4"  runat="server" ErrorMessage="* Required" ControlToValidate="txtQuantity" SetFocusOnError="true" Text="" Display="Dynamic"  ValidationGroup="valTransactions" CssClass="errorClass"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtQuantity" Display="Dynamic" ErrorMessage="* Invalid Entry" Font-Size="8pt" ForeColor="Red" SetFocusOnError="True" ValidationExpression="<%$ AppSettings:NumberValidationExpression3 %>" ValidationGroup="valTransactions"></asp:RegularExpressionValidator> </div>
					                <div> <asp:TextBox ID="txtQuantity" Enabled="False"  Width="314px" runat="server" CssClass="text-box"></asp:TextBox></div>
				                </td>    
                                <td style="width:50%" class="tdpad">
					                <div><i>Requested Unit Price</i><asp:RequiredFieldValidator ID="RequiredFieldValidator3"  runat="server" ErrorMessage="* Required"  ValidationGroup="valTransactions" ControlToValidate="txtUnitPrice" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtUnitPrice" Display="Dynamic" ErrorMessage="* Invalid Entry" Font-Size="8pt" ForeColor="Red" SetFocusOnError="True" ValidationExpression="<%$ AppSettings:NumberValidationExpression2 %>" ValidationGroup="valTransactions"></asp:RegularExpressionValidator> </div>
					                <div> <asp:TextBox ID="txtUnitPrice" Enabled="False" runat="server" CssClass="text-box"></asp:TextBox></div>
				                </td>
                             </tr> 
                             <tr>
                                 <td style="width:50%" class="tdpad">
                                    <div><i>Requested Total Price</i></div>
                                     <input type="text" disabled="disabled" runat="server" id="txtTotalRequestedPrice" style="width:314px" class="text-box"/> 
                                 </td>
                                 <td style="width:50%" class="tdpad">
                                    <div><i>Approved Total Price</i></div>
                                     <input type="text" id="txtTransTotalPrice" style="width:314px" disabled="disabled" class="text-box" /> 
                                 </td>
                             </tr>
                             <tr>
                             <td style="width:50%" class="tdpad">
					                <div><i>&nbsp;Approved Quantity</i><asp:RegularExpressionValidator ID="RegExApprouvedQuantity" runat="server" ControlToValidate="textApprouvedQuantity" Display="Dynamic" ErrorMessage="* Invalid Entry" Font-Size="8pt" ForeColor="Red" SetFocusOnError="True" ValidationExpression="<%$ AppSettings:NumberValidationExpression3 %>" ValidationGroup="valTransactions"></asp:RegularExpressionValidator> </div>
                                 <div> <input type="text" id="textApprouvedQuantity" onkeyup="return calculateTotalPrice()" onchange="return calculateTotalPrice()" style="width: 314px" runat="server" class="text-box" /></div>
				                </td> 
                                <td style="width:50%" class="tdpad">
					                <div><i>Approved Unit Price</i><asp:RegularExpressionValidator ID="RegExApprovedUnitPrice" runat="server" ControlToValidate="textApprovedUnitPrice" Display="Dynamic" ErrorMessage="* Invalid Entry" Font-Size="8pt" ForeColor="Red" SetFocusOnError="True" ValidationExpression="<%$ AppSettings:FormattedNumberValidationExpression %>" ValidationGroup="valTransactions"></asp:RegularExpressionValidator> </div>
					                <div> <input type="text" id="textApprovedUnitPrice" runat="server" ClientIDMode="Static" onkeyup="return groupDigits()" onchange="return groupDigits();"  Width="314px" class="text-box" /></div>
				                </td> 
                             </tr>
                              <tr>
                                <td style="width: 40%"></td>
                                <td style="width: 60%; text-align: right" >
                                    <div style=" margin-top: 1%;  height: 28px;">
					                    <asp:Button ID="btnUpdateTransactionItem" runat="server" Text="Update" OnClick="BtnProcessExpenseTransactionsClick" CssClass="customButton"  ValidationGroup="valTransactions" CommandArgument="1"  />&nbsp;&nbsp;
						                <input type="button" class="customButton" value="Close" id="btnCancelUpdateTransactionItem" />
					                </div>
					            </td>
                            </tr>

                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                        </table>
                    </td>
                </tr>
            </table>
        </fieldset>
     </div>

    <div runat="server" id="dvUserEntries" style="width: 100%;" class="gridDiv">
      <table style="width : 100%">
        <tbody>
           <tr>
			   <td width="100%">
					<table style="border-style: none; border-color: inherit; border-width: 1px; width: 100%; padding: 0px; height: 42px;">
						<tr class="divBackground">
							<td style="width: 40%" class="tdpadd2">
							   
							        <label class="label">Requested Transaction(s)</label> 
                               
						    </td>
                        </tr>
                        <tr>
                          <td colspan="2" style="width: 100%">
                              <table style="width: 100%">
                                  <tr  class="divBackground3">
                                      
                                    <td class="tdpad" style="width: 20%">
                                        <label class="label3"> Filter by Date Range -</label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<label> From:</label>
                                    </td>  
                                    <td class="tdpad" style="width: 20%">
                                    <asp:TextBox   runat="server" ID="txtStart" CssClass="text-box"></asp:TextBox>   
                                     
                                    </td> 
                                     
                                     <td class="tdpad" style="width: 7%">
                                         <asp:RequiredFieldValidator  ValidationGroup="valGetDate" ID="ReqStartDate"  runat="server" ErrorMessage="* Required" ControlToValidate="txtStart" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator>
                                    </td> 
                                    <td class="tdpad">
                                      <label> To:</label>   
                                    </td>  
                                    <td class="tdpad" style="width: 20%">
                                     <asp:TextBox runat="server" ID="txtEndDate" CssClass="text-box"></asp:TextBox>
                                       
                                    </td>  
                                    
                                    <td class="tdpad" style="width: 7%">
                                        <asp:RequiredFieldValidator  ValidationGroup="valGetDate" ID="RequendDate"  runat="server" ErrorMessage="* Required" ControlToValidate="txtEndDate" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator>
                                    </td>
                                      <td style="width: 30%" class="tdpad">
                                         <asp:Button ID="btnDateFilter" Text="Go" CssClass="customButton" OnClick="BtnDateFilterClick" runat="server" Width="72px" ValidationGroup="valGetDate"></asp:Button>
                                              &nbsp;&nbsp;&nbsp;<ajaxToolkit:calendarextender ID="CalendarExtFrom" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtStart"></ajaxToolkit:calendarextender>
                                              <ajaxToolkit:calendarextender ID="CalendarExtTo" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtEndDate"></ajaxToolkit:calendarextender>
                                        
                                     </td>
                                     <td style="width: 10%" class="tdpad"></td>
                                  </tr>
                              </table>
                          </td>
                       </tr>
					</table>
			     </td>
			  </tr>
            <tr>
            <td style="width: 100%">
               <asp:DataGrid ID="dgUserInitiatedTransactions" runat="server" AutoGenerateColumns="False" CellPadding="1" CellSpacing="1" GridLines="none" CssClass="xPlugTextAll_x" DataKeyField="ExpenseTransactionId" 
                   OnItemCommand ="DgUserInitiatedTransactionsItemCommand" ShowFooter="True" Width="100%" >
                        <FooterStyle CssClass="gridFooter" />
                         <AlternatingItemStyle CssClass="alternatingRowStyle"/>
                         <ItemStyle CssClass="gridRowItem" />
                          <HeaderStyle CssClass="gridHeader" />
                            <Columns>
                                <asp:TemplateColumn HeaderText="S/No.">
                                    <HeaderStyle HorizontalAlign="center" Width="2%" CssClass="tdpadtop" />
                                    <ItemStyle CssClass="xPlugTextAll_x" HorizontalAlign="center" VerticalAlign="Top" />
                                   <ItemTemplate>
	                            <asp:Label ID="lblSNo" runat="server" CssClass="xPlugTextAll">
		                            <%# (NowViewing*Limit)+(Container.ItemIndex + 1) %>
	                                </asp:Label>
                                </ItemTemplate>
                                    <%--<ItemTemplate>     
                                        <asp:Label ID="lblUserTransactionTitleSNo" runat="server" CssClass="xPlugTextAll_x" Text="<%# ((dgUserInitiatedTransactions.PageSize*dgUserInitiatedTransactions.CurrentPageIndex) + Container.ItemIndex + 1)%>">
                                        </asp:Label>
                                    </ItemTemplate>--%>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Transaction" >
                                    <HeaderStyle HorizontalAlign="left" Width="13%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserTransactionTitle" runat="server" CssClass="linkStyle" Text='<%# (DataBinder.Eval(Container.DataItem,"ExpenseTitle")) %>' CommandName="Edit" >
                                        </asp:Label>
                                    </ItemTemplate>
                                    <%--<FooterTemplate>
                                       <asp:Label ID="lblTotal" runat="server" Text="Total" Font-Bold="true" ></asp:Label>
                                    </FooterTemplate>--%>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Requested By" >
                                    <HeaderStyle HorizontalAlign="Left" Width="8%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequester"  runat="server" CssClass="commentLink" Text='<%#  GetUserFullName(int.Parse((DataBinder.Eval(Container.DataItem,"RegisteredById")).ToString()))%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Beneficiary" >
                                    <HeaderStyle HorizontalAlign="Left" Width="12%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblBeneficiary"  runat="server" CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Beneficiary.FullName")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                 <asp:TemplateColumn HeaderText="Date Requested" >
                                    <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserTransactionTransactionDate" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"TransactionDate")) %>' >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Time Requested" >
                                    <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserTransactionTransactionTime" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"TransactionTime")) %>' >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Approval Status" >
                                    <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserTransactionStatus"  runat="server" CssClass="xPlugTextAll_x" Text='<%#  (DataBinder.Eval(Container.DataItem,"ApprovalStatus"))%>' ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Action" >
                                    <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkTransactionDetails"  runat="server" CssClass="xPlugTextAll_x" Text='View & Approve' CommandName="viewDetails"></asp:LinkButton>
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
                        <span style=" font-weight: bold; margin-left: 30%"><span>Items Per Page&nbsp;&nbsp;</span>
                        <asp:DropDownList CssClass="span1" runat="server" AutoPostBack="True"  OnSelectedIndexChanged="OnLimitChange" ID="ddlLimit" Width="163px"/></span>  
                        </td>
                       </tr>
                     </table> 
                 </td>
             </tr>
         </tbody>
      </table>
   </div>
   <div style="width: 100%;" class="gridDiv" runat="server" id="dvTransactionItems">
      <table style="width: 100%">
        <tbody>
           <tr>
			   <td style="width: 100%">
					<table style="border-style: none; border-color: inherit; border-width: 1px; width: 100%; padding: 0px; height: 41px;">
						<tr>
							<td style="width: 40%" class="divBackground tdpad">
							  <label class="label">Transaction: </label> <asp:Label ID ="lblTransactionTitle" CssClass="label" style="width: 100%; font-weight: bolder" runat="server" runat="server"></asp:Label>
                            </td>
                            <td style="width: 20%" class="divBackground tdpad">
                              <label style="font-weight: normal" class="label">Date Requested: </label> <label id="lblTransactionRquestDate" class="label" runat="server"></label>  
                            </td>
                            <td style="width: 30%" class="divBackground">
                               <label class="label">Total Amount Requested: </label>&nbsp;<label id="lblRequestedAmmount" style="font-weight: bold; width: 100%; color: #038103" runat="server"></label>
                                <%--<td style="width: 50%">
                                    Approved Total Amount:&nbsp;<label id="lblApprovedTotalAmount" style="font-weight: bold; width: 100%" runat="server"></label> 
                                </td>--%>
                           </td>
                           <td style="width: 10%" class="divBackground">
                                <div style="width: 88%; margin-left: 10%">
                                   <asp:Button ID="btnBackNav" runat="server" Text="<< Back" CssClass="customButton" Width="99%" OnClick="BtnBackNavClick"/>
                                </div>
						  </td>
					   </tr>
			        </table>
			    </td>
			 </tr>
             <tr>
             <td style="width: 100%" id="tdGrid">
              <asp:DataGrid ID="dgExpenseItem" runat="server" ClientIDMode="Static" AutoGenerateColumns="False" CellPadding="1"  CellSpacing="1" GridLines="None" CssClass="xPlugTextAll_x"  DataKeyField="TransactionItemId" 
              ShowFooter="True" Width="100%" OnEditCommand="DgExpenseTransactionEditCommand">
                <FooterStyle CssClass="gridFooter" />
                <AlternatingItemStyle CssClass="alternatingRowStyle"  />
                <ItemStyle CssClass="gridRowItem" />
                <HeaderStyle CssClass="gridHeader" />
                    <Columns>
                        <asp:TemplateColumn HeaderText="S/No.">
                            <HeaderStyle HorizontalAlign="center" Width="2%" CssClass="tdpadtop" />
                            <ItemStyle CssClass="xPlugTextAll_x" HorizontalAlign="center" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblSNo" runat="server" CssClass="xPlugTextAll_x" Text="<%# ((dgExpenseItem.PageSize*dgExpenseItem.CurrentPageIndex) + Container.ItemIndex + 1)%>">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderTemplate>All&nbsp;<input type="checkbox" runat="server" id="chkAll" onclick="CheckAllChanged(this.id);" ClientIDMode="Static" style="margin-left: 3%"/></HeaderTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="4%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
                            <ItemTemplate>
                                <input type="checkbox" id="chkSelect<%# (DataBinder.Eval(Container.DataItem,"TransactionItemId")) %>" onclick="CheckChanged(this.id);" name="selectOption" style="margin-left: 39%"/>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Expense Item" >
                            <HeaderStyle HorizontalAlign="left" Width="20%" CssClass="tdpadtop" /> 
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <label style="color: darkcyan; cursor: pointer" id="lblExpenseItem<%#(DataBinder.Eval(Container.DataItem,"ExpensenseItemId")) %>" class="xPlugTextLabel" ><%# (DataBinder.Eval(Container.DataItem,"ExpenseItem.Title")) %></label>
                            </ItemTemplate>
                            <FooterTemplate>
                               <asp:Label ID="lblTota" runat="server" Text="Total" Font-Bold="False" ></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Description" >
                            <HeaderStyle HorizontalAlign="left" Width="15%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblDescription" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Description")) %>' >
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Quantity Requested" >
                            <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblQuantity" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"RequestedQuantity")) %>' >
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                         <asp:TemplateColumn HeaderText="Approved Quantity" >
                            <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblApprovedQuantity" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"ApprovedQuantity")) %>' >
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Unit Price(For Request)" >
                            <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblUnitPrice" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"RequestedUnitPrice")) %>' ></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate> 
                               <asp:Label ID="lblTotalUnitPriceRequested" runat="server" Text="" Font-Bold="False" ></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Total Price (For Request)" >
                            <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblRequestedTotalPrice" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"TotalPrice")) %>' ></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                               <asp:Label ID="lblTotalPriceRequested" runat="server" Text="" Font-Bold="False" ></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Approved Unit Price" >
                            <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblApprovedUnitPrice" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"ApprovedUnitPrice")) %>' ></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                               <asp:Label ID="lblTotalUnitPriceApproved" runat="server" Text="" Font-Bold="False" ></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Approved Total Price" >
                            <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblApprovedTotalPrice" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"ApprovedTotalPrice"))%>' ></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                               <asp:Label ID="lblTotalPriceApproved" runat="server" Text="" Font-Bold="False" ></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Edit" >
                            <HeaderStyle HorizontalAlign="center" Width="7%" CssClass="tdpadtop"/>
                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" CommandArgument="1" CommandName="Edit" ImageUrl="~/App_Themes/Default/Images/btn_edit_new.gif" style="cursor:hand" ToolTip="Edit" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                   <PagerStyle HorizontalAlign="Right" Mode="NumericPages" />
                </asp:DataGrid> 
             </td>
           </tr>   
         </tbody>
       </table>
       <table style="width: 100%">
           <tr>
               <td style="width: 30%" class="tdpadd">
                  <fieldset class="approvalFieldset">
		             <legend ><b>Options</b></legend>
                       <table style="width: 100%">
                         <tr>
                            <td style="width: 100%">
                               <div style="margin-left: 1%; width: 94%" id="radioDiv">
					              <label ><b>Approve</b></label> <input type="radio" id="radApprove" value="Approve" name="ApproveStatus" onclick="SendIds();" runat="server" />
                                   <label style="margin-left: 3%"><b>Reject</b></label><input type="radio" id="radReject" value="Reject" name="ApproveStatus" onclick="UncheckSlctns(this.id);" runat="server" />
                                   <label style="margin-left: 3%"><b>Void</b></label><input type="radio" id="radVoid" value="Void" name="ApproveStatus" onclick="UncheckSlctns(this.id);"  runat="server" />
				              </div>
                           </td>
                         </tr>
                         <tr>
                            <td style="width: 100%">
                                <div style="width: 100%; margin-top: 3%">
                                   <div style="margin-top: 10px;"> <b>Comment</b><asp:RequiredFieldValidator ID="ReqDescription"  runat="server" ErrorMessage="* Required" ControlToValidate="txtApproverComment"  ValidationGroup="valTransactionApproval" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> </asp:RequiredFieldValidator></div>
                                   <asp:TextBox runat="server" CssClass="text-box" Width="98%" TextMode="MultiLine" Rows="7" Height="20%" ID="txtApproverComment"></asp:TextBox> 
                                   <div style="width: 20%; margin-left: 78%" ><br/>
                                    <asp:Button runat="server" ID="btnApproveTransaction" CssClass="customButton" ValidationGroup="valTransactionApproval" Text="Submit" OnClick="BtnApproveTransactionClick" OnClientClick="javascript:return confirm('Are you sure of this decision?')" CausesValidation="False" Width="96px"/>
                                </div>          
                            </div> 
                          </td>
                        </tr>
                    </table> 
                 </fieldset>
               </td>
               <td style="width: 50%" class="tdpadd">
                 <div style="width: 40%; margin-left: 40%; margin-top: 8%; margin-bottom: 4%">
                      <label style="width: 100%">
                     <b>NOTE</b>: &nbsp;Tick the <b>Void</b> OR <b>Reject</b> Option to Void or Reject the Transaction.
                     </label>
                   <hr style="border: 1px black solid;"/>
                   <label style="width: 20%">
                   Select the needed Items of the Transaction individually, <b>OR</b> Check <b>All</b> to select all Items, then Tick the <b>Approve</b> option to Approve the Transaction.
                  </label>
                 </div>
              </td>
           </tr>
       </table>
     <div> <asp:Button ID="Button1" runat="server" Style="display: none" /></div>
     <ajaxToolkit:ModalPopupExtender ID="mpeExpenseItemsPopup" BackgroundCssClass="popupBackground"  TargetControlID="btnPopUp" CancelControlID="btnCancelUpdateTransactionItem" PopupControlID="dvModifyTransactionItem" RepositionMode="RepositionOnWindowResizeAndScroll" runat="server"></ajaxToolkit:ModalPopupExtender>
    <%--<ajaxToolkit:ModalPopupExtender ID="mpeSimilarTransaction" BehaviorID="mpeSimile" BackgroundCssClass="popupBackground"  TargetControlID="btnPopUp" CancelControlID="closePopUp" PopupControlID="parentDiv" RepositionMode="RepositionOnWindowResizeAndScroll" runat="server"></ajaxToolkit:ModalPopupExtender>--%>
   </div>
       <%--<div style="width: 20%; margin-left: 40%"><input type="button" id="closePopUp" class="customButton" value="Close"/></div>--%>
   
  </div>
  
  <script type="text/javascript">
     
  </script>