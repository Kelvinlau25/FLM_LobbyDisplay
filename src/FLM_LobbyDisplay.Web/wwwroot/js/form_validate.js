  //begin form validate for default
  function validate()
      {
        if ((document.form1.userid.value=="")&&
            (document.form1.passw.value==""))
            //(document.form1.cboCompany.value=="")
        {
        alert ("You need to submit user id, password & company name to sign in!")
        return false
        }
        if (document.form1.userid.value=="")
        {
        alert ("User id required!")
        return false
        }
        if (document.form1.passw.value=="")
        {
        alert ("Password required!")
        return false
        }
        //if (document.form1.cboCompany.value=="")
       // {
        //alert ("Please select your company!")
        //return false
        //}
  }
  //form validate end
  
  //form validation for new purchase&registration
  function validate1()
  {
        if (document.form1.cboSearch.value=="")
        { alert ("Please select the project No. to search!")
          return false
        }
		if (document.form1.cboSearch1.value.length < 14)
		{ alert ("The length of Project No you have enter \n cannot be less or more than 10 characters!")
          return false
        }
  }
  function validate01()
  {
        if ((document.form1.cboSearch1.value=="")&&(document.form1.typeApp.value==""))
        { alert ("Please fill up all the entry and proceed to create new application!")
          return false
        }
        if (document.form1.cboSearch1.value=="")
        { alert ("You need to specify which registered Project No. to create new application!")
          return false
        }
        if (document.form1.typeApp.value=="")
        { alert ("Which type of application you wish to create? Single or Multiple")
          return false
        }
		if (document.form1.cboSearch1.value.length < 10)
		{ alert ("The length of registered Project No you have enter \n cannot be less or more than 10 characters!")
          return false
        }
  }
  
  function validate011()
  {
        if (document.form1.cboSearch1.value=="")
        { alert ("Please enter the asset code to dispose or write off !")
          return false
        }
		if (document.form1.cboSearch1.value.length < 14)
		{ alert ("The length of asset code you have enter \n cannot be less or more than 14 characters!")
          return false
        }
  }		
  
  function validate0111()
  {
        if (document.form1.cboSearch1.value=="")
        { alert ("Please enter the asset code to transfer !")
          return false
        }
		if (document.form1.cboSearch1.value.length < 14)
		{ alert ("The length of asset code you have enter \n cannot be less or more than 14 characters!")
          return false
        }
  }		
  
  function validate02()
  {
        if (document.form1.cboSearch1.value=="")  
        { alert ("Please type any project no to search!")
          return false
        }
  }
  function validate03()
  {
        if (document.form2.cboSearch1.value=="")  
        { alert ("Please type any project no to search!")
          return false
        }
		//if (document.form2.cboSearch1.value.length < 10)
		//{ alert ("The length of project no you have enter \n cannot be less or more than 10 characters!")
        // return false
        //}
  }
  
  function validate2()
  {  
        if ((document.form2.txtDeptCode.value=="")&&(document.form2.txtCapExp.value=="")&&
            (document.form2.txtPurpose.value=="")&&(document.form2.txtStatus.value=="")&&
            (document.form2.txtYear.value=="")&&(document.form2.txtSerialNo.value=="") )
        { alert ("Please fill up all the entry before submit!")
          return false
        } 
        if (document.form2.txtDeptCode.value=="")
        { alert ("Department code required!")
          return false
        }
        if (document.form2.txtCapExp.value=="")
        { alert ("Capitalise/expense code required")
          return false
        }
        if (document.form2.txtPurpose.value=="")
        { alert ("Purpose code required!")
          return false
        }
        if (document.form2.txtStatus.value=="")
        { alert ("Status code required")
          return false
        }
        if (document.form2.txtYear.value=="")
        { alert ("Please specify the year!")
          return false
        }
        if (document.form2.txtSerialNo.value=="")
        { alert ("What is the serial no.!")
          return false
        }
  }
  function validate20()
  {  
        if ((document.form5.txtDeptCode.value=="")&&(document.form5.txtCapExp1.value=="")&&
            (document.form5.txtPurpose.value=="")&&(document.form5.txtStatus.value=="")&&
            (document.form5.txtYear.value=="")&&(document.form5.txtSerialNo.value=="") )
        { alert ("Please fill up all the entry before submit!")
          return false
        } 
        if (document.form3.cboSearch.value=="")
        { alert ("Please specify which registred project No. at section (1) that you want to create as subsequence application!")
          return false
        }
        if (document.form5.txtDeptCode.value=="")
        { alert ("Department code required!")
          return false
        }
        if (document.form5.txtCapExp1.value=="")
        { alert ("Capitalise/expense code required")
          return false
        }
        if (document.form5.txtPurpose.value=="")
        { alert ("Purpose code required!")
          return false
        }
        if (document.form5.txtStatus.value=="")
        { alert ("Status code required")
          return false
        }
        if (document.form5.txtYear.value=="")
        { alert ("Please specify the year!")
          return false
        }
        if (document.form5.txtSerialNo.value=="")
        { alert ("What is the serial no.!")
          return false
        }
        
  }
  //end form validate
  
  //validate form new purchase registration
  function validate3()
      {
        if ((document.form1.txtDeptCode.value=="")&&(document.form1.txtCapExp.value=="")&&
            (document.form1.txtPurpose.value=="")&&(document.form1.txtStatus.value=="")&&
            (document.form1.txtYear.value=="")&&(document.form1.txtSerialNo.value=="")&&
            (document.form1.txtUserDept1.value=="")&&(document.form1.txtUserDept2.value=="")&&
            (document.form1.txtDeptCode1.value=="")&&(document.form1.txtRemark.value=="")&&
            (document.form1.txtDeptCode2.value=="")&&(document.form1.txtAmount.value=="")&&
            (document.form1.cboBudget.value=="")&&(document.form1.txtAppName.value=="")&&
            (document.form1.txtFromDate.value=="")&&(document.form1.txtSubject.value=="")&&
            (document.form1.txtToDate.value=="")&&(document.form1.txtPurpose.value=="")&&
            (document.form1.txtProcessMc1.value=="")&&(document.form1.txtProcessMc2.value=="")&&
            (document.form1.txtDocDate.value=="")&&(document.form1.typeApp.value==""))
        { alert ("Please fill up all the entry before update!")
          return false
        }
        if (document.form1.txtDeptCode.value=="")
        { alert ("Department code of of Project No. required!")
          return false
        }
        if (document.form1.txtCapExp.value=="")
        { alert ("Capitalise/expense code of Project No. required")
          return false
        }
        if (document.form1.txtPurpose.value=="")
        { alert ("Purpose code of Project No. required!")
          return false
        }
        if (document.form1.txtStatus.value=="")
        { alert ("Status code of Project No. required")
          return false
        }
        if (document.form1.txtYear.value=="")
        { alert ("Please specify the year of Project No.!")
          return false
        }
        if (document.form1.txtSerialNo.value=="")
        { alert ("What is the serial no. of Project No.!")
          return false
        }
        if (document.form1.txtUserDept1.value=="")
        { alert ("User department code required!")
          return false
        }
        if (document.form1.txtUserDept2.value=="")
        { alert ("User department name required!")
          return false
        }
        if (document.form1.txtDeptCode1.value=="")
        { alert ("Applicant department code required")
          return false
        }
        if (document.form1.txtDeptCode2.value=="")
        { alert ("Applicant department number required")
          return false
        }
        if (document.form1.cboBudget.value=="")
        { alert ("Budget value required!")
          return false
        }
        if (document.form1.txtAppName.value=="")
        { alert ("Applicant name required!")
          return false
        }
        if (document.form1.txtSubject.value=="")
        { alert ("Subject details required!")
          return false
        }
        if (document.form1.txtPurpose.value=="")
        { alert ("Purpose value required!")
          return false
        }
        if (document.form1.txtFromDate.value=="")
        { alert ("Please make sure you specify the Date From!")
          return false
        }
        if (document.form1.txtToDate.value=="")
        { alert ("Please make sure you specify the To Date!")
          return false
        }
        if (document.form1.txtRemark.value=="")
        { alert ("Explanation of contents required!")
          return false
        }
        if (document.form1.txtAmount.value=="")
        { alert ("Budget Amount required!")
          return false
        }
        //if (document.form1.cboAppBy.value=="")
        //{ alert ("The project to be approved by?")
        //  return false
        //}
        if (document.form1.txtProcessMc1.value=="")
        { alert ("Process/machine code required!")
          return false
        }
        if (document.form1.txtProcessMc2.value=="")
        { alert ("Process/machine name required!")
          return false
        }
        //if (document.form1.typeApp.value=="")
        //{ alert ("Please specify the application type!")
        //  return false
        //}
        //if (document.form1.txtAppliedAmt.value=="")
        //{ alert ("Applied Amount required!")
        //  return false
        //}
  }
  //end validate
  
  //validate form new purchase application
  function validate30()
      {
        if ((document.form1.txtDeptCode.value=="")&&(document.form1.txtCapExp.value=="")&&
            (document.form1.txtPurpose.value=="")&&(document.form1.txtStatus.value=="")&&
            (document.form1.txtYear.value=="")&&(document.form1.txtSerialNo.value=="")&&
            (document.form1.txtUserDept1.value=="")&&(document.form1.txtUserDept2.value=="")&&
            (document.form1.txtDeptCode1.value=="")&&(document.form1.txtRemark.value=="")&&
            (document.form1.txtDeptCode2.value=="")&&(document.form1.txtAppliedAmt.value=="")&&
            (document.form1.cboBudget.value=="")&&(document.form1.txtAppName.value=="")&&
            (document.form1.txtFromDate.value=="")&&(document.form1.txtSubject.value=="")&&
            (document.form1.txtToDate.value=="")&&(document.form1.txtPurpose.value=="")&&
            (document.form1.txtProcessMc1.value=="")&&(document.form1.txtProcessMc2.value=="")&&
            (document.form1.txtDocDate.value=="")&&(document.form1.cboAppBy.value=="")&&
            (document.form1.txtFurtherRemark.value==""))
        { alert ("Please fill up all the entry before update!")
          return false
        }
        if (document.form1.txtDeptCode.value=="")
        { alert ("Department code of of Project No. required!")
          return false
        }
        if (document.form1.txtCapExp.value=="")
        { alert ("Capitalise/expense code of Project No. required")
          return false
        }
        if (document.form1.txtPurpose.value=="")
        { alert ("Purpose code of Project No. required!")
          return false
        }
        if (document.form1.txtStatus.value=="")
        { alert ("Status code of Project No. required")
          return false
        }
        if (document.form1.txtYear.value=="")
        { alert ("Please specify the year of Project No.!")
          return false
        }
        if (document.form1.txtSerialNo.value=="")
        { alert ("What is the serial no. of Project No.!")
          return false
        }
        if (document.form1.txtUserDept1.value=="")
        { alert ("User department code required!")
          return false
        }
        if (document.form1.txtUserDept2.value=="")
        { alert ("User department name required!")
          return false
        }
        if (document.form1.txtDeptCode1.value=="")
        { alert ("Applicant department code required")
          return false
        }
        if (document.form1.txtDeptCode2.value=="")
        { alert ("Applicant department number required")
          return false
        }
        if (document.form1.cboBudget.value=="")
        { alert ("Budget value required!")
          return false
        }
        if (document.form1.txtAppName.value=="")
        { alert ("Applicant name required!")
          return false
        }
        if (document.form1.txtSubject.value=="")
        { alert ("Subject details required!")
          return false
        }
        if (document.form1.txtPurpose.value=="")
        { alert ("Purpose value required!")
          return false
        }
        if (document.form1.txtFromDate.value=="")
        { alert ("Please make sure you specify the Date From!")
          return false
        }
        if (document.form1.txtToDate.value=="")
        { alert ("Please make sure you specify the To Date!")
          return false
        }
        if (document.form1.txtRemark.value=="")
        { alert ("Explanation of contents required!")
          return false
        }
        //if (document.form1.txtAmount.value=="")
        //{ alert ("Budget Amount required!")
        //  return false
        //}
        if (document.form1.txtAppliedAmt.value=="")
        { alert ("Applied Amount required!")
          return false
        }
        if (document.form1.cboAppBy.value=="")
        { alert ("The project to be approved by?")
          return false
        }
        if (document.form1.txtProcessMc1.value=="")
        { alert ("Process/machine code required!")
          return false
        }
        if (document.form1.txtProcessMc2.value=="")
        { alert ("Process/machine name required!")
          return false
        }
        //if (document.form1.typeApp.value=="")
        //{ alert ("Please specify the application type!")
        //  return false
        //}
        
  }
  //end validate
  
  //form validate for new purchase fixed asset
  function validate4()
      {
        if ((document.form1.txtProjectNo.value=="")&&(document.form1.txtAmount.value=="")&&
            (document.form1.txtAppAmt.value=="")&&(document.form1.txtSubject.value=="")&&
            (document.form1.txtPurpose.value==""))
        { alert ("Please fill up all the entry before submit!")
          return false
        }
        if (document.form1.txtProjectNo.value=="")
        { alert ("Please select the project No. to search!")
          return false
        }
        if (document.form1.txtAmount.value=="")
        { alert ("Amount value required!")
          return false
        }
        if (document.form1.txtAppAmt.value=="")
        { alert ("Applied amount value required")
          return false
        }
        if (document.form1.txtSubject.value=="")
        { alert ("Subject value required")
          return false
        }
        if (document.form1.txtPurpose.value=="")
        { alert ("Purpose value required!")
          return false
        }
 }
 //end validate 
 
 //form validate for new purchase user details
 function validate5()
      {
        if ((document.form1.txtProjectNo.value=="")&&(document.form1.txtSubject.value=="")&&
            (document.form1.txtPurpose.value=="")&&(document.form1.lineno.value=="")&&
            (document.form1.amount.value=="")&&(document.form1.desc.value=="")&&
            (document.form1.fromdate.value=="")&&(document.form1.todate.value=="")) 
        { alert ("Please fill up all the entry before submit!")
          return false
        }
        if (document.form1.txtProjectNo.value=="")
        { alert ("Please select the project No. to search!")
          return false
        }
        if (document.form1.txtSubject.value=="")
        { alert ("Subject value required")
          return false
        }
        if (document.form1.txtPurpose.value=="")
        { alert ("Purpose value required!")
          return false
        }
        if (document.form1.lineno.value=="")
        { alert ("Line number required!")
          return false
        }
        if (document.form1.desc.value=="")
        { alert ("Description required!")
          return false
        }
        if (document.form1.amount.value=="")
        { alert ("Please specify the amount!")
          return false
        }
        if (document.form1.fromdate.value=="")
        { alert ("Please specify the date from!")
          return false
        }
        if (document.form1.todate.value=="")
        { alert ("Please specify the date to!")
          return false
        }
        //if (document.form1.txtAmount.value < document.form1.txtCost.value)
        //{ alert ("The total amount already exceed the budget amount! You are not allow to add any details.")
        //  return false
        //}
 }
 //validate end
 
 //form validate for new purchase applied amount
 function validate6()
      {
        if ((document.form.txtAmount.value=="")&&
            (document.form.lineno.value=="")&&(document.form.appAmt.value=="")&&
            (document.form.appDate.value=="")&&(document.form.appNo.value=="")&&
            (document.form.approveDate.value==""))
            
        { alert ("Please fill up all the entry before submit!")
          return false
        }
        if (document.form.txtAmount.value=="")
        { alert ("Please specify the applied amount!")
          return false
        }
        //if (document.form.cboAppBy.value=="")
        //{ alert ("Approve by value required!")
        //  return false
        //}
        if (document.form.lineno.value=="")
        { alert ("Line number required!")
          return false
        }
        if (document.form.appAmt.value=="")
        { alert ("Applied Amount value required!")
          return false
        }
        if (document.form.appDate.value=="")
        { alert ("Applied date value required!")
          return false
        }
        //if (document.form.appNo.value=="")
        // { alert ("Approval no. value required!")
        //  return false

        //}
        //if (document.form.approveDate.value=="")
        //{ alert ("Approve date required!")
        //  return false
        //}
        //if (document.form.txtTempTotal.value > document.form.txtAmount.value)
        //{ var message
        //  message = "Please make sure the total applied amount is not exceed budget amount!\n"
        //  message = message + "Otherwise, the update will not be continued!"
        //  alert (message)
        //  return false
        //}
 }
 //validate end
 
 //form validate for new purchase approval entry
      function validate7()
      {
        if (document.form1.cboSearch.value=="")
        { alert ("Please select the project number to approve!")
          return false
        }
      } 
      function validate8()
      {
        if ((document.form2.txtPurpose.value=="")&&(document.form2.txtDepartment.value=="")&&
            (document.form2.txtAppNo.value=="")&&(document.form2.txtAppAmount.value=="")&&
            (document.form2.txtAppDate.value=="")&&(document.form2.txtAppBy.value=="")&&
            (document.form2.cboMode.value==""))
            
        { alert ("Please fill up all the entry before update!")
          return false
        } 
        if (document.form2.txtPurpose.value=="")
        { alert ("Purpose required!")
          return false
        }
        if (document.form2.txtDepartment.value=="")
        { alert ("Department required!")
          return false
        }
        if (document.form2.txtAppNo.value=="")
        { alert ("Approval no.required!")
          return false
        }
        if (document.form2.txtAppAmount.value=="")
        { alert ("Approval amount required!")
          return false
        }
        if (document.form2.txtAppDate.value=="")
        { alert ("Approval date required!")
          return false
        }
        if (document.form2.txtAppBy.value=="")
        { alert ("Approved by required!")
          return false
        }
        if (document.form2.cboMode.value=="")
        { alert ("Approval decision required!")
          return false
        }
      }
