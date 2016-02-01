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

        private DBContextController DBContext = new DBContextController();

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
}
