<%@ Page Language="VB" MasterPageFile="~/master/Main.master" AutoEventWireup="false" CodeFile="Display_Mst.aspx.vb" Inherits="Acc_TVDisplay_Mst" title="Signage Display" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 150px;
        }
         .styler
        {
            color :Red ;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Cbutton" runat="server">    
  <table width="100%" border="0" cellspacing="0" cellpadding="0">
<tr>
    <td>
<div>
<table width="100%">
<tr>
<td>
 <h2><asp:Label ID="Label8" runat="server" Text="Digital Signage Display (Engineering Room)" 
         Font-Bold="True"></asp:Label></h2><hr />
</td>
</tr>
</table>
</div>
 <br />
<div>
        
             <table>   
            <tr>
            <td class="style1">         
                <asp:Button ID="BtnDisp1" runat="server" Text="Display 1" Font-Bold="True" 
                    Font-Size="Medium" Height="50px" Width="130px" />
            </td>
            <td class="style1">   
             <asp:Button ID="BtnDisp2" runat="server" Text="Display 2" Font-Bold="True" 
                    Font-Size="Medium" Height="50px" Width="130px" />
            </td>
            <td class="style1">   
                 &nbsp;</td>  
            <td class="style1">   
                 &nbsp;</td>       
                <td class="style1">
                    &nbsp;</td>
            </tr>
     
           </table>
                </div>
                 <br />
            
                  <br />
                 <div></div>
                 </td>      
</tr>
</table>
 </asp:Panel>   
 </asp:Content>