//end form validate for new purchase approval entry

//form validate for new purchase Details info Editor
 function validate8()
      {
        if ((document.form.desc.value=="")&&(document.form.amount.value=="")&&
            (document.form.fromdate.value=="")&& (document.form.todate.value=="")&&
            (document.form.lineno.value==""))
            
        { alert ("Please fill up all the entry before submit!")
          return false
        }
        if (document.form.desc.value=="")
        { alert ("Description required!")
          return false
        }
        if (document.form.amount.value=="")
        { alert ("Please specify the amount!")
          return false
        }
        if (document.form.fromdate.value=="")
        { alert ("Please specify the date from!")
          return false
        }
        if (document.form.todate.value=="")
        { alert ("Please specify the date to!")
          return false
        }
 }
 //validate end 

 function validate9()
      {
        if ((document.form.cboAppBy.value=="")&&(document.form.appAmt.value=="")&&
            (document.form.appDate.value=="")&& (document.form.subject2.value=="")&&
            (document.form.purpose2.value=="")&&(document.form.remark2.value=="")&&
            (document.form.fromDate.value=="")&&(document.form.toDate.value=="")&&
            (document.form.appName.value==""))
            
        { alert ("Please fill up all the entry before save!")
          return false
        }
        if (document.form.cboAppBy.value=="")
        { alert ("Line number required!")
          return false
        }
        if (document.form.subject2.value=="")
        { alert ("Subjenct required!")
          return false
        }
        if (document.form.appAmt.value=="")
        { alert ("Applied amount required!")
          return false
        }
        if (document.form.fromDate.value=="")
        { alert ("Date from required!")
          return false
        }
        if (document.form.toDate.value=="")
        { alert ("To date required!")
          return false
        }
        if (document.form.appName.value=="")
        { alert ("Applicant name required!")
          return false
        }
        if (document.form.appDate.value=="")
        { alert ("Applied date required!")
          return false
        }
}
//validate end 
  
