<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="frmManageCompanyAssets.ascx.cs" Inherits="ExpenseManager.AssetManagement.FrmManageCompanyAssets" %>
<%@ Register TagPrefix="uc2" TagName="ErrorDisplay_1_1" Src="~/CoreFramework/ErrorControl/ErrorDisplay.ascx" %>
<%@ Register src="../CoreFramework/AlertControl/ConfirmAlertBox.ascx" tagname="ConfirmAlertBox" tagprefix="uc1" %>
<link href="App_Themes/Default/ControlStyles.css" rel="stylesheet" type="text/css" />

<style type="text/css">
   
</style>

<div class="dvContainer">
    <h2> Manage Company's Assets</h2>
	   <uc1:ConfirmAlertBox ID="ConfirmAlertBox1" runat="server" />
	   <div style="padding-bottom: 10px"><asp:Panel ID="Panel2" runat="server" Width="98%"><uc2:ErrorDisplay_1_1 ID="ErrorDisplay1" runat="server" /></asp:Panel></div>	
       <div id="dvSelectAssetCategory" class="divBackground2 aligncenter tdpad" style="width: 500px; padding:5px" runat="server">
           <asp:DropDownList  ID="ddlAssetCategories" AutoPostBack="True" OnSelectedIndexChanged="DdlAssetCategoriesIndexChanged" runat="server"/>
       </div>
       <div class="single-form-display" style="width:59%; border: 0 groove transparent; border-radius: 5px; display: none" id="dvProcessFixedAssetTypes" >
           <div id="dvFixedAssetsError" style="width: 98%"><uc2:ErrorDisplay_1_1 ID="ErrorDisplayProcessFixedAssets" runat="server"/></div>
           <fieldset >
              <legend runat="server" id="lgFixedAssets" style=""> Manage Fixed Assets</legend>
              <table style="border-style: none; border-color: inherit; border-width: medium; width:100%; padding: 1px; " runat="server" id="tblCreateTFixedAssets">                
                <tr>
                    <td style="width: 60%" class="tdpad">
                        <table>
                            <tr>
                                <td style="width:40%" class="tdpad">
					             Asset Name<asp:RequiredFieldValidator ID="ReqName"  runat="server" ErrorMessage="* Required" ControlToValidate="txtName"  ValidationGroup="valManageFixedAssets" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator> 
					                 <asp:TextBox ID="txtName" runat="server"   CssClass="text-box"></asp:TextBox>
				                </td>
                                <td class="tdpad">
					                 Description <asp:RequiredFieldValidator ID="ReqDescription"  runat="server" ErrorMessage="* Required" ControlToValidate="txtDescription"  ValidationGroup="valManageFixedAssets" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"> </asp:RequiredFieldValidator>
					                 <asp:TextBox ID="txtDescription"   runat="server"  CssClass="text-box" TextMode="MultiLine"></asp:TextBox>
				                </td>
                            </tr>
                            <tr>
                                <td class="tdpad">
                                     Model
					                 <asp:TextBox ID="txtModel"  runat="server"  CssClass="text-box"></asp:TextBox>
                                </td>
                                <td class="tdpad">
                                    Make
					                <asp:TextBox ID="txtMake"  runat="server"  CssClass="text-box"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:50%" class="tdpad">
                                     Brand
					               <asp:TextBox ID="txtBrand"  runat="server"  CssClass="text-box"></asp:TextBox>
				                </td>
                                <td class="tdpad">
                                  Unit Cost of Purchase<asp:RequiredFieldValidator ID="ReqCostofPurchase"  runat="server" ErrorMessage="* Required"  ValidationGroup="valManageCostofPurchase" ControlToValidate="txtCostofPurchase" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpCostofPurchase" runat="server" ControlToValidate="txtCostofPurchase" Display="Dynamic" ErrorMessage="* Invalid Entry" Font-Size="8pt" ForeColor="Red" SetFocusOnError="True" ValidationExpression="<%$ AppSettings:NumberValidationExpression2 %>" ValidationGroup="valManageFixedAssets"></asp:RegularExpressionValidator> 
					                 <asp:TextBox ID="txtCostofPurchase" runat="server"  Width="220px" CssClass="text-box"></asp:TextBox>
				                </td>                    
                            </tr>
                            <tr>
                                <td style="width: 50%" class="tdpad">Transp. & Installation cost<asp:RequiredFieldValidator ID="ReqInstallationcost"  runat="server" ErrorMessage="* Required" ControlToValidate="txtInstallationcost" SetFocusOnError="true" Text="" Display="Dynamic"  ValidationGroup="valManageFixedAssets" CssClass="errorClass"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpInstallationCost" runat="server" ControlToValidate="txtInstallationcost" Display="Dynamic" ErrorMessage="* Invalid Entry" Font-Size="8pt" ForeColor="Red" SetFocusOnError="True" ValidationExpression="<%$ AppSettings:NumberValidationExpression5 %>" ValidationGroup="valManageFixedAssets"></asp:RegularExpressionValidator>
					               <asp:TextBox ID="txtInstallationcost"  Width="220px" runat="server" CssClass="text-box"></asp:TextBox>
                                </td>
                                <td style="width: 50%" class="tdpad">
                                   Date Purchased<span style="color: Red"> (dd/mm/yyyy)</span><asp:RequiredFieldValidator ID="ReqDatePurchsed"  runat="server" ErrorMessage="* Required" ControlToValidate="txtDatePurchsed" SetFocusOnError="true" Text="" Display="Dynamic"  ValidationGroup="valManageFixedAssets" CssClass="errorClass"></asp:RequiredFieldValidator>
					                <asp:TextBox ID="txtDatePurchsed"  Width="220px" runat="server" CssClass="text-box"></asp:TextBox> 
                                    <ajaxToolkit:calendarextender ID="CalendarExtDatePurchased" runat="server" CssClass="MyCalendar" Format="dd/MM/yyyy"  TargetControlID="txtDatePurchsed"></ajaxToolkit:calendarextender>
					            </td>
                            </tr>
                            <tr>
                             <td style="width: 50%" class="tdpad">
                               Quantity/Number of Units<asp:RequiredFieldValidator ID="ReqQuantity"  runat="server" ErrorMessage="* Required" ControlToValidate="txtQuantity" SetFocusOnError="true" Text="" Display="Dynamic"  ValidationGroup="valManageFixedAssets" CssClass="errorClass"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpQuantity" runat="server" ControlToValidate="txtQuantity" Display="Dynamic" ErrorMessage="* Invalid Entry" Font-Size="8pt" ForeColor="Red" SetFocusOnError="True" ValidationExpression="<%$ AppSettings:NumberValidationExpression2 %>" ValidationGroup="valManageFixedAssets"></asp:RegularExpressionValidator>
                                <asp:TextBox ID="txtQuantity"   runat="server"></asp:TextBox>
                             </td>
                             <td style="width: 50%" class="tdpad">

                                Select the Asset receipt: <span style="color: Red">*jpeg/jpg only. 500KB Maximum</span>
                                <asp:FileUpload id="fldAssetReceipt" ClientIDMode="Static"  Width=" 95%" Height="22px" runat="server"/> 
                           </td>
                        </tr>
                      </table>
                    </td>
                    </tr>
                    <tr>
                      <td style="height: 160px;" class="tdpad">
                     <div style="padding: 10px 0px">
                          <img style="width: 100%; height: 225px;" runat="server" id="assetReceipt" src="#" alt="Attach Asset Receipt"/>
                       </div>
                    </td>  
                    </tr>
                    <tr>
                     <td colspan="2">
                         <table style="width: 100%">
                             <tr>
                                 <td style="width: 60%" class="tdpad">
                                     <div><asp:CheckBox runat="server" ID="chkFixedAssetType" Text="Active?" CssClass="customNewCheckbox"/></div>
                                 </td>
                             </tr>
                             <tr>
                                  <td style="width: 70%" class="tdpad">
                                 </td>
                                  <td style="width: 30%" class="tdpad" >
					                <asp:Button ID="btnProcessFixedAssets" runat="server" Text="Add" OnClick="BtnProcessFixedAssetsClick" ClientIDMode="Static" CssClass="customButton"  ValidationGroup="valManageFixedAssets" CommandArgument="1"  />&nbsp;&nbsp;
						            <input type="button" class="customButton" style="" value="Close" id="btnClose" />
                                 </td>
                             </tr>
                         </table>
                     </td>
                 </tr>
               </table>
        </fieldset>
    </div>
    <div> <asp:Button ID="btnPopUp" runat="server" Style="display: none" /></div>
     <ajaxToolkit:ModalPopupExtender ID="mpeProcessFixedAssetTypePopup" BackgroundCssClass="popupBackground"  TargetControlID="btnPopUp" CancelControlID="btnClose" PopupControlID="dvProcessFixedAssetTypes" RepositionMode="RepositionOnWindowResizeAndScroll" runat="server"></ajaxToolkit:ModalPopupExtender>
     <div style="width: 100%;" runat="server" id="dvManageFixedAssets" class="gridDiv">
          
        <table style="width: 100%">
            <tr class="divBackground">
                <td class="tdpaddleft" style="width: 30%;">
            <asp:LinkButton runat="server" ID="lnkNewAssetCategory"  CssClass="prevbtn" Text="<< Select a different Asset Category" OnClick="LnkSelectNewAssetCategoryClick"></asp:LinkButton>       
                </td>
                <td class="tdpadd" style="width: 70%"></td>
            </tr>
        </table>          
        <table style="width: 100%" class="" >
				
				<tr class="divBackground">
                    <td class="tdpad" style="width: 30%">
                   <label class="label2">Retrieve Fixed Assets by a Fixed Asset Type: </label>     
                    </td> 
                    <td style="width: 20%">
                        <div >
                          <asp:DropDownList runat="server" ID="ddlAssetTypes" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="DdlAssetTypesSelectedChanged"/>
                        </div>
			        </td>
                    <td style="width: 50%;text-align: right" class="tdpadd">
				        
					        <asp:Button runat="server" ID="btnAddNewFixedAsset" CssClass="customButton" CommandArgument="1" Text="Add New Fixed Asset" OnClick="BtnAddNewFixedAssetClick" Width="186px"/>
                       
			        </td>
		        </tr>
                <tr>
                   <td colspan="3"></td>
                 </tr>
                  <tr>
                      <td align="left" colspan="3" style="width: 100%">
                           <asp:DataGrid ID="dgFixedAssets" runat="server" AutoGenerateColumns="False" CellPadding="1"  CellSpacing="1" GridLines="None" CssClass="xPlugTextAll_x"  DataKeyField="FixedAssetId" 
                          ShowFooter="True" Width="100%" OnItemCommand="DgFixedAssetsItemCommand">
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
                                    <asp:TemplateColumn HeaderText="Asset" >
                                        <HeaderStyle HorizontalAlign="left" Width="8%" CssClass="tdpadtop" />
                                        <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblAsset" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Name")) %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                           <asp:Label ID="lblAssetTotal" runat="server" Text="Total" Font-Bold="true" ></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>                        
                                    <asp:TemplateColumn HeaderText="Purchase Cost" >
                                        <HeaderStyle HorizontalAlign="left" Width="6%" CssClass="tdpadtop" />
                                        <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblCostOfPurchase" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"CostOfPurchase")) %>' ></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                           <asp:Label ID="lblTotalCostOfPurchase" runat="server" Text="" Font-Bold="False" ></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Transp. & Installation Cost" >
                                        <HeaderStyle HorizontalAlign="left" Width="10%" CssClass="tdpadtop" />
                                        <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblTransportationInstallationCost" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"CostOfTransportationAndInstallation")) %>' ></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                           <asp:Label ID="lblTotalTransportationInstallationCost" runat="server" Text="" Font-Bold="False" ></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Quantity" >
                                        <HeaderStyle HorizontalAlign="left" Width="4%" CssClass="tdpadtop" />
                                        <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuantity" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Quantity")) %>' ></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                           <asp:Label ID="lblTotalQuantity" runat="server" Text="" Font-Bold="False" ></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Total Cost" >
                                        <HeaderStyle HorizontalAlign="Left" Width="1%" CssClass="tdpadtop" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblsubTotalCost" runat="server"  CssClass="xPlugTextAll_x" Text='<%#(DataBinder.Eval(Container.DataItem,"TotalCost"))%>' ></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                           <asp:Label ID="lblGrandTotalCost" runat="server" Text="" Font-Bold="False" ></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Model" >
                                        <HeaderStyle HorizontalAlign="left" Width="5%" CssClass="tdpadtop" />
                                        <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblModel" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Model")) %>' >
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Make" >
                                        <HeaderStyle HorizontalAlign="left" Width="8%" CssClass="tdpadtop"/>
                                        <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblMake" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Make")) %>' >
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Brand" >
                                        <HeaderStyle HorizontalAlign="left" Width="5%" CssClass="tdpadtop" />
                                        <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblBrand" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Brand")) %>' >
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Asset Category" >
                                        <HeaderStyle HorizontalAlign="left" Width="6%" CssClass="tdpadtop" />
                                        <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssetCategory" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"AssetCategory.Name")) %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Asset Type" >
                                        <HeaderStyle HorizontalAlign="left" Width="4%" CssClass="tdpadtop" />
                                        <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblFixedAssetType" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"AssetType.Name")) %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Date Purchased" >
                                        <HeaderStyle HorizontalAlign="left" Width="6%" CssClass="tdpadtop" />
                                        <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblTransactionDate" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"DatePurchased")) %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Code" >
                                        <HeaderStyle HorizontalAlign="Left" Width="2%" CssClass="tdpadtop" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblCode" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Code")) %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="View Receipt" >
                                        <HeaderStyle HorizontalAlign="Left" Width="5%" CssClass="tdpadtop" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbllinkViewReceipt" runat="server" Font-Bold="true"  CssClass="linkStyle" ForeColor="#038103" Text="View" CommandName="ViewReceipt" ></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Edit">
                                        <HeaderStyle HorizontalAlign="center" Width="2%" CssClass="tdpadtop" />
                                        <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgEdit" runat="server" AlternateText="Edit" CommandArgument="1" CommandName="Edit" ImageUrl="~/App_Themes/Default/Images/btn_edit_new.gif" ToolTip="Edit" style="cursor:hand" />
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
    <div class="single-form-display" style="width:auto; height: auto; border: 0 groove transparent; border-radius: 5px; display: none" id="dvViewAssetReceipt" >
      <fieldset style="">
         <legend runat="server" id="lgAssetName" style=""> View Asset Receipt</legend>
         <table style="width: 83%">
             <tr>
                 <td colspan="2" style="width: 100%">
                     <img id="fullReceiptView" runat="server" src="#" alt="Asset Receipt" style="width: 600px; height: 320px"/>
                 </td>
             </tr>
             <tr>
                 <td colspan="2" style="width: 100%">
                     <input type="button" style="margin-left: 50%; padding-top: 5px" value="Close" class="customButton" id="btnCloseViewReceipt" runat="server"/>
                 </td>
             </tr>
         </table>
      </fieldset>
   </div>
    <div class="single-form-display" style="width:32%; border: 0 groove transparent; border-radius: 5px; display: none" id="dvProcessLiquidAssets" >
      <div id="Div2" style="width: 98%"><uc2:ErrorDisplay_1_1 ID="errorDisplayProcessLiquidAssets" runat="server"/></div>
        <fieldset>
            <legend runat="server" id="lgProcessLiquidAssets" style=""> Manage Liquid Assets</legend>
              <table style="border-style: none; border-color: inherit; border-width: medium; width:100%; padding: 3px; " runat="server" id="tbProcessLiquidAssets">                
                <tr>
                    <td style="width: 60%">
                        <table>
                            <tr>
                                <td style="width:50%">
					                Asset Name<asp:RequiredFieldValidator ID="ReqLiquidAssetName"  runat="server" ErrorMessage="* Required" ControlToValidate="txtLiquidAssetName"  ValidationGroup="valManageLiquidAssets" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass"></asp:RequiredFieldValidator>
					                 <asp:TextBox ID="txtLiquidAssetName" runat="server" Width="322px" CssClass="text-box"></asp:TextBox>
				                </td>
                            </tr>
                            <tr>
                                <td style="width: 50%">
                                  Amount<asp:RequiredFieldValidator ID="ReqAmount"  runat="server" ErrorMessage="* Required" ControlToValidate="txtAmount" SetFocusOnError="true" Text="" Display="Dynamic"  ValidationGroup="valManageLiquidAssets" CssClass="errorClass"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExAmount" runat="server" ControlToValidate="txtAmount" Display="Dynamic" ErrorMessage="* Invalid Entry" Font-Size="8pt" ForeColor="Red" SetFocusOnError="True" ValidationExpression="<%$ AppSettings:NumberValidationExpression2 %>" ValidationGroup="valManageLiquidAssets"></asp:RegularExpressionValidator> 
					                <asp:TextBox ID="txtAmount"  Width="323px"  runat="server" CssClass="text-box"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                 </tr>
                 <tr>
                     <td colspan="2" >
                         <table style="width: 100%">
                             <tr>
                                 <td style="width: 70%" class="tdpad">
                                     <asp:CheckBox runat="server" ID="chkLiquidAssetStatus" Text="Active?" CssClass="customNewCheckbox"/>
                                 </td>
                             </tr>
                             <tr>
                                 <td style="width: 80%;text-align: right" class="tdpad">
					                <asp:Button ID="btnProcessLiquidAsset" runat="server" Text="Add" OnClick="BtnProcessLiquidAssetClick" CssClass="customButton"  ValidationGroup="valManageLiquidAssets" CommandArgument="1"  />&nbsp;&nbsp;
						            <input type="button" class="customButton" value="Close" id="btnCloseLiquidAssetPopup" />
                                 </td>
                             </tr>
                         </table>
                     </td>
                 </tr>
             </table>
        </fieldset>
    </div>  
    <div style="width: 100%" runat="server" id="dvManageLiquidAssets">
         <table style="border-style: none; border-color: inherit; border-width: medium; width: 100%; padding: 0px; height: 42px;">
			<tr>
				<td>
					<div style="margin-left: 1%">
					    <asp:LinkButton runat="server" ID="lnkDifferentAssetCategory" ForeColor="#038103" CssClass="linkStyle" Text="<< Select a different Asset Category" OnClick="LnkDifferentAssetCategoryClick"></asp:LinkButton>
					</div>
                </td>
			</tr>
            <tr>
                <td>
                    <div  class="divBackground2 aligncenter tdpad">
                        <label class="label2"> 
                        Retrieve 
                        Liquid Assets by a Liquid Asset Type: </label><asp:DropDownList runat="server" ID="ddlLiquidAssetTypes" AutoPostBack="True" OnSelectedIndexChanged="DdlLiquidAssetTypesSelectedChanged" CssClass="text-box-x" Width="260px"/>
                        &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="lnkShowAllLiquidAssets" Text="Retrieve All Liquid Assets" CssClass="linkStyle" OnClick="LnkShowAllLiquidAssetsClick" ForeColor="green" runat="server" Width="219px"></asp:LinkButton>
                    </div>
                </td>                            
            </tr>
		</table>
        <div  style="width: 100%; " class="gridDiv">
           <table style="width: 100%">
                <tr class="divBackground">
					<td style="width: 80%" class="tdpadd" >
				      <legend class="label">Registered Liquid Asset(s)</legend> 
				  </td>
                  <td style="width: 20%" class="tdpadd">
				    <asp:Button runat="server" ID="btnAddNewLiquidAsset" CssClass="customButton" CommandArgument="1" Text="Add New Liquid Asset" OnClick="BtnAddNewLiquidAssetClick" Width="186px"/>
				</td>
			</tr>
           <tr>
             <td colspan="2" style="width: 100%"></td>
          </tr>
          <tr>
             <td style="width: 100%;" colspan="2">
              <asp:DataGrid ID="dgLiquidAssets" runat="server" AutoGenerateColumns="False" CellPadding="1"  CellSpacing="1" GridLines="None" CssClass="xPlugTextAll_x"  DataKeyField="LiquidAssetId" 
              ShowFooter="True" Width="100%" OnEditCommand="DgLiquidAssetsEditCommand">
                <FooterStyle CssClass="gridFooter" />
                <AlternatingItemStyle CssClass="alternatingRowStyle"  />
                <ItemStyle CssClass="gridRowItem" />
                <HeaderStyle CssClass="gridHeader" />
                    <Columns>
                        <asp:TemplateColumn HeaderText="S/No." >
                            <HeaderStyle HorizontalAlign="center" Width="1%" CssClass="tdpadtop" />
                            <ItemStyle CssClass="xPlugTextAll_x" HorizontalAlign="center" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblLiquidAssetSNo" runat="server" CssClass="xPlugTextAll_x" Text="<%# ((dgLiquidAssets.PageSize*dgLiquidAssets.CurrentPageIndex) + Container.ItemIndex + 1)%>">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Asset" >
                            <HeaderStyle HorizontalAlign="left" Width="13%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblLiquidAssetName" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Name")) %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                               <asp:Label ID="lblLiquidAssetTotal" runat="server"  Text="Total" Font-Bold="true" ></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Code" >
                            <HeaderStyle HorizontalAlign="Left" Width="4%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblLiquidCode" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Code")) %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                         <asp:TemplateColumn HeaderText="Amount" >
                            <HeaderStyle HorizontalAlign="left" Width="8%"  CssClass="tdpadtop"/>
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblLiquidAssetAmount" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"Amount")) %>' ></asp:Label>
                            </ItemTemplate>
                             <FooterTemplate>
                               <asp:Label ID="lblLiquidAssetTotalAmount" runat="server" Text="" Font-Bold="False" ></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                         <asp:TemplateColumn HeaderText="Asset Category" >
                            <HeaderStyle HorizontalAlign="left" Width="12%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblAssetCategory" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"AssetCategory.Name")) %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                         <asp:TemplateColumn HeaderText="Asset Type" >
                            <HeaderStyle HorizontalAlign="left" Width="12%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblLiquidAssetType" runat="server"  CssClass="xPlugTextAll_x" Text='<%# (DataBinder.Eval(Container.DataItem,"AssetType.Name")) %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Status" >
                            <HeaderStyle HorizontalAlign="Left" Width="1%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server"  CssClass="xPlugTextAll_x" Text='<%# ((DataBinder.Eval(Container.DataItem,"Status")).ToString() == "1") ? "Active" : "Inactive" %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Edit" >
                            <HeaderStyle HorizontalAlign="center" Width="2%" CssClass="tdpadtop" />
                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEditLiquidAsset" runat="server" AlternateText="Edit" CommandArgument="1" CommandName="Edit" ImageUrl="~/App_Themes/Default/Images/btn_edit_new.gif" ToolTip="Edit" style="cursor:hand" />
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

         $(document).ready(function () 
         {
             readURL();

         });

         function readURL() 
         {
             $('#fldAssetReceipt').change(function (event) 
             {
                 var input = event.target.files;

                 if (input.length > 0)
                 {
                     var isValid = /\.jpg?$/i.test(document.getElementById('<%=fldAssetReceipt.ClientID %>').value);

                     if (!isValid) {
                         alert('Please select only a jpeg/jpg file!');
                         document.getElementById('<%=fldAssetReceipt.ClientID %>').value = '';
                         return false;
                     }

                     if (input[0].size > 500000) 
                     {
                         alert("Receipt size should NOT be more than 500KB");
                         input.value = '';
                         return false;
                     }

                     else 
                     {
                         document.getElementById('<%=assetReceipt.ClientID%>').src = "";
                         var reader = new FileReader();
                         reader.onload = function (e) 
                         {
                             document.getElementById('<%=assetReceipt.ClientID%>').src = e.target.result;

                         };

                         reader.readAsDataURL(input[0]);
                     }

                 }
                 else 
                 {
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