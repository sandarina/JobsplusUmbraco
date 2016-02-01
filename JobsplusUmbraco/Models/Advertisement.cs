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
using Umbraco.Core.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JobsplusUmbraco.Models
{
    public class Advertisement
    {
        #region Properties
        public int ID { get; set; }

        [DisplayName("Název")]
        [Required(ErrorMessage = "Zadejte název inzerátu.")]
        public string Name { get; set; }
        
        public string Url { get; set; }
        
        public DateTime CreateDate { get; set; }
        
        public DateTime UpdateDate { get; set; }

        [DisplayName("TOP")]
        public bool TOP { get; set; }

        [DisplayName("Požadované vztahy")]
        public TypeOfWork TypeOfWork { get; set; }

        [DisplayName("Obor")]
        public WorkingField WorkingField { get; set; }

        [DisplayName("Dosažené vzdělání")]
        public RequiredEducation RequiredEducation { get; set; }

        [DisplayName("Kraj")]
        public Region Region { get; set; }

        [DisplayName("Město")]
        public string City { get; set; }

        [DisplayName("ZTP")]
        public bool ZTP { get; set; }

        [DisplayName("Další informace")]
        public string Content { get; set; }
        
        public string Advertiser { get; set; }
        
        public string Company { get; set; }
        public string CompanyUrl { get; set; }
        public string CompanyLogo { get; set; }

        /// <summary>
        /// Šablona "Pravomoci a povinnosti", web "Vaší náplní práce bude"
        /// </summary>
        [DisplayName("Pravomoci a povinnosti")]
        [Required(ErrorMessage = "Zadejte pravomoci a povinnosti.")]
        public string JobDescription { get; set; }

        /// <summary>
        /// Šablona "Kvalifikační požadavky", web "Požadujeme"
        /// </summary>
        [DisplayName("Kvalifikační požadavky")]
        [Required(ErrorMessage = "Zadejte kvalifikační požadavky.")]
        public string JobRequirements { get; set; }

        /// <summary>
        /// Šablona "Firemní benefity", web "Nabízíme"
        /// </summary>
        [DisplayName("Firemní benefity")]
        [Required(ErrorMessage = "Zadejte firemní benefity.")]
        public string JobOfferings { get; set; }

        /// <summary>
        /// Vybraná položka SelectListu
        /// </summary>
        public int JobTemplateID { get; set; }

        /// <summary>
        /// Vybraná položka SelectListu
        /// </summary>
        [DisplayName("Požadované vztahy")]
        [Required(ErrorMessage = "Zadejte požadované vztahy.")]
        public int TypeOfWorkID { get; set; }

        /// <summary>
        /// Vybraná položka SelectListu
        /// </summary>
        [DisplayName("Obor")]
        [Required(ErrorMessage = "Zadejte obor.")]
        public int WorkingFieldID { get; set; }

        /// <summary>
        /// Vybraná položka SelectListu
        /// </summary>
        [DisplayName("Dosažené vzdělání")]
        [Required(ErrorMessage = "Zadejte dosažené vzdělání.")]
        public int RequiredEducationID { get; set; }

        /// <summary>
        /// Vybraná položka SelectListu
        /// </summary>
        [DisplayName("Kraj")]
        [Required(ErrorMessage = "Zadejte kraj.")]
        public int RegionID { get; set; }
        #endregion

        #region Custom properties
        UmbracoHelper umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
        MembershipHelper membershipHelper = new MembershipHelper(UmbracoContext.Current);
        #endregion


        #region Method
        public void DynamicToAdverisement(int Id)
        {
            var itemAdvertisement = (IPublishedContent)umbracoHelper.TypedContent(Convert.ToInt32(Id));

            if (itemAdvertisement != null)
            {
                this.ID = itemAdvertisement.Id;
                this.Name = itemAdvertisement.Name;
                this.Url = itemAdvertisement.Url;
                this.CreateDate = itemAdvertisement.CreateDate;
                this.UpdateDate = itemAdvertisement.UpdateDate;
                this.Company = itemAdvertisement.Parent.Parent.Name;
                this.CompanyUrl = itemAdvertisement.Parent.Parent.Url;
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
                    this.CompanyLogo = mediaLogo.umbracoFile;
                }

                this.TOP = itemAdvertisement.GetPropertyValue<string>("aTop", "0") == "1" ? true : false;
                this.TypeOfWork = new TypeOfWork { Name = itemAdvertisement.GetPropertyValue<string>("aTypeOfWork", string.Empty), Value = itemAdvertisement.GetPropertyValue<string>("aTypeOfWork", string.Empty) };
                this.WorkingField = new WorkingField { Name = itemAdvertisement.GetPropertyValue<string>("aWorkingField", string.Empty), Value = itemAdvertisement.GetPropertyValue<string>("aWorkingField", string.Empty) };
                this.RequiredEducation = new RequiredEducation { Name = itemAdvertisement.GetPropertyValue<string>("aRequiredEducation", string.Empty), Value = itemAdvertisement.GetPropertyValue<string>("aRequiredEducation", string.Empty) };
                this.Region = new Region { Name = itemAdvertisement.GetPropertyValue<string>("aRegion", string.Empty), Value = itemAdvertisement.GetPropertyValue<string>("aRegion", string.Empty) };
                this.City = itemAdvertisement.GetPropertyValue<string>("aCity", string.Empty);
                this.ZTP = itemAdvertisement.GetPropertyValue<string>("aZtp", "0") == "1" ? true : false;
                this.Content = itemAdvertisement.GetPropertyValue<string>("aContent", string.Empty);
                //advertisement.Advertiser = itemAdvertisement.GetPropertyValue<int?>("advertiser").HasValue ? Members.GetById(itemAdvertisement.GetPropertyValue<int>("advertiser")).Name : string.Empty;
                this.Advertiser = itemAdvertisement.GetPropertyValue<int?>("aAdvertiser").HasValue ? membershipHelper.GetById(itemAdvertisement.GetPropertyValue<int>("aAdvertiser")).Name : string.Empty;
                this.JobDescription = itemAdvertisement.GetPropertyValue<string>("aJobDescription", string.Empty);
                this.JobOfferings = itemAdvertisement.GetPropertyValue<string>("aJobOfferings", string.Empty);
                this.JobRequirements = itemAdvertisement.GetPropertyValue<string>("aJobRequirements", string.Empty);
            }
        }

        public void DynamicToAdverisement(dynamic item)
        {
            var itemAdvertisement = (IPublishedContent)umbracoHelper.TypedContent(Convert.ToInt32(item["id"]));

            if (itemAdvertisement != null)
            {
                this.ID = itemAdvertisement.Id;
                this.Name = itemAdvertisement.Name;
                this.Url = itemAdvertisement.Url;
                this.CreateDate = itemAdvertisement.CreateDate;
                this.UpdateDate = itemAdvertisement.UpdateDate;
                this.Company = itemAdvertisement.Parent.Parent.Name;
                this.CompanyUrl = itemAdvertisement.Parent.Parent.Url;
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
                    this.CompanyLogo = mediaLogo.umbracoFile;
                }

                this.TOP = itemAdvertisement.GetPropertyValue<string>("aTop", "0") == "1" ? true : false;
                this.TypeOfWork = new TypeOfWork { Name = itemAdvertisement.GetPropertyValue<string>("aTypeOfWork", string.Empty), Value = itemAdvertisement.GetPropertyValue<string>("aTypeOfWork", string.Empty) };
                this.WorkingField = new WorkingField { Name = itemAdvertisement.GetPropertyValue<string>("aWorkingField", string.Empty), Value = itemAdvertisement.GetPropertyValue<string>("aWorkingField", string.Empty) };
                this.RequiredEducation = new RequiredEducation { Name = itemAdvertisement.GetPropertyValue<string>("aRequiredEducation", string.Empty), Value = itemAdvertisement.GetPropertyValue<string>("aRequiredEducation", string.Empty) };
                this.Region = new Region { Name = itemAdvertisement.GetPropertyValue<string>("aRegion", string.Empty), Value = itemAdvertisement.GetPropertyValue<string>("aRegion", string.Empty) };
                this.City = itemAdvertisement.GetPropertyValue<string>("aCity", string.Empty);
                this.ZTP = bool.Parse(itemAdvertisement.GetPropertyValue<string>("aZtp", "0"));
                this.Content = itemAdvertisement.GetPropertyValue<string>("aContent", string.Empty);
                //this.Advertiser = itemAdvertisement.GetPropertyValue<int?>("advertiser").HasValue ? Members.GetById(itemAdvertisement.GetPropertyValue<int>("advertiser")).Name : string.Empty;
                this.Advertiser = itemAdvertisement.GetPropertyValue<int?>("Aadvertiser").HasValue ? membershipHelper.GetById(itemAdvertisement.GetPropertyValue<int>("aAdvertiser")).Name : string.Empty;
                this.JobDescription = itemAdvertisement.GetPropertyValue<string>("aJobDescription", string.Empty);
                this.JobOfferings = itemAdvertisement.GetPropertyValue<string>("aJobOfferings", string.Empty);
                this.JobRequirements = itemAdvertisement.GetPropertyValue<string>("aJobRequirements", string.Empty);
            }
        }
        #endregion
    }
}