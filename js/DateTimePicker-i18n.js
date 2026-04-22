/* ----------------------------------------------------------------------------- 

  jQuery DateTimePicker - Responsive flat design jQuery DateTime Picker plugin for Web & Mobile
  Version 0.1.26
  Copyright (c)2016 Curious Solutions LLP and Neha Kadam
  http://curioussolutions.github.io/DateTimePicker
  https://github.com/CuriousSolutions/DateTimePicker

 ----------------------------------------------------------------------------- */

/*

	language: German
	file: DateTimePicker-i18n-de

*/

(function ($) {
    $.DateTimePicker.i18n["de"] = $.extend($.DateTimePicker.i18n["de"], {
        
    	language: "de",

    	dateTimeFormat: "dd-MMM-yyyy HH:mm:ss",
		dateFormat: "dd-MMM-yyyy",
		timeFormat: "HH:mm:ss",

		shortDayNames: ["So", "Mo", "Di", "Mi", "Do", "Fr", "Sa"],
		fullDayNames: ["Sonntag", "Montag", "Dienstag", "Mittwoch", "Donnerstag", "Freitag", "Samstag"],
		shortMonthNames: ["Jan", "Feb", "Mär", "Apr", "Mai", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dez"],
		fullMonthNames: ["Januar", "Februar", "März", "April", "Mai", "Juni", "Juli", "August", "September", "Oktober", "November", "Dezember"],

		titleContentDate: "Datum auswählen",
		titleContentTime: "Zeit auswählen",
		titleContentDateTime: "Datum & Zeit auswählen",
	
		setButtonContent: "Auswählen",
		clearButtonContent: "Zurücksetzen",

		formatHumanDate: function(oDate, sMode, sFormat)
		{
			if(sMode === "date")
				return oDate.dayShort + ", " + oDate.dd + " " + oDate.month+ ", " + oDate.yyyy;
			else if(sMode === "time")
				return oDate.HH + ":" + oDate.mm + ":" + oDate.ss;
			else if(sMode === "datetime")
				return oDate.dayShort + ", " + oDate.dd + " " + oDate.month+ ", " + oDate.yyyy + " " + oDate.HH + ":" + oDate.mm + ":" + oDate.ss;
		}
        
    });
})(jQuery);



/*

	language: English
	file: DateTimePicker-i18n-en

*/

(function ($) {
    $.DateTimePicker.i18n["en"] = $.extend($.DateTimePicker.i18n["en"], {
        
    	language: "en",

    	dateTimeFormat: "dd-MM-yyyy HH:mm",
		dateFormat: "dd-MM-yyyy",
		timeFormat: "HH:mm",

		shortDayNames: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
		fullDayNames: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"],
		shortMonthNames: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
		fullMonthNames: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],

		titleContentDate: "Set Date",
		titleContentTime: "Set Time",
		titleContentDateTime: "Set Date & Time",
	
		setButtonContent: "Set",
		clearButtonContent: "Clear"
        
    });
})(jQuery);



/*

	language: Spanish
	file: DateTimePicker-i18n-es
	author: kristophone(https://github.com/kristophone)

*/


(function ($) {
    $.DateTimePicker.i18n["es"] = $.extend($.DateTimePicker.i18n["es"], {
        
    	language: "es",

    	dateTimeFormat: "dd-MMM-yyyy HH:mm:ss",
		dateFormat: "dd-MMM-yyyy",
		timeFormat: "HH:mm:ss",

		shortDayNames: ["Dom", "Lun", "Mar", "Mié", "Jue", "Vie", "Sáb"],
		fullDayNames: ["Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"],
	    shortMonthNames: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
	    fullMonthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],

	    titleContentDate: "Ingresar fecha",
		titleContentTime: "Ingresar hora",
		titleContentDateTime: "Ingresar fecha y hora",
	
		setButtonContent: "Guardar",
	    clearButtonContent: "Cancelar",

		formatHumanDate: function(oDate, sMode, sFormat)
		{
			if(sMode === "date")
				return oDate.dayShort + ", " + oDate.dd + " " + oDate.month+ ", " + oDate.yyyy;
			else if(sMode === "time")
				return oDate.HH + ":" + oDate.mm + ":" + oDate.ss;
			else if(sMode === "datetime")
				return oDate.dayShort + ", " + oDate.dd + " " + oDate.month+ ", " + oDate.yyyy + " " + oDate.HH + ":" + oDate.mm + ":" + oDate.ss;
		}
        
    });
})(jQuery);




