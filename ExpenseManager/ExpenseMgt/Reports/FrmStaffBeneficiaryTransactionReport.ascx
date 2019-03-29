<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FrmStaffBeneficiaryTransactionReport.ascx.cs" Inherits="ExpenseManager.ExpenseMgt.Reports.FrmStaffBeneficiaryTransactionReport" %>
<%@ Register TagPrefix="uc2" TagName="ErrorDisplay" Src="~/CoreFramework/ErrorControl/ErrorDisplay.ascx" %>
<link href="App_Themes/Default/ControlStyles.css" rel="stylesheet" type="text/css" />

<style type="text/css">
    .customH2 
    {
        margin-top: 0;
	    font-weight: 600;
	    margin-bottom: 0;
	    line-height: 0;
	    padding-bottom:0;
	    font-family: 'OCR A Extended', arial; 
	    border-bottom: none;
	    font-weight: normal;
    }
    
    .btmSpacing {
        padding-bottom: 100%; 
        margin-bottom: 100%
    }
    .customLabel {
        color: #038103; 
        font-family:'OCR A Extended', arial;
    }
    
     .customLabel2 {
        color: black; 
        font-family:'Trebuchet MS','Tw Cen MT',Tahoma;
        font-size: 8pt
    }
    .text-box-x
    {}
    .customButton
    {}
