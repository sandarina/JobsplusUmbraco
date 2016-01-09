angular.module("umbraco.resources")
	.factory("grantsResource", function ($http) {
	    return {
	        getById: function (id) {
	            return $http.get("backoffice/JobsplusGrants/GrantsApi/GetById?id=" + id);
	        },
	        save: function (grant) {
	            return $http.post("backoffice/JobsplusGrants/GrantsApi/PostSave", angular.toJson(grant));
	        },
	        deleteById: function (id) {
	            return $http.delete("backoffice/JobsplusGrants/GrantsApi/DeleteById?id=" + id);
	        }
	    };
	});
