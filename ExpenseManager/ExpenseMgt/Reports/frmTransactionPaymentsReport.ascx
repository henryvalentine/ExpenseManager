<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="frmTransactionPaymentsReport.ascx.cs" Inherits="ExpenseManager.ExpenseMgt.Reports.FrmTransactionPaymentsReport" %>
<%@ Import Namespace="XPLUG.WEBTOOLS" %>
<%@ Register src="../../CoreFramework/AlertControl/ConfirmAlertBox.ascx" tagname="ConfirmAlertBox" tagprefix="uc1" %>
    <uc1:ConfirmAlertBox ID="ConfirmAlertBox1" runat="server" />
 <style type="text/css">
     .custWidth
     {
         width: 12%;
     }
     .custWidth3
     {
         width: 15%;
     }
     
     </style>

<div style="width: 100%">
    <h2 runat="server" id="hTitle" ></h2>
    <div id="dvAllContent" runat="server">
        <div style="width: 45%; margin-left: 25%" id="dvSearch" runat="server">
      <fieldset>
          <legend>Select Options</legend>
          <table style="width: 100%; border: none; padding: 3px">
             <tr> 
             <td colspan="5">
                 <div style="width: auto"> Status: <span style="color: #FF0000">*</span><asp:RequiredFieldValidator ID="ReqPaymentFilterOption" ValidationGroup="valPayment"  runat="server" ErrorMessage="" ControlToValidate="ddlPaymentFilterOption" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> Required </asp:RequiredFieldValidator><asp:CompareValidator ID="CompareValPaymentFilterOption" runat="server" ErrorMessage="" ValueToCompare="0" ValidationGroup="valPayment" ControlToValidate="ddlPaymentFilterOption" Operator="GreaterThanEqual" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" >Invalid Selection</asp:CompareValidator></div>
                <asp:DropDownList runat="server" ID="ddlPaymentFilterOption" ClientIDMode="Static" Width = "93%" class="text-box"  ></asp:DropDownList>
            </td>
           </tr>
             <tr>
             <td colspan="5">
                 <div style="width: auto">Department:<span style="color: #FF0000">*</span><asp:RequiredFieldValidator ID="ReqPaymentDepartment" ValidationGroup="valPayment"  runat="server" ErrorMessage="" ControlToValidate="ddlPaymentDepartment" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> Required </asp:RequiredFieldValidator><asp:CompareValidator ID="CompareValPaymentDepartment" runat="server" ErrorMessage="" ValueToCompare="1" ValidationGroup="valPayment" ControlToValidate="ddlPaymentDepartment" Operator="GreaterThanEqual" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" >Invalid Selection</asp:CompareValidator></div>
                <asp:DropDownList runat="server" ID="ddlPaymentDepartment" ClientIDMode="Static" Width = "93%" class="text-box"></asp:DropDownList> 
                
            </td> 
           </tr>
             <tr id="trPaymentPeriod" style="display: none">
                <td colspan="5" >
                    <div style="width: auto">Period:<span style="color: #FF0000">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="valPayment"  runat="server" ErrorMessage="" ControlToValidate="ddlPaymentPeriod" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> </asp:RequiredFieldValidator><asp:CompareValidator ID="CompareValPaymentPeriod" runat="server" ErrorMessage="" ValueToCompare="1" ValidationGroup="valPayment" ControlToValidate="ddlPaymentPeriod" Operator="GreaterThanEqual" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" >Invalid Selection</asp:CompareValidator></div>
                    <asp:DropDownList ID="ddlPaymentPeriod" runat="server" CssClass="text-box" Width="93%"></asp:DropDownList>                     
                </td>              
            </tr>
             <tr id="trPaymentYear" style="display: none">
                <td colspan="5">
                    <div style="width: auto">Year:<span style="color: #FF0000">*</span> <asp:RequiredFieldValidator ID="ReqPaymentYear" ValidationGroup="valPayment"  runat="server" ErrorMessage="" ControlToValidate="ddlPaymentYear" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> </asp:RequiredFieldValidator> <asp:CompareValidator ID="CompareValPaymentYear" runat="server" ErrorMessage="" ValueToCompare="1" ValidationGroup="valPayment" ControlToValidate="ddlPaymentYear" Operator="GreaterThanEqual" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" >Required</asp:CompareValidator></div>
                    <asp:DropDownList ID="ddlPaymentYear" runat="server" CssClass="text-box" Width="93%"></asp:DropDownList> 
                </td> 
            </tr>
             <tr id="trPaymentMonth" style="display: none">
                <td style="text-align: left" colspan="5">
                    <div style="width: auto"> Month:<span style="color: #FF0000">*</span> <asp:RequiredFieldValidator ID="ReqMonth" ValidationGroup="valPayment"  runat="server" ErrorMessage="" ControlToValidate="ddlMonth" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> </asp:RequiredFieldValidator><asp:CompareValidator ID="CompareValMonth" runat="server" ErrorMessage="" ValueToCompare="1" ValidationGroup="valPayment" ControlToValidate="ddlMonth" Operator="GreaterThanEqual" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" >Invalid Selection</asp:CompareValidator></div>
                    <asp:DropDownList ID="ddlMonth" runat="server" CssClass="text-box" Width="93%" ></asp:DropDownList>                     
                </td>
            </tr>
             <tr id="trPaymentWeek" style="display: none">
                <td colspan="5">
                    <div style="width: auto">Week:<span style="color: #FF0000">*</span><asp:RequiredFieldValidator ID="ReqPaymentWeek" ValidationGroup="valPayment"  runat="server" ErrorMessage="" ControlToValidate="ddlPaymentWeekly" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator><asp:CompareValidator ID="CompareValPaymentWeekly" runat="server" ErrorMessage="" ValueToCompare="1" ValidationGroup="valPayment" ControlToValidate="ddlPaymentWeekly" Operator="GreaterThanEqual" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" >Invalid Selection</asp:CompareValidator></div>
                     <asp:DropDownList ID="ddlPaymentWeekly" runat="server" CssClass="text-box" Width = "93%" ></asp:DropDownList>
                </td>              
            </tr>
             <tr id="trPaymentDateRange" style="display: none"> 
                <td style="width: 40%">
                    <div style="width: auto"> From:<span style="color: #FF0000">*</span><asp:RequiredFieldValidator ID="ReqPaymentStartDate" ValidationGroup="valPayment"  runat="server" ErrorMessage="" ControlToValidate="txtPaymentStart" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> Required </asp:RequiredFieldValidator></div>
                    <asp:TextBox  runat="server" ID="txtPaymentStart" Width="93%" CssClass="text-box" ></asp:TextBox>
                </td>
                <td style="width: 40%">
                    <div style="width: auto"> To:<span style="color: #FF0000">*</span><asp:RequiredFieldValidator ID="ReqEndDate" ValidationGroup="valPayment"  runat="server" ErrorMessage="" ControlToValidate="txtPaymentEndDate" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> Required </asp:RequiredFieldValidator></div>  
                    <asp:TextBox   runat="server" ID="txtPaymentEndDate" CssClass="text-box" Width="82%"></asp:TextBox> 
                    <ajaxToolkit:calendarextender ID="CalendarExtPaymentFrom" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtPaymentStart"></ajaxToolkit:calendarextender>
                    <ajaxToolkit:calendarextender ID="CalendarExtPaymentTo" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtPaymentEndDate"></ajaxToolkit:calendarextender>  
                </td>
            </tr>
             <tr>
              <td colspan="5"><br/>
                  <div style="width: 21%; margin-left: 74%">
                     <asp:Button ID="btnGenerateReport"  runat="server" ValidationGroup="valPayment" Text="Generate Report" OnClick="BtnGeneratePaymentReportchClick" CssClass="customButton" Width="118px" />
                  </div>
              </td>
            </tr>
       </table>
     </fieldset>
   </div>
    <table style="width: 100%">
       <tr style="border: none">
          <td style="width: 10%;vertical-align: bottom">
               Department:&nbsp;  
          </td>
          <td style="width: 90%;vertical-align: bottom">
              <h2 runat="server" id="hDepartment" style="color: green"></h2> 
          </td>
      </tr>
    </table>
    <div style="width: 99%; padding: 1px;" runat="server" id="dvTransactionPayments" class="gridDiv">
       <table style="width: 100%">
            <tr>
               <td colspan="2" style="width: 100%">
                   <table style="width: 100%">
                       <tr>
                          <td colspan="2" style="width: 100%"> 
                              <asp:DataGrid ID="dgAllTransactionPayments" runat="server" AutoGenerateColumns="False" CellPadding="1"   CellSpacing="1" GridLines="none" 
                                CssClass="xPlugTextAll_x"  DataKeyField="ExpenseTransactionPaymentId" ShowFooter="True" Width="100%" OnItemCommand="DgAllTransactionPaymentsCommand">
                                   <FooterStyle CssClass="gridFooter" />
                                   <AlternatingItemStyle CssClass="alternatingRowStyle"  />
                                   <ItemStyle CssClass="gridRowItem" />
                                   <HeaderStyle CssClass="gridHeader" />
                                   <Columns>
                                       <asp:TemplateColumn HeaderText="S/No.">
                                            <HeaderStyle HorizontalAlign="center" Width="1%" CssClass="tdpadtop" />
                                            <ItemStyle CssClass="xPlugTextAll_x" HorizontalAlign="center" VerticalAlign="Top" />
                                            <ItemTemplate>     
                                                <asp:Label ID="lblSNo" runat="server" CssClass="xPlugTextAll_x" Text="<%# ((dgAllTransactionPayments.PageSize*dgAllTransactionPayments.CurrentPageIndex) + Container.ItemIndex + 1)%>">
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Transaction" >
                                            <HeaderStyle HorizontalAlign="left" Width="6%" CssClass="tdpadtop"  />
                                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblTransaction" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"ExpenseTransaction.ExpenseTitle")) %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotal" runat="server" ForeColor="green" Text="Total" Font-Bold="true" ></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Total Payable Amount" >
                                            <HeaderStyle HorizontalAlign="left" Width="6%" CssClass="tdpadtop" />
                                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalAmountPaid" runat="server"  CssClass="xPlugTextAll_x" Text='<%# NumberMap.GroupToDigits(Eval("TotalAmountPayable").ToString())%>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalAmountPaidFooter" runat="server" ForeColor="green" Font-Bold="False" Wrap="false" Text=""></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Amount Paid" >
                                            <HeaderStyle HorizontalAlign="left" Width="6%" CssClass="tdpadtop" />
                                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmountPaid" runat="server"  CssClass="xPlugTextAll_x" Text='<%# NumberMap.GroupToDigits(Eval("AmountPaid").ToString()) %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblAmountPaidFooter" runat="server" Font-Bold="False" Wrap="false" Text=""></asp:Label>
                                            </FooterTemplate> 
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Balance" >
                                            <HeaderStyle HorizontalAlign="left" Width="6%" CssClass="tdpadtop" />
                                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalance" runat="server"  CssClass="xPlugTextAll_x" Text='<%#NumberMap.GroupToDigits(Eval("Balance").ToString())%>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                    <asp:Label ID="lblBalanceFooter" runat="server" Font-Bold="False" Wrap="false" Text=""></asp:Label>
                                                </FooterTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Last Payment Date">
                                            <HeaderStyle HorizontalAlign="left" Width="6%" CssClass="tdpadtop" />
                                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top"/>
                                            <ItemTemplate>
                                                <asp:Label ID="lblPaymentDate" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"LastPaymentDate")) %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Last Payment Time" >
                                            <HeaderStyle HorizontalAlign="left" Width="6%" CssClass="tdpadtop" />
                                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblPaymentTime" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"LastPaymentTime")) %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Payment Status" >
                                        <HeaderStyle HorizontalAlign="Left" Width="6%" CssClass="tdpadtop"/>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblTransactionStatus" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"PaymentStatus")) %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Payment History" >
                                            <HeaderStyle HorizontalAlign="left" Width="6%" CssClass="tdpadtop" />
                                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lblViewHistory" runat="server" Text="View History" CommandName="ViewHistory"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                   </Columns>
                               </asp:DataGrid>
                              <table style="width: 100%" class="gridFoot">
                                  <tr>
                                     <td style="width: 35%">
		                               <span style="float: left; color: rgb(105, 105, 105); display: none">Total Records: <%= DataCount %></span>
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
                   </td>
               </tr>
           </table>
       </div>
    </div>
     <div id="dvView" runat="server">
          <div style="width: 100%;" runat="server" id="dvTransactionPaymentHistory" class="gridDiv">
           <table style="width: 100%">
               <tr>
                   <td colspan="2" style="width: 100%">
                       <table style="width: 100%">
                           <tr>
                               <td style="width: 80%" class="divBackground tdpadd" >
                                   <div style="font-weight: bolder; color: #038103">
                                       Payment History for: &nbsp; <label runat="server" id="lblHistoryTitle"></label>
                                   </div>
                               </td>
                               <td class="divBackground" style="width: 20%">
                                   <div style="width: 120px; margin-left: 40%">
                                       <asp:Button type="button" runat="server" id="btnBackNav" class="customButton" Text="<< Back" OnClick="BtnBackNavClick" Width="110px"></asp:Button>
                                   </div>
                               </td>
                           </tr>
                       </table>
                   </td>
               </tr>
               <tr>
                   <td colspan="2" style="width: 100%">
                       <asp:DataGrid ID="dgPaymentHistory" runat="server" AutoGenerateColumns="False" CellPadding="1"   CellSpacing="1" GridLines="none" 
                        CssClass="xPlugTextAll_x"  DataKeyField="ExpenseTransactionPaymentHistoryId" ShowFooter="True" Width="100%" OnItemCommand="DgPaymentHistoryCommand">
                           <FooterStyle CssClass="gridFooter" />
                           <AlternatingItemStyle CssClass="alternatingRowStyle"/>
                           <ItemStyle CssClass="gridRowItem" />
                           <HeaderStyle CssClass="gridHeader" />
                           <Columns>
                               <asp:TemplateColumn HeaderText="S/No.">
                                    <HeaderStyle HorizontalAlign="center" Width="1%" CssClass="tdpadtop" />
                                    <ItemStyle CssClass="xPlugTextAll_x" HorizontalAlign="center" VerticalAlign="Top" />
                                    <ItemTemplate>     
                                        <asp:Label ID="lblSNo" runat="server" CssClass="xPlugTextAll_x" Text="<%# ((dgPaymentHistory.PageSize*dgPaymentHistory.CurrentPageIndex) + Container.ItemIndex + 1)%>">
                                        </asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotal" runat="server" ForeColor="green" Text="Total" Font-Bold="true" ></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Amount Paid" >
                                    <HeaderStyle HorizontalAlign="left" Width="6%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblHistoryAmountPaid" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"AmountPaid")) %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblHistoryAmountPaidFooter" runat="server" ForeColor="green" Text="" Font-Bold="true" ></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Payment Mode" >
                                    <HeaderStyle HorizontalAlign="left" Width="6%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblPaymentMode" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"PaymentMode.Name")) %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Date Paid" >
                                    <HeaderStyle HorizontalAlign="left" Width="6%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblHistoryPaymentDate" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"PaymentDate")) %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Time Paid" >
                                    <HeaderStyle HorizontalAlign="left" Width="6%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblHistoryPaymentTime" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"PaymentTime")) %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Paid By" >
                                    <HeaderStyle HorizontalAlign="left" Width="6%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblHistoryPaidBy" runat="server"  CssClass="xPlugTextAll_x" Text='<%#  GetUserFullName(int.Parse((DataBinder.Eval(Container.DataItem,"PaidById")).ToString()))%>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Payment Details" >
                                    <HeaderStyle HorizontalAlign="left" Width="6%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkPaymentDetails" runat="server" ForeColor="darkcyan"  CssClass="linkStyle" Text="View Details" CommandName="ViewDetails">
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                           </Columns>
                       </asp:DataGrid>
                   </td>
               </tr>
           </table>
       </div>
       <div class="single-form-display" style="width:40%; border: 0 groove transparent; border-radius: 5px; vertical-align: top; display: none" id="dvTransactionComment">
              <fieldset>
			    <legend runat="server" id="lgTransactionTitle" style="">Payment Information</legend>
                <table style="border-style: none; border-color: inherit; border-width: medium; width:90%; padding: 3px; height: 217px;">
                 <tr runat="server" id="trChequePayment">
                     <td>
                         <table style="width:100%">
                             <tr>
                                <td style="width:50%">
						            <div><i>Paid By</i></div> 
                                    <div><b><label id="lblChequePaidBy" runat="server" style="width: 98%" ></label> </b> </div>
                                </td>
                               <td style="width:50%">
					             <div><i>Bank</i></div>
                                 <div><b><label id="lblBank" runat="server" style="width: 100%" ></label> </b></div>
                               </td>
                            </tr>
                             <tr>
                                <td style="width:50%">
					                <div><i >Cheque Number</i> </div>
                                    <div><b><label id="lblChequeNumber" runat="server" style="width: 98%"></label></b> </div>
				                </td>
                                <td style="width:50%">
					               <div><i>Cheque Amount</i> </div>
                                   <div><b><label id="lblChequeAmount" runat="server" style="width: 100%"></label> </b> </div></td>
                            </tr>
                            <tr>
                               <td style="width:50%">
					              <div><i>Date Paid</i></div>
                                  <div><b><label id="lblChequeDatePaid" runat="server" style="width: 98%"></label> </b> </div>
				               </td>  
                               <td style="width:50%">
					              <div><i>Time Paid</i></div>
                                  <div><b><label id="lblChequeTimePaid" runat="server" style="width: 100%"></label> </b> </div>
				               </td>  
                            </tr>
                         </table>
                     </td>
                 </tr>
                 <tr  runat="server" id="trCashPayment">
                   <td>
                         <table style="width:100%">
                             <tr>
                                <td style="width:50%">
						            <div><i>Paid By</i></div> 
                                    <div><b><label id="lblCashPaidBy" runat="server" style="width: 98%" ></label> </b> </div>
                                </td>                              
                                <td style="width:50%">
					               <div><i>Cash Amount</i> </div>
                                   <div><b><label id="lblCashAmount" runat="server" style="width: 100%"></label> </b> </div>
                               </td>
                            </tr>
                            <tr>
                               <td style="width:50%">
					              <div><i>Date Paid</i></div>
                                  <div><b><label id="lblCashDatePaid" runat="server" style="width: 98%"></label> </b> </div>
				               </td>  
                               <td style="width:50%">
					              <div><i>Time Paid</i></div>
                                  <div><b><label id="lblCashTimePaid" runat="server" style="width: 100%"></label> </b> </div>
				               </td>  
                            </tr>
                         </table>
                     </td>  
                 </tr>
                 <tr>
                   <td colspan="2">
					  <div><i>Payer's Comment</i></div>
                      <div>
					    <textarea rows="4" cols="70" runat="server" id="txttHistoryComment" readonly="readonly">
                            
                        </textarea>
					  </div>
				   </td>  
                </tr>
                <tr>
                <td colspan="2">
                    <div style="width: 8%; margin-left: 45%">
                        <input id="btnCloseDetails" class="customButton" value="Close" style="width: 100%"/>
                    </div> 
                </td>
             </tr>
          </table>
       </fieldset>
     </div> 
     </div>
      
     <div> <asp:Button ID="btnPopUp" runat="server" Style="display: none" /></div>
      <ajaxToolkit:ModalPopupExtender ID="mpePaymentCommentPopup" BackgroundCssClass="popupBackground" TargetControlID="btnPopUp" CancelControlID="btnCloseDetails" PopupControlID="dvTransactionComment" RepositionMode="RepositionOnWindowResizeAndScroll" runat="server"></ajaxToolkit:ModalPopupExtender>
  </div>
  <script type="text/javascript">

      $(window).load(function ()
      {
          PeriodChanged();
      });

      $(document).ready(function () 
      {
          $('#<%=ddlPaymentDepartment.ClientID%>').on("change", function ()
          {
              ClearControls();
              PeriodChanged();
          });

          $('#<%=ddlPaymentPeriod.ClientID%>').on("change", function ()
          {
              ClearControls2();
              PeriodChanged();
          });
      });

      function PeriodChanged() 
      {
          var deptSel = parseInt($('#<%=ddlPaymentDepartment.ClientID%>').val());

          if (deptSel < 1) {
              HideTrs2();
          }

          if (deptSel > 0) 
          {
              $("#<%=ReqPaymentFilterOption.ClientID %>,#<%=CompareValPaymentFilterOption.ClientID %>").each(function () { this.enabled = true; });
              $('#trPaymentPeriod').css({ "display": "table-row" });
          }

          var timeFrame = parseInt($('#<%=ddlPaymentPeriod.ClientID%>').val());

          if (timeFrame < 1) 
          {
              HideTrs();
          }

          if (timeFrame == 1) {
              $('#trPaymentYear').css({ "display": "table-row" });
              $('#trPaymentMonth').css({ "display": "table-row" });
              var validatorCollection1 = $("#<%=ReqPaymentYear.ClientID %>, #<%=CompareValPaymentYear.ClientID %>, #<%=ReqMonth.ClientID %>, #<%=CompareValMonth.ClientID %>");
              $.each(validatorCollection1, function () {
                  this.enabled = true;
              });

              var activeVal1 = $("#<%=ReqPaymentStartDate.ClientID %>,#<%=ReqEndDate.ClientID %>, #<%=ReqPaymentWeek.ClientID %>,#<%=CompareValPaymentWeekly.ClientID %>");
              $.each(activeVal1, function () {
                  this.enabled = false;
              });

              $('#trPaymentWeek').css({ "display": "none" });
              $('#trPaymentDateRange').css({ "display": "none" });
          }

          if (timeFrame == 2)
          {
              $('#trPaymentYear').css({ "display": "table-row" });
              $('#trPaymentMonth').css({ "display": "table-row" });
              $('#trPaymentWeek').css({ "display": "table-row" });
              var validatorCollection2 = $("#<%=ReqPaymentYear.ClientID %>, #<%=CompareValPaymentYear.ClientID %>, #<%=ReqMonth.ClientID %>, #<%=CompareValMonth.ClientID %>, #<%=ReqPaymentWeek.ClientID %>,#<%=CompareValPaymentWeekly.ClientID %>");
              $.each(validatorCollection2, function () {
                  this.enabled = true;
              });

              var activeVal2 = $("#<%=ReqPaymentStartDate.ClientID %>,#<%=ReqEndDate.ClientID %>");
              $.each(activeVal2, function () {
                  this.enabled = false;
              });

              $('#trPaymentDateRange').css({ "display": "none" });
          }

          if (timeFrame == 3) {
              $('#trPaymentMonth').css({ "display": "none" });
              $('#trPaymentWeek').css({ "display": "none" });
              $('#trPaymentYear').css({ "display": "none" });
              $('#trPaymentDateRange').css({ "display": "table-row" });
              var validatorCollection3 = $("#<%=ReqPaymentYear.ClientID %>, #<%=CompareValPaymentYear.ClientID %>, #<%=ReqMonth.ClientID %>, #<%=CompareValMonth.ClientID %>, #<%=ReqPaymentWeek.ClientID %>,#<%=CompareValPaymentWeekly.ClientID %>");
              $.each(validatorCollection3, function () {
                  this.enabled = false;
              });

              var activeVal = $("#<%=ReqPaymentStartDate.ClientID %>,#<%=ReqEndDate.ClientID %>");
              $.each(activeVal, function () {
                  this.enabled = true;
              });
          }
      }

      function HideTrs() {
          $('#trPaymentYear').css({ "display": "none" });
          $('#trPaymentMonth').css({ "display": "none" });
          $('#trPaymentWeek').css({ "display": "none" });
          $('#trPaymentDateRange').css({ "display": "none" });
          DisableValidators();
      }

      function HideTrs2() {

          $('#trPeriod').css({ "display": "none" });
          $('#trPaymentYear').css({ "display": "none" });
          $('#trPaymentMonth').css({ "display": "none" });
          $('#trPaymentWeek').css({ "display": "none" });
          $('#trPaymentDateRange').css({ "display": "none" });
          DisableValidators();
      }

      function DisableValidators() {
          var validatorCollection = $("#<%=ReqPaymentYear.ClientID %>, #<%=CompareValPaymentYear.ClientID %>, #<%=ReqMonth.ClientID %>, #<%=CompareValMonth.ClientID %>, #<%=ReqPaymentStartDate.ClientID %>,#<%=ReqEndDate.ClientID %>, #<%=ReqPaymentWeek.ClientID %>,#<%=CompareValPaymentWeekly.ClientID %>");
          $.each(validatorCollection, function () {
              this.enabled = false;
          });
      }

      function ClearControls() {
          $('#<%=ddlMonth.ClientID %>').val(0);
          $('#<%=ddlPaymentPeriod.ClientID %>').val(0);
          $('#<%=ddlPaymentWeekly.ClientID %>').val(0);
          $('#<%=ddlPaymentYear.ClientID %>').val(0);
          $('#<%=txtPaymentStart.ClientID %>').val('');
          $('#<%=txtPaymentEndDate.ClientID %>').val('');

      }

      function ClearControls2() {
          $('#<%=ddlMonth.ClientID %>').val(0);
          $('#<%=ddlPaymentWeekly.ClientID %>').val(0);
          $('#<%=ddlPaymentYear.ClientID %>').val(0);
          $('#<%=txtPaymentStart.ClientID %>').val('');
          $('#<%=txtPaymentEndDate.ClientID %>').val('');

      }
  </script>