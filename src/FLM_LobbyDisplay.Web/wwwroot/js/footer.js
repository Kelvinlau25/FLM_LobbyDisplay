
//enter the message you wish to be shown, including html tags
var message='<font color=000000 size=1><center>&copy; 2001 Capex Expenditure  Approval System. All Rights Reserved. <br>Pen-Group Companies. Terms & Conditions of Use Privacy Policy.</center></font>'

//enter a color name to be used as the background color of the message
var backgroundcolor="white"

//enter 0 for always display, 1 for a set period, 2 for random display mode
var displaymode=0

//if displaymode is set to display for a set period, enter the period below (1000=1 sec)
var displayduration=10000

//enter 0 for non-flashing message, 1 for flashing
var flashmode=0
//if above is set to flashing, enter the flash-to color below
var flashtocolor= "white"//"lightgreen"




///////////////do not edit below this line////////////////////////////////////////


function regenerate(){
window.location.reload()
}

var which=0

function regenerate2(){
if (document.layers)
setTimeout("window.onresize=regenerate",400)
}


function display2(){
if (document.layers){
if (topmsg.visibility=="show")
topmsg.visibility="hide"
else
topmsg.visibility="show"
}
else if (document.all){
if (topmsg.style.visibility=="visible")
topmsg.style.visibility="hidden"
else
topmsg.style.visibility="visible"
setTimeout("display2()",Math.round(Math.random()*10000)+10000)
}
}

function flash(){
if (which==0){
if (document.layers)
topmsg.bgColor=flashtocolor
else
topmsg.style.backgroundColor=flashtocolor
which=1
}
else{
if (document.layers)
topmsg.bgColor=backgroundcolor
else
topmsg.style.backgroundColor=backgroundcolor
which=0
}
}


if (document.all){
document.write('<span id="topmsg" style="position:absolute;visibility:hidden;width: 60%">'+message+'</span>')
}


function logoit(){
document.all.topmsg.style.left=document.body.scrollLeft+document.body.clientWidth/2-document.all.topmsg.offsetWidth/2
document.all.topmsg.style.top=document.body.scrollTop+document.body.clientHeight-document.all.topmsg.offsetHeight-4
}


function logoit2(){
topmsg.left=pageXOffset+window.innerWidth/2-topmsg.document.width/2
topmsg.top=pageYOffset+window.innerHeight-topmsg.document.height-5
setTimeout("logoit2()",90)
}

function setmessage(){
document.all.topmsg.style.left=document.body.scrollLeft+document.body.clientWidth/2-document.all.topmsg.offsetWidth/2
document.all.topmsg.style.top=document.body.scrollTop+document.body.clientHeight-document.all.topmsg.offsetHeight-4
document.all.topmsg.style.backgroundColor=backgroundcolor
document.all.topmsg.style.visibility="visible"
if (displaymode==1)
setTimeout("topmsg.style.visibility='hidden'",displayduration)
else if (displaymode==2)
display2()
if (flashmode==1)
setInterval("flash()",1000)
window.onscroll=logoit
window.onresize=new Function("window.location.reload()")
}


function setmessage2(){
topmsg=new Layer(window.innerWidth)
topmsg.bgColor=backgroundcolor
regenerate2()
topmsg.document.write(message)
topmsg.document.close()
logoit2()
topmsg.visibility="show"
if (displaymode==1)
setTimeout("topmsg.visibility='hide'",displayduration)
else if (displaymode==2)
display2()
if (flashmode==1)
setInterval("flash()",1000)
}


if (document.layers)
window.onload=setmessage2
else if (document.all)
window.onload=setmessage


///////////////do not edit below this line////////////////////////////////////////
//             display the tooltips when the mouse point over 
///////////////do not edit below this line////////////////////////////////////////

 