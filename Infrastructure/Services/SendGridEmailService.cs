using System;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Services
{
    public class SendGridEmailService : IEmailService
    {
        private readonly string _apiKey;
        private readonly EmailAddress _from;

        public SendGridEmailService(IConfiguration config)
        {
            // for quick testing only—hard-code your key here
            _apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
                    ?? throw new InvalidOperationException("Missing SENDGRID_API_KEY");

            // you can also hard-code the from address, or still pull from config
            var fromEmail = config["SendGrid:FromEmail"]
                            ?? "jfs18@mail.aub.edu";
            var fromName = config["SendGrid:FromName"]
                            ?? "Event Manager";

            _from = new EmailAddress(fromEmail, fromName);
        }

        public async Task SendEmailAsync(
            string toEmail,
            string subject,
            string plainTextContent,
            string htmlContent = null)
        {
            var client = new SendGridClient(_apiKey);
            var to = new EmailAddress(toEmail);
            var html = htmlContent ?? plainTextContent;
            var msg = MailHelper.CreateSingleEmail(_from, to, subject, plainTextContent, html);
            var response = await client.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"SendGrid failed: {response.StatusCode}");
        }
    }
}
