using Blogy.Business.Services.BlogServices;
using Blogy.Business.Services.CategoryServices;
using Blogy.Business.Services.CommentServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController(IBlogService _blogService, ICategoryService _categoryService,ICommentService _commentService) : Controller
    {

        public async Task<IActionResult> Index()
        {
            var blogs = await _blogService.GetAllAsync();
            var categories = await _categoryService.GetAllAsync();
            var comments = await _commentService.GetAllAsync();

            ViewBag.TotalBlogs = blogs.Count;
            ViewBag.TotalCategories = categories.Count;
            ViewBag.TodayBlogs = blogs.Count(x => x.CreatedDate.Date == DateTime.Now.Date);
            ViewBag.TotalComments = comments.Count;  // ⭐ Yeni eklenen alan

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetBlogsByCategory()
        {
            var categories = await _categoryService.GetCategoriesWithBlogsAsync();

            var labels = categories.Select(x => x.Name).ToList();
            var values = categories.Select(x => x.Blogs.Count).ToList();

            return Json(new { labels, values });
        }
        [HttpGet]
        public async Task<IActionResult> GetBlogsLast7Days()
        {
            var blogs = await _blogService.GetAllAsync();

            var dates = Enumerable.Range(0, 7)
                    .Select(i => DateTime.Now.Date.AddDays(-i))
                    .OrderBy(d => d)
                    .ToList();

            var labels = dates.Select(d => d.ToString("dd MMM")).ToList();
            var values = dates.Select(d => blogs.Count(b => b.CreatedDate.Date == d)).ToList();

            return Json(new { labels, values });
        }
    }
}
