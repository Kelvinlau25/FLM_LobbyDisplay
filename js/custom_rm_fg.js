
/* String Trimer */
String.prototype.trim = function() {
    return this.replace(/^\s+|\s+$/g, "");
};

/* Auto Capitalize */
function AutoCapitalize(o) {
    o.value = o.value.toUpperCase().replace(/([^0-9A-Z])/g, "");
}

/* Print Selected Html Element */
function CallPrint(controlid) {
    var prtContent = document.getElementById(controlid);
    var WinPrint = window.open('', '', 'letf=0,top=0,width=400,height=400,toolbar=0,scrollbars=0,status=0');
    WinPrint.document.write(prtContent.innerHTML);
    WinPrint.document.close();
    WinPrint.focus();
    WinPrint.print();
    WinPrint.close();
}

/* Number Formater */
Number.prototype.numberFormat = function(decimals, dec_point, thousands_sep) {
    dec_point = typeof dec_point !== 'undefined' ? dec_point : '.';
    thousands_sep = typeof thousands_sep !== 'undefined' ? thousands_sep : ',';

    var parts = this.toFixed(decimals).toString().split(dec_point);
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, thousands_sep);

    return parts.join(dec_point);
}


/* Multiline Textbox Maxlength Check */
function ControlTextareaMaxlength(control, maxlength) {
    if (control.value.trim().length > maxlength) {
        control.value = control.value.trim().substring(0, maxlength);
    }
}



function VerifyInteger(con, text) {
    var qty = document.getElementById(con).value.trim();
    if (qty == "-" || qty == "+") qty = 0;
    if (qty.length > 0) {
        if (!(!isNaN(parseInt(qty)) && isFinite(qty))) {
            alert(text + ', allow numeric only!');
            document.getElementById(con).focus();
        }
        else if (parseInt(qty) <= -1) {
            alert(text + ', not allow negative value!');
            document.getElementById(con).focus();
        }
    }
}

function VerifyInteger2(con, text) {
    var qty = document.getElementById(con).value.trim();
    if (qty == "-" || qty == "+") qty = 0;
    if (qty.length > 0) {
        if (!(!isNaN(parseInt(qty)) && isFinite(qty))) {
            alert(text + ', allow numeric only!');
            document.getElementById(con).focus();
        }
    }
}

function VerifyFloat(con, text) {
    var qty = document.getElementById(con).value.trim();
    if (qty.length > 0) {
        if (!(!isNaN(parseFloat(qty)) && isFinite(qty))) {
            alert(text + ', allow numeric only!');
            document.getElementById(con).focus();
        }
        else if (parseFloat(qty) <= -1) {
            alert(text + ', not allow negative value!');
            document.getElementById(con).focus();
        }
    }
}