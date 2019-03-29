<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ExpenseManager.Login" %>
<%@ Import namespace="ExpenseManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title> Expense Manager</title>
   <link href="App_Themes/Default/login.css" rel="stylesheet" type="text/css" />
   <link href="App_Themes/Default/jLogin.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Default/style.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Default/ControlStyles.css" rel="stylesheet" type="text/css" />
   <script src="Scripts/IE7.js" type="text/javascript"></script>
   
   <script src="Scripts/IE8.js" type="text/javascript"></script>
   <script src="Scripts/IE9.js" type="text/javascript"></script>
   
       
</head>
<body>
    <form id="form1" runat="server">  
            <div class="loginform">
                <div class="logininfo"><img src="App_Themes/Default/Images/expenseManager.png" alt="expenseManager Logo" style="margin-top:50px; margin-left:30px;"/></div>	
                   <div style="float:left; position:relative; height:250px;  width:2px; top: 0px; left: 0px; background:url('App_Themes/Default/Images/border.png')"></div>
                      <div class="signin">
                             <h2 style="margin-bottom: 20px; padding-bottom: 0"> User Login</h2>
                          <asp:Login ID="Login2" runat="server"  Width="100%"  FailureText="Login Failed!" DestinationPageUrl="XpenseManager.aspx?tabParentId=1&tabId=35&tabtype=1&tabOrder=1" Height="189px" CssClass="lognew" >
			        <LayoutTemplate>
			        <div>
			            <div>
			                <div style="font-size: 14px; padding-left:40px">
			                   <b>User Name:</b><asp:RequiredFieldValidator ID="req1" runat="server" ControlToValidate="UserName" ErrorMessage="* - Required" ValidationGroup="Login1" ForeColor="Red" Font-Size="9px" />
                            </div>
                            <div style="padding-bottom: 1px; padding-left:40px">
                                <asp:TextBox ID="UserName" runat="server" Width="70%" CssClass="username" Height="25px"></asp:TextBox>
                            </div>
			                <div style="font-size: 14px; padding-left:40px">
			                    <b>Password:</b><asp:RequiredFieldValidator ID="req2" runat="server" ControlToValidate="Password" ErrorMessage="* - Required" ValidationGroup="Login1" ForeColor="Red" Font-Size="9px" />
                            </div>
                            <div style="padding-left:40px">
                                <asp:TextBox ID="Password" runat="server" CssClass="password" TextMode="Password" Width="70%" Height="25px"></asp:TextBox> 
                            </div>
                            <span style="color:Red; text-align:right; margin-left: 35px;"><asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal></span>
                        </div>
                        <div style=" float: right">
                            <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="login" Width="100px"  ValidationGroup="Login1" CssClass="loginButton" />
                        </div>
			        </div>   
				    </LayoutTemplate>
		    </asp:Login>  
                </div>
            </div>
           </form>
          </body>
       </html>