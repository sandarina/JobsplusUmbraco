using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using JobsplusUmbraco.Models;
using System.Net.Mail;

namespace JobsplusUmbraco.Controllers
{
    public class RegisterCandidateController : SurfaceController
    {
        const string _SendToEmail = "info@salmaplus.cz";
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
            var member = memberService.CreateMember(model.Email, model.Email, name, "Zájemce o práci");
            var filepath = "";
            var cvExists = false;

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
                    filepath = fullPath + filename;
                    model.CV.SaveAs(fullPath + filename);
                    cvExists = true;
                }
                catch (Exception ex)
                {
                    filepath = "";
                    ModelState.AddModelError("", "Při nahrávání životopisu došlo k chybě: <br /><h3>" + ex.Message + "</h3><br /><p>" + ex.StackTrace + "</p>");
                    return CurrentUmbracoPage();
                }

                member.SetValue("CV", path + filename);
            }
            member.SetValue("CVExists", cvExists);

            // Not yet allowed to log in!
            member.IsApproved = true;

            // Don't forget to save all these things
            memberService.Save(member);

            // save their password
            memberService.SavePassword(member, model.Password);

            // "Candidate" je skupina členů
            memberService.AssignRole(member.Id, "Zájemce o práci");



            #region Odeslat email spravci
            var mail = new MailMessage("info@jobsplus.cz", _SendToEmail);
            if (!string.IsNullOrEmpty(filepath))
            {
                var atachementPath = Server.MapPath("~" + filepath);

                if (System.IO.File.Exists(atachementPath))
                    mail.Attachments.Add(new Attachment(atachementPath));
            }
            mail.Subject = "NOVÁ REGISTRACE na jobsplus.cz";
            mail.IsBodyHtml = true;
            mail.Body = "<p>Dobrý den,<br />na webu jobsplus.cz se zaregistroval nový uchazeč o zaměstnání.</p><br /><br />" +
                "<h3>Uchazeč</h3>" +
                "<p><b>Jméno a příjmení</b><p>" + name + "</p><br />" +
                "<p><b>Email</b></p><p>" + member.Email + "</p><br />" +
                "<p><b>Datum narození</b><p>" + model.BirthDate + "</p><br />" +
                "<p><b>Telefon</b><p>" + model.Phone + "</p><br />" +
                "<p><b>Registrován na ÚP?</b><p>" + (model.RegistrationUP ? "Ano" : "Ne") + "</p><br />" +
                (model.RegistrationUP ? "<p><b>Úřad práce (město)</b><p>" + model.Town + "</p><br />" : "") +
                (!string.IsNullOrEmpty(model.Comments) ? "<h3>Zpráva od zájemce</h3><p>" + model.Comments + "</p><br /><br />" : "") + "<br />" +
                "<p>S pozdravem,<br />Váš JOBSPLUS AUTOMATICKÝ ROZESÍLAČ e-mailů ;-)</p>";

            try
            {
                var smtpClient = new SmtpClient();
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                var innterMsgText = ex.InnerException != null ? "<br />" + ex.InnerException.Message : "";
                ModelState.AddModelError("", "Odeslání emailu selhalo! Prosím kotaktujte naši technickou podporu na emailu info@salmaplus.cz. Do emailu uveďte následující text:<br /><br />" +
                    ex.Message + "<br />" + ex.StackTrace + innterMsgText + "<br /><br />Děkujeme, a omlouváme se za dočasné obtíže...");
                return CurrentUmbracoPage();
            }
            #endregion

            #region Odeslat email zajemci
            var mailCandidate = new MailMessage("info@jobsplus.cz", model.Email);
            if (!string.IsNullOrEmpty(filepath))
            {
                var atachementPath = Server.MapPath("~" + filepath);

                if (System.IO.File.Exists(atachementPath))
                    mail.Attachments.Add(new Attachment(atachementPath));
            }

            mailCandidate.Subject = "POTVRZENÍ REGISTRACE na jobsplus.cz";
            mailCandidate.IsBodyHtml = true;
            mailCandidate.Body = "<p>Dobrý den,<br />zaznamenali jsme Vaši registraci na webu jobsplus.cz.</p><br /><br />" +
                "<h3>Přihlašovací údaje</h3>" +
                "<p><b>Přihlašovací jméno</b></p><p>" + member.Email + "</p><br />" +
                "<p><b>Heslo</b></p><p>" + model.Password + "</p><br />" +
                "<p>Váš účet je již aktivní a můžete přihlásit na web jobsplus.cz a odpovídat inzerci. Těšíme se Vaší přízni a přejeme brzké nalezení vysněného zaměstnání.</p><br /><br />" +
                "<h3>Údaje o Vás</h3>" +
                "<p><b>Jméno a příjmení</b><p>" + name + "</p><br />" +
                "<p><b>Email</b></p><p>" + member.Email + "</p><br />" +
                "<p><b>Datum narození</b><p>" + model.BirthDate + "</p><br />" +
                "<p><b>Telefon</b><p>" + model.Phone + "</p><br />" +
                "<p><b>Registrován na ÚP?</b><p>" + (model.RegistrationUP ? "Ano" : "Ne") + "</p><br />" +
                (model.RegistrationUP ? "<p><b>Úřad práce (město)</b><p>" + model.Town + "</p><br />" : "") +
                (!string.IsNullOrEmpty(model.Comments) ? "<h3>Poznámka</h3><p>" + model.Comments + "</p><br />" : "") + "<br />" +
                "<p>S pozdravem,<br />Váš JOBSPLUS AUTOMATICKÝ ROZESÍLAČ e-mailů ;-)</p>";
            try
            {
                var smtpClient = new SmtpClient();
                smtpClient.Send(mailCandidate);
            }
            catch (Exception ex)
            {
                var innterMsgText = ex.InnerException != null ? "<br />" + ex.InnerException.Message : "";
                ModelState.AddModelError("", "Odeslání emailu selhalo! Prosím kotaktujte naši technickou podporu na emailu info@salmaplus.cz. Do emailu uveďte následující text:<br /><br />" +
                    ex.Message + "<br />" + ex.StackTrace + innterMsgText + "<br /><br />Děkujeme, a omlouváme se za dočasné obtíže...");
                return CurrentUmbracoPage();
            }
            #endregion

            TempData.Add("RegisterCandidateIsSuccess", true);
            TempData.Add("Email", model.Email);
            return RedirectToCurrentUmbracoPage();
        }
    }
}
