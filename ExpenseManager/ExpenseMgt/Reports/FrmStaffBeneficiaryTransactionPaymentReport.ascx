<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FrmStaffBeneficiaryTransactionPaymentReport.ascx.cs" Inherits="ExpenseManager.ExpenseMgt.Reports.FrmStaffBeneficiaryTransactionPaymentReport" %>
<%@ Register TagPrefix="uc2" TagName="ErrorDisplay" Src="~/CoreFramework/ErrorControl/ErrorDisplay.ascx" %>

<div style="width: 100%">
    <h2 runat="server" id="hTitle" style="font-weight: normal; font-family: 'OCR A Extended', arial; border-bottom-color: #038103; margin-top: 1.3%"></h2>
	<div style="padding-bottom: 5px"><asp:Panel ID="Panel2" runat="server" Width="98%"><uc2:ErrorDisplay ID="ErrorDisplay1" runat="server" /></asp:Panel></div>
    <div class="single-form-display" style="width:30%; border: 0 groove transparent; border-radius: 5px; display: none" runat="server" id="dvTransactionComment">
           <fieldset style="margin-top: -2%; border-radius: 5px; border: 1px groove #038103">
                <legend runat="server" id="lgTransactionTitle" style="color: #038103; font-family: 'OCR A Extended', arial;"></legend>
               <table style="width: 100%">
                   <tr>
                       <td colspan="2" style="width: 100%">
                           <asp:TextBox ReadOnly="True" runat="server" Width="100%" TextMode="MultiLine" ID="txtHistoryComment"></asp:TextBox>
                       </td>
                   </tr>
                   <tr>
                       <td style="width: 80%">
                       
                       </td>
                       <td style="width: 20%">
                           <div style="margin-left: 10%">
                               <input type="button" runat="server" id="btnCloseComment" value="Close" class="customButton"/>
                           </div>
                       </td>
                   </tr>
               </table>
         </fieldset>
       </div>
      <div> <asp:Button ID="btnPopUp" runat="server" Style="display: none" /></div>
      <ajaxToolkit:ModalPopupExtender ID="mpePaymentCommentPopup" BackgroundCssClass="popupBackground" TargetControlID="btnPopUp" CancelControlID="btnCloseComment" PopupControlID="dvTransactionComment" RepositionMode="RepositionOnWindowResizeAndScroll" runat="server"></ajaxToolkit:ModalPopupExtender>
       <div style="width: 99%; padding: 1px;" runat="server" id="dvTransactionPayments" class="gridDiv">
           <table style="width: 100%">
               <tr>
                    <td colspan="2" style="width: 100%" class="divBackground">
                        <label style="width:auto; font-family: arial; font-weight: bolder; color: #038103; margin-left: 10px" runat="server" id="lblFilterReport">All Transaction Payments</label>
                    </td>
               </tr>
               <tr>
                  <td colspan="2">
                     <table style="width: 100%; height: 32px;">
                        <tr>
                          <td style="width: 40%">
                                <div runat="server" id="dvlnkGroup" style="margin-left: 1%; width: 489px;">
                                     <asp:LinkButton ID="lnkCompleted" runat="server" ForeColor="darkcyan" CssClass="linkStyle" BorderColor="cyan" onclick="LnkCompletedClick">Filter Completed Payments</asp:LinkButton>&nbsp;&nbsp;     
                                     <asp:LinkButton ID="lnkUncompleted" runat="server" ForeColor="purple" CssClass="linkStyle" BorderColor="purple" onclick="LnkUncompletedClick">Filter Uncompleted Payments</asp:LinkButton> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;       
                                 </div>
                              </td>
                              <td style="width: 45%" >
                                  <div style="width: 633px; margin-left: 0">
                                    <label style="color: #038103; font-family: 'OCR A Extended', arial; margin-top: -4px"> Filter by a Date Range-</label>&nbsp;
                                    <label style="color: purple; font-family: 'OCR A Extended', arial; margin-top: -4px"> From:</label>
                                    <asp:TextBox Width="18%"  runat="server" ID="txtStart" CssClass="text-box-x"></asp:TextBox>&nbsp;
                                    <label style="color: purple; font-family: 'OCR A Extended', arial; margin-top: -4px"> To:</label>
                                    <asp:TextBox Width="18%"  runat="server" ID="txtEndDate" CssClass="text-box-x"></asp:TextBox>&nbsp;
                                    <asp:Button ID="btnDateFilter" Text="Go" CssClass="customButton" OnClick="BtnDateFilterClick" runat="server" Width="28px"></asp:Button>
                                    <ajaxToolkit:calendarextender ID="CalendarExtFrom" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtStart"></ajaxToolkit:calendarextender>
                                    <ajaxToolkit:calendarextender ID="CalendarExtTo" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtEndDate"></ajaxToolkit:calendarextender>
                                </div>
                            </td>
                            <td style="width: 15%">
                                <div style="width: 65px; margin-left: -2%">
                                    <asp:Button ID="btnRefresh" runat="server" CssClass="customButton" Text="All Payments" onclick="BtnRefreshClick"></asp:Button> 
                                </div>
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
                                CssClass="xPlugTextAll_x"  DataKeyField="StaffExpenseTransactionPaymentId" ShowFooter="True" Width="100%" OnItemCommand="DgAllTransactionPaymentsCommand">
                                   <FooterStyle CssClass="gridFooter" />
                                   <AlternatingItemStyle CssClass="alternatingRowStyle"  />
                                   <ItemStyle CssClass="gridRowItem" />
                                   <HeaderStyle CssClass="gridHeader" />
                                   <Columns>
                                       <asp:TemplateColumn HeaderText="S/No.">
                                            <HeaderStyle HorizontalAlign="center" Width="1%" />
                                            <ItemStyle CssClass="xPlugTextAll_x" HorizontalAlign="center" VerticalAlign="Top" />
                                            <ItemTemplate>     
                                                <asp:Label ID="lblSNo" runat="server" CssClass="xPlugTextAll_x" Text="<%# ((dgAllTransactionPayments.PageSize*dgAllTransactionPayments.CurrentPageIndex) + Container.ItemIndex + 1)%>">
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Transaction" >
                                            <HeaderStyle HorizontalAlign="left" Width="6%" />
                                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblTransaction" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"StaffExpenseTransaction.ExpenseTitle")) %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotal" runat="server" ForeColor="green" Text="Total" Font-Bold="true" ></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Total Payable Amount" >
                                            <HeaderStyle HorizontalAlign="left" Width="6%" />
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
                                            <HeaderStyle HorizontalAlign="left" Width="6%" />
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
                                            <HeaderStyle HorizontalAlign="left" Width="6%" />
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
                                            <HeaderStyle HorizontalAlign="left" Width="6%" />
                                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top"/>
                                            <ItemTemplate>
                                                <asp:Label ID="lblPaymentDate" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"LastPaymentDate")) %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Last Payment Time" >
                                            <HeaderStyle HorizontalAlign="left" Width="6%" />
                                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblPaymentTime" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"LastPaymentTime")) %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Status" >
                                            <HeaderStyle HorizontalAlign="left" Width="2%" />
                                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server"  CssClass="xPlugTextAll_x" Text='<%#  ((DataBinder.Eval(Container.DataItem,"Status")).ToString() == "1")? "Active" : "Inactive" %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Payment Status" >
                                        <HeaderStyle HorizontalAlign="Left" Width="6%" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblTransactionStatus" runat="server"  CssClass="xPlugTextAll_x" Text='<%#  ((DataBinder.Eval(Container.DataItem,"Balance")).ToString() == "0")? "Completed" : "Uncompleted" %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Payment History" >
                                            <HeaderStyle HorizontalAlign="left" Width="6%" />
                                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lblViewHistory" runat="server" Text="View History" CommandName="ViewHistory"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                   </Columns>
                               </asp:DataGrid>
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
                               <td style="width: 80%" class="divBackground">
                                   <div style="font-family: 'OCR A Extended', arial; font-weight: bolder; color: #038103">
                                       Payment History for: &nbsp; <label runat="server" id="lblHistoryTitle"></label>
                                   </div>
                               </td>
                               <td class="divBackground" style="width: 20%">
                                   <div style="width: 120px; margin-left: 40%">
                                       <asp:Button type="button" runat="server" id="btnBackNav" class="customButton" 
                                              Text="<< Back" OnClick="BtnBackNavClick" Width="110px"></asp:Button>
                                   </div>
                               </td>
                           </tr>
                       </table>
                       
                   </td>
               </tr>
               <tr>
                   <td colspan="2" style="width: 100%">
                       <asp:DataGrid ID="dgPaymentHistory" runat="server" AutoGenerateColumns="False" CellPadding="1"   CellSpacing="1" GridLines="none" 
                        CssClass="xPlugTextAll_x"  DataKeyField="StaffExpenseTransactionPaymentHistoryId" ShowFooter="True" Width="100%" OnItemCommand="DgPaymentHistoryCommand">
                           <FooterStyle CssClass="gridFooter" />
                           <AlternatingItemStyle CssClass="alternatingRowStyle"  />
                           <ItemStyle CssClass="gridRowItem" />
                           <HeaderStyle CssClass="gridHeader" />
                           <Columns>
                               <asp:TemplateColumn HeaderText="S/No.">
                                    <HeaderStyle HorizontalAlign="center" Width="1%" />
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
                                    <HeaderStyle HorizontalAlign="left" Width="6%" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblHistoryAmountPaid" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"AmountPaid")) %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblHistoryAmountPaidFooter" runat="server" ForeColor="green" Text="" Font-Bold="true" ></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Date Paid" >
                                    <HeaderStyle HorizontalAlign="left" Width="6%" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblHistoryPaymentDate" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"PaymentDate")) %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Time Paid" >
                                    <HeaderStyle HorizontalAlign="left" Width="6%" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblHistoryPaymentTime" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"PaymentTime")) %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Paid By" >
                                    <HeaderStyle HorizontalAlign="left" Width="6%" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblHistoryPaidBy" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"PaidBy")) %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Payment Comment" >
                                    <HeaderStyle HorizontalAlign="left" Width="6%" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lblHistoryComment" runat="server" ForeColor="darkcyan"  CssClass="linkStyle" Text="View Comment" CommandName="ViewDetails">
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                           </Columns>
                       </asp:DataGrid>
                   </td>
               </tr>
           </table>
       </div>
  </div>