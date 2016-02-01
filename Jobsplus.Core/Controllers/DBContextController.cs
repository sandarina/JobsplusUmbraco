using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Persistence;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using Jobsplus.Backoffice.Models;

namespace Jobsplus.Backoffice.Controllers
{
    public class DBContextController : UmbracoAuthorizedJsonController
    {
        #region Specialization
        public IEnumerable<Specialization> GetAllSpecialization()
        {
            var query = new Sql().Select("*").From("JobsplusSpecializations").OrderBy("Order");
            return DatabaseContext.Database.Fetch<Specialization>(query);
        }

        public Specialization GetSpecializationById(int id)
        {
            var query = new Sql().Select("*").From("JobsplusSpecializations").Where<Specialization>(x => x.Id == id);
            return DatabaseContext.Database.Fetch<Specialization>(query).FirstOrDefault();
        }

        public Specialization PostSaveSpecialization(Specialization specialization)
        {
            if (specialization.Id > 0)
                DatabaseContext.Database.Update(specialization);
            else
                DatabaseContext.Database.Save(specialization);

            return specialization;
        }

        public int DeleteSpecializationById(int id)
        {
            return DatabaseContext.Database.Delete<Specialization>(id);
        }
        #endregion

        #region Job
        public IEnumerable<Job> GetAllJob()
        {
            var query = new Sql().Select("*").From("JobsplusJobs").OrderBy("Name");
            return DatabaseContext.Database.Fetch<Job>(query);
        }

        public Job GetJobById(int id)
        {
            var query = new Sql().Select("*").From("JobsplusJobs").Where<Job>(x => x.Id == id);
            return DatabaseContext.Database.Fetch<Job>(query).FirstOrDefault();
        }

        public Job GetJobByName(string name)
        {
            var query = new Sql().Select("*").From("JobsplusJobs").Where<Job>(x => x.Name.Contains(name));
            return DatabaseContext.Database.Fetch<Job>(query).FirstOrDefault();
        }

        public Job PostSaveJob(Job job)
        {
            if (job.Id > 0)
                DatabaseContext.Database.Update(job);
            else
                DatabaseContext.Database.Save(job);

            return job;
        }

        public int DeleteJobById(int id)
        {
            return DatabaseContext.Database.Delete<Job>(id);
        }
        #endregion

        #region JobTemplate
        public IEnumerable<JobTemplate> GetAllJobTemplate(int? companyID = null)
        {
            var query = new Sql().Select("*").From("JobsplusJobTemplates");
            if (companyID.HasValue) query = query.Where<JobTemplate>(x => x.VisibleForCompanyIds.Contains(companyID.Value.ToString())).OrderBy("Name");
            return DatabaseContext.Database.Fetch<JobTemplate>(query);
        }

        public JobTemplate GetJobTemplateById(int id)
        {
            var query = new Sql().Select("*").From("JobsplusJobTemplates").Where<JobTemplate>(x => x.Id == id);
            return DatabaseContext.Database.Fetch<JobTemplate>(query).FirstOrDefault();
        }

        public JobTemplate PostSaveJobTemplate(JobTemplate jobTemplate)
        {
            if (jobTemplate.Id > 0)
                DatabaseContext.Database.Update(jobTemplate);
            else
                DatabaseContext.Database.Save(jobTemplate);

            return jobTemplate;
        }

        public int DeleteJobTemplateById(int id)
        {
            return DatabaseContext.Database.Delete<JobTemplate>(id);
        }
        #endregion

        #region AdvertisementReply
        public List<AdvertisementReply> GetAdvertisementReplies(int advertisementId)
        {
            var query = new Sql().Select("*").From("JobsplusAdvertisementReply").Where<AdvertisementReply>(x => x.AdvertisementId == advertisementId);
            return DatabaseContext.Database.Fetch<AdvertisementReply>(query);
        }
        #endregion
    }
}
