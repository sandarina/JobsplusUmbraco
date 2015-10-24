using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using JobsplusUmbraco.Models;

namespace JobsplusUmbraco.Controllers
{
    public class RegisterCandidateController : SurfaceController
    {
        //
        // GET: /RegisterCandidate/

        public ActionResult Index()
        {
            return PartialView(new MemberCandidate());
        }

        [HttpPost]
        public ActionResult RegisterCandidateSubmit(MemberCandidate model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            var memberService = Services.MemberService;
            if (memberService.GetByEmail(model.Email) != null)
            {
                ModelState.AddModelError("", "Uživatel se zadaným emailem již existuje!");
                return CurrentUmbracoPage();
            }

            // Candidate je typ členského účtu
            var name = model.Firstname + " " + model.Surname;
            var member = memberService.CreateMember(model.Email, model.Email, name, "Candidate");

            // profilové údaje uživatele - úplný seznam je v /umbraco/Členové/Typy členů/Zájemce o práci
            member.SetValue("FirstName", model.Firstname);
            member.SetValue("Surname", model.Surname);
            member.SetValue("BirthDate", model.BirthDate);
            member.SetValue("RegistrationUP", model.RegistrationUP);
            member.SetValue("RegistrationUPFrom", model.RegistrationUPFrom);
            member.SetValue("Town", model.Town);
            // todo: nagrat zivotopis
            //member.SetValue("CV", model.CV);

            // Not yet allowed to log in!
            member.IsApproved = false;

            // Don't forget to save all these things
            memberService.Save(member);

            // save their password
            memberService.SavePassword(member, model.Password);

            // "Candidate" je skupina členů
            memberService.AssignRole(member.Id, "Candidate");

            TempData.Add("IsSuccess", true);
            TempData.Add("Email", model.Email);
            return RedirectToCurrentUmbracoPage();
        }
    }
}