/*

	language: French
	file: DateTimePicker-i18n-fr
	author: LivioGama(https://github.com/LivioGama)

*/

(function ($) {
    $.DateTimePicker.i18n["fr"] = $.extend($.DateTimePicker.i18n["fr"], {
        
    	language: "fr",

    	dateTimeFormat: "dd-MM-yyyy HH:mm",
		dateFormat: "dd-MM-yyyy",
		timeFormat: "HH:mm",

		shortDayNames: ["Dim", "Lun", "Mar", "Mer", "Jeu", "Ven", "Sam"],
		fullDayNames: ["Dimanche", "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi"],
		shortMonthNames: ["Jan", "Fév", "Mar", "Avr", "Mai", "Jun", "Jul", "Aoû", "Sep", "Oct", "Nov", "Déc"],
		fullMonthNames: ["Janvier", "Février", "Mars", "Avril", "Mai", "Juin", "Juillet", "Août", "Septembre", "Octobre", "Novembre", "Décembre"],

		titleContentDate: "Choisir une date",
		titleContentTime: "Choisir un horaire",
		titleContentDateTime: "Choisir une date et un horaire",
	
		setButtonContent: "Choisir",
		clearButtonContent: "Effacer",
		formatHumanDate: function(oDate, sMode, sFormat)
		{
			if(sMode === "date")
				return oDate.dayShort + " " + oDate.dd + " " + oDate.month+ " " + oDate.yyyy;
			else if(sMode === "time")
				return oDate.HH + ":" + oDate.mm + ":" + oDate.ss;
			else if(sMode === "datetime")
				return oDate.dayShort + " " + oDate.dd + " " + oDate.month+ " " + oDate.yyyy + ", " + oDate.HH + ":" + oDate.mm + ":" + oDate.ss;
		}
        
    });
})(jQuery);



/*

  language: Japanese
  file: DateTimePicker-i18n-ja
  author: JasonYCHuang (https://github.com/JasonYCHuang)

*/

(function ($) {
   $.DateTimePicker.i18n["ja"] = $.extend($.DateTimePicker.i18n["ja"], {

        language: "ja",
        labels: {
            'year': '?',
            'month': '?',
            'day': '?',
            'hour': '?',
            'minutes': '?',
            'seconds': '?',
            'meridiem': '?'
        },
        dateTimeFormat: "yyyy-MM-dd HH:mm",
        dateFormat: "yyyy-MM-dd",
        timeFormat: "HH:mm",

        shortDayNames: ['???', '???', '???', '???', '???', '???', '???'],
        fullDayNames: ['???', '???', '???', '???', '???', '???', '???'],
        shortMonthNames: ['01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'],
        fullMonthNames: ['01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'],

        titleContentDate: "?????",
        titleContentTime: "?????",
        titleContentDateTime: "????????",

        setButtonContent: "??",
        clearButtonContent: "??",
        formatHumanDate: function (oDate, sMode, sFormat) {
            if (sMode === "date")
                return  oDate.dayShort + ", " + oDate.yyyy + "?" +  oDate.month +"?" + oDate.dd + "?";
            else if (sMode === "time")
                return oDate.HH + "?" + oDate.mm + "?" + oDate.ss + "?";
            else if (sMode === "datetime")
                return oDate.dayShort + ", " + oDate.yyyy + "?" +  oDate.month +"?" + oDate.dd + "? " + oDate.HH + "?" + oDate.mm + "?";
        }
    });
})(jQuery);




/*

	language: Dutch
	file: DateTimePicker-i18n-nl
	author: Bernardo(https://github.com/bhulsman)

*/

(function ($) {
    $.DateTimePicker.i18n["nl"] = $.extend($.DateTimePicker.i18n["nl"], {
        
    	language: "nl",

    	dateTimeFormat: "dd-MM-yyyy HH:mm",
		dateFormat: "dd-MM-yyyy",
		timeFormat: "HH:mm",

		shortDayNames: ["zo", "ma", "di", "wo", "do", "vr", "za"],
		fullDayNames: ["zondag", "maandag", "dinsdag", "woensdag", "donderdag", "vrijdag", "zaterdag"],
		shortMonthNames: ["jan", "feb", "mrt", "apr", "mei", "jun", "jul", "aug", "sep", "okt", "nov", "dec"],
		fullMonthNames: ["januari", "februari", "maart", "april", "mei", "juni", "juli", "augustus", "september", "oktober", "november", "december"],

		titleContentDate: "Kies datum",
		titleContentTime: "Kies tijd",
		titleContentDateTime: "Kies datum & tijd",
	
		setButtonContent: "Kiezen",
		clearButtonContent: "Leegmaken"
        
    });
})(jQuery);




