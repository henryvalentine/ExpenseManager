<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FrmAmendRejectedTransaction.ascx.cs" Inherits="ExpenseManager.ExpenseMgt.FrmAmendRejectedTransaction" %>

<%@ Register src="../CoreFramework/AlertControl/ConfirmAlertBox.ascx" tagname="ConfirmAlertBox" tagprefix="uc1" %>
<link href="App_Themes/Default/ControlStyles.css" rel="stylesheet" type="text/css" />
      
  <div class="dvContainer">
    <h2 runat="server" id="hTitle">Manage Pending And Rejected Transactions</h2>	
    <uc1:ConfirmAlertBox ID="ConfirmAlertBox1" runat="server" />
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
						            <div style="width:100%"> <asp:DropDownList ID="ddlExpenseItem" CssClass="text-box"  Width="100%" runat="server"></asp:DropDownList> </div>
                                </td>
                                <td  style="width:50%" class="tdpad">
                                  <div><i>&nbsp;Expense Type</i> <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*Required" ControlToValidate="ddlExpenseType"  ValidationGroup="valTransactions" SetFocusOnError="True" Display="Dynamic"  CssClass="errorClass"></asp:RequiredFieldValidator> <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="* Invalid Selection" ValueToCompare="1" ControlToValidate="ddlExpenseType" Operator="GreaterThanEqual"  ValidationGroup="valTransactions" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" ></asp:CompareValidator></div>
						          <div style="width:100%"> <asp:DropDownList ID="ddlExpenseType" CssClass="text-box"   runat="server"></asp:DropDownList> </div>
                                </td>
                            </tr>
                            <tr>
                                
                                <td style="width:50%" class="tdpad">
					                <div><i>&nbsp; Quantity</i><asp:RequiredFieldValidator ID="RequiredFieldValidator4"  runat="server" ErrorMessage="* Required" ControlToValidate="txtQuantity" SetFocusOnError="true" Text="" Display="Dynamic"  ValidationGroup="valTransactions" CssClass="errorClass"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtQuantity" Display="Dynamic" ErrorMessage="* Invalid Entry" Font-Size="8pt" ForeColor="Red" SetFocusOnError="True" ValidationExpression="<%$ AppSettings:NumberValidationExpression3 %>" ValidationGroup="valTransactions"></asp:RegularExpressionValidator> </div>
					                <div> <asp:TextBox ID="txtQuantity"   runat="server" CssClass="text-box"></asp:TextBox></div>
				                </td> 
                                <td rowspan="2" class="tdpad">
					                <div><i>Transaction Item Description</i><asp:RequiredFieldValidator ID="ReqItemDescription"  runat="server" ErrorMessage="* Required"  ValidationGroup="valTransactions" ControlToValidate="txtItemDescription" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator></div>
					                <div> <asp:TextBox ID="txtItemDescription" runat="server"  Width="302px" CssClass="text-box" TextMode="MultiLine" Height="88px"></asp:TextBox></div>
				                </td>   
                                </tr>
                                <tr>
                                 <td style="width:50%" class="tdpad">
					                <div><i>Unit Price</i><asp:RequiredFieldValidator ID="RequiredFieldValidator3"  runat="server" ErrorMessage="* Required"  ValidationGroup="valTransactions" ControlToValidate="txtUnitPrice" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtUnitPrice" Display="Dynamic" ErrorMessage="* Invalid Entry" Font-Size="8pt" ForeColor="Red" SetFocusOnError="True" ValidationExpression="<%$ AppSettings:NumberValidationExpression2 %>" ValidationGroup="valTransactions"></asp:RegularExpressionValidator> </div>
					                <div> <asp:TextBox ID="txtUnitPrice" runat="server"  Width="314px" CssClass="text-box"></asp:TextBox></div>
				                </td>                  
                             </tr>
                              <tr>
                                <td  class="tdpad" style="width: 50%; height: 20px;">
					            </td>
                                <td class="tdpad" style="width: 50%; text-align:right"> <div>
					                    <asp:Button ID="btnUpdateTransactionItem" runat="server" Text="Update" OnClick="BtnProcessExpenseTransactionsClick" CssClass="customButton"  ValidationGroup="valTransactions" CommandArgument="1" Width="114px" />&nbsp;&nbsp;
						                <input type="button" class="customButton"  value="Close" id="btnCancelUpdateTransactionItem" />
					                </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </fieldset>
     </div>
     <div style="width:25%; display: none; border-radius: 5px;" class="single-form-display" id="dvConfirmation">
      <fieldset>
         <legend>Delete Item and Transaction</legend>
         <table style="width:100%;">
           <tr>
              <td>
                <label id="lblMessage" style="width:25%; border-radius: 5px; border: 0; height: 115px;" runat="server"></label>
              </td>
          </tr>
          <tr>
              <td>
                 <div style="margin-top: 1%; width: 123px; margin-left: 60%">
                     <asp:Button ID="btnDeleletTransaction" Text="Yes" Width ="39%" CssClass="customButton" OnClick="BtnDeleletTransactionClick" runat="server"/> &nbsp; 
                     <input type="button" id="btnNO" value="No" class="customButton" style="width : 47%"/>
                  </div> 
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
							    <div style="width: 99%; font-weight: bolder">
							        <label class="label">Requested Transactions</label> 
                                </div>
						    </td>
                        </tr>
                        <tr>
                          <td colspan="2" style="width: 100%">
                              <table style="width: 100%">
                                  <tr>
                                      <td colspan="7" class="tdpadd">
                                          <label style="width: 100%">
                                             Note on <b>FILTERING</b>: &nbsp;Select a <b>Status</b> and then narrow it down by <b>Date Range</b>
                                          </label>
                                      </td>
                                  </tr>
                                  <tr class="divBackground3">
                                     <td class="tdpad" style="width: 10%">
                                        <label style="margin-left: 70%">Status</label>
                                     </td>
                                 <td class="tdpad" style="width: 15%">
                                  <asp:DropDownList runat="server" ID="ddlAmendFilterOption" Width="90%" CssClass="text-box"></asp:DropDownList>
                                 </td>
                                  <td class="tdpad" style="width: 3%">
                                   <label> From:</label>   
                                 </td>
                                 <td class="tdpad" style="width: 15%">
                                  <asp:TextBox   runat="server" ID="txtStart" CssClass="text-box"></asp:TextBox>   
                                 </td>
                                
                                 <td class="tdpad" style="width: 3%">
                                   <label> To:</label>   
                                 </td>
                                 <td class="tdpad" style="width: 15%"><asp:TextBox   runat="server" ID="txtEndDate" CssClass="text-box"></asp:TextBox>
                                     </td> 
                                      <td style="width: 20%">
                                          <asp:Button ID="btnDateFilter" Text="Go" CssClass="customButton" OnClick="BtnDateFilterClick" runat="server" Width="72px"></asp:Button>
                                              &nbsp;&nbsp;&nbsp;<ajaxToolkit:calendarextender ID="CalendarExtFrom" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtStart"></ajaxToolkit:calendarextender>
                                              <ajaxToolkit:calendarextender ID="CalendarExtTo" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtEndDate"></ajaxToolkit:calendarextender>
                                      </td>
                                      <td style="width: 10%"></td>
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
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Transaction" >
                                    <HeaderStyle HorizontalAlign="left" Width="12%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblTransactionTitle" runat="server" CssClass="linkStyle" Text='<%# (DataBinder.Eval(Container.DataItem,"ExpenseTitle")) %>' CommandName="Edit" >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Requested By" >
                                    <HeaderStyle HorizontalAlign="Left" Width="8%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequester"  runat="server" CssClass="xPlugTextAll_x" Text='<%#  GetUserFullName(int.Parse((DataBinder.Eval(Container.DataItem,"RegisteredById")).ToString()))%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Beneficiary" >
                                    <HeaderStyle HorizontalAlign="Left" Width="7%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblBeneficiary"  runat="server" CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Beneficiary.FullName")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                 <asp:TemplateColumn HeaderText="Date Requested" >
                                    <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequestedDate" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"TransactionDate")) %>' >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Time Requested" >
                                    <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequestedTime" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"TransactionTime")) %>' >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Status" >
                                    <HeaderStyle HorizontalAlign="Left" Width="9%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblApprovalStatus"  runat="server" CssClass="xPlugTextAll_x" Text='<%#  (DataBinder.Eval(Container.DataItem,"ApprovalStatus"))%>' ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Date Rejected" >
                                    <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblApprovedDate" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"DateApproved")) %>' >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Time Rejected" >
                                    <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblApprovedTime" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"TimeApproved")) %>' >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Approver's Comment" >
                                    <HeaderStyle HorizontalAlign="Left" Width="13%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkApproverComment"  runat="server" CssClass="commentLink" Text='View Comment' CommandName="viewComment"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Details" >
                                    <HeaderStyle HorizontalAlign="Left" Width="10%"  CssClass="tdpadtop"/>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkTransactionDetails" Font-Bold="True" runat="server" CssClass="xPlugTextAll_x" Text='View & Edit' CommandName="viewDetails"></asp:LinkButton>
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
							<td style="width: 50%" class="divBackground tdpad">
							    <div style="width: 100%; font-weight: bolder; color: #333;" >
							     <b> Transaction</b>:  <asp:Label ID ="lblTransactionTitle" style="width: 100%; color: #038103" runat="server" Text="" runat="server"></asp:Label>
                                </div> 
							</td>
                            <td style="width: 45%" class="divBackground">
                               <table style="width: 100%">
                                   <tr>
                                       <td style="width: 50%">
                                          <label class="infoLabel">Requested Total Amount:</label>&nbsp;<label id="lblRequestedAmmount" style=" width: 100%; color: #038103" runat="server"></label>
                                       </td>
                                       <td style="width: 50%">
                                         <label class="infoLabel">Approved Total Amount:</label>&nbsp;<label id="lblApprovedTotalAmount" style=" width: 100%; color: #038103" runat="server"></label> 
                                       </td>
                                   </tr>
                               </table>
                            </td>
                            <td style="width: 5%" class="divBackground">
                                <div style="width: 100%; margin-left: 1%">
                                    <asp:Button ID="btnBackNav" runat="server" Text="<< Back" CssClass="customButton" Width="94%" OnClick="BtnBackNavClick"/>
                                </div>
				            </td>
						</tr>
					 </table>
			      </td>
			  </tr>
             <tr>
              <td colspan="2" style="width: 100%">
                  <label style="width: 100%">
                     
                  </label>
              </td>
           </tr>
           <tr>
           <td style="width: 100%">
              <asp:DataGrid ID="dgExpenseItem" runat="server" AutoGenerateColumns="False" CellPadding="1"  CellSpacing="1" GridLines="None" CssClass="xPlugTextAll_x"  DataKeyField="TransactionItemId" 
              ShowFooter="True" Width="100%" OnEditCommand="DgTransactionItemEditCommand" OnDeleteCommand="DgTransactionItemDeleteCommand">
                <FooterStyle CssClass="gridFooter" />
                <AlternatingItemStyle CssClass="alternatingRowStyle"  />
                <ItemStyle CssClass="gridRowItem" />
                <HeaderStyle CssClass="gridHeader" />
                    <Columns>
                        <asp:TemplateColumn HeaderText="S/No.">
                            <HeaderStyle HorizontalAlign="center" Width="2%"  CssClass="tdpadtop"  />
                            <ItemStyle CssClass="xPlugTextAll_x" HorizontalAlign="center" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblSNo" runat="server" CssClass="xPlugTextAll_x" Text="<%# ((dgExpenseItem.PageSize*dgExpenseItem.CurrentPageIndex) + Container.ItemIndex + 1)%>">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Expense Item" >
                            <HeaderStyle HorizontalAlign="left" Width="20%"  CssClass="tdpadtop"  />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblExpenseItemTitle" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"ExpenseItem.Title")) %>' >
                                </asp:Label>
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
                        <asp:TemplateColumn HeaderText="Requested Quantity" >
                            <HeaderStyle HorizontalAlign="left" Width="10%"  CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblQuantity" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"RequestedQuantity")) %>' >
                                </asp:Label>
                            </ItemTemplate>
                            <%--<FooterTemplate>
                               <asp:Label ID="lblTotalQuantity" runat="server" Text="" Font-Bold="False" ></asp:Label>
                            </FooterTemplate>--%>
                        </asp:TemplateColumn>
                         <asp:TemplateColumn HeaderText="Approved Quantity" >
                            <HeaderStyle HorizontalAlign="left" Width="10%"  CssClass="tdpadtop"  />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblApprovedQuantity" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"ApprovedQuantity")) %>' >
                                </asp:Label>
                            </ItemTemplate>
                            <%--<FooterTemplate>
                               <asp:Label ID="lblTotalApprovedQuantity" runat="server" Text="" Font-Bold="False" ></asp:Label>
                            </FooterTemplate>--%>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Requested Unit Price" >
                            <HeaderStyle HorizontalAlign="left" Width="10%"  CssClass="tdpadtop"  />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblUnitPrice" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"RequestedUnitPrice")) %>' ></asp:Label>
                            </ItemTemplate>
                             <FooterTemplate>
                               <asp:Label ID="lblTotalUnitPrice" runat="server" Text="" Font-Bold="False" ></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Approved Unit Price" >
                            <HeaderStyle HorizontalAlign="left" Width="10%"  CssClass="tdpadtop"  />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblApprovedUnitPrice" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"ApprovedUnitPrice")) %>' ></asp:Label>
                            </ItemTemplate>
                             <FooterTemplate>
                               <asp:Label ID="lblTotalApprovedUnitPrice" runat="server" Text="" Font-Bold="False" ></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Edit" >
                            <HeaderStyle HorizontalAlign="center" Width="7%"  CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" CommandArgument="1" CommandName="Edit" ImageUrl="~/App_Themes/Default/Images/btn_edit_new.gif" style="cursor:hand" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Delete" >
                                <HeaderStyle HorizontalAlign="center" Width="7%"  CssClass="tdpadtop"  />
                                <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgDelete" runat="server" OnClientClick="javascript:return confirm('Are you sure you want to delete this item from the list?')" CausesValidation="False" CommandArgument="1" CommandName="Delete" ImageUrl="~/App_Themes/Default/Images/btn_delete_new.gif" style="cursor:hand" />
                                </ItemTemplate>
                         </asp:TemplateColumn>
                    </Columns>
                   <PagerStyle HorizontalAlign="Right" Mode="NumericPages" />
                </asp:DataGrid> 
             </td>
           </tr>  
           <tr>
               <td style="width: 100%" class="divBackground">
                    <div style="width: 10%; margin-left: 90%; display: none">
                        <asp:Button ID="btnSubmit" runat="server"  CommandArgument="0" />
                    </div>
				</td>
           </tr>         
         </tbody>
      </table>
   </div> 
   <div  class="single-form-display" style="width:40%; border: 0 groove transparent; border-radius: 5px; display: none" runat="server" id="dvRejection">
       <fieldset style="border-radius: 5px; border: 1px groove #038103">
			<legend runat="server" id="lgCommentTitle" style="">Transaction Rejection Comment</legend>
            <table style="width: 100%; height: 90%">
                <tr>
                   <td colspan="2" style="width: 100%; height: 98%">
                       <asp:TextBox  runat="server" id="txtRejComment" TextMode="MultiLine" Width = "100%" Height = "170px" CssClass="text-box" ></asp:TextBox>
                   </td> 
                </tr>
                <tr>
                    <td>
                         <div style="margin-left: 85%; width: 56px;">
                            <input type="button" class="customButton" style="width: 111%" value="Close" id="closerejection" /> 
                        </div>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <div> <asp:Button ID="btnPopUp" runat="server" Style="display: none" /></div>
     <ajaxToolkit:ModalPopupExtender ID="mpeExpenseItemsPopup" BackgroundCssClass="popupBackground"  TargetControlID="btnPopUp" CancelControlID="btnCancelUpdateTransactionItem" PopupControlID="dvModifyTransactionItem" RepositionMode="RepositionOnWindowResizeAndScroll" runat="server"></ajaxToolkit:ModalPopupExtender>
 </div>