<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="frmManageExpensePayment.ascx.cs" Inherits="ExpenseManager.ExpenseMgt.FrmManageExpensePayment" %>
<%@ Register src="~/CoreFramework/AlertControl/ConfirmAlertBox.ascx" tagname="ConfirmAlertBox" tagprefix="uc1" %>
<%@ Register TagPrefix="uc2" TagName="ErrorDisplay_1_1" Src="~/CoreFramework/ErrorControl/ErrorDisplay.ascx" %>
<link href="App_Themes/Default/ControlStyles.css" rel="stylesheet" type="text/css" />

<div class="dvContainer">
    <div> <asp:Button ID="btnPopUp" runat="server" Style="display: none" /></div>
    <h2 runat="server" id="hTitle">Transaction Payment</h2> 
	 <uc1:ConfirmAlertBox ID="ConfirmAlertBox1" runat="server" />
     <input type="button" runat="server" id="closePop" style="display: none"/>
    <ajaxToolkit:ModalPopupExtender ID="mpeSelectDateRangePopup" BackgroundCssClass="popupBackground"  TargetControlID="btnPopUp" CancelControlID="btnReset" PopupControlID="dvExpensePayment" RepositionMode="RepositionOnWindowResizeAndScroll" runat="server"></ajaxToolkit:ModalPopupExtender>
    <div class="single-form-display" style="width:53%; border: 0 groove transparent; border-radius: 5px;display: none " runat="server" id="dvExpensePayment" >
       <fieldset>
			<legend class="label"><b>Transaction Details</b></legend>
            <div id="dvExpensePaymentError" style="width: 98%"><uc2:ErrorDisplay_1_1 ID="ErrorDisplayExpensePayment" runat="server"/></div>
            <table style="width:100%; border: none" runat="server" id="tblCreateTransaction">
                <tr>
                   <td class="tdpad">
                      <div><i>Total Approved Quantity</i></div>
					  <div> <asp:TextBox ID="txtApprovedQuantity" Enabled="False" ClientIDMode="Static" runat="server" CssClass="text-box"></asp:TextBox></div>
                    </td>
                     <td class="tdpad">
                      <div><i>Total Approved Amount</i></div>
                       
					  <div> <asp:TextBox ID="txtApprovedTotalAmount" runat="server" ClientIDMode="Static" Enabled="False"  CssClass="text-box"></asp:TextBox></div>
                   </td>
                </tr>
                <tr>
                  <td class="tdpad">
                      <div><i>Date Approved</i></div>
					  <div> <asp:TextBox ID="txtApprovedDate" runat="server" ClientIDMode="Static" Enabled="False"  CssClass="text-box"></asp:TextBox></div>
                   </td>
                   <td class="tdpad">
                      <div><i>Time Approved</i></div>
					  <div> <asp:TextBox ID="txtApprovalTime" Enabled="False" ClientIDMode="Static" runat="server"  CssClass="text-box"></asp:TextBox></div>
                   </td>
                </tr>
				<tr>
				   <td colspan="2" style="width: 100%">
				        <fieldset style=""><legend><b>Payment</b></legend>
                            <table style="border-style: none; border-color: inherit; border-width: medium; width:100%;" runat="server" id="tblApproveTransaction">
                                <tr>
                                    <td style="width: 50%" class="tdpad">
                                        <div><i>Cash Amount</i> <asp:RequiredFieldValidator  ValidationGroup="valPayment" ID="ReqAmountPaid"  runat="server" ErrorMessage="* Required" ControlToValidate="txtAmountPaid" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator> <asp:RegularExpressionValidator ID="RegularExpAmountPaid" runat="server" ControlToValidate="txtAmountPaid" Display="Dynamic" ErrorMessage="* Invalid Entry" Font-Size="8pt" ForeColor="Red" SetFocusOnError="True" ValidationExpression="^\$?[0-9]+(,[0-9]{3})*(\.[0-9]{2})?$" ValidationGroup="valPayment"></asp:RegularExpressionValidator></div>
					                    <div><input class="text-box" ClientIDMode="Static" id="txtAmountPaid"  onchange="return calaculateBalance()" runat="server" style="" type="text" /></div>
                                    </td>
                                    <td style="width: 50%" class="tdpad">
                                       <div><i runat="server" id="iBalance">&nbsp; Balance</i></div>
                                        <div> <input type="text" id="txtBalance" disabled="disabled" style="width: 97%; height: 22px;" class="text-box"/></div>
                                    </td>
                                </tr>
                                <tr>
                                     <td colspan="2" class="tdpad" >
                                         <asp:Panel ID="divDeptOwner" runat="server" Visible="False">
                                         <div><i runat="server" id="i2">Department</i><asp:RequiredFieldValidator ValidationGroup="valPayment" id="reqDdlDepartment" ErrorMessage ="* Required" InitialValue="0" ControlToValidate="ddlDepartment" Runat="server" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" /></div>
                                         <div><asp:DropDownList ID="ddlDepartment" CssClass="text-box" runat="server"></asp:DropDownList> </div>
                                         </asp:Panel>
                                     </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="tdpad" >
                                        <div><i>Comment</i><asp:RequiredFieldValidator  ValidationGroup="valPayment" ID="RequComment"  runat="server" ErrorMessage="* Required" ControlToValidate="txtPaymentComment" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator></div>
                                        <div style="width: 97%; height: 100%">
                                            <asp:TextBox runat="server" ID="txtPaymentComment" TextMode="MultiLine" ClientIDMode="Static" Width="102%" Rows="6" Height="78%"></asp:TextBox>
                                        </div>
                                    </td>
                                 </tr>
                                 <tr>
                                     <td style="width: 50%">
                                     </td>
                                    <td style="width: 50%; text-align: right">
                                       <div style="">
                                         <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="BtnSubmitClick" ValidationGroup="valPayment"  CssClass="customButton" CommandArgument="1"  Width="97px" />&nbsp;
					                     <asp:Button ID="btnReset" runat="server" Text="Close" class="customButton" Width="79px" /> 
                                     </div>
                                </td>
                            </tr>
                         </table>
                      </fieldset>
                   </td>
			   </tr>
           </table>
        </fieldset>
    </div>	
    <table style="width: 45%; margin-left: 25%; margin-top: 4%; margin-bottom: 2%">
      <tr>
        <td colspan="2" >
            <fieldset>
			    <legend>Select OR Filter Unpaid Transactions by Date Range</legend>
                <table style="width: 100%">
                    <tr>
                    <td style="width: 30%" class="tdpad">
					   From<asp:RequiredFieldValidator  ValidationGroup="valSearchDate" ID="ReqStartDate"  runat="server" ErrorMessage="* Required" ControlToValidate="txtStart" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator>
                         <asp:TextBox ID="txtStart" ClientIDMode="Static" CssClass="text-box" Width="97%" runat="server"></asp:TextBox>
                        <ajaxToolkit:calendarextender ID="CalendarExtDateFrom" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtStart"></ajaxToolkit:calendarextender>
                   </td>
                    <td style="width: 30%" class="tdpad">
                        To<asp:RequiredFieldValidator  ValidationGroup="valSearchDate" ID="ReqEndDate"  runat="server" ErrorMessage="* Required" ControlToValidate="txtEndDate" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator>
					       <div> <asp:TextBox ID="txtEndDate" runat="server" ClientIDMode="Static" Width="96%" CssClass="text-box"></asp:TextBox>
                           <ajaxToolkit:calendarextender ID="CalendarExtDateTo" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtEndDate"></ajaxToolkit:calendarextender>
                       </div>
                    </td>
                    <td style="width: 40%"class="tdpad">
                        <div><i>&nbsp;&nbsp;&nbsp;</i></div>
                        <div style="margin-left: 5%; width: 217px;">
                           <asp:Button id="btnSearchByDate" CssClass="customButton" Text="Search" onclick="BtnSearchByDateClick" Width=" 45%"  runat="server" ValidationGroup="valSearchDate"/>&nbsp;
                            <asp:Button type="button" id="btnGetAllTransactions" CssClass="customButton" Width=" 45%"  Text="Get All" onclick="BtnGetAllTransactions" runat="server"/>
                        </div>
					</td>
                  </tr>
                   <tr>
                      <td colspan="3">
                          <div style="height: 2px"></div>
                      </td>
                  </tr>
                  <tr>
                    <td colspan="2" class="tdpad">
                      Transaction<asp:RequiredFieldValidator ValidationGroup="valBeneficiaryTransaction" ID="ReqTransaction" runat="server" ErrorMessage="*Required" ControlToValidate="ddlExpenseTransactions" SetFocusOnError="True" Display="Dynamic"  CssClass="errorClass"></asp:RequiredFieldValidator> <asp:CompareValidator ID="CompareValExpenseTransaction"  ValidationGroup="valBeneficiaryTransaction" runat="server" ErrorMessage="* Invalid Selection" ValueToCompare="1" ControlToValidate="ddlExpenseTransactions" Operator="GreaterThanEqual" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" ></asp:CompareValidator>
                      <asp:DropDownList CssClass="text-box" runat="server" id="ddlExpenseTransactions" ClientIDMode="Static" Width ="100%"></asp:DropDownList>
                    </td>
                    <td style="width: 50%" class="tdpad">Payment Mode
                       <asp:DropDownList runat="server" ID="ddlPaymentMode" CssClass="text-box" Width="97%"  />
                    </td>
                   </tr>
                   <tr>
                       <td colspan="2" style="width: 80px">
                       </td>
                       <td  style="width: 20px;text-align: right; font-weight: bold" class="tdpadd">
                        <asp:Button runat="server" ID="btnContinue" Text="Continue" ForeColor="#ffffff" CssClass="btnNext" OnClick="BtnContinueClick" style="width: 45%;"></asp:Button>   
                       </td>
                   </tr>
                </table>
            </fieldset>
          </td>
       </tr>
    </table>
    <div runat="server" id="divReport" style="width: 100%; " class="gridDiv">
     <table border="0" cellspacing="0" cellpadding="2" width="100%" runat="server" id="tbExpenseType">
        <tbody>
           <tr>
               <td style="width: 100%;">
					<table style="width: 100%; border: none; padding: 0px">
						<tr class="divBackground">
							<td style="width: 80%" class="tdpadd2">
						       
						        <label class="label">Current Transaction Payment(s)</label>
                            </td>
					     </tr>
					</table>
			   </td>
			</tr>
          <tr>
          <td style="width:100%">
             <asp:DataGrid ID="dgExpenseTransactionPayment" runat="server" AutoGenerateColumns="False" CellPadding="1"   CellSpacing="1" GridLines="none" CssClass="xPlugTextAll_x"  DataKeyField="ExpenseTransactionPaymentId" ShowFooter="True" Width="100%" >
                    <FooterStyle CssClass="gridFooter" />
                    <AlternatingItemStyle CssClass="alternatingRowStyle"  />
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
                        <asp:TemplateColumn HeaderText="Transaction" >
                            <HeaderStyle HorizontalAlign="left" Width="6%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lnkExpenseTransactionTitle" runat="server" CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"ExpenseTransaction.ExpenseTitle")) %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ExpenseTransaction.ExpenseTitle") %>' CommandName="UpdateTransactionPayment"></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                               <asp:Label ID="lblTotal" runat="server" Text="Total" Font-Bold="true" ></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Total Payable Amount" >
                            <HeaderStyle HorizontalAlign="left" Width="5%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:label ID="lblTotalAmountPayable" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"TotalAmountPayable")) %>' ></asp:label>
                            </ItemTemplate>
                                <FooterTemplate>
                                   <asp:label ID="lblTotalAmountPayableTotal" runat="server" Font-Bold="false" Wrap="false" Text=""></asp:label>
                                </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Amount Already Paid" >
                            <HeaderStyle HorizontalAlign="left" Width="5%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblAmountPaid" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"AmountPaid")) %>' ></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:label ID="lblAmountPaidTotal" runat="server" Font-Bold="false" Wrap="false" Text=""></asp:label>
                            </FooterTemplate>
                            </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Balance" >
                            <HeaderStyle HorizontalAlign="left" Width="5%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblBalance" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Balance")) %>' ></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:label ID="lblBalanceTotal" runat="server" Font-Bold="false" Wrap="false" Text=""></asp:label>
                            </FooterTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Last Payment Date" >
                                <HeaderStyle HorizontalAlign="left" Width="5%" CssClass="tdpadtop" />
                                <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblLastPaymentDate" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"LastPaymentDate")) %>' >
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Last Payment Time" >
                                <HeaderStyle HorizontalAlign="left" Width="4%" CssClass="tdpadtop" />
                                <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblLastPaymentTime" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"LastPaymentTime")) %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Payment Status" >
                                <HeaderStyle HorizontalAlign="Left" Width="4%" CssClass="tdpadtop" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblTransactionStatus" runat="server"  CssClass="xPlugTextAll_x" Text='<%#  ((DataBinder.Eval(Container.DataItem,"Balance")).ToString() == "0")? "Paid" : "Partly Paid" %>' ></asp:Label>
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
       </tbody>
     </table> 
     <div class="single-form-display" style="width:53%; border: 0 groove transparent; border-radius: 5px; display: none" runat="server" id="dvChequePayment" >
       <fieldset >
			<legend class="label"><b>Transaction Details</b></legend>
            <div id="divErrorDispChequePayment" style="width: 98%"><uc2:ErrorDisplay_1_1 ID="ErrorDispChequePayment" runat="server"/></div>
            <table style="width:100%; border: none" runat="server" id="Table1">
                <tr>
                   <td class="tdpad">
                      <div><i>Total Approved Quantity</i></div>
					  <div> <asp:TextBox ID="txtChequeApprovedTotalQuantity" Enabled="False" ClientIDMode="Static" runat="server"   CssClass="text-box"></asp:TextBox></div>
                    </td>
                     <td class="tdpad">
                      <div><i id="iChequeAmountBalance" runat="server">Total Approved Amount</i></div>
					  <div> <asp:TextBox ID="txtChequeApprovedTotalAmount" runat="server" ClientIDMode="Static" Enabled="False" CssClass="text-box"></asp:TextBox></div>
                   </td>
                </tr>
                <tr>
                  <td class="tdpad">
                      <div><i>Date Approved</i></div>
					  <div> <asp:TextBox ID="txtChequeApprovedDate" runat="server" ClientIDMode="Static" Enabled="False" CssClass="text-box"></asp:TextBox></div>
                   </td>
                   <td class="tdpad">
                      <div><i>Time Approved</i></div>
					  <div> <asp:TextBox ID="txtChequeApprovedTime" Enabled="False" ClientIDMode="Static" runat="server" CssClass="text-box"></asp:TextBox></div>
                   </td>
                </tr>
				<tr>
				   <td colspan="2" style="width: 100%" class="tdpad">
				       <div id="div1" style="width: 98%"><uc2:ErrorDisplay_1_1 ID="ErrorDisplay_1_1" runat="server"/></div>
				        <fieldset style=""><legend><b>Payment Details</b></legend>
                            <table style="border-style: none; border-color: inherit; border-width: medium; width:100%; height: 171px;" runat="server" id="Table2">
                                <tr>
                                    <td colspan="2">
                                        <table style="width: 100%; height: 190px; ">
                                            <tr>
                                               <td style="width: 50%" class="tdpad">
                                                   <div style="vertical-align: top"><i>Bank</i><asp:RequiredFieldValidator  ValidationGroup="valChequePayment" ID="ReqBank"  runat="server" ErrorMessage="* Required" ControlToValidate="ddBank" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator><asp:CompareValidator ID="CompareValBanks"  ValidationGroup="valChequePayment" runat="server" ErrorMessage="* Invalid Selection" ValueToCompare="1" ControlToValidate="ddBank" Operator="GreaterThanEqual" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" ></asp:CompareValidator></div>
                                                  <select style="width: 100%;" id="ddBank" runat="server" class="text-box"/>
                                               </td>
                                               <td style="width: 50%" rowspan="4" class="tdpad">
                                                  <div style="width: 100%; margin-left: 2%; height: 173px; vertical-align: top">
                                                       <div style="width: 96%; height: 171px;">
                                                       <img src="" alt="Cheque Copy" id="imgChequeCopy" style="width: 102%; vertical-align: top; height: 122px" /> 
                                                       <div style="width: 246px"><i>Scanned Cheque Copy</i>(<span style="color: Red">*jpeg/jpg only. 500KB Maximum.</span>)</div>
                                                        <asp:FileUpload id="fldChequeCopy" ClientIDMode="Static"  Width=" 100%" Height="22px" runat="server"/> 
                                                    </div>
                                                 </div> 
                                               </td> 
                                            </tr>
                                            <tr>
                                              <td class="tdpad">
                                                <div ><i>Cheque Number</i><asp:RequiredFieldValidator  ValidationGroup="valChequePayment" ID="RequChequeNo"  runat="server" ErrorMessage="* Required" ControlToValidate="txtChequNo" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator> </div>
                                                <input type="text" id="txtChequNo" class="text-box" style="" runat="server"/>                                                   
                                              </td>
                                            </tr>
                                            <tr>
                                                <td class="tdpad">
                                                   <div><i>Cheque Amount</i> <asp:RequiredFieldValidator  ValidationGroup="valChequePayment" ID="RequChequeAmount"  runat="server" ErrorMessage="* Required" ControlToValidate="txtChequeAmount" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator><%--<asp:RegularExpressionValidator ID="RegularExChequAmount" runat="server" ControlToValidate="txtChequeAmount" Display="Dynamic" ErrorMessage="* Invalid Entry" Font-Size="8pt" ForeColor="Red" SetFocusOnError="True" ValidationExpression="^(\d|,)*\.?\d*$" ValidationGroup="valChequePayment"></asp:RegularExpressionValidator>--%></div>
                                                  <input type="text" id="txtChequeAmount" ClientIDMode="Static" class="text-box" style="" runat="server" onchange="return calaculateChequeBalance()"/>
                                               </td>
                                            </tr>
                                            <tr>
                                                <td class="tdpad" >
                                                   <div><i runat="server" id="i1">&nbsp;Balance</i></div>
					                                <div> <input type="text" id="txtChequeBalance" disabled="disabled" style="" class="text-box"/></div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="tdpad" >
                                         <asp:Panel ID="pnlChequeDepartment" runat="server" Visible="False">
                                         <div><i runat="server" id="i3">Department</i><asp:RequiredFieldValidator ValidationGroup="valChequePayment" id="reqFieldChequeDepart" ErrorMessage ="* Required" InitialValue="0" ControlToValidate="ddlChequeDepartment" Runat="server" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" /></div>
                                         <div><asp:DropDownList ID="ddlChequeDepartment" CssClass="text-box" runat="server"></asp:DropDownList> </div>
                                         </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="tdpad" >
                                        <div><i>Comment</i><asp:RequiredFieldValidator  ValidationGroup="valChequePayment" ID="RequiredFieldValidator2"  runat="server" ErrorMessage="* Required" ControlToValidate="txtChequeComment" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator></div>
                                        <div style="width: 97%; height: 107px;">
                                            <asp:TextBox runat="server" ID="txtChequeComment" TextMode="MultiLine" ClientIDMode="Static" Width="100%" Rows="6" Height="100%"></asp:TextBox>
                                        </div>
                                    </td>
                                 </tr>
                                 <tr>
                                     <td style="width: 50%">
                                     </td>
                                    <td style="width: 50%">
                                       <div style="width: 60%; margin-left: 30%; margin-top:30px;">
                                         <asp:Button ID="btnChequeSubmit" runat="server" Text="Submit" OnClick="BtnSubmitClick" ValidationGroup="valChequePayment"  CssClass="customButton" CommandArgument="1"  />&nbsp;
					                     <asp:Button ID="btnChequeClose" runat="server" Text="Close" class="customButton" OnClick="BtnChequeCloseClick" Width="79px" /> 
                                     </div>
                                </td>
                            </tr>
                         </table>
                      </fieldset>
                   </td>
			   </tr>
           </table>
        </fieldset>
    </div>	
   </div>   
   
   <script type="text/javascript">

       function calaculateChequeBalance() 
       {
           //$('#txtChequeAmount').formatCurrency({ symbol: '', roundToDecimalPlace: 0 });
           $('#txtChequeAmount').val(accounting.formatMoney($('#txtChequeAmount').val(), { symbol: "", format: "%v %s" })).replace(/ /g, '').trim();
           var newBalanceControl = $get('txtChequeBalance');
           var newPaymentValue = $get('<%=txtChequeAmount.ClientID%>').value;
           var totalAmount = $get('<%=txtChequeApprovedTotalAmount.ClientID%>').value;
           if (newPaymentValue === null || newPaymentValue == "")
           {
               newBalanceControl.value = "";
           }

           else {
               var newVal = parseFloat(totalAmount) - parseFloat(newPaymentValue.replace(',', ''));
               newBalanceControl.value = newVal;
               $('#txtChequeBalance').formatCurrency({ symbol: '', roundToDecimalPlace: 0 });
           }

           return false;
       }

       function calaculateBalance()
       {
           $('#txtAmountPaid').val(accounting.formatMoney($('#txtAmountPaid').val(), { symbol: "", format: "%v %s" }));
           $('#txtAmountPaid').val().trim();
           var totalAmountControl = document.getElementById("txtApprovedTotalAmount");
           var paymentControl = document.getElementById("txtAmountPaid");
           var balanceControl = document.getElementById("txtBalance");
           var paymentValue = paymentControl.value;
           var totalAmount = totalAmountControl.value;
           if (paymentValue === null || paymentValue == "")
           {
               balanceControl.value = "";
           }

           else {
               var newVal = parseFloat(totalAmount) - parseFloat(paymentValue.replace(',', ''));

               balanceControl.value = newVal;
               $("#txtBalance").formatCurrency({ symbol: '', roundToDecimalPlace: 0 });
           }

           return false;
       }


       $(document).ready(function () {
           readURL();

       });

       function readURL() {
           $('#fldChequeCopy').change(function (event) {
               var input = event.target.files;

               if (input.length > 0) {
                   var isValid = /\.jpg?$/i.test($get('<%=fldChequeCopy.ClientID %>').value);

                   if (!isValid) {
                       alert('Please select only a jpeg/jpg file!');
                       $get('<%=fldChequeCopy.ClientID %>').value = '';
                       return false;
                   }
                   if (input[0].size > 500000) {
                       alert("Receipt size should NOT be more than 500KB");
                       input.value = '';
                       return false;
                   }
                   else {
                       var reader = new FileReader();
                       reader.onload = function (e) {
                           $get('imgChequeCopy').src = e.target.result;

                       };

                       reader.readAsDataURL(input[0]);
                   }

               }
               else {
                   alert("Please select the Cheque copy to be attached!");
               }
               return false;
           });
       }
       
   </script>
    <!--[if IE]>
<script src="http://html5shiv.googlecode.com/svn/trunk/html5.js"></script>
<![endif]-->
</div>