using Blogy.Business.Services.ContactServices;
using Blogy.Entity.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        public IActionResult Index()
        {
            return View(new ContactMessage());
        }

        [HttpPost]
        public async Task<IActionResult> Index(ContactMessage message)
        {
            if (!ModelState.IsValid)
                return View(message);

            // CreatedDate zaten BaseEntity'de otomatik atanıyor
            await _contactService.SendMessageAsync(message);

            return RedirectToAction("Success", new { id = message.Id });
        }

        public async Task<IActionResult> Success(int id)
        {
            var msg = await _contactService.GetMessageByIdAsync(id);
            return View(msg);
        }
    }
}
