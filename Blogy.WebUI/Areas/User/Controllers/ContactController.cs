using Blogy.Business.DTOs.ContactDtos;
using Blogy.Business.Services.ContactServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        // GET: /User/Contact
        public IActionResult Index()
        {
            return View(new CreateContactMessageDto());
        }

        // POST: /User/Contact
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CreateContactMessageDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var id = await _contactService.SendMessageAsync(dto);
            return RedirectToAction("Success", new { id });
        }

        // GET: /User/Contact/Success/5
        public async Task<IActionResult> Success(int id)
        {
            var msg = await _contactService.GetMessageByIdAsync(id);

            if (msg == null)
                return RedirectToAction("Index");

            return View(msg);
        }
    }
}
