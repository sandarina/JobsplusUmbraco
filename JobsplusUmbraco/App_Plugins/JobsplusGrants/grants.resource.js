angular.module("umbraco.resources")
	.factory("grantsResource", function ($http) {
	    return {
            // 1: dotace
	        getGrantById: function (id) {
	            return $http.get("backoffice/JobsplusGrants/GrantsApi/GetById?id=" + id);
	        },
	        saveGrant: function (grant) {
	            return $http.post("backoffice/JobsplusGrants/GrantsApi/PostSave", angular.toJson(grant));
	        },
	        deleteGrantById: function (id) {
	            return $http.delete("backoffice/JobsplusGrants/GrantsApi/DeleteById?id=" + id);
	        },
            // 2: definice dotací
	        getGrantDefinitionById: function (id) {
	            return $http.get("backoffice/JobsplusGrants/GrantDefinitionsApi/GetById?id=" + id);
	        },
	        saveGrantDefinition: function (grantDef) {
	            return $http.post("backoffice/JobsplusGrants/GrantDefinitionsApi/PostSave", angular.toJson(grantDef));
	        },
	        deleteGrantDefinitionById: function (id) {
	            return $http.delete("backoffice/JobsplusGrants/GrantDefinitionsApi/DeleteById?id=" + id);
	        },
	        // 3: obecné čísleníky:
            // kraj
	        getRegions: function () {
	            return $http.get("backoffice/JobsplusGrants/RegionsApi/GetAll");
	        }
	    };
	});
