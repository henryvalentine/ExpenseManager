<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridPager.ascx.cs" Inherits="ExpenseManager.CoreFramework.GridPager.GridPager" %>
<link href="Assets/pagerStyle.css" rel="stylesheet" type="text/css" />
 <div id="gridDv">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" >
            <tr>
                <td align="left" width="100%" valign="top">
                    <table width="100%" border="0" cellpadding="1" cellspacing="0">
                        <tr>
                            <td width="5%" align="right"><asp:ImageButton runat="server" id="FirstPage" OnClick="PagerButtonClick" CommandArgument="0" ImageUrl="Assets/rrw.png"/></td>
                            <td width="1%" align="center"><img width="1px" height="20px"/></td>
                            <td width="5%" align="left"><asp:ImageButton runat="server" id="PreviousPage" OnClick="PagerButtonClick" CommandArgument="prev" ImageUrl="Assets/prev.png"/></td>
                            <td width="1%"></td>
                            <td width="5%" valign="middle"><asp:TextBox ID="txt_OfText" runat="server" CssClass="tBox2x" ReadOnly="true" Width="50px">0</asp:TextBox></td>
                            <td width="1%"></td>
                            <td width="1%"><font size="3pt" color="black">/</font></td>
                            <td width="1%"></td>
                            <td width="8%" valign="middle"><asp:Label ID="lblTotalPages" runat="server" Text="0" CssClass="lBox2x" Font-Bold="true" Width="40px"></asp:Label></td>
                            <td width="4%" align="right"><asp:ImageButton runat="server" id="NextPage" OnClick="PagerButtonClick" CommandArgument="next" ImageUrl="Assets/next.png"/></td>
                            <td width="1%" align="center"><img width="1px" height="20px"/></td>
                            <td width="5%" align="left"><asp:ImageButton runat="server" id="LastPage" OnClick="PagerButtonClick" CommandArgument="last" ImageUrl="Assets/ffw.png"/></td>
                            <td width="1%" align="center"><img width="1px" height="20px"/></td>
                            <td width="16%" align="right"><span style="font-size:9pt">Total Items</span></td>
                            <td width="1%" align="center"></td>
                            <td width="12%" align="left" valign="middle"><asp:Label ID="lblTotalItems" runat="server" Font-Bold="true" Text="0"  CssClass="lBox2x" Width="40px"></asp:Label></td>
                            <td width="3%" align="center"><img width="1px" height="20px" /></td>
                            <td width="13%" align="right"> <span style="font-size:9pt">Items/Page</span></td>
                            <td width="5%" align="right"> <asp:TextBox ID="Items" runat="server" CssClass="tBox2x" Width="50px">0</asp:TextBox></td>
                            <td width="3%"></td>
                            <td width="15%" align="center"><asp:ImageButton runat="server" id="btnGo"  
                                    CommandArgument="0" ImageUrl="Assets/goBtn.png" onclick="btnGo_Click"/></td>
                        </tr>
                    </table>
                </td>
             </tr>
        </table>
 </div>