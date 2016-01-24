angular.module("umbraco").controller("JobsplusGrants.GrantDefinitionEditController",
	function ($scope, $routeParams, grantsResource, notificationsService, navigationService) {
	    var grantPrefix = "grant-";
	    var grantDefinitionPrefix = "grantdef-";

	    $scope.loaded = false;
	    $scope.grant = null;

        // vytvari se novy dotacni program
	    if ($routeParams.create) {
	        var grantId = $routeParams.id.replace(grantPrefix, "");
	        $scope.grantDef = { GrantId: grantId };
	        $scope.grantId = grantId;
	        $scope.loaded = true;
	    }
        // upravuji existujici dotacni program
	    else {
	        if ($routeParams.id.indexOf(grantDefinitionPrefix) == 0) {
	            // vytahnu si id definici dotace z URL -> service
	            grantsResource.getGrantDefinitionById($routeParams.id.replace(grantDefinitionPrefix, "")).then(function (response) {
	                $scope.grantDef = response.data;
	                $scope.grantId = response.data.GrantId;
	                $scope.loaded = true;
	            });
	        }
	        else {
	            notificationsService.error("GetDefinitionError", "Id '" + $routeParams.id + "' neodpovídá typu definice dotačního programu.");
	        }
	    }


	    grantsResource.getGrantById($scope.grantId).then(function (response) {
	        $scope.grant = response.data;
	    });


	    $scope.save = function (grantDef) {
	        grantsResource.saveGrantDefinition(grantDef).then(function (response) {
	            $scope.grantDef = response.data;
	            $scope.grantId = response.data.GrantId;
	            $scope.grantDefForm.$dirty = false;
	            navigationService.syncTree({ tree: 'JobsplusGrantsTree', path: [-1, grantDefinitionPrefix + response.data.Id], forceReload: true });
	            notificationsService.success("Úspěch", 'Definice dotačního programu "' + grantDef.Name + '" byla uložena.');
	        });
	    };

        

	});
