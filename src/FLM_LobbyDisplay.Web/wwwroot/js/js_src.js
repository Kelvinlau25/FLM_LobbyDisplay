
function changeStyle() {
	if(event.srcElement.type == "button" || event.srcElement.type == "submit" || event.srcElement.type == "reset")
		event.srcElement.style.borderStyle = "outset";
	if(event.srcElement.type == "text" || event.srcElement.type == "password")
		event.srcElement.style.borderColor = "black";
}

function unchangeStyle() {
	if(event.srcElement.type == "button" || event.srcElement.type == "submit" || event.srcElement.type == "reset")
		event.srcElement.style.borderStyle = "solid";
	if(event.srcElement.type == "text" || event.srcElement.type == "password")
		event.srcElement.style.borderColor = "#c0c0c0";
}

function refresh()
{
  opener.location.reload()
}

function refresh1()
{
  window.reload()
}

function formatCurrency(num) {
  num = num.toString().replace(/\|\,/g,'');
  if(isNaN(num))
    num = "0";
  sign = (num == (num = Math.abs(num)));
  num = Math.floor(num*100+0.50000000001);
  cents = num%100;
  num = Math.floor(num/100).toString();
  
  if(cents<10)
    cents = "0" + cents;
  for (var i = 0; i < Math.floor((num.length-(1+i))/3); i++)
      num = num.substring(0,num.length-(4*i+3))+','+
  num.substring(num.length-(4*i+3));
  return (((sign)?'':'-') + '' + num + '.' + cents);
}

function caps() {
document.forms[0].elements[0].value = document.forms[0].elements[0].value.toUpperCase()
}

function numCheck() {
  if (event.keyCode < 45 || event.keyCode > 57) 
      event.returnValue = false;
}	  

function sendValue1(str){
  var projectno = str
  window.opener.document.form1.cboSearch1.value = projectno;
  window.close();
}

function sendValue2(str){
  var projectno = str
  window.opener.document.form2.cboSearch1.value = projectno;
  window.close();
}

function sendValue3(str){
  var projectno = str
  window.opener.document.form1.cboSearch1.value = projectno;
  window.close();
}

function sendValue_to(str){
  var toEmailAddress = str
  window.opener.document.form1.to.value = toEmailAddress;
  window.close();
}

function sendValue_cc(str){
  var ccEmailAddress = str
  window.opener.document.form1.cc.value = ccEmailAddress;
  window.close();
}

function sendValue_bcc(str){
  var bccEmailAddress = str
  window.opener.document.form1.bcc.value = bccEmailAddress;
  window.close();
}
