using System.Net.Mail;
using System.Net;

namespace MainBlog.Services.AuthenticationsServices
{
    public class EmailSenderService : IEmailSenderService
    {

        private readonly IConfiguration _configuration;

        public EmailSenderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var smtpSettings = _configuration.GetSection("SMTP");

            if (!int.TryParse(smtpSettings["Port"], out var port))
            {
                throw new ArgumentException("O valor da chave 'Port' no SMTP está inválido ou ausente.");
            }

            using var client = new SmtpClient(smtpSettings["Host"])
            {
                Port = 587,
                Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpSettings["SenderEmail"]),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);
            await client.SendMailAsync(mailMessage);
        }
    }
}
