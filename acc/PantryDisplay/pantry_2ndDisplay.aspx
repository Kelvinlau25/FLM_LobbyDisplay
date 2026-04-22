<%@ Page Language="C#" AutoEventWireup="true" CodeFile="pantry_2ndDisplay.aspx.cs" Inherits="acc_PantryDisplay_pantry_2ndDisplay" title="2nd Display Video" %>

<!DOCTYPE html>
<html>
<meta http-equiv="X-UA-Compatible" content="IE=edge" />
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
    <video preload="auto" id="idle_video" width="100%" height="50%" onended="onVideoEnded();" style="object-fit: fill;" muted>
	Sorry, your browser doesn't support embedded videos.
	</video>
	
	<video preload="auto" id="idle_video2" width="100%" height="50%" onended="onVideoEnded2();" style="object-fit: fill;position: relative;top: -5px;" muted>
	Sorry, your browser doesn't support embedded videos.
	</video>
    
   <script type="text/javascript">

        var video_list      = <%= video_lists %> 
        var video_list2     = <%= video_lists2 %> 
        
        var seek_starts     = <%= seek_starts %>
        var seek_ends       = <%= seek_ends %>
        var seek_starts2     = <%= seek_starts2 %>
        var seek_ends2       = <%= seek_ends2 %>
        var period_starts   = <%= period_starts %> 
        var period_ends     = <%= period_ends %>
        var period_starts2   = <%= period_starts2 %> 
        var period_ends2     = <%= period_ends2 %>
		
		
		function loaded(vl,ss,se,ps,pe,vl2,ss2,se2,ps2,pe2)
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
			 
			 var vla2 = new Array();
             vla2 = vl2.split(",");
             video_list2  = vla2;

             var ssa2 = new Array();
             ssa2 = ss2.split(",");  
             seek_starts2 = ssa2;
             
             var sea2 = new Array();
             sea2 = se2.split(",");  
             seek_ends2   = sea2;
             
             var psa2 = new Array();
             psa2 = ps2.split(",");  
             period_starts2 = psa2;
             
             var pea2 = new Array();
             pea2 = pe2.split(",");  
             period_ends2 = pea2;

            //alert("loaded : " + seek_starts)
            var refresh=600000; // Refresh rate in milli seconds
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
        var video_index2     = 0;
        var video_player    = null;
        var video_player2   = null;
        var sys_time        = new Date()
		var options 		= { year: 'numeric', month: 'long', day: 'numeric' };		
		var sys_date		= (new Date).toLocaleDateString("en-US", options) + " ";
		var seek_start 		= seek_starts[video_index];
		var seek_end 		= seek_ends[video_index];
		var seek_start2     = seek_starts2[video_index2];
		var seek_end2 		= seek_ends2[video_index2];
        var period_start    = new Date(sys_date + period_starts[video_index]);
        var period_end      = new Date(sys_date + period_ends[video_index]);
        var period_start2   = new Date(sys_date + period_starts2[video_index2]);
        var period_end2     = new Date(sys_date + period_ends2[video_index2]);
		var x				= 0 
		var x2				= 0


        function onload(){
            video_player        = document.getElementById("idle_video");
            video_player2       = document.getElementById("idle_video2");
            video_player.setAttribute("src", video_list[video_index]);
            video_player2.setAttribute("src", video_list2[video_index2]);
			
			document.getElementById('idle_video').addEventListener('loadedmetadata', function() {
			  this.currentTime = seek_start;
			}, false);
			document.getElementById('idle_video2').addEventListener('loadedmetadata', function() {
			  this.currentTime = seek_start2;
			}, false);

			if (sys_time.getTime() >= period_start.getTime() && sys_time.getTime() <= period_end.getTime()){
            video_player.play();
            }else{
			video_player.setAttribute("src", "");
            onVideoEnded();
            }
            if (sys_time.getTime() >= period_start2.getTime() && sys_time.getTime() <= period_end2.getTime()){
            video_player2.play();
            }else{
			video_player2.setAttribute("src", "");
            onVideoEnded2();
            }
			
			document.getElementById('idle_video').addEventListener('timeupdate', function() {
			if(seek_end != 0){
			  if(this.currentTime > seek_end){
				onVideoEnded()
			  }
			}
			});
			
			
			document.getElementById('idle_video2').addEventListener('timeupdate', function() {
			if(seek_end2 != 0){
			  if(this.currentTime > seek_end2){
				onVideoEnded2()
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
            x = 0;
			video_player.play();
            }else{
			video_player.setAttribute("src", "");
            onVideoEnded();
            }
        }
        
         function onVideoEnded2(){
		 
			x2++;
			if (x2 > 10) {
				delay(function(){
					window.location.reload();
				}, 60000 ); // end delay
			}
			
            if(video_index2 < video_list2.length - 1){
                video_index2++;
				seek_start2 = seek_starts2[video_index2].replace(/'/g,"");
				seek_end2 = seek_ends2[video_index2].replace(/'/g,"");	
				period_start2   = new Date(sys_date + period_starts2[video_index2].replace(/'/g,""));
				period_end2     = new Date(sys_date + period_ends2[video_index2].replace(/'/g,""));	
            }
            else{
                video_index2 = 0;
				seek_start2 = seek_starts2[video_index2].replace(/'/g,"");
				seek_end2 = seek_ends2[video_index2].replace(/'/g,"");
				period_start2   = new Date(sys_date + period_starts2[video_index2].replace(/'/g,""));
				period_end2     = new Date(sys_date + period_ends2[video_index2].replace(/'/g,""));
            }
            video_player2.setAttribute("src", video_list2[video_index2].replace(/'/g,""));
            
			if (sys_time.getTime() >= period_start2.getTime() && sys_time.getTime() <= period_end2.getTime()){
			x2 = 0;
            video_player2.play();
            }else{
			video_player2.setAttribute("src", "");
            onVideoEnded2();
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