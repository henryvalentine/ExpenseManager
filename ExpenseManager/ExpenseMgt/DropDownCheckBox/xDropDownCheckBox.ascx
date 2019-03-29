<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="xDropDownCheckBox.ascx.cs" Inherits="megPayPayroll.CoreFramework.DropDownCheckBox.xDropDownCheckBox" %>

<script type = "text/javascript">
    function CheckItem(checkBoxList) {
        var options = checkBoxList.getElementsByTagName('input');
        var arrayOfCheckBoxLabels = checkBoxList.getElementsByTagName("label");
        var s = "";
        for (var i = 0; i < options.length; i++) {
            var opt = options[i];
            if (opt.checked) {
                s = s + ", " + arrayOfCheckBoxLabels[i].innerHTML;   
            }
        }
        if (s.length > 0) {
            s = s.substring(2, s.length); //separate by 'coma'
        }
       
        var txt = checkBoxList.id + 'txtCombo';
        var txtBox = document.getElementById(txt);
        txtBox.value = s.replace('&amp;', ' & ');
        var hf = checkBoxList.id + 'hidVal';
        document.getElementById(hf).value = s;
    }
</script>
<ajaxToolkit:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="chkListtxtCombo" PopupControlID="pnlContainer" Position="Bottom"></ajaxToolkit:PopupControlExtender>
<asp:TextBox ID="chkListtxtCombo" runat="server"  Width="200" CssClass="dd_chk_select_cust" Font-Size="X-Small"></asp:TextBox>
<input type="hidden" name="hidVal" id="chkListhidVal" runat="server" />
<asp:Panel ID="pnlContainer" runat="server" ScrollBars="Vertical" Width="250" style="height: auto; max-height: 150px;" BackColor="AliceBlue" BorderColor="Gray" BorderWidth="1">
    <asp:CheckBoxList ID="chkList" runat="server" style="height: auto;  max-height: 150px;" onclick="CheckItem(this)"></asp:CheckBoxList>
</asp:Panel>