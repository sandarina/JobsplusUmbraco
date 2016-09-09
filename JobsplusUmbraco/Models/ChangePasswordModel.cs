using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace JobsplusUmbraco.Models
{
    public class ChangePasswordModel
    {
        [Required]
        [DataMember()]
        public int MemberId { get; set; }

        [Required(ErrorMessage = "Vyplňte Vaše současné heslo!")]
        [DataMember(Name = "Současné heslo")]
        public string ActualPassword { get; set; }

        [Required(ErrorMessage = "Vyplňte nové heslo!")]
        [DataMember(Name = "Nové heslo")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Vyplňte znovu nové heslo!")]
        [DataMember(Name = "Nové heslo (znovu)")]
        public string NewPasswordSecond { get; set; }

        public ChangePasswordModel()
        {
        }

        public ChangePasswordModel(int memberId)
        {
            MemberId = memberId;
        }
    }
}