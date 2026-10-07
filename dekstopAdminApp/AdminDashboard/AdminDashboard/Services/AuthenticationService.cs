using System.Net.Http.Json;
using AdminDashboard.Models;
using System.Text;

namespace AdminDashboard.Services
{
    public class AuthenticationService
    {
        private readonly HttpClient _httpClient;

        public AuthenticationService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(ApiConfig.BaseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<AuthResponse> LoginAsync(string email, string password)
        {
            try
            {
                var request = new AuthRequest
                {
                    Email = email,
                    Password = password
                };

                // Log request details
                var json = System.Text.Json.JsonSerializer.Serialize(request);
                Console.WriteLine($"Request URL: {_httpClient.BaseAddress}/auth/login");
                Console.WriteLine($"Request body: {json}");

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/auth/login", content);

                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response status code: {response.StatusCode}");
                Console.WriteLine($"Response content: {responseContent}");

                response.EnsureSuccessStatusCode();

                var authResponse = System.Text.Json.JsonSerializer.Deserialize<AuthResponse>(responseContent);
                if (authResponse == null)
                {
                    throw new Exception("Failed to deserialize response");
                }

                return authResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                throw;
            }
        }
    }
}