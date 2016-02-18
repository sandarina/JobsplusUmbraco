angular.module("umbraco")
    .controller("JobsplusGrants.EmployDepartmentController",
    function ($scope, grantsResource) {

        $scope.loaded = false;
        $scope.list = [];

        grantsResource.getEmployDepartments().then(function (response) {
            //$scope.list = response.data;
            angular.forEach(response.data, function (value, key) {
                var regionName = "";
                grantsResource.getRegionById(value.RegionId).then(function (response_region) {
                    regionName = response_region.data.Name;
                    $scope.list.push({
                        Id: value.Id.toString(),
                        Name: value.Name,
                        RegionName: regionName
                    });
                });
            });
            $scope.loaded = true;
        });
            
    });