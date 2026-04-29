// JavaScript Document
//var prm = Sys.WebForms.PageRequestManager.getInstance();
//prm.add_initializeRequest(initializeRequest);
//var postbackElement;

//// checks the PageRequestManager if there is already a postback being processed

//// and aborts the postback (question: which postback does it abort?)

//// See details here: http://microsoftmiles.blogspot.com/2006/11/maintaining-gridview-scroll-position-in.html

//// and http://geekswithblogs.net/rashid/archive/2007/08/08/Asp.net-Ajax-UpdatePanel-Simultaneous-Update---A-Remedy.aspx
//function initializeRequest(sender, args) {
//    document.body.style.cursor = "wait";
//    if (prm.get_isInAsyncPostBack()) {

//        //debugger

//        args.set_cancel(true);

//    }

//}
//function endRequest(sender, args) {
//    document.body.style.cursor = "default";

//}

function AutoFillUp(field1)
{    
    var data = field1.id;
    var dataarray = data.split("_");
    document.getElementById(dataarray[0]+"_"+dataarray[1]+"_ddlPastel").value=field1.value;
    document.getElementById(dataarray[0]+"_"+dataarray[1]+"_ddlMedium").value=field1.value;
    document.getElementById(dataarray[0]+"_"+dataarray[1]+"_ddlDark").value=field1.value;
    document.getElementById(dataarray[0]+"_"+dataarray[1]+"_ddlExDark").value=field1.value;
}
function AutoFillUpText(field1)
{    
    var data = field1.id;
    var dataarray = data.split("_");
    document.getElementById(dataarray[0]+"_"+dataarray[1]+"_txtWhite").value=field1.value;
    document.getElementById(dataarray[0]+"_"+dataarray[1]+"_txtPastel").value=field1.value;
    document.getElementById(dataarray[0]+"_"+dataarray[1]+"_txtMedium").value=field1.value;
    document.getElementById(dataarray[0]+"_"+dataarray[1]+"_txtDark").value=field1.value;
    document.getElementById(dataarray[0]+"_"+dataarray[1]+"_txtExDark").value=field1.value;
}
function popupwindow()
{
     
     if(checkSelectItem()==false)
     {
        alert('No selected records to ' + document.getElementById("GridHeader1_ddlAction").value);          
     }
     else
     {        
        if(document.getElementById("GridHeader1_ddlAction").value=="Print")
            window.open('../Print.aspx','',"scrollable=yes,resizable=yes,width=800,height=600,center=yes,status=no,help=no");
        else
        {
            if(document.getElementById("GridHeader1_ddlAction").value=="Email")
                window.open('../Email.aspx','',"scrollable=yes,resizable=yes,width=800,height=600,center=yes,status=no,help=no");
        }         
    }
    document.getElementById("GridHeader1_ddlAction").value="";   
}
function checkSelectItem()
{
        var frm = document.forms[0];

        for (i=0;i<frm.elements.length;i++) {

            if (frm.elements[i].type == "checkbox") {
                if(frm.elements[i].name!='Search1$chkDeleted')
                    if(frm.elements[i].checked)
                        return true;
            }
        }
        return false;
}

/* Select All Checkbox */
function SelectAll(id) {
    var frm = document.forms[0];
    for (i=0;i<frm.elements.length;i++) {
        if (frm.elements[i].type == "checkbox") {
            frm.elements[i].checked = document.getElementById(id).checked;
        }
    }
}

// Let's use a lowercase function name to keep with JavaScript conventions
function selectAll(invoker) {
    // Since ASP.NET checkboxes are really HTML input elements
    //  let's get all the inputs 
    var inputElements = document.getElementsByTagName('input');

    for (var i = 0 ; i < inputElements.length ; i++) {
        var myElement = inputElements[i];
        // Filter through the input types looking for checkboxes
        if (myElement.type === "checkbox") {

           // Use the invoker (our calling element) as the reference 
           //  for our checkbox status
            myElement.checked = invoker.checked;
        }
    }
} 
    
