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

        public ActionResult Publish(int? id)
        {
            if (id.HasValue)
            {
                var content = Services.ContentService.GetById(id.Value);
                Services.ContentService.Publish(content);
            }
            return Redirect("/firma/maketingove-aktuality/");
           // content.
        }

        public ActionResult Unpublish(int? id)
        {

            if (id.HasValue)
            {
                var content = Services.ContentService.GetById(id.Value);
                Services.ContentService.UnPublish(content);
            }
            return Redirect("/firma/maketingove-aktuality/");
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