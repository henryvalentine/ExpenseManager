<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="frmManageBeneficiaries.ascx.cs" Inherits="ExpenseManager.ExpenseMgt.FrmManageBeneficiaries" %>
<%@ Register src="../CoreFramework/AlertControl/ConfirmAlertBox.ascx" tagname="ConfirmAlertBox" tagprefix="uc1" %>
<link href="App_Themes/Default/ControlStyles.css" rel="stylesheet" type="text/css" />
<%@ Register TagPrefix="uc2" TagName="ErrorDisplay_1" Src="~/CoreFramework/ErrorControl/ErrorDisplay.ascx" %>


<div class="dvContainer">
    <style type="text/css">
      .customNewCheckbox
       {
           text-align: center;
            float: right;
            margin-right: 15%;
            width: auto;
        }
        
       #btnClose
       {
           width: 96px;
       }
        .text-box-x
        {}
   </style> 

 <h2 id="hTitle"> Manage Beneficiaries </h2>
	 <uc1:ConfirmAlertBox ID="ConfirmAlertBox1" runat="server" />
	 <div> <asp:Button ID="btnPopUp" runat="server" Style="display: none" /></div>
     <ajaxToolkit:ModalPopupExtender ID="mpeSelectDateRangePopup" BackgroundCssClass="popupBackground"  TargetControlID="btnPopUp" CancelControlID="btnClose" PopupControlID="dvNewBeneficiary" RepositionMode="RepositionOnWindowResizeAndScroll" runat="server"></ajaxToolkit:ModalPopupExtender>
     <div class="single-form-display" style="width:44%; border: 0 groove transparent; border-radius: 5px; display: none"  runat="server" id="dvNewBeneficiary">
        <fieldset style="border-radius: 5px; border: 1px groove">
			<legend id="lgTitle" runat="server" style=""></legend>
            <table style="width:100%; padding: 2px; border: none" runat="server" id="tbNewBeneficiary">
                <tr>
                    <td colspan="2">
                        <div><i>Beneficiary Type</i><asp:RequiredFieldValidator ID="ReqBeneficiaryType" ValidationGroup="valManageBeneficiaries"  runat="server" ErrorMessage="* Required" ControlToValidate="ddlBeneficiaryType" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> </asp:RequiredFieldValidator><asp:CompareValidator ID="CompareValBeneficiaryType" runat="server" ErrorMessage="* Invalid Selection" ValueToCompare="1" ValidationGroup="valManageBeneficiaries" ControlToValidate="ddlBeneficiaryType" Operator="GreaterThanEqual" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" ></asp:CompareValidator></div>
                       <asp:DropDownList runat="server" ID="ddlBeneficiaryType" CssClass="text-box" Width="100%"/> 
                    </td>
                </tr>
                <tr>  
                   <td  style="width:50%">
                       <div><i> Department</i><asp:RequiredFieldValidator ID="ReqDepartment" ValidationGroup="valManageBeneficiaries"  runat="server" ErrorMessage="* Required" ControlToValidate="ddlDepartment" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> </asp:RequiredFieldValidator><asp:CompareValidator ID="CompareValDepartment" runat="server" ErrorMessage="* Invalid Selection" ValueToCompare="1" ValidationGroup="valManageBeneficiaries" ControlToValidate="ddlDepartment" Operator="GreaterThanEqual" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" ></asp:CompareValidator></div>
                     <asp:DropDownList runat="server" ID="ddlDepartment" CssClass="text-box" Width="100%"/> 
                   </td>
                   <td  style="width:50%">
                       <div><i>Unit</i><asp:RequiredFieldValidator ID="ReqUnit" ValidationGroup="valManageBeneficiaries"  runat="server" ErrorMessage="* Required" ControlToValidate="ddlUnit" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> </asp:RequiredFieldValidator><asp:CompareValidator ID="CompareValUnit" runat="server" ErrorMessage="* Invalid Selection" ValueToCompare="1" ValidationGroup="valManageBeneficiaries" ControlToValidate="ddlUnit" Operator="GreaterThanEqual" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" ></asp:CompareValidator></div>
                     <asp:DropDownList runat="server" ID="ddlUnit" CssClass="text-box" Width="100%"/> 
                   </td>
                </tr>
                <tr>
                   <td style="width:50%">
					  <div><i> Full Name</i><asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="valManageBeneficiaries"  runat="server" ErrorMessage="* Required" ControlToValidate="txtFullName" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> </asp:RequiredFieldValidator></div>
					  <div> <asp:TextBox style="width:98%" ID="txtFullName" runat="server" CssClass="text-box" ></asp:TextBox></div>
				    </td>
                    <td style="width:50%">
					   <div><i>Company</i></div>
					   <div>
					      <asp:TextBox ID="txtCompanyName" style="width:100%" runat="server" CssClass="text-box" ClientIDMode="Static"></asp:TextBox>                           
                        </div>
                    </td>
                </tr>
                 <tr>
                    <td style="width:50%">
					   <div><i>Phone Number(1) (GSM) </i> <asp:RequiredFieldValidator ID="ReqPhone1" ValidationGroup="valManageBeneficiaries"  runat="server" ErrorMessage="* Required" ControlToValidate="txtPhone1" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> </asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExPhone1" runat="server" ErrorMessage="* Invalid" ControlToValidate="txtPhone1" Display="Dynamic" SetFocusOnError="True"  CssClass="errorClass" ValidationExpression="<% $ AppSettings:GSMNoHyphenValidationExpression %>"></asp:RegularExpressionValidator></div>
					   <div> <asp:TextBox ID="txtPhone1" style="width:98%" runat="server" CssClass="text-box"  MaxLength="11"></asp:TextBox></div>
				    </td>
                    <td style="width:50%">
					  <div><i>Phone Number(2) (GSM)</i><asp:RegularExpressionValidator  ID="RegularExPhone2" runat="server" ErrorMessage="* Invalid" ControlToValidate="txtPhone2" Display="Dynamic" SetFocusOnError="True"  CssClass="errorClass" ValidationExpression="<% $ AppSettings:GSMNoHyphenValidationExpression %>"></asp:RegularExpressionValidator></div>
					  <div> <asp:TextBox ID="txtPhone2" style="width:100%" runat="server" CssClass="text-box" MaxLength="11"></asp:TextBox></div>
				   </td>
                </tr>
                <tr>
                  <td style="width:50%">
					<div><i>Email Address </i> <asp:RequiredFieldValidator ID="ReqEmail" ValidationGroup="valManageBeneficiaries" runat="server" ErrorMessage="* Required" ControlToValidate="txtEmail" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> </asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExEmail" runat="server" ErrorMessage="* Invalid" ControlToValidate="txtEmail" Display="Dynamic" SetFocusOnError="True"  CssClass="errorClass" ValidationExpression="<%$AppSettings:eMailValidationExpression %>"></asp:RegularExpressionValidator></div>
					<div> <asp:TextBox  ID="txtEmail" style="width:98%" runat="server" CssClass="text-box" ></asp:TextBox></div>
				  </td>
                  <td style="width:50%">
					<div><i>Sex</i> <asp:RequiredFieldValidator ID="ReqSex" ValidationGroup="valManageBeneficiaries" runat="server" ErrorMessage="* Required" ControlToValidate="ddlSex" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> </asp:RequiredFieldValidator> <asp:CompareValidator ID="CompareValSex" runat="server" ErrorMessage="* Invalid Selection" ValidationGroup="valManageBeneficiaries" ValueToCompare="1" ControlToValidate="ddlSex" Operator="GreaterThanEqual" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" ></asp:CompareValidator></div>
					<div> <asp:DropDownList style="width:100%" ID="ddlSex" runat="server" CssClass="text-box"></asp:DropDownList></div>
				</td>
             </tr>
             <tr>
                <td style="width:50%">
				  <div><asp:CheckBox ID="chkBeneficiary" CssClass="customNewCheckbox" runat="server" Text="Active?"  /></div>
                </td>
                <td style="width:50%;">
				  <span style="margin-left: 8%">
					 <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="customButton" ValidationGroup="valManageBeneficiaries"  CommandArgument="1" onclick="BtnSubmitClick" Width="130px" />&nbsp;
                     <input type="button" id="btnClose" style="width : 103px" value="Close" class="customButton" />
				 </span> 
             </td>
		   </tr>
        </table>
      </fieldset>
    </div>
    <div runat="server" id="divReport" class="gridDiv">
       <table border="0" cellspacing="0" cellpadding="2" width="100%" runat="server" id="tbExpenseType">
          <tbody>
             <tr>
			    <td style="width: 100%">
				   <table style="width: 100%; border: none; padding: 0px">
					   <tr>
						  <td style="width: 20%" class="divBackground">
						     <div style="width: 67%; font-size: 1em; font-weight: bolder">
						        <label>Beneficiaries</label>
						     </div> 
						</td>
                        <td style="width: 60%" class="divBackground">
                            <div style="margin-left: 20%; width: 69%;">
                                <label> Search Beneficiary By Name: </label>&nbsp;
                                <asp:TextBox Width="39%"  runat="server" ID="txtSearch" CssClass="text-box-x" 
                                    Height="22px" ></asp:TextBox>&nbsp;&nbsp;
                                <asp:Button ID="btnSearch" runat="server" CssClass="customButton"  Width="55px" Text="Search" onclick="BtnSearchClick"></asp:Button>
                           </div>  
                        </td>
						<td style="width: 20%" class="divBackground">
						    <div style="margin-left: 5%; width: 179px;" >
							   <asp:Button ID="btnAddItem" runat="server" Text="Add New Beneficiary" CssClass="customButton" onclick="BtnAddItemClick" Width="165px" />
							</div>
						</td>
					</tr>
				</table>
			</td>
		 </tr>
         <tr>
            <td style="width: 100%">
                <asp:DataGrid ID="dgBeneficiaries" runat="server" AutoGenerateColumns="False" CellPadding="1" OnEditCommand="DgBeneficiariesEditCommand" CellSpacing="1" GridLines="none" CssClass="xPlugTextAll_x"  DataKeyField="BeneficiaryId" ShowFooter="True" Width="100%">
                    <FooterStyle CssClass="gridFooter" />
                    <AlternatingItemStyle CssClass="alternatingRowStyle"/>
                    <ItemStyle CssClass="gridRowItem" />
                    <HeaderStyle CssClass="gridHeader" />
                    <Columns>
                        <asp:TemplateColumn HeaderText="S/No.">
                            <HeaderStyle HorizontalAlign="center" Width="1%" />
                            <ItemStyle CssClass="xPlugTextAll_x" HorizontalAlign="center" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblSNo" runat="server" CssClass="xPlugTextAll_x" Text="<%# ((dgBeneficiaries.PageSize*dgBeneficiaries.CurrentPageIndex) + Container.ItemIndex + 1)%>">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Beneficiary" >
                            <HeaderStyle HorizontalAlign="left" Width="12%" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:label ID="lblBeneficiary" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"FullName")) %>' >
                                </asp:label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Company" >
                            <HeaderStyle HorizontalAlign="left" Width="10%" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblCompany" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"CompanyName")) %>' >
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Department" >
                            <HeaderStyle HorizontalAlign="left" Width="5%" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:label ID="lblDepartment" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Department")) %>' >
                                </asp:label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Unit" >
                            <HeaderStyle HorizontalAlign="left" Width="5%" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblUnit" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Unit")) %>' >
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Email" >
                            <HeaderStyle HorizontalAlign="left" Width="8%" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblEmail" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Email")) %>' >
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="GSM No 1" >
                            <HeaderStyle HorizontalAlign="left" Width="5%" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:label ID="lblGSMNo1" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"GSMNo1")) %>' >
                                </asp:label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="GSM No 2" >
                            <HeaderStyle HorizontalAlign="left" Width="5%" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblGSMNo2" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"GSMNo2")) %>' >
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <%--<asp:TemplateColumn HeaderText="Date Registered" >
                            <HeaderStyle HorizontalAlign="left" Width="6%" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblRegisteredDate" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"DateRegistered")) %>' >
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Time Registered" >
                            <HeaderStyle HorizontalAlign="Left" Width="6%" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblTimeRegistered" runat="server"  CssClass="xPlugTextAll_x" Text='<%#  (DataBinder.Eval(Container.DataItem,"TimeRegistered"))%>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>--%>
                        <asp:TemplateColumn HeaderText="Sex" >
                            <HeaderStyle HorizontalAlign="Left" Width="2%" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblSex" runat="server"  CssClass="xPlugTextAll_x" Text='<%#  ((DataBinder.Eval(Container.DataItem,"Sex")).ToString() == "1")? "Male" : "Female"%>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Status" >
                            <HeaderStyle HorizontalAlign="Left" Width="2%" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server"  CssClass="xPlugTextAll_x" Text='<%#  ((DataBinder.Eval(Container.DataItem,"Status")).ToString() == "1")? "Active" : "Inactive"%>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Register As Portal user" >
                            <HeaderStyle HorizontalAlign="left" Width="10%" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkRegAsPortalUser" runat="server"   CssClass="linkStyle" Text='Register' CommandName="Register">
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Edit">
                            <HeaderStyle HorizontalAlign="center" Width="2%" />
                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" AlternateText="Edit"  CommandArgument="1" CommandName="Edit" ImageUrl="~/App_Themes/Default/Images/btn_edit_new.gif" ToolTip="Edit" style="cursor:hand" />
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
</div>