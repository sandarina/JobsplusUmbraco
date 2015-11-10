using System;
using System.IO;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using JobsplusUmbraco.Models;
using Umbraco.Core.Models;

namespace JobsplusUmbraco.Controllers
{
    public class EditCandidateController : SurfaceController
    {
        //
        // GET: /EditCandidate/

        public ActionResult Index()
        {
            var memberService = Services.MemberService;
            var profile = Members.GetCurrentMemberProfileModel();
            var candidate = memberService.GetByUsername(Members.CurrentUserName); // Members.GetByUsername(Members.CurrentUserName) as IMember;
            MemberCandidate model = new MemberCandidate();
            model.Firstname = candidate.GetValue<string>("Firstname");
            model.Surname = candidate.GetValue<string>("Surname");
            if (candidate.GetValue("BirthDate") != null)
                model.BirthDate = candidate.GetValue<DateTime>("BirthDate");
            model.Phone = candidate.GetValue<string>("Phone");
            model.RegistrationUP = candidate.GetValue<bool>("RegistrationUP");
            if (candidate.GetValue("RegistrationUPFrom") != null)
                model.RegistrationUPFrom = candidate.GetValue<DateTime>("RegistrationUPFrom");
            model.Town = candidate.GetValue<string>("Town");
            model.Comments = candidate.Comments;

            TempData.Add("EditCandidateCV", candidate.GetValue<string>("CV"));

            // prihlasovaci udaje
            model.Email = profile.UserName;
            model.Password = candidate.RawPasswordValue;
            // souhlas
            model.Confirm = true;

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult EditCandidateSubmit(MemberCandidate model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            var memberService = Services.MemberService;
            if (memberService.GetByEmail(model.Email) == null)
            {
                ModelState.AddModelError("", "Uživatel se zadaným emailem neexistuje!");
                return CurrentUmbracoPage();
            }

            // Candidate je typ členského účtu
            var name = model.Firstname + " " + model.Surname;
            var member = memberService.GetByEmail(model.Email);

            // profilové údaje uživatele - úplný seznam je v /umbraco/Členové/Typy členů/Zájemce o práci
            member.SetValue("FirstName", model.Firstname);
            member.SetValue("Surname", model.Surname);
            member.SetValue("BirthDate", model.BirthDate);
            member.SetValue("Phone", model.Phone);
            member.SetValue("RegistrationUP", model.RegistrationUP);
            member.SetValue("RegistrationUPFrom", model.RegistrationUPFrom);
            member.SetValue("Town", model.Town);
            member.Comments = model.Comments;
            // todo: nahrat zivotopis
            if (model.CV != null && model.CV.InputStream != null)
            {
                var filename = Path.GetFileName(model.CV.FileName);
                var path = Server.MapPath("~/media/cv/");
                var dir = new DirectoryInfo(path);
                if (!dir.Exists)
                    dir.Create();
                path += RemoveDiacritics(model.Surname).ToLower() + "_" + model.BirthDate.Value.ToString("yyyy-MM-dd") + "/";
                var dirUser = new DirectoryInfo(path);
                if (!dirUser.Exists)
                    dirUser.Create();

                var filepath = path + filename;
                try
                {
                    model.CV.SaveAs(filepath);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Při nahrávání životopisu došlo k chybě: <br /><h3>" + ex.Message + "</h3><br /><p>" + ex.StackTrace + "</p>");
                    return CurrentUmbracoPage();
                }

                TempData["EditCandidateCV"] = filepath;
                member.SetValue("CV", filepath);
            }

            // Don't forget to save all these things
            memberService.Save(member);

            // save their password
           // memberService.SavePassword(member, model.Password);

            TempData.Add("EditCandidateIsSuccess", true);
            return RedirectToCurrentUmbracoPage();
        }

        static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
