using System.Text.Json.Serialization;

namespace PMG_s_Game_Repo.Services
{
    public class ChatCompletionsRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = "mistral";
        [JsonPropertyName("messages")]
        public List<ChatMessage> Messages { get; set; } = new();
        [JsonPropertyName("temperature")]
        public double? Temperature { get; set; } = 0.2;
    }

    public class ChatMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = "user";
        [JsonPropertyName("content")]
        public string Content { get; set; } = "";
    }

    public class ChatCompletionsResponse
    {
        [JsonPropertyName("choices")]
        public List<ChatChoice> Choices { get; set; } = new();
    }

    public class ChatChoice
    {
        [JsonPropertyName("message")]
        public ChatMessage Message { get; set; } = new();
    }
}
