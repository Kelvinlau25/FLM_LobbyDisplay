<%@ Page Language="C#" MasterPageFile="~/master/Main.master" AutoEventWireup="true" CodeFile="Upper2ndScreen_Dtl.aspx.cs" Inherits="acc_MstMain_Upper2ndScreen_Dtl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script src="<%= ResolveUrl("~/js/jquery-1.4.2.min.js") %>" type="text/javascript"></script>
    <link href="<%= ResolveUrl("~/css/overcast/jquery-ui-1.8.18.custom.css") %>" rel="stylesheet" type="text/css"/>
    <script src="<%= ResolveUrl("~/js/jquery-ui-1.8.9.custom.min.js") %>"  type="text/javascript"></script>
    <style type="text/css">
        .style1
        {
            width: 70px;
        }
        .style2
        {
            width: 99px;
        }
        .style5
        {
            width: 407px;
            height: 30px;
        }
        .style6
        {
            height: 30px;
        }
        .style7
        {
            width: 407px;
        }
          .styler
        {
           color :Red;
        }
        
      .style7 input
      {
      	width:700px;
      	}
    </style>
	
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <h2>
     <asp:Label ID="lblMainTitle" runat="server"></asp:Label></h2>
 <hr />
  
    <control:Error ID="Error1" runat="server" ValidationGroup="Group1"/>
    
     <asp:Panel ID="pnlmodify" runat="server">
        <table class="detailinfo" width="100%">
        <tr>
            <td class="style1"><asp:Label ID="Label3" runat="server" Text ="*" CssClass ="styler" /> Attach File: </td>
            <td class="style7">
               <asp:FileUpload runat ="server"  ID="fileupload" />
               <asp:Label ID="lblFileUpload" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr> 
            <td class="style1"><asp:Label ID="Label1" runat="server" Text ="*" CssClass ="styler" /> Seek Start: </td>
            <td class="style6">
                <asp:TextBox ID="txt_skstart" runat="server" Text="0" Style="text-align:right;"></asp:TextBox>
                <span style="font-size: x-small; color: #FF0000">Leave Value as Zero for Default Setting.</span>
            </td>
        </tr>    
        <tr>  
            <td class="style1"><asp:Label ID="Label2" runat="server" Text ="*" CssClass ="styler" /> Seek End: </td>
            <td class="style6">
                <asp:TextBox ID="txt_skstop" runat="server" Text="0" Style="text-align:right;"></asp:TextBox>
                <span style="font-size: x-small; color: #FF0000">Leave Value as Zero for Default Setting.</span>
            </td>
        </tr>
        <tr> 
            <td class="style1"><asp:Label ID="Label4" runat="server" Text ="*" CssClass ="styler" /> Start Time: </td>
            <td class="style6">
                <asp:TextBox ID="txt_tstart" runat="server" Text="00:00" Style="text-align:right;"></asp:TextBox>
                <span style="font-size: x-small; color: #FF0000">Leave Value as 00:00 for Default Setting. (24hr Time Format ##:##)</span>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat=server ValidationGroup="Group1" Display="None"
                        ControlToValidate="txt_tstart" 
                        ErrorMessage="Start Time must be in 24hr Time Format (##:##)." 
                        ValidationExpression="([01]?[0-9]|2[0-3]):[0-5][0-9]" />
            </td>
        </tr>    
        <tr>  
            <td class="style1"><asp:Label ID="Label5" runat="server" Text ="*" CssClass ="styler" /> End Time: </td>
            <td class="style6">
                <asp:TextBox ID="txt_tstop" runat="server" Text="23:59" Style="text-align:right;"></asp:TextBox>
                <span style="font-size: x-small; color: #FF0000">Leave Value as 23:59 for Default Setting. (24hr Time Format ##:##)</span>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat=server ValidationGroup="Group1" Display="None"
                       ControlToValidate="txt_tstop" 
                       ErrorMessage="End Time must be in 24hr Time Format (##:##)." 
                       ValidationExpression="([01]?[0-9]|2[0-3]):[0-5][0-9]" />
            </td>
        </tr>           
        </table>
                        <asp:ValidationSummary 
                              id="valSum" 
                              DisplayMode="BulletList"
                              ShowMessageBox="true" 
                              runat="server"
                              HeaderText="Validation Errors:"
                              Font-Names="verdana" 
                              Font-Size="12"/>
    </asp:Panel>
    
     <asp:Panel ID="pnlAudit" runat="server">
      <table class="detailinfo" width="100%">
        
        <tr>
            <td class="style1">Created By: </td>
            <td class="style7"><asp:Label ID="lblcreatedby" runat ="server" /></td>
        </tr>
        
        <tr>
            <td class="style1">Created Date: </td>
            <td class="style7"><asp:Label ID="lblcreateddate" runat ="server" /></td>
        </tr>
        
        <tr>
            <td class="style1">Created Loc: </td>
            <td class="style7"><asp:Label ID="lblcreatedloc" runat ="server" /></td>
        </tr>
        
          <tr>
            <td class="style1">Updated By: </td>
            <td class="style7"><asp:Label ID="lblupdatedby" runat ="server" /></td>
        </tr>
        
        <tr>
            <td class="style1">Updated Date: </td>
            <td class="style7"><asp:Label ID="lblupdateddate" runat ="server" /></td>
        </tr>
        
        <tr>
            <td class="style1">Updated Loc: </td>
            <td class="style7"><asp:Label ID="lblupdatedloc" runat ="server" /></td>
        </tr>
       
    </table>
    </asp:Panel> 
   
     <asp:Label ID="lblerror" runat ="server" Text ="No Records Found" Visible ="false"  />
   <asp:Button ID="btnsumit" runat="server" Text="Submit" ValidationGroup="Group1" OnClick="btnSubmit_Click"/>
  <asp:Button ID="btnback" runat="server" PostBackUrl ="~/acc/MstMainPan/MM_VerticalScreenFull.aspx" Text="Back" CausesValidation ="False"  />
  
</asp:Content>
