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
            jobs.Add(new Job { Id = 0, Name = "-- vyberte pracovní pozici --" });
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

        // GET: JobTemplates10
        public ActionResult Index()
        {
            return PartialView(DBContext.GetAllJobTemplate());
        }

        public ActionResult Details(int? id)
        {
            JobTemplate jobTemplate = null;
            if (id.HasValue)
            {
                jobTemplate = DBContext.GetJobTemplateById(id.Value);
                if (jobTemplate == null)
                    return HttpNotFound();

                jobTemplate.slJob = GetJobSelectListItem(jobTemplate.JobId.ToString());
            }
            else
            {
                jobTemplate = new JobTemplate();
                jobTemplate.slJob = GetJobSelectListItem(String.Empty);
            }
            
            
            return PartialView(jobTemplate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Name,IsGeneralTemplate,IsVisibleForAll,TemplateUrl,JobName,JobDescription,JobRequirements,JobOfferings")] JobTemplate jobTemplate)
        public ActionResult Details([Bind(Include = "Id,Name,IsGeneralTemplate,JobId,JobDescription,JobRequirements,JobOfferings")] JobTemplate jobTemplate)
        {
            if (!ModelState.IsValid || jobTemplate == null)
                return CurrentUmbracoPage();

            var company = Company();
            Job job = DBContext.GetJobById(jobTemplate.JobId);
            if (job != null)
            {
                jobTemplate.JobName = job.Name;
                jobTemplate.VisibleForCompanyIds = company != null ? company.Id.ToString() : " ";
                jobTemplate.Save();

                if (TempData.ContainsKey("JobTemplateSubmitIsSuccess")) TempData.Remove("JobTemplateSubmitIsSuccess");
                TempData.Add("JobTemplateSubmitIsSuccess", true);

                //return Redirect("/firma/sablony");
                return RedirectToAction("Index");
            }

            return RedirectToCurrentUmbracoPage();
        }

        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            JobTemplate jobTemplate = DBContext.GetJobTemplateById(id.Value);
            
            if (jobTemplate == null)
                return HttpNotFound();

            return RedirectToCurrentUmbracoPage(); //View(jobTemplate);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DBContext.DeleteJobTemplateById(id);
            return RedirectToAction("Index");
        }

        public ActionResult CloseSuccessMessage(string url)
        {
            if (TempData.ContainsKey("JobTemplateSubmitIsSuccess")) TempData.Remove("JobTemplateSubmitIsSuccess");
            if (!string.IsNullOrEmpty(url))
                return Redirect(url);
            else
                return RedirectToCurrentUmbracoPage();
        }
    }
}
