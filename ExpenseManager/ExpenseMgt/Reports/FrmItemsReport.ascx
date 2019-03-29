<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FrmItemsReport.ascx.cs" Inherits="ExpenseManager.ExpenseMgt.Reports.FrmItemsReport" %>
<%@ Import Namespace="XPLUG.WEBTOOLS" %>
<%@ Register src="../../CoreFramework/ErrorControl/ErrorDisplay.ascx" tagname="ErrorDisplay" tagprefix="uc2" %>
<link href="App_Themes/Default/ControlStyles.css" rel="stylesheet" type="text/css" />
<div class="dvContainer">
    <h2 runat="server" id="hTitle">Cost Reports By Expense Items</h2>
    <uc2:ErrorDisplay ID="ErrorDisplay1" runat="server" />
    <table style="width: 100%">
        <tr>
            <td colspan="7" class="tdpadd">
            <label style="width: 100%">
                Note on <b>FILTERING</b>: &nbsp;Select an <b>Expense Item</b> and then get it's cost 
                History by <b>Date Range</b>
            </label>
        </td>
        </tr>
        <tr class="divBackground3">
            <td class="tdpad" style="width: 7%">
                <div style="width: 100%">
                <label >Expense Item</label>
                                             
                </div>
            </td>
            <td class="tdpad" style="width: 35%">
                <asp:DropDownList runat="server" ID="ddlExpenseItems" Width="100%" CssClass="text-box"></asp:DropDownList>    
            </td>
            <td class="tdpad" style="width: 3%">
                <label> From:</label>
            </td>
            <td class="tdpad" style="width: 22%">
            <asp:TextBox  runat="server" ID="txtStart" CssClass="text-box" Width="100%"></asp:TextBox>
            </td>
            <td class="tdpad" style="width: 3%">
            <label> To:</label>    
            </td>
            <td class="tdpad" style="width: 22%">
            <asp:TextBox   runat="server" ID="txtEndDate" CssClass="text-box" Width="100%"></asp:TextBox>   
            </td>

            <td style="width: 8%">
                <asp:Button ID="btnDateFilter" Text="Go" CssClass="customButton" OnClick="BtnDateFilterClick" runat="server" style="width: 72px; margin-left:20%"></asp:Button>
                <ajaxToolkit:calendarextender ID="CalendarExtFrom" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtStart"></ajaxToolkit:calendarextender>
                <ajaxToolkit:calendarextender ID="CalendarExtTo" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtEndDate"></ajaxToolkit:calendarextender>
            </td>
       </tr>
       <tr class="gridFoot">
           <td colspan="7"></td>
       </tr>
       <tr class="gridFoot">
           <td colspan="5" style="font-weight: bold"><br/>
               Expense Item: &nbsp;
              <label id="itemTitle" runat="server" style="color: green; width: auto"></label><br/><br/>
           </td>
           <td colspan="2" style="font-weight: bold"><br/>
               Grand Total Cost For the selected Period: &nbsp;<label style="color: green; font-weight: bold" id="lblGrandTotal" runat="server"></label><br/><br/>
           </td>
       </tr>
       <tr>
            <td style="width: 100%" colspan="7">
               <asp:DataGrid ID="dgCostItems" runat="server" AutoGenerateColumns="False" CellPadding="1" CellSpacing="1" GridLines="none" CssClass="xPlugTextAll_x" DataKeyField="ExpensenseItemId" 
                    ShowFooter="True" Width="100%" >
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
                                <asp:TemplateColumn HeaderText="Total Quantity" >
                                    <HeaderStyle HorizontalAlign="Left" Width="8%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblApprovedQuantity"  runat="server"  Text='<%#(DataBinder.Eval(Container.DataItem,"TotalApprovedQuantity"))%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Unit Price(N)" >
                                    <HeaderStyle HorizontalAlign="Left" Width="12%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblCostUnitPrice"  runat="server" CssClass="xPlugTextAll_x" Text='<%#NumberMap.GroupToDigits( (DataBinder.Eval(Container.DataItem,"ApprovedUnitPrice")).ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Total Price(N)" >
                                    <HeaderStyle HorizontalAlign="Left" Width="12%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblCostTotalPrice"  runat="server" CssClass="xPlugTextAll_x" Text='<%# NumberMap.GroupToDigits((DataBinder.Eval(Container.DataItem,"TotalApprovedPrice")).ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Date Approved" >
                                    <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblCostItemApprovalDate" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"ExpenseTransaction.DateApproved")) %>' >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Time Approved" >
                                    <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblCostItemApprovalTime" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"ExpenseTransaction.TimeApproved")) %>' >
                                        </asp:Label>
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
    </table>
</div>