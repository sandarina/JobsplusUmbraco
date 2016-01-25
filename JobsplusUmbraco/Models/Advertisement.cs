using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Web;
using Umbraco.Web.Controllers;
using Umbraco.Web.Models;
using ClientDependency.Core.Mvc;
using Umbraco.Web.Security;
using System.Web.Mvc;

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
        public TypeOfWork TypeOfWork { get; set; }
        public WorkingField WorkingField { get; set; }
        public RequiredEducation RequiredEducation { get; set; }
        public Region Region { get; set; }
        public string City { get; set; }
        public bool ZTP { get; set; }
        public string Content { get; set; }
        public string Advertiser { get; set; }
        public string Company { get; set; }
        public string CompanyUrl { get; set; }
        public string CompanyLogo { get; set; }

        /// <summary>
        /// Šablona "Pravomoci a povinnosti", web "Vaší náplní práce bude"
        /// </summary>
        public string JobDescription { get; set; }

        /// <summary>
        /// Šablona "Kvalifikační požadavky", web "Požadujeme"
        /// </summary>
        public string JobRequirements { get; set; }

        /// <summary>
        /// Šablona "Firemní benefity", web "Nabízíme"
        /// </summary>
        public string JobOfferings { get; set; }

        /// <summary>
        /// SelectList šablon
        /// </summary>
        public IEnumerable<SelectListItem> slJobTemplate { get; set; }

        /// <summary>
        /// Vybraná položka SelectListu
        /// </summary>
        public int JobTemplateID { get; set; }

        /// <summary>
        /// SelectList požadovaných vztahů
        /// </summary>
        public IEnumerable<SelectListItem> slTypeOfWork { get; set; }

        /// <summary>
        /// Vybraná položka SelectListu
        /// </summary>
        public int TypeOfWorkID { get; set; }

        /// <summary>
        /// SelectList oboru
        /// </summary>
        public IEnumerable<SelectListItem> slWorkingField { get; set; }

        /// <summary>
        /// Vybraná položka SelectListu
        /// </summary>
        public int WorkingFieldID { get; set; }

        /// <summary>
        /// SelectList dosaženého vzdělání
        /// </summary>
        public IEnumerable<SelectListItem> slRequiredEducation { get; set; }

        /// <summary>
        /// Vybraná položka SelectListu
        /// </summary>
        public int RequiredEducationID { get; set; }

        /// <summary>
        /// SelectList krajů
        /// </summary>
        public IEnumerable<SelectListItem> slRegion { get; set; }

        /// <summary>
        /// Vybraná položka SelectListu
        /// </summary>
        public int RegionID { get; set; }
    }
}