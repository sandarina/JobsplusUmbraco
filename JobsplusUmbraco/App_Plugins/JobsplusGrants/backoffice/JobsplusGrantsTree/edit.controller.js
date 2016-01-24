angular.module("umbraco").controller("JobsplusGrants.GrantEditController",
	function ($scope, $routeParams, grantsResource, notificationsService, navigationService) {
	    var grantPrefix = "grant-";
	    var grantDefinitionPrefix = "grantdef-";

	    $scope.loaded = false;
	    $scope.allRegions = null;

	    grantsResource.getRegions().then(function (response) {
	         $scope.allRegions = response.data;
	    });

        // vytvari se novy dotacni program
	    if ($routeParams.id == -1) {
	        $scope.grant = {};
	        $scope.loaded = true;
	    }
        // upravuji existujici dotacni program
	    else {
	        if ($routeParams.id.indexOf(grantPrefix) == 0)
	        {
	            // vytahnu si id dotace z URL -> service	            
	            grantsResource.getGrantById($routeParams.id.replace(grantPrefix, "")).then(function (response) {
	                $scope.grant = response.data;
	                $scope.loaded = true;
	            });
	        }
	        else if ($routeParams.id.indexOf(grantDefinitionPrefix) == 0) {
	            // vytahnu si id definici dotace z URL -> service
	            grantsResource.getGrantDefinitionById($routeParams.id.replace(grantDefinitionPrefix, "")).then(function (response) {
	                $scope.grant = response.data;
	                $scope.loaded = true;
	            });
	        }
	    }


	    $scope.save = function (grant) {
	        grantsResource.saveGrant(grant).then(function (response) {
	            $scope.grant = response.data;
	            $scope.grantForm.$dirty = false;
	            navigationService.syncTree({ tree: 'JobsplusGrantsTree', path: [-1, grantPrefix + response.data.Id], forceReload: true });
	            notificationsService.success("Úspěch", 'Dotační program "' + grant.Name + '" byl uložen.');
	        });
	    };

        

	});
