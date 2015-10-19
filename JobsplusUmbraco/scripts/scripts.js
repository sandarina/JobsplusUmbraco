(function($) {
	$('#aboutCompany').hide();
	$('#advertisementList').show();
	
	$('#TOPCompanyList').show();
	$('#TOPAdvertisementList').hide();
	$('#NewsList').hide();
	
	$('#ListAdvertisemet').hide();
	
	$('#detailAdvertisement').show();
	$('#similar').hide();
	
	$('#divTOPCompanyList').addClass('selected');
    $('#divAdvertisementList').addClass('selected');
	$('#divDetailAdvertisement').addClass('selected');
	
	$('#divDetailAdvertisement').click(function(){
		$('#detailAdvertisement').show();
		$('#similar').hide();
		
		$('#divAdvertisementList').addClass('selected');
		$('#divSimilar').removeClass('selected');
	});
	
	$('#divSimilar').click(function(){
		$('#detailAdvertisement').hide();
		$('#similar').show();
		
		$('#divAdvertisementList').removeClass('selected');
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
	
	
	/*$('#btnAdvertisement').click(function(){
    	$('#ListAdvertisemet').show();
		var url = '<umbraco:Macro runat="server" Alias="pvmGenerateAdvertisementList" language="cshtml" />';
		var url = '@Umbraco.RenderMacro("pvmGenerateAdvertisementList")';
		$('#ListAdvertisemet').append(url);
	});
	
	
	$('#btnAdvertisement').click(function() {
		var url = '@Url.Action("Results", "AdvertisementController")';
  		var wf = $('#ddlworkingField').val();
		var r = $('#ddlregion').val();
		$('#ListAdvertisemet').load(url, { workingField: wf, region: r });
	});*/
	
	
	
	/*object.onclick=function(){myScript};*/

})(jQuery);