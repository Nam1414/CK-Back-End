using System.Net;
using System.Net.Mail;

namespace OrderManagementAPI.Services
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

            var mail = "smtp.gmail.com";
            var port = 587;
            var senderEmail = emailSettings["SenderEmail"];
            var password = emailSettings["Password"];
            var senderName = emailSettings["SenderName"];

            var client = new SmtpClient(mail, port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(senderEmail, password)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail!, senderName),
                Subject = subject,
                Body = message,
                IsBodyHtml = true // Cho phép gửi HTML đẹp
            };

            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
        }
    }
}