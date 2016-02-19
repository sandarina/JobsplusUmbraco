using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using JobsplusUmbraco.Models;
using Umbraco.Core.Models;
using Jobsplus.Backoffice;
using Jobsplus.Backoffice.Controllers;

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
            if (candidate.GetValue("EmployeeDepartment") != null)
            { 
                var employeeDepartmentId = candidate.GetValue<int>("EmployeeDepartment");
                model.EmployeeDepartmentId = employeeDepartmentId;
                var ctrl = new EmployDepartmentsApiController();
                model.EmployeeDepartmentName = ctrl.GetById(employeeDepartmentId).Name;
            }
            model.Comments = candidate.Comments;

            if (TempData.ContainsKey("EditCandidateCV")) TempData.Remove("EditCandidateCV");
            TempData.Add("EditCandidateCV", candidate.GetValue<string>("CV"));

            // prihlasovaci udaje
            model.Email = profile.UserName;
            model.Password = candidate.RawPasswordValue;
            // souhlas
            model.Confirm = true;

            return PartialView(model);
        }

        public ActionResult CloseSuccessMessage(string url)
        {
            if (TempData.ContainsKey("EditCandidateIsSuccess")) TempData.Remove("EditCandidateIsSuccess");
            if (!string.IsNullOrEmpty(url))
                return Redirect(url);
            else
                return RedirectToCurrentUmbracoPage();
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
            var cvExists = false;

            // profilové údaje uživatele - úplný seznam je v /umbraco/Členové/Typy členů/Zájemce o práci
            member.Name = name;
            member.SetValue("FirstName", model.Firstname);
            member.SetValue("Surname", model.Surname);
            member.SetValue("BirthDate", model.BirthDate);
            member.SetValue("Phone", model.Phone);
            member.SetValue("RegistrationUP", model.RegistrationUP);
            member.SetValue("RegistrationUPFrom", model.RegistrationUPFrom);
            member.SetValue("EmployeeDepartment", model.EmployeeDepartmentId);
            member.Comments = model.Comments;

            if (model.EmployeeDepartmentId.HasValue)
            {
                var ctrl = new EmployDepartmentsApiController();
                model.EmployeeDepartmentName = ctrl.GetById(model.EmployeeDepartmentId.Value).Name;
            }

            // todo: nahrat zivotopis
            if (model.CV != null && model.CV.InputStream != null)
            {
                var filename = "zivotopis_" + DateTime.Now.ToString("yyyy-MM-dd") + Path.GetExtension(model.CV.FileName);
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
                    cvExists = true;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", JobsplusConstants.SendEmailErrorMsg);
                    TempData.Add("ValidationErrorInfo", JobsplusHelpers.GetMsgFromException(ex));
                    return CurrentUmbracoPage();
                }
                
                var filepath = path + filename;
                TempData["EditCandidateCV"] = filepath;
                member.SetValue("CV", filepath);
            }
            member.SetValue("CVExists", cvExists);

            // Don't forget to save all these things
            memberService.Save(member);

            // save their password
           // memberService.SavePassword(member, model.Password);

            if (TempData.ContainsKey("EditCandidateIsSuccess")) TempData.Remove("EditCandidateIsSuccess");
            TempData.Add("EditCandidateIsSuccess", true);
            return RedirectToCurrentUmbracoPage();
        }
    }
}
