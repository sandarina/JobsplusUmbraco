using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Web;
using Umbraco.Web.Controllers;
using Umbraco.Web.Models;
using ClientDependency.Core.Mvc;
using Umbraco.Web.Security;

namespace JobsplusUmbraco.Models
{
    public class Advertisement
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public bool TOP { get; set; }
        public string TypeOfWork { get; set; }
        public WorkingField WorkingField { get; set; }
        public string RequiredEducation { get; set; }
        public Region Region { get; set; }
        public string City { get; set; }
        public bool ZTP { get; set; }
        public string Content { get; set; }
        public string Advertiser { get; set; }
    }
}