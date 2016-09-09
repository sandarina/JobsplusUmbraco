using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using umbraco.cms.businesslogic.member;
using Umbraco.Web.Mvc;
using Umbraco.Core;
using JobsplusUmbraco.Models;
using System;

namespace JobsplusUmbraco.Controllers
{
    public class LoginController : SurfaceController
    {

        [HttpPost]
        public ActionResult HandleLogin([Bind(Prefix = "loginModel")]LoginModel model)
        {
            if (ModelState.IsValid == false)
            {
                return CurrentUmbracoPage();
            }

            if (Members.Login(model.Username, model.Password) == false)
            {
                //don't add a field level error, just model level
                ModelState.AddModelError("loginModel", "Špatné přihlašovací jméno nebo heslo.");
                return CurrentUmbracoPage();
            }
            
            string[] memberRoles = Roles.GetRolesForUser(model.Username);
            if (!memberRoles.Contains(model.RoleName))
            {
                ModelState.AddModelError("loginModel", "Přihlašovaný uživatel nemá požadovanou roli '" + model.RoleName + "'.");
                return CurrentUmbracoPage();
            }

            TempData["LoginSuccess"] = true;

            // DKO: kontrola zda uživatel neobnovoval heslo
            var member = Services.MemberService.GetByUsername(model.Username);
            bool changePasswordAfterReset = member.GetValue<bool>("passwordReset");

            TempData["ChangePasswordAfterReset"] = changePasswordAfterReset;
            TempData["ChangePasswordMemberId"] = member.Id;

            //if there is a specified path to redirect to then use it
            if (!changePasswordAfterReset && model.RedirectUrl.IsNullOrWhiteSpace() == false)
            {
                return Redirect(model.RedirectUrl);
            }

            //redirect to current page by default

            return RedirectToCurrentUmbracoPage();
            //return RedirectToCurrentUmbracoUrl();
        }

        public ActionResult HandleChangePassword([Bind(Prefix = "changePasswordModel")]ChangePasswordModel model)
        {
            if (ModelState.IsValid == false)
            {
                return CurrentUmbracoPage();
            }

            var member = Services.MemberService.GetById(model.MemberId);

            if (!Membership.ValidateUser(member.Username, model.ActualPassword))
            {
                ModelState.AddModelError("changePassword", "Zadali jste nesprávné současné heslo!");
                return CurrentUmbracoPage();
            }

            if (model.NewPassword != model.NewPasswordSecond)
            {
                ModelState.AddModelError("changePassword", "Nové heslo a nové heslo znovu nejsou stejné!");
                return CurrentUmbracoPage();
            }

            Services.MemberService.SavePassword(member, model.NewPassword);
            member.SetValue("passwordReset", false);
            Services.MemberService.Save(member);

            return RedirectToCurrentUmbracoPage();
        }
    }
}

