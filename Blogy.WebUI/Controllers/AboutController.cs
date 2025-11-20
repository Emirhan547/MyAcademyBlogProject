using Blogy.Business.DTOs.AboutDtos;
using Blogy.Business.Services.AboutServices;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Controllers
{
    public class AboutController : Controller
    {
        private readonly IAboutService _aboutService;

        public AboutController(IAboutService aboutService)
        {
            _aboutService = aboutService;
        }

        public async Task<IActionResult> Index()
        {
            var about = (await _aboutService.GetAllAsync()).FirstOrDefault();

            return View(about ?? new ResultAboutDto
            {
                Content = "Hakkımızda bilgisi henüz eklenmemiştir."
            });
        }
    }
}
