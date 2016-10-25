(function ($) {

    $('#advertisementReply').on('shown.bs.modal', function () {
        $('#EditCandidateFormSubmit').focus()
    })

    $('#aboutCompany').hide();
    $('#brigadesList').hide();
	$('#advertisementList').show();

	$('#TOPAdvertisementList').show();
	$('#TOPBrigadesList').hide();
	$('#TOPCompanyList').hide();
	$('#NewsList').hide();
	
	$('#ListAdvertisemet').hide();
	
	$('#detailAdvertisement').show();
	$('#similar').hide();

	$('#divTOPAdvertisementList').addClass('selected');
	//$('#divTOPBrigadesList').addClass('selected');
    $('#divAdvertisementList').addClass('selected');
	$('#divDetailAdvertisement').addClass('selected');
	
	$(document).ready(function () {
	    $(document).on('focus', ':input', function () {
	        $(this).attr('autocomplete', 'off');
	    });
	    $('#tBody .txtWorkingField input.searchbox').attr("placeholder", "Kde?");
	    $('#tBody .txtRegion input.searchbox').attr("placeholder", "Obor?");
	    $('#searchRegions input.searchbox').attr("placeholder", "Kde?");
	    $('#searchRegions input.searchbox').addClass("form-control");
	    $('#searchWorkingFields input.searchbox').attr("placeholder", "Obor?");
	    $('#searchWorkingFields input.searchbox').addClass("form-control");
	    $('#searchTypeOfWorks input.searchbox').attr("placeholder", "Typ (hlavní, brigáda, apod.)?");
	    $('#searchTypeOfWorks input.searchbox').addClass("form-control");
	});

	$('#tBody .txtWorkingField input.searchbox').prop("autocomplete", "off"); 
	$('#tBody .txtRegion input.searchbox').prop("autocomplete", "off");
	
	if($('#IsZTP').is(":checked")) {
    	$('#schbIsZTP').addClass('checked');
    }
	else {
		$('#schbIsZTP').removeClass('checked');
	};

	
	$('#schbIsZTP').click(function(){
		if($('#IsZTP').is(":checked")) {
			$('#IsZTP').prop("checked", false)
			$('#schbIsZTP').removeClass('checked');
		}
		else {
			$('#IsZTP').prop("checked", true)
			$('#schbIsZTP').addClass('checked');
		}
	});
	
	
		
	if($('#chbUP').is(":checked")) {
    	$('#schbUP').addClass('checked');
    }
	else {
		$('#schbUP').removeClass('checked');
	};

	
	$('#schbUP').click(function(){
		if($('#chbUP').is(":checked")) {
			$('#chbUP').prop("checked", false)
			$('#schbUP').removeClass('checked');
		}
		else {
			$('#chbUP').prop("checked", true)
			$('#schbUP').addClass('checked');
		}
	});
	
	$('#divDetailAdvertisement').click(function(){
	    $('#detailAdvertisement').show();
		$('#similar').hide();
		
		$('#divDetailAdvertisement').addClass('selected');
		$('#divSimilar').removeClass('selected');
	});
	
	$('#divSimilar').click(function(){
	    $('#detailAdvertisement').hide();
		$('#similar').show();
		
		$('#divDetailAdvertisement').removeClass('selected');
		$('#divSimilar').addClass('selected');
	});

	$('#divAdvertisementList').click(function(){
	    $('#aboutCompany').hide();
	    $('#brigadesList').hide();
		$('#advertisementList').show();
		
		$('#divAdvertisementList').addClass('selected');
		$('#divBrigadesList').removeClass('selected');
		$('#divAboutCompany').removeClass('selected');
	});

	$('#divBrigadesList').click(function () {
	    $('#aboutCompany').hide();
	    $('#brigadesList').show();
	    $('#advertisementList').hide();

	    $('#divAdvertisementList').removeClass('selected');
	    $('#divBrigadesList').addClass('selected');
	    $('#divAboutCompany').removeClass('selected');
	});
		
	$('#divAboutCompany').click(function(){
	    $('#aboutCompany').show();
	    $('#brigadesList').hide();
		$('#advertisementList').hide();	
		
		$('#divAdvertisementList').removeClass('selected');
		$('#divBrigadesList').removeClass('selected');
		$('#divAboutCompany').addClass('selected');
	});


	$('#divTOPBrigadesList').click(function () {
	    $('#TOPAdvertisementList').hide();
	    $('#TOPBrigadesList').show();
	    $('#TOPCompanyList').hide();
	    $('#NewsList').hide();

	    $('#divTOPAdvertisementList').removeClass('selected');
	    $('#divTOPBrigadesList').addClass('selected');
	    $('#divTOPCompanyList').removeClass('selected');
	    $('#divNewsList').removeClass('selected');
	});

	$('#divTOPAdvertisementList').click(function () {
	    $('#TOPAdvertisementList').show();
	    $('#TOPBrigadesList').hide();
	    $('#TOPCompanyList').hide();
	    $('#NewsList').hide();

	    $('#divTOPAdvertisementList').addClass('selected');
	    $('#divTOPBrigadesList').removeClass('selected');
	    $('#divTOPCompanyList').removeClass('selected');
	    $('#divNewsList').removeClass('selected');
	});
	
	$('#divTOPCompanyList').click(function(){
		$('#TOPAdvertisementList').hide();
		$('#TOPBrigadesList').hide();
		$('#TOPCompanyList').show();
		$('#NewsList').hide();
		
		$('#divTOPAdvertisementList').removeClass('selected');
		$('#divTOPBrigadesList').removeClass('selected');
		$('#divTOPCompanyList').addClass('selected');
		$('#divNewsList').removeClass('selected');		
	});
	
	$('#divNewsList').click(function(){
		$('#TOPAdvertisementList').hide();
		$('#TOPBrigadesList').hide();
		$('#TOPCompanyList').hide();
		$('#NewsList').show();

		$('#divTOPAdvertisementList').removeClass('selected');
		$('#divTOPBrigadesList').removeClass('selected');
		$('#divTOPCompanyList').removeClass('selected');
		$('#divNewsList').addClass('selected');
	});

})(jQuery);