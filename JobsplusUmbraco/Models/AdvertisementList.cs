using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebServices;
using umbraco.NodeFactory;
using Umbraco.Web.UI.Controls;
using System.Xml.XPath;
using Umbraco.Core.Models;
using System.Data;
using Examine;
using Examine.LuceneEngine.SearchCriteria;
using umbraco.cms.businesslogic.member;
using Umbraco.Core.Persistence;
using Umbraco.Web.Security;
using umbraco.MacroEngines;
using Examine.SearchCriteria;
using Jobsplus.Backoffice;
using Jobsplus.Backoffice.Models;

namespace JobsplusUmbraco.Models
{
    public class AdvertisementList : RenderModel
    {
        #region Properties
        public string fulltext { get; set; }
        public string workingField { get; set; }
        public string region { get; set; }
        public string typeOfWork { get; set;  }
        public bool IsTOP { get; set; }
        public List<Advertisement> lAdvertisements { get; set; }
        public IEnumerable<SelectListItem> slWorkingFields { get; set; }
        public IEnumerable<SelectListItem> slRegions { get; set; }
        public IEnumerable<SelectListItem> slTypeOfWork { get; set; } 
        public bool IsZTP { get; set; }

        UmbracoHelper umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
        MembershipHelper membershipHelper = new MembershipHelper(UmbracoContext.Current);

        public AdvertisementList() : 
            base(UmbracoContext.Current.PublishedContentRequest.PublishedContent) { }
        #endregion

