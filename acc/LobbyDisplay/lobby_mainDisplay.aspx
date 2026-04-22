<%@ Page Language="VB" AutoEventWireup="false" CodeFile="lobby_mainDisplay.aspx.vb" Inherits="Acc_LobbyDisplay_lobby_mainDisplay" title="Main Display Video" %>

<!DOCTYPE html>
<html>

<script src="<%= ResolveUrl("../../js/jquery-1.7.2.min.js") %>"  type="text/javascript"></script>
<link rel="stylesheet" type="text/css" href="css/marquee.css" />

<style type="text/css">
html {
    margin: 0;
    padding: 0;
    height: 100%;
    background-color:black;
}

body {
    margin: 0;
    padding: 0;
    height: 96%;
    max-height: 96%;
    float: left;
    width: 100%;
	overflow: hidden
}
</style>

<head id="Head1" runat="server">
</head>

<body onload="onload();" style="height:96vh;">
    <video preload="auto" id="idle_video" width="100%" height="100%" onended="onVideoEnded();" style="object-fit: fill;" muted>
	Sorry, your browser doesn't support embedded videos.
	</video>
    
   <script type="text/javascript">

        var video_list      = <%= video_lists %>;
        var seek_starts     = <%= seek_starts %>;
        var seek_ends       = <%= seek_ends %>;
        var period_starts   = <%= period_starts %>; 
        var period_ends     = <%= period_ends %>;
		
		
		function loaded(vl,ss,se,ps,pe)
           {

             var vla = new Array();
             vla = vl.split(",");
             video_list  = vla;

             var ssa = new Array();
             ssa = ss.split(",");  
             seek_starts = ssa;
             
             var sea = new Array();
             sea = se.split(",");  
             seek_ends   = sea;
             
             var psa = new Array();
             psa = ps.split(",");  
             period_starts = psa;
             
             var pea = new Array();
             pea = pe.split(",");  
             period_ends = pea;

            //alert("loaded : " + seek_starts)
            var refresh=600000; // Refresh rate in milli seconds //60000
            mytime=setTimeout('refreshdata()',refresh);
           }
           
        function loaded2()
           {
         
            var refresh2=2000; // Refresh rate in milli seconds
            mytime2=setTimeout('refreshdata()',refresh2);
           }
           
           
        function refreshdata()
           {
             __doPostBack('TimerPanel','');
           }    	
		
        var video_index     = 0;
        var video_player    = null;
        var sys_time        = new Date();
		var options 		= { year: 'numeric', month: 'long', day: 'numeric' };		
		var sys_date		= (new Date).toLocaleDateString("en-US", options) + " ";
		var seek_start 		= seek_starts[video_index];
		var seek_end 		= seek_ends[video_index];
        var period_start    = new Date(sys_date + period_starts[video_index]);
        var period_end      = new Date(sys_date + period_ends[video_index]);
		var x 				= 0;
		
	


        function onload(){
            video_player        = document.getElementById("idle_video");
            video_player.setAttribute("src", video_list[video_index]);
			
			document.getElementById('idle_video').addEventListener('loadedmetadata', function() {
			  this.currentTime = seek_start;
			}, false);

			if (sys_time.getTime() >= period_start.getTime() && sys_time.getTime() <= period_end.getTime()){
            video_player.play();
            }else{
			video_player.setAttribute("src", "");
			onVideoEnded();
			}
			
			document.getElementById('idle_video').addEventListener('timeupdate', function() {
			if(seek_end != 0){
			  if(this.currentTime > seek_end){
				onVideoEnded()
			  }
			}
			});
        }

        function onVideoEnded(){
		
			x++;
			if (x > 10) {
				delay(function(){
					window.location.reload();
				}, 60000 ); // end delay
			}

            if(video_index < video_list.length - 1){
                video_index++;
				seek_start = seek_starts[video_index].replace(/'/g,"");
				seek_end = seek_ends[video_index].replace(/'/g,"");
				period_start    = new Date(sys_date + period_starts[video_index].replace(/'/g,""));
				period_end      = new Date(sys_date + period_ends[video_index].replace(/'/g,""));
            }
            else{
                video_index = 0;
				seek_start = seek_starts[video_index].replace(/'/g,"");
				seek_end = seek_ends[video_index].replace(/'/g,"");	
				period_start    = new Date(sys_date + period_starts[video_index].replace(/'/g,""));
				period_end      = new Date(sys_date + period_ends[video_index].replace(/'/g,""));
            }
            video_player.setAttribute("src", video_list[video_index].replace(/'/g,""));
            if (sys_time.getTime() >= period_start.getTime() && sys_time.getTime() <= period_end.getTime()){
            video_player.play();
            }else{
			video_player.setAttribute("src", "");
            onVideoEnded();
            }
        }
		
		var delay = ( function() {
			var timer = 0;
			return function(callback, ms) {
				clearTimeout (timer);
				timer = setTimeout(callback, ms);
			};
		})();
		
    </script>
	
</body>
<footer style="height:100%;">

	<form runat="server">
       <asp:ScriptManager id="ScriptManager1" runat="server" AsyncPostBackTimeout="360000">
			</asp:ScriptManager>
		<asp:UpdatePanel ID="TimerPanel" runat="server" UpdateMode="Conditional">    
			<ContentTemplate>
				<div class='marquee'>
					<div class='marquee-text'>
					<%=scrollingText%>
					</div>
				</div>
			</ContentTemplate>
		</asp:UpdatePanel>   
    </form>  
	

<script>
   
            $(function(){
                onPageLoad();
            });

            function onPageLoad(){
            var marquee = $('div.marquee');
            marquee.each(function() {
                var mar = $(this),indent = mar.width();
                mar.marquee = function() {
                    indent--;
                    mar.css('text-indent',indent);
                    if (indent < -1 * mar.children('div.marquee-text').width()) {
                        indent = mar.width();
                    }
                };
                mar.data('interval',setInterval(mar.marquee,1000/100));
            });
            }
			
			
			var prm = Sys.WebForms.PageRequestManager.getInstance();
			if (prm != null) {
				prm.add_endRequest(function (sender, e) {
					if (sender._postBackSettings.panelsToUpdate != null) {
						if (e.get_error() != null) {
							var ex = e.get_error();
							var mesg = "HttpStatusCode: " + ex.httpStatusCode;
							mesg += "\n\nName: " + ex.name;
							mesg += "\n\nMessage: " + ex.message;
							mesg += "\n\nDescription: " + ex.description;
							console.log(mesg);
							e.set_errorHandled(true);
						delay(function(){
							refreshdata();
						}, 60000 ); // end delay
		
						}
					}
				});
			};
        
</script>
</footer>
</html>