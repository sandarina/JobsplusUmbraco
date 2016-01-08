﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsplusUmbraco.Models
{
    public class AdvertisementReplyForm
    {
        /// <summary>
        /// Id člena - zájemce o pracovní pozici
        /// </summary>
        public int? CandidateMemberId { get; set; }

        /// <summary>
        /// NodeId inzerátu
        /// </summary>
        public int? AdvertisementNodeId { get; set; }

        /// <summary>
        /// Název inzerátu
        /// </summary>
        public string AdvertisementName { get; set; }

        /// <summary>
        /// Url inzerátu
        /// </summary>
        public string AdvertisementUrl { get; set; }

        /// <summary>
        /// Email zájemce
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Životopis zájemce
        /// </summary>
        public string CVPath { get; set; }

        /// <summary>
        /// Životopis zájemce
        /// </summary>
        public HttpPostedFileBase NewCV { get; set; }

        /// <summary>
        /// Komentář zájemce.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Email, na který dojde reakce zájemce o pracovní pozici.
        /// </summary>
        public string SendToEmail { get; set; }

        /// <summary>
        /// Souhlas se zpracováním osobních údajů.
        /// </summary>
        public bool Confirm { get; set; }
    }
}