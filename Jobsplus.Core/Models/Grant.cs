using Jobsplus.Backoffice.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using umbraco.businesslogic;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Umbraco.Web.Models;

namespace Jobsplus.Backoffice.Models
{
    /// <summary>
    /// Kraje
    /// </summary>
    [TableName("JobsplusRegions")]
    [PrimaryKey("Id", autoIncrement = true)]
    public class Region
    {
        #region Ctor
        public Region() { }
        #endregion

        #region Properties
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        /// <summary>
        /// Název
        /// </summary>
        public string Name { get; set; }
        #endregion

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// Okres
    /// </summary>
    [TableName("JobsplusDistricts")]
    [PrimaryKey("Id", autoIncrement = true)]
    public class District
    {
        #region Ctor
        public District() { }
        #endregion

        #region Properties
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        /// <summary>
        /// Název
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Zkratka
        /// </summary>        
        public string Shortcut { get; set; }

        /// <summary>
        /// Kraj
        /// </summary>
        [ForeignKey(typeof(Region))]
        public int RegionId { get; set; }
        #endregion

        public override string ToString()
        {
            return Name;
        }
    }

    #region Grants
    
    /// <summary>
    /// Dotační program.
    /// </summary>
    [TableName("JobsplusGrants")]
    [PrimaryKey("Id", autoIncrement = true)]
    public class Grant
    {
        #region Ctor
        public Grant() { }
        #endregion

        #region Properties
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        /// <summary>
        /// Název
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Popis
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Kraj
        /// </summary>
        [ForeignKey(typeof(Region))]
        public int RegionId { get; set; }
        #endregion

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// Definice osoby - dotatačního programu.
    /// </summary>
    [TableName("JobsplusGrantDefinitions")]
    [PrimaryKey("Id", autoIncrement = true)]
    public class GrantDefinition
    {
        #region Ctor
        public GrantDefinition() { }
        #endregion

        #region Properties
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        /// <summary>
        /// Id dotačního programu
        /// </summary>
        [ForeignKey(typeof(Grant))]
        public int GrantId { get; set; }

        /// <summary>
        /// Název
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Rozmezí věku od (včetně)
        /// </summary>
        public int AgeFrom { get; set; }

        /// <summary>
        /// Rozmezí věku do (včetně)
        /// </summary>
        public int AgeTo { get; set; }

        /// <summary>
        /// Délka evidence (v měsících)
        /// </summary>
        public int EvidenceMonths { get; set; }

        /// <summary>
        /// Typ smlouvy (určitá, neurčitá, aj.)
        /// </summary>
        public string ContractType { get; set; }

        /// <summary>
        /// Délka dotace (v měsících)
        /// </summary>
        public int GrantMonths { get; set; }

        /// <summary>
        /// Dotace v CZK
        /// </summary>
        public int GrantValue { get; set; }
        #endregion

        public override string ToString()
        {
            return Name;
        }
    }

    [TableName("JobsplusGrantsGrantDefinitions")]
    public class GrantGrantDefinition
    {         
        [ForeignKey(typeof(Grant))]
        public int GrantId { get; set; }
        
        [ForeignKey(typeof(GrantDefinition))]
        public int GrantDefinitionId { get; set; }
    }

    [TableName("JobsplusGrantDefEmployDeparts")]
    public class GrantDefEmployDeparts
    {
        [ForeignKey(typeof(GrantDefinition))]
        public int GrantDefinitionId { get; set; }

        [ForeignKey(typeof(EmployDepartment))]
        public int EmployDepartmentId { get; set; }
    }
    #endregion



    /// <summary>
    /// Úřad práce
    /// </summary>
    [TableName("JobsplusEmployDepartments")]
    [PrimaryKey("Id", autoIncrement = true)]
    public class EmployDepartment
    {
        #region Ctor
        public EmployDepartment() { }
        #endregion

        #region Properties
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        /// <summary>
        /// Název
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Okres
        /// </summary>
        [ForeignKey(typeof(Region))]
        public int RegionId { get; set; }

        /// <summary>
        /// Okres
        /// </summary>
        [ForeignKey(typeof(District))]
        public int DistrictId { get; set; }
        #endregion

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// Obor
    /// </summary>
    [TableName("JobsplusSpecializations")]
    [PrimaryKey("Id", autoIncrement = true)]
    public class Specialization
    {
        #region Ctor
        public Specialization() { }
        #endregion

        #region Properties
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        /// <summary>
        /// Název
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Pořadí
        /// </summary>
        public int Order { get; set; }
        #endregion

        public DBContextController DBContext = new DBContextController();

        #region Method
        public override string ToString()
        {
            return Name;
        }

        public Specialization Save()
        {
            return DBContext.PostSaveSpecialization(this);
        }
        #endregion
    }

    /// <summary>
    /// Pracovní pozice
    /// </summary>
    [TableName("JobsplusJobs")]
    [PrimaryKey("Id", autoIncrement = true)]
    public class Job
    {
        #region Ctor
        public Job() { }
        #endregion

        #region Properties
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        /// <summary>
        /// Název
        /// </summary>
        [DisplayName("Název")]
        [Required(ErrorMessage = "Zadejte název pracovní pozice.")]
        public string Name { get; set; }

        /// <summary>
        /// Obor
        /// </summary>
        [ForeignKey(typeof(Specialization))]
        [Required(ErrorMessage = "Zadejte obor.")]
        public int SpecializationId { get; set; }
        #endregion

        public DBContextController DBContext = new DBContextController();

        #region Method
        public override string ToString()
        {
            return Name;
        }

        public Job Save()
        {
            return DBContext.PostSaveJob(this);
        }

        /*public Specialization Specialization
        {
            get { return DBContext.GetSpecializationById(this.SpecializationId); }
        }

        public string SpecializationName
        {
            get { return Specialization != null ? Specialization.Name : String.Empty; }
        }*/
        #endregion
    }

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
        public int Id { get; set; }

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

        public DBContextController DBContext = new DBContextController();

        /// <summary>
        /// SelectList pracovních pozic
        /// </summary>
        public IEnumerable<SelectListItem> slJob { get; set; }

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
