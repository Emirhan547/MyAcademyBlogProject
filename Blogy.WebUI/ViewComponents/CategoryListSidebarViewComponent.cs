using Blogy.Business.Services.CategoryServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blogy.WebUI.ViewComponents
{
    public class CategoryListSidebarViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public CategoryListSidebarViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Categories + Blog listesi olan DTO
            var categories = await _categoryService.GetCategoriesWithBlogsAsync();
            return View(categories);
        }
    }
}
