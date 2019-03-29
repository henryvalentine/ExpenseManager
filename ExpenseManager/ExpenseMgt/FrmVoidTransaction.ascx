<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FrmVoidTransaction.ascx.cs" Inherits="ExpenseManager.ExpenseMgt.FrmVoidTransaction" %>
<%@ Register TagPrefix="uc1" TagName="ConfirmAlertBox" Src="~/CoreFramework/AlertControl/ConfirmAlertBox.ascx" %>
<link href="../App_Themes/Default/ControlStyles.css" rel="stylesheet" type="text/css" />

  <div class="dvContainer">
    <uc1:ConfirmAlertBox ID="ConfirmAlertBox1" runat="server" />
    <h2>Void approved Transaction Requests</h2>
    <div runat="server" id="dvUserEntries" style="width: 100%;" class="gridDiv">
      <table style="width : 100%">
        <tbody>
           <tr>
			   <td width="100%">
					<table style="border-style: none; border-color: inherit; border-width: 1px; width: 100%; padding: 0px;">
						<tr class="divBackground">
							<td style="width: 40%" class="tdpadd2">
							    <div style="width: 99%; font-weight: bolder">
							        <label class="label">Requested Transaction(s)</label> 
                                </div>
						    </td>
                        </tr>
                        <tr>
                          <td colspan="2" style="width: 100%">
                              <table style="width: 100%">
                                  <tr class="divBackground3">
                                      
                                    <td class="tdpad" style="width: 20%; text-align: right">
                                         <label class="label3"> Filter by Date Range - </label>&nbsp;<label> From:</label>
                                    </td>
                                    <td class="tdpad">
                                        <asp:TextBox  runat="server" ID="txtStart" CssClass="text-box"></asp:TextBox>
                                    </td>
                                    <td class="tdpad" style="text-align: right">
                                        <label> To:</label>
                                    </td>
                                    <td class="tdpad">
                                        <asp:TextBox  runat="server" ID="txtEndDate" CssClass="text-box"></asp:TextBox>
                                    </td>
                                      <td style="width: 30%" class="tdpad">
                                              <asp:Button ID="btnDateFilter" Text="Go" CssClass="customButton" 
                                                  OnClick="BtnDateFilterClick" runat="server" Width="72px"></asp:Button>
                                              &nbsp;&nbsp;&nbsp;<ajaxToolkit:calendarextender ID="CalendarExtFrom" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtStart"></ajaxToolkit:calendarextender>
                                              <ajaxToolkit:calendarextender ID="CalendarExtTo" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtEndDate"></ajaxToolkit:calendarextender>
                                      </td>
                                      <td style="width: 10%"></td>
                                  </tr>
                              </table>
                          </td>
                       </tr>
					</table>
			     </td>
			  </tr>
            <tr>
            <td style="width: 100%">
               <asp:DataGrid ID="dgUserInitiatedTransactions" runat="server" AutoGenerateColumns="False" CellPadding="1" CellSpacing="1" GridLines="none" CssClass="xPlugTextAll_x" DataKeyField="ExpenseTransactionId" 
                   OnItemCommand ="DgUserInitiatedTransactionsItemCommand" ShowFooter="True" Width="100%" >
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
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Transaction" >
                                    <HeaderStyle HorizontalAlign="left" Width="13%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserTransactionTitle" runat="server" CssClass="linkStyle" Text='<%# (DataBinder.Eval(Container.DataItem,"ExpenseTitle")) %>' CommandName="Edit" >
                                        </asp:Label>
                                    </ItemTemplate>
                                    <%--<FooterTemplate>
                                       <asp:Label ID="lblTotal" runat="server" Text="Total" Font-Bold="true" ></asp:Label>
                                    </FooterTemplate>--%>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Requested By" >
                                    <HeaderStyle HorizontalAlign="Left" Width="12%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequester"  runat="server" CssClass="commentLink" Text='<%#  GetUserFullName(int.Parse((DataBinder.Eval(Container.DataItem,"RegisteredById")).ToString()))%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Beneficiary" >
                                    <HeaderStyle HorizontalAlign="Left" Width="12%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblBeneficiary"  runat="server" CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Beneficiary.FullName")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                 <asp:TemplateColumn HeaderText="Date Requested" >
                                    <HeaderStyle HorizontalAlign="left" Width="8%" CssClass="tdpadtop"/>
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserTransactionTransactionDate" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"TransactionDate")) %>' >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                 <asp:TemplateColumn HeaderText="Date Approved" >
                                    <HeaderStyle HorizontalAlign="left" Width="8%" CssClass="tdpadtop"/>
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserTransactionApprovalDate" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"DateApproved")) %>' >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Time Requested" >
                                    <HeaderStyle HorizontalAlign="left" Width="8%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserTransactionTransactionTime" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"TransactionTime")) %>' >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Time Approved" >
                                    <HeaderStyle HorizontalAlign="left" Width="8%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserTransactionApprovedTime" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"TimeApproved")) %>' >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Approval Status" >
                                    <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserTransactionStatus"  runat="server" CssClass="xPlugTextAll_x" Text='<%#  (DataBinder.Eval(Container.DataItem,"ApprovalStatus"))%>' ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Action" >
                                    <HeaderStyle HorizontalAlign="Left" Width="8%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkTransactionDetails"  runat="server" CssClass="xPlugTextAll_x" Text='View & Void' CommandName="viewDetails"></asp:LinkButton>
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
         </tbody>
      </table>
   </div>
   <div style="width: 100%;" class="gridDiv" runat="server" id="dvTransactionItems">
       
      <table style="width: 100%">
        <tbody>
           <tr>
               <tr>
             <td colspan="3"></td>
            <tr>
			   <td style="width: 100%">
					<table style="border-style: none; border-color: inherit; border-width: 1px; width: 100%; padding: 0px; height: 41px;">
						<tr>
							<td style="width: 30%" class="divBackground tdpad">
							    <div style="width: 100%; font-weight: bold" >
							       <asp:Label ID ="lblTransactionTitle" CssClass="label" style="width: 100%" runat="server" Text="Transaction Title" runat="server"></asp:Label>
                                </div> 
							</td>
                            <td style="width: 30%" class="divBackground">
                               <label class="label">Requested Total Amount: </label>&nbsp;<label id="lblRequestedAmmount" style="font-weight: bold; width: 100%; color: #038103" runat="server"></label>
                           </td>
                           <td style="width: 20%" class="divBackground">
                              <label class="label">Approved Total Amount:</label>&nbsp;<label id="lblApprovedTotalAmount" style=" width: 100%; font-weight: bold; color: #038103" runat="server"></label> 
                           </td>
                           <td style="width: 10%" class="divBackground">
                              <div style="width: 50%; margin-left: 10%">
                                 <asp:Button ID="btnBackNav" runat="server" Text="<< Back" CssClass="customButton" Width="176%" OnClick="BtnBackNavClick"/>
                              </div>
						  </td>
					   </tr>
			        </table>
			    </td>
			 </tr>
             <tr>
             <td style="width: 100%">
              <asp:DataGrid ID="dgExpenseItem" runat="server" ClientIDMode="Static" AutoGenerateColumns="False" CellPadding="1"  CellSpacing="1" GridLines="None" CssClass="xPlugTextAll_x"  DataKeyField="TransactionItemId" 
              ShowFooter="True" Width="100%">
                <FooterStyle CssClass="gridFooter" />
                <AlternatingItemStyle CssClass="alternatingRowStyle"  />
                <ItemStyle CssClass="gridRowItem" />
                <HeaderStyle CssClass="gridHeader" />
                    <Columns>
                        <asp:TemplateColumn HeaderText="S/No.">
                            <HeaderStyle HorizontalAlign="center" Width="2%" CssClass="tdpadtop" />
                            <ItemStyle CssClass="xPlugTextAll_x" HorizontalAlign="center" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblSNo" runat="server" CssClass="xPlugTextAll_x" Text="<%# ((dgExpenseItem.PageSize*dgExpenseItem.CurrentPageIndex) + Container.ItemIndex + 1)%>">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Expense Item" >
                            <HeaderStyle HorizontalAlign="left" Width="20%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblExpenseItemTitle" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"ExpenseItem.Title")) %>' >
                                </asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                               <asp:Label ID="lblTota" runat="server" Text="Total" Font-Bold="False" ></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Description" >
                            <HeaderStyle HorizontalAlign="left" Width="15%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblDescription" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Description")) %>' >
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Requested Quantity" >
                            <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblQuantity" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"RequestedQuantity")) %>' >
                                </asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                               <asp:Label ID="lblTotalQuantity" runat="server" Text="" Font-Bold="False" ></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                         <asp:TemplateColumn HeaderText="Approved Quantity" >
                            <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblApprovedQuantity" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"ApprovedQuantity")) %>' >
                                </asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                               <asp:Label ID="lblTotalApprovedQuantity" runat="server" Text="" Font-Bold="False" ></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Requested Unit Price" >
                            <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblUnitPrice" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"RequestedUnitPrice")) %>' ></asp:Label>
                            </ItemTemplate>
                             <FooterTemplate>
                               <asp:Label ID="lblTotalUnitPrice" runat="server" Text="" Font-Bold="False" ></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Approved Unit Price" >
                            <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblApprovedUnitPrice" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"ApprovedUnitPrice")) %>' ></asp:Label>
                            </ItemTemplate>
                             <FooterTemplate>
                               <asp:Label ID="lblTotalApprovedUnitPrice" runat="server" Text="" Font-Bold="False" ></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Status" >
                            <HeaderStyle HorizontalAlign="left" Width="7%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server"  CssClass="xPlugTextAll_x" Text='<%#  ((DataBinder.Eval(Container.DataItem,"Status")).ToString() == "1")? "Approved" : "Voided"%>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderTemplate><input type="checkbox" runat="server" id="slctAll" onclick="CheckAllIdChanged(this.id);" ClientIDMode="Static" style="margin-left: -8%" />Select All</HeaderTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="6%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
                            <ItemTemplate>
                                <input type="checkbox" onclick="CheckVoidChanged(this.id);" name="optionSlct" style="margin-left: 1%"  id="chkSlct<%# (DataBinder.Eval(Container.DataItem,"TransactionItemId")) %>"/>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                   <PagerStyle HorizontalAlign="Right" Mode="NumericPages" />
                </asp:DataGrid> 
             </td>
           </tr>   
         </tbody>
       </table>
       <table style="width: 100%">
           <tr>
               <td style="width: 30%">
                  <fieldset class="approvalFieldset">
		              <legend ><b>Options</b></legend>
                       <table style="width: 100%">
                         <tr>
                            <td style="width: 50%">
                               <div style="margin-left: 1%; width: 94%">
                                   <asp:RadioButton ID="rdVoidTransaction" onclick="UncheckItems(this.id);" runat="server" Text="Void Transaction" value="VoidTransaction" GroupName="VoidStatus" />
				              </div>
                           </td>
                           <td style="width: 50%">
                               <div style="margin-left: 1%; width: 94%">
                                   <asp:RadioButton ID="rdVoidItems" runat="server" Text="Void Selected Item(s)" onclick="SendItemIds();"  value="VoidItems" GroupName="VoidStatus" />
				              </div>
                           </td>
                         </tr>
                         <tr>
                            <td style="width: 50%" colspan="2">
                              <div style="width: 100%; margin-top: 1%">
                                <div> <b>Comment</b><asp:RequiredFieldValidator ID="ReqDescription"  runat="server" ErrorMessage="* Required" ControlToValidate="txtApproverComment"  ValidationGroup="valVoidTransaction" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> </asp:RequiredFieldValidator></div>
                                    <asp:TextBox runat="server" CssClass="text-box" Width="100%" 
                                      TextMode="MultiLine" Rows="7" Height="20%" ID="txtApproverComment"></asp:TextBox> 
                                <div style="width: 20%; margin-left: 80%" ><br/>
                                    <asp:Button runat="server" ID="btnVoidTransaction" CssClass="customButton" ValidationGroup="valVoidTransaction" Text="Submit" OnClick="BtnVoidTransactionClick" Width="96px" OnClientClick="javascript:return confirm('Are you sure you want to VOID this Transaction?')" CausesValidation="False"/>
                                </div>          
                             </div> 
                          </td>
                        </tr>
                    </table> 
                 </fieldset> 
               </td>
               <td style="width: 50%" class="tdpadd">
                 <div style="width: 40%; margin-left: 40%; margin-top: -10%">
                      <label style="width: 20%">
                     <b>NOTE</b>: &nbsp;Tick the <b>Void Transaction</b> Option to Void the Transaction <b>OR</b> Check unwanted Items of the Transaction and Tick <b>Void 
                   Selected Item(s)</b> to <b>Void those Items only.</b>
                   <hr style="border: 1px black solid;"/>
                   If you tick <b>Select All</b>, the Transaction and its Items will be Voided.
                  </label>
                 </div>
              </td>
           </tr>
       </table>
    </div> 
  </div>

  <script type="text/javascript">

      //var rdList = document.getElementsByName("VoidStatus");
      var idsCollection = "";
      var idArrayColl = [];
      var voidItmsRd = document.getElementById('<%=rdVoidItems.ClientID%>');
      function CheckVoidChanged(id) 
      {
          if ($('#' + id).is(':checked'))
          {
              var gt = id.split('t');
              if (parseInt(gt[1]) > 0)
              {
                  idArrayColl.push(gt[1]);
                  SendItemIds();
              }
          }
          else 
          {
//              $('#' + id).prop('checked', false);
              var dt = id.split('t');
              idArrayColl.splice($.inArray(dt[1], idArrayColl), 1);
              SendItemIds();
          }
      }

      function UncheckItems(id)
      {
          var slctListC = document.getElementsByName("optionSlct");
          var slctAll = document.getElementById("slctAll");
          if ($('#' + id).is(':checked'))
          {

              if (slctAll.checked == true)
              {
                  slctAll.checked = false;
              }
              idsCollection = "";
              idArrayColl = [];
              for (var i = 0; i < slctListC.length; i++) 
              {
                  if (slctListC[i].checked == true) 
                  {
                      slctListC[i].checked = false;
                  }
                
              }

          }
      }


      function CheckAllIdChanged(id)
      {
          var slctListC = document.getElementsByName("optionSlct");

          if ($('#' + id).is(':checked')) 
          {
              var voidTransRd = document.getElementById('<%=rdVoidTransaction.ClientID%>');
              
              if (voidTransRd.checked == true) 
              {
                  voidTransRd.checked = false;
              }
              
              idsCollection = "";
              idArrayColl = [];
              for (var i = 0; i < slctListC.length; i++) 
              {
                  slctListC[i].checked = true;
                  var tf = slctListC[i].id.split('t');
                  if (parseInt(tf[1]) > 0) 
                  {
                      idArrayColl.push(tf[1]);

                  }

              }

              SendItemIds();
          }
          else
          {
              for (var j = 0; j < slctListC.length; j++) 
              {

                  slctListC[j].prop('checked', false);
                  idArrayColl = [];
                  idsCollection = "";
              }

          }
      }

      function SendItemIds() 
      {
          idsCollection = '';
          if (voidItmsRd.checked == true && voidItmsRd.value === "VoidItems" && idArrayColl.length > 0) 
          {
              for (var x = 0; x < idArrayColl.length; x++)
                    {
                        if (parseInt(idArrayColl[x]) > 0)
                        {
                            idsCollection = idsCollection + idArrayColl[x] + ",";

                        }

                    }

                    if (idsCollection.length > 0) 
                    {
                        var dataToSend = JSON.stringify({ "transCollection": idsCollection });

                        $.ajax({
                            url: "expenseManagerStructuredServices.asmx/GetTransCollection",
                            data: dataToSend,
                            contentType: "application/json; charset=utf-8",
                            dataType: 'json',
                            type: 'POST',
                            success: nullIdCollection
                        });
                    }
            }
        }
        
      function nullIdCollection() 
      {
          idsCollection = "";
      }
  </script>