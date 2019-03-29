<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FrmStaffBeneficiaryReport.ascx.cs" Inherits="ExpenseManager.ExpenseMgt.Reports.FrmStaffBeneficiaryReport" %>
<%@ Register TagPrefix="uc2" TagName="ErrorDisplay_1" Src="~/CoreFramework/ErrorControl/ErrorDisplay.ascx" %>

<style type="text/css">
         
       #btnClose
       {
           width: 96px;
       }
   </style> 

 <h2 id="hTitle" style=" font-weight: normal; font-family: 'OCR A Extended', arial; border-bottom-color: #038103; margin-top: 1.3%">Staff Beneficiaries </h2>
	 <div> <asp:Button ID="btnPopUp" runat="server" Style="display: none" /></div>
	 <div style="padding-bottom: 5px"><asp:Panel ID="Panel2" runat="server" Width="98%"><uc2:ErrorDisplay_1 ID="ErrorDisplay1" runat="server" /></asp:Panel></div>
     <div runat="server" id="divReport" style="width: 99%; padding: 1px; " class="gridDiv">
       <table border="0" cellspacing="0" cellpadding="2" width="100%" runat="server" id="tbExpenseType">
          <tbody>
             <tr>
				<td style="width: 30%" class="divBackground">
					<div style="width: 92%; font-weight: bolder" >
					<label style="color: #038103; font-family: 'OCR A Extended', arial;">Registered Staff Beneficiaries</label></div> 
			    </td>
                <td style="width: 70%" class="divBackground">
                    <div style="margin-left: 30%; width: 594px;">
                        <label style="color: #038103; font-family: 'OCR A Extended', arial; margin-top: -4px"> Filter by Department</label>&nbsp;
                        <asp:DropDownList Width="40%"  runat="server" ID="ddlDepartment" CssClass="text-box-x" AutoPostBack="True" OnSelectedIndexChanged="DdlDepartmentIndexChanged"></asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                           <asp:Button ID="btnkRefresh" runat="server" CssClass="customButton" Width="15%" Text="Rtrieve All" onclick="BtnRefreshClick"></asp:Button>
                   
                   </div>  
                </td>
		     </tr>
            <tr>
            <td style="width: 100%" colspan="2">
                <asp:DataGrid ID="dgBeneficiaries" runat="server" AutoGenerateColumns="False" CellPadding="1" CellSpacing="1" GridLines="none" CssClass="xPlugTextAll_x"  DataKeyField="StaffBeneficiaryId" ShowFooter="True" Width="100%">
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
                                <asp:label ID="lblStaffBeneficiary" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"FullName")) %>' >
                                </asp:label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Department" >
                            <HeaderStyle HorizontalAlign="left" Width="10%" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblStaffDepartment" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Department.Name")) %>' >
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Unit" >
                            <HeaderStyle HorizontalAlign="left" Width="10%" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblStaff" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Unit.Name")) %>' >
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Email" >
                            <HeaderStyle HorizontalAlign="left" Width="10%" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblStaffEmail" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Email")) %>' >
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="GSM No." >
                            <HeaderStyle HorizontalAlign="left" Width="5%" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:label ID="lblGSMNo" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"GSMNo")) %>' >
                                </asp:label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Date Registered" >
                            <HeaderStyle HorizontalAlign="left" Width="6%" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblStaffRegisteredDate" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"DateRegistered")) %>' >
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Time Registered" >
                            <HeaderStyle HorizontalAlign="center" Width="6%" />
                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblStaffTimeRegistered" runat="server"  CssClass="xPlugTextAll_x" Text='<%#  (DataBinder.Eval(Container.DataItem,"TimeRegistered"))%>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Sex" >
                            <HeaderStyle HorizontalAlign="center" Width="2%" />
                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblStaffSex" runat="server"  CssClass="xPlugTextAll_x" Text='<%#  ((DataBinder.Eval(Container.DataItem,"Sex")).ToString() == "1")? "Male" : "Female"%>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Status" >
                            <HeaderStyle HorizontalAlign="center" Width="2%" />
                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblStaffStatus" runat="server"  CssClass="xPlugTextAll_x" Text='<%#  ((DataBinder.Eval(Container.DataItem,"Status")).ToString() == "1")? "Active" : "Inactive"%>' ></asp:Label>
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