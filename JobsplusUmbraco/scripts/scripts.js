(function($) {
	$('#aboutCompany').hide();
	$('#advertisementList').show();
	
	$('#TOPCompanyList').hide();
	$('#TOPAdvertisementList').show();
	$('#NewsList').hide();
	
	$('#ListAdvertisemet').hide();
	
	$('#detailAdvertisement').show();
	$('#similar').hide();
	
	$('#divTOPAdvertisementList').addClass('selected');
    $('#divAdvertisementList').addClass('selected');
	$('#divDetailAdvertisement').addClass('selected');
	
	$(document).ready(function () {
	    $(document).on('focus', ':input', function () {
	        $(this).attr('autocomplete', 'off');
	    });
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
		$('#advertisementList').show();
		
		$('#divAdvertisementList').addClass('selected');
		$('#divAboutCompany').removeClass('selected');
	});
		
	$('#divAboutCompany').click(function(){
		$('#aboutCompany').show();
		$('#advertisementList').hide();	
		
		$('#divAdvertisementList').removeClass('selected');
		$('#divAboutCompany').addClass('selected');
	});
	
	
	$('#divTOPCompanyList').click(function(){
		$('#TOPCompanyList').show();
		$('#TOPAdvertisementList').hide();
		$('#NewsList').hide();
		
		$('#divTOPCompanyList').addClass('selected');
		$('#divTOPAdvertisementList').removeClass('selected');
		$('#divNewsList').removeClass('selected');		
	});
	
	$('#divTOPAdvertisementList').click(function(){
		$('#TOPCompanyList').hide();
		$('#TOPAdvertisementList').show();
		$('#NewsList').hide();
		
		$('#divTOPAdvertisementList').addClass('selected');
		$('#divTOPCompanyList').removeClass('selected');
		$('#divNewsList').removeClass('selected');	
	});
	
	$('#divNewsList').click(function(){
		$('#TOPCompanyList').hide();
		$('#TOPAdvertisementList').hide();
		$('#NewsList').show();
		
		$('#divNewsList').addClass('selected');
		$('#divTOPAdvertisementList').removeClass('selected');
		$('#divTOPCompanyList').removeClass('selected');	
	});

})(jQuery);