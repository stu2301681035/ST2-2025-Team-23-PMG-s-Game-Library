using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PMG_s_Game_Repo.Services
{
    public class RawgService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public RawgService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["RawgApi:ApiKey"] ?? throw new InvalidOperationException("RAWG API key not configured");
        }

        public async Task<int> GetTotalGameCountAsync()
        {
            string url = $"https://api.rawg.io/api/games?key={_apiKey}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            // Extract the "count" property from the JSON
            if (doc.RootElement.TryGetProperty("count", out var countElement))
                return countElement.GetInt32();

            return 0;
        }

        // Add this method to the RawgService class to fix CS1061
        public async Task<RawgGameDetailsDto> GetGameByIdAsync(int gameId)
        {
            var response = await _httpClient.GetAsync($"https://api.rawg.io/api/games/{gameId}?key={_apiKey}");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var game = System.Text.Json.JsonSerializer.Deserialize<RawgGameDetailsDto>(json, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return game;
        }
    }
}
