using Jobsplus.Backoffice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace Jobsplus.Backoffice.Models
{
    public class RegisterEvents : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            //Get the Umbraco Database context
            var ctx = applicationContext.DatabaseContext;
            var db = new DatabaseSchemaHelper(ctx.Database, applicationContext.ProfilingLogger.Logger, ctx.SqlSyntax);

            //Check if the DB table does NOT exist
            if (!db.TableExist("JobsplusGrants"))
            {
                //Create DB table - and set overwrite to false
                db.CreateTable<Grant>(false);
            }

        }
    }
}
