using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using JobsplusUmbraco.Models;
using Umbraco.Core.Models;
using System.Xml.XPath;
using Jobsplus.Backoffice.Models;
using Jobsplus.Backoffice.Controllers;
using umbraco.cms.businesslogic.web;
using Umbraco.Web;
using System.Net;

namespace JobsplusUmbraco.Controllers
{
    public class AdvertisementController : SurfaceController
    {
        private DBContextController DBContext = new DBContextController();

        #region Region
        public List<JobsplusUmbraco.Models.Region> lRegions
        {
            get
            {
                List<JobsplusUmbraco.Models.Region> rCollection = new List<JobsplusUmbraco.Models.Region>();
                XPathNodeIterator iRegions = umbraco.library.GetPreValues(1139);
                if (iRegions.Count > 0 && iRegions.Current.HasChildren)
                {
                    iRegions.MoveNext();
                    XPathNodeIterator pvRegions = iRegions.Current.SelectChildren("preValue", "");
                    rCollection.Add(new JobsplusUmbraco.Models.Region { Name = "-- vyberte kraj --", Id = 0 });
                    while (pvRegions.MoveNext())
                    {
                        rCollection.Add(new JobsplusUmbraco.Models.Region { Name = pvRegions.Current.Value, Id = Convert.ToInt32(pvRegions.Current.GetAttribute("id", "")) });
                    }
                }

                return rCollection;
            }
        }

        public IEnumerable<SelectListItem> GetRegionSelectListItem(string selectItem)
        {
            return from s in lRegions
                   select new SelectListItem
                   {
                       Text = s.Name,
                       Value = s.Id.ToString(),
                       Selected = (/*s.Id.ToString() == selectItem ||*/ s.Name == selectItem)
                   };
        }
        #endregion

        #region WorkingField
        public List<WorkingField> lWorkingFields
        {
            get
            {
                List<WorkingField> wfCollection = new List<WorkingField>();
                XPathNodeIterator iWorkingFields = umbraco.library.GetPreValues(1147);
                if (iWorkingFields.Count > 0 && iWorkingFields.Current.HasChildren)
                {
                    iWorkingFields.MoveNext();
                    XPathNodeIterator pvWorkingFields = iWorkingFields.Current.SelectChildren("preValue", "");
                    wfCollection.Add(new WorkingField { Name = "-- vyberte obor --", Value = "0" });
                    //wfCollection.Add(new WorkingField { Name = "Obor nebo pozice?", Value = "Obor nebo pozice?" });
                    while (pvWorkingFields.MoveNext())
                    {
                        wfCollection.Add(new WorkingField { Name = pvWorkingFields.Current.Value, Value = pvWorkingFields.Current.GetAttribute("id", "") });
                    }
                }

                return wfCollection;
            }
        }

        public IEnumerable<SelectListItem> GetWorkingFieldSelectListItem(string selectItem)
        {
            return from s in lWorkingFields
                   select new SelectListItem
                   {
                       Text = s.Name,
                       Value = s.Value,
                       Selected = s.Value == selectItem || s.Name == selectItem
                   };
        }
        #endregion

        #region TypeOfWork
        public List<TypeOfWork> lTypeOfWorks
        {
            get
            {
                List<TypeOfWork> wfCollection = new List<TypeOfWork>();
                XPathNodeIterator iTypeOfWorks = umbraco.library.GetPreValues(1146);
                if (iTypeOfWorks.Count > 0 && iTypeOfWorks.Current.HasChildren)
                {
                    iTypeOfWorks.MoveNext();
                    XPathNodeIterator pvTypeOfWorks = iTypeOfWorks.Current.SelectChildren("preValue", "");
                    wfCollection.Add(new TypeOfWork { Name = "-- vyberte požadovaný vztah --", Value = "0" });
                    while (pvTypeOfWorks.MoveNext())
                    {
                        wfCollection.Add(new TypeOfWork { Name = pvTypeOfWorks.Current.Value, Value = pvTypeOfWorks.Current.GetAttribute("id", "") });
                    }
                }

                return wfCollection;
            }
        }

        public IEnumerable<SelectListItem> GetTypeOfWorkSelectListItem(string selectItem)
        {
            return from s in lTypeOfWorks
                   select new SelectListItem
                   {
                       Text = s.Name,
                       Value = s.Value,
                       Selected = s.Value == selectItem || s.Name == selectItem
                   };
        }
        #endregion

