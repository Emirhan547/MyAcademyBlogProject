using Blogy.Business.DTOs.BlogDtos;
using Blogy.Business.DTOs.CategoryDtos;
using Blogy.Business.DTOs.CommentDtos;
using Blogy.Business.Services.BlogServices;
using Blogy.Business.Services.CategoryServices;
using Blogy.Business.Services.CommentServices;
using Blogy.Business.Services.ToxicityServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;
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
        public async Task<IActionResult> Index(int page = 1, int pageSize = 6)
        {
            var blogs = await _blogService.GetBlogsWithCategoriesAsync();
            var pagedBlogs = new PagedList<ResultBlogDto>(blogs.AsQueryable(), page, pageSize);
            return View(pagedBlogs);
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
        [Authorize(Roles = "User,Writer")]
        [HttpPost]
        public async Task<IActionResult> AddComment(CreateCommentDto dto)
        {
            if (!ModelState.IsValid)
                return Redirect("/Blog/Detail/" + dto.BlogId);

            // 1) Toksiklik skorunu al
            var score = await _toxicityService.CheckToxicityAsync(dto.Content);

            // 2) Eşik üstüyse KAYDETME, kullanıcıyı uyar
            if (score > 0.50m)
            {
                TempData["CommentError"] =
                    $"Yorumunuz toksik olarak algılandı (oran: {score:F2}). " +
                    "Lütfen daha uygun bir dil kullanın.";

                return Redirect("/Blog/Detail/" + dto.BlogId);
            }

            // 3) Buraya gelmişse toksik değildir → IsToxic = false
            dto.IsToxic = false;

            // 4) UserId’yi claim’den al
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            dto.UserId = int.Parse(userIdString!);

            // 5) CreatedDate DTO’da varsa doldur
            dto.CreatedDate = DateTime.Now;

            // 6) Yorumu kaydet
            await _commentService.CreateAsync(dto);

            TempData["CommentSuccess"] = "Yorumunuz başarıyla kaydedildi.";
            return Redirect("/Blog/Detail/" + dto.BlogId);
        }

    }
}
