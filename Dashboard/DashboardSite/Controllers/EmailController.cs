using DashboardSite.BusinessService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace DashboardSite.Controllers
{
    public class EmailController : Controller
    {
        //
        // GET: /Email/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendEmail(FormCollection collection)
        {
            try
            {
                var emailMgr = new EmailManager();
                MailMessage mail = new MailMessage();

                SmtpClient smtpServer = new SmtpClient();
                mail.From = new MailAddress(emailMgr.SupportEmail);
                mail.To.Add(emailMgr.SupportEmail);
                mail.Subject = "Request from Portal user";
                mail.Body = "How to recover password?";

                smtpServer.Send(mail);

                return RedirectToAction("Contact", "Home");
            }
            catch(Exception e)
            {
                return View("Contact");
            }
        }

    }
}
