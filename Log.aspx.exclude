<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="Log.aspx.vb" Inherits="Log" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<link href="<%= ResolveUrl("~/css/Detail.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= ResolveUrl("~/css/PenGroup.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= ResolveUrl("~/css/prodSystem.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= ResolveUrl("~/css/stylesheetmsdn.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%= ResolveUrl("~/js/jquery-1.7.1.min.js") %>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/js/common.js") %>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/js/commonsearch.js") %>"  type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <control:main id="UCTitle" runat="server" Audit="true" />
    <div id="divDesc" runat="server"></div><br />
    <table width="100%">
        <tr>
            <td>
                 <asp:GridView ID="grdResult" RowStyle-CssClass="content" HeaderStyle-CssClass="title_bar" Width="100%" runat="server" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True">
                    <PagerSettings Visible="False" />
                    <RowStyle CssClass="content"></RowStyle>
                        <Columns>           
                            <asp:BoundField HeaderStyle-Width="20%" HeaderText="Field" DataField="FieldName"> 
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" CssClass="content" VerticalAlign="Middle" />
                            </asp:BoundField>  
                            <asp:BoundField HeaderStyle-Width="20%" HeaderText="Old Value" DataField="B4Update"> 
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" CssClass="content" VerticalAlign="Middle" />
                            </asp:BoundField>  
                            <asp:BoundField HeaderStyle-Width="20%" HeaderText="New Value" DataField="AFUpdate"> 
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" CssClass="content" VerticalAlign="Middle" />
                            </asp:BoundField>  
                            <asp:BoundField HeaderStyle-Width="20%" HeaderText="User" DataField="UpdateBy"> 
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" CssClass="content" VerticalAlign="Middle" />
                            </asp:BoundField>  
                            <asp:BoundField HeaderText="Time" DataField="UpdatedDate"> 
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" CssClass="content" VerticalAlign="Middle" />
                            </asp:BoundField>  
                        </Columns>
                        <PagerStyle HorizontalAlign="Right" />
                    <HeaderStyle CssClass="title_bar"></HeaderStyle>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <control:Footer ID="UCFooter" runat="server" Audit="true" />
    <div style="width:100%;text-align:center"><asp:Button ID="btnclose" runat="server" OnClientClick="window.open('','_self','');window.close();" Text="Close" /></div>
</asp:Content>


