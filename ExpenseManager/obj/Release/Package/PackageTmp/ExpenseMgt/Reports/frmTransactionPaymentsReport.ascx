<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="frmTransactionPaymentsReport.ascx.cs" Inherits="ExpenseManager.ExpenseMgt.Reports.FrmTransactionPaymentsReport" %>
<%@ Register src="../../CoreFramework/AlertControl/ConfirmAlertBox.ascx" tagname="ConfirmAlertBox" tagprefix="uc1" %>
    <uc1:ConfirmAlertBox ID="ConfirmAlertBox1" runat="server" />
<div style="width: 100%">
    <h2 runat="server" id="hTitle" ></h2>
       <div style="width: 99%; padding: 1px;" runat="server" id="dvTransactionPayments" class="gridDiv">
           <table style="width: 100%">
               <tr class="divBackground">
                    <td colspan="2" style="width: 100%" class="tdpadd2">
                        <label class="label" runat="server" id="lblFilterReport">All Transaction Payments</label>
                    </td>
               </tr>
               <tr>
                  <td colspan="2">
                     <table style="width: 100%; height: 32px;">
                         <tr>
                            <td colspan="7" class="tdpadd">
                                <label style="width: 100%">Note on <b>FILTERING</b>: &nbsp;Select a <b>Status</b> and then narrow it down by <b>Date Range</b></label></td>
                          </tr>
                          <tr class="divBackground3">
                            <td class="tdpad" style="width: 10%">
                               <div style="width: 100%">
                                  <label style="margin-left: 70%">Status</label>
                                </div>
                             </td>
                             <td class="tdpad" style="width: 12%">
                                <asp:DropDownList runat="server" ID="ddlFilterOption" Width="60%" CssClass="text-box"></asp:DropDownList>    
                              </td>
                              <td style="width: 3%; text-align: right" class="tdpad">
                                <label> From:</label> 
                              </td>
                               <td style="width: 20%" class="tdpad" >
                                 <asp:TextBox   runat="server" ID="txtStart" CssClass="text-box"></asp:TextBox>    
                               </td>
                                <td style="width: 3%; text-align: right" class="tdpad" >
                                  <label> To:</label>   
                                </td>
                               <td style="width: 20%" class="tdpad" >
                                     <asp:TextBox   runat="server" ID="txtEndDate" CssClass="text-box"></asp:TextBox>
                              </td>
                             <td style="width: 20%" class="tdpad" >
                                <asp:Button ID="btnDateFilter" Text="Go" CssClass="customButton" OnClick="BtnDateFilterClick" runat="server" ></asp:Button>
                                <ajaxToolkit:calendarextender ID="CalendarExtFrom" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtStart"></ajaxToolkit:calendarextender>
                                <ajaxToolkit:calendarextender ID="CalendarExtTo" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtEndDate"></ajaxToolkit:calendarextender>
                           </td>
                       </tr>
                   </table>
                 </td>
              </tr>
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
                                                <asp:Label ID="lblTotalAmountPaid" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"TotalAmountPayable")) %>'>
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
                                                <asp:Label ID="lblAmountPaid" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"AmountPaid")) %>'>
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
                                                <asp:Label ID="lblBalance" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Balance")) %>'>
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
                                        <%--<asp:TemplateColumn HeaderText="Status" >
                                            <HeaderStyle HorizontalAlign="left" Width="2%" CssClass="tdpadtop" />
                                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server"  CssClass="xPlugTextAll_x" Text='<%#  ((DataBinder.Eval(Container.DataItem,"Status")).ToString() == "1")? "Active" : "Inactive" %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>--%>
                                        <asp:TemplateColumn HeaderText="Payment Status" >
                                        <HeaderStyle HorizontalAlign="Left" Width="6%" CssClass="tdpadtop"/>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblTransactionStatus" runat="server"  CssClass="xPlugTextAll_x" Text='<%#  ((DataBinder.Eval(Container.DataItem,"Balance")).ToString() == "0")? "Fully Paid" : "Partly Paid" %>' ></asp:Label>
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
                   </td>
               </tr>
           </table>
       </div>
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
					    <textarea rows="4" cols="70" runat="server" id="txttHistoryComment">
                            
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
     <div> <asp:Button ID="btnPopUp" runat="server" Style="display: none" /></div>
      <ajaxToolkit:ModalPopupExtender ID="mpePaymentCommentPopup" BackgroundCssClass="popupBackground" TargetControlID="btnPopUp" CancelControlID="btnCloseDetails" PopupControlID="dvTransactionComment" RepositionMode="RepositionOnWindowResizeAndScroll" runat="server"></ajaxToolkit:ModalPopupExtender>
  </div>
  