angular.module("umbraco").controller("JobsplusGrants.GrantEditController",
	function ($scope, $routeParams, grantsResource, notificationsService, navigationService) {
	    var grantPrefix = 'grant-';
	    var grantDefinitionPrefix = 'grantdef-';

	    $scope.loaded = false;

        // vytvari se novy dotacni program
	    if ($routeParams.id == -1) {
	        $scope.grant = {};
	        $scope.loaded = true;
	    }
        // upravuji existujici dotacni program
	    else {
	        if ($routeParams.id.indexOf(grantPrefix) == 0) // jedna se o dotaci
	        {
	            // vytahnu si id dotace z URL -> service
	            grantsResource.getGrantById($routeParams.id).then(function (response) {
	                $scope.grant = response.data;

	                $scope.loaded = true;

	            });
	        }
	    }


	    $scope.saveGrant = function (grant) {
	        grantsResource.saveGrant(grant).then(function (response) {
	            $scope.grant = response.data;
	            $scope.grantForm.$dirty = false;
	            navigationService.syncTree({ tree: 'grantsTree', path: [-1, -1], forceReload: true });
	            notificationsService.success("Success", 'Dotační program "' + grant.Name + '" byl uložen.');
	        });
	    };


	});
