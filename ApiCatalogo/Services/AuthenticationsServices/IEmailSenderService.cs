
namespace MainBlog.Services.AuthenticationsServices
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}
