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
                // Önce Türkçeyi İngilizceye çevir
                var language = await _aiContentService.DetectLanguageAsync(text);
                var englishText = language != "en"
                    ? await _aiContentService.TranslateToEnglishAsync(text, language)
                    : text;

                var client = _httpClientFactory.CreateClient();
                var hfToken = _configuration["HuggingFace:ApiKey"];

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {hfToken}");

                var request = new
                {
                    inputs = englishText
                };

                var response = await client.PostAsJsonAsync(
                    "https://api-inference.huggingface.co/models/unitary/toxic-bert",
                    request
                );

                if (!response.IsSuccessStatusCode)
                    return 0;

                var json = await response.Content.ReadFromJsonAsync<JsonElement>();

                // Toxic label score'unu bul
                decimal toxicScore = 0;

                if (json.ValueKind == JsonValueKind.Array && json.GetArrayLength() > 0)
                {
                    var innerArray = json[0];
                    if (innerArray.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var item in innerArray.EnumerateArray())
                        {
                            var label = item.GetProperty("label").GetString();
                            var score = item.GetProperty("score").GetDecimal();

                            if (label?.Equals("toxic", StringComparison.OrdinalIgnoreCase) == true)
                            {
                                toxicScore = score;
                                break;
                            }
                        }
                    }
                }

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
