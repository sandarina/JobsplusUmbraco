﻿using System;
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
        private UmbracoDatabase db { get { return ApplicationContext.DatabaseContext.Database; } }

        #region Specialization
        public IEnumerable<Specialization> GetAllSpecialization()
        {
            var query = new Sql().Select("*").From("JobsplusSpecializations").OrderBy("Order");
            return db.Fetch<Specialization>(query);
        }

        public Specialization GetSpecializationById(int id)
        {
            var query = new Sql().Select("*").From("JobsplusSpecializations").Where<Specialization>(x => x.Id == id);
            return db.Fetch<Specialization>(query).FirstOrDefault();
        }

        public Specialization PostSaveSpecialization(Specialization specialization)
        {
            if (specialization.Id > 0)
                db.Update(specialization);
            else
                db.Insert(specialization);

            return specialization;
        }

        public int DeleteSpecializationById(int id)
        {
            return db.Delete<Specialization>(id);
        }
        #endregion

        #region Job
        public IEnumerable<Job> GetAllJob()
        {
            var query = new Sql().Select("*").From("JobsplusJobs").OrderBy("Name");
            return db.Fetch<Job>(query);
        }

        public Job GetJobById(int id)
        {
            var query = new Sql().Select("*").From("JobsplusJobs").Where<Job>(x => x.Id == id);
            return db.Fetch<Job>(query).FirstOrDefault();
        }

        public Job GetJobByName(string name)
        {
            var query = new Sql().Select("*").From("JobsplusJobs").Where<Job>(x => x.Name.Contains(name));
            return db.Fetch<Job>(query).FirstOrDefault();
        }

        public Job PostSaveJob(Job job)
        {
            if (job.Id > 0)
                db.Update(job);
            else
                db.Insert(job);

            return job;
        }

        public int DeleteJobById(int id)
        {
            return db.Delete<Job>(id); 
        }
        #endregion

        #region JobTemplate
        public IEnumerable<JobTemplate> GetAllJobTemplate(int? companyID = null)
        {
            var query = new Sql().Select("*").From("JobsplusJobTemplates");
            if (companyID.HasValue)
            {
                // DKO: doplňující INFO
                // jedná se obecnou šablonu viditelnou pro všechny nebo pro tuto firmu
                // jedná se o šablonu, kterou si založila přihlášená firma pro své vlastní účely
                /*
                query = query.Where<JobTemplate>(x => 
                    (x.IsGeneralTemplate && (x.IsVisibleForAll || x.VisibleForCompanyIds.Equals(companyID.Value.ToString()) || x.VisibleForCompanyIds.Contains("," + companyID.Value.ToString()))) ||
                    (x.CreatedByCompanyId.HasValue && x.CreatedByCompanyId.Value.Equals(companyID.Value))
                    ).OrderBy("Name");
                 */
                var sQuery = @"(IsGeneralTemplate = 1 AND (IsVisibleForAll = 1 OR (VisibleForCompanyIds = @0 OR VisibleForCompanyIds LIKE '%,@0%'))) 
                    OR (CreatedByCompanyId IS NOT NULL AND CreatedByCompanyId = @0)";
                query.Where(sQuery, companyID.Value.ToString());
            }
            return db.Fetch<JobTemplate>(query); 
        }

        public JobTemplate GetJobTemplateById(int id)
        {
            var query = new Sql().Select("*").From("JobsplusJobTemplates").Where<JobTemplate>(x => x.Id == id);
            return db.Fetch<JobTemplate>(query).FirstOrDefault(); 
        }

        public JobTemplate PostSaveJobTemplate(JobTemplate jobTemplate)
        {
            if (jobTemplate.Id > 0)
                db.Update(jobTemplate);
            else
                db.Insert(jobTemplate);

            return jobTemplate;
        }

        public int DeleteJobTemplateById(int id)
        {
            return db.Delete<JobTemplate>(id); 
        }
        #endregion

        #region AdvertisementReply
        public List<AdvertisementReply> GetAdvertisementReplies(int advertisementId)
        {
            var query = new Sql().Select("*").From("JobsplusAdvertisementReply").Where<AdvertisementReply>(x => x.AdvertisementId == advertisementId);
            return db.Fetch<AdvertisementReply>(query);
        }
        #endregion
    }
}
