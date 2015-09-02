(function($) {
	$('#aboutCompany').hide();
	$('#advertisementList').show();
	
	$('#TOPCompanyList').show();
	$('#TOPAdvertisementList').hide();
	$('#NewsList').hide();
	
	$('#ListAdvertisemet').hide();
	
	

	$('#divAdvertisementList').click(function(){
		$('#aboutCompany').hide();
		$('#advertisementList').show();
		
	});
		
	$('#divAboutCompany').click(function(){
		$('#aboutCompany').show();
		$('#advertisementList').hide();	
	});
	
	
	$('#divTOPCompanyList').click(function(){
		$('#TOPCompanyList').show();
		$('#TOPAdvertisementList').hide();
		$('#NewsList').hide();
		
	});
	
	$('#divTOPAdvertisementList').click(function(){
		$('#TOPCompanyList').hide();
		$('#TOPAdvertisementList').show();
		$('#NewsList').hide();
		
	});
	
	$('#divNewsList').click(function(){
		$('#TOPCompanyList').hide();
		$('#TOPAdvertisementList').hide();
		$('#NewsList').show();
		
	});
	
	
	/*$('#btnAdvertisement').click(function(){
    	$('#ListAdvertisemet').show();
		var url = '<umbraco:Macro runat="server" Alias="pvmGenerateAdvertisementList" language="cshtml" />';
		var url = '@Umbraco.RenderMacro("pvmGenerateAdvertisementList")';
		$('#ListAdvertisemet').append(url);
	});*/
	
	
	$('#btnAdvertisement').click(function() {
		var url = '@Url.Action("Results", "AdvertisementController")';
  		var wf = $('#ddlworkingField').val();
		var r = $('#ddlregion').val();
		$('#ListAdvertisemet').load(url, { workingField: wf, region: r });
	});
	
	
	
	/*object.onclick=function(){myScript};*/

})(jQuery);