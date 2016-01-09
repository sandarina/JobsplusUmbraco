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
    public class GrantsApiController : UmbracoAuthorizedJsonController
    {
        public IEnumerable<Grant> GetAll()
        {

            var query = new Sql().Select("*").From("JobsplusGrants");
            return DatabaseContext.Database.Fetch<Grant>(query);
        }

        public Grant GetById(int id)
        {

            var query = new Sql().Select("*").From("JobsplusGrants").Where<Grant>(x => x.Id == id);
            return DatabaseContext.Database.Fetch<Grant>(query).FirstOrDefault();
            
        }

        public Grant PostSave(Grant grant)
        {
            if (grant.Id > 0)
                DatabaseContext.Database.Update(grant);
            else
                DatabaseContext.Database.Save(grant);

            return grant;
        }

        public int DeleteById(int id)
        {
            return DatabaseContext.Database.Delete<Grant>(id);     
        }

    }
}
