var oLastBtn=0;

function RaiseButton(){
	window.event.cancelBubble=true;
	oBtn = window.event.srcElement;
	var bChosen = false;
	if(oLastBtn && oLastBtn != oBtn){
 		HideButton();
	}
	if(oBtn.buttonType){
		oBtn.className = oBtn.buttonType + "Up";
		oLastBtn=oBtn;
		}
	else {
	oLastBtn = 0;
	}
}

function DepressButton(){
	window.event.cancelBubble=true;
	oBtn = window.event.srcElement;
	if(oBtn.buttonType){
		oBtn.className = oBtn.buttonType + "Down";
	}
}

function HideButton(){
	if ((oLastBtn.buttonType == "LeftNavChosen") || (oLastBtn.buttonType == "LeftNavSubChosen") || (oLastBtn.buttonType == "appNavChosen") || (oLastBtn.buttonType == "appNavSubChosen")) {
		oLastBtn.className = oLastBtn.buttonType;
	}
	else {
	oLastBtn.className = oLastBtn.buttonType + "Off";
	}
}