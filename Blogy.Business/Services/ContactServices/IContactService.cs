using Blogy.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogy.Business.Services.ContactServices
{
    public interface IContactService
    {
        Task<int> SendMessageAsync(ContactMessage message);
        Task<List<ContactMessage>> GetAllMessagesAsync();
        Task<ContactMessage> GetMessageByIdAsync(int id);
    }
}
