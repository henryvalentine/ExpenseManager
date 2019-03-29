<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="frmTransactionReport.ascx.cs" Inherits="ExpenseManager.ExpenseMgt.Reports.FrmTransactionReport" %>
<%@ Register src="../../CoreFramework/AlertControl/ConfirmAlertBox.ascx" tagname="ConfirmAlertBox" tagprefix="uc1" %>
<link href="App_Themes/Default/ControlStyles.css" rel="stylesheet" type="text/css" />


 <div class="dvContainer">
    
    <h2 runat="server" id="hTitle">Transaction Reports</h2>	
    <div id="dvReports" style="margin-top: 2%" runat="server">
    <div style="width: 45%; margin-left: 25%" id="dvSearch" runat="server">
      <fieldset>
          <legend>Select Options</legend>
          <table style="width: 100%; border: none; padding: 3px">
             <tr> 
             <td colspan="5">
                 <div style="width: auto"> Status: <span style="color: #FF0000">*</span><asp:RequiredFieldValidator ID="ReqFilterOption" ValidationGroup="valSelection"  runat="server" ErrorMessage="" ControlToValidate="ddlFilterOption" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> Required </asp:RequiredFieldValidator><asp:CompareValidator ID="CompareValFilterOption" runat="server" ErrorMessage="" ValueToCompare="0" ValidationGroup="valSelection" ControlToValidate="ddlFilterOption" Operator="GreaterThanEqual" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" >Invalid Selection</asp:CompareValidator></div>
                <asp:DropDownList runat="server" ID="ddlFilterOption" ClientIDMode="Static" Width = "93%" class="text-box"  ></asp:DropDownList>
            </td>
           </tr>
           <tr>
             <td colspan="5">
                 <div style="width: auto">Department:<span style="color: #FF0000">*</span><asp:RequiredFieldValidator ID="ReqDepartment" ValidationGroup="valSelection"  runat="server" ErrorMessage="" ControlToValidate="ddlDepartment" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> Required </asp:RequiredFieldValidator><asp:CompareValidator ID="CompareValDepartment" runat="server" ErrorMessage="" ValueToCompare="1" ValidationGroup="valSelection" ControlToValidate="ddlDepartment" Operator="GreaterThanEqual" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" >Invalid Selection</asp:CompareValidator></div>
                <asp:DropDownList runat="server" ID="ddlDepartment" ClientIDMode="Static" Width = "93%" class="text-box"></asp:DropDownList> 
                
            </td> 
           </tr>
           <tr id="trPeriod" style="display: none">
                <td colspan="5" >
                    <div style="width: auto">Period:<span style="color: #FF0000">*</span><asp:RequiredFieldValidator ID="Req" ValidationGroup="valSelection"  runat="server" ErrorMessage="" ControlToValidate="ddlPeriod" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> </asp:RequiredFieldValidator><asp:CompareValidator ID="CompareValPeriod" runat="server" ErrorMessage="" ValueToCompare="1" ValidationGroup="valSelection" ControlToValidate="ddlPeriod" Operator="GreaterThanEqual" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" >Invalid Selection</asp:CompareValidator></div>
                    <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="text-box" Width="93%"></asp:DropDownList>                     
                </td>              
            </tr>
            <tr id="trYear" style="display: none">
                <td colspan="5">
                    <div style="width: auto">Year:<span style="color: #FF0000">*</span> <asp:RequiredFieldValidator ID="ReqYear" ValidationGroup="valSelection"  runat="server" ErrorMessage="" ControlToValidate="ddlYear" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> </asp:RequiredFieldValidator> <asp:CompareValidator ID="CompareValYear" runat="server" ErrorMessage="" ValueToCompare="1" ValidationGroup="valSelection" ControlToValidate="ddlYear" Operator="GreaterThanEqual" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" >Required</asp:CompareValidator></div>
                    <asp:DropDownList ID="ddlYear" runat="server" CssClass="text-box" Width="93%"></asp:DropDownList> 
                </td> 
            </tr>
            <tr id="trMonth" style="display: none">
                <td style="text-align: left" colspan="5">
                    <div style="width: auto"> Month:<span style="color: #FF0000">*</span> <asp:RequiredFieldValidator ID="ReqMonth" ValidationGroup="valSelection"  runat="server" ErrorMessage="" ControlToValidate="ddlMonth" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> </asp:RequiredFieldValidator><asp:CompareValidator ID="CompareValMonth" runat="server" ErrorMessage="" ValueToCompare="1" ValidationGroup="valSelection" ControlToValidate="ddlMonth" Operator="GreaterThanEqual" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" >Invalid Selection</asp:CompareValidator></div>
                    <asp:DropDownList ID="ddlMonth" runat="server" CssClass="text-box" Width="93%" ></asp:DropDownList>                     
                </td>
            </tr>
             <tr id="trWeek" style="display: none">
                <td colspan="5">
                    <div style="width: auto">Week:<span style="color: #FF0000">*</span><asp:RequiredFieldValidator ID="ReqWeek" ValidationGroup="valSelection"  runat="server" ErrorMessage="" ControlToValidate="ddlWeekly" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator><asp:CompareValidator ID="CompareValWeekly" runat="server" ErrorMessage="" ValueToCompare="1" ValidationGroup="valSelection" ControlToValidate="ddlWeekly" Operator="GreaterThanEqual" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" >Invalid Selection</asp:CompareValidator></div>
                     <asp:DropDownList ID="ddlWeekly" runat="server" CssClass="text-box" Width = "93%" ></asp:DropDownList>
                </td>              
            </tr>
            <tr id="trDateRange" style="display: none"> 
                <td style="width: 40%">
                    <div style="width: auto"> From:<span style="color: #FF0000">*</span><asp:RequiredFieldValidator ID="ReqStartDate" ValidationGroup="valSelection"  runat="server" ErrorMessage="" ControlToValidate="txtStart" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> Required </asp:RequiredFieldValidator></div>
                    <asp:TextBox  runat="server" ID="txtStart" Width="93%" CssClass="text-box" ></asp:TextBox>
                </td>
                <td style="width: 40%">
                    <div style="width: auto"> To:<span style="color: #FF0000">*</span><asp:RequiredFieldValidator ID="ReqEndDate" ValidationGroup="valSelection"  runat="server" ErrorMessage="" ControlToValidate="txtEndDate" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> Required </asp:RequiredFieldValidator></div>  
                    <asp:TextBox   runat="server" ID="txtEndDate" CssClass="text-box" Width="82%"></asp:TextBox> 
                    <ajaxToolkit:calendarextender ID="CalendarExtFrom" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtStart"></ajaxToolkit:calendarextender>
                    <ajaxToolkit:calendarextender ID="CalendarExtTo" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtEndDate"></ajaxToolkit:calendarextender>  
                </td>
            </tr>
            <tr>
              <td colspan="5"><br/>
                  <div style="width: 21%; margin-left: 74%">
                     <asp:Button ID="btnGenerateReport"  runat="server" ValidationGroup="valSelection" Text="Generate Report" OnClick="BtnGenerateReportchClick" CssClass="customButton" Width="118px" />
                  </div>
              </td>
            </tr>
       </table>
     </fieldset>
   </div>
      <uc1:ConfirmAlertBox ID="ConfirmAlertBox1" runat="server" />
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
    <div runat="server" id="dvUserEntries" style="width: 100%;" class="gridDiv">
      <table style="width : 100%">
        <tbody>
            <tr class="divBackground3">
                <td style="width: 100%; text-align: left">
                    <asp:DataGrid ID="dgUserInitiatedTransactions" runat="server" AutoGenerateColumns="False" CellPadding="1" CellSpacing="1" GridLines="none" CssClass="xPlugTextAll_x" DataKeyField="ExpenseTransactionId" OnItemCommand ="DgUserInitiatedTransactionsItemCommand" ShowFooter="True" style="background: whitesmoke; width: 100%; max-width: 100%">
                        <FooterStyle CssClass="gridFooter" />
                            <AlternatingItemStyle CssClass="alternatingRowStyle"/>
                            <ItemStyle CssClass="gridRowItem" />
                            <HeaderStyle CssClass="gridHeader" />                            
                            <PagerStyle HorizontalAlign="Right"/>
                            <Columns>
                            <asp:TemplateColumn HeaderText="S/No.">
                            <HeaderStyle HorizontalAlign="center" Width="2%" CssClass="tdpadtop" />
                            <ItemStyle CssClass="xPlugTextAll_x" HorizontalAlign="center" VerticalAlign="Top" />
                                <ItemTemplate>
	                               <asp:Label ID="lblSNo" style ="text-align: center" runat="server" CssClass="xPlugTextAll" Text="<%# ((dgUserInitiatedTransactions.PageSize*dgUserInitiatedTransactions.CurrentPageIndex) + Container.ItemIndex + 1)%>">
	                            </asp:Label>
                                </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Transaction" >
                                    <HeaderStyle HorizontalAlign="left" Width="12%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" CssClass="xPlugTextAll_x" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblTransactionTitle" runat="server" CssClass="linkStyle" Text='<%# (DataBinder.Eval(Container.DataItem,"ExpenseTitle")) %>' CommandName="Edit" >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Requested By" >
                                    <HeaderStyle HorizontalAlign="Left" Width="8%" CssClass="tdpadtop"/>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" CssClass="xPlugTextAll_x"/>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequester"  runat="server" CssClass="xPlugTextAll_x" Text='<%#  GetUserFullName(int.Parse((DataBinder.Eval(Container.DataItem,"RegisteredById")).ToString()))%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Beneficiary" >
                                    <HeaderStyle HorizontalAlign="Left" Width="7%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"  CssClass="xPlugTextAll_x"/>
                                    <ItemTemplate>
                                        <asp:Label ID="lblBeneficiary"  runat="server" CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Beneficiary.FullName")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Date Requested" >
                                    <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" CssClass="xPlugTextAll_x"/>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequestedDate" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"TransactionDate")) %>' >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Time Requested" >
                                    <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" CssClass="xPlugTextAll_x"/>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequestedTime" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"TransactionTime")) %>' >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Approval Status" >
                                    <HeaderStyle HorizontalAlign="Left" Width="9%" CssClass="tdpadtop"/>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" CssClass="xPlugTextAll_x"/>
                                    <ItemTemplate>
                                        <asp:Label ID="lblApprovalStatus"  runat="server" CssClass="xPlugTextAll_x" Text='<%#  (DataBinder.Eval(Container.DataItem,"ApprovalStatus"))%>' ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Date Approved" >
                                    <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" CssClass="xPlugTextAll_x"/>
                                    <ItemTemplate>
                                        <asp:Label ID="lblApprovedDate" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"DateApproved")) %>' >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Time Approved" >
                                    <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" CssClass="xPlugTextAll_x"/>
                                    <ItemTemplate>
                                        <asp:Label ID="lblApprovedTime" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"TimeApproved")) %>' >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Approver's Comment" >
                                    <HeaderStyle HorizontalAlign="Left" Width="13%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" CssClass="xPlugTextAll_x"/>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkApproverComment"  runat="server" CssClass="commentLink" Text='View Comment' CommandName="viewComment"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Payment Status" >
                                    <HeaderStyle HorizontalAlign="Left" Width="12%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" CssClass="xPlugTextAll_x"/>
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserTransactionPaymentStatus"  runat="server" CssClass="xPlugTextAll_x" Text='<%#  (DataBinder.Eval(Container.DataItem,"PaymentStatus"))%>' ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Details" >
                                    <HeaderStyle HorizontalAlign="Left" Width="8%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" CssClass="xPlugTextAll_x"/>
                                    <ItemTemplate>
                                        <a id="lnkTransactionDetails<%#  (DataBinder.Eval(Container.DataItem,"ExpenseTransactionId"))%>" style ="color: darkcyan; font-weight: bold; cursor: pointer"  class="xPlugTextAll_x kkkxc" >View</a>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid> 
                    </td>
              </tr>
        </tbody>
    </table>
   </div>
  <%-- <input type="button" id="btnScrollTop" value="Go to Top" style="float: left; z-index: 1000"/> --%>
    <div  class="single-form-display" style="width:40%; border: 0 groove transparent; -ms-border-radius: 5px; border-radius: 5px; display: none" runat="server" id="dvRejection">
       <fieldset style="border-radius: 5px; border: 1px groove #038103">
			<legend runat="server" id="lgCommentTitle" style="">Transaction Rejection Comment</legend>
            <table style="width: 100%; height: 90%">
                <tr>
                   <td colspan="2" style="width: 100%; height: 98%">
                       <asp:TextBox  runat="server" id="txtRejComment" TextMode="MultiLine" Width = "100%" Height = "170px" CssClass="text-box" ></asp:TextBox>
                   </td> 
                </tr>
                <tr>
                    <td>
                         <div style="margin-left: 85%; width: 56px;">
                            <input type="button" class="customButton" style="width: 111%" value="Close" id="closerejection" /> 
                        </div>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <div > <asp:Button ID="btnPopUp" runat="server" Style="display: none" /></div>
     <ajaxToolkit:ModalPopupExtender ID="mpeExpenseItemsPopup" BackgroundCssClass="popupBackground"  TargetControlID="btnPopUp" CancelControlID="closerejection" PopupControlID="dvRejection" RepositionMode="RepositionOnWindowResizeAndScroll" runat="server"></ajaxToolkit:ModalPopupExtender>
   
  </div>
   <div id="dvReportTransactionItems" class="gridDiv" style="width: 100%; display: none; margin-top: 4%">
        <table style="width: 80%; margin-left: 10%; border-left: 1px solid  darkcyan; border-right: 1px solid  darkcyan" id="tbSxContainer">
            <tbody>
                <tr>
                    <td style="width: 100%">
                       <table style=" border-width: 1px; width: 99.9%">
                         <tbody>
                             <tr class="gridHeader">
                                 <td style="width: 30%">
                                     <div style="width: 100%; font-weight: bolder">
                                        &nbsp; <span id="lblTransactionTitle" style="width: 100%; color: #038103"></span>
                                     </div>
                                 </td>
                                 <td style="width: 60%">
                                     <table style="width: 100%">
                                         <tbody>
                                             <tr>
                                                 <td style="width: 50%">
                                                     <label class="infoLabel">Requested Total Amount:</label>
                                                     <label id="lblRequestedAmmount" style="width: 100%; color: #038103"></label>
                                                 </td>
                                                 <td style="width: 50%">
                                                     <label class="infoLabel"> Approved Total Amount:</label> 
                                                     <label id="lblApprovedTotalAmount" style="width: 100%; color: #038103"></label>
                                                 </td>
                                             </tr> 
                                         </tbody>
                                     </table>
                                 </td>
                             </tr>
                         </tbody>
                      </table>
                   </td>
                </tr>
               <tr>
                   <td class="tdpadd" style="width: 100%"><label style="width: 100%"><b>Transaction Items</b></label></td>
               </tr><tr><td style="width: 100%"><table id="tbldgExpenseItem" class="xPlugTextAll_x" cellspacing="1" cellpadding="1" border="0" style="width:100%;">
         <tbody><tr class="gridHeader"><th class="tdpadtop" align="center" style="width:2%;">S/No.</th><th class="tdpadtop" align="left" style="width:20%;">Expense Item</th><th class="tdpadtop" align="left" style="width:15%;">Description
         </th><th class="tdpadtop" align="left" style="width:10%;"> Requested Quantity</th><th class="tdpadtop" align="left" style="width:10%;">Approved Quantity
         </th><td class="tdpadtop" align="left" style="width:10%;">Requested Unit Price</td><th class="tdpadtop" align="left" style="width:10%;">Approved Unit Price
         </th><th class="tdpadtop" align="left" style="width:7%;text-align:center">Status</th></tr></tbody> </table></td></tr>
            </tbody> 
        </table>
    </div>  
    <div id="dvEXP" style="width: auto; height: auto; display: none"></div>
