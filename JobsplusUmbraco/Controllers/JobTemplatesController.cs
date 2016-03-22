using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Jobsplus.Backoffice.Models;
using Jobsplus.Backoffice.Controllers;
using JobsplusUmbraco.Models;
using Umbraco.Web.Mvc;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace JobsplusUmbraco.Controllers
{
    public class JobTemplatesController : SurfaceController
    {
        private DBContextController DBContext = new DBContextController();

        #region Job
        public List<Job> lJobs
        {
            get
            {
                IEnumerable<Job> ieJobs = DBContext.GetAllJob();
                if (ieJobs == null) return null;

                return ieJobs.ToList();
            }
        }

        public IEnumerable<SelectListItem> GetJobSelectListItem(string selectItem)
        {
            List<Job> jobs = new List<Job>();
            jobs.Add(new Job { Id = null, Name = "-- vyberte pracovní pozici --" });
            jobs.AddRange(lJobs);

            return from s in jobs
                   select new SelectListItem
                   {
                       Text = s.Name,
                       Value = s.Id.ToString(),
                       Selected = s.Id.ToString() == selectItem
                   };
        }
        #endregion

        #region Method
        public int GetMemberId()
        {
            var memberCompany = GetMember();
            return memberCompany != null ? memberCompany.Id : 0;
        }

        public IMember GetMember()
        {
            var memberService = Services.MemberService;
            var profile = Members.GetCurrentMemberProfileModel();
            return memberService.GetByUsername(Members.CurrentUserName);
        }

        public IPublishedContent Company()
        {
            var memberCompany = GetMember();
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            // DKO: získá napojení na stránku firmy z nastavení uživatele v členské sekci
            return umbracoHelper.Content(memberCompany.Properties["CompanyPage"].Value);
        }
        #endregion

        #region ActionResult
        public ActionResult Index()
        {
            return PartialView(DBContext.GetAllJobTemplate());
        }

        public ActionResult Details(int? id)
        {
            if (TempData.ContainsKey("CompanyId")) TempData.Remove("CompanyId");
            JobTemplate jobTemplate = null;
            if (id.HasValue)
            {
                jobTemplate = DBContext.GetJobTemplateById(id.Value);
                TempData.Add("CompanyId", Company().Id);
                
                if (jobTemplate == null)
                    return HttpNotFound();

                ViewData["slJob"] = GetJobSelectListItem(jobTemplate.JobId.ToString());
            }
            else
            {
                jobTemplate = new JobTemplate();
                ViewData["slJob"] = GetJobSelectListItem(String.Empty);
            }       
            
            return PartialView(jobTemplate);
        }

        [HttpPost]
        public ActionResult JobTemplateSubmit(JobTemplate jobTemplate)
        {
            if (!ModelState.IsValid || jobTemplate == null)
                return CurrentUmbracoPage();

            var company = Company();
            Job job = DBContext.GetJobById(jobTemplate.JobId);
            if (job != null)
            {
                if (jobTemplate.Id > 0)
                {
                    List<int> visibleCompanyIds = jobTemplate.GetForCompanyIds();
                    if (!visibleCompanyIds.Contains(company.Id))
                        visibleCompanyIds.Add(company.Id);
                    jobTemplate.VisibleForCompanyIds = JobsplusHelpers.ArrayToString(visibleCompanyIds.ToArray(), ",");
                    jobTemplate.UpdatedDate = DateTime.Now;
                }
                else
                {
                    jobTemplate.IsGeneralTemplate = false;
                    jobTemplate.IsVisibleForAll = false;
                        jobTemplate.VisibleForCompanyIds = " ";
                    jobTemplate.TemplateUrl = String.Empty;
                    jobTemplate.CreatedDate = DateTime.Now;
                    jobTemplate.UpdatedDate = DateTime.Now;
                    if (company != null)
                    {                        
                        jobTemplate.VisibleForCompanyIds = company.Id.ToString();
                        jobTemplate.CreatedByCompanyId = company.Id;
                        jobTemplate.CreatedByCompanyName = company.Name;
                    } 
                }

                jobTemplate.JobName = job.Name;
                jobTemplate.Save();

                if (TempData.ContainsKey("JobTemplateSubmitIsSuccess")) TempData.Remove("JobTemplateSubmitIsSuccess");
                TempData.Add("JobTemplateSubmitIsSuccess", "save");

                return Redirect("/firma/sablony");
            }

            return CurrentUmbracoPage();
        }

        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            DBContext.DeleteJobTemplateById(id.Value);

            if (TempData.ContainsKey("JobTemplateSubmitIsSuccess")) TempData.Remove("JobTemplateSubmitIsSuccess");
            TempData.Add("JobTemplateSubmitIsSuccess", "delete");

            return Redirect("/firma/sablony");
        }

        public ActionResult CloseSuccessMessage(string url)
        {
            if (TempData.ContainsKey("JobTemplateSubmitIsSuccess")) TempData.Remove("JobTemplateSubmitIsSuccess");
            if (!string.IsNullOrEmpty(url))
                return Redirect(url);
            else
                return RedirectToCurrentUmbracoPage();
        }
        #endregion
    }
}
