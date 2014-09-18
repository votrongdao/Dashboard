using DashboardSite.BusinessService.Interface;
using DashboardSite.Core;
using DashboardSite.Core.Utilities;
using DashboardSite.Model;
using DashboardSite.Repository;
using DashboardSite.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DashboardSite.BusinessService
{
    public class EmailManager : IEmailManager
    {
        private IEmailRepository emailRepository;
        private ILookupManager lookupMgr;
        private UnitOfWork unitOfWorkLive;
        private string supportEmail = "fake@mail.com";

        public string SupportEmail
        {
            get { return supportEmail; }
        }
        
        public EmailManager() 
        {
            unitOfWorkLive = new UnitOfWork();
            emailRepository = new EmailRepository(unitOfWorkLive);
            lookupMgr = new LookupManager(); 
        }

        /// <summary>
        /// change later to return bool, false/true indicate success save or not
        /// </summary>
        /// <param name="email"></param>
        public void SaveEmail(EmailModel email)
        {
            try
            {
                emailRepository.SaveEmail(email);
                unitOfWorkLive.Save();
            }
            catch(Exception e)
            {
                throw new InvalidOperationException(string.Format("Error occurred while saving email {0}", email.ToString()), e.InnerException);
            }
        }

        public bool SendResetPasswordEmail(string userName, string htmlEmbedLink)
        {
            try
            {
                if (string.IsNullOrEmpty(userName))
                    return false;
                MailMessage mail = new MailMessage();

                SmtpClient smtpServer = new SmtpClient();
                mail.From = new MailAddress(supportEmail);
                mail.To.Add(userName);
                mail.Bcc.Add(supportEmail);
                mail.Subject = "Please reset your password within 48 hours upon receiving this email";
                mail.Body = string.Format(@"To reset your password, <a href='{0}'> please click the link and enter new password.</a>",
                    htmlEmbedLink);
                mail.IsBodyHtml = true;

                var emailToSave = new EmailModel();
                emailToSave.EmailTo = TextUtils.TokenDelimitedText(mail.To.ToList().Select(p => p.Address), ";");
                emailToSave.EmailFrom = mail.From.Address;
                emailToSave.Subject = mail.Subject;
                emailToSave.ContentHtml = emailToSave.ContentText = mail.Body;
                emailToSave.MailTypeId = lookupMgr.GetAllEmailTypes().FirstOrDefault(p => p.EmailTypeCd == "PWDRC").EmailTypeId;
                var emailMgr = new EmailManager();
                emailMgr.SaveEmail(emailToSave);

                smtpServer.Send(mail);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }    
    }
}
