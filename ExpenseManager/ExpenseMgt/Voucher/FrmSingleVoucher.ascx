<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FrmSingleVoucher.ascx.cs" Inherits="ExpenseManager.ExpenseMgt.Reports.FrmVouchers" %>

<link href="App_Themes/Default/ControlStyles.css" rel="stylesheet" type="text/css" />
     <style type="text/css">
     </style>
   <div class="dvContainer">
          <h2> Print Single Transaction Payment Voucher</h2>
     <div style="width: 100%"   id="dvVouchers" runat="server">
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
                <asp:DropDownList runat="server" ID="ddlVoucherFilterOption" ClientIDMode="Static" Width = "93%" class="text-box" ></asp:DropDownList>
            </td>
            <td class="tdpad" style="width: 4%">
            <label class="label3">Department</label>
            </td> 
             <td class="tdpad" style="width: 15%">
                <asp:DropDownList runat="server" ID="ddlDepartmentVoucher" onchange="clearForm()" ClientIDMode="Static" Width = "93%" class="text-box"></asp:DropDownList> 
            </td>
            <td style="width: 50%" class="tdpad">
               <label> From:</label>
               <asp:TextBox Width="22%"  runat="server" ClientIDMode="Static" id="txtVoucherStartDate" CssClass="text-box-x" ></asp:TextBox>&nbsp;
               <label> To:</label>
               <asp:TextBox Width="22%"  runat="server" ID="txtVoucherEndDate" CssClass="text-box-x" ClientIDMode="Static"></asp:TextBox>&nbsp;
               <input type="button" id="btnVoucherDateFilter" value="Go" class="customButton" style="width: 72px" />
                &nbsp;&nbsp;&nbsp;<ajaxToolkit:calendarextender ID="CalendarExtFrom" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtVoucherStartDate"></ajaxToolkit:calendarextender>
                <ajaxToolkit:calendarextender ID="CalendarExtTo" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtVoucherEndDate"></ajaxToolkit:calendarextender>
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
	   <div style="width: 30%; margin-top: 5%; margin-left: 25%" id="dvVouchersInfo">
	      <select id="ddlTransactions" class="text-box" style="width: 100%; visibility: visible; display: none;"> </select> 
	   </div>
      <div id="hiddenDiv" style="visibility: visible; display: none; margin-top: 4%; width: 73%; margin-left: 10%">
      </div>
   </div>
   </div>
