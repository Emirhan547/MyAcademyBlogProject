using Blogy.Business.Services.CategoryServices;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.ViewComponents
{
    public class DefaultBlogsViewComponent(ICategoryService _categoryService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categoriesWithBlogs = await _categoryService.GetCategoriesWithBlogsAsync();
            return View(categoriesWithBlogs);
        }
    }
}