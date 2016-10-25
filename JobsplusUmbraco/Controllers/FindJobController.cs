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
        #endregion

        #region ActionResult
        public ActionResult tFindJob()
        {
            if (TempData["AdvertisementListModel"] != null) tFindJob((AdvertisementList)TempData["AdvertisementListModel"]);

            AdvertisementList model = new AdvertisementList();
            model.Fill();
            return CurrentTemplate(model);
        }  

        [HttpPost]
        public ActionResult tFindJob(AdvertisementList model)
        {
            model.Fill();
            return CurrentTemplate(model);
        }

        public ActionResult tFindBrigade()
        {
            if (TempData["AdvertisementListModel"] != null) tFindBrigade((AdvertisementList)TempData["AdvertisementListModel"]);

            AdvertisementList model = new AdvertisementList();
            model.FillBrigades();
            return CurrentTemplate(model);
        }

        [HttpPost]
        public ActionResult tFindBrigade(AdvertisementList model)
        {
            model.FillBrigades();
            return CurrentTemplate(model);
        }   

        public ActionResult Index()
        {
            return View();
        }
        #endregion
    }
}
