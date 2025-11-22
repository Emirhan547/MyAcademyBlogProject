using AutoMapper;
using Blogy.Business.DTOs.ContactDtos;
using Blogy.DataAccess.Repositories.ContactRepositories;
using Blogy.Business.Services.AiServices;
using Blogy.Entity.Entities;

namespace Blogy.Business.Services.ContactServices
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IAiContentService _aiService;
        private readonly IMapper _mapper;

        public ContactService(IContactRepository contactRepository, IAiContentService aiService, IMapper mapper)
        {
            _contactRepository = contactRepository;
            _aiService = aiService;
            _mapper = mapper;
        }

        public async Task<int> SendMessageAsync(CreateContactMessageDto dto)
        {
            var entity = _mapper.Map<ContactMessage>(dto);

            // AI işlemleri
            entity.DetectedLanguage = await _aiService.DetectLanguageAsync(dto.Message);
            entity.AutoReply = await _aiService.GenerateAutoReplyAsync(dto.Message, entity.DetectedLanguage);

            entity.IsReplied = true;
            entity.RepliedDate = DateTime.Now;
            entity.CreatedDate = DateTime.Now;

            await _contactRepository.CreateAsync(entity);

            return entity.Id;
        }

        public async Task<List<ResultContactMessageDto>> GetAllMessagesAsync()
        {
            var data = await _contactRepository.GetAllAsync();
            return _mapper.Map<List<ResultContactMessageDto>>(data);
        }

        public async Task<GetContactMessageDetailDto> GetMessageByIdAsync(int id)
        {
            var entity = await _contactRepository.GetByIdAsync(id);
            return _mapper.Map<GetContactMessageDetailDto>(entity);
        }
    }
}
