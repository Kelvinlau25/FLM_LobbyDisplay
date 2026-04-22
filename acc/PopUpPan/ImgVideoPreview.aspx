<%@ Page Language="C#" MasterPageFile="~/master/Main.master" AutoEventWireup="true" CodeFile="ImgVideoPreview.aspx.cs" Inherits="acc_PopUp_ImgVideoPreview" title="Untitled Page" %>

<%@ Register Assembly="Media-Player-ASP.NET-Control" Namespace="Media_Player_ASP.NET_Control"
    TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

 
  <script type="text/javascript">
 
 function cls(){
 self.close();
}
   </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <video width="620" height="380" autoplay controls>
	  <source src="<%= paths %>" type="video/mp4">
	Your browser does not support the video tag.
	</video>

</asp:Content>

