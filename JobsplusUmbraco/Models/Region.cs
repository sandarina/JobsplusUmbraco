using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.XPath;
using Umbraco.Web.Models;

namespace JobsplusUmbraco.Models
{
    public class Region
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}