function popemail(url){
//    if (confirm(url))
 window.open(url,"Email","center=yes, scrollbars=yes, resizable=no, statusbar=no, width=800, height=600");
}

function popwindow(url) {
    var width = screen.width * 90 / 100;
    var height = screen.height * 90 / 100;
    var XPost = screen.width * 5 / 100;
    var YPost = screen.height * 5 / 100;
    window.open(url, "", "toolbar=0, scrollbars=1, location=0, statusbar=0, menubar=0, resizable=0, " +
            "width=" + width + ", height=" + height + ", left=" + XPost + ", top=" + YPost);
}

function modalpopwindow(url){
    window.showModalDialog(url,'','dialogWidth:800px,dialogHeight:600pxcenter:yes;status:no;help:no;resizable:no;scroll:yes');
}

function modalpopwindowWithReturnValue(url) {
    return window.showModalDialog(url, '', 'dialogWidth:800px,dialogHeight:600pxcenter:yes;status:no;help:no;resizable:no;scroll:yes');
}

function displaySearch(stat1, stat2){
    document.getElementById("searchPanel").style.display = stat1;
    document.getElementById("searchMssg").style.display = stat2;
}
function comment1(mssg, no){
    prompt("Please key in the reason for " + mssg + " " + no + ":");
}
function callChange(selfid){
    if (document.getElementById(selfid).checked == true) {
        for (var i = 1; ;i++) {
            if (document.getElementById("imagedelete" + i) == null)
                break;
            document.getElementById("imagedelete" + i).style.display = 'none';
            document.getElementById("chk" + i).style.display = 'block';
        }
    }
    else  {
        for (var i = 1; ;i++) {
            if (document.getElementById("imagedelete" + i) == null)
                break;
            document.getElementById("imagedelete" + i).style.display = 'block';
            document.getElementById("chk" + i).style.display = 'none';
        }
    }
}
function callInput(id, queryName, formname)  {
    var inputValue = document.getElementById(id).value;
    var formName = document.getElementsByName(formname)[0];

//    alert ( "inputValue=" + inputValue + ", formName=" + formName + " !!!");

    if(inputValue == "Email"){
        var arrayIds = lg_getAllSelectedIds();
        if(arrayIds.length<=0) {
            alert("No selected records to Email");
            // added by khchai on 1/Jun/06 . To reset the drop down menu back to empty
            document.getElementById(id).value='';
            return;
        }
        var strIds = '';
        for(var i=0; i<arrayIds.length; i++) {
            if(i>0) {
                strIds += ';'
            }
            strIds += arrayIds[i];
        }

        popemail('Email.do?queryName='+queryName+'&action=write&selectedIds='+strIds);

    } else if(inputValue == "Print"){
        var arrayIds = lg_getAllSelectedIds();
        if(arrayIds.length<=0) {
            alert("No selected records to print");
           // added by khchai on 1/Jun/06 . To reset the drop down menu back to empty
          document.getElementById(id).value='';
            return;
        }
        var strIds = '';
        for(var i=0; i<arrayIds.length; i++) {
            if (i > 0) {
                strIds += ';'
            }
            strIds += arrayIds[i];
        }
        popwindow('Print.do?queryName='+queryName+'&selectedIds='+strIds);

    }/* else if (confirm("Are you sure you want to " + document.formName.Action.value + " this/these record(s)?")) {
        prompt("Please insert the reason ");
        for (var i = 1; ;i++) {
            if (document.getElementById("chk" + i) == null)
                break;
            document.getElementById("chk" + i).checked = false;
        }
        document.getElementById(id).value = '';
    } */else {
        document.getElementById(id).value = '';
    }
    // added by khchai on 1/Jun/06 . To reset the drop down menu back to empty
    document.getElementById(id).value='';
}
function checkMultiple(selfid, id)  {
    if (document.getElementById(selfid).checked == true)
        document.getElementById(id).style.display = "block";
    else {
        document.getElementById(id).style.display = "none";
    }
    callChange(selfid);
}