/*

	language: Romanian
	file: DateTimePicker-i18n-nl
	author: Radu Mogo?(https://github.com/pixelplant)

 */

(function ($) {
	$.DateTimePicker.i18n["ro"] = $.extend($.DateTimePicker.i18n["ro"], {

		language: "ro",

		dateTimeFormat: "dd-MM-yyyy HH:mm",
		dateFormat: "dd-MM-yyyy",
		timeFormat: "HH:mm",

		shortDayNames: ["Dum", "Lun", "Mar", "Mie", "Joi", "Vim", "Sâm"],
		fullDayNames: ["Duminica", "Luni", "Mar?i", "Miercuri", "Joi", "Vineri", "Sâmbata"],
		shortMonthNames: ["Ian", "Feb", "Mar", "Apr", "Mai", "Iun", "Iul", "Aug", "Sep", "Oct", "Noi", "Dec"],
		fullMonthNames: ["Ianuarie", "Februarie", "Martie", "Aprilie", "Mai", "Iunie", "Iulie", "August", "Septembrie", "Octombrie", "Noiembrie", "Decembrie"],

		titleContentDate: "Setare Data",
		titleContentTime: "Setare Ora",
		titleContentDateTime: "Setare Data ?i Ora",
	
		setButtonContent: "Seteaza",
		clearButtonContent: "?terge"

	});
})(jQuery);



/*

  language: Russian
  file: DateTimePicker-i18n-ru
  author: Valery Bogdanov (https://github.com/radkill)

*/

(function ($) {
    $.DateTimePicker.i18n["ru"] = $.extend($.DateTimePicker.i18n["ru"], {

    language: "ru",

    dateTimeFormat: "dd-MM-yyyy HH:mm",
    dateFormat: "dd-MM-yyyy",
    timeFormat: "HH:mm",

    shortDayNames: ["??", "??", "??", "??", "??", "??", "??"],
    fullDayNames: ["???????????", "???????????", "???????", "?????", "???????", "???????", "???????"],
    shortMonthNames: ["???", "???", "???", "???", "???", "???", "???", "???", "???", "???", "???", "???"],
    fullMonthNames: ["??????", "???????", "?????", "??????", "???", "????", "????", "???????", "????????", "???????", "??????", "???????"],

    titleContentDate: "???????? ????",
    titleContentTime: "???????? ?????",
    titleContentDateTime: "???????? ???? ? ?????",

    setButtonContent: "???????",
    clearButtonContent: "????????",

    formatHumanDate: function(oDate, sMode, sFormat)
		{
			if(sMode === "date")
				return oDate.dayShort + ", " + oDate.dd + " " + oDate.month+ " " + oDate.yyyy;
			else if(sMode === "time")
				return oDate.HH + ":" + oDate.mm + ":" + oDate.ss;
			else if(sMode === "datetime")
				return oDate.dayShort + ", " + oDate.dd + " " + oDate.month+ " " + oDate.yyyy + ", " + oDate.HH + ":" + oDate.mm + ":" + oDate.ss;
		}

    });
})(jQuery);



/*

  language: Ukrainian
  file: DateTimePicker-i18n-uk
  author: Valery Bogdanov (https://github.com/radkill)

*/

