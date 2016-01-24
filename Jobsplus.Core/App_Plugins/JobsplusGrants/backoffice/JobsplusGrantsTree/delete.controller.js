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
                    $location.path('grants');
                    notificationsService.success("Success", "Dotace byla úspěšně smazána");
                });
            }
                // TODO : smazani definice dotace
            else
            {
                notificationsService.error("Delete Error", "Identifikátor '"+id+"' není povoleným typem pro odtranění!");
            }
        };

        $scope.Cancel = function () {
            navigationService.hideNavigation();
        };
    });