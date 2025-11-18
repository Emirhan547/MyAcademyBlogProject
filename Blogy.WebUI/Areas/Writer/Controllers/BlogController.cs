using AutoMapper;
using Blogy.Business.DTOs.AiDtos;
using Blogy.Business.DTOs.BlogDtos;
using Blogy.Business.Services.AiServices;
using Blogy.Business.Services.BlogServices;
using Blogy.Business.Services.CategoryServices;
using Blogy.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PagedList.Core;
using System.Text.Json;

namespace Blogy.WebUI.Areas.Writer.Controllers
{
    [Area("Writer")]
    [Authorize(Roles = "Writer")]
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly ICategoryService _categoryService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAiContentService _aiService;

        public BlogController(
            IBlogService blogService,
            ICategoryService categoryService,
            UserManager<AppUser> userManager,
            IAiContentService aiService)
        {
            _blogService = blogService;
            _categoryService = categoryService;
            _userManager = userManager;
            _aiService = aiService;
        }

        private async Task GetCategoriesAsync()
        {
            var categories = await _categoryService.GetAllAsync();
            ViewBag.categories = categories
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList();
        }


        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var blogs = await _blogService.GetAllAsync();
            blogs = blogs.Where(x => x.WriterId == user.Id).ToList();

            var pagedBlogs = new PagedList<ResultBlogDto>(blogs.AsQueryable(), page, pageSize);
            return View(pagedBlogs);
        }


        public async Task<IActionResult> Create()
        {
            await GetCategoriesAsync();
            return View(new CreateBlogDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBlogDto dto)
        {
            if (!ModelState.IsValid)
            {
                await GetCategoriesAsync();
                return View(dto);
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            dto.WriterId = user.Id;

            await _blogService.CreateAsync(dto);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> GenerateBlogWithAi(string keywords, string prompt)
        {
            var jsonString = await _aiService.GenerateBlogJsonAsync(keywords, prompt);

            try
            {
                var data = JsonSerializer.Deserialize<AiBlogJson>(jsonString);

                if (data == null)
                    return Json(new { error = "AI JSON boş döndü." });

                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    error = "JSON parse hatası",
                    detail = ex.Message,
                    raw = jsonString
                });
            }
        }
    }
}
