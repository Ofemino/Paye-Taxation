using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Paye.Infrastructure.Configuration;

namespace Paye.Infrastructure.Services
{
    public interface IExternalAuthProvider
    {
        Task<ExternalAuthResponse?> ValidateUserAsync(string username, string password, CancellationToken cancellationToken);
    }

    public class ExternalAuthResponse
    {
        public bool IsValid { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
    }

    public class ExternalAuthProvider : IExternalAuthProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ExternalAuthSettings _settings;
        private readonly ILogger<ExternalAuthProvider> _logger;

        public ExternalAuthProvider(HttpClient httpClient, IOptions<ExternalAuthSettings> settings, ILogger<ExternalAuthProvider> logger)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _logger = logger;
            
            if (_settings.TimeoutSeconds > 0)
            {
                _httpClient.Timeout = TimeSpan.FromSeconds(_settings.TimeoutSeconds);
            }
            
            if (!string.IsNullOrEmpty(_settings.ApiKey))
            {
                _httpClient.DefaultRequestHeaders.Add("X-Api-Key", _settings.ApiKey);
            }
        }

        public async Task<ExternalAuthResponse?> ValidateUserAsync(string username, string password, CancellationToken cancellationToken)
        {
            if (!_settings.IsEnabled) return null;

            try
            {
                var request = new { Username = username, Password = password };
                var response = await _httpClient.PostAsJsonAsync(_settings.Endpoint, request, cancellationToken);
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ExternalAuthResponse>(cancellationToken: cancellationToken);
                }
                
                _logger.LogWarning("External auth failed. Status: {StatusCode}", response.StatusCode);
                return new ExternalAuthResponse { IsValid = false };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling external authentication endpoint.");
                return new ExternalAuthResponse { IsValid = false };
            }
        }
    }
}
