using Jobsplus.Backoffice.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;
using Umbraco.Core;

namespace Jobsplus.Backoffice.Models
{
    /// <summary>
    /// Reakce na inzerát
    /// </summary>
    [TableName("JobsplusAdvertisementReply")]
    [PrimaryKey("Id", autoIncrement = true)]
    public class AdvertisementReply
    {
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        /// <summary>
        /// NodeId firmy
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// NodeId inzerátu
        /// </summary>
        public int AdvertisementId { get; set; }

        /// <summary>
        /// MemberId zájemce o práci, který odpověď odeslal
        /// </summary>
        public int CandidateId { get; set; }

        /// <summary>
        /// Jméno zájemce o práci
        /// </summary>
        public string CandidateName { get; set; }

        /// <summary>
        /// Email zájemce o práci
        /// </summary>
        public string CandidateEmail { get; set; }

        /// <summary>
        /// Životopis zájemce o práci - cesta k souboru
        /// </summary>
        [NullSetting(NullSetting=NullSettings.Null)]
        public string CandidateCV { get; set; }

        /// <summary>
        /// Poznámka udaná zájemcem o práci při odeslání reakce na inzerát
        /// </summary>
        [NullSetting(NullSetting = NullSettings.Null)]
        public string CandidateReplyNote { get; set; }

        /// <summary>
        /// Datum odeslání reakce zájemcem o práci
        /// </summary>
        public DateTime CreateDate { get; set; }

        #region Údaje o reakci zamestnavatele
        /// <summary>
        /// Reakce shlédnuta zaměstnavatelem?
        /// </summary>
        public bool IsViewed { get; set; }

        /// <summary>
        /// Datum shlédnutí reakce zaměstnavatelem
        /// </summary>
        [NullSetting(NullSetting = NullSettings.Null)]
        public DateTime? ViewDate { get; set; }

        /// <summary>
        /// Reakce označena zaměstnavatelem jako vyřízena
        /// </summary>
        public bool IsCheckOut { get; set; }

        /// <summary>
        /// Datum označení reakce zaměstnavatelem za vyřízenou
        /// </summary>
        [NullSetting(NullSetting = NullSettings.Null)]
        public DateTime? CheckOutDate { get; set; }

        /// <summary>
        /// Reakce POTVRZENA zaměstnavatelem? Označuje zaměstnavatel při vyřízení reakce. 
        /// </summary>
        [NullSetting(NullSetting = NullSettings.Null)]
        public bool? IsApproved { get; set; }

        /// <summary>
        /// Reakce ODMÍTNUTA zaměstnavatelem? Označuje zaměstnavatel při vyřízení reakce.
        /// </summary>
        [NullSetting(NullSetting = NullSettings.Null)]
        public bool? IsDiscarded { get; set; }

        [Ignore]
        public string CandidateGrantDefs 
        { 
            get 
            {
                var ctrl = new GrantDefinitionsApiController();
                var result = ctrl.GetAllByMember(this.CandidateId);
                this.CandidateGrantDefsCount = result.GrantDefinitions == null ? 0 : result.GrantDefinitions.Count();
                var resultHtml = "<div class=\"" + (result.IsError ? "alert alert-danger" : "alert alert-success") + "\">" + result.CheckMessage + "</div>";
                if (result.IsError)
                {
                    resultHtml += "<p>Nelze určit splnění nároku na dotace.</p>";
                }
                else
                {
                    resultHtml += "<p><strong>" + CandidateName + "</strong></p>" +
                        "<p><a href=\"mailto:" + CandidateEmail + "\">" + CandidateEmail + "</a>, tel. " + result.Phone + "</p>" +
                        "<table style=\"width: 100%\">" +
                        "<tr><th>Věk zájemce</th><th>Délka evidence na ÚP (poč. měsíců)</th><th>Kontrolní pracoviště ÚP</th></tr>" +
                        "<tr><td>" + result.Age.ToString() + "</td><td>" + result.EvidenceMonths.ToString() + "</td><td>" + result.EmployDepartment.Name + "</td></tr>" +
                        "</table><br />" +
                        "<p>Zájemce splňuje kritéria pro nárok na dotace v počtu: " + CandidateGrantDefsCount.ToString() + "</p><br />";
                } 
                if (CandidateGrantDefsCount > 0)
                {
                    foreach(var def in result.GrantDefinitions)
                    {
                        resultHtml += "<p><strong>" + def.Name + "</strong></p>" +
                            "<table style=\"width: 100%\">" +
                    "<tr><th>Věk (od-do)</th><th>Délka dotace</th><th>Výše dotace</th><th>Typ smlouvy</th></tr>" +
                    "<tr><td>" + def.AgeFrom.ToString() + "-" + def.AgeTo.ToString() + "</td><td>" + def.GrantMonths.ToString() + " měsíců</td><td>" + def.GrantValue.ToString("N0") + " Kč/měsíc</td><td>" + def.ContractType + "</td></tr>" +
                    (!string.IsNullOrWhiteSpace(def.Note) ? "<tr><td colspan=\"4\"><i>Pozn.: " + def.Note + "</i></td></tr>" : "")  +
                    "</table><br />";
                    }
                    resultHtml += "<p><strong>Kritéria pro splnění nároku na dotace vždy ověřte přímo u zájemce o práci!</strong></p>";
                }
                return resultHtml;
            }
        }

        [Ignore]
        public int CandidateGrantDefsCount { get; set; }
        #endregion

        private DBContextController DBContext = new DBContextController();

        public static AdvertisementReply Get(int id, UmbracoDatabase db)
        {
            var query = new Sql().Select("*").From("JobsplusAdvertisementReply").Where("Id = " + id);
            return db.Fetch<AdvertisementReply>(query).FirstOrDefault();
        }

        public static List<AdvertisementReply> GetAdvertisementReplies(int advertisementId, UmbracoDatabase db)
        {
            var query = new Sql().Select("*").From("JobsplusAdvertisementReply").Where("AdvertisementId = " + advertisementId);
            return db.Fetch<AdvertisementReply>(query);
        }

        public List<AdvertisementReply> GetAdvertisementReplies(int advertisementId)
        {
            return DBContext.GetAdvertisementReplies(advertisementId);
        }
    }
}