        #region RequiredEducation
        public List<RequiredEducation> lRequiredEducations
        {
            get
            {
                List<RequiredEducation> wfCollection = new List<RequiredEducation>();
                XPathNodeIterator iRequiredEducations = umbraco.library.GetPreValues(1149);
                if (iRequiredEducations.Count > 0 && iRequiredEducations.Current.HasChildren)
                {
                    iRequiredEducations.MoveNext();
                    XPathNodeIterator pvRequiredEducations = iRequiredEducations.Current.SelectChildren("preValue", "");
                    wfCollection.Add(new RequiredEducation { Name = "-- vyberte dosažené vzdělání --", Value = "0" });
                    while (pvRequiredEducations.MoveNext())
                    {
                        wfCollection.Add(new RequiredEducation { Name = pvRequiredEducations.Current.Value, Value = pvRequiredEducations.Current.GetAttribute("id", "") });
                    }
                }

                return wfCollection;
            }
        }

        public IEnumerable<SelectListItem> GetRequiredEducationSelectListItem(string selectItem)
        {
            return from s in lRequiredEducations
                   select new SelectListItem
                   {
                       Text = s.Name,
                       Value = s.Value,
                       Selected = s.Value == selectItem || s.Name == selectItem
                   };
        }
        #endregion

        #region JobTemplate
        public List<JobTemplate> lJobTemplates
        {
            get
            {
                var company = Company();
                if (company == null) return null;

                IEnumerable<JobTemplate> ieJobTemplates = DBContext.GetAllJobTemplate(company.Id);
                if (ieJobTemplates == null) return null;

                return ieJobTemplates.ToList();
            }
        }

        public IEnumerable<SelectListItem> GetJobTemplateSelectListItem(string selectItem)
        {
            List<JobTemplate> jobTemplates = new List<JobTemplate>();
            jobTemplates.Add(new JobTemplate { Id = 0, Name = "-- vyberte šablonu --" });
            jobTemplates.AddRange(lJobTemplates);

            return from s in jobTemplates
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
            /*
            var memberPicker = GetMemberId();

            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            var company = umbracoHelper.TypedContentSingleAtXPath("//dtCompanyList").Children.Where("cMemberPicker =" + memberPicker);

            if (company != null && company.Count() > 0)
                return (IPublishedContent)umbracoHelper.TypedContent(Convert.ToInt32(company.First().Id));
            else
                return null;
             */
            var memberCompany = GetMember();
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            // DKO: získá napojení na stránku firmy z nastavení uživatele v členské sekci
            return umbracoHelper.Content(memberCompany.Properties["CompanyPage"].Value);  
        }

        public IPublishedContent CompanyContent()
        {
            var company = Company();
            return company != null && company.FirstChild() != null ? company.FirstChild() : null;
        }

        public IEnumerable<IPublishedContent> AdvertisementList()
        {
            var company = Company();
            return company != null && company.FirstChild() != null && company.FirstChild().Children() != null ? company.FirstChild().Children().Where("Visible") : null;
        }

        public List<Advertisement> Fill(IEnumerable<IPublishedContent> advertisementList)
        {
            if (advertisementList != null)
            {
                List<Advertisement> advertisements = new List<Advertisement>();
                foreach (var result in advertisementList)
                {
                    //Advertisement advertisement = this.DynamicToAdverisement(result.Id);
                    Advertisement advertisement = new Advertisement();
                    advertisement.DynamicToAdverisement(result.Id);
                    if (advertisement != null) advertisements.Add(advertisement);
                }
                return advertisements;
            }
            else
                return new List<Advertisement>();
        }

        // GET: Advertisement
        public ActionResult Index()
        {            
            return PartialView();
        }

        public ActionResult Details(int? id)
        {
            Advertisement advertisement = new Advertisement();
            if (id.HasValue)
            {
                if (TempData.ContainsKey("VisibleJobTemplate")) TempData.Remove("VisibleJobTemplate");
                advertisement.DynamicToAdverisement(id.Value);
                ViewData["slRegion"] = GetRegionSelectListItem(advertisement.Region.Name);
                ViewData["slWorkingField"] = GetWorkingFieldSelectListItem(advertisement.WorkingField.Name);
                ViewData["slTypeOfWork"] = GetTypeOfWorkSelectListItem(advertisement.TypeOfWork.Name);
                ViewData["slRequiredEducation"] = GetRequiredEducationSelectListItem(advertisement.RequiredEducation.Name);
            }
            else
            {
                if (TempData.ContainsKey("VisibleJobTemplate")) TempData.Remove("VisibleJobTemplate");
                TempData.Add("VisibleJobTemplate", true);

                if (TempData.ContainsKey("JobTemplate"))
                {
                    JobTemplate jobTemplate = (JobTemplate)TempData["JobTemplate"];
                    if (jobTemplate != null)
                    {
                        advertisement.Name = jobTemplate.JobName;
                        advertisement.JobDescription = jobTemplate.JobDescription;
                        advertisement.JobOfferings = jobTemplate.JobOfferings;
                        advertisement.JobRequirements = jobTemplate.JobRequirements;
                        ViewData["slJobTemplate"] = GetJobTemplateSelectListItem(jobTemplate.Id.ToString());
                    }
                }
                else
                {
                    ViewData["slJobTemplate"] = GetJobTemplateSelectListItem(String.Empty);
                }

                ViewData["slRegion"] = GetRegionSelectListItem(String.Empty);
                ViewData["slWorkingField"] = GetWorkingFieldSelectListItem(String.Empty);
                ViewData["slTypeOfWork"] = GetTypeOfWorkSelectListItem(String.Empty);
                ViewData["slRequiredEducation"] = GetRequiredEducationSelectListItem(String.Empty);
            }

            return PartialView(advertisement);            
        }

