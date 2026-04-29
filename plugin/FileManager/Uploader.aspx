<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Uploader.aspx.cs" Inherits="FileManager_Uploader" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/ui-lightness/jquery-ui-1.8.4.custom.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .hide
        {
            display:none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="ui-widget-content" style="height:28px;border:none; padding-left: 5px;" >
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td width="180px"><asp:FileUpload ID="FileUpload1" runat="server" Width="180px" /> </td>
                <td width="60px"><asp:Button ID="bntUpload" runat="server" Text="upload" onclick="bntUpload_Click" Width="60px" /> </td>
                <td>&nbsp;&nbsp;
                    <span id="spanerror" runat="server" style="color:Red; font-size: 11pt; height:25px;"></span>
                    <asp:TextBox CssClass="hide" ID="txtPath" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
