namespace PMG_s_Game_Repo.Services
{
    public class LlmOptions
    {
        public string BaseUrl { get; set; } = "http://localhost:1234";
        public string Model { get; set; } = "mistral-7b-instruct";
        public string? ApiKey { get; set; } = "lm - studio";
        public int TimeoutSeconds { get; set; } = 90;
    }
}
