using Blogy.Business.DTOs.BlogDtos;
using Blogy.Business.DTOs.CategoryDtos;
using Blogy.Business.DTOs.CommentDtos;
using Blogy.Business.Services.BlogServices;
using Blogy.Business.Services.CategoryServices;
using Blogy.Business.Services.CommentServices;
using Blogy.Business.Services.ToxicityServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blogy.WebUI.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly ICategoryService _categoryService;
        private readonly ICommentService _commentService;
        private readonly IToxicityService _toxicityService;

        public BlogController(
            IBlogService blogService,
            ICategoryService categoryService,
            ICommentService commentService,
            IToxicityService toxicityService)
        {
            _blogService = blogService;
            _categoryService = categoryService;
            _commentService = commentService;
            _toxicityService = toxicityService;
        }

        // ----------------------------------------------------------
        // TÜM BLOG LİSTESİ
        // ----------------------------------------------------------
        public async Task<IActionResult> Index()
        {
            var blogs = await _blogService.GetAllAsync();
            return View(blogs);
        }

        // ----------------------------------------------------------
        // KATEGORİYE GÖRE BLOG LİSTESİ
        // ----------------------------------------------------------
        public async Task<IActionResult> GetBlogsByCategory(int id)
        {
            var categories = await _categoryService.GetAllAsync();
            var category = categories.FirstOrDefault(x => x.Id == id);

            if (category == null)
                return NotFound();

            ViewBag.CategoryName = category.Name;

            var blogs = await _blogService.GetBlogsByCategoryIdAsync(id);
            return View(blogs);
        }

        // ----------------------------------------------------------
        // BLOG DETAY
        // ----------------------------------------------------------
        public async Task<IActionResult> Detail(int id)
        {
            var blog = await _blogService.GetSingleByIdAsync(id);

            if (blog == null)
                return NotFound();

            return View(blog);
        }

        // ----------------------------------------------------------
        // YORUM EKLEME (AI Toxicity dahil)
        // ----------------------------------------------------------
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment(CreateCommentDto dto)
        {
            if (!ModelState.IsValid)
                return Redirect("/Blog/Detail/" + dto.BlogId);

            // Toxicity hesapla
            var toxicity = await _toxicityService.CheckToxicityAsync(dto.Content);
            dto.IsToxic = toxicity > 0.5m; // DTO’ya yazıyoruz

            // UserId INT → Claim'ten alıyoruz
            var userIdString = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            dto.UserId = int.Parse(userIdString);

            // CreatedDate DTO içinde olduğundan burada ekliyoruz
            dto.CreatedDate = DateTime.Now;

            // Artık tek parametreli CreateAsync çalışır
            await _commentService.CreateAsync(dto);

            return Redirect("/Blog/Detail/" + dto.BlogId);
        }

    }
}
