using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using umbraco.businesslogic;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Jobsplus.Backoffice.Models
{
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
        public Region RegionId { get; set; }
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
        public Grant GrantId { get; set; }

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
        public Grant GrantId { get; set; }

        public GrantDefinition GrantDefinitionId { get; set; }
    }
    #endregion

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
        /// Kraj
        /// </summary>
        public int RegionId { get; set; }
        #endregion

        public override string ToString()
        {
            return Name;
        }
    }

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

        public override string ToString()
        {
            return Name;
        }
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
        public string Name { get; set; }

        /// <summary>
        /// Obor
        /// </summary>
        public Specialization SpecializationId { get; set; }
        #endregion

        public override string ToString()
        {
            return Name;
        }
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
        public Job JobId { get; set; }

        /// <summary>
        /// Název
        /// </summary>
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
        public int[] VisibleForCompanyIds { get; set; }

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
        public string JobDescription { get; set; }

        /// <summary>
        /// Šablona "Kvalifikační požadavky", web "Požadujeme"
        /// </summary>
        public string JobRequirements { get; set; }

        /// <summary>
        /// Šablona "Firemní benefity", web "Nabízíme"
        /// </summary>
        public string JobOfferings { get; set; }
	    #endregion
        #endregion

        public override string ToString()
        {
            return Name;
        }
    }
}
