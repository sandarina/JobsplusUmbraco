using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

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
        #endregion

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
    }
}
