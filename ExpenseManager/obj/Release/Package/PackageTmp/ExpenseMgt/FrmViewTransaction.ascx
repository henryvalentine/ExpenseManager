<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FrmViewTransaction.ascx.cs" Inherits="ExpenseManager.ExpenseMgt.FrmViewTransaction" %>
<%@ Register src="../CoreFramework/AlertControl/ConfirmAlertBox.ascx" tagname="ConfirmAlertBox" tagprefix="uc1" %>
<link href="App_Themes/Default/ControlStyles.css" rel="stylesheet" type="text/css" />

       
      
         <div class="dvContainer">
    
    <h2 runat="server" id="hTitle">Approved And Voided Transactions</h2>	
    <uc1:ConfirmAlertBox ID="ConfirmAlertBox1" runat="server" />
    <div runat="server" id="dvUserEntries" style="width: 100%;" class="gridDiv">
      <table style="width : 100%">
        <tbody>
           <tr>
			   <td width="100%">
					<table style="border-style: none; border-color: inherit; border-width: 1px; width: 100%; padding: 0px;">
						<tr class="divBackground">
							<td style="width: 40%" class="tdpadd2">
							   
							        <label class="label">Requested Transaction(s)</label> 
                                
						    </td>
                        </tr>
                        <tr>
                          <td colspan="2" style="width: 100%">
                              <table style="width: 100%">
                                  <tr>
                                      <td colspan="7" class="tdpadd">
                                      <label style="width: 100%">
                                         Note on <b>FILTERING</b>: &nbsp;Select a <b>Status</b> and then narrow it down by <b>Date Range</b>
                                      </label>
                                    </td>
                                  </tr>
                                  <tr class="divBackground3">
                                       <td class="tdpad" style="width: 10%">
                                          <div style="width: 100%">
                                            <label style="margin-left: 70%">Status</label>
                                             
                                          </div>
                                      </td>
                                       <td class="tdpad" style="width: 12%">
                                       <asp:DropDownList runat="server" ID="ddlFilterOption" Width="60%" CssClass="text-box"></asp:DropDownList>    
                                       </td>
                                       <td class="tdpad" style="width: 3%">
                                            <label> From:</label>
                                       </td>
                                       <td class="tdpad" style="width: 15%">
                                       <asp:TextBox  runat="server" ID="txtStart" CssClass="text-box"></asp:TextBox>
                                       </td>
                                       <td class="tdpad" style="width: 3%">
                                       <label> To:</label>    
                                       </td>
                                       <td class="tdpad" style="width: 15%">
                                        <asp:TextBox   runat="server" ID="txtEndDate" CssClass="text-box"></asp:TextBox>   
                                       </td>

                                      <td style="width: 20%">
                                         
                                              <asp:Button ID="btnDateFilter" Text="Go" CssClass="customButton" OnClick="BtnDateFilterClick" runat="server" Width="72px"></asp:Button>
                                              &nbsp;&nbsp;&nbsp;<ajaxToolkit:calendarextender ID="CalendarExtFrom" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtStart"></ajaxToolkit:calendarextender>
                                              <ajaxToolkit:calendarextender ID="CalendarExtTo" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtEndDate"></ajaxToolkit:calendarextender>
                                        
                                       </td>
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
                                    <HeaderStyle HorizontalAlign="left" Width="25%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblTransactionTitle" runat="server" CssClass="linkStyle" Text='<%# (DataBinder.Eval(Container.DataItem,"ExpenseTitle")) %>' >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Requested By" >
                                    <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequester"  runat="server" CssClass="xPlugTextAll_x" Text='<%#  GetUserFullName(int.Parse((DataBinder.Eval(Container.DataItem,"RegisteredById")).ToString()))%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Beneficiary" >
                                    <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblBeneficiary"  runat="server" CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Beneficiary.FullName")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                 <asp:TemplateColumn HeaderText="Date Requested" >
                                    <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequestedDate" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"TransactionDate")) %>' >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Time Requested" >
                                    <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequestedTime" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"TransactionTime")) %>' >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Approval Status" >
                                    <HeaderStyle HorizontalAlign="Left" Width="7%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblApprovalStatus"  runat="server" CssClass="xPlugTextAll_x" Text='<%#  (DataBinder.Eval(Container.DataItem,"ApprovalStatus"))%>' ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Date Approved" >
                                    <HeaderStyle HorizontalAlign="left" Width="7%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblApprovedDate" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"DateApproved")) %>' >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Time Approved" >
                                    <HeaderStyle HorizontalAlign="left" Width="8%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblApprovedTime" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"TimeApproved")) %>' >
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Approver's Comment" >
                                    <HeaderStyle HorizontalAlign="Left" Width="13%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkApproverComment"  runat="server" CssClass="commentLink" Text='View Comment' CommandName="viewComment"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Payment Status">
                                    <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserTransactionPaymentStatus"  runat="server" CssClass="xPlugTextAll_x" Text='<%#  (DataBinder.Eval(Container.DataItem,"PaymentStatus"))%>' ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Details" >
                                    <HeaderStyle HorizontalAlign="Left" Width="8%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkTransactionDetails" Font-Bold="True" runat="server" CssClass="xPlugTextAll_x" Text='View' CommandName="viewDetails"></asp:LinkButton>
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
      <table style="width: 100%" class="gridFoot">
        <tbody>
           <tr>
			   <td style="width: 100%">
					<table style="border-style: none; border-color: inherit; border-width: 1px; width: 100%; padding: 0px; height: 41px;">
						<tr>
							<td style="width: 30%" class="divBackground tdpad" >
							    <div style="width: 100%; font-weight: bolder" >
							    <b style="color: #333;"> Transaction Items</b>:   <asp:Label ID ="lblTransactionTitle" style="width: 100%; color: #038103" runat="server" Text="" runat="server"></asp:Label>
                                </div> 
							</td>
                            <td style="width: 80%" class="divBackground">
                               <table style="width: 100%">
                                   <tr>
                                       <td style="width: 50%">

                                          <label class="label">Requested Total Amount:</label>&nbsp;<label id="lblRequestedAmmount" style=" width: 100%; color: #038103" runat="server"></label>

                                         
                                       </td>
                                       <td style="width: 50%">

                                         <label class="label">Approved Total Amount:</label>&nbsp;<label id="lblApprovedTotalAmount" style=" width: 100%; color: #038103" runat="server"></label> 

                       
                                       </td>
                                   </tr>
                               </table>
                            </td>
                            <td style="width: 10%" class="divBackground">
                                
                                   <asp:Button ID="btnBackNav" runat="server" Text="<< Back" CssClass="customButton"  OnClick="BtnBackNavClick"/>
                                
							</td>
						</tr>
					 </table>
			      </td>
			  </tr>
             <tr>
              <td colspan="2" style="width: 100%">
                  <label style="width: 100%">
                     
                  </label>
              </td>
           </tr>
           <tr>
           <td style="width: 100%">
              <asp:DataGrid ID="dgExpenseItem" runat="server" AutoGenerateColumns="False" CellPadding="1"  CellSpacing="1" GridLines="None" CssClass="xPlugTextAll_x"  DataKeyField="TransactionItemId" 
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
                    </Columns>
                   <PagerStyle HorizontalAlign="Right" Mode="NumericPages" />
                </asp:DataGrid> 
             </td>
           </tr>           
         </tbody>
      </table>
   </div> 

   <div  class="single-form-display" style="width:40%; border: 0 groove transparent; border-radius: 5px; display: none" runat="server" id="dvRejection">
       <fieldset style="border-radius: 5px; border: 1px groove #038103">
			<legend runat="server" id="lgCommentTitle" style=""></legend>
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
    <div> <asp:Button ID="btnPopUp" runat="server" Style="display: none" /></div>
     <ajaxToolkit:ModalPopupExtender ID="mpeExpenseItemsPopup" BackgroundCssClass="popupBackground"  TargetControlID="btnPopUp" CancelControlID="closerejection" PopupControlID="dvRejection" RepositionMode="RepositionOnWindowResizeAndScroll" runat="server"></ajaxToolkit:ModalPopupExtender>
 </div>

