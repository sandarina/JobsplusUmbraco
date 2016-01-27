using JobsplusUmbraco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using System.Net.Mail;
using System.IO;
using System.Web.Security;
using Jobsplus.Backoffice;
using Jobsplus.Backoffice.Models;
using Umbraco.Web;

namespace JobsplusUmbraco.Controllers
{
    public class AdvertisementReplyFormController : SurfaceController
    {
        // GET: AdvertisementReplyForm
        public ActionResult Index(string sendToEmail)
        {
            AdvertisementReplyForm advertisementReplyForm = null;

            if (CurrentPage.DocumentTypeAlias == "dtAdvertisement")
            {
                advertisementReplyForm = new AdvertisementReplyForm();
                advertisementReplyForm.SendToEmail = sendToEmail;
                advertisementReplyForm.AdvertisementNodeId = CurrentPage.Id;
                advertisementReplyForm.CompanyNodeId = CurrentPage.Parent.Parent.Id;
                advertisementReplyForm.AdvertisementName = CurrentPage.Name;
                advertisementReplyForm.AdvertisementUrl = CurrentPage.Url;

                if (Members.IsLoggedIn())
                {
                    if (!Roles.IsUserInRole(Members.CurrentUserName, "Zájemce o práci"))
                    {
                        ModelState.AddModelError("", "Na inzerát mohou reagovat pouze uživatelé s rolí \"Zájemce o práci\".");
                    }
                    else
                    {
                        var memberService = Services.MemberService;
                        var candidate = memberService.GetByUsername(Members.CurrentUserName);
                        if (candidate != null)
                        {
                            advertisementReplyForm.CandidateMemberId = candidate.Id;
                            advertisementReplyForm.Email = candidate.Email;
                            advertisementReplyForm.CVPath = candidate.GetValue<string>("CV");
                        }
                    }
                }
            }

            return View(advertisementReplyForm);
        }

