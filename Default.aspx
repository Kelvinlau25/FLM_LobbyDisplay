<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=8" /> 
    <title><%=ConfigurationManager.AppSettings("title")%></title>
    <link href="css/SignIn.css" rel="stylesheet" type="text/css" />
</head>
<body>
        <table width="100%" style="height:100%;" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td>
                    <table style="margin:auto;height:500px;" border="0" cellpadding="0" cellspacing="0" >
                        <tr>
                            <td>
                                <form id="form2" runat="server">
                                    <asp:ValidationSummary ID="vsSummary" ValidationGroup="login" ShowMessageBox="true" ShowSummary="false" DisplayMode="BulletList" runat="server" />
                                    <asp:CustomValidator runat="server" id="cusCustom" ValidationGroup="login" onservervalidate="cusCustom_ServerValidate" Display="None" errormessage="Invalid password and username!" />
                                    <div class="centered">
                                      <table>
                                        <tr>
                                            <td class="form">
                                                <div>
                                                    <table>
                                                        <tr>
                                                            <td colspan="2" class="header"><img src="image/1.png" width="500px" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <table class="login">
                                                                    <tr id="trcompanyselection" runat="server">
                                                                        <td>Company : </td>
                                                                        <td><asp:DropDownList ID="ddlcompany" runat="server"></asp:DropDownList></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>User Id : </td>
                                                                        <td><asp:TextBox ID="txtusername" runat="server" MaxLength="50" CssClass="value"></asp:TextBox></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Password : </td>
                                                                        <td><asp:TextBox ID="txtpassword" TextMode="Password" runat="server" MaxLength="50" CssClass="value"></asp:TextBox></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td></td>
                                                                        <td><asp:ImageButton ID="btnSubmit" ValidationGroup="login" runat="server" ImageUrl="image/button-login.gif" />&nbsp;<a  style="display:none;"  href="ChangePassword.aspx">Changed Password</a></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                      </table>
                                    </div>
                                </form>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
</body>
</html>
