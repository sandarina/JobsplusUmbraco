using Jobsplus.Backoffice.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Jobsplus.Backoffice.Models
{
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
        public int? Id { get; set; }

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

        private DBContextController DBContext = new DBContextController();

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
}
