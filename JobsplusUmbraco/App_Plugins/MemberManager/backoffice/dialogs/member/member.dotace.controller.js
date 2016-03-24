angular.module("umbraco")
    .controller("MemberManager.Dialogs.Member.DotaceController",
    function ($scope, $routeParams, $q, $timeout, $window, appState, memberResource, memberExtResource, entityResource, navigationService, notificationsService, angularHelper, serverValidationManager, contentEditingHelper, fileManager, formHelper, umbRequestHelper, umbModelMapper, $http) {
        //setup scope vars
        $scope.model = {};
        $scope.loaded = false;
        $scope.checkLoaded = false;

        var dialogOptions = $scope.$parent.dialogOptions;

        function performGet() {
            var deferred = $q.defer();
            if (angular.isObject(dialogOptions.entity)) {
                $scope.loaded = true;
                deferred.resolve(dialogOptions.entity);
            } else {
                if (dialogOptions.id && dialogOptions.id.toString().length < 9) {
                    entityResource.getById(dialogOptions.id, "Member").then(function (entity) {
                        memberResource.getByKey(entity.key).then(function (data) {

                            //in one particular special case, after we've created a new item we redirect back to the edit
                            // route but there might be server validation errors in the collection which we need to display
                            // after the redirect, so we will bind all subscriptions which will show the server validation errors
                            // if there are any and then clear them so the collection no longer persists them.
                            serverValidationManager.executeAndClearAllSubscriptions();
                            $scope.loaded = true;

                            deferred.resolve(data);
                        });
                    });
                }
                else {
                    //we are editing so get the content item from the server
                    memberResource.getByKey(dialogOptions.id)
                        .then(function (data) {

                            $scope.loaded = true;

                            //in one particular special case, after we've created a new item we redirect back to the edit
                            // route but there might be server validation errors in the collection which we need to display
                            // after the redirect, so we will bind all subscriptions which will show the server validation errors
                            // if there are any and then clear them so the collection no longer persists them.
                            serverValidationManager.executeAndClearAllSubscriptions();

                            deferred.resolve(data);
                        });
                }
                
            }

            return deferred.promise;
        };

        
        function performGetGrantDefinitions() {
            memberExtResource.getGrantDefinitionByMember($scope.model.entity.id)
                    .then(function (result) {
                        $scope.checkLoaded = true;
                        return result.data;
                    });
        };


        performGet().then(function (content) {
            $scope.model.entity = $scope.filterTabs(content, dialogOptions.tabFilter);


            $scope.model.checkResult = memberExtResource.getGrantDefinitionByMember($scope.model.entity.id)
                    .then(function (result) {
                        $scope.checkLoaded = true;
                        return result.data;
                    });
        });

        $scope.filterTabs = function (entity, blackList) {
            if (blackList) {
                _.each(entity.tabs, function (tab) {
                    tab.hide = _.contains(blackList, tab.alias);
                });
            }

            return entity;
        };

    });