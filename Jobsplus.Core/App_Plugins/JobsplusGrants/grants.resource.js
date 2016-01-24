angular.module("umbraco.resources")
	.factory("grantsResource", function ($http) {
	    return {
	        getGrantById: function (id) {
	            return $http.get("backoffice/JobsplusGrants/GrantsApi/GetById?id=" + id);
	        },
	        saveGrant: function (grant) {
	            return $http.post("backoffice/JobsplusGrants/GrantsApi/PostSave", angular.toJson(grant));
	        },
	        deleteGrantById: function (id) {
	            return $http.delete("backoffice/JobsplusGrants/GrantsApi/DeleteById?id=" + id);
	        },
	        getRegions: function () {
	            return $http.get("backoffice/JobsplusGrants/GrantsApi/GetAllRegions");
	        }
	    };
	});
