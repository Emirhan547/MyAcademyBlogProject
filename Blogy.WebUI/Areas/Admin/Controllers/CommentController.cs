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
    public class CommentController(ICommentService _commentService,
                                    IBlogService _blogService,
                                    UserManager<AppUser> _userManager,IToxicityService _toxicityService) : Controller
    {
        private async Task GetBlogs()
        {
            var blogs = await _blogService.GetAllAsync();
            TempData["blogs"] = (from blog in blogs
                                 select new SelectListItem
                                 {
                                     Text = blog.Title,
                                     Value = blog.Id.ToString()
                                 }).ToList();
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
        public async Task<IActionResult> CreateComment(CreateCommentDto createCommentDto)
        {
            // Toksiklik kontrolü
            var isToxic = await _toxicityService.IsToxicAsync(createCommentDto.Content);

            if (isToxic)
            {
                ModelState.AddModelError("", "Yorumunuz uygun olmayan içerik içermektedir.");
                return View();
            }

            await _commentService.CreateAsync(createCommentDto);
            return RedirectToAction(nameof(Index));
        }
    }
}