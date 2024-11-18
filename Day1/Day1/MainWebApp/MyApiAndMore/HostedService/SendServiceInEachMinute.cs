
using Microsoft.Extensions.Options;
using MyApiAndMore.Models.Config;

namespace MyApiAndMore.HostedService
{
    public class SendServiceInEachMinute : BackgroundService
    {
        private readonly IOptionsMonitor<SmtpConfig> _config;

        public SendServiceInEachMinute(IOptionsMonitor<SmtpConfig> config)
        {
            _config = config;
            _config.OnChange(OnConfigChange);
        }

        void OnConfigChange(SmtpConfig updatedConfig, string? source)
        {

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                await Task.Delay(60000);
                var currentConfig = _config.CurrentValue.HostName;
                await Console.Out.WriteLineAsync(currentConfig);
            }
        }
    }
}