function validate10()
      {
        if (document.form1.cboSearch.value=="")
        { alert ("Please specify the project number that you want to approve!")
          return false
        }
}
//validate end 

function validate11()
{
        if ((document.form1.cboDecision.value=="")&&(document.form1.txtRemark1.value=="")&&
            (document.form1.txtRemark2.value=="")&&(document.form1.txtAppDate.value=="")&&
            (document.form1.txtProNo.value=="")&&(document.form1.txtDepartment.value=="")&&
            (document.form1.txtYear.value=="")&&(document.form1.txtCompany.value=="")&&
            (document.form1.txtAccAppAmt.value=="")&&(document.form1.txtAppBy.value=="")&&
            (document.form1.txtAppNo.value=="")&&(document.form1.txtAppAmount.value=="")&&
            (document.form1.txtAccAppAmt.value=="")&&(document.form1.txtAppBy.value=="")&&
            (document.form1.appno.value==""))
                
        { alert ("Please fill up all the entry before submit!")
          return false
        }
        if (document.form1.txtAppDate.value=="")
        { alert ("Approve date required!")
          return false
        }
        if (document.form1.cboDecision.value=="")
        { alert ("Decision required!")
          return false
        }
        
}
//validate end 

function validate12()
      {
        if (document.form1.cboSearch.value=="")
        { alert ("Please select the project number to approve!")
          return false
        }
} 
function validate13()
{
        if ((document.form2.txtPurpose.value=="")&&(document.form2.txtDepartment.value=="")&&
            (document.form2.txtAppNo.value=="")&&(document.form2.txtAppAmount.value=="")&&
            (document.form2.txtAppDate.value=="")&&(document.form2.txtAppBy.value=="")&&
            (document.form2.cboMode.value==""))
            
        { alert ("Please fill up all the entry before update!")
          return false
        } 
        if (document.form2.txtPurpose.value=="")
        { alert ("Purpose required!")
          return false
        }
        if (document.form2.txtDepartment.value=="")
        { alert ("Department required!")
          return false
        }
        if (document.form2.txtAppNo.value=="")
        { alert ("Approval no.required!")
          return false
        }
        if (document.form2.txtAppAmount.value=="")
        { alert ("Approval amount required!")
          return false
        }
        if (document.form2.txtAppDate.value=="")
        { alert ("Approval date required!")
          return false
        }
        if (document.form2.txtAppBy.value=="")
        { alert ("Approved by required!")
          return false
        }
        if (document.form2.cboMode.value=="")
        { alert ("Approval decision required!")
          return false
        }
}

