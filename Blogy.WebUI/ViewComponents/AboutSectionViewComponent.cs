using Blogy.Business.Services.AboutServices;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.ViewComponents
{
    public class AboutSectionViewComponent : ViewComponent
    {
        private readonly IAboutService _aboutService;

        public AboutSectionViewComponent(IAboutService aboutService)
        {
            _aboutService = aboutService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var about = await _aboutService.GetSingleByIdAsync(1);

            if (about == null)
                return View("Default", "Hakkımızda içeriği henüz eklenmemiş.");

            return View("Default", about.Content);
        }

    }
}