</style>

    <table style="width: 98%; margin-top: 1.3%; border-bottom: 1px solid #038103">
        <tr>
            <td style="width: 50%">
               <h2 runat="server" id="hTitle" class="customH2">Staff Beneficiaries' Transactions Report</h2> 
            </td>
            <td style="width: 50%">
                <div style="margin-left: 1%; width: 612px;">
                    <asp:Linkbutton ID="lnkViewPrintVouchers" runat="server" CssClass="linkStyleColor2" ForeColor="#038103"  Width="25%" Text="View & Print Vouchers >>" OnClick="LnkViewPrintVouchersClick"></asp:Linkbutton>
                    <asp:Linkbutton ID="lnkCloseVouchersSection" runat="server" CssClass="linkStyleColor" ForeColor="#038103" Width="150px" Text=" << Close" onclick="LnkCloseVouchersSectionClick"></asp:Linkbutton>
                </div>
            </td>
        </tr>
    </table>
	<div style="padding-bottom: 5px"><asp:Panel ID="Panel2" runat="server" Width="98%"><uc2:ErrorDisplay ID="ErrorDisplay1" runat="server" /></asp:Panel></div>
    <div style="width: 99%; padding: 1px;" class="gridDiv" >
       <table border="0" cellspacing="0" cellpadding="2" width="100%" runat="server" id="tblTransactionsReport">
          <tbody>
             <tr>
				<td style="width: 100%" class="divBackground">
					<div style="width: 41%;font-weight: bolder" >
						<label style="color: #038103; font-family: 'OCR A Extended', arial">All registered Expense Transactions </label>
                    </div>
				</td>
			  </tr>
             <tr>
              <td colspan="2" style="width: 100%">
                  <table style="width: 100%">
                      <tr>
                          <td style="width: 60%">
                              <div>
                                  <label style="color: purple; font-family: 'OCR A Extended', arial; margin-top: -4px"> Filter by Department</label>
                                  <asp:DropDownList Width="19%"  runat="server" ID="ddlDepartment" CssClass="text-box-x" AutoPostBack="True" OnSelectedIndexChanged="DdlDepartmentIndexChanged"></asp:DropDownList>&nbsp;
                                  <label style="color: purple; font-family: 'OCR A Extended', arial; margin-top: -4px">&nbsp; Filter by Portal User</label>
                                  <asp:DropDownList Width="19%"  runat="server" ID="ddlPortalUser" CssClass="text-box-x" AutoPostBack="True" OnSelectedIndexChanged="DdlPortalUserIndexChanged"></asp:DropDownList>&nbsp;
                              </div>
                          </td>
                          <td style="width: 30%">
                              <div style="width: 540px; margin-left: 1%">
                                  <label style="color: #038103; font-family: 'OCR A Extended', arial; margin-top: -4px"> Filter by a Date Range-</label>&nbsp;
                                  <label style="color: purple; font-family: 'OCR A Extended', arial; margin-top: -4px"> From:</label>
                                  <asp:TextBox Width="14%"  runat="server" ID="txtStart" CssClass="text-box-x"></asp:TextBox>&nbsp;
                                  <label style="color: purple; font-family: 'OCR A Extended', arial; margin-top: -4px"> To:</label>
                                  <asp:TextBox Width="14%"  runat="server" ID="txtEndDate" CssClass="text-box-x"></asp:TextBox>&nbsp;
                                  <asp:Button ID="btnDateFilter" Text="Go" CssClass="customButton" OnClick="BtnDateFilterClick" runat="server" Width="24px"></asp:Button>
                                  &nbsp;&nbsp;&nbsp;<ajaxToolkit:calendarextender ID="CalendarExtFrom" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtStart"></ajaxToolkit:calendarextender>
                                  <ajaxToolkit:calendarextender ID="CalendarExtTo" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtEndDate"></ajaxToolkit:calendarextender>
                              </div>
                          </td>
                          <td style="width: 10%">
                              <div style="margin-left: -5%">
                                  <asp:Button ID="btnGetAllTransactions" runat="server" CssClass="customButton" Width="140px" Text="Get All Transactions" onclick="BtnRefreshClick"></asp:Button>
                              </div>
                          </td>
                      </tr>
                  </table>
              </td>
           </tr>
           <tr>
             <td style="width: 100%" colspan="3">
               <asp:DataGrid ID="dgExpenseTransaction" runat="server" AutoGenerateColumns="False" CellPadding="1"  CellSpacing="1" GridLines="None" CssClass="xPlugTextAll_x"  DataKeyField="StaffExpenseTransactionId" 
                 ShowFooter="True" Width="100%">
                 <FooterStyle CssClass="gridFooter" />
                 <AlternatingItemStyle CssClass="alternatingRowStyle"  />
                 <ItemStyle CssClass="gridRowItem" />
                 <HeaderStyle CssClass="gridHeader" />
                    <Columns>
                        <asp:TemplateColumn HeaderText="S/No.">
                            <HeaderStyle HorizontalAlign="center" Width="2%" />
                            <ItemStyle CssClass="xPlugTextAll_x" HorizontalAlign="center" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblSNo" runat="server" CssClass="xPlugTextAll_x" Text="<%# ((dgExpenseTransaction.PageSize*dgExpenseTransaction.CurrentPageIndex) + Container.ItemIndex + 1)%>">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Expense Title" >
                            <HeaderStyle HorizontalAlign="left" Width="15%" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblTitle" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"ExpenseTitle")) %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ExpenseTitle") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Quantity" >
                            <HeaderStyle HorizontalAlign="left" Width="2%" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblQuantity" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Quantity")) %>' >
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Unit Price" >
                            <HeaderStyle HorizontalAlign="left" Width="6%" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblUnitPrice" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"UnitPrice")) %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Expense Item" >
                            <HeaderStyle HorizontalAlign="left" Width="12%" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblExpenseItemTitle" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"ExpenseItem.Title")) %>' >
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Accounts Head" >
                            <HeaderStyle HorizontalAlign="left" Width="10%" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblAccountsHead" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"AccountsHead")) %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Expense Category" >
                            <HeaderStyle HorizontalAlign="left" Width="7%" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblExpenseCategoryTitle" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"ExpenseCategory.Title")) %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Expense Type" >
                            <HeaderStyle HorizontalAlign="left" Width="7%" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblExpenseType" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"ExpenseType.Name")) %>' >
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Beneficiary" >
                            <HeaderStyle HorizontalAlign="left" Width="10%" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblBeneficiaryName" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"StaffBeneficiary.FullName")) %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Requested By" >
                                <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblRequester"  runat="server" CssClass="commentLink" Text='<%#  GetUserFullName(int.Parse((DataBinder.Eval(Container.DataItem,"RegisteredById")).ToString()))%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Payment Status" >
                                <HeaderStyle HorizontalAlign="Left" Width="8%" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblUserTransactionPaymentStatus"  runat="server" CssClass="xPlugTextAll_x" Text='<%#  (DataBinder.Eval(Container.DataItem,"PaymentStatus"))%>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Date" >
                            <HeaderStyle HorizontalAlign="left" Width="4%" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblTransactionDate" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"TransactionDate")) %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Time" >
                            <HeaderStyle HorizontalAlign="left" Width="6%" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblTransactionTime" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"TransactionTime")) %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Status" >
                            <HeaderStyle HorizontalAlign="center" Width="7%" />
                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server"  CssClass="xPlugTextAll_x" Text='<%#  (DataBinder.Eval(Container.DataItem,"ApprovalStatus"))%>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                   <PagerStyle HorizontalAlign="Right" Mode="NumericPages" />
                </asp:DataGrid> 
             </td>
           </tr>
         </tbody>
      </table>
      <div id="staffContentDv" style="width: 100%; height: auto"></div>
     <div style="width: 100%"   id="dvStaffVouchers" runat="server">
      <table style="width: 100%">
        <tr>
            <td style="width: 40%; height: 5%" class="divBackground">
                <div style="width: 102%; margin-left: 2.5%">
                     <label style="color: #038103; font-family: 'OCR A Extended', arial; margin-top: -4px"> Filter Vouchers by Department</label>
                     <select style="width: 32%; height: 22px; margin-bottom: 0px;"  runat="server" ClientIDMode="Static" ID="ddlStaffDepartmentVouchers" onchange="toggleStaffPortalUsersSelection()" class="text-box-x"></select>    
                </div>
            </td>
            <td style="width: 40%" class="divBackground">
                <div style="width: 90%; margin-left: 2.5%">
                    <label style="color: #038103; font-family: 'OCR A Extended', arial; margin-top: -4px">Filter by Portal User</label>
                    <select style="width: 43%"  runat="server" ClientIDMode="Static" ID="ddlStaffPortalUsersVouchers" onchange="toggleStaffDepartmentSelection()" 
                        class="text-box-x"></select>&nbsp;
                </div> 
            </td>
            <td style="width: 30%" class="divBackground">
               <div style="width: 287px">
                   <input type="button" id="btnGetAllStaffVouchers" ClientIDMode="Static" style=" width: 114px; margin-left: 1%" runat="server" class="customButton" value="Get All Vouchers"/>
                   <input type="button" id="btnPrintAllStaffVouchers" ClientIDMode="Static" style=" width: 124px; margin-left: 10px" runat="server" class="customButton" value="Print All Vouchers"/>
               </div>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                
            </td>
        </tr>
        <tr>
            <td style="width: 30%">
                <div style="width: 99%; margin-left: 2.5%">
                    <label style="color: #038103; font-family: 'OCR A Extended', arial; margin-top: -4px"> Filter by a Date Range-</label>&nbsp;
                    <label style="color: purple; font-family: 'OCR A Extended', arial; margin-top: -4px"> From:</label>
                    <asp:TextBox Width="34%"  runat="server" id="txtStaffVoucherDateFrom" CssClass="text-box-x" ClientIDMode="Static" Height="20px"></asp:TextBox>&nbsp;
                    <ajaxToolkit:calendarextender ID="ceStaffVoucherDateFrom" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtStaffVoucherDateFrom"></ajaxToolkit:calendarextender>
                </div> 
            </td>
            <td style="width: 40%">
                <div style="width: 93%; margin-left: 2.5%">
                    <label style="color: purple; font-family: 'OCR A Extended', arial; margin-top: -4px"> To:</label>
                    <asp:TextBox Width="34%"  runat="server" id="txtStaffVoucherDateTo" CssClass="text-box-x" ClientIDMode="Static" Height="20px"></asp:TextBox>&nbsp;
                    <ajaxToolkit:calendarextender ID="ceStaffVoucherDateTo" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtStaffVoucherDateTo"></ajaxToolkit:calendarextender>&nbsp;
                    <input type="button" id="btnGetStaffVouchersByFields" ClientIDMode="Static" value="Go" style="width : 34%" class="customizedButton" runat="server" /> 
                </div>
             </td>
             <td style="width: 30%">
                 
             </td>
        </tr>
      </table>
      <div style="width: 100%; margin-top: 5%; margin-left: 16%" id="dvStaffVouchersInfo">
     </div>
   </div>
</div>