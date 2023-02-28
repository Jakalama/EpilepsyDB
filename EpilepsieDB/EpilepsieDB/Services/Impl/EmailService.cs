using EpilepsieDB.Settings;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace EpilepsieDB.Services.Impl
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public Task SendConfirmationMail(string email, string confirmationLink)
        {
            return SendEmailAsync(email, "Invite to EpilepsieDB", $"Please confirm with this link <a href=\"{confirmationLink}\">.</a>");
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            if (_emailSettings.Disabled)
                return Task.CompletedTask;

            SmtpClient client = new SmtpClient
            {
                Port = _emailSettings.MailPort,
                Host = _emailSettings.MailServer,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.Password)
            };

            var message = new MailMessage(_emailSettings.SenderEmail, email)
            {
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            return client.SendMailAsync(message);
        }
    }
}
