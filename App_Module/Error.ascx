<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Error.ascx.vb" Inherits="App_Module_ErrorReport" %>
<!--[if IE 6]>
<style>
    .errors{height:80px; overflow-y:auto;}
</style>
<![endif]-->
<asp:ValidationSummary ID="vsSummary" CssClass="errors" runat="server" />
