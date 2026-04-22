<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewFile.aspx.cs" Inherits="Pages_Article_PlayVideo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    
    <style type="text/css">
        * { overflow: hidden; padding:0; margin:0; }
        body { overflow: hidden; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        
        <% if (_IsPreviousLink) { %>
        <div style="margin-bottom:7px;">
            <a href="javascript:history.go(-1);">
                <img src="<%= ResolveUrl("~/images/back" + ImageLanguagePrefix + ".png") %>" border="0" />
            </a>
            <div class="clear"></div>
        </div>
        <div class="clear"></div>
        <% } %>
        
        <% if (_IsValid) {%>
        <iframe width="100%" height="544px" src="<%= _V %>"></iframe>
        <% }%>
        
        <script language="javascript" type="text/javascript">
            <asp:Literal ID="litJava" runat="server"></asp:Literal>
        </script>
    </form>
</body>
</html>
