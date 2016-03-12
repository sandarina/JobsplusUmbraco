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

namespace JobsplusUmbraco.Controllers
{
    public class MarketingActualsController : SurfaceController
    {
        // GET: MarketingActuals
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            return PartialView(GetMarketingActualsList());
        }

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
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
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
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            var list = CurrentPage.AncestorsOrSelf().First().Descendants("dtNews").Where(c => c.GetPropertyValue<int>("nCompanyNodeId") == company.Id);
         //       (c => c.DocumentTypeAlias == "dtNews" && c.GetPropertyValue<int>("nCompanyNodeId") == company.Id);//.Where(c => c.GetPropertyValue<int>("nCompanyNodeId") == company.Id); //.Where("nCompanyNodeId == @0", company.Id.ToString()).ToList<IPublishedContent>();

            foreach(var item in list)
            {
                var dItem = item.AsDynamic();
                actualList.Add(
                    new MarketingActual()
                    {
                        ID = dItem.Id,
                        Name = dItem.Name,
                        Date = dItem.nDate,
                        IsPublished = dItem.Published,
                        Thumbnail = dItem.nThubnail,
                        BodyText = dItem.nBodyText
                    }
                );
            }
            return actualList;
            //var list = umbracoHelper.TypedContentAtRoot().Where(c => c.DocumentTypeAlias == "dtNews" && c.Properties["nCompanyNodeId"] == company.Id);

        }
        #endregion
    }
}