using Jobsplus.Backoffice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsplusUmbraco.Models
{
    public enum ESubmitAction
    {
        None,
        DiscadWithEmail,
        Discard
    }

    public class RepliesForm
    {
        public int AdvertisementId { get; set; }

        public string CompanyName { get; set; }

        public List<AdvertisementReply> Replies { get; set; }

        public ESubmitAction SubmitAction { get; set; }

        public string EmailText { get; set; }
    }
}