using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Blogy.Business.Services.BlogServices;

namespace Blogy.WebUI.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class HomeController : Controller
    {
        private readonly IBlogService _blogService;

        public HomeController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public async Task<IActionResult> Index()
        {
            var blogs = await _blogService.GetBlogsWithCategoriesAsync();
            return View(blogs);
        }
    }
}
