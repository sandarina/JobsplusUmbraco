using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobsplusUmbraco.Models
{
    /// <summary>
    /// Zájemnce o pracovní pozici.
    /// </summary>
    public class MemberCandidate
    {
        #region Profile
        /* Údaje o zajemci - profil uživatele */

        [DisplayName("Jméno")]
        [Required(ErrorMessage="Zadejte vaše jméno.")]
        public string Firstname { get; set; }

        [DisplayName("Příjmení")]
        [Required(ErrorMessage = "Zadejte vaše příjmení.")]
        public string Surname { get; set; }

        [DisplayName("Datum narození")]
        [Required(ErrorMessage = "Zadejte vaše datum narození.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd. MM. yyyy}", NullDisplayText = "")]
        public DateTime? BirthDate { get; set; }

        [DisplayName("Registrován na úřadu práce?")]
        public bool RegistrationUP { get; set; }

        [DisplayName("Datum registrace na úřadu práce")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd. MM. yyyy}", NullDisplayText="")]
        public DateTime? RegistrationUPFrom { get; set; }

        [DisplayName("Město")]
        public string Town { get; set; }

        [DisplayName("Životopis")]
        public HttpPostedFileBase CV { get; set; }
        #endregion

        #region Account
        /* Údaje o uživatelském účtu - přihlašovací jméno (email) a heslo */

        [DisplayName("Email")]
        [Required(ErrorMessage = "Zadejte váš email, kterým se budete přihlašovat.")]
        [EmailAddress(ErrorMessage="Zadete emailovou adresu ve správném formátu.")]
        public string Email { get; set; }

        [DisplayName("Heslo")]
        [Required(ErrorMessage = "Zadejte vaše heslo, kterým se budete přihlašovat.")]
        [MinLength(4, ErrorMessage="Heslo musí obsahovat alespoň 4 znaky.")]
        public string Password { get; set; }
        #endregion
    }
}