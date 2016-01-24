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
    [PluginController("JobsplusJobTemplate")]
    public class JobTemplateApiController : UmbracoAuthorizedJsonController
    {
        DBContextController DBContext = new DBContextController();

        public IEnumerable<JobTemplate> GetAll()
        {
            return DBContext.GetAllJobTemplate();
        }

        public JobTemplate GetById(int id)
        {
            return DBContext.GetJobTemplateById(id);
        }

        public JobTemplate PostSave(JobTemplate jobTemplate)
        {
            return DBContext.PostSaveJobTemplate(jobTemplate);
        }

        public int DeleteById(int id)
        {
            return DBContext.DeleteJobTemplateById(id);
        }
    }
}
