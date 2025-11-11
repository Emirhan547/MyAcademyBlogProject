using Blogy.Business.Services.AiServices;
using Blogy.DataAccess.Repositories.ContactRepositories;
using Blogy.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogy.Business.Services.ContactServices
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IAiContentService _aiContentService;

        public ContactService(IContactRepository contactRepository,
            IAiContentService aiContentService)
        {
            _contactRepository = contactRepository;
            _aiContentService = aiContentService;
        }

        public async Task<int> SendMessageAsync(ContactMessage message)
        {
            // Dili tespit et
            var language = await _aiContentService.DetectLanguageAsync(message.Message);
            message.DetectedLanguage = language;

            // Otomatik yanıt oluştur
            var autoReply = await _aiContentService.GenerateAutoReplyAsync(
                message.Message,
                language
            );
            message.AutoReply = autoReply;
            message.IsReplied = true;
            message.RepliedDate = DateTime.Now;

            await _contactRepository.CreateAsync(message);
            return message.Id;
        }

        public async Task<List<ContactMessage>> GetAllMessagesAsync()
        {
            return await _contactRepository.GetAllAsync();
        }

        public async Task<ContactMessage> GetMessageByIdAsync(int id)
        {
            return await _contactRepository.GetByIdAsync(id);
        }
    }
}
