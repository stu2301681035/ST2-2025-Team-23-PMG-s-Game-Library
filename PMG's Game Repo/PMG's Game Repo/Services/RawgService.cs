using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PMG_s_Game_Repo.Services
{
    public class RawgService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "b64878ea13b24360aaf06d6a2003e30e";

        public RawgService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> GetTotalGameCountAsync()
        {
            string url = $"https://api.rawg.io/api/games?key={"b64878ea13b24360aaf06d6a2003e30e"}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            // Extract the "count" property from the JSON
            if (doc.RootElement.TryGetProperty("count", out var countElement))
                return countElement.GetInt32();

            return 0;
        }
    }
}
