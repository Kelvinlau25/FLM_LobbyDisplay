/*****************************************************************************
java script to initialize the link description for main.asp page
*****************************************************************************/
/*<!-- Begin*/
nextfield = "userid"; // name of first box on page
netscape = "";
ver = navigator.appVersion; len = ver.length;
for(iln = 0; iln < len; iln++) if (ver.charAt(iln) == "(") break;
netscape = (ver.charAt(iln+1).toUpperCase() != "C");

function keyDown(DnEvents) { // handles keypress
// determines whether Netscape or Internet Explorer
k = (netscape) ? DnEvents.which : window.event.keyCode;
if (k == 13) { // enter key pressed
if (nextfield == 'done') return true; // submit, we finished all fields
else { // we're not done yet, send focus to next box
eval('document.form1.' + nextfield + '.focus()');
return false;
      }
   }
}
document.onkeydown = keyDown; // work together to analyze keystrokes
if (netscape) document.captureEvents(Event.KEYDOWN|Event.KEYUP);
/*  End -->
/*****************************************************************************
end script
*****************************************************************************/
/*
Example:
========
<form name=yourform>
Box 1: <input type=text name=box1 onFocus="nextfield ='box2';"><br>
Box 2: <input type=text name=box2 onFocus="nextfield ='box3';"><br>
Box 3: <input type=text name=box3 onFocus="nextfield ='box4';"><br>
Box 4: <input type=text name=box4 onFocus="nextfield ='done';"><br>
<input type=submit name=done value="Submit">
</form>
</center>*/
