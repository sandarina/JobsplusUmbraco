angular.module('umbraco').controller('JobsplusGrants.GrantDeleteController',
    function ($scope, $routeParams, grantsResource, notificationsService, navigationService, $location, treeService) {

        $scope.Delete = function (id) {
            var grantPrefix = 'grant-';
            var grantDefinitionPrefix = 'grantdef-';

            if (id.indexOf(grantPrefix) == 0) // jedna se o dotaci
            {
                var grantId = id.replace(grantPrefix, '');

                grantsResource.deleteGrantById(grantId).then(function () {
                    navigationService.hideNavigation();
                    treeService.removeNode($scope.currentNode);
                    //$location.path('JobsPlusGrants');
                    notificationsService.success("Úspěch", "Dotace byla smazána");
                });
            }

            else if (id.indexOf(grantDefinitionPrefix) == 0) // jedna se o definici dotace
            {

                var grantDefId = id.replace(grantDefinitionPrefix, '');

                grantsResource.deleteGrantDefinitionById(grantDefId).then(function () {
                    navigationService.hideNavigation();
                    treeService.removeNode($scope.currentNode);
                    //$location.path('JobsPlusGrants');
                    notificationsService.success("Úspěch", "Dotace byla smazána");
                });
            }
            else
            {
                notificationsService.error("Chyba", "Identifikátor '"+id+"' není povoleným typem pro odstranění!");
            }
        };

        $scope.Cancel = function () {
            navigationService.hideNavigation();
        };
    });