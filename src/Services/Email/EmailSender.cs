using Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;

namespace Services.Email
{
    public class EmailSender : IEmailSendService
    {
        private IConfiguration config;

        public EmailSender(IConfiguration config)
        {
            this.config = config;
        }

        public bool SendEmail(string email, string subject, string message)
        {
            try
            {
                var client = new SmtpClient("smtp.gmail.com")
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(config["EmailService:Address"], config["EmailService:Password"]),
                    Timeout = 2000,

                };

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(config["EmailService:Address"]);
                mailMessage.To.Add(email);
                mailMessage.Body = message;
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = true;

                string result = string.Empty;

                client.SendAsync(mailMessage, result);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
