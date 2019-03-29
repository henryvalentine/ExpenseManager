<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FrmStaffExpenseApprovalReport.ascx.cs" Inherits="ExpenseManager.ExpenseMgt.Reports.FrmStaffExpenseApprovalReport" %>
<%@ Register TagPrefix="uc2" TagName="ErrorDisplay" Src="~/CoreFramework/ErrorControl/ErrorDisplay.ascx" %>
    <style type="text/css">
     
        .customNewCheckbox
       {
           text-align: center;
            float: right;
            margin-right: 15%;
            width: auto;
        }
         
        </style>
    
    <div> <asp:Button ID="btnPopUp" runat="server" Style="display: none" /></div>
    <h2 style=" font-weight: normal; font-family: 'OCR A Extended', arial; border-bottom-color: #038103; margin-top: 1.3%">
        &nbsp;Transaction Approvals for Staff Beneficiaries</h2>
    <div style="padding-bottom: 5px"><asp:Panel ID="Panel2" runat="server" Width="98%"><uc2:ErrorDisplay ID="ErrorDisplay1" runat="server" /></asp:Panel></div>
     <div runat="server" id="divReport" style="width: 99%; padding: 1px;" class="gridDiv">
        <table border="0" cellspacing="0" cellpadding="2" width="100%" runat="server" id="tbExpenseType">
            <tbody>
               <tr>
                  <td width="100%" colspan="2">
				     <table style="border-style: none; border-color: inherit; border-width: medium; width: 100%; padding: 0px; height: 38px;">
					    <tr>
						   <td style="width: 20%" class="divBackground">
						        <div style="width: 98%; font-weight: bolder" >
						           <label style="color: #038103; font-family: 'OCR A Extended', arial;">Approved Transactions</label>
                                </div>
                            </td>
                            <td style="width: 70%" class="divBackground">
                              <div style="width: 789px; margin-left: 9%">
                                <label style="color: #038103; font-family: 'OCR A Extended', arial; margin-top: -4px"> Filter by a Date Range-</label>&nbsp;
                                <label style="color: purple; font-family: 'OCR A Extended', arial; margin-top: -4px"> From:</label>
                                <asp:TextBox Width="22%"  runat="server" ID="txtStart" CssClass="text-box-x"></asp:TextBox>&nbsp;
                                <label style="color: purple; font-family: 'OCR A Extended', arial; margin-top: -4px"> To:</label>
                                <asp:TextBox Width="22%"  runat="server" ID="txtEndDate" CssClass="text-box-x"></asp:TextBox>&nbsp;
                                <asp:Button ID="btnDateFilter" Text="Go" CssClass="customButton" OnClick="BtnDateFilterClick" runat="server" Width="24px"></asp:Button>
                                <ajaxToolkit:calendarextender ID="CalendarExtFrom" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtStart"></ajaxToolkit:calendarextender>
                                <ajaxToolkit:calendarextender ID="CalendarExtTo" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtEndDate"></ajaxToolkit:calendarextender>
                            </div>
                        </td>
                        <td style="width: 10%" class="divBackground">
                                <div style="margin-left: 10; width: 80px;" >
                                    <asp:Button ID="btnRefresh" runat="server" CssClass="customButton" Text="Retrieve All" onclick="BtnRefreshClick"></asp:Button> 
                                </div>
                        </td>
					 </tr>
				   </table>
				 </td>
			   </tr>
              <tr>
                <td width="100%" align="left">
                    <asp:DataGrid ID="dgApprovedTransactions" runat="server" AutoGenerateColumns="False" CellPadding="1" CellSpacing="1" GridLines="none" CssClass="xPlugTextAll_x"  DataKeyField="StaffExpenseApprovalId" ShowFooter="True" Width="100%">
                        <FooterStyle CssClass="gridFooter" />
                        <AlternatingItemStyle CssClass="alternatingRowStyle"/>
                        <ItemStyle CssClass="gridRowItem" />
                        <HeaderStyle CssClass="gridHeader" />
                            <Columns>
                                <asp:TemplateColumn HeaderText="S/No.">
                                    <HeaderStyle HorizontalAlign="center" Width="2%" />
                                    <ItemStyle CssClass="xPlugTextAll_x" HorizontalAlign="center" VerticalAlign="Top" />
                                    <ItemTemplate>     
                                        <asp:Label ID="lblSNo" runat="server" CssClass="xPlugTextAll_x" Text="<%# ((dgApprovedTransactions.PageSize*dgApprovedTransactions.CurrentPageIndex) + Container.ItemIndex + 1)%>">
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Transaction" >
                                    <HeaderStyle HorizontalAlign="left" Width="15%" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblTitle" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"StaffExpenseTransaction.ExpenseTitle")) %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Approved Quantity" >
                                    <HeaderStyle HorizontalAlign="left" Width="10%" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuantity" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"ApprovedQuantity")) %>' >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Approved Unit Price" >
                                    <HeaderStyle HorizontalAlign="left" Width="12%" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnitPrice" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"ApprovedUnitAmmount")) %>' >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                 <asp:TemplateColumn HeaderText="Approved Total Price" >
                                    <HeaderStyle HorizontalAlign="left" Width="12%" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblApprovedTotalCost" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"ApprovedTotalAmmount")) %>' >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                 <asp:TemplateColumn HeaderText="Date Approved" >
                                    <HeaderStyle HorizontalAlign="left" Width="10%" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblDateApproval" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"DateApproved")) %>' >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Time Approved" >
                                    <HeaderStyle HorizontalAlign="left" Width="10%" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblTimeofApproval" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"TimeApproved")) %>' >
                                        </asp:Label>
                                    </ItemTemplate>
                                 </asp:TemplateColumn>
                                 <asp:TemplateColumn HeaderText="Approved By" >
                                    <HeaderStyle HorizontalAlign="left" Width="8%" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblApprovalPersonel" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"ApprovedBy")) %>' >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Approval Status" >
                                    <HeaderStyle HorizontalAlign="center" Width="10%" />
                                    <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" CssClass="xPlugTextAll_x" Text='<%#  ((DataBinder.Eval(Container.DataItem,"StaffExpenseTransaction.Status")).ToString() == "1")? "Approved" : "Not Approved"%>'>
                                        </asp:Label>
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