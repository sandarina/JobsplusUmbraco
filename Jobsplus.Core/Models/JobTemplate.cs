using Jobsplus.Backoffice.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Jobsplus.Backoffice.Models
{
    /// <summary>
    /// Šablona pracovní pozice
    /// </summary>
    [TableName("JobsplusJobTemplates")]
    [PrimaryKey("Id", autoIncrement = true)]
    public class JobTemplate
    {
        #region Ctor
        public JobTemplate() { }
        #endregion

        #region Properties
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int? Id { get; set; }

        /// <summary>
        /// Pracovní pozice
        /// </summary>
        [ForeignKey(typeof(Job))]
        [DisplayName("Pracovní pozice")]
        [Required(ErrorMessage = "Zadejte pracovní pozici.")]
        public int JobId { get; set; }

        /// <summary>
        /// Název
        /// </summary>
        [DisplayName("Název")]
        [Required(ErrorMessage = "Zadejte název šablony.")]
        public string Name { get; set; }

        /// <summary>
        /// Jedná se o obecnou šablonu pracovní pozice - tzn. lze k ní stáhnout Word/PDF formulář pracovní pozice
        /// </summary>
        public bool IsGeneralTemplate { get; set; }

        /// <summary>
        /// Viditelná pro všechny firmy?
        /// </summary>
        public bool IsVisibleForAll { get; set; }

        /// <summary>
        /// Viditelná pro všechny firmy - identifikátory (NodeId) firem, pro něž je šablona viditelná
        /// </summary>
        public string VisibleForCompanyIds { get; set; }

        /// <summary>
        /// Relativní URL formuláře obecné pracovní pozice ke stažení (Word/PDF).
        /// </summary>
        public string TemplateUrl { get; set; }

        #region Template Form Properties
        /// <summary>
        /// Název pracovní pozice
        /// </summary>
        public string JobName { get; set; }

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
        #endregion
        #endregion

        private DBContextController DBContext = new DBContextController();

        #region Method
        public override string ToString()
        {
            return Name;
        }

        public JobTemplate Save()
        {
            return DBContext.PostSaveJobTemplate(this);
        }

        /*public Job Job
        {
            get { return DBContext.GetJobById(this.JobId); }
        }*/

        public List<int> GetForCompanyIds()
        {
            if (!String.IsNullOrEmpty(this.VisibleForCompanyIds))
            {
                var sIds = this.VisibleForCompanyIds.Split(new char[] { ',' });
                return sIds.Select(id => Convert.ToInt32(id)).ToList();
            }
            else
                return new List<int>();
        }
        #endregion
    }
}