        [HttpPost]
        public ActionResult JobTemplateSubmit(Advertisement model)
        {
            /*if (!ModelState.IsValid)
                return CurrentUmbracoPage();*/

            if (ViewData["slJobTemplate"] == null)
                ViewData["slJobTemplate"] = GetJobTemplateSelectListItem(String.Empty);
            if (ViewData["slRegion"] == null)
                ViewData["slRegion"] = GetRegionSelectListItem(String.Empty);
            if (ViewData["slWorkingField"] == null)
                ViewData["slWorkingField"] = GetWorkingFieldSelectListItem(String.Empty);
            if (ViewData["slTypeOfWork"] == null)
                ViewData["slTypeOfWork"] = GetTypeOfWorkSelectListItem(String.Empty);
            if (ViewData["slRequiredEducation"] == null)
                ViewData["slRequiredEducation"] = GetRequiredEducationSelectListItem(String.Empty);

            JobTemplate jobTemplate = DBContext.GetJobTemplateById(model.JobTemplateID);
            if (jobTemplate != null)
            {
                model.Name = jobTemplate.JobName;
                model.JobDescription = jobTemplate.JobDescription;
                model.JobOfferings = jobTemplate.JobOfferings;
                model.JobRequirements = jobTemplate.JobRequirements;
            }

            if (TempData.ContainsKey("JobTemplate")) TempData.Remove("JobTemplate");
            TempData.Add("JobTemplate", jobTemplate);

            return CurrentUmbracoPage();
        }

        [HttpPost]
        public ActionResult AdvertisementSubmit(Advertisement model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            var companyContent = CompanyContent();

            var contentService = Services.ContentService;
            var produkt = contentService.CreateContent(model.Name, companyContent.Id, "dtAdvertisement");
            produkt.SetValue("aContent", !String.IsNullOrEmpty(model.Content) ? model.Content : " ");
            produkt.SetValue("aTypeOfWork", model.TypeOfWorkID);
            produkt.SetValue("aRegion", model.RegionID);
            produkt.SetValue("aWorkingField", model.WorkingFieldID);
            produkt.SetValue("aRequiredEducation", model.RequiredEducationID);
            produkt.SetValue("aCity", !String.IsNullOrEmpty(model.City) ? model.City : " ");
            produkt.SetValue("aZTP", model.ZTP ? "1" : "0");
            produkt.SetValue("aJobDescription", !String.IsNullOrEmpty(model.JobDescription) ? model.JobDescription : " ");
            produkt.SetValue("aJobOfferings", !String.IsNullOrEmpty(model.JobOfferings) ? model.JobOfferings : " ");
            produkt.SetValue("aJobRequirements", !String.IsNullOrEmpty(model.JobRequirements) ? model.JobRequirements : " ");

            var status = contentService.SaveAndPublishWithStatus(produkt);

            if (TempData.ContainsKey("AdvertisementSubmitIsSuccess")) TempData.Remove("AdvertisementSubmitIsSuccess");
            if (status.Success) TempData.Add("AdvertisementSubmitIsSuccess", "save");

            return Redirect("/firma/inzeraty");
        }

        public ActionResult List()
        {
            var advertisementList = AdvertisementList();
            return PartialView(Fill(advertisementList));
        }

        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var contentService = Services.ContentService;
            var advertisement = contentService.GetById(id.Value);
            contentService.Delete(contentService.GetById(id.Value));

            if (TempData.ContainsKey("AdvertisementSubmitIsSuccess")) TempData.Remove("AdvertisementSubmitIsSuccess");
            TempData.Add("AdvertisementSubmitIsSuccess", "delete");

            return Redirect("/firma/inzeraty");
        }

        public ActionResult CloseSuccessMessage(string url)
        {
            if (TempData.ContainsKey("AdvertisementSubmitIsSuccess")) TempData.Remove("AdvertisementSubmitIsSuccess");
            if (!string.IsNullOrEmpty(url))
                return Redirect(url);
            else
                return RedirectToCurrentUmbracoPage();
        }
    }
}