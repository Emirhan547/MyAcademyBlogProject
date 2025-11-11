using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogy.Business.Services.AiServices
{
    public interface IAiContentService
    {
        Task<string> GenerateArticleAsync(string keywords, string prompt);
        Task<string> GenerateAboutTextAsync();
        Task<string> GenerateAutoReplyAsync(string message, string detectedLanguage);
        Task<string> DetectLanguageAsync(string text);
        Task<string> TranslateToEnglishAsync(string text, string sourceLanguage);
    }
}
