<%@ Control Language="VB" ClassName="GridFooter" AutoEventWireup="false" CodeFile="GridFooter.ascx.vb" Inherits="UserControl_GridFooter" %>
<table class="navigatorbar" width="100%">
    <tr>
        <th align="right" valign="middle" style="height: 20px">
            <asp:Label ID="lblTotalRecord" runat="server" Text="Total of 0 records found " Height="20" />
            &nbsp;<asp:ImageButton ID="btnFirst" runat="server" ImageUrl="~/image/first.gif" Height="20" />
            &nbsp;<asp:ImageButton ID="btnPrevious" runat="server" ImageUrl="~/image/prior.gif" Height="20"  />
            &nbsp;<asp:DropDownList ID="ddlPage" runat="server" AutoPostBack="True" Height="20" />
            &nbsp;<asp:ImageButton ID="btnNext" runat="server" ImageUrl="~/image/next.gif" Height="20" />
            &nbsp;<asp:ImageButton ID="btnLast" runat="server" ImageUrl="~/image/last.gif" Height="20" />
        </th>
    </tr>
</table>
