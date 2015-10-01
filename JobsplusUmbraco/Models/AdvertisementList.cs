using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobsplusUmbraco.Models
{
    public class AdvertisementList
    {
        public string workingField { get; set; }
        public string region { get; set; }
        public bool IsTOP { get; set; }
        public List<Advertisement> lAdvertisements{ get; set; }
        public IEnumerable<SelectListItem> slWorkingFields { get; set; }
        public IEnumerable<SelectListItem> slRegions { get; set; }
    }
}