        #region Method
        public List<Region> lRegions
        {
            get
            {
                List<Region> rCollection = new List<Region>();
                XPathNodeIterator iRegions = umbraco.library.GetPreValues(1139);
                if (iRegions.Count > 0 && iRegions.Current.HasChildren)
                {
                    iRegions.MoveNext();
                    XPathNodeIterator pvRegions = iRegions.Current.SelectChildren("preValue", "");
                    rCollection.Add(new Region { Name = "", Value = "" });
                    while (pvRegions.MoveNext())
                    {
                        rCollection.Add(new Region { Name = pvRegions.Current.Value, Value = pvRegions.Current.Value }); //pvRegions.Current.GetAttribute("id", "")
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
                    wfCollection.Add(new WorkingField { Name = "", Value = "" });
                    //wfCollection.Add(new WorkingField { Name = "Obor nebo pozice?", Value = "Obor nebo pozice?" });
                    while (pvWorkingFields.MoveNext())
                    {
                        wfCollection.Add(new WorkingField { Name = pvWorkingFields.Current.Value, Value = pvWorkingFields.Current.Value });
                    }
                }

                return wfCollection;
            }
        }

        public List<TypeOfWork> lTypeOfWork
        {
            get
            {
                List<TypeOfWork> wfCollection = new List<TypeOfWork>();
                XPathNodeIterator iTypeOfWorks = umbraco.library.GetPreValues(1146);
                if (iTypeOfWorks.Count > 0 && iTypeOfWorks.Current.HasChildren)
                {
                    iTypeOfWorks.MoveNext();
                    XPathNodeIterator pvTypeOfWorks = iTypeOfWorks.Current.SelectChildren("preValue", "");
                    wfCollection.Add(new TypeOfWork { Name = "", Value = "" });
                    while (pvTypeOfWorks.MoveNext())
                    {
                        wfCollection.Add(new TypeOfWork { Name = pvTypeOfWorks.Current.Value, Value = pvTypeOfWorks.Current.Value });
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

        public IEnumerable<SelectListItem> GetTypeOfWorkSelectListItem(string selectItem)
        {
            return from s in lTypeOfWork
                   select new SelectListItem
                   {
                       Text = s.Name,
                       Value = s.Value,
                       Selected = s.Value == selectItem
                   };
        }

        public void Fill()
        {
            /*
            var rootNode = new Node(-1);
            var allAdverts = rootNode.ChildrenAsList.Where(x => x.NodeTypeAlias.Equals("dtAdvertisement"));

            if (!String.IsNullOrEmpty(workingField))
                allAdverts = allAdverts.Where(x => x.GetProperty("aWorkingField").Equals(workingField));
            if (!String.IsNullOrEmpty(region))
                allAdverts = allAdverts.Where(x => x.GetProperty("aRegion").Equals(region));
            if (IsZTP)
                allAdverts = allAdverts.Where(x => x.GetProperty("aZtp").Equals("1"));

            var excludedDoctypes = new[] { "BlogPost", "NewsArticle" };
            DynamicNode node = Model; // your start node
            var childNodes = node.ChildrenAsList.Where(x => !excludedDoctypes.Contains(x.NodeTypeAlias));*/

            var searcher = ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"];
            //var searcher = ExamineManager.Instance.SearchProviderCollection["AdvertismentSearcher"];
            var criteria = searcher.CreateSearchCriteria(UmbracoExamine.IndexTypes.Content);
            Examine.SearchCriteria.IBooleanOperation filter = null;

            criteria.OrderBy(new string[] { "DateCreate" });
            filter = criteria.NodeTypeAlias("dtAdvertisement");

            #region Hledani v nazvu inzeratu
            // VYHLEDAVAC 1.0
            // DKO: puvodni vyhledavani - CASE SENSITIVE, pouze celá slova
            /*if (!String.IsNullOrEmpty(fulltext))
                filter.And().GroupedOr(new string[] { "nodeName" }, fulltext.Trim().ToLower()); */

            // VYHLEDAVAC 2.0
            // DKO: dalsi pokus o vyhledavani - CASE INSENSITIVE, pouze celá slova, bere v potaz i vyhledání uprostřed slov v názvu,
            // pokud uživatel zadá více jak jedno slovo, nefunguje :-(
            /*
            if (!String.IsNullOrEmpty(fulltext))
                filter.And().GroupedOr(new string[] { "nodeName" }, fulltext.Trim().ToLower().MultipleCharacterWildcard());*/

            // VYHLEDAVAC 3.0
            if (!String.IsNullOrWhiteSpace(fulltext))
            {
                var values = new List<Examine.SearchCriteria.IExamineValue>();
                var searchTerms = fulltext.Trim().ToLower().Split(' '); // rozdělení vyhledávaného výrazu na jednotlivé termíny
                foreach (var term in searchTerms) // přidání každého termínu zvlášť do skupiny vyhledáváných slov v názvu
                {
                    if (string.IsNullOrWhiteSpace(term)) continue;
                    values.Add(term.MultipleCharacterWildcard()); // MultipleCharacterWildcard zajistí relevantnější prohledávání - i uprostřed slov apod.
                }
                if (values.Count() > 0)
                {
                    filter.And().GroupedOr(new string[] { "nodeName" }, values.ToArray<IExamineValue>());
                }
            }
            #endregion
            if (!String.IsNullOrEmpty(workingField))
            {
                filter.And().Field("aWorkingField", workingField.MultipleCharacterWildcard());
            }
            if (!String.IsNullOrEmpty(region))
                filter.And().Field("aRegion", region.MultipleCharacterWildcard());
            if (!String.IsNullOrEmpty(typeOfWork))
                filter.And().Field("aTypeOfWork", typeOfWork.MultipleCharacterWildcard());
            if (IsZTP)
                filter.And().Field("aZtp", ("1").MultipleCharacterWildcard());

            //if (String.IsNullOrEmpty(fulltext)) fulltext = "Pozice?";
            if (String.IsNullOrEmpty(region)) region = "";
            if (String.IsNullOrEmpty(workingField)) workingField = "";
            if (String.IsNullOrEmpty(typeOfWork)) typeOfWork = "";
            
            slRegions = this.GetRegionSelectListItem(region);
            slWorkingFields = this.GetWorkingFieldSelectListItem(workingField);
            slTypeOfWork = this.GetTypeOfWorkSelectListItem(typeOfWork);

            List<Advertisement> advertisements = new List<Advertisement>();
            var searchResult = searcher.Search(filter.Compile());
            foreach (var result in searchResult)
            {
                //Advertisement advertisement = this.DynamicToAdverisement(result);
                Advertisement advertisement = new Advertisement();
                advertisement.DynamicToAdverisement(result);
                if (advertisement != null) advertisements.Add(advertisement);
            }
            lAdvertisements = advertisements;
             
        }

        public void Fill(IEnumerable<IPublishedContent> advertisementList)
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
                lAdvertisements = advertisements;
            }
            else
                lAdvertisements = new List<Advertisement>();  
        }
        #endregion

    }
}