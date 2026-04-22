/**
*This function is called to switch between "Advanced Search" and "Basic Search"
*basicSearchID should be defined in jsp/html file before call this function
*@param searchMode search mode "Advance"/"Basic"
*/
function switchSearch(searchMode) {
    if (searchMode.toLowerCase() == "advance") {	//switch to advance search
        top.frames.rightframe.document.getElementById('AdvanceSearch').style.display = 'block';
        top.frames.rightframe.document.getElementById(basicSearchID).style.display = 'none';
    }
    else if (searchMode.toLowerCase() == "basic") {	//switch to basic search
        top.frames.rightframe.document.getElementById('AdvanceSearch').style.display = 'none';
        top.frames.rightframe.document.getElementById(basicSearchID).style.display = 'block';
    }
    top.frames.rightframe.document.getElementsByName("searchMode")[0].value = searchMode;
}

/**
*This Function is called when hit "+" button
*@param searchUsing name of "searchUsing" list
*@param Operator name of "operator" list
*@param searchValue name of "searchValue" text field
*@param condition name of "condition" list
*@param formName name of the form to be submitted
*/
function addSearchCriteria(searchUsing, operator, searchValue, condition, formName) {
    //searchCriteria is a list holding all search criteria
    var criteriaList = top.frames.rightframe.formName.searchCriteria;
    var hiddenList = top.frames.rightframe.formName.hiddenFieldType;
    var thislength = criteriaList.options.length;
    var searchValues = searchValue.value.toUpperCase();
    if (searchUsing.value == "" || searchUsing.value == "-") {
        return;
    } else if ((operator != '' && condition != '') && (operator.value == "" || operator.value == "-")) {
        return;
    } else if (searchValue.value == "" || searchValue.value == "-") {
        return;
    } else if ((operator != '' && condition != '') && (condition.value == "" || condition.value == "-") && (thislength > 0)) {
        alert("the Condition is empty!");
        return;
    } else {
        //text and value of searchUsing is different for text is localized
        var strText1 = searchUsing.options[searchUsing.selectedIndex].text + " ";
        var strValue1 = searchUsing.value + " ";
        var strText2 = "LIKE ";
        if (operator != '') {
            //strText2 = operator.value + " ";
            if (operator.value == 'GT')
                strText2 = ">" + " ";
            else if (operator.value == 'GE')
                strText2 = ">=" + " ";
            else if (operator.value == 'LT')
                strText2 = "<" + " ";
            else if (operator.value == 'LE')
                strText2 = "<=" + " ";
            else strText2 = operator.value + " ";
        }
        var strText3;
        var datatype = hiddenList.options[searchUsing.selectedIndex].text;
        //if((datatype.toLowerCase() != "string") && (strText2 == "LIKE ")) {
        //	strText2 = "= ";
        //}
        // added by khchai 25-Jul-06
        // add one custom datatype called strings
        if ((datatype.toLowerCase() != "string") && (datatype.toLowerCase() != "strings") && (strText2 == "LIKE ")) {
            strText2 = "= ";
        }
        if (datatype.toLowerCase() == "string") {
            if (strText2 == "LIKE ") {
                strText3 = "'%" + searchValues + "%'";
            }
            else {
                strText3 = "'" + searchValues + "'";
            }
        }
        // added by khchai 25-Jul-06.
        // especially for technical module - Sample ID - custom datatype is Strings
        // the search condition just match the back portion
        else if (datatype.toLowerCase() == "strings") {
            //strText3 = "'" + searchValues + "%'";
            if (strText2 == "LIKE ") {
                strText3 = "'" + searchValues + "%'";
            }
            else {
                strText3 = "'" + searchValues + "'";
            }
        }
        else if (datatype.toLowerCase() == "date") {
            //strText3 = "(timestamp '" + searchValues + "')"; //for Borland
            strText3 = "to_date('" + searchValue.value + "','dd/mm/yyyy')"; //for Oracle
        }
        else if (datatype.toLowerCase() == "timestamp") {
            //strText3 = "(timestamp '" + searchValues + "')"; //for Borland
            strText3 = "to_date('" + searchValue.value + "','dd/mm/yyyy hh:mi:ss am')"; //for Oracle
        }
        else {
            strText3 = searchValues;
        }
        var strText4 = "";
        if ((operator != '' && condition != '') && (thislength > 0)) {
            strText4 = condition.value + " ";
        }

        var newOptionText, newOptionValue;
        if ((operator != '' && condition != '') && (parseInt(thislength) > 0)) {	//already has some criteria, need "AND"/"OR"
            newOptionText = strText4 + strText1 + strText2 + strText3;
            //newOptionValue = strText4 + strValue1 + strText2 + strText3;
            newOptionValue = strText4 + "upper( " + strValue1 + " )" + strText2 + strText3;

        } else {	//has no criteria, no need to add "AND"/"OR"
            newOptionText = strText1 + strText2 + strText3;
            //newOptionValue = strValue1 + strText2 + strText3;
            newOptionValue = "upper( " + strValue1 + " )" + strText2 + strText3;

        }

        if (operator != '' && condition != '') {
            criteriaList.options.length = thislength + 1;
            criteriaList.options[thislength] = new Option(newOptionText, newOptionValue);
        }
        else {
            //clear exist search conditions
            criteriaList.options.length = 0;
            //add and select the new added criteria
            var newOptionText = strText1 + strText2 + strText3;
            //var newOptionValue = strValue1 + strText2 + strText3;
            var newOptionValue = "upper( " + strValue1 + " )" + strText2 + strText3;

            criteriaList.options[0] = new Option(newOptionText, newOptionValue);
        }
    }
}


