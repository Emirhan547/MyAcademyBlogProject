using Blogy.Business.Services.BlogServices;
using Blogy.Business.Services.CategoryServices;
using Blogy.Business.Services.AboutServices;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Controllers
{
    public class DefaultController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly ICategoryService _categoryService;
        private readonly IAboutService _aboutService;

        public DefaultController(IBlogService blogService, ICategoryService categoryService, IAboutService aboutService)
        {
            _blogService = blogService;
            _categoryService = categoryService;
            _aboutService = aboutService;
        }

        public async Task<IActionResult> Index()
        {
            var blogs = await _blogService.GetLast3BlogsAsync();
            var categories = await _categoryService.GetCategoriesWithBlogsAsync();

            // Veritabanından about çekiyoruz
            var about = (await _aboutService.GetAllAsync()).FirstOrDefault();
            ViewBag.About = about?.Content ?? "Hakkımızda bilgisi henüz eklenmemiştir.";

            ViewBag.Categories = categories;

            return View(blogs);
        }
    }
}
