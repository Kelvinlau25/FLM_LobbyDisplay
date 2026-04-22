<%@ Control Language="C#" ClassName="GridFooter" AutoEventWireup="true" CodeFile="GridFooter.ascx.cs" Inherits="UserControl_GridFooter" %>
<table class="navigatorbar" width="100%">
    <tr>
        <th align="right" valign="middle" style="height: 20px">
            <asp:Label ID="lblTotalRecord" runat="server" Text="Total of 0 records found " Height="20" />
            &nbsp;<asp:ImageButton ID="btnFirst" runat="server" ImageUrl="~/image/first.gif" Height="20" OnClick="btnFirst_Click" />
            &nbsp;<asp:ImageButton ID="btnPrevious" runat="server" ImageUrl="~/image/prior.gif" Height="20" OnClick="btnPrevious_Click"  />
            &nbsp;<asp:DropDownList ID="ddlPage" runat="server" AutoPostBack="True" Height="20" OnSelectedIndexChanged="ddlPage_SelectedIndexChanged" />
            &nbsp;<asp:ImageButton ID="btnNext" runat="server" ImageUrl="~/image/next.gif" Height="20" OnClick="btnNext_Click" />
            &nbsp;<asp:ImageButton ID="btnLast" runat="server" ImageUrl="~/image/last.gif" Height="20" OnClick="btnLast_Click" />
        </th>
    </tr>
</table>
