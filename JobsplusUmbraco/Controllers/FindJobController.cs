using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using JobsplusUmbraco.Models;
using Umbraco.Web.WebServices;
using umbraco.NodeFactory;
using Umbraco.Web.UI.Controls;
using Umbraco.Web;
using System.Xml.XPath;
using Umbraco.Core.Models;
using System.Data;
using Examine;
using umbraco.cms.businesslogic.member;
using Umbraco.Web.Security;
//using umbraco.presentation.nodeFactory;

namespace JobsplusUmbraco.Controllers
{
    public class FindJobController : RenderMvcController
    {
        #region Properties
        UmbracoHelper umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
        MembershipHelper membershipHelper = new MembershipHelper(UmbracoContext.Current);
        public IPublishedContent ipcAdvertisements;
        public IQueryable<IPublishedContent> iqAdvertisements;

        public List<Advertisement> AdverisementCollection
        {
            get
            {
                ipcAdvertisements = umbracoHelper.TypedContentSingleAtXPath("//dtAdvertisement");


                Node currentNode = Node.GetCurrent();
                var rootNode = new Node(int.Parse(currentNode.Path.Split(',')[1]));
                DataTable dtAdvertisement = rootNode.ChildrenAsTable("dtAdvertisement");

                foreach (var rAdvertisement in dtAdvertisement.Rows)
                {

                }

                if (ipcAdvertisements != null && ipcAdvertisements.Children.Count() > 0)
                {
                    if (IsTOP)
                    {
                        iqAdvertisements = ipcAdvertisements.Children.OrderBy("Name").OrderBy("topAdvertisement descending");
                    }
                    else
                    {
                        iqAdvertisements = ipcAdvertisements.Children.OrderBy("DateCreate");
                    }
                }
                // dodělat načtení properties inzerátu
                List<Advertisement> aCollection = new List<Advertisement>();
                foreach (var advertisement in ipcAdvertisements.Children)
                {
                    Advertisement ad = new Advertisement();
                    ad.TypeOfWork = advertisement.GetPropertyValue<string>("typeOfWork");

                    aCollection.Add(ad);
                }
                return aCollection;
            }
        }

        public bool IsTOP
        {
            get
            {
                string resultStr = QueryString("IsTOPAdvertisement").ToUpperInvariant();
                return (resultStr == "YES" || resultStr == "TRUE" || resultStr == "1");
            }
        }

        #endregion

        #region Methods
        public string QueryString(string name)
        {
            string result = string.Empty;
            if (Request != null && Request.QueryString[name] != null)
                result = Request.QueryString[name].ToString();
            return result;
        }

        protected List<Region> lRegions
        {
            get
            {
                List<Region> rCollection = new List<Region>();
                XPathNodeIterator iRegions = umbraco.library.GetPreValues(1139);
                if (iRegions.Count > 0 && iRegions.Current.HasChildren)
                {
                    iRegions.MoveNext();
                    XPathNodeIterator pvRegions = iRegions.Current.SelectChildren("preValue", "");
                    rCollection.Add(new Region { Name = string.Empty, Value = string.Empty });
                    while (pvRegions.MoveNext())
                    {
                        rCollection.Add(new Region { Name = pvRegions.Current.Value, Value = pvRegions.Current.Value });
                    }
                }

                return rCollection;
            }
        }

        protected IEnumerable<SelectListItem> GetRegionSelectListItem(string selectItem)
        {
            return from s in lRegions
                   select new SelectListItem
                   {
                       Text = s.Name,
                       Value = s.Value,
                       Selected = s.Value == selectItem
                   };
        }

        protected List<WorkingField> lWorkingFields
        {
            get
            {
                List<WorkingField> wfCollection = new List<WorkingField>();
                XPathNodeIterator iWorkingFields = umbraco.library.GetPreValues(1147);
                if (iWorkingFields.Count > 0 && iWorkingFields.Current.HasChildren)
                {
                    iWorkingFields.MoveNext();
                    XPathNodeIterator pvWorkingFields = iWorkingFields.Current.SelectChildren("preValue", "");
                    wfCollection.Add(new WorkingField { Name = string.Empty, Value = string.Empty });
                    while (pvWorkingFields.MoveNext())
                    {
                        wfCollection.Add(new WorkingField { Name = pvWorkingFields.Current.Value, Value = pvWorkingFields.Current.Value });
                    }
                }

                return wfCollection;
            }
        }

        protected IEnumerable<SelectListItem> GetWorkingFieldSelectListItem(string selectItem)
        {
            return from s in lWorkingFields
                   select new SelectListItem
                   {
                       Text = s.Name,
                       Value = s.Value,
                       Selected = s.Value == selectItem
                   };
        }

