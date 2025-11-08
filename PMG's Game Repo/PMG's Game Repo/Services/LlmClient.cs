using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace PMG_s_Game_Repo.Services
{
    public class LlmClient
    {
        private readonly HttpClient _http;
        private readonly LlmOptions _opt;
        private static readonly JsonSerializerOptions JsonOpts = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        public LlmClient(HttpClient http, IOptions<LlmOptions> opt)
        {
            _http = http;
            _opt = opt.Value;
            if (!string.IsNullOrWhiteSpace(_opt.ApiKey))
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _opt.ApiKey);
        }

        public async Task<string> GetJsonPlanAsync(string userNaturalQuery, string systemPrompt)
        {
            var req = new ChatCompletionsRequest
            {
                Model = _opt.Model,
                Temperature = 0.2,
                Messages = new()
                {
                    new ChatMessage { Role = "system", Content = systemPrompt },
                    new ChatMessage { Role = "user", Content = userNaturalQuery }
                }
            };

            using var content = new StringContent(JsonSerializer.Serialize(req, JsonOpts), Encoding.UTF8, "application/json");

            // Many OpenAI-compatible local servers support /v1/chat/completions
            var url = "/v1/chat/completions"; // if your server is different adjust
            var resp = await _http.PostAsync(url, content);
            if (!resp.IsSuccessStatusCode)
            {
                var body = await resp.Content.ReadAsStringAsync();
                throw new HttpRequestException($"LLM HTTP {(int)resp.StatusCode}: {body}");
            }

            using var stream = await resp.Content.ReadAsStreamAsync();
            var data = await JsonSerializer.DeserializeAsync<ChatCompletionsResponse>(stream, JsonOpts);
            var raw = data?.Choices?.FirstOrDefault()?.Message?.Content ?? string.Empty;
            return ExtractJson(raw);
        }

        // strip surrounding ```json / text and other chatter — return the JSON object substring
        private static string ExtractJson(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "{}";
            int start = s.IndexOf('{');
            int end = s.LastIndexOf('}');
            return (start >= 0 && end > start)
                ? s.Substring(start, end - start + 1).Trim()
                : "{}";
        }
    }
}
