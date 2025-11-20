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
@"Kesinlikle ve sadece şu formatta geçerli bir JSON üret:

{
  ""Title"": ""Blog Başlığı"",
  ""Description"": ""Blog yazısı (tam olarak 1000 karakter, ne eksik ne fazla)""
}

Açıklama kısmı düz metin olmalı.  
Tam olarak 1000 karakter üret.  
Karakter sayısı boşluklar dahil olacak.  
JSON dışında asla başka açıklama ekleme."
            },
            new
            {
                role = "user",
                content = $"Konu: {keywords}\nDetay: {prompt}\n\n1000 karakterlik blog yazısı üret."
            }
        },
                temperature = 0.4,
                max_tokens = 2000
            };

            var response = await client.PostAsJsonAsync(
                "https://api.openai.com/v1/chat/completions", request);

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var parsed = JsonDocument.Parse(jsonResponse);

            string content = null;

            // 1) message.content
            if (parsed.RootElement
                .GetProperty("choices")[0]
                .TryGetProperty("message", out var msg)
                && msg.TryGetProperty("content", out var msgContent))
            {
                content = msgContent.GetString();
            }

            // 2) delta.content (streaming modellerde)
            if (content == null &&
                parsed.RootElement
                    .GetProperty("choices")[0]
                    .TryGetProperty("delta", out var delta)
                    && delta.TryGetProperty("content", out var deltaContent))
            {
                content = deltaContent.GetString();
            }

            return content ?? jsonResponse;
        }

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
                var client = CreateClient();

                var request = new
                {
                    model = "gpt-4o-mini",
                    messages = new[]
                    {
                new
                {
                    role = "system",
                    content = "Detect the language of the user text. Respond ONLY with the ISO code such as: tr, en, fr, de, ar, es."
                },
                new
                {
                    role = "user",
                    content = text
                }
            },
                    temperature = 0
                };

                var response = await client.PostAsJsonAsync(
                    "https://api.openai.com/v1/chat/completions",
                    request);

                var json = await response.Content.ReadAsStringAsync();
                var parsed = JsonSerializer.Deserialize<JsonElement>(json);

                var lang = parsed
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString()
                    ?.Trim()
                    ?.ToLower();

                return lang ?? "en";
            }
            catch
            {
                return "en";
            }
        }

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
