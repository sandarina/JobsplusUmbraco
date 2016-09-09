using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Umbraco.Web.Models;

namespace JobsplusUmbraco.Models
{
    public class ResetPasswordModel : PostRedirectModel
    {
        [DisplayName("Email")]
        [Required(ErrorMessage = "Zadejte email k vašemu účtu.")]
        public string Email { get; set; }

        public ResetPasswordModel()
        {
        }
    }
}