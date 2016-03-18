using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace JobsplusUmbraco.Models
{
    public class MarketingActual
    {
        #region Properties
        [DisplayName("ID")]
        public int? ID { get; set; }

        [DisplayName("Datum vydání")]
        [Required(ErrorMessage = "Zadejte datum vydání marketingové aktuality.")]
        public string Date { get; set; }

        [DisplayName("Název")]
        [Required(ErrorMessage = "Zadejte název marketingové aktuality.")]
        public string Name { get; set; }

        [DisplayName("Náhledový obrázek")]
        [Required(ErrorMessage = "Nahrajte náhledový obrázek")]
        public ImageCropDataSet Thumbnail { get; set; }

        [DisplayName("Popis")]
        [Required(ErrorMessage = "Zadejte popis marketingové aktuality.")]
        public string Description { get; set; }

        [DisplayName("Obsah")]
        [Required(ErrorMessage = "Zadejte obsah marketingové aktuality.")]
        public HtmlString Content { get; set; }

        [DisplayName("URL odkaz")]
        public string Url { get; set; }

        [DisplayName("Zveřejněno?")]
        public bool IsPublished { get; set; }
        #endregion
    }
}