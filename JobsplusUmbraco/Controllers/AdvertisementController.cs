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
using Umbraco.Core.Persistence;
using Jobsplus.Backoffice;
using System.Net.Mail;

namespace JobsplusUmbraco.Controllers
{
    public class AdvertisementController : SurfaceController
    {
        private DBContextController DBContext = new DBContextController();
        
        private UmbracoDatabase _db { get { return ApplicationContext.DatabaseContext.Database; } }

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
                    rCollection.Add(new JobsplusUmbraco.Models.Region { Name = "-- vyberte kraj --", Value = "0" });
                    while (pvRegions.MoveNext())
                    {
                        rCollection.Add(new JobsplusUmbraco.Models.Region { Name = pvRegions.Current.Value, Value = pvRegions.Current.GetAttribute("id", "") });
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
                       Value = s.Value,
                       Selected = s.Value == selectItem,
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
                       Selected = s.Value == selectItem
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
                       Selected = s.Value == selectItem
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
                       Selected = s.Value == selectItem
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

        public List<AdvertisementReply> GetReplies(int advertisementId)
        {
            return AdvertisementReply.GetAdvertisementReplies(advertisementId, _db);
        }

        // GET: Advertisement
        public ActionResult Index()
        {            
            return PartialView();
        }

        public ActionResult Create()
        {
            Advertisement model = new Advertisement();
            if (TempData.ContainsKey("JobTemplate"))
            {
                JobTemplate jobTemplate = (JobTemplate)TempData["JobTemplate"];
                if (jobTemplate != null)
                {
                    model.Name = jobTemplate.JobName;
                    model.JobDescription = jobTemplate.JobDescription;
                    model.JobOfferings = jobTemplate.JobOfferings;
                    model.JobRequirements = jobTemplate.JobRequirements;
                    model.slJobTemplate = GetJobTemplateSelectListItem(jobTemplate.Id.ToString());
                } 
            }
            else
            {
                model.slJobTemplate = GetJobTemplateSelectListItem(String.Empty);
            }

            model.slRegion = GetRegionSelectListItem(String.Empty);
            model.slWorkingField = GetWorkingFieldSelectListItem(String.Empty);
            model.slTypeOfWork = GetTypeOfWorkSelectListItem(String.Empty);
            model.slRequiredEducation = GetRequiredEducationSelectListItem(String.Empty);
            
                        
            return PartialView(model);            
        }

        [HttpPost]
        public ActionResult JobTemplateSubmit(Advertisement model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            if (model.slJobTemplate == null)
                model.slJobTemplate = GetJobTemplateSelectListItem(String.Empty);
            if (model.slRegion == null)
                model.slRegion = GetRegionSelectListItem(String.Empty);
            if (model.slWorkingField == null)
                model.slWorkingField = GetWorkingFieldSelectListItem(String.Empty);
            if (model.slTypeOfWork == null)
                model.slTypeOfWork = GetTypeOfWorkSelectListItem(String.Empty);
            if (model.slRequiredEducation == null)
                model.slRequiredEducation = GetRequiredEducationSelectListItem(String.Empty);

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
            produkt.SetValue("aContent", model.Content);
            produkt.SetValue("aTypeOfWork", model.TypeOfWorkID);
            produkt.SetValue("aRegion", model.RegionID);
            produkt.SetValue("aWorkingField", model.WorkingFieldID);
            produkt.SetValue("aRequiredEducation", model.RequiredEducationID);
            produkt.SetValue("aCity", model.City);
            produkt.SetValue("aZTP", model.ZTP ? "1" : "0");
            produkt.SetValue("aJobDescription", model.JobDescription);
            produkt.SetValue("aJobOfferings", model.JobOfferings);
            produkt.SetValue("aJobRequirements", model.JobRequirements);

            var status = contentService.SaveAndPublishWithStatus(produkt);

            if (TempData.ContainsKey("AdvertisementSubmitIsSuccess")) TempData.Remove("AdvertisementSubmitIsSuccess");
            if (status.Success)
                TempData.Add("AdvertisementSubmitIsSuccess", true);
            else
                TempData.Add("AdvertisementSubmitIsSuccess", false);

            return Redirect("/firma/inzeraty");
        }

        public ActionResult List()
        {
            var advertisementList = AdvertisementList();
            AdvertisementList model = new AdvertisementList();
            model.Fill(advertisementList);

            return PartialView(model);
        }

        public ActionResult Replies(int AdvertisementId)
        {
            TempData.Add("MemberCannotViewAdvertisement", false);
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            var advertisement = umbracoHelper.Content(AdvertisementId) as IPublishedContent;

            var company = Company();
            var companyName = company.Name;

            if (advertisement.Parent.Parent.Id != company.Id)
            {
                TempData.Add("MemberCannotViewAdvertisement", true);
                return CurrentUmbracoPage();
            }

            var model = new RepliesForm();
            model.AdvertisementId = AdvertisementId;
            var replies = GetReplies(AdvertisementId);
            model.Replies = replies;
            // označit reakce za zobrazené
            foreach (var reply in replies)
            {
                if (!reply.ViewDate.HasValue)
                {
                    reply.ViewDate = DateTime.Now;
                    _db.Save(reply);
                }
                else if (!reply.IsViewed)
                {
                    reply.IsViewed = true;
                    _db.Save(reply);
                }
            }
            model.CompanyName = companyName;
            /*
            model.Selection = new Dictionary<int,bool>();
            foreach(var reply in model.Replies)
            {
                model.Selection.Add(reply.Id, false);
            }*/
            model.SubmitAction = ESubmitAction.None;
            model.EmailText = @"Dobrý den,<br /><br />
děkujeme za Váš zájem o práci v naší firmě. Bohužel, do užšího výběru postoupili jiní uchazeči, kteří lépe odpovídali našim požadavkům. 
Ceníme si Vašich vědomostí a dovedností a proto jsme si dovolili diskrétně uložit Váš životopis do naší databáze uchazečů o zaměstnání.  
Rádi se s Vámi spojíme, vznikne-li u nás pracovní pozice odpovídající Vaší kvalifikaci.<br /><br />
Sledujte i nadále naše nabídky volných pracovních míst, které naleznete na webovém portálu http://jobsplus.cz/. <br /><br />
Přejeme Vám mnoho osobních i pracovních úspěchů.<br /><br />S pozdravem,<br />" + companyName;

            return PartialView(model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult RepliesSendSubmit(RepliesForm model, int[] replySelect)
        {
            #region Validation
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            if (replySelect.Count() == 0)
            {
                ModelState.AddModelError("", "Nevybrali jste žádné reakce k odmítnutí!");
                return CurrentUmbracoPage();
            }

            if (model.SubmitAction == ESubmitAction.DiscadWithEmail && string.IsNullOrWhiteSpace(model.EmailText))
            {
                ModelState.AddModelError("", "Uchazeči nebyli odmítnuti ,protože text emailu je prázdný!");
                return CurrentUmbracoPage();
            }
            #endregion

            if (TempData.ContainsKey("RepliesSendSubmitMsg")) TempData.Remove("RepliesSendSubmitMsg");

            foreach(var id in replySelect)
            {
                var reply = AdvertisementReply.Get(id, _db);

                switch(model.SubmitAction)
                {
                    case ESubmitAction.DiscadWithEmail:
                        var mailCandidate = new MailMessage(JobsplusConstants.EmailRobotEmail, reply.CandidateEmail);

                        mailCandidate.Subject = "Odpověď od " + model.CompanyName;
                        mailCandidate.IsBodyHtml = true;
                        mailCandidate.Body = model.EmailText;
                        try
                        {
                            var smtpClient = new SmtpClient();
                            smtpClient.Send(mailCandidate);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", JobsplusConstants.SendEmailErrorMsg);
                            TempData.Add("ValidationErrorInfo", JobsplusHelpers.GetMsgFromException(ex));
                            return CurrentUmbracoPage();
                        }
                        DiscardReply(reply);
                        break;
                    case ESubmitAction.Discard:
                        DiscardReply(reply);
                        break;
                    default:
                        break;
                }
            }
            
            return RedirectToCurrentUmbracoPage("?AdvertisementId=" + model.AdvertisementId);
        }

        /// <summary>
        /// Označí reakci na inzerát za vyřízenou a odmítnutou. Uloží do DB.
        /// </summary>
        /// <param name="reply"></param>
        private void DiscardReply(AdvertisementReply reply)
        {
            reply.IsDiscarded = true;
            reply.IsCheckOut = true;
            reply.CheckOutDate = DateTime.Now;
            _db.Save(reply);
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