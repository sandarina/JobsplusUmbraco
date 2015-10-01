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
//using umbraco.presentation.nodeFactory;

namespace JobsplusUmbraco.Controllers
{
    public class AdvertisementListController : SurfaceController //Controller
    {
        #region Properties
        UmbracoHelper umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
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
            var itemAdvertisement = (IPublishedContent)Umbraco.TypedContent(Convert.ToInt32(item["id"]));

            Advertisement advertisement = new Advertisement();
            advertisement.ID = itemAdvertisement.Id;
            advertisement.Name = itemAdvertisement.Name;
            advertisement.Url = itemAdvertisement.Url;
            advertisement.CreateDate = itemAdvertisement.CreateDate;
            advertisement.UpdateDate = itemAdvertisement.UpdateDate;

            advertisement.TOP = itemAdvertisement.GetPropertyValue<string>("topAdvertisement", "0") == "1" ? true : false;
            advertisement.TypeOfWork = itemAdvertisement.GetPropertyValue<string>("typeOfWork", string.Empty);
            advertisement.WorkingField = new WorkingField { Name = itemAdvertisement.GetPropertyValue<string>("workingField", string.Empty), Value = itemAdvertisement.GetPropertyValue<string>("workingField", string.Empty) };
            advertisement.RequiredEducation = itemAdvertisement.GetPropertyValue<string>("requiredEducation", string.Empty);
            advertisement.Region = new Region { Name = itemAdvertisement.GetPropertyValue<string>("region", string.Empty), Value = itemAdvertisement.GetPropertyValue<string>("region", string.Empty) };
            advertisement.City = itemAdvertisement.GetPropertyValue<string>("city", string.Empty);
            advertisement.ZTP = itemAdvertisement.GetPropertyValue<string>("ztp", "0") == "1" ? true : false;
            advertisement.ShortTextAdvertisement = itemAdvertisement.GetPropertyValue<string>("shortTextAdvertisement", string.Empty);
            advertisement.ContentAdvertisement = itemAdvertisement.GetPropertyValue<string>("contentAdvertisement", string.Empty);
            advertisement.Advertiser = itemAdvertisement.GetPropertyValue<int?>("advertiser").HasValue ? Members.GetById(itemAdvertisement.GetPropertyValue<int>("advertiser")).Name : string.Empty;

            return advertisement;
        }
        #endregion

        #region ActionResult

        public ActionResult Overview()
        {
            IEnumerable<SelectListItem> slRegions = GetRegionSelectListItem(string.Empty);
            IEnumerable<SelectListItem> slWorkingFields = GetWorkingFieldSelectListItem(string.Empty);

            List<Advertisement> advertisements = new List<Advertisement>();
            if (IsTOP)
            {
                var searcher = ExamineManager.Instance.SearchProviderCollection["InternalSearcher"];
                var criteria = searcher.CreateSearchCriteria(UmbracoExamine.IndexTypes.Content);
                Examine.SearchCriteria.IBooleanOperation filter = null;

                criteria.OrderByDescending(new string[] { "topAdvertisement" }).And().OrderBy(new string[] { "DateCreate" });
                filter = criteria.NodeTypeAlias("dtAdvertisement");

                foreach (var result in searcher.Search(filter.Compile()))
                {
                    Advertisement advertisement = DynamicToAdverisement(result);
                    advertisements.Add(advertisement);
                }
            }

            AdvertisementList model = new AdvertisementList();
            model.slRegions = slRegions;
            model.slWorkingFields = slWorkingFields;
            model.lAdvertisements = advertisements;

            return PartialView("AdvertisementList/Overview", model);
        }

        [HttpPost]
        public ActionResult Overview(AdvertisementList model)
        {
            string selectRegion = model.region;
            string selectWorkingField = model.workingField;
            
            IEnumerable<SelectListItem> slRegions = GetRegionSelectListItem(selectRegion);
            IEnumerable<SelectListItem> slWorkingFields = GetWorkingFieldSelectListItem(selectWorkingField);

            var searcher = ExamineManager.Instance.SearchProviderCollection["InternalSearcher"];
            var criteria = searcher.CreateSearchCriteria(UmbracoExamine.IndexTypes.Content);
            Examine.SearchCriteria.IBooleanOperation filter = null;

            criteria.OrderBy(new string[] { "DateCreate" });
            filter = criteria.NodeTypeAlias("dtAdvertisement");

            if (!String.IsNullOrEmpty(selectWorkingField))
                filter.And().Field("workingField", selectWorkingField);
            if (!String.IsNullOrEmpty(selectRegion))
                filter.And().Field("region", selectRegion);

            List<Advertisement> advertisements = new List<Advertisement>();
            foreach (var result in searcher.Search(filter.Compile()))
            {
                Advertisement advertisement = DynamicToAdverisement(result);
                advertisements.Add(advertisement);
            }

            model = new AdvertisementList();
            model.slRegions = slRegions;
            model.slWorkingFields = slWorkingFields;
            model.lAdvertisements = advertisements;

            return PartialView("AdvertisementList/Overview", model);
        }

        //
        // GET: /AdvertisementList/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /AdvertisementList/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /AdvertisementList/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AdvertisementList/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /AdvertisementList/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /AdvertisementList/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /AdvertisementList/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /AdvertisementList/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion
    }
}