function validate14()
      {
        if ((document.form2.txtAssetCode.value=="")&&(document.form2.txtDescription.value=="")&&
            (document.form2.txtModelNo.value=="")&&(document.form2.txtSerialNo.value=="")&&
            (document.form2.txtDepartment.value=="")&&(document.form2.txtApplicant.value=="")&&
            (document.form2.txtDate.value=="")&&(document.form2.txtReason.value=="")&&
            (document.form2.txtDepartment1.value=="")&&(document.form2.txtName.value==""))
            
        { alert ("Please fill up all the entry before update!")
          return false
        } 
        if (document.form2.txtAssetCode.value=="")
        { alert ("Asset code required!")
          return false
        }
        if (document.form2.txtDescription.value=="")
        { alert ("Description required!")
          return false
        }
        if (document.form2.txtModelNo.value=="")
        { alert ("Model no.required!")
          return false
        }
        if (document.form2.txtSerialNo.value=="")
        { alert ("Serial no. required!")
          return false
        }
        if (document.form2.txtApplicant.value=="")
        { alert ("Applicant name required!")
          return false
        }
        if (document.form2.txtDepartment.value=="")
        { alert ("Tranferor's department required!")
          return false
        }
        if (document.form2.txtDate.value=="")
        { alert ("Effective Date required!")
          return false
        }
        if (document.form2.txtReason.value=="")
        { alert ("Reason required!")
          return false
        }
        if (document.form2.txtDepartment1.value=="")
        { alert ("Tranferee's department required!")
          return false
        }
        if (document.form2.txtName.value=="")
        { alert ("Tranferee's name required!")
          return false
        }
}

