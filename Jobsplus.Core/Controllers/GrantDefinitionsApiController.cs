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
    public class GrantDefinitionsApiController : UmbracoAuthorizedJsonController
    {
        #region Grant Definitions
        /// <summary>
        /// Získá všechny definice dotace dle filtru.
        /// </summary>
        /// <param name="grantId">Id dotace</param>
        /// <returns></returns>
        public IEnumerable<GrantDefinition> GetAll(int? grantId = null)
        {

            var query = new Sql().Select("*").From("JobsplusGrantDefinitions");
            if (grantId.HasValue) query.Where<GrantDefinition>(item => item.GrantId == grantId.Value);
            return DatabaseContext.Database.Fetch<GrantDefinition>(query);
        }


        public GrantDefinition GetById(int id)
        {

            var query = new Sql().Select("*").From("JobsplusGrantDefinitions").Where<GrantDefinition>(item => item.Id == id);
            return DatabaseContext.Database.Fetch<GrantDefinition>(query).FirstOrDefault();

        }

        public GrantDefinition PostSave(GrantDefinition grantDefinition)
        {
            if (grantDefinition.ContractType == null) grantDefinition.ContractType = "";
            if (grantDefinition.Id > 0)
                DatabaseContext.Database.Update(grantDefinition);
            else
                DatabaseContext.Database.Save(grantDefinition);

            return grantDefinition;
        }

        public int DeleteById(int id)
        {
            return DatabaseContext.Database.Delete<GrantDefinition>(id);
        }
        #endregion

    }
}
