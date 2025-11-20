using Blogy.Business.DTOs.CommentDtos;
using Blogy.Business.Services.BlogServices;
using Blogy.Business.Services.CommentServices;
using Blogy.Business.Services.ToxicityServices;
using Blogy.Entity.Entities;
using Blogy.WebUI.Const;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blogy.WebUI.Areas.Admin.Controllers
{
    [Area(Roles.Admin)]
    [Authorize(Roles = Roles.Admin)]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IBlogService _blogService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IToxicityService _toxicityService;

        public CommentController(
            ICommentService commentService,
            IBlogService blogService,
            UserManager<AppUser> userManager,
            IToxicityService toxicityService)
        {
            _commentService = commentService;
            _blogService = blogService;
            _userManager = userManager;
            _toxicityService = toxicityService;
        }

        private async Task GetBlogs()
        {
            var blogs = await _blogService.GetAllAsync();
            TempData["blogs"] = blogs
                .Select(x => new SelectListItem
                {
                    Text = x.Title,
                    Value = x.Id.ToString()
                })
                .ToList();
        }

        public async Task<IActionResult> Index()
        {
            var comments = await _commentService.GetAllAsync();
            return View(comments);
        }

        public async Task<IActionResult> CreateComment()
        {
            await GetBlogs();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateCommentDto dto)
        {
            var isToxic = await _toxicityService.IsToxicAsync(dto.Content);

            if (isToxic)
            {
                ModelState.AddModelError("", "Yorum toksik içerik içeriyor.");
                await GetBlogs();
                return View(dto);
            }

            await _commentService.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DeleteComment(int id)
        {
            await _commentService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
