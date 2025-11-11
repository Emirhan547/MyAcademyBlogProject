using AutoMapper;
using Blogy.Business.DTOs.BlogDtos;
using Blogy.Business.Services.BlogServices;
using Blogy.Business.Services.CategoryServices;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;

namespace Blogy.WebUI.Controllers
{
    public class BlogController(IBlogService _blogService, ICategoryService _categoryService, IMapper _mapper) : Controller
    {

        public async Task<IActionResult> Index(int page = 1, int pageSize = 2)
        {
            var blogs = await _blogService.GetAllAsync();

            var values = new PagedList<ResultBlogDto>(blogs.AsQueryable(), page, pageSize);

            return View(values);
        }

        public async Task<IActionResult> GetBlogsByCategory(int id, int page = 1, int pageSize = 2)
        {
            try
            {
                var category = await _categoryService.GetByIdAsync(id);
                if (category == null)
                    return NotFound();

                ViewBag.categoryName = category.Name;
                ViewBag.categoryId = id;

                var blogs = await _blogService.GetBlogsByCategoryIdAsync(id);
                var pagedBlogs = new PagedList<ResultBlogDto>(
                    blogs.AsQueryable(),
                    page,
                    pageSize
                );

                return View(pagedBlogs);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Blog");
            }
        }

        public async Task<IActionResult> BlogDetails(int id)
        {
            try
            {
                var blog = await _blogService.GetSingleByIdAsync(id);
                if (blog == null)
                    return NotFound();

                return View(blog);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
    }
}