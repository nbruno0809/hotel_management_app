using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text.Unicode;

namespace HotelManagement.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings emailSettings;
        
        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            this.emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string email,string subject,string htmlMessage)
        {
            var client = new SmtpClient(emailSettings.Host,emailSettings.Port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(emailSettings.Email,emailSettings.Password)
            };

            MailAddress from = new MailAddress(emailSettings.Email,emailSettings.Name);
            MailAddress to = new MailAddress(email);

            MailMessage message = new MailMessage(from,to)
            {
                Body = htmlMessage,
                BodyEncoding = System.Text.Encoding.UTF8,
                IsBodyHtml = true,
                Subject = subject,
                SubjectEncoding = System.Text.Encoding.UTF8,
            };

            await client.SendMailAsync(message);
        }
    }
}
