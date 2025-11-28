using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace ProductAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var emailSettings = _config.GetSection("EmailSettings");

            var senderEmail = emailSettings["SenderEmail"];
            var password = emailSettings["Password"];
            var senderName = emailSettings["SenderName"];

            // ✅ CHECK AN TOÀN – KHÔNG CONFIG THÌ KHÔNG CRASH
            if (string.IsNullOrWhiteSpace(senderEmail) || string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Email service is not configured. Please set EmailSettings in appsettings.json.");
            }

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(senderEmail, password)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail, senderName),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
        }
    }
}
