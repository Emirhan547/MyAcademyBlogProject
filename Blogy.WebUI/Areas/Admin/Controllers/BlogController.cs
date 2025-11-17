using Blogy.Business.DTOs.BlogDtos;
using Blogy.Business.Services.BlogServices;
using Blogy.Business.Services.CategoryServices;
using Blogy.Entity.Entities;
using Blogy.WebUI.Const;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

[Area("Admin")]
[Authorize(Roles = $"{Roles.Admin}")]
public class BlogController : Controller
{
    private readonly IBlogService _blogService;
    private readonly ICategoryService _categoryService;
    private readonly UserManager<AppUser> _userManager;

    public BlogController(
        IBlogService blogService,
        ICategoryService categoryService,
        UserManager<AppUser> userManager)
    {
        _blogService = blogService;
        _categoryService = categoryService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var blogs = await _blogService.GetAllAsync();
        return View(blogs);
    }

    public async Task<IActionResult> DeleteBlog(int id)
    {
        await _blogService.DeleteAsync(id);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> UpdateBlog(int id)
    {
        await GetCategoriesAsync();
        var blog = await _blogService.GetByIdAsync(id);
        return View(blog);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateBlog(UpdateBlogDto dto)
    {
        if (!ModelState.IsValid)
        {
            await GetCategoriesAsync();
            return View(dto);
        }

        var user = await _userManager.FindByNameAsync(User.Identity!.Name!);
        dto.WriterId = user.Id;

        await _blogService.UpdateAsync(dto);
        return RedirectToAction("Index");
    }

    private async Task GetCategoriesAsync()
    {
        var categories = await _categoryService.GetAllAsync();
        ViewBag.categories = categories
            .Select(c => new SelectListItem
            {
                Text = c.CategoryName,
                Value = c.Id.ToString()
            }).ToList();
    }
}
