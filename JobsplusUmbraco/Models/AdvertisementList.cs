﻿using System;
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
using umbraco.cms.businesslogic.member;
using Umbraco.Web.Security;
using umbraco.MacroEngines;

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


        public Advertisement DynamicToAdverisement(int Id)
        {
            var itemAdvertisement = (IPublishedContent)umbracoHelper.TypedContent(Convert.ToInt32(Id));

            if (itemAdvertisement == null)
                return null;

            Advertisement advertisement = new Advertisement();
            advertisement.ID = itemAdvertisement.Id;
            advertisement.Name = itemAdvertisement.Name;
            advertisement.Url = itemAdvertisement.Url;
            advertisement.CreateDate = itemAdvertisement.CreateDate;
            advertisement.UpdateDate = itemAdvertisement.UpdateDate;
            advertisement.Company = itemAdvertisement.Parent.Parent.Name;
            advertisement.CompanyUrl = itemAdvertisement.Parent.Parent.Url;
            dynamic mediaLogo;
            try
            {
                mediaLogo = umbracoHelper.Media(itemAdvertisement.Parent.GetPropertyValue<int>("cLogo"));
            }
            catch
            {
                mediaLogo = null;
            }
            if (mediaLogo != null)
            {
                advertisement.CompanyLogo = mediaLogo.umbracoFile;
            }
            //advertisement.CompanyLogo =

            advertisement.TOP = itemAdvertisement.GetPropertyValue<string>("aTop", "0") == "1" ? true : false;
            advertisement.TypeOfWork = new TypeOfWork { Name = itemAdvertisement.GetPropertyValue<string>("aTypeOfWork", string.Empty), Value = itemAdvertisement.GetPropertyValue<string>("aTypeOfWork", string.Empty) };
            advertisement.WorkingField = new WorkingField { Name = itemAdvertisement.GetPropertyValue<string>("aWorkingField", string.Empty), Value = itemAdvertisement.GetPropertyValue<string>("aWorkingField", string.Empty) };
            advertisement.RequiredEducation = new RequiredEducation { Name = itemAdvertisement.GetPropertyValue<string>("aRequiredEducation", string.Empty), Value = itemAdvertisement.GetPropertyValue<string>("aRequiredEducation", string.Empty) };
            advertisement.Region = new Region { Name = itemAdvertisement.GetPropertyValue<string>("aRegion", string.Empty), Value = itemAdvertisement.GetPropertyValue<string>("aRegion", string.Empty) };
            advertisement.City = itemAdvertisement.GetPropertyValue<string>("aCity", string.Empty);
            advertisement.ZTP = itemAdvertisement.GetPropertyValue<string>("aZtp", "0") == "1" ? true : false;
            advertisement.Content = itemAdvertisement.GetPropertyValue<string>("aContent", string.Empty);
            //advertisement.Advertiser = itemAdvertisement.GetPropertyValue<int?>("advertiser").HasValue ? Members.GetById(itemAdvertisement.GetPropertyValue<int>("advertiser")).Name : string.Empty;
            advertisement.Advertiser = itemAdvertisement.GetPropertyValue<int?>("Aadvertiser").HasValue ? membershipHelper.GetById(itemAdvertisement.GetPropertyValue<int>("aAdvertiser")).Name : string.Empty;

            return advertisement;
        }

        public Advertisement DynamicToAdverisement(dynamic item)
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
            advertisement.Company = itemAdvertisement.Parent.Parent.Name;
            advertisement.CompanyUrl = itemAdvertisement.Parent.Parent.Url;
            dynamic mediaLogo;            
            try
            {
                mediaLogo = umbracoHelper.Media(itemAdvertisement.Parent.GetPropertyValue<int>("cLogo"));
            }
            catch
            {
                mediaLogo = null;
            }
            if (mediaLogo != null)
            {
                advertisement.CompanyLogo = mediaLogo.umbracoFile;
            }
            //advertisement.CompanyLogo =

            advertisement.TOP = itemAdvertisement.GetPropertyValue<string>("aTop", "0") == "1" ? true : false;
            advertisement.TypeOfWork = new TypeOfWork { Name = itemAdvertisement.GetPropertyValue<string>("aTypeOfWork", string.Empty), Value = itemAdvertisement.GetPropertyValue<string>("aTypeOfWork", string.Empty) };
            advertisement.WorkingField = new WorkingField { Name = itemAdvertisement.GetPropertyValue<string>("aWorkingField", string.Empty), Value = itemAdvertisement.GetPropertyValue<string>("aWorkingField", string.Empty) };
            advertisement.RequiredEducation = new RequiredEducation { Name = itemAdvertisement.GetPropertyValue<string>("aRequiredEducation", string.Empty), Value = itemAdvertisement.GetPropertyValue<string>("aRequiredEducation", string.Empty) };
            advertisement.Region = new Region { Name = itemAdvertisement.GetPropertyValue<string>("aRegion", string.Empty), Value = itemAdvertisement.GetPropertyValue<string>("aRegion", string.Empty) };
            advertisement.City = itemAdvertisement.GetPropertyValue<string>("aCity", string.Empty);
            advertisement.ZTP = bool.Parse(itemAdvertisement.GetPropertyValue<string>("aZtp", "0"));
            advertisement.Content = itemAdvertisement.GetPropertyValue<string>("aContent", string.Empty);
            //advertisement.Advertiser = itemAdvertisement.GetPropertyValue<int?>("advertiser").HasValue ? Members.GetById(itemAdvertisement.GetPropertyValue<int>("advertiser")).Name : string.Empty;
            advertisement.Advertiser = itemAdvertisement.GetPropertyValue<int?>("Aadvertiser").HasValue ? membershipHelper.GetById(itemAdvertisement.GetPropertyValue<int>("aAdvertiser")).Name : string.Empty;

            return advertisement;
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


            var searcher = ExamineManager.Instance.SearchProviderCollection["InternalSearcher"];
            var criteria = searcher.CreateSearchCriteria(UmbracoExamine.IndexTypes.Content);
            Examine.SearchCriteria.IBooleanOperation filter = null;

            criteria.OrderBy(new string[] { "DateCreate" });
            filter = criteria.NodeTypeAlias("dtAdvertisement");

            if (!String.IsNullOrEmpty(fulltext))
                filter.And().GroupedOr(new string[] { "nodeName" }, fulltext);
            if (!String.IsNullOrEmpty(workingField)) 
                filter.And().Field("aWorkingField", workingField);
            if (!String.IsNullOrEmpty(region))
                filter.And().Field("aRegion", region);
            if (!String.IsNullOrEmpty(typeOfWork))
                filter.And().Field("aTypeOfWork", typeOfWork);
            if (IsZTP)
                filter.And().Field("aZtp", "1");

            if (String.IsNullOrEmpty(fulltext)) region = "Pracovní pozice?";
            if (String.IsNullOrEmpty(region)) region = "Kde?";
            if (String.IsNullOrEmpty(workingField)) workingField = "Obor?";
            slRegions = this.GetRegionSelectListItem(region);
            slWorkingFields = this.GetWorkingFieldSelectListItem(workingField);
            slTypeOfWork = this.GetTypeOfWorkSelectListItem(typeOfWork);

            List<Advertisement> advertisements = new List<Advertisement>();
            var searchResult = searcher.Search(filter.Compile());
            //searchResult = searchResult.Select(r => r.Fields["nodeName"].ContainsInsensitive(fulltext));
            foreach (var result in searchResult)
            {
                Advertisement advertisement = this.DynamicToAdverisement(result);
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
                    Advertisement advertisement = this.DynamicToAdverisement(result.Id);
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