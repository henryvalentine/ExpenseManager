<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MultiVoucherManager.aspx.cs" Inherits="ExpenseManager.ExpenseMgt.Voucher.MultiVoucherManager" %>

<%@ Register src="../../CoreFramework/AlertControl/ConfirmAlertBox.ascx" tagname="ConfirmAlertBox" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="App_Themes/Default/portal.css" rel="stylesheet" type="text/css" />
	<link href="App_Themes/Default/tab.css" rel="stylesheet" type="text/css" />
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
        
           <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" EnablePageMethods="True" AsyncPostBackTimeout="2000" runat="server" EnablePartialRendering="true">
            <Scripts >
				<asp:ScriptReference Path="~/Scripts/jquery-2.0.3.min.js" />
                <asp:ScriptReference Path="~/Scripts/jquery.validate.min.js" />
           </Scripts >
        </ajaxToolkit:ToolkitScriptManager><table style=""></table>
        <uc1:ConfirmAlertBox ID="ConfirmAlertBox1" runat="server" />
    
    </div>
    </form>
</body>
</html>
