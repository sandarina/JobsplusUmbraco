using Jobsplus.Backoffice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Persistence;
using Umbraco.Web;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace Jobsplus.Backoffice.Controllers
{
    [PluginController("JobsplusGrants")]
    public class GrantDefinitionsApiController : UmbracoAuthorizedJsonController
    {
        #region Grant Definitions
        /// <summary>
        /// Získá všechny definice dotace dle filtru.
        /// </summary>
        /// <param name="grantId">Id dotace</param>
        /// <returns></returns>
        public IEnumerable<GrantDefinition> GetAll(int? grantId = null)
        {

            var query = new Sql().Select("*").From("JobsplusGrantDefinitions");
            if (grantId.HasValue) query.Where<GrantDefinition>(item => item.GrantId == grantId.Value);
            return DatabaseContext.Database.Fetch<GrantDefinition>(query);
        }


        public GrantDefinition GetById(int id)
        {

            var query = new Sql().Select("*").From("JobsplusGrantDefinitions").Where<GrantDefinition>(item => item.Id == id);
            return DatabaseContext.Database.Fetch<GrantDefinition>(query).FirstOrDefault();

        }

        public GrantDefinition PostSave(GrantDefinition grantDefinition)
        {
            if (grantDefinition.ContractType == null) grantDefinition.ContractType = "";
            if (grantDefinition.Id > 0)
                DatabaseContext.Database.Update(grantDefinition);
            else
                DatabaseContext.Database.Save(grantDefinition);

            return grantDefinition;
        }

        public int DeleteById(int id)
        {
            return DatabaseContext.Database.Delete<GrantDefinition>(id);
        }
        #endregion

        /// <summary>
        /// Získá všechny definice dotací, na které má nárok člen.
        /// </summary>
        /// <param name="memberId">Id člena</param>
        /// <returns></returns>
        public MemberGrantDefResult GetAllByMember(int memberId)
        {
            var result = new MemberGrantDefResult();
            result.CheckDate = DateTime.Now;

            // načtení údajů o členovi
            var memberService = ApplicationContext.Services.MemberService;
            var member = memberService.GetById(memberId);

            #region Validation member
            // ověření člena
            if (member == null)
            {
                result.IsError = true;
                result.CheckMessage = "Nepodařilo se načíst zájemce o práci, jeho profil byl odstraněn.";
                return result;
            }
            // typ - zájemce o práci?
            if (member == null || member.ContentTypeAlias != "Candidate")
            {
                result.IsError = true;
                result.CheckMessage = "Nejedná se zájemce o práci!";
                return result;
            }
            #endregion
            result.Id = member.Id;
            result.Phone = member.GetValue<string>("Phone");

            DateTime? birthDate = null;
            if (member.GetValue("BirthDate") != null)
            {
                birthDate = member.GetValue<DateTime>("BirthDate");
            }
            else
            {
                // věk nelze určit => nelze určit nárok na dotace
                result.IsError = true;
                result.CheckMessage = "Zájemce nemá uvedeno datum narození, na jehož základě se určuje nárok na dotace dle věku!";
                return result;
            }
            result.Age = result.CheckDate.Year - birthDate.Value.Year;

            if (!member.GetValue<bool>("RegistrationUP"))
            {
                result.IsError = true;
                result.CheckMessage = "Zájemce uvedl, že NENÍ registrován na ÚP.";
                return result;
            }

            int evidenceMonths = 0;
            if (member.GetValue("RegistrationUPFrom") != null)
            {
                var registrationUPFrom = member.GetValue<DateTime>("RegistrationUPFrom");
                evidenceMonths = ((result.CheckDate.Year - registrationUPFrom.Year) * 12) + result.CheckDate.Month - registrationUPFrom.Month;
            }
            else
            {
                result.IsError = true;
                result.CheckMessage = "Zájemce uvedl, že je registrován na ÚP, ale neuvedl od kterého data!";
                return result;
            }
            result.EvidenceMonths = evidenceMonths;

            if (member.GetValue("EmployeeDepartment") != null)
            {
                var employeeDepartmentId = member.GetValue<int>("EmployeeDepartment");
                var ctrl = new EmployDepartmentsApiController();                
                result.EmployDepartment = ctrl.GetById(employeeDepartmentId);
                if (result.EmployDepartment == null)
                {
                    result.IsError = true;
                    result.CheckMessage = "Při načtení vybraného ÚP selhalo! EmployeeDepartment.Id = " + employeeDepartmentId.ToString();
                    return result;
                }
            }
            else
            {
                result.IsError = true;
                result.CheckMessage = "Zájemce uvedl, že je registrován na ÚP, ale neuvedl na kterém ÚP!";
                return result;
            }

            // načtení kritérií pro kontrolu nároku na dotace proběhlo v pořádku
            result.IsError = false;
            result.CheckMessage = "Zájemce o práci má uvedeny všechny potřebné údaje pro ověření nároku na dotace.";

            var query = new Sql().Select("gd.*").From("JobsplusGrantDefinitions gd").
                LeftJoin("JobsplusGrantDefEmployDeparts gded").On("gded.GrantDefinitionId = gd.Id").
                Where(@"
                    (gd.AgeFrom <= @0 AND gd.AgeTo >= @0) AND
                    gd.EvidenceMonths <= @1 AND
                    gded.EmployDepartmentId = @2", result.Age, result.EvidenceMonths, result.EmployDepartment.Id);
            result.GrantDefinitions = DatabaseContext.Database.Fetch<GrantDefinition>(query);

            return result;
        }

        public int CountAllByMember(int memberId)
        {
            var defs = GetAllByMember(memberId);
            return defs.GrantDefinitions == null ? 0 : defs.GrantDefinitions.Count();
        }
    }
}
