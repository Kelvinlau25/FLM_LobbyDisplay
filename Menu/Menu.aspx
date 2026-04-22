<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Menu.aspx.vb" Inherits="Style2_Menu" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=ConfigurationManager.AppSettings("title")%></title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=8" / >
    <link href="../resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <link href="../css/Header.css" rel="stylesheet" type="text/css" />
    <link href="../css/Menu.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.4.3.js" type="text/javascript"></script>
    <script src="../js/ext-base.js" type="text/javascript"></script>
    <script src="../js/ext-all.js" type="text/javascript"></script>
    <script type="text/javascript">    
	    function change_vs(obj) {
	        var vs_id = obj.checked;

	        if (vs_id)
	            Ext.get(obj.id + "-vs").setStyle('display', 'list-item');
	        else
	            Ext.get(obj.id + "-vs").setStyle('display', 'none');
	    }

	    Ext.onReady(function() {

	        // NOTE: This is an example showing simple state management. During development,
	        // it is generally best to disable state management as dynamically-generated ids
	        // can change across page loads, leading to unpredictable results.  The developer
	        // should ensure that stable state ids are set for stateful components in real apps.
	        Ext.state.Manager.setProvider(new Ext.state.CookieProvider());

	        var viewport = new Ext.Viewport({
	            layout: 'border',
	            items: [
                new Ext.BoxComponent({ // raw
                    region: 'north',
                    el: 'north',
                    height: 100
                }),
				{
				    region: 'west',
				    id: 'west-panel',
				    title: 'Menu',
				    split: true,
				    width: 200,
				    minSize: 175,
				    maxSize: 300,
				    collapsible: true,
				    margins: '0 0 0 5',
				    layout: 'accordion',
				    layoutConfig: {
				        animate: true
				    },
				    items: <%= _list.ToString() %>
				},
                {
                    region: 'center',
                    margins: '0',
                    layout: 'column',
                    autoScroll: true,
                    items: [{
                        columnWidth: 1,
                        title: 'Page',
                        contentEl: 'main-div'
                        }]
                    }
             ]
	        });
	    });
	</script>
    <style type="text/css">
        div#ext-gen9{overflow-y:auto;}
        div.home a{color:#FFFFFF}
    </style>
<script type="text/javascript">
		function setIframeHeight(iframeName) {
		  //var iframeWin = window.frames[iframeName];
		  var iframeEl = document.getElementById? document.getElementById(iframeName): document.all? document.all[iframeName]: null;
		  if (iframeEl) {
		  iframeEl.style.height = "auto"; // helps resize (for some) if new doc shorter than previous
		  //var docHt = getDocHeight(iframeWin.document);
		  // need to add to height to be sure it will all show
		  var h = alertSize();
		  var new_h = (h-148);
		  iframeEl.style.height = new_h + "px";
		  //alertSize();
		  }
		}

		function alertSize() {
		  var myHeight = 0;
		  if( typeof( window.innerWidth ) == 'number' ) {
		    //Non-IE
		    myHeight = window.innerHeight;
		  } else if( document.documentElement && ( document.documentElement.clientWidth || document.documentElement.clientHeight ) ) {
		    //IE 6+ in 'standards compliant mode'
		    myHeight = document.documentElement.clientHeight;
		  } else if( document.body && ( document.body.clientWidth || document.body.clientHeight ) ) {
		    //IE 4 compatible
		    myHeight = document.body.clientHeight;
		  }
		  //window.alert( 'Height = ' + myHeight );
		  return myHeight;
		}
	</script>
</head>
<body onload="setIframeHeight('frContent');" onresize="setIframeHeight('frContent');">
  <asp:Literal ID="liItems" runat="server"></asp:Literal>
  <div class="remark" id="main-div">
    <iframe scrolling="auto" runat="server" name="page" frameborder="0" width="100%" id="frContent"></iframe>
  </div>
  <div id="north">
    <div id="divinfo">
        <div>
            <span><%=Me._words%>, <%= Session("gettemp") %> </span>
        </div>
        <br />
        <div class="time">
            <span>Date : <%=Session("LoginHis")%> </span>
        </div>
        <br />
        <div class="home" id="trhome" runat="server">
            <a target="_parent" href='<%= Me.SignOutURL() %>'>Log Out</a>
            <a target="_parent" runat="server" id="ahrefhome">Home</a>
        </div>
        <div class="clear"></div>
    </div>
    <img class="imgheader" src="../image/header1.jpg" />
  </div>
</body>
</html>
