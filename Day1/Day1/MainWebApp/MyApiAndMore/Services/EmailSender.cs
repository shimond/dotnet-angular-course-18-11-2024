using Microsoft.Extensions.Options;
using MyApiAndMore.Models.Config;

namespace MyApiAndMore.Services
{
    public interface IEmailSender
    {
        Task<string> SendEmail();
    }

    public class EmailSender : IEmailSender
    {
        private readonly SmtpConfig _config;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IOptionsSnapshot<SmtpConfig> config, ILogger<EmailSender> logger)
        {
            _config = config.Value;
            _logger = logger;
        }

        public async Task<string> SendEmail()
        {
            _logger.LogInformation("Config values ", this._config);
            return _config.HostName;

        }

    }
}
