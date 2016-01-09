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
    /// <summary>
    /// Dotační program.
    /// </summary>
    [TableName("JobsplusGrants")]
    public class Grant
    {
        #region Ctor
        public Grant() { }
        #endregion

        #region Properties
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Region { get; set; }
        #endregion

        public override string ToString()
        {
            return Name + " - " + Region;
        }
    }
}
