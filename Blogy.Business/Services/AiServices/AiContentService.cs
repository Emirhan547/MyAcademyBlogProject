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
                ?? throw new ArgumentNullException("OpenAI API key is missing.");
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient CreateClient()
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            return client;
        }

        // -------------------------------------------------------
        // AI - BLOG JSON ÜRET (SADELEŞTİRİLMİŞ)
        // -------------------------------------------------------
        public async Task<string> GenerateBlogJsonAsync(string keywords, string prompt)
        {
            var client = CreateClient();

            var request = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new
                    {
                        role = "system",
                        content =
@"Sadece şu formatta GEÇERLİ bir JSON üret:

{
  ""title"": ""Başlık"",
  ""description"": ""Blog açıklaması""
}

Başka hiçbir açıklama, metin, yorum veya işaret ekleme.
Sadece JSON döndür."
                    },
                    new
                    {
                        role = "user",
                        content = $"Konu: {keywords}\nDetay: {prompt}"
                    }
                },
                max_tokens = 800,
                temperature = 0.4
            };

            var response = await client.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", request);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            try
            {
                var parsed = JsonSerializer.Deserialize<JsonElement>(jsonResponse);

                return parsed.GetProperty("choices")[0]
                             .GetProperty("message")
                             .GetProperty("content")
                             .GetString() ?? "";
            }
            catch
            {
                return "";
            }
        }

        // -------------------------------------------------------
        // ABOUT (istersen yine AI üzerinden üret)
        // -------------------------------------------------------
        public async Task<string> GenerateAboutTextAsync()
        {
            return await GenerateBlogJsonAsync("Blogy", "300 karakterlik about yazısı üret.") ?? "";
        }

        // -------------------------------------------------------
        // LANGUAGE DETECT
        // -------------------------------------------------------
        public async Task<string> DetectLanguageAsync(string text)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                var content = new StringContent($"text={Uri.EscapeDataString(text)}",
                    Encoding.UTF8, "application/x-www-form-urlencoded");

                var response = await client.PostAsync("https://ws.detectlanguage.com/0.2/detect", content);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadFromJsonAsync<JsonElement>();

                if (json.TryGetProperty("detections", out var detections) &&
                    detections.GetArrayLength() > 0)
                {
                    return detections[0].GetProperty("language").GetString() ?? "en";
                }

                return "en";
            }
            catch
            {
                return "en";
            }
        }

        // -------------------------------------------------------
        // TRANSLATE
        // -------------------------------------------------------
        public async Task<string> TranslateToEnglishAsync(string text, string sourceLanguage)
        {
            if (sourceLanguage == "en")
                return text;

            var client = CreateClient();

            var request = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new { role = "system", content = "Translate only to English." },
                    new { role = "user", content = text }
                }
            };

            var response = await client.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", request);
            var json = await response.Content.ReadAsStringAsync();

            try
            {
                var parsed = JsonSerializer.Deserialize<JsonElement>(json);
                return parsed.GetProperty("choices")[0]
                             .GetProperty("message")
                             .GetProperty("content")
                             .GetString() ?? text;
            }
            catch
            {
                return text;
            }
        }

        // -------------------------------------------------------
        // AUTO REPLY (MESAJ CEVAPLAMA)
        // -------------------------------------------------------
        public async Task<string> GenerateAutoReplyAsync(string message, string detectedLanguage)
        {
            var client = CreateClient();

            var request = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new {
                        role = "system",
                        content = $"You are a helpful assistant. Reply in {detectedLanguage}."
                    },
                    new {
                        role = "user",
                        content = message
                    }
                },
                max_tokens = 200
            };

            var response = await client.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", request);
            var json = await response.Content.ReadAsStringAsync();

            try
            {
                var parsed = JsonSerializer.Deserialize<JsonElement>(json);

                return parsed.GetProperty("choices")[0]
                             .GetProperty("message")
                             .GetProperty("content")
                             .GetString() ?? "";
            }
            catch
            {
                return "Yanıt oluşturulamadı.";
            }
        }
    }
}
