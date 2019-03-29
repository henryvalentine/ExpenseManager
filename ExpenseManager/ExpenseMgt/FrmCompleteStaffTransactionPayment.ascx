<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FrmCompleteStaffTransactionPayment.ascx.cs" Inherits="ExpenseManager.ExpenseMgt.FrmCompleteStaffTransactionPayment" %>
<link href="../App_Themes/Default/ControlStyles.css" rel="stylesheet" type="text/css" />
<%@ Register TagPrefix="uc2" TagName="ErrorDisplay_1" Src="~/CoreFramework/ErrorControl/ErrorDisplay.ascx" %>
 
<div class="dvContainer">
       <style type="text/css">
        .customNewCheckbox
       {
           text-align: center;
            float: right;
            margin-left: 60%;
            width: auto;
            top: 5px;
        }
       
          .style2
          {
              width: 371px;
          }
        
           .text-box
           {}
        
          </style>
    <div> <asp:Button ID="btnPopUp" runat="server" Style="display: none" /></div>
    <h2 runat="server" id="hTitle" style="font-family: 'OCR A Extended', arial;" > Update or Complete Staff Transaction Payments</h2>
	<div style="padding-bottom: 10px; padding-top: 5px"><asp:Panel ID="Panel2" runat="server" Width="98%"><uc2:ErrorDisplay_1 ID="ErrorDisplay1" runat="server" /></asp:Panel></div>
    <ajaxToolkit:ModalPopupExtender ID="mpeSelectDateRangePopup" BackgroundCssClass="popupBackground"  TargetControlID="btnPopUp" CancelControlID="btnReset" PopupControlID="dvExpensePayment" RepositionMode="RepositionOnWindowResizeAndScroll" runat="server"></ajaxToolkit:ModalPopupExtender>
    <div id="divBeneficiary" class="single-form-display" style="width:20%; margin-left: 30%; margin-top: 4%; border: .5px groove #038103; border-radius: 5px" runat="server">
         <asp:DropDownList CssClass="text-box" ID="ddlBeneficiaries" AutoPostBack="True" 
             OnSelectedIndexChanged="DdlBeneficiariesIndexChanged" runat="server" 
             Height="24px" Width="256px"/>
    </div>
    <div class="single-form-display" style="width:53%; border: 0 groove transparent; border-radius: 5px; display: none" runat="server" id="dvExpensePayment" >
       <asp:Panel ID="PanelCreateTransactions" runat="server" Width="98%"><uc2:ErrorDisplay_1 ID="ErrorDisplayCompletePayment" runat="server" /></asp:Panel>
       <fieldset style="border-radius: 5px; border: 1px groove #038103">
			<legend style="color: #038103; font-family:" runat="server" id="lgTransactionTitle">Expense Transaction Details</legend>
            <table style="width:100%; border: none" runat="server" id="tblCreateTransaction">
                 <tr>
                    <td class="style6">
                         <div><i>Transaction</i></div>
					    <div style="width: 314px"> <asp:TextBox ID="txtTitle" Enabled="False" runat="server"  Width="314px" CssClass="text-box"></asp:TextBox></div>
				    </td>
                    <td style="width:50%">
                        <div><i>Category</i></div>
                        <div> <asp:TextBox Enabled="False" ID="txtExpenseCategory" Width="314px" CssClass="text-box" runat="server"></asp:TextBox></div>
					</td>
                </tr>
                <tr>
                    <td class="style6">
					   <div><i>Expense Item</i></div>
						<div> <asp:TextBox Enabled="False"  ID="txtExpenseItem" CssClass="text-box"  Width="314px" runat="server"></asp:TextBox> </div>
					</td>
                    <td>
                        <div><i>Approved Quantity</i></div>
					    <div> <asp:TextBox ID="txtApprovedQuantity" Enabled="False"  runat="server"  Width="314px" CssClass="text-box"></asp:TextBox></div>
                    
                    </td>
                </tr>
                <tr>
                     <td>
                        <div><i>Approved Unit Price</i></div>
					    <div> <asp:TextBox ID="txtApprovedUnitPrice" runat="server" Enabled="False"   Width="314px" CssClass="text-box"></asp:TextBox></div>
                    </td>
                    <td>
                        <div><i>Approved Total Amount</i></div>
					    <div> <asp:TextBox ID="txtApprovedTotalAmount" runat="server" Enabled="False"  Width="314px" CssClass="text-box"></asp:TextBox></div>
                   </td>
                </tr>
                <tr>
                    <td>
                        <div><i>Approved By</i></div>
					    <div> <asp:TextBox ID="txtApprovedBy" runat="server" Enabled="False" Width="314px" CssClass="text-box"></asp:TextBox></div>
                   </td>
                   <td>
                        <div><i>Date & Time of Approval</i></div>
					    <div> <asp:TextBox ID="txtApprovalDateTime" Enabled="False" runat="server"  Width="314px" CssClass="text-box"></asp:TextBox></div>
                   </td>
                </tr>
				<tr>
				   <td colspan="2" style="width: 100%">
				        <fieldset style="border-radius: 5px; border: 1px groove #038103; padding: 2px; width: 95%;">
			                <legend style="color: #038103;">Update or Complete Transaction Payment</legend>
                            <table style="border-style: none; border-color: inherit; border-width: medium; width:100%; " runat="server" id="tblApproveTransaction">
                                <tr>
                                    <td>
                                        <div><i>Amount to Pay</i> <asp:RequiredFieldValidator  ValidationGroup="valManageExpensePayment" ID="ReqAmountPaid"  runat="server" ErrorMessage="* Required" ControlToValidate="txtAmountPaid" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpAmountPaid" runat="server" ControlToValidate="txtAmountPaid" Display="Dynamic" ErrorMessage="* Invalid Entry" Font-Size="8pt" ForeColor="Red" SetFocusOnError="True" ValidationExpression="<%$ AppSettings:NumberValidationExpression2 %>" ValidationGroup="valManageExpensePayment"></asp:RegularExpressionValidator> </div>
                                        <div><input type="text" id="txtAmountPaid" runat="server" onkeyup="return calaculateBalance()" style="width: 278px; height: 18px" class="text-box"/></div>
                                    </td>
                                    <td>
                                       <div><i runat="server" id="iBalance">Balance</i></div>
					                    <div> <asp:TextBox ID="txtBalance" runat="server" Enabled="False"  Width="96%" CssClass="text-box"></asp:TextBox></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%" colspan="2" >
                                        <div><i>Official Comment</i><asp:RequiredFieldValidator  ValidationGroup="valManageExpensePayment" ID="ReqPaymentComment"  runat="server" ErrorMessage="* Required" ControlToValidate="txtAmountPaid" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator></div>
                                        <div style="width: 97%">
                                            <asp:TextBox runat="server" ID="txtPaymentComment" TextMode="MultiLine" Width="101%" Height="32px"></asp:TextBox>
                                        </div>
                                    </td>
                                 </tr>
                                 <tr>
                                     <td>
                                        
                                     </td>
                                    <td style="width: 60%">
                                       <div style="width: 70%; margin-left: 25%">
                                         <asp:Button ID="btnSubmit" runat="server" Text="Complete Payment" OnClick="BtnSubmitClick"  ValidationGroup="valManageExpensePayment" CssClass="customButton" CommandArgument="1"  Width="130px" />&nbsp;
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
       <table style="width: 100%">
         <tr>
             <td colspan="2" style="width: 100%">
                <div style="width: 65%; margin-left: .4%">
                    <asp:LinkButton runat="server" ID="lnkNewBeneficiary" ForeColor="#038103" CssClass="linkStyle" Text="<< Select a different Beneficiary" OnClick="LnkSelectNewBeneficiaryClick"></asp:LinkButton>
                </div>
            </td>
         </tr>
    </table>
    <div style="width: 100%; margin-top: 5px; " class="gridDiv" >
        <table style="width: 100%; height: 5px">
            <tr>
                <td >
                    <table style=" width: 100%">
                        <tr>
                            <td style="width: 100%" class="divBackground">
                              <div style="width: 100%">
                                  <label style="font-size: 1.2em; margin-left: 1%; color: #038103; width: auto;  width: 30%;"> Uncompleted Transaction Payment(s) for</label>&nbsp;
                                  <label runat="server" id="hBeneficiary" style=" font-size: 1.2em; width: auto; margin-left:0; font-weight: bolder; color: #038103">Beneficiary</label>
                              </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
               <td colspan="2" style="width: 100%">
                   <asp:DataGrid ID="dgBeneficiaryPaymentTrack" runat="server" AutoGenerateColumns="False" CellPadding="1"   CellSpacing="1" GridLines="none" CssClass="xPlugTextAll_x"  DataKeyField="StaffExpenseTransactionPaymentId" ShowFooter="True" Width="100%" OnItemCommand="DgBeneficiaryPaymentTrackCommand">
                        <FooterStyle CssClass="gridFooter" />
                        <AlternatingItemStyle CssClass="alternatingRowStyle"  />
                        <ItemStyle CssClass="gridRowItem" />
                        <HeaderStyle CssClass="gridHeader" />
                        <Columns>
                            <asp:TemplateColumn HeaderText="S/No.">
                                <HeaderStyle HorizontalAlign="center" Width="1%" />
                                <ItemStyle CssClass="xPlugTextAll_x" HorizontalAlign="center" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblSNo" runat="server" CssClass="xPlugTextAll_x" Text="<%# ((dgBeneficiaryPaymentTrack.PageSize*dgBeneficiaryPaymentTrack.CurrentPageIndex) + Container.ItemIndex + 1)%>"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Transaction" >
                                <HeaderStyle HorizontalAlign="left" Width="13%" />
                                <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblTransaction" runat="server" ssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"StaffExpenseTransaction.ExpenseTitle")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Transaction Date" >
                                <HeaderStyle HorizontalAlign="left" Width="7%" />
                                <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblTransactionDate" runat="server" CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"StaffExpenseTransaction.TransactionDate")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Transaction Time" >
                                <HeaderStyle HorizontalAlign="left" Width="5%" />
                                <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblPaymentTime" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"StaffExpenseTransaction.TransactionTime")) %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Total Amount of Transaction" >
                                <HeaderStyle HorizontalAlign="Left" Width="8%" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalAmount" runat="server"  CssClass="xPlugTextAll_x" Text='<%#  (DataBinder.Eval(Container.DataItem,"TotalAmountPayable")) %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Amount Already Paid" >
                                <HeaderStyle HorizontalAlign="left" Width="6%" />
                                <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblViewTransactionHistory" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"AmountPaid")) %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Balance" >
                                <HeaderStyle HorizontalAlign="left" Width="7%" />
                                <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:label ID="lblAmountPaid" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Balance")) %>' ></asp:label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Payment Status" >
                                <HeaderStyle HorizontalAlign="Left" Width="7%" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblTransactionStatus" runat="server"  CssClass="xPlugTextAll_x" Text='<%#  ((DataBinder.Eval(Container.DataItem,"Balance")).ToString() == "0")? "Payment Completed" : "Payment not completed" %>' ></asp:Label>
                                </ItemTemplate>
                          </asp:TemplateColumn>
                          <asp:TemplateColumn HeaderText="Action" >
                                <HeaderStyle HorizontalAlign="Left" Width="9%" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkTransactionPayment" runat="server" ForeColor="darkcyan" Text='Update/Complete Payment'  CommandName="CompletePayment"></asp:LinkButton>
                                </ItemTemplate>
                          </asp:TemplateColumn>
                       </Columns>
                       <PagerStyle HorizontalAlign="Right" Mode="NumericPages" />
                   </asp:DataGrid>
               </td> 
            </tr>
        </table>
    </div>
  </div>
    
    
    <script type="text/javascript">

        var oldBalance = $get("<%=txtBalance.ClientID%>").value;

        function calaculateBalance() {
            var balaceLabel = document.getElementById("<%=iBalance.ClientID%>");
            balaceLabel.innerHTML = "New Balance";
            var paymentControl = $get("<%=txtAmountPaid.ClientID%>");
            var balanceControl = $get("<%=txtBalance.ClientID%>");
            var paymentValue = paymentControl.value;
            if (paymentValue === null || paymentValue == "") {
                balanceControl.value = oldBalance;
                balaceLabel.innerHTML = "Old Balance";
            }
            else {
                balanceControl.value = oldBalance - paymentValue;
            }
            return false;
        }
    </script>
 </div>