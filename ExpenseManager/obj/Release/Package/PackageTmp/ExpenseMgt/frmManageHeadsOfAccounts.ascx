<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="frmManageHeadsOfAccounts.ascx.cs" Inherits="ExpenseManager.ExpenseMgt.FrmManageHeadsOfAccounts1" %>
<%@ Register src="../CoreFramework/AlertControl/ConfirmAlertBox.ascx" tagname="ConfirmAlertBox" tagprefix="uc1" %>
<link href="App_Themes/Default/ControlStyles.css" rel="stylesheet" type="text/css" />
<%@ Register TagPrefix="uc2" TagName="ErrorDisplay" Src="~/CoreFramework/ErrorControl/ErrorDisplay.ascx" %>
<div class="dvContainer">
   
    <h2>Manage Accounts Heads</h2>
    <uc1:ConfirmAlertBox ID="ConfirmAlertBox1" runat="server" />
    <div> <asp:Button ID="btnPopUp" runat="server" Style="display: none" /></div>
    <ajaxToolkit:ModalPopupExtender runat="server" ID="mpeProcessAccountsHead" BackgroundCssClass="popupBackground"  TargetControlID="btnPopUp" PopupControlID="dvProcessAccountsHead" CancelControlID="btnCloseProcessAccounts" RepositionMode="RepositionOnWindowResizeAndScroll"/>
    <div id="dvProcessAccountsHead" class="single-form-display" style="width:35%; display: none; border: 0 groove transparent; border-radius: 5px; ">
        <div style="padding-bottom: 10px"><asp:Panel ID="Panel2" runat="server" Width="98%"><uc2:ErrorDisplay ID="ErrorDisplayProcessAccountsHead" runat="server" /></asp:Panel></div>
        <fieldset style="">
            <legend style="" runat="server" id="lgAccountsTitle">Create new Accounts Head</legend>
           <table style="width: 97%; padding: 3px; border: none">
            <tr>
                <td class="tdpad">
                   Select Expense Category <asp:RequiredFieldValidator ID="ReqCategories" ValidationGroup="valManageAccountsHead" runat="server" ErrorMessage="*Required" ControlToValidate="ddlExpenseCategory" SetFocusOnError="True" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator> <asp:CompareValidator ID="CompareValAccountsHead" runat="server" ErrorMessage="* Invalid Selection" ValidationGroup="valManageAccountsHead" ValueToCompare="1" ControlToValidate="ddlExpenseCategory" Operator="GreaterThanEqual" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:CompareValidator>
                    <asp:DropDownList runat="server" ID="ddlExpenseCategory" Width="100%" />
                </td>
            </tr>
            <tr>
                <td class="tdpad">
                   Title<asp:RequiredFieldValidator ID="ReqTitle" runat="server" ValidationGroup="valManageAccountsHead" ErrorMessage="*Required" ControlToValidate="txtTitle" SetFocusOnError="True" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" Width="100%" ID="txtTitle" CssClass="text-box"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdpad">
                  <div><i>Description</i><asp:RequiredFieldValidator ID="ReqDescription" ValidationGroup="valManageAccountsHead" runat="server" ErrorMessage="*Required" ControlToValidate="txtDescription" SetFocusOnError="True" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator></div>
                  <asp:TextBox runat="server" Width="100%" ID="txtDescription" TextMode="MultiLine" Height="20%" CssClass="text-box"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdpad">
                    <asp:CheckBox ID="chkAccountsHead" Text="Active?" CssClass="customNewCheckbox" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdpad" style="text-align: right">
                    <asp:Button runat="server" ID="btnSubmitAccountsHead" CommandArgument="1" Text="Submit" CssClass="customButton" OnClick="BtnSubmitAccountsHeadClick" ValidationGroup="valManageAccountsHead" Width="119px"/>&nbsp;
                    <input type="button" id="btnCloseProcessAccounts" style=" width: 88px" value="Close" class="customButton"/>
                </td>
            </tr>
        </table>
     </fieldset>
    </div>
    <table style="width: 100%" class="divBackground2">
            <tr>
                <td class="tdpad" style="width: 17%">
                        <label class="label2"> Retrieve Accounts Heads by Expense Category: </label>&nbsp;&nbsp;&nbsp;
                </td>
                 <td class="tdpad" style="width: 18%">
                 <asp:DropDownList runat="server" ID="ddlCategory" AutoPostBack="True" OnSelectedIndexChanged="DdlCategorySelectedChanged" 
                 />     
                 </td>
                 <td class="tdpad" style="width: 20%;text-align: left" >
                     <%--<asp:LinkButton ID="lnkShowAll" Text="Retrieve All Accounts Heads" CssClass="linkStyle" OnClick="LnkShowAllClick" ForeColor="green" runat="server" Width="219px"></asp:LinkButton>--%>
                 </td>
             </tr>
        </table>
    <div id="dvCategoriesList" style="width: 100%; " class="gridDiv">
      <div class = "divLine">
            <table style="width: 100%">
                <tr class="divBackground" >
                    <td style="width: 80%" class="tdpadtop">
                       <label class="label">Registered Accounts Heads</label>
                    </td>
                    <td style="width: 20%" class="tdpadtop">
                        <div style="margin-left: 20%; width: 189px">
                            <asp:Button ID="btnAddNewAccountsHead" runat="server" Text="Add New Accounts Head" CssClass="customButton" Width="183px" onclick="BtnAddNewAccountsHeadClick"  />
                        </div>
                    </td>
                </tr>
                 <tr >
                    <td style="width: 100%" colspan="2">
                        <asp:DataGrid runat="server" ID="dgAccountsHead" Width="100%" AutoGenerateColumns="False" CellPadding="1" CellSpacing="1" GridLines="none" CssClass="xPlugTextAll_x" ShowFooter="True" DataKeyField="AccountsHeadId" OnEditCommand="DgAccountsHeadEditCommand">
                            <FooterStyle CssClass="gridFooter" />
                            <AlternatingItemStyle CssClass="alternatingRowStyle"  />
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
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Accounts Head" >
                                    <HeaderStyle HorizontalAlign="left" Width="12%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:label ID="lblAccountsTitle" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Title")) %>' ></asp:label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Expense Category" >
                                    <HeaderStyle HorizontalAlign="left" Width="6%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:label ID="lblExpenseCategory" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"ExpenseCategory.Title")) %>' ></asp:label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Description" >
                                    <HeaderStyle HorizontalAlign="left" Width="20%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:label ID="lblDescription" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Description")) %>' ></asp:label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Code" >
                                    <HeaderStyle HorizontalAlign="left" Width="4%" CssClass="tdpadtop"/>
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:label ID="lblCode" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Code")) %>' ></asp:label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Status" >
                                    <HeaderStyle HorizontalAlign="left" Width="2%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:label ID="lblStatus" runat="server"  CssClass="xPlugTextAll_x" Text='<%# ((DataBinder.Eval(Container.DataItem,"Status")).ToString() == "1") ? "Active" : "Inactive" %>' ></asp:label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Edit" >
                                    <HeaderStyle HorizontalAlign="center" Width="2%" CssClass="tdpadtop" />
                                    <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgEdit" runat="server" AlternateText="Edit" CommandArgument="1" CommandName="Edit" ImageUrl="~/App_Themes/Default/Images/btn_edit_new.gif" ToolTip="Edit" style="cursor:hand" />
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
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
</div>