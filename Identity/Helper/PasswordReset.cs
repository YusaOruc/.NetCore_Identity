using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace Identity.Helper
{
    public static class PasswordReset
    {
        public static void PAsswordResetSendEmail(string link, string email, string userName)
        {
            var fromAddress = new MailAddress("test@gmail.com", "Admin");
            var toAddress = new MailAddress(email, userName);
            const string fromPassword = "123456";
            const string subject = "www.domain.com::Reset Password";
            string body = $"<a href='{link}'>Reset Password Link</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
        }
    }
}
