angular.module("umbraco.resources")
	.factory("grantsResource", function ($http) {
	    return {
            // 1: DOTACE
	        getGrantById: function (id) {
	            return $http.get("backoffice/JobsplusGrants/GrantsApi/GetById?id=" + id);
	        },
	        saveGrant: function (grant) {
	            return $http.post("backoffice/JobsplusGrants/GrantsApi/PostSave", angular.toJson(grant));
	        },
	        deleteGrantById: function (id) {
	            return $http.delete("backoffice/JobsplusGrants/GrantsApi/DeleteById?id=" + id);
	        },
            // 2: DEFINICE DOTACÍ
	        getGrantDefinitionById: function (id) {
	            return $http.get("backoffice/JobsplusGrants/GrantDefinitionsApi/GetById?id=" + id);
	        },
	        saveGrantDefinition: function (grantDef) {
	            return $http.post("backoffice/JobsplusGrants/GrantDefinitionsApi/PostSave", angular.toJson(grantDef));
	        },
	        deleteGrantDefinitionById: function (id) {
	            return $http.delete("backoffice/JobsplusGrants/GrantDefinitionsApi/DeleteById?id=" + id);
	        },
	        // 3: OBECNÉ ČÍSELNÍKY:
            // kraj
	        getRegions: function () {
	            return $http.get("backoffice/JobsplusGrants/RegionsApi/GetAll");
	        },
	        getRegionById: function (id) {
	            return $http.get("backoffice/JobsplusGrants/RegionsApi/GetById?id=" + id);
	        },
	        // úřady práce
	        getEmployDepartments: function () {
	            return $http.get("backoffice/JobsplusGrants/EmployDepartmentsApi/GetAll");
	        },
	        getEmployDepartmentsByRegion: function (regionId) {
	            return $http.get("backoffice/JobsplusGrants/EmployDepartmentsApi/GetAll?regionId=" + regionId);
	        },
	        getEmployDepartmentById: function (id) {
	            return $http.get("backoffice/JobsplusGrants/EmployDepartmentsApi/GetById?id=" + id);
	        },
	        getEmployDepartsByGrantDef: function (grantDefId) {
	            return $http.get("backoffice/JobsplusGrants/EmployDepartmentsApi/GetEmployDepartsByGrantDef?grantDefId=" + grantDefId);
	        },
	        saveEmployDepartToGrantDef: function (grantDefId, employDepartmentId) {
	            return $http.post("backoffice/JobsplusGrants/EmployDepartmentsApi/SaveEmployDepartToGrantDef?grantDefId=" + grantDefId + "&employDepartmentId=" + employDepartmentId);
	        },
	        deleteEmployDepartsByGrantDef: function (grantDefId) {
	            return $http.delete("backoffice/JobsplusGrants/EmployDepartmentsApi/DeleteEmployDepartsByGrantDef?grantDefId=" + grantDefId);
	        }
	    };
	});
