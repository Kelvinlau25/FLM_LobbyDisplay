/*****************************************************************************
java script to initialize the link description for main.asp page
*****************************************************************************/
var description = new Array(); 
description[0] = "<p align=left>Please select one of the option to proceed</p>";
description[1] = "<p align=left>An option to do <b><font color=black>New Project Registration</font></b>, <b><font color=black>New Project Application</font></b>, <b><font color=black>Fixed Assets Disposal/Write Off</font></b> and <b><font color=black>Fixed Assets Transfer</font></b></p>";
description[2] = "<p align=left>Send <b><font color=black>email notification</font></b> to Controllers for confirmation of Registered Project (Budget)</p>";
description[3] = "<p align=left>An <b><font color=black>approval update</font></b> option that allow Secretaries to update approved <b><font color=black>Project Application</font></b>, <b><font color=black>Fixed Assets Disposal/Write Off</font></b> and Controllers to update <b><font color=black>Fixed Assets Transfer</font></b></p>";
description[4] = "<p align=left><b><font color=black>Print</font></b> application form for <b><font color=black>New Purchase</font></b>, <b><font color=black>Fixed Assets Disposal/Write Off</font></b> and <b><font color=black>Fixed Assets Transfer Form</font></b></p>";
description[5] = "<p align=left>Inquiry of <b><font color=black>Registered Project Listing</font></b>, <b><font color=black>Applied Project Listing</font></b>, <b><font color=black>Project Utilisation Status</font></b>, <b><font color=black>Project Details</font></b> and <b><font color=black>Fixed Assets Details</font></b></p>";
description[6] = "<p align=left>Print Summary Listing Report for <b><font color=black>New Purchase</font></b>,<b><font color=black>Fixed Assets Disposal/Write Off</font></b>, <b><font color=black>Fixed Assets Transfer</font></b>, <b><font color=black>Project Utilisation Report By PR/PO</font></b> and <b><font color=black>Project Follow Up Reports</font></b></p>";
description[7] = "<p align=left>An update process option that allow Controllers to run <b><font color=black>Year End Process</font></b> and <b><font color=black>Update Registered (Budgeted) Projects</font></b></p>";
description[8] = "<p align=left>File maintenance option that allow Controllers to maintain data in database master table file</p>";
description[9] = "<p align=left>Sign off the system before close up the screen</p>";
description[10] = "";
/*****************************************************************************
end script
*****************************************************************************/

/*****************************************************************************
end script
*****************************************************************************/
imgOver = "../image/on.gif"
imgOut = "../image/out.gif"
imgOut_m = "../image/off.gif"

function imageOver(imgname){
     imgname.src = imgOver
}

function imageOut(imgname){
     imgname.src = imgOut
}

function imageOut_m(imgname){
     imgname.src = imgOut_m
}
/*****************************************************************************
end script
*****************************************************************************/
