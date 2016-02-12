using Jobsplus.Backoffice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Persistence;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace Jobsplus.Backoffice.Controllers
{
    [PluginController("JobsplusGrants")]
    public class EmployDepartmentsApiController : UmbracoAuthorizedJsonController
    {
        #region Employ Departments - Úřady práce
        /// <summary>
        /// Získá všechny ÚP
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EmployDepartment> GetAll(int? regionId = null)
        {
            var query = new Sql().Select("*").From("JobsplusEmployDepartments");
            if (regionId.HasValue) query.Where<EmployDepartment>(i => i.RegionId == regionId.Value);
            return DatabaseContext.Database.Fetch<EmployDepartment>(query);
        }

        /// <summary>
        /// Získá ÚP na základě ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public EmployDepartment GetById(int id)
        {

            var query = new Sql().Select("*").From("JobsplusEmployDepartments").Where<EmployDepartment>(item => item.Id == id);
            return DatabaseContext.Database.Fetch<EmployDepartment>(query).FirstOrDefault();

        }

        public EmployDepartment PostSave(EmployDepartment employDepartment)
        {
            if (employDepartment.Id > 0)
                DatabaseContext.Database.Update(employDepartment);
            else
                DatabaseContext.Database.Save(employDepartment);

            return employDepartment;
        }

        public int DeleteById(int id)
        {
            return DatabaseContext.Database.Delete<EmployDepartment>(id);
        }

        
        public IEnumerable<EmployDepartment> GetEmployDepartsByGrantDef(int grantDefId)
        {
            return DatabaseContext.Database.Fetch<EmployDepartment>(@"Select ed.* from JobsplusEmployDepartments ed 
                LEFT OUTER JOIN JobsplusGrantDefEmployDeparts gded ON ed.Id = gded.EmployDepartmentId
                WHERE gded.GrantDefinitionId = @0", grantDefId);
        }


        public int DeleteEmployDepartsByGrantDef(int grantDefId)
        {
            return DatabaseContext.Database.Execute(@"DELETE FROM JobsplusGrantDefEmployDeparts 
                WHERE GrantDefinitionId = @0", grantDefId);
        }

        public GrantDefEmployDeparts SaveEmployDepartToGrantDef(int grantDefId, int employDepartmentId)
        {
            var gded = new GrantDefEmployDeparts()
            {
                EmployDepartmentId = employDepartmentId,
                GrantDefinitionId = grantDefId
            };
            
            DatabaseContext.Database.Insert(gded);

            return gded;
        }
        #endregion
    }
}