/**
* This Function is called when hit "-" button
*/
function deleteSearchCriteria() {
    //searchCriteria is a list holding all search criteria
    var criteriaList = top.frames.rightframe.formName.searchCriteria;
    var thislength = criteriaList.options.length;
    if (thislength > 0) {
        if (confirm("Are you going to delete the search conditon?") != true) {
            return;
        }
        for (var i = thislength - 1; i >= 0; i--) {
            if (criteriaList.options[i].selected == true) {
                criteriaList.options[i] = null;
            }
        }
    }
    else {
        alert("There is no search condition to delete");
        return;
    }

    if (criteriaList.options.length > 0) {
        //the first criteria need not begin with "AND"/"OR", remove it
        var optionText = criteriaList.options[0].text;
        var optionValue = criteriaList.options[0].value;
        if (optionText.indexOf("AND ") == 0) {
            optionText = optionText.substring(4);
            optionValue = optionValue.substring(4);
        }
        else if (optionText.indexOf("OR ") == 0) {
            optionText = optionText.substring(3);
            optionValue = optionValue.substring(3);
        }
        criteriaList.options[0].text = optionText;
        criteriaList.options[0].value = optionValue;
    }
}

/**
* This Function is called when hit "Reset" button
*/
function resetSearchCriteria() {
    //searchCriteria is a list holding all search criteria
    var criteriaList = top.frames.rightframe.formName.searchCriteria;
    var thislength = criteriaList.options.length;
    if (thislength > 0) {
        if (confirm("Are you going to delete all the search conditions?") != true) {
            return;
        }
        criteriaList.options.length = 0;
    }
}

/**
*This Function is called when hit "Submit" button
*@param searchUsing name of "searchUsing" list
*@param operator name of "operator" list
*@param searchValue name of "searchValue" text field
*@param condition name of "condition" list
*@param formName name of the form to be submitted
*@param searchMode search mode "Advance"/"Basic"
*/
function conditionalSearch(searchUsing, operator, searchValue, condition, formName, searchMode) {
    //searchCriteria is a list holding all search criteria
    var criteriaList = top.frames.rightframe.formName.searchCriteria;
    var hiddenList = top.frames.rightframe.formName.hiddenFieldType;
    if (searchMode.toLowerCase() == "basic") {
        criteriaList.options.length = 0;
        addSearchCriteria(searchUsing, '', searchValue, '', formName);
    }
    var thislength = criteriaList.options.length;
    if ((parseInt(thislength) <= 0) && (confirm("Continue without search conditon?") != true)) {
        return;
    }

    for (var i = 0; i < thislength; i++) {
        criteriaList.options[i].selected = true;
    }
    lg_reset(); //Needed by CommonList Component
    top.frames.rightframe.formName.actionMode.value = "new";
    formName.submit();
}

/**
*This function is called when the main frame page is changed
*hiddenFieldList is a hidden data storage list in the right frame
*spanID_FieldList is a mark tag ID in the left frame where to insert some html content
*@param formName name of form in main frame
*/
function setFilterList(formName) {
    var strHtml = "";
    var hiddenOption = top.frames.rightframe.formName.hiddenFieldList;
    //html for create a group of field name checkboxes
    for (var i = 0; i < hiddenOption.options.length; i++) {
        strHtml += "<tr><td><input type=checkbox name='fieldsFilterList'" +
				" value=" + hiddenOption.options[i].value +
				((hiddenOption.options[i].selected) ? " checked" : "") + ">" +
				hiddenOption.options[i].text + "</td></tr><br>";
    }

    //html for create an "Update" button under the group of checkboxes
    strHtml += "<tr><td><input type='button' value='Update' onclick='showSelectedColumns(top.frames.rightframe.document." + formName.name + ")' target='rightframe'></td></tr>";

    //insert html into marked place
    top.frames.leftframe.document.getElementById("spanID_FieldList").innerHTML = strHtml;
}

/**
*This function is called when user selects some column and request to update table
*hiddenFieldList is a hidden data storage list in the right frame
*fieldsFilterList is a group of field name checkboxes in the left frame
*@param formName name of form in main frame
*/
function showSelectedColumns(formName) {
    var selectedNumber = 0;
    var hiddenFieldsList = formName.hiddenFieldList.options;
    var fieldsFilterList = document.getElementsByName("fieldsFilterList");
    for (var i = 0; i < fieldsFilterList.length; i++) {
        //make hiddenFieldList and fieldsFilterList have the same checked/slected status
        hiddenFieldsList[i].selected = fieldsFilterList[i].checked;
    }
    formName.submit();
}
