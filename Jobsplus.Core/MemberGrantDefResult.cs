using Jobsplus.Backoffice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobsplus.Backoffice
{
    /// <summary>
    /// Výsledek nároku na dotace zájemce o práci. Jedná se o pomocnou třídu!
    /// </summary>
    public class MemberGrantDefResult
    {
        #region Ctor
        public MemberGrantDefResult() { }
        #endregion

        #region Properties
        /// <summary>
        /// Id zájemnce o práci - MemberId v systému Umbraco.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Telefon zájemnce o práci - Member v systému Umbraco.
        /// </summary>
        public string Phone { get; set; }


        #region Check data and message
        /// <summary>
        /// Datum, kdy proběhlo ověření nároku na dotace.
        /// </summary>
        public DateTime CheckDate { get; set; }

        /// <summary>
        /// Při ověření nároku na dotace došlo k chybě?
        /// </summary>
        public bool IsError { get; set; }

        /// <summary>
        /// Výsledná zpráva o průběhu ověření nároku na dotace - úšpech x chybová zpráva s informací o jakou chybu se jedná.
        /// </summary>
        public string CheckMessage { get; set; }
        #endregion

        /// <summary>
        /// Vypočtený věk zájemnce o práci dle uvedeného data narození.
        /// </summary>
        public int? Age { get; set; }

        /// <summary>
        /// Vypočtená doba evidence zájemnce o práci na ÚP dle uvedeného data registrace na ÚP. V měsících.
        /// - U zájemců bez registrace na ÚP => NULL. 
        /// - U zájemců bez uvedeného data registrace od => 0.
        /// </summary>
        public int? EvidenceMonths { get; set; }

        /// <summary>
        /// Úřad práce, na kterém je zájemce registrovaný.
        /// </summary>
        public EmployDepartment EmployDepartment { get; set; }

        /// <summary>
        /// Definice dotací, které zájemce slpňuje ke dni načtení.
        /// </summary>
        public IEnumerable<GrantDefinition> GrantDefinitions { get; set; }

        #endregion
    }
}
