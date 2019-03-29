<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ErrorDisplay.ascx.cs" Inherits="ExpenseManager.CoreFramework.ErrorControl.ErrorDisplay" %>
<div style="width:100%; padding-left: 4px">
	<table width="100%" style="width: 100%; border: none; padding: 0px"  runat="server" id="tbError">
		<tr>
			<td width="100%" class="errorMessageFrame">
				<div class="errDv"><span style="color:Red; vertical-align:middle; padding-right:2px; font-size:18px; font-weight:bolder">*</span><%= LoadErrorMessage() %></div>
			</td>
		</tr>
	</table>
	<table style="width: 100%; border: none; padding: 0px"  runat="server" id="tbMessage">
			<tr>
				<td width="100%" class="messageFrame">
					<%= LoadPlainMessage()%>
				</td>   
		</tr>
	</table>    
</div>