using Blogy.Business.Services.AboutServices;
using Blogy.Business.DTOs.AboutDtos;
using Blogy.Business.Services.AiServices;   // <-- EKLE
using Blogy.WebUI.Const;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Areas.Admin.Controllers
{
    [Area(Roles.Admin)]
    [Authorize(Roles = $"{Roles.Admin}")]
    public class AboutController : Controller
    {
        private readonly IAboutService _aboutService;
        private readonly IAiContentService _aiService;   // <-- EKLE

        public AboutController(IAboutService aboutService, IAiContentService aiService)
        {
            _aboutService = aboutService;
            _aiService = aiService;  // <-- EKLE
        }

        public async Task<IActionResult> Index()
        {
            var values = await _aboutService.GetAllAsync();
            return View(values);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAboutDto dto)
        {
            await _aboutService.CreateAsync(dto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            var value = await _aboutService.GetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateAboutDto dto)
        {
            await _aboutService.UpdateAsync(dto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _aboutService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        // --------------------------------------------------
        //               AI ABOUT GENERATOR
        // --------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> GenerateAiAbout()
        {
            var aboutText = await _aiService.GenerateAboutTextAsync();

            return Json(aboutText);
        }
    }
}
