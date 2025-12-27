using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Paye.Infrastructure.BackgroundJobs
{
    public class NotificationBackgroundService : BackgroundService
    {
        private readonly ILogger<NotificationBackgroundService> _logger;
        public NotificationBackgroundService(ILogger<NotificationBackgroundService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Simulate notification logic
                _logger.LogInformation("[Notification] Submission confirmation sent.");
                _logger.LogInformation("[Notification] Incomplete submission reminder sent.");
                _logger.LogInformation("[Notification] Non-submission alert sent.");
                await Task.Delay(60000, stoppingToken); // Run every 60 seconds
            }
        }
    }
}
