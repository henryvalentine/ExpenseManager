<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="frmManageTransactions.ascx.cs" Inherits="ExpenseManager.ExpenseMgt.FrmManageTransactions" %>
<%@ Register src="../CoreFramework/AlertControl/ConfirmAlertBox.ascx" tagname="ConfirmAlertBox" tagprefix="uc1" %>
<%@ Register src="../CoreFramework/ErrorControl/ErrorDisplay.ascx" tagname="ErrorDisplay" tagprefix="uc2" %>
<link href="App_Themes/Default/ControlStyles.css" rel="stylesheet" type="text/css" />

 <div class="dvContainer">
     <h2> Initiate Transaction(s)</h2><label></label>
       <uc1:ConfirmAlertBox ID="ConfirmAlertBox1" runat="server" />
      <div class="single-form-display" style=" width:47%;" runat="server" id="dvFirstStage" >
         <table style="width: 100%">
             <tr>
                 <td style="width: 50%">
                    <div><b>Beneficiary Type</b> <asp:RequiredFieldValidator ID="ReqBeneficiaryType" runat="server" ErrorMessage="*Required" ControlToValidate="ddlBeneficiaryType"  ValidationGroup="valFirstStage" SetFocusOnError="True" Display="Dynamic"  CssClass="errorClass"></asp:RequiredFieldValidator> <asp:CompareValidator ID="CompareValBeneficiaryType" runat="server" ErrorMessage="* Invalid Selection" ValueToCompare="1" ControlToValidate="ddlBeneficiaryType" Operator="GreaterThanEqual"  ValidationGroup="valFirstStage" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" ></asp:CompareValidator></div>
                    <asp:DropDownList CssClass="text-box" id="ddlBeneficiaryType" runat="server" style="width:97%"/>
                    <ajaxToolkit:cascadingdropdown ID="ccdBeneficiaryType" runat="server" TargetControlID="ddlBeneficiaryType" ParentControlID="" Category="BeneficiaryTypeId" LoadingText="Loading Beneficiary Types. Please wait ..."  PromptText="--- Select Beneficiary Type ---" PromptValue="0" EmptyValue="0" EmptyText="-- List is empty --" SelectedValue="0" ServiceMethod="LoadBeneficiaryTypeService" ServicePath="~/expenseManagerStructuredServices.asmx"></ajaxToolkit:cascadingdropdown>
                 </td>
                 <td style="width: 50%">
                    <div><b>Beneficiary</b> <asp:RequiredFieldValidator ID="ReqBeneficiaries" runat="server" ErrorMessage="*Required" ControlToValidate="ddlBeneficiaries" ValidationGroup="valFirstStage" SetFocusOnError="True" Display="Dynamic"  CssClass="errorClass"></asp:RequiredFieldValidator> <asp:CompareValidator ID="CompareValBeneficiaries" runat="server" ErrorMessage="* Invalid Selection" ValueToCompare="1" ControlToValidate="ddlBeneficiaries" Operator="GreaterThanEqual"  ValidationGroup="valFirstStage" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" ></asp:CompareValidator></div>
                    <asp:DropDownList class="text-box" id="ddlBeneficiaries" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DdlBeneficiariesSelectedIndexChanged" style="width:97%"/>
                    <ajaxToolkit:cascadingdropdown ID="ccdBeneficiaries" runat="server" TargetControlID="ddlBeneficiaries" ParentControlID="ddlBeneficiaryType" Category="BeneficiaryId" LoadingText="Loading Beneficiaries. Please wait ..."  PromptText="--- Select Beneficiary ---" PromptValue="0" EmptyValue="0" EmptyText="-- List is empty --" SelectedValue="0" ServiceMethod="LoadBeneficiariesByBeneficiaryType" ServicePath="~/expenseManagerStructuredServices.asmx"></ajaxToolkit:cascadingdropdown>
                 </td>
             </tr>
         </table>
       </div>
       <div class="single-form-display" style="width:53%; border: 0 groove transparent; border-radius: 5px; display: none" id="dvCreateTransaction" >
        <fieldset style="border-radius: 5px; border: 1px groove;">
            <legend runat="server" id="lgTitle" style="">Add new Transaction Item</legend>
            <div style="width: 100%"><uc2:ErrorDisplay ID="ErrorDisplayManageTransaction" runat="server" /></div>
            <table style="border-style: none; border-color: inherit; border-width: medium; width:100%; padding: 3px; " runat="server" id="tblCreateTransaction">
              <tr>
                  <td  style="width:50%" class="tdpad">
                     <div><i>&nbsp; Account Head</i><asp:RequiredFieldValidator ID="ReqDepartment" ValidationGroup="valTransactions"  runat="server" ErrorMessage="* Required" ControlToValidate="ddlAccountHead" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> </asp:RequiredFieldValidator><asp:CompareValidator ID="CompareValAccountHead" runat="server" ErrorMessage="* Invalid Selection" ValueToCompare="1" ValidationGroup="valTransactions" ControlToValidate="ddlAccountHead" Operator="GreaterThanEqual" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" ></asp:CompareValidator></div>
                     <asp:DropDownList runat="server" ID="ddlAccountHead" CssClass="text-box"  />
                    <ajaxToolkit:cascadingdropdown ID="ccdAccountHead" runat="server" TargetControlID="ddlAccountHead" ParentControlID="" Category="AccountHeadId" LoadingText="Loading Account Heads. Please wait ..."  PromptText="--- Select Account Head ---" PromptValue="0" EmptyValue="0" EmptyText="-- List is empty --" SelectedValue="0" ServiceMethod="LoadAccountsHeadService" ServicePath="~/expenseManagerStructuredServices.asmx"></ajaxToolkit:cascadingdropdown>
				  </td >
                  <td style="width:50%" class="tdpad">
				    <div><i>Expense Item</i> <asp:RequiredFieldValidator ID="ReqExpenseItem" runat="server" ErrorMessage="*Required" ControlToValidate="ddlExpenseItem"  ValidationGroup="valTransactions" SetFocusOnError="True" Display="Dynamic"  CssClass="errorClass"></asp:RequiredFieldValidator> <asp:CompareValidator ID="CompareValExpenseItem" runat="server" ErrorMessage="* Invalid Selection" ValueToCompare="1" ControlToValidate="ddlExpenseItem" Operator="GreaterThanEqual"  ValidationGroup="valTransactions" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" ></asp:CompareValidator></div>
					<asp:DropDownList ID="ddlExpenseItem" CssClass="text-box" runat="server" ></asp:DropDownList>
					<ajaxToolkit:cascadingdropdown ID="ccdExpenseItem" runat="server" TargetControlID="ddlExpenseItem" ParentControlID="ddlAccountHead" Category="ExpenseItemId" LoadingText="Loading Expense Item. Please wait ..."  PromptText="--- Select Expense Item ---" PromptValue="0" EmptyValue="0" EmptyText="-- List is empty --" SelectedValue="0" ServiceMethod="LoadExpenseItemList" ServicePath="~/expenseManagerStructuredServices.asmx"></ajaxToolkit:cascadingdropdown>
                  </td>
                </tr>
                <tr>
                  <td style="width:50%" class="tdpad">
                    <div><i>&nbsp;Expense Type</i> <asp:RequiredFieldValidator ID="RequExpenseType" runat="server" ErrorMessage="*Required" ControlToValidate="ddlExpenseType"  ValidationGroup="valTransactions" SetFocusOnError="True" Display="Dynamic"  CssClass="errorClass"></asp:RequiredFieldValidator> <asp:CompareValidator ID="CompareValExpenseType" runat="server" ErrorMessage="* Invalid Selection" ValueToCompare="1" ControlToValidate="ddlExpenseType" Operator="GreaterThanEqual"  ValidationGroup="valTransactions" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" ></asp:CompareValidator></div>
				    <asp:DropDownList ID="ddlExpenseType" CssClass="text-box"  Width="314px" runat="server"></asp:DropDownList>
                  </td>
                  <td style="width:50%" rowspan="3" class="tdpad">
                     <div><i>Transaction Item Description</i> <asp:RequiredFieldValidator ID="ReqItemDescription" runat="server" ErrorMessage="*Required" ControlToValidate="txtItemDescription"  ValidationGroup="valTransactions" SetFocusOnError="True" Display="Dynamic"  CssClass="errorClass"></asp:RequiredFieldValidator></div>
                     <asp:TextBox runat="server" ID="txtItemDescription" Width="302px" 
                          CssClass="text-box" TextMode="MultiLine" Height="110px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width:50%" class="tdpad">
					  <div><i>&nbsp; Quantity</i><asp:RequiredFieldValidator ID="ReqQuantity"  runat="server" ErrorMessage="* Required" ControlToValidate="txtQuantity" SetFocusOnError="true" Text="" Display="Dynamic"  ValidationGroup="valTransactions" CssClass="errorClass"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtQuantity" Display="Dynamic" ErrorMessage="* Invalid Entry" Font-Size="8pt" ForeColor="Red" SetFocusOnError="True" ValidationExpression="<%$ AppSettings:NumberValidationExpression3 %>" ValidationGroup="valTransactions"></asp:RegularExpressionValidator> </div>
					  <div> <asp:TextBox ID="txtQuantity"  Width="302px" runat="server" 
                              CssClass="text-box"></asp:TextBox>
                      </div>
				   </td>  
                </tr>
                <tr>
                    <td style="width:50%" class="tdpad">
					  <div><i>Unit Price</i><asp:RequiredFieldValidator ID="ReqUnitPrice"  runat="server" ErrorMessage="* Required"  ValidationGroup="valTransactions" ControlToValidate="txtUnitPrice" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpUnitPrice" runat="server" ControlToValidate="txtUnitPrice" Display="Dynamic" ErrorMessage="* Invalid Entry" Font-Size="8pt" ForeColor="Red" SetFocusOnError="True" ValidationExpression="<%$ AppSettings:NumberValidationExpression2 %>" ValidationGroup="valTransactions"></asp:RegularExpressionValidator> </div>
					   <asp:TextBox ID="txtUnitPrice" runat="server"  Width="302px" CssClass="text-box"></asp:TextBox>
				    </td> 
                </tr>
                <tr>
                    <td style="width:50%">
                    
                    </td>
                    <td style="width:50%" class="tdpad"><br/>
                        <div style="text-align: right">
					        <asp:Button ID="btnProcessExpenseTransactions" runat="server" Text="Add" OnClick="BtnProcessExpenseTransactionsClick" CssClass="customButton"  ValidationGroup="valTransactions" CommandArgument="1" Width="114px" />&nbsp;&nbsp;
						    <input type="button" class="customButton" style="" value="Close" id="btnClose" />
					    </div>
					</td>
                </tr>
            </table>
        </fieldset>
     </div>
    <div> <asp:Button ID="btnPopUp" runat="server" Style="display: none" /></div>
    <ajaxToolkit:ModalPopupExtender ID="mpeExpenseItemsPopup" BackgroundCssClass="popupBackground" TargetControlID="btnPopUp" CancelControlID="btnClose" PopupControlID="dvCreateTransaction" RepositionMode="RepositionOnWindowResizeAndScroll" runat="server"></ajaxToolkit:ModalPopupExtender>
    <div style="width: 100%" runat="server" id="dvContents" >
    <table style="width: 100%">
        <tr>
            <td style="width: 45%">
              <div class="single-form-display" style="width:100%; border: 0 groove transparent; margin-left: 15%; border-radius: 5px; vertical-align: top" runat="server" id="dvBeneficiaryInfo">
              <fieldset>
			    <legend runat="server" id="lgTitleScope" style="">Beneficiary Information</legend>
                <table style="border-style: none; border-color: inherit; border-width: medium; width:100%; padding: 3px; height: 217px;">
                 <tr>
                    <td style="width:50%">
						<div><i>Full Name</i></div><div><b><label id="lblFullName" runat="server" style="width: 98%" ></label> </b> </div>
                    </td>
                   <td style="width:50%">
					 <div><i>Company</i></div><div><b><label id="lblCompany" runat="server" style="width: 100%" ></label> </b></div>
                   </td>
                </tr>
                 <tr>
                    <td style="width:50%">
					    <div><i >GSM NO.1</i> </div><div><b><label id="lblFirstGSMNO" runat="server" style="width: 98%"></label></b> </div>
				    </td>
                    <td style="width:50%">
					   <div><i>GSM NO.2</i> </div><div><b><label id="lblSecondGSMNO" runat="server" style="width: 100%"></label> </b> </div></td>
                </tr>
                <tr>
                   <td style="width:50%">
					  <div><i>Date Registered</i></div><div><b><label id="lblDateRegistered" runat="server" style="width: 98%"></label> </b> </div>
				   </td>  
                   <td style="width:50%">
					  <div><i>Time Registered</i></div><div><b><label id="lblTimeRegistered" runat="server" style="width: 100%"></label> </b> </div>
				   </td>  
                </tr>
                 <tr>
                   <td style="width:50%">
					  <div><i>Sex</i> </div><div><b><label id="lblSex" runat="server" style="width: 98%"></label></b></div>
				   </td>  
                   <td style="width:50%">
					  <div><i>Email</i></div><div><b><label id="lblEmail" runat="server" style="width: 100%"></label></b></div>
				   </td>  
                </tr>
                <tr>
                    <td>
                      <div style=" border-top-width: 2px; margin-bottom: 10px; width: 100%; height: 2px; color: green"></div>  
                    </td>
                </tr>
                   <tr>
                    <td colspan="2">
                      <asp:Button runat="server" ID="btnNewBeneficiary"  CssClass="btnBack" Text="Select a different Beneficiary" OnClick="BtnSelectNewBeneficiaryClick" Width="230px"></asp:Button>
                 </td>
             </tr>
          </table>
       </fieldset>
         </div> 
            </td>
            <td style="width: 55%">
               <div class="single-form-display" style="width:60%; border: 0 groove transparent; border-radius: 5px; margin-left:20%">
                  <fieldset>
                    <legend class="label">Create a Transaction</legend>
                       <table style="border-style: none; border-color: inherit; border-width: medium; width:99%; padding: 3px; ">
                         <tr>
                           <td colspan="2">
                              <div style="margin-top: 5px;"><b>Transaction Title</b><asp:RequiredFieldValidator ID="RequiredFieldValidator1"  runat="server" ErrorMessage="* Required" ControlToValidate="txtTransactionTitle"  ValidationGroup="valNewTransaction" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator></div>
                              <asp:TextBox runat="server" ID="txtTransactionTitle" Width="98%" CssClass="text-box"></asp:TextBox>
                         </td>
                     </tr>
                  <tr>
                   <td colspan="2">
                      <div style="margin-top: 5px;"><b>Description</b><asp:RequiredFieldValidator ID="ReqDescription"  runat="server" ErrorMessage="* Required" ControlToValidate="txtDescription"  ValidationGroup="valNewTransaction" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> </asp:RequiredFieldValidator></div>
					   <div> <asp:TextBox ID="txtDescription"  Width="100%" runat="server" Height="20%" CssClass="text-box" Rows="6" TextMode="MultiLine"></asp:TextBox></div> 
                  </td>
                  </tr>
                       <tr>
                          <td style="width: 55%" class="tdpad">
			                  <asp:LinkButton runat="server" ID="lnkAddTransactionItem" CssClass="linkStyleCustom" ForeColor="#038103" OnClick="LnkAddTransactionItemClick"></asp:LinkButton>
                           </td> 
                           <td style="width: 45%" class="tdpaddright" >
                            <div  runat="server" id="divCreateTransaction" style="font-weight: bold; width: 80%; margin-left:50%">
                               <asp:Button ID="btnCreateNewTransaction" CssClass="btnNext" runat="server" Text="&nbsp;&nbsp;Continue" OnClick="BtnCreateNewTransactionClick" ForeColor="#ffffff"  ValidationGroup="valNewTransaction" CommandArgument="1" style="width: 90%;"/>
                             </div>
                            <div id="divEditTransaction" runat="server">
                               <asp:LinkButton runat="server" ID="lnkEditTransaction" ForeColor="#038103" Text="Edit Transaction Details" OnClick="LnkEditTransactionDetailsClick"></asp:LinkButton>
                            </div>
                          </td>
                        </tr>
                    </table>
                  </fieldset> 
                </div>
            </td>
        </tr>
    </table>
   <div style="width: 100%;" class="gridDiv">
      <table style="width: 100%">
        <tbody>
            
           <tr class="divBackground">
             <td colspan="2" style="width: 100%">
                <asp:GridView ID="grdViewTransMain" runat="server" AutoGenerateColumns="false" DataKeyNames="ExpenseItemId" OnRowDataBound="grdViewTransMain_OnRowDataBound" CssClass="Grid" Width="100%"                  
                    AlternatingRowStyle-CssClass="alt"    PagerStyle-CssClass="pgr">      
                <Columns>
				    <asp:TemplateField ItemStyle-Width="20px">
					<ItemTemplate>
						<a href="#"> 
							<img alt="Details" id="imgdiv<%# Eval("ExpenseItemId") %>" src="Images/MessageIcons/icon_profile_16px.gif" />
						</a> <asp:Label ID="lblExpenseItemTitle" runat="server" CssClass="xPlugTextAll_x" Text='<%# Eval("Title") %>'></asp:Label>
						<div id="div<%# Eval("ExpenseItemId") %>" >
							<asp:GridView ID="grdViewTransItems" runat="server" AutoGenerateColumns="false" DataKeyNames="TempId" OnRowCommand="grdViewTransItems_OnRowCommand" Width="100%" CssClass="xPlugTextAll_x">
								<Columns>
                                    <asp:TemplateField HeaderText="S/No." >
                                        <HeaderStyle HorizontalAlign="center" Width="5%" CssClass="" />
                                        <ItemStyle HorizontalAlign="center" VerticalAlign="Top" CssClass="" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblSNo" runat="server" CssClass="xPlugTextAll_x" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" >
                                        <HeaderStyle HorizontalAlign="center" Width="20%" CssClass="" />
                                        <ItemStyle HorizontalAlign="center" VerticalAlign="Top" CssClass="" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescription" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Description")) %>' ></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                            <asp:Label ID="lblTotal" runat="server" Text="Total" Font-Bold="False" ></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Accounts Head" >
                                        <HeaderStyle HorizontalAlign="center" Width="10%" CssClass="" />
                                        <ItemStyle HorizontalAlign="center" VerticalAlign="Top" CssClass="" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblAccountsHead" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"AccountsHeadTitle")) %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quantity" >
                                        <HeaderStyle HorizontalAlign="center" Width="10%" CssClass="gridHeader" />
                                        <ItemStyle HorizontalAlign="center" VerticalAlign="Top" CssClass="alternatingRowStyle" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuantity" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"RequestedQuantity")) %>' ></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalQuantity" runat="server" Text="" Font-Bold="False" ></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit Price" >
                                    <HeaderStyle HorizontalAlign="center" Width="10%" CssClass="gridHeader" />
                                    <ItemStyle HorizontalAlign="center" VerticalAlign="Top" CssClass="gridRowItem" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnitPrice" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"RequestedUnitPrice")) %>' ></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotalPrice" runat="server" Text="" Font-Bold="False" ></asp:Label>
                                    </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Price" >
                                    <HeaderStyle HorizontalAlign="center" Width="10%" CssClass="gridHeader" />
                                    <ItemStyle HorizontalAlign="center" VerticalAlign="Top" CssClass="alternatingRowStyle" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubTotalPrice" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"TotalPrice")) %>' ></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotalPrice" runat="server" Text="" Font-Bold="False" ></asp:Label>
                                    </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit" >
                                    <HeaderStyle HorizontalAlign="center" Width="7%" CssClass="gridHeader" />
                                    <ItemStyle HorizontalAlign="center" VerticalAlign="Top" CssClass="gridRowItem" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgEdit" runat="server" CommandArgument='<%# Eval("TempId") %>' CommandName="EditRow" ImageUrl="~/App_Themes/Default/Images/btn_edit_new.gif" style="cursor:hand" />
                                    </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete" >
                                    <HeaderStyle HorizontalAlign="center" Width="7%" CssClass="gridHeader" />
                                    <ItemStyle HorizontalAlign="center" VerticalAlign="Top" CssClass="alternatingRowStyle" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgDelete" runat="server" CommandArgument='<%# Eval("TempId") %>' OnClientClick="javascript:return confirm('Are you sure you want to delete this item from the list?')" CommandName="DeleteRow" ImageUrl="~/App_Themes/Default/Images/btn_delete_new.gif" style="cursor:hand" />
                                    </ItemTemplate>
                                    </asp:TemplateField>
								</Columns>
							</asp:GridView>
						</div>
					</ItemTemplate>
				    </asp:TemplateField>
			    </Columns>
		      </asp:GridView>
             </td>
           </tr>
           <tr>
               <td colspan="2">
                   <table style="width: 100%" border="0" cellspacing="0" cellpadding="2" class="gridFoot">
                       <tr>
                           <td class=" tdpadd" style="width: 80%"> 
                               <div style="margin-left: 88%; width: 172px;padding-top: 4px; padding-bottom: 4px">
					                <asp:Button ID="btnCancel" ClientIDMode="Static" runat="server" Text="Cancel" CssClass="customButton" onclick="BtnCancelTransactionsClick" OnClientClick="showNotification()" Width="100px" />
				               </div>
                           </td>
                          <td class="tdpadd" style="width: 20%">
				            <div style="margin-left: 30%; width: 172px;padding-top: 4px; padding-bottom: 4px" > 
					            <asp:Button ID="btnAddItem" runat="server" ClientIDMode="Static" Text="Submit" CssClass="customButton" onclick="BtnSubmitTransactionsClick" OnClientClick="confirmSubmission()" Width="100px"/>
				            </div>
                          </td>
                       </tr>
                   </table>
               </td>
           </tr>
         </tbody>
      </table>
   </div>  
 </div>   
   <label runat="server" ClientIDMode="Static" id="lblAffirm" style="display: none"></label>
</div>

   <script type="text/javascript">
       function confirmSubmission() 
       {
           if (parseInt($('#lblAffirm').html()) < 1) 
           {
               $('#lblAffirm').html(1);
           }
       }

       function showNotification()
       {
           if (parseInt($('#lblAffirm').html()) > 0)
           {
               $('#lblNotification').css({ display: "block" });

               var notificationLabel = $('#lblNotification');
               var currentColor = "#038103";
               notificationLabel.css({
                   color: "#038103"
               });

               notificationLabel.addClass("customButtonTransit");
               setInterval(function () 
               {
                   if (currentColor === "#038103") 
                   {
                       currentColor = "black";
                       notificationLabel.css({
                           color: "black"
                       });
                   }
                   else 
                   {
                       currentColor = "#038103";
                       notificationLabel.css({
                           color: "#038103"
                       });
                   }
               }, 1000);
           }
       }
   </script>