using Blogy.Business.Services.AiServices;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;

namespace Blogy.Business.Services.ToxicityServices
{
    public class ToxicityService : IToxicityService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IAiContentService _aiContentService;

        public ToxicityService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IAiContentService aiContentService)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _aiContentService = aiContentService;
        }

        public async Task<decimal> CheckToxicityAsync(string text)
        {
            try
            {
                var language = await _aiContentService.DetectLanguageAsync(text);
                var englishText = language != "en"
                    ? await _aiContentService.TranslateToEnglishAsync(text, language)
                    : text;

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization",
                    $"Bearer {_configuration["HuggingFace:ApiKey"]}");

                var request = new { inputs = englishText };

                var response = await client.PostAsJsonAsync(
                    "https://router.huggingface.co/hf-inference/models/unitary/toxic-bert",
                    request);

                var str = await response.Content.ReadAsStringAsync();

                var json = JsonDocument.Parse(str).RootElement;

                decimal toxicScore = 0;

                // --------------------------
                // FORMAT 1: nested array
                // --------------------------
                if (json.ValueKind == JsonValueKind.Array &&
                    json[0].ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in json[0].EnumerateArray())
                    {
                        if (item.TryGetProperty("label", out var labelProp) &&
                            item.GetProperty("label").GetString() == "toxic")
                        {
                            toxicScore = item.GetProperty("score").GetDecimal();
                            return toxicScore;
                        }
                    }
                }

                // --------------------------
                // FORMAT 2: entity/score
                // --------------------------
                if (json.ValueKind == JsonValueKind.Array &&
                    json[0].ValueKind == JsonValueKind.Object &&
                    json[0].TryGetProperty("entity", out _))
                {
                    foreach (var item in json.EnumerateArray())
                    {
                        var entity = item.GetProperty("entity").GetString();
                        var score = item.GetProperty("score").GetDecimal();

                        if (entity == "toxic")
                        {
                            toxicScore = score;
                            return toxicScore;
                        }
                    }
                }

                // --------------------------
                // FORMAT 3: error response
                // --------------------------
                if (json.TryGetProperty("error", out _))
                    return 0;

                return toxicScore;
            }
            catch
            {
                return 0;
            }
        }



        public async Task<bool> IsToxicAsync(string text, decimal threshold = 0.50m)
        {
            var score = await CheckToxicityAsync(text);
            return score > threshold;
        }
    }
}
