using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Umbraco.Web.Models;

namespace JobsplusUmbraco.Models
{
    public class LoginModel : PostRedirectModel
    {
        [Required]
        [DataMember(Name = "username", IsRequired = true)]
        public string Username { get; set; }

        [Required]
        [DataMember(Name = "password", IsRequired = true)]
        public string Password { get; set; }

        [Required]
        [DataMember(Name = "roleName", IsRequired = true)]
        public string RoleName { get; set; }

        public LoginModel()
        {
        }

        public LoginModel(string roleName)
        {
            this.RoleName = roleName;
        }

    }
}