(function ($) {
    $.DateTimePicker.i18n["uk"] = $.extend($.DateTimePicker.i18n["uk"], {

    language: "uk",

    dateTimeFormat: "dd-MM-yyyy HH:mm",
    dateFormat: "dd-MM-yyyy",
    timeFormat: "HH:mm",

    shortDayNames: ["??", "??", "??", "??", "??", "??", "??"],
    fullDayNames: ["???????????", "???????????", "???????", "?????", "???????", "???????", "???????"],
    shortMonthNames: ["???", "???", "???", "???", "???", "???", "???", "???", "???", "???", "???", "???"],
    fullMonthNames: ["??????", "???????", "?????", "??????", "???", "????", "????", "???????", "????????", "???????", "??????", "???????"],
    fullDayNames: ["??????", "?????????", "????????", "??????", "??????", "?'??????", "??????"],
    shortMonthNames: ["???", "???", "???", "???", "???", "???", "???", "???", "???", "???", "???", "???"],
    fullMonthNames: ["?????", "??????", "???????", "??????", "??????", "??????", "?????", "??????", "???????", "??????", "?????????", "??????"],

    titleContentDate: "???????? ????",
    titleContentTime: "???????? ???",
    titleContentDateTime: "???????? ???? ? ???",

    setButtonContent: "???????",
    clearButtonContent: "????????",

    formatHumanDate: function(oDate, sMode, sFormat)
		{
			if(sMode === "date")
				return oDate.dayShort + ", " + oDate.dd + " " + oDate.month+ " " + oDate.yyyy;
			else if(sMode === "time")
				return oDate.HH + ":" + oDate.mm + ":" + oDate.ss;
			else if(sMode === "datetime")
				return oDate.dayShort + ", " + oDate.dd + " " + oDate.month+ " " + oDate.yyyy + ", " + oDate.HH + ":" + oDate.mm + ":" + oDate.ss;
		}

    });
})(jQuery);



/*

  language: Traditional Chinese
  file: DateTimePicker-i18n-zh-TW
  author: JasonYCHuang (https://github.com/JasonYCHuang)

*/

(function ($) {
   $.DateTimePicker.i18n["zh-TW"] = $.extend($.DateTimePicker.i18n["zh-TW"], {

        language: "zh-TW",
        labels: {
            'year': '?',
            'month': '?',
            'day': '?',
            'hour': '?',
            'minutes': '?',
            'seconds': '?',
            'meridiem': '?'
        },
        dateTimeFormat: "yyyy-MM-dd HH:mm",
        dateFormat: "yyyy-MM-dd",
        timeFormat: "HH:mm",

        shortDayNames: ['???', '???', '???', '???', '???', '???', '???'],
        fullDayNames: ['???', '???', '???', '???', '???', '???', '???'],
        shortMonthNames: ['01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'],
        fullMonthNames: ['01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'],

        titleContentDate: "????",
        titleContentTime: "????",
        titleContentDateTime: "???????",

        setButtonContent: "??",
        clearButtonContent: "??",
        formatHumanDate: function (oDate, sMode, sFormat) {
            if (sMode === "date")
                return  oDate.dayShort + ", " + oDate.yyyy + "?" +  oDate.month +"?" + oDate.dd + "?";
            else if (sMode === "time")
                return oDate.HH + "?" + oDate.mm + "?" + oDate.ss + "?";
            else if (sMode === "datetime")
                return oDate.dayShort + ", " + oDate.yyyy + "?" +  oDate.month +"?" + oDate.dd + "? " + oDate.HH + "?" + oDate.mm + "?";
        }
    });
})(jQuery);




/*

	language: Simple Chinese
	file: DateTimePicker-i18n-zh-CN
	author: Calvin(https://github.com/Calvin-he)

*/

(function ($) {
   $.DateTimePicker.i18n["zh-CN"] = $.extend($.DateTimePicker.i18n["zh-CN"], {

        language: "zh-CN",
        labels: {
            'year': '?',
            'month': '?',
            'day': '?',
            'hour': '?',
            'minutes': '?',
            'seconds': '?',
            'meridiem': '?'
        },
        dateTimeFormat: "yyyy-MM-dd HH:mm",
        dateFormat: "yyyy-MM-dd",
        timeFormat: "HH:mm",

        shortDayNames: ['???', '???', '???', '???', '???', '???', '???'],
        fullDayNames: ['???', '???', '???', '???', '???', '???', '???'],
        shortMonthNames: ['01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'],
        fullMonthNames: ['01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'],

        titleContentDate: "????",
        titleContentTime: "????",
        titleContentDateTime: "???????",

        setButtonContent: "??",
        clearButtonContent: "??",
        formatHumanDate: function (oDate, sMode, sFormat) {
            if (sMode === "date")
                return  oDate.dayShort + ", " + oDate.yyyy + "?" +  oDate.month +"?" + oDate.dd + "?";
            else if (sMode === "time")
                return oDate.HH + "?" + oDate.mm + "?" + oDate.ss + "?";
            else if (sMode === "datetime")
                return oDate.dayShort + ", " + oDate.yyyy + "?" +  oDate.month +"?" + oDate.dd + "? " + oDate.HH + "?" + oDate.mm + "?";
        }
    });
})(jQuery);