function validate15()
      {
        if ((document.form.seqno.value=="")&&(document.form.explanation.value==""))
        { alert ("Please fill up all the entry before update!")
          return false
        } 
        if (document.form.seqno.value=="")
        { alert ("Sequence No. required!")
          return false
        }
        if (document.form.explanation.value=="")
        { alert ("You need to fill up the explanation to update!")
          return false
        }
}

function disableButton()
{
  if (event.button == 2)  
  {
    alert("Sorry! the button function has been disabled!")
  }
}

//function remote(){
// window.open("new_purchase_appAmt.asp","","width=600,height=400,scrollbars")
// creator=self
//}

function showWindow() {
	self.name = "Project Management Control System";  
	var windowprops = "toolbar=0,location=0,directories=0,status=0, " +
	"menubar=0,scrollbars=1,resizable=0,width=600,height=500,left=70,top=180";
	OpenWindow = window.open("new_purchase_appAmt.asp", "remote", windowprops); // opens remote control
}

function showEdit(lineno) {
	self.name = "Project Management Control System";  
	var windowprops = "toolbar=0,location=0,directories=0,status=0, " +
	"menubar=0,scrollbars=0,resizable=0,width=420,height=280,left=250,top=200";
	OpenWindow = window.open("new_purchase_detail_edit.asp?lineno=" + lineno + "", "remote", windowprops); // opens remote control
}

