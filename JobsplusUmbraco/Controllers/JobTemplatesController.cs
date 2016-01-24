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

namespace JobsplusUmbraco.Controllers
{
    public class JobTemplatesController : SurfaceController
    {
        private DBContextController DBContext = new DBContextController();

        // GET: JobTemplates10
        public ActionResult Index()
        {
            return PartialView(DBContext.GetAllJobTemplate());
        }

        // GET: JobTemplates/Details/5
        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            JobTemplate jobTemplate = DBContext.GetJobTemplateById(id.Value);
            
            if (jobTemplate == null)
            {
                return HttpNotFound();
            }
            return RedirectToCurrentUmbracoPage();//View(jobTemplate);
        }

        // GET: JobTemplates/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: JobTemplates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,IsGeneralTemplate,IsVisibleForAll,TemplateUrl,JobName,JobDescription,JobRequirements,JobOfferings")] JobTemplate jobTemplate)
        {
            if (ModelState.IsValid && jobTemplate != null)
            {
                Job job = DBContext.GetJobByName(jobTemplate.JobName);
                if (job != null)
                {
                    jobTemplate.JobId = job.Id;
                    jobTemplate.VisibleForCompanyIds = "";
                    jobTemplate.Save();
                    return RedirectToAction("Index");
                }              
            }

            return RedirectToCurrentUmbracoPage();
        }

        // GET: JobTemplates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            JobTemplate jobTemplate = DBContext.GetJobTemplateById(id.Value);
            
            if (jobTemplate == null)
            {
                return HttpNotFound();
            }
            return RedirectToCurrentUmbracoPage();//View(jobTemplate);
        }

        // POST: JobTemplates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,IsGeneralTemplate,IsVisibleForAll,TemplateUrl,JobName,JobDescription,JobRequirements,JobOfferings")] JobTemplate jobTemplate)
        {
            if (ModelState.IsValid)
            {
                jobTemplate.Save();
                return RedirectToAction("Index");
            }
            return View(jobTemplate);
        }

        // GET: JobTemplates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            JobTemplate jobTemplate = DBContext.GetJobTemplateById(id.Value);
            
            if (jobTemplate == null)
            {
                return HttpNotFound();
            }
            return RedirectToCurrentUmbracoPage(); //View(jobTemplate);
        }

        // POST: JobTemplates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DBContext.DeleteJobTemplateById(id);
            return RedirectToAction("Index");
        }

        /*protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }*/
    }
}
