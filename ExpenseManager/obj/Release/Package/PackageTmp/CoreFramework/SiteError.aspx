<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SiteError.aspx.cs" Inherits="ExpenseManager.CoreFramework.SiteError" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Site Error</title>
    <link href="../App_Themes/Default/Master.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table cellpadding="0" cellspacing="0" width="100%" border="0">
           <tr>
             <td  width="100%">
                <table cellpadding="1" cellspacing="0" width="100%" border="0">
                  <tr><td width="100%" height="90px" id="upper"></td></tr>
                  <tr><td width="100%" height="100px"></td></tr>
                  <tr>
                     <td>
                      <table cellpadding="2" cellspacing="0" width="100%" border="0">
                          <tr>
                             <td width="20%">&nbsp;</td>
                             <td width="60%" bgcolor="#666666"><span  class="style1">User Session Expired!</span>
                              </td>
                               <td width="20%"></td>
                            </tr>
                          <tr>
                             <td width="20%" align="right" valign="middle"></td>
                             <td width="60%" align="center">
                                 Your Session has expired. Please Re- Login to continue</td>
                              <td width="20%"></td>
                           </tr>
                          <tr>
                             <td width="20%" align="right" valign="middle">&nbsp;</td>
                             <td width="60%" align="center" height="9px">
                                 &nbsp;</td>
                              <td width="20%">&nbsp;</td>
                           </tr>
                          <tr>
                             <td width="20%" align="right" valign="middle">&nbsp;</td>
                             <td width="60%" align="center">
                                 <asp:LinkButton ID="LinkButton1" runat="server" Font-Bold="True" 
                                     Font-Size="10pt" CausesValidation="False" PostBackUrl="~/Login.aspx">Click Here To Re - Login</asp:LinkButton>
                             </td>
                              <td width="20%">&nbsp;</td>
                           </tr>
                          </table>
                     </td>
                   </tr>
                   <tr>
                     <td width="100%" align="center" height="100px">&nbsp;</td>
                   </tr>
                  <tr>
                     <td width="100%" align="center"> 
                       <div style="text-align:center; font-family: Trebuchet MS, Tw Cen MT, Arial, Verdana; font-size:9pt; color:#333366"> 
                         xPlug Technologies Limited. Copyright © 2009 - 2012. All Rights Reserved
                       </div>
                     </td>
                  </tr>
                </table>
              </td>
              </tr>
            </table>
    </div>
    </form>
</body>
</html>