function showAmtEdit(linecode) {
	self.name = "Project Management Control System"; 
	var windowprops = "toolbar=0,location=0,directories=0,status=0, " +
	"menubar=0,scrollbars=0,resizable=0,width=550,height=510,left=180,top=140";
	OpenWindow = window.open("new_purchase_appAmt_edit.asp?lineno=" + linecode + "", "remote", windowprops); 
}

function showPurposeDetail() {
	self.name = "Project Management Control System"; 
	var windowprops = "toolbar=0,location=0,directories=0,status=0, " +
	"menubar=0,scrollbars=0,resizable=0,width=360,height=300,left=200,top=150";
	OpenWindow = window.open("purpose_desc.asp", "remote", windowprops); // opens remote control
}

function showApproveAuthorities_capex() {
	self.name = "Project Management Control System"; 
	var windowprops = "toolbar=0,location=0,directories=0,status=0, " +
	"menubar=0,scrollbars=0,resizable=0,width=380,height=300,left=200,top=150";
	OpenWindow = window.open("approving_authorities_capex.asp", "remote", windowprops); // opens remote control
}

function showApproveAuthorities_disposal() {
	self.name = "Project Management Control System"; 
	var windowprops = "toolbar=0,location=0,directories=0,status=0, " +
	"menubar=0,scrollbars=0,resizable=0,width=380,height=300,left=200,top=150";
	OpenWindow = window.open("approving_authorities_disposal.asp", "remote", windowprops); // opens remote control
}

