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
using System.Data.SqlClient;
using System.Configuration;
using umbraco.cms.businesslogic.web;


namespace JobsplusUmbraco.Controllers
{
    public class HomeController : RenderMvcController
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

        public static SqlConnection CreateSQLConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ConnectionString;
            return new SqlConnection(connectionString);
        }

        protected void CheckExpiredTop()
        {
            using (SqlConnection conn = CreateSQLConnection())
            {
                SqlCommand dbCommand = conn.CreateCommand();
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.CommandText = "pUpdateExpiredPropertyData";
                conn.Open();
                dbCommand.ExecuteNonQuery();
                conn.Close();
            }
        }
        #endregion

        #region ActionResult
        public ActionResult tHome()
        {
            CheckExpiredTop();

            /*var contentService = Services.ContentService;
            var produkt = contentService.CreateContent("Nový inzerát", 1446, "dtAdvertisement");
            produkt.SetValue("aContent", "obsahuje inzerát");
            produkt.SetValue("aTypeOfWork", "55");
            produkt.SetValue("aRegion", "240");
            produkt.SetValue("aWorkingField", "133");
            produkt.SetValue("aRequiredEducation", "242");
            var status = contentService.SaveAndPublishWithStatus(produkt);*/

            AdvertisementList model = new AdvertisementList();
            model.Fill();

            return CurrentTemplate(model);
        }

        [HttpPost]
        public ActionResult tHome(AdvertisementList model)
        {
            model.Fill();

            if (TempData["AdvertisementListModel"] == null)
                TempData.Add("AdvertisementListModel", model);
            else
                TempData["AdvertisementListModel"] = model;

            return PartialView("tFindJob", model);
            //return RedirectToAction("tFindJob", "FindJob");
            //return Redirect("/najit-praci");
            //return CurrentTemplate(model);
        }

        public ActionResult Index()
        {
            return View();
        }
        #endregion
    }
}
