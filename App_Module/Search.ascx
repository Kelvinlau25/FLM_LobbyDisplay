<%@ Control Language="VB" ClassName="Search" AutoEventWireup="false" CodeFile="Search.ascx.vb" EnableViewState="true" Inherits="Search" %>
<div id='searchMsg'>
    <h3><asp:Label ID="lblSearch" runat="server" Text="Basic Search" /></h3>
    <asp:Panel ID="pnlBasic" runat="server">
        <script type="text/javascript">
            function BasicSearchChecker() {
                if ($.trim($(".basicsearch").val()).length == 0) {
                    if (confirm('Continue without search?') == false) {
                        $(".basicsearch").focus();
                        return false;
                    } else {
                        return true;
                    }
                };

                if ($.trim($(".bcddlsearch").val()) == '-') {
                    alert('Please select the search selection!');
                    $(".bcddlsearch").focus();
                    return false
                }

                var value = $(".basicsearch").val();
                if (/'/.test(value) == true) {
                    alert('Invalid Character in the search value!');
                    $(".basicsearch").focus();
                    return false
                }
                 
                return true;
            };
        </script>
        <table width="640" border="0" cellspacing="0" cellpadding="2">
            <tr>
                <td><asp:DropDownList CssClass="bcddlsearch" ID="ddlSearch" runat="server"></asp:DropDownList></td>
                <td width="120">
                    <asp:TextBox ID="txtSearch" CssClass="basicsearch" runat="server" TabIndex="2" Width="150px"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClientClick="return BasicSearchChecker();" Width="74px" />
                </td>
                <td width="305">
                    <asp:LinkButton ID="lbAdvSearch" runat="server">Advance Search</asp:LinkButton>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlAdvance" runat="server">
        <script type="text/javascript">
            function AdvSearchChecker() {
                if ($(".searchCriteria").find("option").length == 0) {
                    if (confirm('Continue without search?') == false) {
                        $(".advddlsearch").focus();
                        return false;
                    } else {
                        return true;
                    }
                }

                return true;
            };

            function AddConditionCheck() {
                if ($.trim($(".searchusing").val()).length == 0) {
                    alert('The search value not allow empty!');
                    $(".searchusing").focus();
                    return false
                };

                var value = $(".searchusing").val();
                if (/'/.test(value) == true) {
                    alert('Invalid Character in the search value!');
                    $(".searchusing").focus();
                    return false
                }

                if ($.trim($(".advddlsearch").val()) == '-') {
                    alert('Please select the search selection!');
                    $(".advddlsearch").focus();
                    return false
                };

                if ($.trim($(".advoperator1").val()) == '-') {
                    alert('Please select the first operator!');
                    $(".advoperator1").focus();
                    return false
                };

                if ($(".searchCriteria").find("option").length > 0) {
                    if ($.trim($(".advoperator2").val()) == '-') {
                        alert('Please select the second operator!');
                        $(".advoperator2").focus();
                        return false
                    };
                };

                return true;
            }
        </script>
        <table>
            <tr>
                <td>Search Using</td>
                <td>Operator</td>
                <td>Show Record Containing</td>
                <td>Operator</td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="ddlSearchUsing" CssClass="advddlsearch" runat="server"></asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="ddlOperator1" CssClass="advoperator1" runat="server">
                        <asp:ListItem>-</asp:ListItem>
                        <asp:ListItem Value="=">=</asp:ListItem>
                        <asp:ListItem Value="&gt;">&gt;</asp:ListItem>
                        <asp:ListItem Value="&lt;">&lt;</asp:ListItem>
                        <asp:ListItem Value="&gt;=">&gt;=</asp:ListItem>
                        <asp:ListItem>&lt;=</asp:ListItem>
                        <asp:ListItem>LIKE</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:TextBox ID="txtSearchUsing" CssClass="searchusing" runat="server" Width="150px"></asp:TextBox>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSearch2" CssClass="advoperator2" runat="server">
                        <asp:ListItem>-</asp:ListItem>
                        <asp:ListItem Value="AND">AND</asp:ListItem>
                        <asp:ListItem Value="OR">OR</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btnPlus" runat="server" OnClientClick="return AddConditionCheck();" Text="+" />
                </td>
            </tr>
            <tr>
                <td colspan="5">Search Criteria</td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:ListBox ID="searchCriteria" class="searchCriteria" runat="server" Width="377px" />
                </td>
                <td>
                    <asp:Button ID="btnMinus" runat="server" Text="-" />
                </td>
                <td>
                    <asp:Button ID="btnSubmit2" runat="server" Text="Submit Query" OnClientClick="return AdvSearchChecker();" />
                </td>
                <td>
                    <asp:Button ID="btnReset" runat="server" Text="Reset" OnClientClick="return confirm('Are you going to delete all the search condition?')" />
                </td>
                <td>
                    <asp:LinkButton ID="lbBasicSearch" runat="server">Basic Search</asp:LinkButton>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <table>
        <tr>
            <td>
                <asp:CheckBox ID="chkDeleted" runat="server" Text="Show Deleted Records " AutoPostBack="true" Width="345px" />
            </td>
        </tr>
    </table>
</div>

