<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VoucherManager.aspx.cs" Inherits="ExpenseManager.ExpenseMgt.Voucher.VoucherManager" %>
<%@ Import Namespace="XPLUG.WEBTOOLS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
       <%-- <style type="text/css">
            
            .customH2 {
                margin-top: 0;font-weight: 600;margin-bottom: 0;line-height: 0;padding-bottom:0;color: darkorange;font-family: 'OCR A Extended'; border-bottom: none;font-weight: normal;
            }
            .customLabel {
                color: #038103; font-family:wf_segoe-ui_light,"Segoe UI Light","Segoe WP Light","Segoe UI","Segoe WP",Tahoma,Arial,sans-serif; font-size: 0.9em;
            }
            .customLabel2 {
                color: black;font-family: wf_segoe-ui_light,"Segoe UI Light","Segoe WP Light","Segoe UI","Segoe WP",Tahoma,Arial,sans-serif;font-weight: bold; font-size: 1em;
            }
            
             .customLabel3 {
                color: black;font-family: wf_segoe-ui_light,"Segoe UI Light","Segoe WP Light","Segoe UI","Segoe WP",Tahoma,Arial,sans-serif;font-weight: bold; font-size: .9em;
            }
            
            @page {
            size: auto; padding: 0.25in 0.5in;}
            .xPlugTextAll_x 
            {
                font-family: 'Times New Roman','Trebuchet MS', 'Tw Cen MT', Tahoma, Arial; font-size: 1.04em;color: black;
            }
            body {
                font-size:1em;font-family:'Times New Roman', 'Helvetica Neue','Lucida Grande','Segoe UI',Arial,Helvetica,Verdana,'sans-serif';color: black;
            }
            .style1
            {
                width: 100%;
            }
        </style>--%>
       <%-- <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" EnablePageMethods="True" AsyncPostBackTimeout="2000" runat="server" EnablePartialRendering="true">
            <Scripts >
				<asp:ScriptReference Path="~/Scripts/jquery-2.0.3.min.js" />
                <asp:ScriptReference Path="~/Scripts/jquery.validate.min.js" />
                <asp:ScriptReference Path="~/Scripts/jquery.formatCurrency-1.4.0.min.js" />
                <asp:ScriptReference Path="~/Scripts/xpenseManager.js" />
                <asp:ScriptReference Path="~/Scripts/xpenseManagerNumberToWord.js" />
           </Scripts >
        </ajaxToolkit:ToolkitScriptManager><table style=""></table>--%>
   <%-- <div>
    <div style="width: 100%" id="dvVoucherManager">
     <div id="staffParentDv" style="page-break-after: always">
      <table style="width: 100%" id="staffContainerTbl">
         <tr>
           <td colspan="2">
              <table style="width: 100%">
               <tr>
                   <td id="imgDV" colspan="2">
                       <div id="logoDv" style="margin-left: 40%; width: 11%;">
                           <img src="" runat="server" id="imgLogo"  alt="" style="width: 100%; height: 50%" />
                       </div>
                   </td>
               </tr>
               <tr>
                   <td colspan="2">
                       <div style=" font-weight: bolder; width: 22%;  margin-left: 38%; font-size: 1.3em">
                           <label  class="customLabel2" style="width: 100%;"> Transaction Details</label>
                       </div>
                   </td>
               </tr>
               <tr>
                   <td colspan="2"></td>
               </tr>
               <tr>
                   <td colspan="2">
                      <label class="customLabel3">Transaction: </label>&nbsp;<label  class="customLabel2" style="width: 100%;" id="lblTransactionTitle" runat="server"></label> 
                   </td>
               </tr>
               <tr id = "trCheque">
                   <td colspan="2" class="divBackground2">
                       <table style="width: 100%;">
                           <tr id="chequeRow" runat="server">
                               <td style="width: 60%"></td>
                               <td style="width: 20%">
                                   <label style="width: 100%" class="customLabel">Cheque No:</label>
                               </td>
                               <td style="width: 20%">
                                   <label class="customLabel2" id="lblChequeNo" style="width: 100%" runat="server"></label>
                               </td>
                           </tr>
                           <tr>
                               <td style="width: 60%"></td>
                               <td style="width: 20%">
                                   <label style="width: 100%;" class="customLabel">PCV No:</label>
                               </td>
                               <td style="width: 20%">
                                   <label class="customLabel2" id="lblPCV" style="width: 100%" runat="server"></label>
                               </td>
                           </tr>
                           <tr>
                               <td style="width: 60%"></td>
                               <td style="width: 20%">
                                   <label style="width: 100%" class="customLabel">Date: </label>
                                </td>
                                <td style="width: 20%"><label class="customLabel2" id="lblPaymentDate" runat="server"></label></td>
                           </tr>
                           <tr>
                              <td colspan="3">
                                 <div style="padding-top: 1%">
                                    <label class="customLabel">Requested by: </label>
                                    <label id="lblRequestedBy" style="width: 100%" class="customLabel2" runat="server"></label>
                                </div>
                            </td>
                        </tr>
                  </table>
              </td>
        </tr>
      </table>
      <table id="staffTblk" class="xPlugTextAll_x" style="width:100%; border-collapse:collapse">
        <tr>
            <td style="width: 100%">
               <asp:DataGrid ID="dgUserInitiatedTransactions" runat="server" AutoGenerateColumns="False"  CellPadding="1" CellSpacing="1" GridLines="Vertical" CssClass="xPlugTextAll_x" DataKeyField="ExpenseTransactionId" ShowFooter="False" Width="100%" >
                     <FooterStyle CssClass="gridFooter" />
                       <AlternatingItemStyle CssClass="alternatingRowStyle"/>
                         <ItemStyle CssClass="gridRowItem" />
                          <HeaderStyle CssClass="gridHeader" />
                            <Columns>
                                <asp:TemplateColumn HeaderText="S/No.">
                                    <HeaderStyle HorizontalAlign="center" Width="4%" CssClass="tdpadtop" ForeColor="black" BorderStyle="Solid" BorderColor="black" BorderWidth="1px"/>
                                    <ItemStyle CssClass="xPlugTextAll_x" HorizontalAlign="center" VerticalAlign="Top" />
                                    <ItemTemplate>     
                                        <asp:Label ID="lblUserTransactionTitleSNo" runat="server" CssClass="xPlugTextAll_x" Text="<%# ((dgUserInitiatedTransactions.PageSize*dgUserInitiatedTransactions.CurrentPageIndex) + Container.ItemIndex + 1)%>">
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Item Code" >
                                    <HeaderStyle HorizontalAlign="left" Width="12%" CssClass="tdpadtop" ForeColor="black" BorderStyle="Solid" BorderColor="black" BorderWidth="1px"/>
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblItemCode" runat="server" CssClass="linkStyle" Text='<%# (DataBinder.Eval(Container.DataItem,"ExpenseItem.Code")) %>' CommandName="Edit" >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Particulars of Payment" >
                                    <HeaderStyle HorizontalAlign="Left" Width="40%" CssClass="tdpadtop" ForeColor="black" BorderStyle="Solid" BorderColor="black" BorderWidth="1px"/>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblItemTitle"  runat="server" CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"ExpenseItem.Title")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                 <asp:TemplateColumn HeaderText="Quantity" >
                                    <HeaderStyle HorizontalAlign="left" Width="5%" CssClass="tdpadtop" ForeColor="black" BorderStyle="Solid" BorderColor="black" BorderWidth="1px"/>
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuantity" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"ApprovedQuantity")) %>' >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Unit Price(N)" >
                                    <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop" ForeColor="black" BorderStyle="Solid" BorderColor="black" BorderWidth="1px"/>
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnitPrice" runat="server"  CssClass="xPlugTextAll_x" Text='<%#NumberMap.GroupToDigits((DataBinder.Eval(Container.DataItem,"ApprovedUnitPrice")).ToString()) %>' >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Total Price(N)" >
                                    <HeaderStyle HorizontalAlign="Left" Width="13%" CssClass="tdpadtop" ForeColor="black" BorderStyle="Solid" BorderColor="black" BorderWidth="1px"/>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblApprovalStatus"  runat="server" CssClass="xPlugTextAll_x" Text='<%#  NumberMap.GroupToDigits((DataBinder.Eval(Container.DataItem,"ApprovedTotalPrice")).ToString())%>' ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                          </Columns>
                       <PagerStyle HorizontalAlign="Right" Mode="NumericPages" />
                    </asp:DataGrid>
                 </td>
             </tr> 
             <tr>
                 <td>
                     
                 </td>
             </tr>
             <tr>
                 <td style="width: 100%">
                     <table class="xPlugTextAll_x" cellspacing="0" style="width: 100%; border-collapse:collapse; border-spacing:0 0; border-left: none">
                        <tr>
                            <td style="width:4%"></td>
                            <td style="width:12%"></td>
                            <td style="width:40%"></td>
                            <td style="width:5%"></td>
                            <td style="width:14%">
                                <label style="color: black; font-weight: bold; margin-left: 20%; width: 60%">TOTAL:</label>
                            </td>
                            <td style="border-top: solid 1px black; width: 14%; border-bottom: solid 1px black; border-left: none; border-right: none;">
                               <label id="lblTotals" ClientIDMode="Static" runat="server" style="width: 100%; color: black; font-style: italic"></label> 
                            </td>
                        </tr>
                    </table> 
                 </td>
             </tr>
             <tr>
                 <td style="width: 100%">
                    <table style="width: 100%">                    
                        <tr>
                            <td colspan="3">
                               <table style="width: 100%">
                                   <tr>
                                       <td style="width: 17%">
                                           <label style="color: black; font-weight: bold; width: 100%">Amount in words:</label> 
                                       </td>
                                       <td style="width: 70%">
                                          <label id="lblAmountInWords" style="width: 100%"></label>  
                                       </td>
                                   </tr>
                               </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <table style="width:  100%">
                                    <tr>
                                        <td style="width: 30%">
                                            <label style="width: 100%; font-weight: normal; color: black">Approved By:</label>
                                        </td>
                                        <td style="width: 40%">
                                            <label style="width: 100%; font-weight: normal; color: black">Received By:</label>
                                        </td>
                                        <td style="width: 30%">
                                            <label style="width: 100%; font-weight: normal; color: black">Amount Received:</label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%">
                                            <label style="width: 100%; font-weight: normal; color: black" id="lblApprover" runat="server"></label>
                                        </td>
                                        <td style="width: 40%">
                                            <label style="width: 100%; font-weight: normal; color: black" id="lblReceiver" runat="server"></label>
                                        </td>
                                        <td style="width: 30%">
                                            <label style="width: 100%; font-weight: normal; color: black; font-style: italic" id="lblAmountReceived" runat="server"></label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table> 
                 </td>
             </tr>
         </table>
       </table>
     </div>
    </div>
    </div>--%>
   <%-- <script type="text/javascript">
        $(document).ready(function () 
        {
            var naira = " Naira";
            var kobo = "Kobo";
            var wrdLbl = $get('lblAmountInWords');
            var totalPriceControl = $get('lblTotals');
            var ammountReceivedControl = $get('lblAmountReceived');
            var totalPrice = totalPriceControl.innerHTML;
            getFormatedAmountInWords(numbersToWord(totalPrice, naira, kobo), wrdLbl.id, naira);
            $('#' + totalPriceControl.id).formatCurrency({ symbol: 'N', roundToDecimalPlace: 1 });
            $('#' + ammountReceivedControl.id).formatCurrency({ symbol: 'N', roundToDecimalPlace: 1 });
        });
    </script>--%>
    </form>
</body>
</html>
