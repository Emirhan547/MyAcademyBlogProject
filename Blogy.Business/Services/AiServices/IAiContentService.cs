using System.Threading.Tasks;

namespace Blogy.Business.Services.AiServices
{
    public interface IAiContentService
    {
        Task<string> GenerateAboutTextAsync();
        Task<string> GenerateAutoReplyAsync(string message, string detectedLanguage);
        Task<string> DetectLanguageAsync(string text);
        Task<string> TranslateToEnglishAsync(string text, string sourceLanguage);

        // TÜM BLOG ALANLARINI JSON OLARAK ÜRETİR
        Task<string> GenerateBlogJsonAsync(string keywords, string prompt);
    }
}
