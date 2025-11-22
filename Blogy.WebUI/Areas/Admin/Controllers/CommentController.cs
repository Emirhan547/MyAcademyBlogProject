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

        private readonly UserManager<AppUser> _userManager;


        public CommentController(
            ICommentService commentService,
            IBlogService blogService,
            UserManager<AppUser> userManager,
            IToxicityService toxicityService)
        {
            _commentService = commentService;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            var comments = await _commentService.GetAllAsync();
            return View(comments);
        }

       
        public async Task<IActionResult> DeleteComment(int id)
        {
            await _commentService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
