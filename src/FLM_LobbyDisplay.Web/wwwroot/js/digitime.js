function show2(){
   var Digital=new Date()
   var hours=Digital.getHours()
   var minutes=Digital.getMinutes()
   var seconds=Digital.getSeconds()
   var dn="AM" 
   if (hours>12){
   dn="PM"
   hours=hours-12
   }
   if (hours==0)
      hours=12
   if (minutes<=9)
      minutes="0"+minutes
   if (seconds<=9)
      seconds="0"+seconds
   
   var ctime="<font face='arial' size=2 color='#FFFFFF'>"+hours+":"+minutes+":"+seconds+" "+dn+"</font>"
   if (!document.all)
   document.write(ctime)
   else
   tick2.innerHTML=ctime
 }
 
 function loadclock(){
   if (document.all)
   setInterval("show2()",1000)
 }
 if (!document.all)
 show2()
