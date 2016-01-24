angular.module("umbraco").controller("JobsplusGrants.GrantEditController",
	function ($scope, $routeParams, grantsResource, notificationsService, navigationService) {

	    $scope.loaded = false;
	    $scope.allRegions = function () {
	        grantsResource.getRegions().then(function (response) {
	            return response.data;
	        });
	    };

        // vytvari se novy dotacni program
	    if ($routeParams.id == -1) {
	        $scope.grant = {};
	        $scope.loaded = true;
	    }
        // upravuji existujici dotacni program
	    else {
	        // vytahnu si id dotace z URL -> service
	        grantsResource.getById($routeParams.id).then(function (response) {
	            $scope.grant = response.data;
	            $scope.loaded = true;
	        });
	    }


	    $scope.save = function (grant) {
	        grantsResource.saveGrant(grant).then(function (response) {
	            $scope.grant = response.data;
	            $scope.grantForm.$dirty = false;
	            navigationService.syncTree({ tree: 'grantsTree', path: [-1, -1], forceReload: true });
	            notificationsService.success("Success", 'Dotační program "' + grant.Name + '" byl uložen.');
	        });
	    };


	});
