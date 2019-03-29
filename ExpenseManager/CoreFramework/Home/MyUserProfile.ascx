<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyUserProfile.ascx.cs" Inherits="ExpenseManager.CoreFramework.Home.MyUserProfile" %>
<%@ Register TagPrefix="uc1" TagName="ErrorDisplay" Src="~/CoreFramework/ErrorControl/ErrorDisplay.ascx" %>
<link href="../../App_Themes/Default/cbtControlStyles.css" rel="stylesheet" type="text/css" />
<link href="../../App_Themes/Default/ControlStyles.css" rel="stylesheet" type="text/css" />
 
<div class="dvContainer">
    <h2 style="">My Profile</h2>
     <div style="padding-bottom: 8px"><asp:Panel ID="Panel2" runat="server" Width="98%"><uc1:ErrorDisplay ID="ErrorDisplay1" runat="server" /></asp:Panel></div>
     <div style="width: 80%; margin-left: auto; margin-right: auto;">
        <%-- <table style="width: 100%">
             <tr>
                 <td width="40%">
                     
                 </td>
              </tr>
              
           <tr>
               <td style="width: 60%">
                      

               </td>
           </tr>   

         </table>--%>
         
    <div class="profiledetail" style="width: 400px;">
      
   <div style="width:100%" runat="server" id="detailDiv">
		                <fieldset style="">
			                <legend style="">My Profile</legend>
			                <table id="tbUserInfo" style="width:100%; padding: 3px" runat="server">
					                <tr>
						                <td style="width:100%" class="tdpad">
							                First Name <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFirstName" ErrorMessage="" ValidationGroup="regValidation" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass">* Required</asp:RequiredFieldValidator>
							                <asp:TextBox ID="txtFirstName" runat="server" CssClass="text-box" ReadOnly="False" ValidationGroup="regValidation"></asp:TextBox>
						                </td>
					                </tr>
					                <tr>
						                <td style="width:100%" class="tdpad">
							               Last Name <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLastName" ErrorMessage="" ValidationGroup="regValidation" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass">* Required</asp:RequiredFieldValidator>
							                <asp:TextBox ID="txtLastName" runat="server" CssClass="text-box" ReadOnly="true" ValidationGroup="regValidation"></asp:TextBox>
						                </td>
					                </tr>
					                <tr>
						                <td class="tdpad">
							                 Sex
							                <asp:TextBox ID="txtSex" runat="server" CssClass="text-box" ReadOnly="true" ValidationGroup="regValidation"></asp:TextBox>
						                </td>
					                </tr>
					                <tr>
						                <td style="width:100%" class="tdpad">
							                Mobile Number: <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtMobileNumber" ErrorMessage="" ValidationGroup="regValidation" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass">* Required</asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"  ControlToValidate="txtMobileNumber" ErrorMessage=""  ValidationExpression="^0[7-8][0-9]\d{8}$"  ValidationGroup="regValidation" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass">* Invalid Mobile Number</asp:RegularExpressionValidator>
							               <asp:TextBox ID="txtMobileNumber" runat="server" CssClass="text-box" ReadOnly="false" ></asp:TextBox>
						                </td>
					                </tr>
					                <tr>
						                <td style="width:100%" class="tdpad">
							                Email <asp:RequiredFieldValidator ID="req1" runat="server" ControlToValidate="txtEmail" ErrorMessage="" ValidationGroup="regValidation" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass">* Required</asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegExpr1" runat="server"  ControlToValidate="txtEmail" ErrorMessage=""  ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"  ValidationGroup="regValidation" SetFocusOnError="true" Text="" Display="Dynamic" CssClass="errorClass">* Invalid Email</asp:RegularExpressionValidator>
							                <asp:TextBox ID="txtEmail" runat="server" CssClass="text-box" ReadOnly="false" ></asp:TextBox>
						                </td>
					                </tr>
					                 <tr>
						                <td style="width:100%" class="tdpad">
							               Designation
							                <asp:TextBox ID="txtDesignation" runat="server" CssClass="text-box" ReadOnly="False"></asp:TextBox>
						                </td>
					                </tr>
					
					                <tr><td style="height: 5px"></td></tr>
					                <tr>
						                <td style="width:100%; text-align: right; vertical-align: top" colspan="2">
							                <asp:Button ID="btnSubmit" runat="server" Text="Update" CssClass="customButton" ValidationGroup="regValidation" Visible="False"  CommandArgument="1" OnClick="btnSubmit_Click" />
						                </td>
				                </tr>
			                </table>
		                </fieldset>
	                </div>       
     
      
  </div>      
  
   <div class="profiledetail2">
      
  
     <div class="profilepics">
         </div>  
         
    <div class="profilepicsel">
        
        <ul>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
        </ul>
        

         </div>  
           
       <table style="width: 100%; padding: 0; cellspacing: 0">
    
      <tr>
       <td colspan="3"><h2>profile Details</h2></td>
       
    </tr>

    <tr>
       <td style="text-align: right; padding-right: 25px">Details Here</td>
       <td style="text-align: left">Details Here</td>
       <td></td>
    </tr>
    
      <tr>
       <td style="text-align: right; padding-right: 25px">Details Here</td>
       <td style="text-align: left">Details Here</td>
       <td></td>
    </tr>
    
      <tr>
       <td style="text-align: right; padding-right: 25px">Details Here</td>
       <td style="text-align: left">Details Here</td>
       <td></td>
    </tr>
    
      <tr>
       <td style="text-align: right; padding-right: 25px">Details Here</td>
       <td style="text-align: left">Details Here</td>
       <td></td>
    </tr>
</table>

   
      
  </div>          
   
         
         
         

     </div>
</div>

