using Blogy.Business.Services.ContactServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _contactService.GetAllMessagesAsync();
            return View(list); // DTO
        }

        public async Task<IActionResult> Detail(int id)
        {
            var msg = await _contactService.GetMessageByIdAsync(id);
            return View(msg); // DTO
        }
    }
}
