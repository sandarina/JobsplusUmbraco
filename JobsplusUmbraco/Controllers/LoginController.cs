using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using umbraco.cms.businesslogic.member;
using Umbraco.Web.Mvc;
using Umbraco.Core;
using JobsplusUmbraco.Models;

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

