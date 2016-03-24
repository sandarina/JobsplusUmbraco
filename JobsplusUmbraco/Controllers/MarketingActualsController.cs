using JobsplusUmbraco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;
using Umbraco.Web.Models;
using Newtonsoft.Json;
using System.IO;
using System.Drawing;

namespace JobsplusUmbraco.Controllers
{
    public class MarketingActualsController : SurfaceController
    {
        #region Actions
        
        // GET: MarketingActuals
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            return PartialView(GetMarketingActualsList());
        }

        public ActionResult Details(int? id)
        {
            var marketingActual = new MarketingActual();
            if (id.HasValue)
            {
                var ma = Services.ContentService.GetById(id.Value);
                marketingActual.ID = ma.Id;
                marketingActual.Name = ma.Name;
                marketingActual.Thumbnail = JsonConvert.DeserializeObject<ImageCropDataSet>(ma.GetValue<string>("nThumbnail"));
                marketingActual.Description = ma.GetValue<string>("nDescription");
                marketingActual.Content = ma.GetValue<HtmlString>("nContent");
                marketingActual.IsPublished = ma.Published;
                marketingActual.Date = ma.GetValue<DateTime>("nDate").ToString("dd.MM.yyyy");
            }
            else
            {
                marketingActual.Date = DateTime.Now.ToString("dd.MM.yyyy");
            }
            return PartialView(marketingActual);
        }

        public ActionResult Publish(int? id)
        {
            if (id.HasValue)
            {
                var content = Services.ContentService.GetById(id.Value);
                Services.ContentService.Publish(content);
            }
            return Redirect("/firma/marketingove-aktuality/");
           // content.
        }

        public ActionResult Unpublish(int? id)
        {

            if (id.HasValue)
            {
                var content = Services.ContentService.GetById(id.Value);
                Services.ContentService.UnPublish(content);
            }
            return Redirect("/firma/marketingove-aktuality/");
        }

        [HttpPost]
        public ActionResult MarketingActualSubmit(MarketingActual marketingActual)
        {
            if (!ModelState.IsValid || marketingActual == null)
                return CurrentUmbracoPage();

            var company = Company();
            var contentService = Services.ContentService;
             
            IContent ma = null;
            if (!marketingActual.ID.HasValue)
            {
                var maParent = umbracoHelper.TypedContentAtRoot().First().FirstChild(x => x.ContentType.Alias.Equals("dtNewsList"));
                ma = contentService.CreateContent(marketingActual.Name, maParent.Id, "dtNews");
            }
            else
                ma = contentService.GetById(marketingActual.ID.Value);

            ma.Name = marketingActual.Name;
            ma.SetValue("nCompanyNodeId", company.Id);
            ma.SetValue("nDescription", marketingActual.Description);
            ma.SetValue("nContent", marketingActual.Content);
            if (marketingActual.NewThumbnail != null && marketingActual.NewThumbnail.InputStream != null)
            {
                var filename = JobsplusHelpers.RemoveDiacritics(Path.GetFileName(marketingActual.NewThumbnail.FileName));

                var path = "/media/" + ma.Id.ToString() + "/";
                var fullPath = Server.MapPath("~" + path);
                var dir = new DirectoryInfo(fullPath);
                if (!dir.Exists)
                    dir.Create();
                try
                {
                    
                    marketingActual.NewThumbnail.SaveAs(fullPath + filename);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Při nahrávání náhledového obrázku došlo k chybě:");
                    TempData.Add("ValidationErrorInfo", JobsplusHelpers.GetMsgFromException(ex));
                    return CurrentUmbracoPage();
                }
                var filepath = path + filename;
                Image img = null;
                try
                {
                    img = Image.FromFile(fullPath + filename);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Nahrávaný soubor náhledového obrázku nemá správný formát:");
                    System.IO.File.Delete(fullPath + filename);
                    TempData.Add("ValidationErrorInfo", JobsplusHelpers.GetMsgFromException(ex));
                    return CurrentUmbracoPage();
                }

                var newThumbnail = new ImageCropDataSet();
                newThumbnail.FocalPoint.Left = new decimal(0.5);
                newThumbnail.FocalPoint.Top = new decimal(0.5);
                newThumbnail.Src = filepath;
                ma.SetValue("nThumbnail", newThumbnail);
            }
            contentService.Save(ma);

            if (marketingActual.IsPublished)
                contentService.Publish(ma);
            else
                contentService.UnPublish(ma);

            return Redirect("/firma/marketingove-aktuality/");
        }
        #endregion

        #region Custom Properties
        public UmbracoHelper umbracoHelper
        {
            get
            {
                return new UmbracoHelper(UmbracoContext.Current);
            }
        }
        #endregion

        #region Methods

        public IMember GetMember()
        {
            var memberService = Services.MemberService;
            var profile = Members.GetCurrentMemberProfileModel();
            return memberService.GetByUsername(Members.CurrentUserName);
        }

        public IPublishedContent Company()
        {
            var memberCompany = GetMember();
            // DKO: získá napojení na stránku firmy z nastavení uživatele v členské sekci
            return umbracoHelper.Content(memberCompany.Properties["CompanyPage"].Value);
        }

        public IPublishedContent CompanyContent()
        {
            var company = Company();
            return company != null && company.FirstChild() != null ? company.FirstChild() : null;
        }

        public IEnumerable<MarketingActual> GetMarketingActualsList()
        {
            var actualList = new List<MarketingActual>();
            var company = Company();

            // DKO pozn.:
            // UmbracoHelper => pracuje pouze z publikovaným obsahem
            // ContentService => pracuje s veškerým obsahem, včetně nebulikovaného, vyhozeného v koši, atd.
            var root = umbracoHelper.TypedContentAtRoot().First(); 
            var list = Services.ContentService.GetDescendants(root.Id).Where(x => !x.Trashed && x.ContentType.Alias.Equals("dtNews") && x.GetValue<int>("nCompanyNodeId").Equals(company.Id));
            
            foreach(var item in list)
            {
                DateTime? dt = item.GetValue("nDate") as DateTime?;
                actualList.Add(
                    new MarketingActual()
                    {
                        ID = item.Id,
                        Url = item.Published ? umbracoHelper.NiceUrl(item.Id) : "#",
                        Name = item.Name,
                        Date = dt.HasValue ? dt.Value.ToString("dd.MM.yyyy") : "-",
                        IsPublished = item.Published,
                        Thumbnail = item.GetValue<ImageCropDataSet>("nThumbnail"),
                        Description = item.GetValue<string>("nDescription"),
                        Content = item.GetValue<HtmlString>("nContent")
                    }
                );
            }
            return actualList;
        }
        #endregion
    }
}