function showAbout() {
    self.name = "About"; 
	var windowprops = "toolbar=0,location=0,directories=0,status=0, " +
	"menubar=0,scrollbars=0,resizable=0,width=280,height=205,left=280,top=150";
	OpenWindow = window.open("about.asp", "remote", windowprops); // opens remote control
}

function showProjectCodingFormat() {
    self.name = "Project Management Control System"; 
	var windowprops = "toolbar=0,location=0,directories=0,status=0, " +
	"menubar=0,scrollbars=1,resizable=0,width=600,height=400,left=100,top=70";
	OpenWindow = window.open("project_coding_format.asp", "remote", windowprops); // opens remote control
}

function showApprovalCodingFormat() {
    self.name = "Project Management Control System"; 
	var windowprops = "toolbar=0,location=0,directories=0,status=0, " +
	"menubar=0,scrollbars=1,resizable=0,width=600,height=400,left=100,top=70";
	OpenWindow = window.open("approval_coding_format.asp", "remote", windowprops); // opens remote control
}

function showSearchRegisteredForm() {
    self.name = "Project Management Control System";  
	var windowprops = "toolbar=0,location=0,directories=0,status=0, " +
	"menubar=0,scrollbars=1,resizable=0,width=600,height=400,left=100,top=70";
	OpenWindow = window.open("search_registered_list.asp", "remote", windowprops); // opens remote control
}

function showSearchAppliedForm(type,dbname) {
    self.name = "Project Management Control System";  
	var windowprops = "toolbar=0,location=0,directories=0,status=0, " +
	"menubar=0,scrollbars=1,resizable=0,width=600,height=400,left=100,top=70";
	OpenWindow = window.open("search_applied_list.asp?type="+type+"&dbname="+dbname+"", "remote", windowprops); // opens remote control
}

function showSearchApproveAssetForm(dbname) {
    self.name = "Project Management Control System"; 
	var windowprops = "toolbar=0,location=0,directories=0,status=0, " +
	"menubar=0,scrollbars=1,resizable=0,width=600,height=400,left=100,top=70";
	OpenWindow = window.open("search_approve_asset_list.asp?dbname="+dbname+"", "remote", windowprops); // opens remote control
} 

function showSearchTransferAssetForm() {
    self.name = "Project Management Control System"; 
	var windowprops = "toolbar=0,location=0,directories=0,status=0, " +
	"menubar=0,scrollbars=1,resizable=0,width=600,height=400,left=100,top=70";
	OpenWindow = window.open("search_transfer_asset_list.asp", "remote", windowprops); // opens remote control
}

function showSearchApprovedTransferAssetForm() {
    self.name = "Project Management Control System"; 
	var windowprops = "toolbar=0,location=0,directories=0,status=0, " +
	"menubar=0,scrollbars=1,resizable=0,width=600,height=400,left=100,top=70";
	OpenWindow = window.open("search_approve_transfer_asset_list.asp", "remote", windowprops); // opens remote control
}

function showSearchAssetForm() {
    self.name = "Project Management Control System";  
	var windowprops = "toolbar=0,location=0,directories=0,status=0, " +
	"menubar=0,scrollbars=1,resizable=0,width=600,height=400,left=100,top=70";
	OpenWindow = window.open("search_asset_list.asp", "remote", windowprops); // opens remote control
}