</div>

<script type="text/javascript">

    $(window).load(function ()
    {
        PeriodChanged();

    });

    $(document).ready(function ()
    {
        $('#<%=ddlDepartment.ClientID%>').on("change", function ()
        {
            ClearControls();
            PeriodChanged();
        });

        $('#<%=ddlPeriod.ClientID%>').on("change", function ()
        {
            ClearControls2();
            PeriodChanged();
        });

        $('.kkkxc').on('click', function ()
        {
            getReportData($(this).attr('id'));
        });
    });

    var ftxg = '';

    function getReportData(x)
    {
        var txxc = parseInt(x.replace('lnkTransactionDetails', ''));
        if (txxc < 1)
        {
            alert('Invalid Selection!');
            return;
        }
        
        //##################################################################################################

        if ($('#ctl00_MainContent_ctl00_dgUserInitiatedTransactions #trManjTemp' + txxc).length > 0)
            {

                if ($('#trManjTemp' + txxc).css('display') == 'table-row')
                {
                    $('#trManjTemp' + txxc).fadeOut('fast');
                    $('#lnkTransactionDetails' + txxc).text('View');
                    $('#lnkTransactionDetails' + txxc).attr('title', 'Expand...');
                }
                else
                {
                    $('#trManjTemp' + txxc).fadeIn().slideDown('slow');
                    $('#lnkTransactionDetails' + txxc).text('Collapse');
                    $('#lnkTransactionDetails' + txxc).attr('title', 'collapse');
                }
                
                return;
            }

       
        ftxg = txxc;

        var dtx = JSON.stringify({ "reportId": txxc });

        $.ajax
            ({
                url: "expenseManagerStructuredServices.asmx/GetReportData",
                data: dtx,
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                type: 'POST',
                success: generateDetails
            });
    }
    
    function PeriodChanged () 
    {
        var deptSel = parseInt($('#<%=ddlDepartment.ClientID%>').val());
            
        if (deptSel < 1) 
        {
            HideTrs2();
        }

        if (deptSel > 0)
        {
            $("#<%=ReqFilterOption.ClientID %>,#<%=CompareValFilterOption.ClientID %>").each(function()
            {
                 this.enabled = true;
            });
            $('#trPeriod').css({ "display": "table-row" });
        }
            
        var timeFrame = parseInt($('#<%=ddlPeriod.ClientID%>').val());

        if (timeFrame < 1)
        {
            HideTrs();
        }

        if (timeFrame == 1) 
        {
            $('#trYear').css({ "display": "table-row" });
            $('#trMonth').css({ "display": "table-row" });
            var validatorCollection1 = $("#<%=ReqYear.ClientID %>, #<%=CompareValYear.ClientID %>, #<%=ReqMonth.ClientID %>, #<%=CompareValMonth.ClientID %>");
            $.each(validatorCollection1, function ()
            {
                this.enabled = true;
            });

            var activeVal1 = $("#<%=ReqStartDate.ClientID %>,#<%=ReqEndDate.ClientID %>, #<%=ReqWeek.ClientID %>,#<%=CompareValWeekly.ClientID %>");
            $.each(activeVal1, function () 
            {
                this.enabled = false;
            });

            $('#trWeek').css({ "display": "none" });
            $('#trDateRange').css({ "display": "none" });
        }

        if (timeFrame == 2) {
            $('#trYear').css({ "display": "table-row" });
            $('#trMonth').css({ "display": "table-row" });
            $('#trWeek').css({ "display": "table-row" });
            var validatorCollection2 = $("#<%=ReqYear.ClientID %>, #<%=CompareValYear.ClientID %>, #<%=ReqMonth.ClientID %>, #<%=CompareValMonth.ClientID %>, #<%=ReqWeek.ClientID %>,#<%=CompareValWeekly.ClientID %>");
            $.each(validatorCollection2, function ()
            {
                this.enabled = true;
            });

            var activeVal2 = $("#<%=ReqStartDate.ClientID %>,#<%=ReqEndDate.ClientID %>");
            $.each(activeVal2, function ()
            {
                this.enabled = false;
            });

            $('#trDateRange').css({ "display": "none" });
        }
        
        if (timeFrame == 3) {
            $('#trMonth').css({ "display": "none" });
            $('#trWeek').css({ "display": "none" });
            $('#trYear').css({ "display": "none" });
            $('#trDateRange').css({ "display": "table-row" });
            var validatorCollection3 = $("#<%=ReqYear.ClientID %>, #<%=CompareValYear.ClientID %>, #<%=ReqMonth.ClientID %>, #<%=CompareValMonth.ClientID %>, #<%=ReqWeek.ClientID %>,#<%=CompareValWeekly.ClientID %>");
            $.each(validatorCollection3, function ()
            {
                this.enabled = false;
            });

            var activeVal = $("#<%=ReqStartDate.ClientID %>,#<%=ReqEndDate.ClientID %>");
            $.each(activeVal, function () 
            {
                this.enabled = true;
            });
        }
    }

    function HideTrs() 
    {
        $('#trYear').css({ "display": "none" });
        $('#trMonth').css({ "display": "none" });
        $('#trWeek').css({ "display": "none" });
        $('#trDateRange').css({ "display": "none" });
        DisableValidators();
    }

    function HideTrs2()
    {
        $('#trPeriod').css({ "display": "none" });
        $('#trYear').css({ "display": "none" });
        $('#trMonth').css({ "display": "none" });
        $('#trWeek').css({ "display": "none" });
        $('#trDateRange').css({ "display": "none" });
        DisableValidators();
    }

    function DisableValidators() 
    {
        var validatorCollection = $("#<%=ReqYear.ClientID %>, #<%=CompareValYear.ClientID %>, #<%=ReqMonth.ClientID %>, #<%=CompareValMonth.ClientID %>, #<%=ReqStartDate.ClientID %>,#<%=ReqEndDate.ClientID %>, #<%=ReqWeek.ClientID %>,#<%=CompareValWeekly.ClientID %>"); 
        $.each(validatorCollection, function () 
        {
            this.enabled = false;
        });
    }

    function ClearControls() 
    {
        $('#<%=ddlMonth.ClientID %>').val(0);
        $('#<%=ddlPeriod.ClientID %>').val(0);
        $('#<%=ddlWeekly.ClientID %>').val(0);
        $('#<%=ddlYear.ClientID %>').val(0);
        $('#<%=txtStart.ClientID %>').val('');
        $('#<%=txtEndDate.ClientID %>').val('');
      
    }

    function ClearControls2() 
    {
        $('#<%=ddlMonth.ClientID %>').val(0);
        $('#<%=ddlWeekly.ClientID %>').val(0);
        $('#<%=ddlYear.ClientID %>').val(0);
        $('#<%=txtStart.ClientID %>').val('');
        $('#<%=txtEndDate.ClientID %>').val('');

    }
    
    function generateDetails(retDataxx)
    {
        var retDatax =  $.parseJSON(retDataxx.d);

        if (retDatax.length < 1 || retDatax === '-1')
        {
            alert('Report Details could not be retrieved. Please Try again.');
            return;
        }
        
        var totalRequestQty = 0;
        var totalRequestUnitPrice = 0;
        var totalApprovedQty = 0;
        var totalApprovedUnitPrice = 0;

        if ($('#dvReportTransactionItems #grdReportFooter').length > 0)
        {
            $('#dvReportTransactionItems #grdReportFooter').remove();
        }

        //$("#dvReportTransactionItems #tbldgExpenseItem").find("tr:gt(0)").remove();

        $.each(retDatax, function (q, u) 
        {
            var dxx = u;
            totalRequestQty += dxx.RequestedQuantity;
            totalRequestUnitPrice += dxx.RequestedUnitPrice;

            totalApprovedQty += dxx.ApprovedQuantity;
            totalApprovedUnitPrice += dxx.ApprovedUnitPrice;
            
            var sts = '';

            if (dxx.Status === 1)
            {
                sts = 'Approved';
            }
            else 
            {
                if (dxx.ExpenseTransaction.Status < 1)
                {
                    sts = 'Pending';
                }
                if (dxx.ExpenseTransaction.Status === 1 || dxx.ExpenseTransaction.Status > 1)
                {
                    sts = 'Rejected';
                }
            }
            $('#dvEXP').empty();

            if (dxx.Ismultiple === true)
                {
                    $($('#dvReportTransactionItems #tbldgExpenseItem tbody:last').append($('<tr class="xc" id="txr' + dxx.TransactionItemId + '"><td class="xPlugTextAll_x" valign="top" style="width:2%;"><img src="../../Images/add2.png" id="exp' + dxx.TransactionItemId + '" style ="width: 13px; height: 13px; cursor: pointer; background:darkcyan; border-radius:7px; border:none" onclick="expandX(this.id)" title="Expand..."/>&nbsp;</td>'
                        + '<td valign="top" align="left" style="width:20%;"><span id="lblExpenseItemTitle" class="xPlugTextAll_x" style="font-weight: bold; color:black">' + dxx.ExpenseItem.Title + '</span></td><td valign="top" align="left" style="width:15%;">'
                        + '<span id="lblDescription" class="xPlugTextAll_x" style="font-weight: bold; color:black"></span></td><td valign="top" style="width:10%; text-align: center"><span id="lblQuantity" class="xPlugTextAll_x" style="font-weight: bold; color:black"></span> </td>'
                        + '<td valign="top" style="width:10%; text-align: center"><span id="lblApprovedQuantity" class="xPlugTextAll_x" style="font-weight: bold; color:black"></span></td><td valign="top" style="width:10%; text-align: center">'
                        + '<span id="lblUnitPriceRT" class="xPlugTextAll_x" style="font-weight: bold; color:black"></span> </td><td valign="top" style="width:10%; text-align: center"><span id="lblApprovedUnitPriceRT class="xPlugTextAll_x" style="font-weight: bold; color:black"></span>'
                        + '</td><td valign="top" style="width:7%"><span id="lblStatus" class="xPlugTextAll_x"></span></td></tr>')));
                }
                else
                {

                    var sdx = (q + 1); 
                    $($('#dvReportTransactionItems #tbldgExpenseItem tbody:last').append($('<tr class="xcR klxx" id="txr' + dxx.TransactionItemId + '"><td class="xPlugTextAll_x" valign="top" style="width:2%;">' + sdx + '</td>'
                    + '<td valign="top" style="width:20%;"><span id="lblExpenseItemTitle" class="xPlugTextAll_x" >' + dxx.ExpenseItem.Title + '</span></td><td valign="top" align="left" style="width:15%;">'
                    + '<span id="lblDescription" class="xPlugTextAll_x">' + dxx.Description + '</span></td><td valign="top" style="width:10%; text-align: center"><span id="lblQuantity" class="xPlugTextAll_x">' + dxx.RequestedQuantity + ' </span> </td>'
                    + '<td valign="top" style="width:10%; text-align: center"><span id="lblApprovedQuantity" class="xPlugTextAll_x">' + dxx.ApprovedQuantity + '</span></td><td valign="top" align="left" style="width:10%; text-align: center">'
                    + '<span id="lblUnitPriceRT' + sdx + '" class="xPlugTextAll_x">' + dxx.RequestedUnitPrice + '</span> </td><td valign="top" style="width:10%; text-align: center"><span id="lblApprovedUnitPriceRT' + sdx + '" class="xPlugTextAll_x">' + dxx.ApprovedUnitPrice + '</span>'
                    + '</td><td valign="top" align="left" style="width:7%; text-align: center"><span id="lblStatus" class="xPlugTextAll_x">' + sts + '</span></td></tr>')));
                }

                 

                $('#dvReportTransactionItems #lblUnitPriceRT' + sdx).formatCurrency({ symbol: '', roundToDecimalPlace: 2 });
                $('#dvReportTransactionItems #lblApprovedUnitPriceRT' + sdx).formatCurrency({ symbol: '', roundToDecimalPlace: 2 });

            });

            $('#dvReportTransactionItems #lblTransactionTitle').text(retDatax[0].ExpenseTransaction.ExpenseTitle).css({'color':'darkcyan'});
            $('#dvReportTransactionItems #lblRequestedAmmount').text(retDatax[0].ExpenseTransaction.TotalTransactionAmount).css({ 'color': 'darkcyan' });
            $('#dvReportTransactionItems #lblApprovedTotalAmount').text(retDatax[0].ExpenseTransaction.TotalApprovedAmount).css({ 'color': 'darkcyan' });

            $('#dvReportTransactionItems #lblRequestedAmmount').formatCurrency({ symbol: 'N', roundToDecimalPlace: 2 });
            $('#dvReportTransactionItems #lblApprovedTotalAmount').formatCurrency({ symbol: 'N', roundToDecimalPlace: 2 });

            $($('#dvReportTransactionItems #tbldgExpenseItem tbody:last').append($('<tr class="gridFooter" id="grdReportFooter"><td  style="width:2%"></td><td  style="width:20%"><span id="lblTotal" style="font-weight:normal;">'
            + 'Total</span></td><td  style="width:15%"></td><td style="width:10%; text-align: center"><span id="lblTotalQuantityGT' + ftxg + '" style="font-weight:normal;">' + totalRequestQty
            + '</span></td><td style="width:10%; text-align: center"><span id="lblTotalApprovedQuantityGT' + ftxg + '" style="font-weight:normal;">' + totalApprovedQty + '</span></td><td style="width:10%; text-align: center">'
            + '<span id="lblTotalUnitPriceGT' + ftxg + '" style="font-weight:normal;">' + totalRequestUnitPrice + '</span></td><td style="width:10%; text-align: center"><span id="lblTotalApprovedUnitPriceGT' + ftxg + '" style="font-weight:normal;">' + totalApprovedUnitPrice
            + '</span></td><td></td></tr>')));

            $('#lblTotalUnitPriceGT' + ftxg).formatCurrency({ symbol: 'N', roundToDecimalPlace: 2 });
            $('#lblTotalApprovedUnitPriceGT' + ftxg).formatCurrency({ symbol: 'N', roundToDecimalPlace: 2 });  

            $('#dvReportTransactionItems #tbldgExpenseItem tr:even').find("tr:gt(0)").addClass('gridRowItem');
            $('#dvReportTransactionItems #tbldgExpenseItem tr:odd').find("tr:gt(0)").addClass('alternatingRowStyle');
        
//$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
           
            $('#lnkTransactionDetails' + ftxg).closest('tr').after($('<tr id="trManjTemp' + ftxg + '" style="background: whitesmoke"><td colspan= "12" id="tdManjTemp' + ftxg + '" style="background: whitesmoke"></td></tr>'));
            $('#tdManjTemp' + ftxg).append($('#dvReportTransactionItems').html());
            $('#trManjTemp' + ftxg).css({ 'border-top': '1px solid darkcyan', 'border-bottom': '1px solid darkcyan' });
            $('#trManjTemp' + ftxg).fadeIn().slideDown('slow'); 
            $('#lnkTransactionDetails' + ftxg).text('Collapse');
            $('#lnkTransactionDetails' + ftxg).attr('title', 'collapse');
            $('#dvReportTransactionItems #tbldgExpenseItem').find("tr:gt(0)").remove();
           
        }

       var tcd = '';

        function expandX(xxg) 
        {
            //if the detail has already been retrieved, let no request be made to the server again

            var dzx = parseInt(xxg.replace('exp', ''));
            if (dzx < 1)
            {
                alert('Invalid Selection!');
                return;
            }

            tcd = dzx;

            if ($('#tbltemp' + dzx).length > 0) 
            {
                if ($('#tdk' + dzx).css('display') != 'none')
                {
                    $('#tdk' + dzx).fadeOut('fast');
                    $('#exp' + dzx).attr('src', '../../Images/add2.png');
                    $('#exp' + tcd).attr('title', 'Expand...');
                }
                else
                {
                    $('#tdk' + dzx).fadeIn().slideDown('slow');
                    $('#exp' + dzx).attr('src', '../../Images/minus2.png');
                    $('#exp' + tcd).attr('title', 'collapse');
                }
                
                return;
            }

            var vxc = JSON.stringify({ jxl: dzx }); 
           
            $.ajax
            ({
                url: "expenseManagerStructuredServices.asmx/GetDetailedReportData",
                data: vxc,
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                type: 'POST',
                success: sxvt
            });
        }
        

        function sxvt(jq) 
        {
            var dk = $.parseJSON(jq.d);
            if (dk.length < 1)
            {
                alert('Report Details could not be retrieved. Please try again later.');
                return;
            }

            $('#dvEXP').empty();

            $('#dvEXP').append($('<table id="tbltemp' + tcd + '" style="background: whitesmoke; width: 100%"><tr></tr></table>'));
            
          for (var hx = 0; hx < dk.length; hx++)
            {
                var dxx = dk[hx];
              
                var sts = '';

                if (dxx.Status === 1)
                {
                    sts = 'Approved';
                }
                else 
                {
                    if (dxx.ExpenseTransaction.Status < 1)
                    {
                        sts = 'Pending';
                    }
                    if (dxx.ExpenseTransaction.Status === 1 || dxx.ExpenseTransaction.Status > 1)
                    {
                        sts = 'Rejected';
                    }
                }
              
//                var bv = '';

//                if ($('#tbltemp' + tcd + ' tr.klx').length < 1)
//                {
//                    bv = $('#trManjTemp' + ftxg + ' #tbldgExpenseItem tr.xcR').length + 1;
//                }
                //                else 
//                {
                   // bv = $('#trManjTemp' + ftxg + ' #tbldgExpenseItem tr.xcR').length + $('#tbltemp' + tcd + ' tr.klx').length + 1;
//                }

                $($('#tbltemp' + tcd + ' tbody:last').append($('<tr class="gridRowItem klxx" id="txr' + dxx.TransactionItemId + '"><td class="xPlugTextAll_x" valign="top" style="width:2%; padding: 0px 5px"></td>'
                        + '<td valign="top" style="width:22%;"><span id="lblExpenseItemTitle" class="xPlugTextAll_x" ><img src="Images/checkmark5.png" style= "width: 14px; height: 14px; border:none; margin-left:14%" /></span></td>'
                        + '<td valign="top" align="left" style="width:15%;">'
                        + '<span id="lblDescription">' + dxx.Description + '</span></td><td valign="top" style="width:10%; text-align: center"><span id="lblQuantity" class="xPlugTextAll_x">' + dxx.RequestedQuantity + ' </span> </td>'
                        + '<td valign="top" align="left" style="width:10%; text-align: center"><span id="lblApprovedQuantity" class="xPlugTextAll_x">' + dxx.ApprovedQuantity + '</span></td><td valign="top" align="center" style="width:10%; text-align: center">'
                        + '<span id="lblUnitPrice' + dxx.TransactionItemId + '" class="xPlugTextAll_x">' + dxx.RequestedUnitPrice + '</span> </td><td valign="top" style="width:10%; text-align: center"><span id="lblApprovedUnitPrice' + dxx.TransactionItemId + '" class="xPlugTextAll_x">' + dxx.ApprovedUnitPrice + '</span>'
                        + '</td><td valign="top" style="width:7%; text-align: center"><span id="lblStatus" class="xPlugTextAll_x">' + sts + '</span></td></tr>')));

                        $('#lblUnitPrice' + dxx.TransactionItemId).formatCurrency({ symbol: '', roundToDecimalPlace: 2 });
                        $('#lblApprovedUnitPrice' + dxx.TransactionItemId).formatCurrency({ symbol: '', roundToDecimalPlace: 2 });
            }

            $('#tbldgExpenseItem #txr' + tcd).after($('<tr id="txr' + tcd + '"><td colspan="8" id="tdk' + tcd + '"></td></tr>'));
            $('#tdk' + tcd).append($('#dvEXP').html());

            $('#trManjTemp' + ftxg + ' tr.klxx').each(function (k)
            {
                $(this).children('td.xPlugTextAll_x').text(k + 1);
            });

            $('#tdk' + tcd).fadeIn().slideDown('slow');
            $('#exp' + tcd).attr('src', '/Images/minus2.png');
            $('#exp' + tcd).attr('title','collapse');
            $('#dvEXP').empty();
         }

</script>