using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using umbraco.cms.businesslogic.member;
using Umbraco.Web.Mvc;
using Umbraco.Core;
using JobsplusUmbraco.Models;
using System.Text.RegularExpressions;
using System.Text;
using Jobsplus.Backoffice;

namespace JobsplusUmbraco.Controllers
{
    public class ResetPasswordController : SurfaceController
    {
        [HttpPost]
        public ActionResult HandleResetPassword([Bind(Prefix = "resetPasswordModel")]ResetPasswordModel model)
        {
            if (ModelState.IsValid == false)
            {
                return CurrentUmbracoPage();
            }
            
            var member = Services.MemberService.GetByEmail(model.Email);

            if (member == null)
            {
                ModelState.AddModelError("resetPassword", "Uživatel se zadaným emailem neexistuje!");
                return CurrentUmbracoPage();
            }

            var password = Membership.GeneratePassword(10, 1); 
            password = Regex.Replace(password, @"[^a-zA-Z0-9]", m => "9"); 
            
            // Change the password to the new generated one above
            ApplicationContext.Services.MemberService.SavePassword(member, password);
            member.SetValue("passwordReset", true);
            // Save the password/member 
            Services.MemberService.Save(member);


                 // Now email the member their password 
                 var sb = new StringBuilder();
                 sb.AppendFormat("<p>Vážený uživateli,</p>");
                 sb.AppendFormat("<p>na webu www.jobsplus.cz jsme obdrželi žádost o změnu hesla k vašemu účtu. Vaše nové heslo je:</p>"); 
                 sb.AppendFormat("<p><b>{0}</b></p>", password); 
                 sb.AppendFormat(
                     "<p>(Požadavek na změnu hesla jsme obdrželi z následující IP adresy {0}. Pokud jste o změnu hesla nežádal(a), prosím neprodleně kontaktujte administrátora webu na emailu " + JobsplusConstants.EmailRobotEmail + ".)</p>",
                     Umbraco.UmbracoContext.HttpContext.GetCurrentRequestIpAddress());

                 umbraco.library.SendMail(JobsplusConstants.EmailRobotEmail, member.Email, "Obnovení hesla", sb.ToString(), true); 

            TempData["PasswordResetSuccess"] = true;

            //if there is a specified path to redirect to then use it
            if (model.RedirectUrl.IsNullOrWhiteSpace() == false)
            {
                return Redirect(model.RedirectUrl);
            }

            //redirect to current page by default

            return RedirectToCurrentUmbracoPage();
            //return RedirectToCurrentUmbracoUrl();
        }
    }
}