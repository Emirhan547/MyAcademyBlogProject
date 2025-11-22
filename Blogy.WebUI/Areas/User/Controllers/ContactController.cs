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

        public IActionResult Index()
        {
            return View(new CreateContactMessageDto());
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateContactMessageDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var id = await _contactService.SendMessageAsync(dto);
            return RedirectToAction("Success", new { id });
        }

        public async Task<IActionResult> Success(int id)
        {
            var msg = await _contactService.GetMessageByIdAsync(id);
            return View(msg); // DTO
        }

    }
}