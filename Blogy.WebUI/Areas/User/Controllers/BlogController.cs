using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Blogy.Business.Services.BlogServices;
using Blogy.Business.Services.CommentServices;
using Blogy.Business.DTOs.CommentDtos;
using Microsoft.AspNetCore.Identity;
using Blogy.Entity.Entities;

namespace Blogy.WebUI.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly ICommentService _commentService;
        private readonly UserManager<AppUser> _userManager;

        public BlogController(IBlogService blogService,
                              ICommentService commentService,
                              UserManager<AppUser> userManager)
        {
            _blogService = blogService;
            _commentService = commentService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Detail(int id)
        {
            var blog = await _blogService.GetSingleByIdAsync(id);
            return View(blog);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(CreateCommentDto model)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            model.UserId = user.Id;

            await _commentService.CreateAsync(model);
            return Redirect($"/User/Blog/Detail/{model.BlogId}");
        }
    }
}
