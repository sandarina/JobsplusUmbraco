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

        /// <summary>
        /// Poznámka
        /// </summary>
        public string Note { get; set; }
        #endregion

        public override string ToString()
        {
            return Name;
        }
    }

    /* DKO: 5.2.2016  tato tabulka nejspis nebude potreba
    [TableName("JobsplusGrantsGrantDefinitions")]
    public class GrantGrantDefinition
    {         
        [ForeignKey(typeof(Grant))]
        public int GrantId { get; set; }
        
        [ForeignKey(typeof(GrantDefinition))]
        public int GrantDefinitionId { get; set; }
    }*/

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
        /// Kraj
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
}
