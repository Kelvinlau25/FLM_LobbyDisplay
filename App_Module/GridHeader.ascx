<%@ Control Language="C#" ClassName="GridHeader" AutoEventWireup="true" CodeFile="GridHeader.ascx.cs" Inherits="UserControl_GridHeader" %>

<script language="javascript">
    function Highlight(row) {
        row.style.backgroundColor = '#DDFFEE';
    }
    
    function UnHighlight(row) {
        row.style.backgroundColor = '#FFFFFF';
    }
</script>

<table width="100%" class="navigatorbar">
    <tr>
        <th valign="middle" align="right">
            <asp:DropDownList ID="ddlAction" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAction_SelectedIndexChanged">
                <asp:ListItem Value="-" Text="-" />
                <asp:ListItem Value="PRINT" Text="Print" />
            </asp:DropDownList>            
            &nbsp;| &nbsp; <a runat="server" id="hypAdd" tabindex="13">Add Item </a>
        </th>
    </tr>
</table>