function showReportForm(type,proNo) {
	self.name = "Project Management Control System";  
	var windowprops = "toolbar=0,location=0,directories=0,status=0, " +
	"menubar=0,scrollbars=0,resizable=0,width=800,height=565,left=0,top=0";
	OpenWindow = window.open("../report/approval_form.asp?report=" + type + "&prono="+proNo+"", "remote", windowprops); // opens remote control
}

function showReportForm1(proNo){
	self.name = "Project Management Control System";  
	var windowprops = "toolbar=0,location=0,directories=0,status=0, " +
	"menubar=0,scrollbars=0,resizable=0,width=800,height=565,left=0,top=0";
	OpenWindow = window.open("../report/approval_form1.asp?para=" + proNo + "", "remote", windowprops); // opens remote control
}

function showExplanationForm(type){ 
    self.name = "Project Management Control System"; 
	var windowprops = "toolbar=0,location=0,directories=0,status=0, " +
	"menubar=0,scrollbars=1,resizable=0,width=500,height=360,left=200,top=200";
	OpenWindow = window.open("further_explanation.asp?explan=" + type + "", "remote", windowprops); // opens remote control
}

function showExplanReportForm(projno) {
	self.name = "Project Management Control System"; 
	var windowprops = "toolbar=0,location=0,directories=0,status=0, " +
	"menubar=0,scrollbars=0,resizable=0,width=800,height=565,left=0,top=0";
	OpenWindow = window.open("../report/explanation_form.asp?para=" + projno + "", "remote", windowprops); // opens remote control
}

function showInfoReportForm(projno) {
	self.name = "Project Management Control System"; 
	var windowprops = "toolbar=0,location=0,directories=0,status=0, " +
	"menubar=0,scrollbars=0,resizable=0,width=800,height=565,left=0,top=0";
	OpenWindow = window.open("../report/info_form.asp?para=" + projno + "" ,"remote", windowprops); // opens remote control
}

function showActualSaleWO(assetcode) {
	self.name = "Project Management Control System";  
	var windowprops = "toolbar=0,location=0,directories=0,status=0, " +
	"menubar=0,scrollbars=0,resizable=0,width=350,height=280,left=250,top=200";
	OpenWindow = window.open("disposal_review_cost_edit.asp?ac=" + assetcode + "", "remote", windowprops); // opens remote control
}

function showDisStsCode() {
	self.name = "Project Management Control System"; 
	var windowprops = "toolbar=0,location=0,directories=0,status=0, " +
	"menubar=0,scrollbars=0,resizable=0,width=200,height=250,left=400,top=202";
	OpenWindow = window.open("disposal_status_code.asp", "remote", windowprops); // opens remote control
}

function showAssetGroup() {
	self.name = "Project Management Control System"; 
	var windowprops = "toolbar=0,location=0,directories=0,status=0, " +
	"menubar=0,scrollbars=0,resizable=0,width=300,height=315,left=400,top=180";
	OpenWindow = window.open("disposal_asset_group.asp", "remote", windowprops); // opens remote control
}

function showEmail_TO(address) {
	self.name = "Project Management Control System"; 
	var windowprops = "toolbar=0,location=0,directories=0,status=0, " +
	"menubar=0,scrollbars=0,resizable=0,width=280,height=150,left=352,top=295";
	OpenWindow = window.open("email_address.asp?sendMail="+address+"", "remote", windowprops); // opens remote control
}

function showEmail_CC(address) {
	self.name = "Project Management Control System"; 
	var windowprops = "toolbar=0,location=0,directories=0,status=0, " +
	"menubar=0,scrollbars=0,resizable=0,width=280,height=150,left=352,top=319";
	OpenWindow = window.open("email_address.asp?sendMail="+address+"", "remote", windowprops); // opens remote control
}

function showEmail_BCC(address) {
	self.name = "Project Management Control System"; 
	var windowprops = "toolbar=0,location=0,directories=0,status=0, " +
	"menubar=0,scrollbars=0,resizable=0,width=280,height=150,left=352,top=343";
	OpenWindow = window.open("email_address.asp?sendMail="+address+"", "remote", windowprops); // opens remote control
}

var checkflag = "false";
function check(field) {
	if (checkflag == "false") {
		for (i = 0; i < field.length; i++) {
			field[i].checked = true;
			}
		checkflag = "true";
		return "Uncheck All"; 
	 }
	else {
		for (i = 0; i < field.length; i++) {
			field[i].checked = false; 
			}
		checkflag = "false";
		return "Check All"; 
	}
}

