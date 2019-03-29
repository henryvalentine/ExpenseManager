<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="frmCompleteExpensePayment.ascx.cs" Inherits="ExpenseManager.ExpenseMgt.FrmCompleteExpensePayment" %>
<%@ Register src="../CoreFramework/AlertControl/ConfirmAlertBox.ascx" tagname="ConfirmAlertBox" tagprefix="uc1" %>
<%@ Register TagPrefix="uc2" TagName="ErrorDisplay_1_1" Src="~/CoreFramework/ErrorControl/ErrorDisplay.ascx" %>
<link href="../App_Themes/Default/ControlStyles.css" rel="stylesheet" type="text/css" />
 
<div class="dvContainer">
       
    <div> <asp:Button ID="btnPopUp" runat="server" Style="display: none" /></div>
    <h2 runat="server" id="hTitle" > Update or Complete Transaction Payments</h2>
    <uc1:ConfirmAlertBox ID="ConfirmAlertBox1" runat="server" />
    <ajaxToolkit:ModalPopupExtender ID="mpeSelectDateRangePopup" BackgroundCssClass="popupBackground"  TargetControlID="btnPopUp" CancelControlID="btnReset" PopupControlID="dvExpensePayment" RepositionMode="RepositionOnWindowResizeAndScroll" runat="server"></ajaxToolkit:ModalPopupExtender>
    <div id="divBeneficiary" class="single-form-display"  runat="server" style="border: 1px solid #ccc; margin-top: 2%;">
        <label class="label2">Select a Beneficiary</label>&nbsp; &nbsp;<asp:DropDownList   ID="ddlBeneficiaries" AutoPostBack="True" OnSelectedIndexChanged="DdlBeneficiariesIndexChanged" runat="server" />
    </div>
    <div class="single-form-display" style="width:53%; border: 0 groove transparent; border-radius: 5px; display: none" runat="server" id="dvChequePayment" >

       <fieldset>
			<legend class="label" runat="server" id="lgChequeUpdate"><b>Transaction Details</b></legend>

        <div id="divErrorDispChequePayment" style="width: 98%"><uc2:ErrorDisplay_1_1 ID="ErrorDispChequePayment" runat="server"/></div>
      
            <table style="width:100%; border: none" >
                <tr>
                   <td class="tdpad">
                      <div><i>Total Payable Amount</i></div>
					  <div> <asp:TextBox ID="txtChequeTotalPayableAmount" Enabled="False" ClientIDMode="Static" runat="server" Height="22px"  Width="97%" CssClass="text-box"></asp:TextBox></div>
                    </td>
                     <td class="tdpad">
                      <div><i>Balance</i></div>
					  <div> <asp:TextBox ID="txtOldChequeBalance" runat="server" ClientIDMode="Static" Enabled="False" Width="97%" Height="22px" CssClass="text-box"></asp:TextBox></div>
                   </td>
                </tr>
				<tr>
				   <td colspan="2" style="width: 100%">
				        <fieldset ><legend><b>Payment Details</b></legend>
                            <table style="border-style: none; border-color: inherit; border-width: medium; width:100%; height: 171px;" runat="server" id="Table2">
                                <tr>
                                    <td colspan="2">
                                        <table style="width: 100%; height: 190px; ">
                                            <tr>
                                               <td style="width: 50%" class="tdpad">
                                                   <div style="vertical-align: top"><i>Bank</i><asp:RequiredFieldValidator  ValidationGroup="valUpdateChequePayment" ID="ReqBank"  runat="server" ErrorMessage="* Required" ControlToValidate="ddChequeBank" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator><asp:CompareValidator ID="CompareValidator1"  ValidationGroup="valUpdateChequePayment" runat="server" ErrorMessage="* Invalid Selection" ValueToCompare="1" ControlToValidate="ddChequeBank" Operator="GreaterThanEqual" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" ></asp:CompareValidator></div>
                                                  <select style="width: 100%;" id="ddChequeBank" runat="server" class="text-box"/>
                                               </td>
                                               <td style="width: 50%" rowspan="4" class="tdpad">
                                                  <div style="width: 100%; margin-left: 2%;  vertical-align: top">
                                                       <div style="width: 96%;">
                                                       <img src="" alt="Cheque Copy" id="imgChequeUpdate" style="width: 102%; vertical-align: top;" /> 
                                                       <div style="width: 246px"><i>New Cheque Copy</i>(<span style="color: Red">*jpeg/jpg only. 500KB Maximum</span>)</div>
                                                        <asp:FileUpload id="updateFileUploadControl" ClientIDMode="Static" class="text-box" Width=" 97%" runat="server"/> 
                                                    </div>
                                                 </div> 
                                               </td> 
                                            </tr>
                                            <tr>
                                                <td class="tdpad">
                                                  <div ><i>Cheque Number</i><asp:RequiredFieldValidator  ValidationGroup="valUpdateChequePayment" ID="RequChequeNo"  runat="server" ErrorMessage="* Required" ControlToValidate="txtChequNo" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator> </div>
                                                  <input type="text" id="txtChequNo" class="text-box" style="width: 97%;" runat="server"/>                                                   
                                               </td>
                                            </tr>
                                            <tr>
                                                <td class="tdpad">
                                                   <div><i>Amount to Pay</i> <asp:RequiredFieldValidator  ValidationGroup="valUpdateChequePayment" ID="RequChequeAmount"  runat="server" ErrorMessage="* Required" ControlToValidate="txtChequeAmountToPay" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExChequeAmountToPay" runat="server" ControlToValidate="txtChequeAmountToPay" Display="Dynamic" ErrorMessage="* Invalid Entry" Font-Size="8pt" ForeColor="Red" SetFocusOnError="True" ValidationExpression="^(\d|,)*\.?\d*$" ValidationGroup="valUpdateChequePayment"></asp:RegularExpressionValidator></div>
                                                  <input type="text" id="txtChequeAmountToPay" ClientIDMode="Static" class="text-box" style="width: 97%;" runat="server" onkeyup="return calaculateChequeBalance()" onchange="return calaculateChequeBalance()"/>
                                               </td>
                                            </tr>
                                            <tr>
                                                <td class="tdpad" >
                                                   <div><i runat="server" id="i2">&nbsp;Balance</i></div>
					                                <div> <input type="text" id="txtChequeNewBalance" disabled="disabled" style="width: 97%;" class="text-box"/></div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="tdpad" >
                                        <div><i>Comment</i><asp:RequiredFieldValidator  ValidationGroup="valUpdateChequePayment" ID="ReqUpdateChequePayment"  runat="server" ErrorMessage="* Required" ControlToValidate="txUpdateChequePaymentChequeComment" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator></div>
                                        <div style="width: 97%; height: 107px;">
                                            <asp:TextBox runat="server" ID="txUpdateChequePaymentChequeComment" TextMode="MultiLine" ClientIDMode="Static" Width="100%" Rows="6" Height="100%"></asp:TextBox>
                                        </div>
                                    </td>
                                 </tr>
                                 <tr>
                                     <td style="width: 50%">
                                         
                                     </td>
                                    <td style="width: 50%">
                                       <div style="width: 60%; margin-left: 30%; margin-top: 10%">
                                         <asp:Button ID="btnUpdatePayment" runat="server" Text="Update" OnClick="BtnUpdatePaymentClick" ValidationGroup="valUpdateChequePayment"  CssClass="customButton" CommandArgument="1"  Width="97px" />&nbsp;
					                     <asp:Button ID="btnChequeClose" runat="server" Text="Close" class="customButton" OnClick="BtnChequeCloseClick" Width="79px" /> 
                                     </div>
                                </td>
                            </tr>
                         </table>
                      </fieldset>
                   </td>
			   </tr>
           </table>
        </fieldset>
    </div>
    <div class="single-form-display" style="width:53%; border: 0 groove transparent; border-radius: 5px; display: none" runat="server" id="dvExpensePayment" >
       <fieldset style="">
			<legend class="label" runat="server" id="lgUpdatePayment"><b>Transaction Details</b></legend>
            <div id="div1" style="width: 98%"><uc2:ErrorDisplay_1_1 ID="ErrorDisplayCashPayment" runat="server"/></div>
            <table style="width:100%; border: none" id="tblCreateTransaction">
                <tr>
                   <td class="tdpad">
                      <div><i>Total Payable Amount</i></div>
					  <div> <asp:TextBox ID="txtTotalPayableAmount" Enabled="False" ClientIDMode="Static" runat="server" Width="98%" CssClass="text-box"></asp:TextBox></div>
                    </td>
                     <td class="tdpad">
                      <div><i>Old Balance</i></div>
					  <div> <asp:TextBox ID="txtOldBalance" runat="server" ClientIDMode="Static" Enabled="False" Width="98%" CssClass="text-box"></asp:TextBox></div>
                   </td>
                </tr>
				<tr>
				   <td colspan="2" style="width: 100%" >
				        <fieldset style=""><legend><b>Payment</b></legend>
                            <table style="border-style: none; border-color: inherit; border-width: medium; width:100%; height: 171px;" 
                                runat="server" id="tblApproveTransaction">
                                <tr>
                                    <td style="width: 50%" class="tdpad">
                                        <div><i>Amount to Pay</i> <asp:RequiredFieldValidator  ValidationGroup="valUpdatePayment" ID="ReqAmountPaid"  runat="server" ErrorMessage="* Required" ControlToValidate="txtUpdateAmount" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator> <asp:RegularExpressionValidator ID="RegularExpAmountPaid" runat="server" ControlToValidate="txtUpdateAmount" Display="Dynamic" ErrorMessage="* Invalid Entry" Font-Size="8pt" ForeColor="Red" SetFocusOnError="True" ValidationExpression="^(\d|,)*\.?\d*$" ValidationGroup="valUpdatePayment"></asp:RegularExpressionValidator></div>
					                    <div><input class="text-box" ClientIDMode="Static" id="txtUpdateAmount" onkeyup="return calaculateBalance()" onchange="return calaculateBalance()" runat="server" style="" type="text" /></div>
                                    </td>
                                    <td style="width: 50%" class="tdpad">
                                       <div><i>&nbsp;New Balance</i></div>
                                        <div><input type="text" id="txtNewBalance" disabled="disabled" style="" class="text-box"/></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="tdpad" >
                                        <div><i>Comment</i><asp:RequiredFieldValidator  ValidationGroup="valUpdatePayment" ID="RequComment"  runat="server" ErrorMessage="* Required" ControlToValidate="txtUpdatePaymentComment" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator></div>
                                        <div style="height: 100%">
                                            <asp:TextBox runat="server" ID="txtUpdatePaymentComment" TextMode="MultiLine" 
                                                ClientIDMode="Static" Rows="6" Width="98%" Height="15%"></asp:TextBox>
                                        </div>
                                    </td>
                                 </tr>
                                 <tr>
                                     <td style="width: 50%">
                                         
                                     </td>
                                    <td style="width: 50%; text-align: right">
                                       <div style="">
                                         <asp:Button ID="btnSubmit" runat="server" Text="Update" OnClick="BtnUpdatePaymentClick" ValidationGroup="valUpdatePayment"  CssClass="customButton" CommandArgument="1"  Width="97px" />&nbsp;
					                     <asp:Button ID="btnReset" runat="server" Text="Close" class="customButton" Width="79px" /> 
                                     </div>
                                </td>
                            </tr>
                         </table>
                      </fieldset>
                   </td>
			   </tr>
           </table>
        </fieldset>
    </div>	
    <div runat="server" id="divPaymentTrack" style="margin-top: 20px">
    
    <table style="width: 100%"  class="single-form-display">
        <tr>
            <td style="width: 80%" class="tdpaddleft"> 
              <asp:LinkButton runat="server" ID="lnkNewBeneficiary" ForeColor="#038103" CssClass="prevbtn" Text="<< Select a different Beneficiary" OnClick="LnkSelectNewBeneficiaryClick"></asp:LinkButton>  
            </td>
            <td style="width: 20%" class="tdpad">
          
                <asp:DropDownList runat="server" ID="ddlPaymentMode"   />
                
            </td>
        </tr>
    </table>
    <div style="width: 100%; margin-top: 5px; " class="gridDiv" >
        <table style="width: 100%; height: 5px">
            <tr>
                <td >
                    <table style=" width: 100%">
                        <tr class="divBackground">
                            <td style="width: 100%" class="tdpadd">
                              <div style="width: 100%">
                                  <label class="label"> Uncompleted Transaction Payment(s) for</label>&nbsp;
                                  <label runat="server" id="hBeneficiary" style=" font-size: 1.2em; width: auto; margin-left:0; font-weight: bolder; color: #038103">Beneficiary</label>
                              </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
               <td colspan="2" style="width: 100%">
                   <asp:DataGrid ID="dgBeneficiaryPaymentTrack" runat="server" AutoGenerateColumns="False" CellPadding="1"   CellSpacing="1" GridLines="none" 
                        CssClass="xPlugTextAll_x"  DataKeyField="ExpenseTransactionPaymentId" ShowFooter="True" Width="100%" OnItemCommand="DgBeneficiaryPaymentTrackCommand">
                        <FooterStyle CssClass="gridFooter" />
                        <AlternatingItemStyle CssClass="alternatingRowStyle"  />
                        <ItemStyle CssClass="gridRowItem" />
                        <HeaderStyle CssClass="gridHeader" />
                        <Columns>
                            <asp:TemplateColumn HeaderText="S/No.">
                                <HeaderStyle HorizontalAlign="center" Width="1%" CssClass="tdpadtop" />
                                <ItemStyle CssClass="xPlugTextAll_x" HorizontalAlign="center" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblSNo" runat="server" CssClass="xPlugTextAll_x" Text="<%# ((dgBeneficiaryPaymentTrack.PageSize*dgBeneficiaryPaymentTrack.CurrentPageIndex) + Container.ItemIndex + 1)%>"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Transaction" >
                                <HeaderStyle HorizontalAlign="left" Width="20%" CssClass="tdpadtop" />
                                <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblTransaction" runat="server" ssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"ExpenseTransaction.ExpenseTitle")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Transaction Date" >
                                <HeaderStyle HorizontalAlign="left" Width="7%" CssClass="tdpadtop" />
                                <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblTransactionDate" runat="server" CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"ExpenseTransaction.TransactionDate")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Transaction Time" >
                                <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop"/>
                                <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblPaymentTime" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"ExpenseTransaction.TransactionTime")) %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Total Payable Amount" >
                                <HeaderStyle HorizontalAlign="Left" Width="8%" CssClass="tdpadtop" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalAmount" runat="server"  CssClass="xPlugTextAll_x" Text='<%#  (DataBinder.Eval(Container.DataItem,"TotalAmountPayable")) %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Amount Already Paid" >
                                <HeaderStyle HorizontalAlign="left" Width="8%" CssClass="tdpadtop" />
                                <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblViewTransactionHistory" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"AmountPaid")) %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Balance" >
                                <HeaderStyle HorizontalAlign="left" Width="5%" CssClass="tdpadtop" />
                                <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:label ID="lblAmountPaid" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Balance")) %>' ></asp:label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Payment Status" >
                                <HeaderStyle HorizontalAlign="Left" Width="15%" CssClass="tdpadtop" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblTransactionStatus" runat="server"  CssClass="xPlugTextAll_x" Text='<%#  ((DataBinder.Eval(Container.DataItem,"Balance")).ToString() == "0")? "Fully Paid" : "Partly Paid" %>' ></asp:Label>
                                </ItemTemplate>
                          </asp:TemplateColumn>
                          <asp:TemplateColumn HeaderText="Action" >
                                <HeaderStyle HorizontalAlign="Left" Width="12%" CssClass="tdpadtop" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkTransactionPayment" runat="server" ForeColor="darkcyan" Text='Update/Complete Payment'  CommandName="CompletePayment"></asp:LinkButton>
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
                        <span style=" font-weight: bold; margin-left: 30%"><span>Items Per Page&nbsp;&nbsp;</span><asp:DropDownList 
                                CssClass="span1" runat="server" AutoPostBack="True"  OnSelectedIndexChanged="OnLimitChange" ID="ddlLimit" Width="163px"/></span>  
                        </td>
                       </tr>
                     </table> 
               </td> 
            </tr>
        </table>
    </div>
  </div>
    
     <script type="text/javascript">

         function calaculateChequeBalance() 
         {
             $('#txtChequeAmountToPay').formatCurrency({ symbol: '', roundToDecimalPlace: 0 });
             var newBalanceControl = $get('txtChequeNewBalance');
             var newPaymentValue = $get('<%=txtChequeAmountToPay.ClientID%>').value;
             var totalAmount = $get('<%=txtOldChequeBalance.ClientID%>').value;
             if (newPaymentValue === null || newPaymentValue == "") {
                 newBalanceControl.value = "";
             }

             else {
                 var newVal = parseFloat(totalAmount) - parseFloat(newPaymentValue.replace(',', ''));
                 newBalanceControl.value = newVal;
                 $('#txtChequeNewBalance').formatCurrency({ symbol: '', roundToDecimalPlace: 0 });
             }

             return false;
         }

         function calaculateBalance() {
             $('#txtUpdateAmount').formatCurrency({ symbol: '', roundToDecimalPlace: 0 });
             var totalAmountControl = document.getElementById("<%=txtOldBalance.ClientID %>");
             var paymentControl = document.getElementById("<%=txtUpdateAmount.ClientID %>");
             var balanceControl = document.getElementById("txtNewBalance");
             var paymentValue = paymentControl.value;
             var totalAmount = totalAmountControl.value;
             if (paymentValue === null || paymentValue == "") 
             {
                 balanceControl.value = "";
                 
             }

             else {
                 var newVal = parseFloat(totalAmount) - parseFloat(paymentValue.replace(',', ''));
                 balanceControl.value = newVal;
                 $('#txtNewBalance').formatCurrency({ symbol: '', roundToDecimalPlace: 0 });
                 
             }

             return false;
         }


         $(document).ready(function () {
             readURL();

         });

         function readURL() {
             $('#updateFileUploadControl').change(function (event) 
             {
                 var input = event.target.files;

                 if (input.length > 0) {
                     var isValid = /\.jpg?$/i.test($get('<%=updateFileUploadControl.ClientID %>').value);

                     if (!isValid) 
                     {
                         alert('Please select only a jpeg/jpg file!');
                         $get('<%=updateFileUploadControl.ClientID %>').value = '';
                         return false;
                     }

                     if (input[0].size > 500000) {
                         alert("Receipt size should NOT be more than 500KB");
                         input.value = '';
                         return false;
                     }
                     
                     else 
                     {
                         var reader = new FileReader();
                         reader.onload = function (e) {
                             $get('imgChequeUpdate').src = e.target.result;

                         };

                         reader.readAsDataURL(input[0]);
                     }

                 }
                 else {
                     alert("Please select the Cheque copy to be attached!");
                 }
                 return false;
             });
         }
       
   </script>
    <!--[if IE]>
<script src="http://html5shiv.googlecode.com/svn/trunk/html5.js"></script>
<![endif]-->
 </div>