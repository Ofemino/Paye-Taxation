using System.Net.Http.Json;
using System.Net.Http.Headers;
using Paye.Client.DTOs;
using System.Text.Json;

namespace Paye.Client.Services
{
    public class ApiClient
    {
        private readonly HttpClient _http;
        private string? _token;

        public ApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", new LoginDto { UserName = username, Password = password });
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();
                _token = result?.Data?.Token;
                if (!string.IsNullOrEmpty(_token))
                {
                    _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> SubmitTaxReliefAsync(TaxReliefSubmissionDto submission)
        {
            var response = await _http.PostAsJsonAsync("api/tax-relief-submissions", submission);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<TaxReliefSubmissionDto>> GetReportsAsync(string? staffId, string? branch, string? state, int? taxYear)
        {
            var query = new List<string>();
            if (!string.IsNullOrEmpty(staffId)) query.Add($"staffId={staffId}");
            if (!string.IsNullOrEmpty(branch)) query.Add($"branch={branch}");
            if (!string.IsNullOrEmpty(state)) query.Add($"state={state}");
            if (taxYear.HasValue) query.Add($"taxYear={taxYear}");

            var queryString = query.Any() ? "?" + string.Join("&", query) : "";
            var response = await _http.GetAsync($"api/reporting{queryString}");
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<TaxReliefSubmissionDto>>>();
                return result?.Data ?? new List<TaxReliefSubmissionDto>();
            }
            return new List<TaxReliefSubmissionDto>();
        }

        public async Task<string?> UploadDocumentAsync(MultipartFormDataContent content)
        {
            var response = await _http.PostAsync("api/documents/upload", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<JsonElement>>();
                if (result?.Data.ValueKind == JsonValueKind.Object && result.Data.TryGetProperty("path", out var filePath))
                {
                    return filePath.GetString();
                }
            }
            return null;
        }
    }
}
