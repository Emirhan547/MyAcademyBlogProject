using AutoMapper;
using Blogy.Business.DTOs.BlogDtos;
using Blogy.Business.Services.AiServices;
using Blogy.Business.Services.BlogServices;
using Blogy.Business.Services.CategoryServices;
using Blogy.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        private async Task LoadCategories()
        {
            var categories = await _categoryService.GetAllAsync();
            ViewBag.categories = categories
                .Select(x => new SelectListItem
                {
                    Text = x.CategoryName,
                    Value = x.Id.ToString()
                }).ToList();
        }

        // Writer kendi bloglarını görür
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var blogs = await _blogService.GetAllAsync();
            blogs = blogs.Where(x => x.WriterId == user.Id).ToList();

            return View(blogs);
        }

        // CREATE GET
        public async Task<IActionResult> Create()
        {
            await LoadCategories();
            return View();
        }

        // CREATE POST
        [HttpPost]
        public async Task<IActionResult> Create(CreateBlogDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategories();
                return View(dto);
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            dto.WriterId = user.Id;

            await _blogService.CreateAsync(dto);
            return RedirectToAction("Index");
        }

        // AI ile blog oluşturma
        [HttpPost]
        public async Task<IActionResult> GenerateBlogWithAi(string keywords, string prompt)
        {
            await LoadCategories();

            var aiText = await _aiService.GenerateArticleAsync(keywords, prompt);

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var dto = new CreateBlogDto
            {
                Title = keywords,
                Description = aiText,
                CategoryId = 0,
                WriterId = user.Id
            };

            return View("Create", dto);
        }

        // UPDATE GET
        public async Task<IActionResult> Update(int id)
        {
            await LoadCategories();
            var blog = await _blogService.GetByIdAsync(id);
            return View(blog);
        }

        // UPDATE POST
        [HttpPost]
        public async Task<IActionResult> Update(UpdateBlogDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategories();
                return View(dto);
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            dto.WriterId = user.Id;

            await _blogService.UpdateAsync(dto);
            return RedirectToAction("Index");
        }

        // DELETE
        public async Task<IActionResult> Delete(int id)
        {
            await _blogService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