        protected Advertisement DynamicToAdverisement(dynamic item)
        {
            //var itemAdvertisement = (IPublishedContent)Umbraco.TypedContent(Convert.ToInt32(item["id"])); 
            var itemAdvertisement = (IPublishedContent)umbracoHelper.TypedContent(Convert.ToInt32(item["id"]));

            if (itemAdvertisement == null)
                return null;

            Advertisement advertisement = new Advertisement();
            advertisement.ID = itemAdvertisement.Id;
            advertisement.Name = itemAdvertisement.Name;
            advertisement.Url = itemAdvertisement.Url;
            advertisement.CreateDate = itemAdvertisement.CreateDate;
            advertisement.UpdateDate = itemAdvertisement.UpdateDate;

            advertisement.TOP = itemAdvertisement.GetPropertyValue<string>("aTop", "0") == "1" ? true : false;
            advertisement.TypeOfWork = itemAdvertisement.GetPropertyValue<string>("aTypeOfWork", string.Empty);
            advertisement.WorkingField = new WorkingField { Name = itemAdvertisement.GetPropertyValue<string>("aWorkingField", string.Empty), Value = itemAdvertisement.GetPropertyValue<string>("aWorkingField", string.Empty) };
            advertisement.RequiredEducation = itemAdvertisement.GetPropertyValue<string>("aRequiredEducation", string.Empty);
            advertisement.Region = new Region { Name = itemAdvertisement.GetPropertyValue<string>("aRegion", string.Empty), Value = itemAdvertisement.GetPropertyValue<string>("aRegion", string.Empty) };
            advertisement.City = itemAdvertisement.GetPropertyValue<string>("aCity", string.Empty);
            advertisement.ZTP = itemAdvertisement.GetPropertyValue<string>("aZtp", "0") == "1" ? true : false;
            advertisement.Content = itemAdvertisement.GetPropertyValue<string>("aContent", string.Empty);
            //advertisement.Advertiser = itemAdvertisement.GetPropertyValue<int?>("advertiser").HasValue ? Members.GetById(itemAdvertisement.GetPropertyValue<int>("advertiser")).Name : string.Empty;
            advertisement.Advertiser = itemAdvertisement.GetPropertyValue<int?>("Aadvertiser").HasValue ? membershipHelper.GetById(itemAdvertisement.GetPropertyValue<int>("aAdvertiser")).Name : string.Empty;

            return advertisement;
        }
        #endregion

        #region ActionResult

        public ActionResult tFindJob()
        {
            IEnumerable<SelectListItem> slRegions = GetRegionSelectListItem(string.Empty);
            IEnumerable<SelectListItem> slWorkingFields = GetWorkingFieldSelectListItem(string.Empty);

            List<Advertisement> advertisements = new List<Advertisement>();
            if (IsTOP)
            {
                var searcher = ExamineManager.Instance.SearchProviderCollection["InternalSearcher"];
                var criteria = searcher.CreateSearchCriteria(UmbracoExamine.IndexTypes.Content);
                Examine.SearchCriteria.IBooleanOperation filter = null;

                criteria.OrderByDescending(new string[] { "aTop" }).And().OrderBy(new string[] { "DateCreate" });
                filter = criteria.NodeTypeAlias("dtAdvertisement");

                foreach (var result in searcher.Search(filter.Compile()))
                {
                    Advertisement advertisement = DynamicToAdverisement(result);
                    if (advertisement != null) advertisements.Add(advertisement);
                }
            }

            AdvertisementList model = new AdvertisementList();
            model.slRegions = slRegions;
            model.slWorkingFields = slWorkingFields;
            model.lAdvertisements = advertisements;

            return CurrentTemplate(model);
        }

        [HttpPost]
        public ActionResult tFindJob(AdvertisementList model)
        {
            string selectRegion = model.region;
            string selectWorkingField = model.workingField;
            bool selectIsZTP = model.IsZTP;

            IEnumerable<SelectListItem> slRegions = GetRegionSelectListItem(selectRegion);
            IEnumerable<SelectListItem> slWorkingFields = GetWorkingFieldSelectListItem(selectWorkingField);

            var searcher = ExamineManager.Instance.SearchProviderCollection["InternalSearcher"];
            var criteria = searcher.CreateSearchCriteria(UmbracoExamine.IndexTypes.Content);
            Examine.SearchCriteria.IBooleanOperation filter = null;

            criteria.OrderBy(new string[] { "DateCreate" });
            filter = criteria.NodeTypeAlias("dtAdvertisement");

            if (!String.IsNullOrEmpty(selectWorkingField))
                filter.And().Field("aWorkingField", selectWorkingField);
            if (!String.IsNullOrEmpty(selectRegion))
                filter.And().Field("aRegion", selectRegion);
            filter.And().Field("aZtp", selectIsZTP ? "1" : "0");

            List<Advertisement> advertisements = new List<Advertisement>();
            foreach (var result in searcher.Search(filter.Compile()))
            {
                Advertisement advertisement = DynamicToAdverisement(result);
                if (advertisement != null) advertisements.Add(advertisement);
            }

            model = new AdvertisementList();
            model.IsZTP = selectIsZTP;
            model.slRegions = slRegions;
            model.slWorkingFields = slWorkingFields;
            model.lAdvertisements = advertisements;

            return CurrentTemplate(model);
        }

        public ActionResult Index()
        {
            return View();
        }
        #endregion
    }
}
