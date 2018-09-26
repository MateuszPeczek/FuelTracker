using Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace Services.Email
{
    public class EmailSender : IEmailSendService
    {
        private IConfiguration config;
        private ILogger logger;

        public EmailSender(IConfiguration config, ILogger<EmailSender> logger)
        {
            this.config = config;
            this.logger = logger;
        }

        public async Task<bool> SendEmail(string email, string subject, string message)
        {
            try
            {
                var apiKey = config["EmailService:ApiKey"];
                var client = new SendGridClient(apiKey);

                var from = new EmailAddress(config["EmailService:Address"], "Fuel Tracker");
                var to = new EmailAddress(email);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, string.Empty, message);

                var response = await client.SendEmailAsync(msg);

                return response.StatusCode == System.Net.HttpStatusCode.Accepted;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return false;
            }
        }
    }
}
