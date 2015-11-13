using System;
using System.IO;
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
            if (!model.Confirm)
            {
                ModelState.AddModelError("", "Pro dokončení registrace je nezbytné udělit souhlas se zpracováním Vašich osobních údajů, a také souhlas s našimi obchodními podmínkami a podmínkami užití.");
                return CurrentUmbracoPage();
            }

            // Candidate je typ členského účtu
            var name = model.Firstname + " " + model.Surname;
            var member = memberService.CreateMember(model.Email, model.Email, name, "Candidate");

            // profilové údaje uživatele - úplný seznam je v /umbraco/Členové/Typy členů/Zájemce o práci
            member.SetValue("FirstName", model.Firstname);
            member.SetValue("Surname", model.Surname);
            member.SetValue("BirthDate", model.BirthDate);
            member.SetValue("Phone", model.Phone);
            member.Comments = model.Comments;
            member.SetValue("RegistrationUP", model.RegistrationUP);
            member.SetValue("RegistrationUPFrom", model.RegistrationUPFrom);
            member.SetValue("Town", model.Town);
            // todo: nahrat zivotopis
            if (model.CV != null && model.CV.InputStream != null)
            {
                var filename = Path.GetFileName(model.CV.FileName);
                var path = "/media/cv/";
                var fullPath = Server.MapPath("~" + path);
                var dir = new DirectoryInfo(fullPath);
                if (!dir.Exists)
                    dir.Create();
                path += JobsplusHelpers.RemoveDiacritics(model.Surname).ToLower() + "_" + model.BirthDate.Value.ToString("yyyy-MM-dd") + "/";
                fullPath = Server.MapPath("~" + path); 
                var dirUser = new DirectoryInfo(fullPath);
                if (!dirUser.Exists)
                    dirUser.Create();

                try
                {
                    model.CV.SaveAs(fullPath + filename);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Při nahrávání životopisu došlo k chybě: <br /><h3>" + ex.Message + "</h3><br /><p>" + ex.StackTrace + "</p>");
                    return CurrentUmbracoPage();
                }

                member.SetValue("CV", path + filename);
            }
            //member.SetValue("CV", model.CV);

            // Not yet allowed to log in!
            member.IsApproved = true;

            // Don't forget to save all these things
            memberService.Save(member);

            // save their password
            memberService.SavePassword(member, model.Password);

            // "Candidate" je skupina členů
            memberService.AssignRole(member.Id, "Candidate");

            TempData.Add("RegisterCandidateIsSuccess", true);
            TempData.Add("Email", model.Email);
            return RedirectToCurrentUmbracoPage();
        }
    }
}
