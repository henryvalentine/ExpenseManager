<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="frmManageBeneficiary.ascx.cs" Inherits="ExpenseManager.ExpenseMgt.FrmManageBeneficiary" %>
<link href="App_Themes/Default/ControlStyles.css" rel="stylesheet" type="text/css" />
<%@ Register TagPrefix="cc1" Namespace="kPortal.Common.EnumControl" Assembly="kPortal.Common" %>
<%@ Register TagPrefix="uc2" TagName="ErrorDisplay_1_1" Src="~/CoreFramework/ErrorControl/ErrorDisplay.ascx" %>
<div class="dvContainer">
    
 <h2 id="hTitle"> Manage Beneficiaries </h2>
	
	 <div style=" padding-bottom: 1%; width: 98%"><uc2:ErrorDisplay_1_1 ID="ErrorDisplay1" runat="server" /></div>
	 <div> <asp:Button ID="btnPopUp" runat="server" Style="display: none" /></div>
     <ajaxToolkit:ModalPopupExtender ID="mpeRegisterBeneficiaryInfo" BackgroundCssClass="popupBackground"  TargetControlID="btnPopUp" CancelControlID="btnClose" PopupControlID="dvNewBeneficiary" RepositionMode="RepositionOnWindowResizeAndScroll" runat="server"></ajaxToolkit:ModalPopupExtender>
      <div class="single-form-display" style="width:44%; border: 0 groove transparent; border-radius: 5px; display: none "  runat="server" id="dvNewBeneficiary">
        <div id="dvBeneficiaryError" style="width: 98%"><uc2:ErrorDisplay_1_1 ID="ErrorDisplayBeneficiary" runat="server"/></div>
        <fieldset>
			<legend id="lgTitle" runat="server" style=""></legend>
            <table style="width:100%; padding: 2px; border: none" runat="server" id="tbNewBeneficiary">
                <tr>
                    <td colspan="2">
                        Beneficiary Type<asp:RequiredFieldValidator ID="ReqBeneficiaryType" ValidationGroup="valManageBeneficiaryInfo"  runat="server" ErrorMessage="* Required" ControlToValidate="ddlBeneficiaryType" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> </asp:RequiredFieldValidator><asp:CompareValidator ID="CompareValBeneficiaryType" runat="server" ErrorMessage="* Invalid Selection" ValueToCompare="1" ValidationGroup="valManageBeneficiaryInfo" ControlToValidate="ddlBeneficiaryType" Operator="GreaterThanEqual" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" ></asp:CompareValidator>
                       <asp:DropDownList runat="server" ID="ddlBeneficiaryType"  Width="100%" AutoPostBack="True" OnSelectedIndexChanged="DdlBeneficiaryTypeIndexChanged"/> 
                    </td>
                </tr>
                <tr>  
                   <td style="width:50%" class="tdpad">
                    Department<asp:RequiredFieldValidator ID="ReqDepartment" ValidationGroup="valManageBeneficiaryInfo"  runat="server" ErrorMessage="* Required" ControlToValidate="ddlDepartment" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> </asp:RequiredFieldValidator><asp:CompareValidator ID="CompareValDepartment" runat="server" ErrorMessage="* Invalid Selection" ValueToCompare="1" ValidationGroup="valManageBeneficiaryInfo" ControlToValidate="ddlDepartment" Operator="GreaterThanEqual" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" ></asp:CompareValidator>
                     <asp:DropDownList runat="server" ID="ddlDepartment"  Width="100%"/> 
                     <ajaxToolkit:cascadingdropdown ID="ccdDepartment" runat="server" TargetControlID="ddlDepartment" ParentControlID="" Category="DepartmentId" LoadingText="Loading Departments. Please wait ..."  PromptText="--- Select Expense Department ---" PromptValue="0" EmptyValue="0" EmptyText="-- List is empty --" SelectedValue="0" ServiceMethod="LoadDepartmentService" ServicePath="~/expenseManagerStructuredServices.asmx"></ajaxToolkit:cascadingdropdown>
                   </td>
                   <td  style="width:50%" class="tdpad">
                     Unit<asp:RequiredFieldValidator ID="ReqUnit" ValidationGroup="valManageBeneficiaryInfo"  runat="server" ErrorMessage="* Required" ControlToValidate="ddlUnit" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> </asp:RequiredFieldValidator><asp:CompareValidator ID="CompareValUnit" runat="server" ErrorMessage="* Invalid Selection" ValueToCompare="1" ValidationGroup="valManageBeneficiaryInfo" ControlToValidate="ddlUnit" Operator="GreaterThanEqual" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" ></asp:CompareValidator>
                     <asp:DropDownList runat="server" ID="ddlUnit" ClientIDMode="Static"  Width="100%"/> 
                     <ajaxToolkit:cascadingdropdown ID="ccdUnit" runat="server" TargetControlID="ddlUnit" ParentControlID="ddlDepartment" Category="UnitId" LoadingText="Loading Units. Please wait ..."  PromptText="--- Select Unit ---" PromptValue="0" EmptyValue="0" EmptyText="-- List is empty --" SelectedValue="0" ServiceMethod="LoadUnitList" ServicePath="~/expenseManagerStructuredServices.asmx"></ajaxToolkit:cascadingdropdown>
                   </td>
                </tr>
                <tr>
                   <td style="width:50%" class="tdpad">
					  Full Name<asp:RegularExpressionValidator  ID="RegularExpTransactionTitle" runat="server" ErrorMessage="* Invalid" ControlToValidate="txtFullName" Display="Dynamic" SetFocusOnError="True"  CssClass="errorClass" ValidationExpression="<%$ AppSettings:NameValidationExpression%>" ValidationGroup="valManageBeneficiaryInfo"></asp:RegularExpressionValidator><asp:RequiredFieldValidator ID="ReqFullName" ValidationGroup="valManageBeneficiaryInfo"  runat="server" ErrorMessage="* Required" ControlToValidate="txtFullName" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> </asp:RequiredFieldValidator>
					 <asp:TextBox style="width:98%" ID="txtFullName" runat="server" CssClass="text-box" ></asp:TextBox>
				    </td>
                    <td style="width:50%" class="tdpad">
					   
					   <div>Company<asp:RequiredFieldValidator ID="ReqtxtCompanyName" ValidationGroup="valManageBeneficiaryInfo"  runat="server" ErrorMessage="* Required" ControlToValidate="txtCompanyName" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> </asp:RequiredFieldValidator></div>
					      <asp:TextBox ID="txtCompanyName" style="width:100%" runat="server" CssClass="text-box" ClientIDMode="Static"></asp:TextBox>                          
                        
                    </td>
                </tr>
                 <tr>
                    <td style="width:50%" class="tdpad">
					   Phone Number(1) (GSM) <asp:RequiredFieldValidator ID="ReqPhone1" ValidationGroup="valManageBeneficiaryInfo"  runat="server" ErrorMessage="* Required" ControlToValidate="txtPhone1" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> </asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExPhone1" runat="server" ErrorMessage="* Invalid" ControlToValidate="txtPhone1" Display="Dynamic" SetFocusOnError="True"  CssClass="errorClass" ValidationExpression="<%$AppSettings:GSMValidationExpression %>" ValidationGroup="valManageBeneficiaryInfo"></asp:RegularExpressionValidator>
					    <asp:TextBox ID="txtPhone1" style="width:98%" runat="server" CssClass="text-box"  MaxLength="11"></asp:TextBox>
				    </td>
                    <td style="width:50%" class="tdpad">
					  Phone Number(2) (GSM)<asp:RegularExpressionValidator  ID="RegularExPhone2" runat="server" ErrorMessage="* Invalid" ControlToValidate="txtPhone2" Display="Dynamic" SetFocusOnError="True"  CssClass="errorClass" ValidationExpression="<%$ AppSettings:GSMValidationExpression%>"></asp:RegularExpressionValidator>
					 <asp:TextBox ID="txtPhone2" style="width:100%" runat="server" CssClass="text-box" MaxLength="11"></asp:TextBox>
				   </td>
                </tr>
                <tr>
                  <td style="width:50%" class="tdpad">
					Email Address  <asp:RequiredFieldValidator ID="ReqEmail" ValidationGroup="valManageBeneficiaryInfo" runat="server" ErrorMessage="* Required" ControlToValidate="txtEmail" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> </asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExEmail" runat="server" ErrorMessage="* Invalid" ControlToValidate="txtEmail" Display="Dynamic" SetFocusOnError="True" ValidationGroup="valManageBeneficiaryInfo"  CssClass="errorClass" ValidationExpression="<%$AppSettings:eMailValidationExpression %>"></asp:RegularExpressionValidator>
					 <asp:TextBox  ID="txtEmail" style="width:98%" runat="server" CssClass="text-box" ></asp:TextBox>
				  </td>
                  <td style="width:50%" class="tdpad">
					Sex<asp:RequiredFieldValidator ID="ReqSex" ValidationGroup="valManageBeneficiaryInfo" runat="server" ErrorMessage="* Required" ControlToValidate="ddlSex" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> </asp:RequiredFieldValidator> <asp:CompareValidator ID="CompareValSex" runat="server" ErrorMessage="* Invalid Selection" ValidationGroup="valManageBeneficiaryInfo" ValueToCompare="1" ControlToValidate="ddlSex" Operator="GreaterThanEqual" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass" ></asp:CompareValidator>
					 <asp:DropDownList style="width:100%" ID="ddlSex" runat="server"></asp:DropDownList>
				</td>
             </tr>
             <tr>
                <td style="width:50%" class="tdpad">
				 <asp:CheckBox ID="chkBeneficiary" CssClass="customNewCheckbox" runat="server" Text="Active?"  />
                </td>
                
		   </tr>
           <tr>
               <td style="width: 20px"></td>
              <td style="text-align: right;width: 80px" class="tdpad">
				<asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="customButton" ValidationGroup="valManageBeneficiaryInfo"  CommandArgument="1" onclick="BtnSubmitClick" />&nbsp;
                <input type="button" id="btnClose" style="" value="Close" class="customButton" />
             </td> 
           </tr>
        </table>
      </fieldset>
    </div>
    <div runat="server" id="divReport" class="gridDiv">
       <table border="0" cellspacing="0" cellpadding="2" width="100%" runat="server" id="tbExpenseType">
          <tbody>
             <tr>
			    <td width="100%">
				   <table style="width: 100%; border: none; padding: 0px">
				       
					   <tr class="divBackground">
						  <td style="width: 80%" class="tdpadd">
						     <label class="label">Beneficiaries</label>
						</td>
						<td style="width: 10%" class="tdpadd">
							<asp:Button ID="btnAddNewBeneficiary" runat="server" Text="Add New Beneficiary" CssClass="customButton" onclick="BtnAddNewBeneficiaryClick" Width="165px" />
						</td>
					</tr>
				</table>
			</td>
		 </tr>
        <tr>
            <td width="100%" align="left">
                <asp:DataGrid ID="dgBeneficiaries" runat="server" AutoGenerateColumns="False" CellPadding="1" OnItemCommand="DgBeneficiariesCommand" CellSpacing="1" GridLines="none" CssClass="xPlugTextAll_x"  DataKeyField="BeneficiaryId" ShowFooter="True" Width="100%">
                    <FooterStyle CssClass="gridFooter" />
                    <AlternatingItemStyle CssClass="alternatingRowStyle"/>
                    <ItemStyle CssClass="gridRowItem" />
                    <HeaderStyle CssClass="gridHeader" />
                    <Columns>
                        <asp:TemplateColumn HeaderText="S/No.">
                            <HeaderStyle HorizontalAlign="center" Width="1%" CssClass="tdpadtop" />
                            <ItemStyle CssClass="xPlugTextAll_x" HorizontalAlign="center" VerticalAlign="Top" />
                            <ItemTemplate>
	                            <asp:Label ID="lblSNo" runat="server" CssClass="xPlugTextAll">
		                            <%# (NowViewing*Limit)+(Container.ItemIndex + 1) %>
	                            </asp:Label>
                            </ItemTemplate>
                            <%--<ItemTemplate>
                                <asp:Label ID="lblSNo" runat="server" CssClass="xPlugTextAll_x" Text="<%# ((dgBeneficiaries.PageSize*dgBeneficiaries.CurrentPageIndex) + Container.ItemIndex + 1)%>">
                                </asp:Label>
                            </ItemTemplate>--%>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Beneficiary" >
                            <HeaderStyle HorizontalAlign="left" Width="10%"  CssClass="tdpadtop"/>
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:label ID="lblBeneficiary" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"FullName")) %>' >
                                </asp:label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Company" >
                            <HeaderStyle HorizontalAlign="left" Width="15%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblCompany" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"CompanyName")) %>' >
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Department" >
                            <HeaderStyle HorizontalAlign="left" Width="8%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblDepartment" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Department.Name")) %>' >
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Unit" >
                            <HeaderStyle HorizontalAlign="left" Width="7%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblUnit" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Unit.Name")) %>' >
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Email" >
                            <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblEmail" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Email")) %>' >
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="GSM No." >
                            <HeaderStyle HorizontalAlign="left" Width="5%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:label ID="lblGSMNo" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"GSMNO1")) %>' >
                                </asp:label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Sex" >
                            <HeaderStyle HorizontalAlign="Left" Width="2%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblSex" runat="server"  CssClass="xPlugTextAll_x" Text='<%#  ((DataBinder.Eval(Container.DataItem,"Sex")).ToString() == "1")? "Male" : "Female"%>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Status" >
                            <HeaderStyle HorizontalAlign="Left" Width="2%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server"  CssClass="xPlugTextAll_x" Text='<%#  ((DataBinder.Eval(Container.DataItem,"Status")).ToString() == "1")? "Active" : "Inactive"%>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Register As A user" >
                            <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkRegAsPortalUser" runat="server"   CssClass="linkStyle" Text='Register' CommandName="Register">
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Edit">
                            <HeaderStyle HorizontalAlign="center" Width="2%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" AlternateText="Edit"  CommandArgument="1" CommandName="Edit" ImageUrl="~/App_Themes/Default/Images/btn_edit_new.gif" ToolTip="Edit" style="cursor:hand" />
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
  <div style="width:25%; display: none; border-radius: 5px; border: 0 groove transparent; height: 143px;" runat="server" class="single-form-display" id="dvConfirmation">
      <div style="border: 0; height: 115px; width:100%; margin-bottom: 1%">
          <label id="lblMessage" style="width:25%; border-radius: 5px; border: 0; height: 115px;" runat="server"></label>
      </div>
      <div style="margin-top: 1%; width: 123px; margin-left: 60%; height: 26px;">
          <asp:Button ID="btnOK" Text="OK" Width ="39%" CssClass="customButton" OnClick="BtnOkClick" runat="server"/> &nbsp; 
          <input type="button" id="btnNO" value="Cancel" runat="server" class="customButton" style="width: 47%"/>
      </div>
  </div>
  <div style="width:50%; display: none; border-radius: 5px; border: 0 groove transparent;" runat="server" class="single-form-display" id="detailDiv">
      
        <fieldset style=" border-radius: 5px; border: 1px groove">
		   <legend>Create a Portal User Profile for Beneficiary</legend>
            <div id="Div2" style="width: 98%"><uc2:ErrorDisplay_1_1 ID="ErrorDisplayPortalUser" runat="server"/></div>
			   <table id="tbUserInfo" style="width:100%; padding: 4px" runat="server">
			       <tr>
						<td style="width:50%">
							<div><i>First Name</i><asp:RegularExpressionValidator  ID="RegularExpressionValidator3" runat="server" ErrorMessage="* Invalid" ControlToValidate="txtFirstName" Display="Dynamic" SetFocusOnError="True"  CssClass="errorClass" ValidationExpression="<%$ AppSettings:NameValidationExpression%>" ValidationGroup="valCreateStaffUserInfo"></asp:RegularExpressionValidator><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtFirstName" ErrorMessage="" ValidationGroup="valCreateStaffUserInfo" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass">* Required</asp:RequiredFieldValidator></div>
							<div style="padding-left: 2px"><asp:TextBox ID="txtFirstName" runat="server" CssClass="text-box" ReadOnly="False" ></asp:TextBox></div>
						</td>
                        <td style="width:50%">
							<div style="padding-left: 2px"><i>Last Name</i><asp:RegularExpressionValidator  ID="RegularExpLastName" runat="server" ErrorMessage="* Invalid" ControlToValidate="txtLastName" Display="Dynamic" SetFocusOnError="True"  CssClass="errorClass" ValidationExpression="<%$ AppSettings:NameValidationExpression%>" ValidationGroup="valCreateStaffUserInfo"></asp:RegularExpressionValidator> <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtLastName" ErrorMessage="" ValidationGroup="valCreateStaffUserInfo" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass">* Required</asp:RequiredFieldValidator></div>
							<div style="padding-left: 2px"><asp:TextBox ID="txtLastName" runat="server" CssClass="text-box" ReadOnly="False"></asp:TextBox></div>
						</td>
					</tr>
					<tr>
						<td style="width:50%">
							 <div><i>Sex</i> </div>
							 <div><cc1:DropDownListEnum ID="ddlPortalUserSex" runat="server" Width="100%" CssClass="ddl-box" EnumType="kPortal.Common.EnumControl.Enums.Sex" SetDefaultSelectValue="true" FixNames="true" UseXmlEnumNames="false" /></div>
						</td>
                        <td style="width:50%">
							<div style="padding-left: 2px"><i>Mobile Number:</i> <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtMobileNumber" ErrorMessage="" ValidationGroup="valCreateStaffUserInfo" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass">* Required</asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"  ControlToValidate="txtMobileNumber" ErrorMessage=""  ValidationExpression="<%$AppSettings:GSMValidationExpression %>"  ValidationGroup="valCreateStaffUserInfo" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass">* Invalid Mobile Number</asp:RegularExpressionValidator></div>
							<div style="padding-left: 2px"><asp:TextBox ID="txtMobileNumber" MaxLength="11" runat="server" CssClass="text-box" ReadOnly="false" ></asp:TextBox></div>
						</td>
					</tr>
					<tr>
						<td style="width:50%">
							<div><i>Email</i> <asp:RequiredFieldValidator ID="req1" runat="server" ControlToValidate="txtPortalUserEmail" ErrorMessage="" ValidationGroup="valCreateStaffUserInfo" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass">* Required</asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegExpr1" runat="server"  ControlToValidate="txtPortalUserEmail" ErrorMessage=""  ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"  ValidationGroup="valCreateStaffUserInfo" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass">* Invalid Email</asp:RegularExpressionValidator></div>
							<div><asp:TextBox ID="txtPortalUserEmail" runat="server" CssClass="text-box" ReadOnly="false" ></asp:TextBox></div>
						</td>
                        <td style="width:50%">
							<div style="padding-left: 2px"><i>Designation</i></div>
							<div style="padding-left: 2px"><asp:TextBox ID="txtDesignation" runat="server" CssClass="text-box" ReadOnly="False"></asp:TextBox></div>
						</td>
					</tr>
					 <tr>
						 <td colspan="2" style="width:100%; padding-bottom: 2px; margin-bottom: 5px; font-weight: bold; border-bottom: solid 1px ">Login Information</td>
					</tr>
					
					<tr>
						<td style="width:50%; padding-top: 8px">
							<div><i>User Name</i> <asp:RequiredFieldValidator ID="ReqUserName" runat="server" ControlToValidate="txtUserName" ErrorMessage="" ValidationGroup="valCreateStaffUserInfo" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass">* Required</asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"  ControlToValidate="txtUserName" ErrorMessage=""  ValidationExpression="^[a-zA-Z1-9'.]{6,20}$"  ValidationGroup="valCreateStaffUserInfo">* Invalid User Name</asp:RegularExpressionValidator></div>
							<div><asp:TextBox ID="txtUserName" runat="server" Width="98%" ReadOnly="false" ></asp:TextBox></div>
						</td>
                        <td style="width:50%; padding-top: 8px">
							<div style="height: auto; padding: 10px;">
								<asp:CheckBox ID="chkActive" runat="server" Text="Active?" TextAlign="Left" />
							</div>
						</td>
					</tr>
					<tr>
						<td style="width:50%">
							<div><i>Password</i> <asp:RequiredFieldValidator ID="reqPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="" ValidationGroup="valCreateStaffUserInfo" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass">* Required</asp:RequiredFieldValidator></div>
							<div><asp:TextBox ID="txtPassword" runat="server" CssClass="text-box" ReadOnly="False" TextMode="Password" ValidationGroup="valCreateStaffUserInfo"></asp:TextBox></div>
						</td>
                        <td style="width:50%">
							<div style="padding-left: 2px"><i>Confirm Password</i> <asp:RequiredFieldValidator ID="reqConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword" ErrorMessage="" ValidationGroup="valCreateStaffUserInfo">* Required</asp:RequiredFieldValidator><asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword" Display="Dynamic" ErrorMessage="" ValidationGroup="valCreateStaffUserInfo" CssClass="lDisplay" SetFocusOnError="True">* Password and Confirmation Password must match</asp:CompareValidator></div>
							<div style="padding-left: 2px"><asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="text-box" ReadOnly="False" TextMode="Password" ValidationGroup="valCreateStaffUserInfo"></asp:TextBox></div>
						</td>
					</tr>
                     <tr>
						 <td colspan="2" style="width:100%; padding-bottom: 2px; margin-bottom: 5px; border-bottom: solid 1px #999; text-shadow: 0 0 2px #ccc;">Identified User Role(s)</td>
					</tr>
                    <tr>
						<td style="width:100%; padding-top: 8px" colspan="2">
						   <div style="width: 100%; background-color: #f1f1f1; height: auto">
								<asp:CheckBoxList  ID="chkRoles" CssClass="checkBoxListWrap" runat="server"  Width="100%" TextAlign="Right" CellPadding="-1" CellSpacing="-1" RepeatLayout="Table"  RepeatDirection="Horizontal" RepeatColumns="3"></asp:CheckBoxList>
							</div>
						</td>
                    </tr>
					<tr><td style="height: 5px"></td></tr>
					<tr>
					  <td style="width:100%; text-align: right; vertical-align: top" colspan="2">
						 <asp:Button ID="btnCreatePortalProfile" runat="server" Text="Create User" CssClass="customButton" Width="130px"  ValidationGroup="valCreateStaffUserInfo"  CommandArgument="1" OnClick="BtnCreatePortalProfileClick" />&nbsp;
					     <input id="btnCancelProfile" type="button" value="Close" class="customButton"/>
                    </td>
				</tr>
			</table>
		</fieldset>
	</div>
</div>

<%--<script type="text/javascript">

    $(document).ready(function () {
        var beneficiaryTypeControl = document.getElementById('<%=ddlBeneficiaryType.ClientID%>');
        var benficiaryControl = document.getElementById('<%=ddlDepartment.ClientID%>');
        var unitControl = $('#ddlUnit');
        beneficiaryTypeControl.addEventListener('change', function () 
        {
            var val = beneficiaryTypeControl.options[beneficiaryTypeControl.selectedIndex].value;
            alert(val);
            if (parseInt(val) === 2) 
            {
                benficiaryControl.value = 8;
                unitControl.append($("<option></option>").val("14").html("Not Applicable"));
                //unitControl.value = 14;
                benficiaryControl.disabled = true;
                unitControl.disabled = true;
            }
        });
    });
    
   
</script>--%>