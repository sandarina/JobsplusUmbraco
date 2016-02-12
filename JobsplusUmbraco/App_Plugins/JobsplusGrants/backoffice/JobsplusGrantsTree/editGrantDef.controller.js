angular.module("umbraco").controller("JobsplusGrants.GrantDefinitionEditController",
	function ($scope, $routeParams, grantsResource, notificationsService, navigationService) {
	    var grantPrefix = "grant-";
	    var grantDefinitionPrefix = "grantdef-";
	    var grantId = 0;

	    $scope.loaded = false;
	    $scope.grantDef = null;
	    $scope.grant = null;
	    $scope.region = null;
	    $scope.employDeparts = null;
	    $scope.employDepartsSelection = [];

        // vytvari se novy dotacni program
	    if ($routeParams.create) {
	        grantId = $routeParams.id.replace(grantPrefix, "");
	        $scope.grantDef = { GrantId: grantId };
	        getGrantById(grantId);
	        //$scope.grantId = grantId;
	        $scope.loaded = true;
	    }
        // upravuji existujici dotacni program
	    else {
	        if ($routeParams.id.indexOf(grantDefinitionPrefix) == 0) {
	            // vytahnu si id definici dotace z URL -> service
	            var grantDefId = $routeParams.id.replace(grantDefinitionPrefix, "");
	            grantsResource.getGrantDefinitionById(grantDefId).then(function (response) {
	                $scope.grantDef = response.data;
	                grantId = response.data.GrantId;
	                getGrantById(grantId);
	                $scope.loaded = true;
	            });
	        }
	        else {
	            notificationsService.error("GetDefinitionError", "Id '" + $routeParams.id + "' neodpovídá typu definice dotačního programu.");
	        }
	    }

	    function getGrantById(grantId) {
	        grantsResource.getGrantById(grantId).then(function (response) {
	            $scope.grant = response.data;
	            var regionId = response.data.RegionId;
	            getEmployDeparts(regionId);
	            getEmployDepartsSelection($scope.grantDef.Id, regionId);
	            getRegion(regionId);
	        });
	    }
	    
	    $scope.isSelected = function isSelected(employDepartmentId) {
	        /*var idx = -1;
	        angular.forEach($scope.employDepartsSelection, function (value, key) {
	            if (value.Id == ed.Id) {
	                idx = key;
	            }
	        });*/
	        var idx = $scope.employDepartsSelection.indexOf(employDepartmentId);
	        return idx > -1;
	    };

	    $scope.toggleSelection = function toggleSelection(employDepartmentId) {
	        /*var idx = -1;
	        angular.forEach($scope.employDepartsSelection, function (value, key) {
	            if (value.Id == ed.Id) {
	                idx = key;
	            }
	        });*/

	        var idx = $scope.employDepartsSelection.indexOf(employDepartmentId);

	        // is currently selected
	        if (idx > -1) {
	            $scope.employDepartsSelection.splice(idx, 1);
	        }

	            // is newly selected
	        else {
	            $scope.employDepartsSelection.push(employDepartmentId);
	        }
	    };

	    $scope.save = function (grantDef) {
	        grantsResource.saveGrantDefinition(grantDef).then(function (response) {
	            $scope.grantDef = response.data;
	            $scope.grantId = response.data.GrantId;

	            // smazat všechna napojeni na ÚP
	            var grandDefId = response.data.Id;
	            grantsResource.deleteEmployDepartsByGrantDef(grandDefId).then(function (response) { 
	                angular.forEach($scope.employDepartsSelection, function (employDepartmentId, key) {
	                    grantsResource.saveEmployDepartToGrantDef(grandDefId, employDepartmentId);
	                });
	            });

	            $scope.grantDefForm.$dirty = false;

	            navigationService.syncTree({ tree: 'JobsplusGrantsTree', path: [-1, grantDefinitionPrefix + response.data.Id], forceReload: true });
	            notificationsService.success("Úspěch", 'Definice dotačního programu "' + grantDef.Name + '" byla uložena.');
	        });
	    };



	    function getEmployDeparts(regionId) {
	        if (regionId != null) {
	            grantsResource.getEmployDepartmentsByRegion(regionId).then(function (response) {
	                $scope.employDeparts = response.data;
	                if ($routeParams.create) {
	                    $scope.employDepartsSelection.length = 0;
	                    angular.forEach($scope.employDeparts, function (ed, key) {
	                        $scope.employDepartsSelection.push(ed.Id);
	                    });
	                }
	            });
	        }
	    };

	    function getEmployDepartsSelection(grantDefId, regionId) {
	        if (grantDefId != null) {
	            grantsResource.getEmployDepartsByGrantDef(grantDefId).then(function (response) {
	                $scope.employDepartsSelection.length = 0;
	                angular.forEach(response.data, function (ed, key) {
	                    $scope.employDepartsSelection.push(ed.Id);
	                });
	            });
	        }
	    };

	    function getRegion(regionId) {
	        if (regionId != null) {
	            grantsResource.getRegionById(regionId).then(function (response) {
	                $scope.region = response.data;
	            });
	        }
	    };

	});
