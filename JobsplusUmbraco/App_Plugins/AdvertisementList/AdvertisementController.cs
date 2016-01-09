using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace JobsplusUmbraco.App_Plugins.AdvertisementList
{
    public class AdvertisementController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Results(string workingField, string region)
        {
            var model = new AdvertisementModel();
            model.WorkingField = workingField;
            model.Region = region;
            return PartialView("Views/Partials/pvGenerateAdvertisementList", model);
        }
        /*protected  ViewResult View(AdvertisementModel model)
        {
            return View(null, model);
        }

        protected ViewResult View(string view, AdvertisementModel model)
        {
            return base.View(model);
        }*/
        /*// GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }*/
    }
}