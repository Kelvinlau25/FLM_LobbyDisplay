$(document).ready(function($) {
	
	$('.weather1').weather({
		city: null,
		autocompleteMinLength: 3
	});

	$('.weather2').weather({
		city: null,
		tempUnit: 'C',
		displayDescription: true,
		// displayMinMaxTemp: true,
		// displayWind: true,
		// displayHumidity: true
	});
	$('.weather3').weather({
		city: null,
		tempUnit: 'C',
		displayDescription: true,
		// displayMinMaxTemp: true,
		// displayWind: true,
		// displayHumidity: true
	});
	$('.weather4').weather({
		city: null,
		tempUnit: 'C',
		displayDescription: true,
		// displayMinMaxTemp: true,
		// displayWind: true,
		// displayHumidity: true
	});

});