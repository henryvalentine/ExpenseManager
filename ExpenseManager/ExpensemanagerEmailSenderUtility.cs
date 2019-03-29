using System;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using XPLUG.WEBTOOLS;

namespace ExpenseManager
{
    public class ExpensemanagerEmailSenderUtility
    {

        public bool SendMail(MailAddress fromAddress, string to, string subject, string body, string userName, string password)
        {
            try
            {
                var mail = new MailMessage { From = fromAddress };
                
                var smtp = new SmtpClient
                               {
                                   Port = 587,
                                   EnableSsl = true,
                                   DeliveryMethod = SmtpDeliveryMethod.Network,
                                   UseDefaultCredentials = false,
                                   Credentials = new NetworkCredential(userName, password),
                                   Host = "smtp.gmail.com"
                               };

          
                mail.To.Add(new MailAddress(to));
                mail.IsBodyHtml = true;
                mail.Subject = subject;
                mail.Body = body;
                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }
    }
}