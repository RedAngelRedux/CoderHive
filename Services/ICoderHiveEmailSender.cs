using Microsoft.AspNetCore.Identity.UI.Services;

namespace CoderHive.Services
{
    public interface ICoderHiveEmailSender : IEmailSender
    {
        public Task SendContactEmailAsync(string emailFrom, string name, string subject, string htmlBody);
    }
}
