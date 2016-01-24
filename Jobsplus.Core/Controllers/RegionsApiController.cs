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
    public class RegionsApiController : UmbracoAuthorizedJsonController
    {
        #region Regions
        /// <summary>
        /// Získá všechny kraje
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Region> GetAll()
        {

            var query = new Sql().Select("*").From("JobsplusRegions");
            //query.OrderBy<Region>(item => item.Name);
            return DatabaseContext.Database.Fetch<Region>(query);
        }


        public Region GetById(int id)
        {

            var query = new Sql().Select("*").From("JobsplusRegions").Where<Region>(item => item.Id == id);
            return DatabaseContext.Database.Fetch<Region>(query).FirstOrDefault();

        }

        public Region PostSave(Region region)
        {
            if (region.Id > 0)
                DatabaseContext.Database.Update(region);
            else
                DatabaseContext.Database.Save(region);

            return region;
        }

        public int DeleteById(int id)
        {
            return DatabaseContext.Database.Delete<Region>(id);
        }
        #endregion

    }
}
