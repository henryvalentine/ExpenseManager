<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FrmMulttipleVoucher.ascx.cs" Inherits="ExpenseManager.ExpenseMgt.Reports.FrmMulttipleVoucher" %>
<%@ Import Namespace="XPLUG.WEBTOOLS" %>

<%@ Register src="../../CoreFramework/AlertControl/ConfirmAlertBox.ascx" tagname="ConfirmAlertBox" tagprefix="uc1" %>

<link href="App_Themes/Default/ControlStyles.css" rel="stylesheet" type="text/css" />
    
   <div class="dvContainer">
        <uc1:ConfirmAlertBox ID="ConfirmAlertBox1" runat="server" />
          <h2>Print Multiple Transaction Payment Vouchers</h2>
      <div style="width: 100%"   id="dvMultiVouchers"> 
      <table style="width: 100%">
         <tr> 
             <td colspan="3"></td>
         </tr>
          <tr>
              <td colspan="5" class="tdpadd">
                  <label style="width: 100%">
                     Note on <b>FILTERING</b>: &nbsp;Select a <b>Status</b> for a <b>Department</b> and then narrow it down by <b>Date Range</b>
                  </label>
              </td>
          </tr>
         <tr class="divBackground3">
             
            <td class="tdpad" style="width: 4%">
               <label class="label3">Status</label>
            </td> 

             <td class="tdpad" style="width: 15%">
                <asp:DropDownList runat="server" ID="ddlMultiVoucherFilterOption" 
                     ClientIDMode="Static" Width = "93%" class="text-box" ></asp:DropDownList>
            </td>
            <td class="tdpad" style="width: 4%">
               <label class="label3">Department</label>
            </td> 
             <td class="tdpad" style="width: 15%">
                <asp:DropDownList runat="server" ID="ddlDepartmentVoucher" ClientIDMode="Static" Width = "93%" class="text-box"></asp:DropDownList> 
            </td>
            <td style="width: 50%" class="tdpad">
               <label> From:</label>
               <asp:TextBox Width="22%"  runat="server" ClientIDMode="Static" id="txtMultiVoucherStartDate" CssClass="text-box-x" ></asp:TextBox>&nbsp;
               <label> To:</label>
               <asp:TextBox Width="22%"  runat="server" ID="txtMultiVoucherEndDate" CssClass="text-box-x" ></asp:TextBox>&nbsp;
               <asp:Button id="btnMultiVoucherDateFilter" Text="Go" class="customButton" Width ="72px" runat="server" onclick="BtnMultiVoucherDateFilterClick" />
                &nbsp;&nbsp;&nbsp;<ajaxToolkit:calendarextender ID="MultiCalendarExtFrom" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtMultiVoucherStartDate"></ajaxToolkit:calendarextender>
                <ajaxToolkit:calendarextender ID="MultiCalendarExtTo" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtMultiVoucherEndDate"></ajaxToolkit:calendarextender>
             </td>

             <td style="width: 30%">
                   <div style="margin-left: 1%; width: 94%" id="MultiVoucherRadioDiv">
					  <%--<label ><b>Single Voucher</b></label> <input type="radio" id="radSingle" value="Single" name="printOption"/>
                      <label style="margin-left: 3%"><b>Multiple Vouchers</b></label><input type="radio" id="radMultiple" value="Multiple" name="printOption" />--%>
                      <%--<a href="#" id="ancMultiple" title="Print Multiple Vouchers">Print Multiple Vouchers >> </a>--%>
				  </div>
             </td>
            <%--<td style="width: 10%" class="tdpad">
               <input type="button" id="btnPrintAllVouchers" style=" width: 118px; margin-left: 10px" class="customButton" value="Print All"/>
           </td>--%>
        </tr>
      </table>
	   <%--<div style="width: 30%; margin-top: 5%; margin-left: 25%" id="dvMultiVouchersInfo">
	      <select id="ddlMultiTransactions" class="text-box" style="width: 100%; visibility: visible; display: none;"> </select> 
	   </div>--%>
        <div id="MultiDiv" style=" width: 100%" class="gridDiv">
          <asp:DataGrid ID="dgVouchers" runat="server" AutoGenerateColumns="False" CellPadding="1"   CellSpacing="1" GridLines="none" CssClass="xPlugTextAll_x"  DataKeyField="ExpenseTransactionPaymentHistoryId" ShowFooter="True" Width="100%" >
                <FooterStyle CssClass="gridFooter" />
                <AlternatingItemStyle CssClass="alternatingRowStyle"  />
                <ItemStyle CssClass="gridRowItem" />
                <HeaderStyle CssClass="gridHeader" />
                <Columns>
                    <asp:TemplateColumn HeaderText="S/No.">
                        <HeaderStyle HorizontalAlign="center" Width="1%" CssClass="tdpadtop" />
                        <ItemStyle CssClass="xPlugTextAll_x" HorizontalAlign="center" VerticalAlign="Top" />
                        <ItemTemplate>     
                            <asp:Label ID="lblSNo" runat="server" CssClass="xPlugTextAll_x" Text="<%# ((dgVouchers.PageSize*dgVouchers.CurrentPageIndex) + Container.ItemIndex + 1)%>">
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Transaction" >
                        <HeaderStyle HorizontalAlign="left" Width="13%" CssClass="tdpadtop"  />
                        <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:Label ID="lblTransaction" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"ExpenseTransaction.ExpenseTitle")) %>'>
                            </asp:Label>
                        </ItemTemplate>
                        <%--<FooterTemplate>
                            <asp:Label ID="lblTotal" runat="server" ForeColor="green" Text="Total" Font-Bold="true" ></asp:Label>
                        </FooterTemplate>--%>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Total Approved Amount(N)" >
                        <HeaderStyle HorizontalAlign="left" Width="7%" CssClass="tdpadtop" />
                        <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:Label ID="lblTotalAmountPaid" runat="server"  CssClass="xPlugTextAll_x" Text='<%#NumberMap.GroupToDigits((DataBinder.Eval(Container.DataItem,"ExpenseTransaction.TotalApprovedAmount")).ToString()) %>'>
                            </asp:Label>
                        </ItemTemplate>
                        <%--<FooterTemplate>
                            <asp:Label ID="lblTotalAmountPaidFooter" runat="server" ForeColor="green" Font-Bold="False" Wrap="false" Text=""></asp:Label>
                        </FooterTemplate>--%>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Amount Paid(N)" >
                        <HeaderStyle HorizontalAlign="left" Width="5%" CssClass="tdpadtop" />
                        <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:Label ID="lblAmountPaid" runat="server"  CssClass="xPlugTextAll_x" Text='<%# NumberMap.GroupToDigits((DataBinder.Eval(Container.DataItem,"AmountPaid")).ToString()) %>'>
                            </asp:Label>
                        </ItemTemplate>
                       <%-- <FooterTemplate>
                            <asp:Label ID="lblAmountPaidFooter" runat="server" Font-Bold="False" Wrap="false" Text=""></asp:Label>
                        </FooterTemplate>--%>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Balance(N)" >
                        <HeaderStyle HorizontalAlign="left" Width="4%" CssClass="tdpadtop" />
                        <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:Label ID="lblBalance" runat="server"  CssClass="xPlugTextAll_x" Text='<%# NumberMap.GroupToDigits((DataBinder.Eval(Container.DataItem,"ExpenseTransactionPayment.Balance")).ToString()) %>'>
                            </asp:Label>
                        </ItemTemplate>
                        <%--<FooterTemplate>
                                <asp:Label ID="lblBalanceFooter" runat="server" Font-Bold="False" Wrap="false" Text=""></asp:Label>
                            </FooterTemplate>--%>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Received By" >
                        <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop" />
                        <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:Label ID="lblPaymentTime" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Receiver")) %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Date Paid">
                        <HeaderStyle HorizontalAlign="left" Width="4%" CssClass="tdpadtop" />
                        <ItemStyle HorizontalAlign="left" VerticalAlign="Top"/>
                        <ItemTemplate>
                            <asp:Label ID="lblPaymentDate" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"PaymentDate")) %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Time Paid">
                        <HeaderStyle HorizontalAlign="left" Width="4%" CssClass="tdpadtop" />
                        <ItemStyle HorizontalAlign="left" VerticalAlign="Top"/>
                        <ItemTemplate>
                            <asp:Label ID="lblPaymentDate" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"PaymentTime")) %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn>
                        <HeaderTemplate><input type="checkbox" runat="server" id="slctAllprintOption" onclick="CheckAllprintOptionIdChanged(this.id);" ClientIDMode="Static" style="margin-left: -6%" />Select All</HeaderTemplate>
                        <HeaderStyle HorizontalAlign="Left" Width="4%" CssClass="tdpadtop" />
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
                        <ItemTemplate>
                            <input type="checkbox" onclick="CheckPrintChanged(this.id)" name="printOptionSlct" style="margin-left: 1%"  id="chkPrintPreview<%#(DataBinder.Eval(Container.DataItem,"ExpenseTransactionPaymentHistoryId")) %>"/>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <%--<asp:TemplateColumn HeaderText="Payment History" >
                        <HeaderStyle HorizontalAlign="left" Width="6%" CssClass="tdpadtop" />
                        <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:LinkButton ID="lblViewHistory" runat="server" Text="View History" CommandName="ViewHistory"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateColumn>--%>
                </Columns>
            </asp:DataGrid>
         </div>
         <table style="width: 100%;">
             <tr>
                 <td style="width: 70%">
                 </td>
                 <td style="width: 30%">
                     <input class="customButton" type="button" value="Preview Selection" style="width: 130px; margin-left: 50%; margin-top: 3%; display: none" onclick="SendOptionIds()" id="btnPrintSelection"/>
                 </td>
             </tr>
         </table>
      </div>
      
       <div id="dvPreview" style="visibility: visible; margin-top: 1%; width: 100%;">
          <table style="width: 100%">
             <tr>
                 <td style="width: 80%">
                     
                 </td>
                 <td style="width: 20%">
                   <a  style="width: 115px; margin-left: 40%; display: none; color: green; font-weight: bold; cursor: pointer; font-size: 13pt" onclick="HideDivs()" id="btnBack"><< Back</a>
                 </td>
             </tr>
           </table>
         <div id="dvPreviewMultiple" style="width: 100%; margin-left: 13%"></div>
        </div>
      <div id="dvItemsView" style="margin-top: 1%; width: 100%; visibility: visible;"></div>
   </div>
   
  