        [HttpPost]
        public ActionResult AdvertisementReplyCandidateSubmit(AdvertisementReplyForm model)
        {
            ShowAdvertisementReplyJS();

            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            if (!model.CandidateMemberId.HasValue)
            {
                ModelState.AddModelError("", "Nelze identifikovat zájemce o pracovní pozici! Zkontrolujte zda jste řádně přihlášeni!");
                return CurrentUmbracoPage();
            }

            if (!model.Confirm)
            {
                ModelState.AddModelError("", "Pro odeslání reakce na pracovní pozici je nezbytné souhlasit s našimi podmínkami.");
                return CurrentUmbracoPage();
            }

            var memberService = Services.MemberService;
            var member = memberService.GetById(model.CandidateMemberId.Value);
            if (member == null)
            {
                ModelState.AddModelError("", "Uživatel se zadaným emailem neexistuje!");
                return CurrentUmbracoPage();
            }

            var filepath = model.CVPath;

            if (model.NewCV != null && model.NewCV.InputStream != null)
            {

                var filename = Path.GetFileName(model.NewCV.FileName);
                var path = "/media/cv/";
                var fullPath = Server.MapPath("~" + path);
                var dir = new DirectoryInfo(fullPath);
                if (!dir.Exists)
                    dir.Create();
                path += JobsplusHelpers.RemoveDiacritics(member.GetValue<string>("Surname")).ToLower() + "_" + (member.GetValue<DateTime>("BirthDate")).ToString("yyyy-MM-dd") + "/";
                fullPath = Server.MapPath("~" + path); 
                var dirUser = new DirectoryInfo(fullPath);
                if (!dirUser.Exists)
                    dirUser.Create();

                try
                {                    
                    model.NewCV.SaveAs(fullPath + filename);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Při nahrávání životopisu došlo k chybě:");
                    TempData.Add("ValidationErrorInfo", "<h3>" + ex.Message + "</h3><br /><p>" + ex.StackTrace + "</p>");
                    return CurrentUmbracoPage();
                }

                filepath = path + filename;
                model.CVPath = filepath;
                member.SetValue("CV", filepath);
                member.SetValue("CVExists", true);
                memberService.Save(member);
            }

            #region Ulozeni reakce do databaze
            try
            {                    
                AdvertisementReply reply = new AdvertisementReply();
                reply.CompanyId = model.CompanyNodeId.Value;
                reply.AdvertisementId = model.AdvertisementNodeId.Value;
                reply.CandidateId = model.CandidateMemberId.Value;
                reply.CandidateName = member.Name;
                reply.CandidateEmail = member.Email;
                reply.CandidateCV = filepath;
                reply.CandidateReplyNote = model.Comment;
                reply.CreateDate = DateTime.Now;

                DatabaseContext.Database.Save(reply);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Při ukládání reakce do dataáze došlo k chybě:");
                TempData.Add("ValidationErrorInfo", "<h3>" + ex.Message + "</h3><br /><p>" + ex.StackTrace + "</p>");
                return CurrentUmbracoPage();
            }
            #endregion

            #region Odeslat email spravci
            var mail = new MailMessage(JobsplusConstants.EmailRobotEmail, model.SendToEmail);
            // TODO: odeslat oznámení na emaily účtů správců firmy, která vydala inzerát
            // ....
            if (!string.IsNullOrEmpty(filepath))
            {
                var atachementPath = Server.MapPath("~" + filepath);

                if (System.IO.File.Exists(atachementPath))
                    mail.Attachments.Add(new Attachment(atachementPath));
            }
            mail.Subject = "ZÁJEMCE O POZICI z webu jobsplus.cz";
            mail.IsBodyHtml = true;
            mail.Body = "<p>Dobrý den,<br />na webu jobsplus.cz projevil uživatel zájem o pracovní pozici. Prosím věnujte poroznost následujícím údajům a ozvěte se uživateli, co nejdříve.</p><br /><br />" +
                "<p><b>Pozice</b><p>"+
                "<p><a href=\"http://www.jobsplus.cz" + model.AdvertisementUrl + "\">" + model.AdvertisementName + "</a> (http://www.jobsplus.cz" + model.AdvertisementUrl + ")</p><br />" +
                "<p><b>Zájemce</a></b><p>" + member.Name + "</p><br />" +
                "<p><b>Email zájemce</b><p>" + member.Email + "</p><br />" +
                "<p><b>Zpráva od zájemce</b></p>" +
                "<p>" + model.Comment + "</p><br /><br />" +
                "<p>S pozdravem,<br />Váš JOBSPLUS AUTOMATICKÝ ROZESÍLAČ e-mailů ;-)</p>";

            try
            {
                var smtpClient = new SmtpClient();
                smtpClient.Send(mail);
            }
            catch(Exception ex)
            {
                var innterMsgText = ex.InnerException != null ? "<br />"+ex.InnerException.Message : "";
                ModelState.AddModelError("", "Odeslání emailu selhalo! Prosím kotaktujte naši technickou podporu na emailu info@salmaplus.cz. Do emailu uveďte následující text:");
                TempData.Add("ValidationErrorInfo", "<h3>" + ex.Message + "</h3><br /><p>" + ex.StackTrace + innterMsgText + "</p>");
                return CurrentUmbracoPage();
            }
            #endregion

            #region Odeslat email zajemci
            var mailCandidate = new MailMessage(JobsplusConstants.EmailRobotEmail, model.Email);
            if (!string.IsNullOrEmpty(filepath))
            {
                var atachementPath = Server.MapPath("~" + filepath);

                if (System.IO.File.Exists(atachementPath))
                    mail.Attachments.Add(new Attachment(atachementPath));
            }
            mailCandidate.Subject = "Projevili jste ZÁJEM O POZICI z webu jobsplus.cz";
            mailCandidate.IsBodyHtml = true;
            mailCandidate.Body = "<p>Dobrý den,<br />děkujeme Vám za projevený zájem o pracovní pozici na webu jobsplus.cz. Vaše údaje byly úspěšně odeslány inzerujícímu zaměstnavateli, který Vás bude v brzké době kontaktovat.</p><br /><br />" +
                 "<p><b>Pozice</b><p>" +
                "<p><a href=\"http://www.jobsplus.cz" + model.AdvertisementUrl + "\">" + model.AdvertisementName + "</a> (http://www.jobsplus.cz" + model.AdvertisementUrl + ")</p><br />" +
                "<p><b>Zájemce</a></b><p>" + member.Name + "</p><br />" +
                "<p><b>Email zájemce</b><p>" + member.Email + "</p><br />" +
                "<p><b>Zpráva od zájemce</b></p>" +
                "<p>" + model.Comment + "</p><br /><br />" +
                "<p>S pozdravem,<br />Váš JOBSPLUS AUTOMATICKÝ ROZESÍLAČ e-mailů ;-)</p>";
            try
            {
                var smtpClient = new SmtpClient();
                smtpClient.Send(mailCandidate);
            }
            catch (Exception ex)
            {
                var innterMsgText = ex.InnerException != null ? "<br />" + ex.InnerException.Message : "";
                ModelState.AddModelError("", "Odeslání emailu selhalo! Prosím kotaktujte naši technickou podporu na emailu info@salmaplus.cz. Do emailu uveďte následující text:");
                TempData.Add("ValidationErrorInfo", "<h3>" + ex.Message + "</h3><br /><p>" + ex.StackTrace + innterMsgText + "</p>");
                return CurrentUmbracoPage();
            }
            #endregion

            if (TempData.ContainsKey("AdvertisementReplyIsSuccess")) TempData.Remove("AdvertisementReplyIsSuccess");
            TempData.Add("AdvertisementReplyIsSuccess", true);
            return RedirectToCurrentUmbracoPage();
        }

        private void ShowAdvertisementReplyJS()
        {
            if (TempData.ContainsKey("AdvertisementReplyJS")) TempData.Remove("AdvertisementReplyJS");
            TempData.Add("AdvertisementReplyJS", "$('#advertisementReply').modal('show');");
        }
    }
}