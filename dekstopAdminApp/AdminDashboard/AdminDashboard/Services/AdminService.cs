using System.Net.Http.Json;
using AdminDashboard.Models;

namespace AdminDashboard.Services
{
    public class AdminService
    {
        private readonly HttpClient _httpClient;

        public AdminService(string token)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(ApiConfig.BaseUrl);
            _httpClient.DefaultRequestHeaders.Clear();

            Console.WriteLine($"Initializing AdminService with token: {token}");

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("No authentication token provided");
            }

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            _httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Post>> GetPendingPostsAsync()
        {
            try
            {
                foreach (var header in _httpClient.DefaultRequestHeaders)
                {
                    Console.WriteLine($"Header: {header.Key} = {string.Join(", ", header.Value)}");
                }

                var response = await _httpClient.GetAsync("/post/pending");
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"GetPendingPosts Response: {(int)response.StatusCode} {response.StatusCode}");
                Console.WriteLine($"Response Content: {content}");

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<Post>>() ?? new List<Post>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPendingPostsAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/admin/users");
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"GetAllUsers Response: {(int)response.StatusCode} {response.StatusCode}");
                Console.WriteLine($"Response Content: {content}");

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<User>>() ?? new List<User>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllUsersAsync: {ex.Message}");
                throw;
            }
        }

        public async Task ApprovePostAsync(int postId)
        {
            var response = await _httpClient.PutAsync($"/admin/posts/{postId}/approve", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task RejectPostAsync(int postId)
        {
            var response = await _httpClient.PutAsync($"/admin/posts/{postId}/reject", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task BanUserAsync(int userId)
        {
            var response = await _httpClient.DeleteAsync($"/admin/users/{userId}");
            response.EnsureSuccessStatusCode();
        }
    }
}