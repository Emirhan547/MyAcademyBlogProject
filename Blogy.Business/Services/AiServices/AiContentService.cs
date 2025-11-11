using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Blogy.Business.Services.AiServices
{
    public class AiContentService : IAiContentService
    {
        private readonly string _apiKey;
        private readonly IHttpClientFactory _httpClientFactory;

        public AiContentService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _apiKey = configuration["OpenAI:ApiKey"]
                ?? throw new ArgumentNullException("OpenAI API key is missing in configuration.");
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient CreateClient()
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            return client;
        }

        public async Task<string> GenerateArticleAsync(string keywords, string prompt)
        {
            var client = CreateClient();

            var request = new
            {
                model = "gpt-4",
                messages = new[]
                {
                    new { role = "system", content = "You are a professional blog writer. Write exactly 1000 characters article." },
                    new { role = "user", content = $"Write article about: {keywords}. Additional context: {prompt}" }
                },
                max_tokens = 500,
                temperature = 0.7
            };

            var response = await client.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<JsonElement>();
            return result.TryGetProperty("choices", out var choices)
                ? choices[0].GetProperty("message").GetProperty("content").GetString() ?? "Article generation failed"
                : "Article generation failed";
        }

        public async Task<string> GenerateAboutTextAsync()
        {
            var prompt = "Write a professional about section for a blog platform called 'Blogy'. Keep it around 300 characters.";
            return await GenerateArticleAsync("", prompt);
        }

        public async Task<string> GenerateAutoReplyAsync(string message, string detectedLanguage)
        {
            var client = CreateClient();

            var request = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = $"You are a helpful support assistant. Reply in {detectedLanguage}." },
                    new { role = "user", content = message }
                },
                max_tokens = 200,
                temperature = 0.7
            };

            var response = await client.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<JsonElement>();
            return result.TryGetProperty("choices", out var choices)
                ? choices[0].GetProperty("message").GetProperty("content").GetString() ?? "Reply generation failed"
                : "Reply generation failed";
        }

        public async Task<string> DetectLanguageAsync(string text)
        {
            var client = _httpClientFactory.CreateClient();
            var content = new StringContent($"text={Uri.EscapeDataString(text)}",
                Encoding.UTF8, "application/x-www-form-urlencoded");

            try
            {
                var response = await client.PostAsync("https://ws.detectlanguage.com/0.2/detect", content);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<JsonElement>();
                if (result.TryGetProperty("detections", out var detections) && detections.GetArrayLength() > 0)
                    return detections[0].GetProperty("language").GetString() ?? "en";

                return "en";
            }
            catch
            {
                return "en";
            }
        }

        public async Task<string> TranslateToEnglishAsync(string text, string sourceLanguage)
        {
            if (sourceLanguage == "en") return text;

            var client = CreateClient();

            var request = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = "You are a translator. Translate to English only." },
                    new { role = "user", content = $"Translate from {sourceLanguage}: {text}" }
                },
                max_tokens = 200
            };

            var response = await client.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<JsonElement>();
            return result.TryGetProperty("choices", out var choices)
                ? choices[0].GetProperty("message").GetProperty("content").GetString() ?? text
                : text;
        }
    }
}
