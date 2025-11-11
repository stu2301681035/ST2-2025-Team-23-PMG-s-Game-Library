// Services/AIService.cs
using System.Text;
using System.Text.Json;

namespace PMG_s_Game_Repo.Services
{
    public class AIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _aiServerUrl;

        public AIService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _aiServerUrl = configuration["AIServer:BaseUrl"] ?? "http://localhost:1234";
        }

        public async Task<string> SendChatQueryAsync(string userMessage)
        {
            try
            {
                var request = new
                {
                    messages = new[]
                    {
                        new { role = "user", content = userMessage }
                    },
                    temperature = 0.2,
                    max_tokens = 512
                };

                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Use the plain text endpoint from your modified Python server
                var response = await _httpClient.PostAsync($"{_aiServerUrl}/api/chat", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return $"Error: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                return $"Connection error: {ex.Message}";
            }
        }
    }
}