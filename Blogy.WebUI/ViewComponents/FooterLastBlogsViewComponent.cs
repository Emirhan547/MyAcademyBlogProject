using Blogy.Business.Services.BlogServices;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.ViewComponents
{
    public class FooterLastBlogsViewComponent : ViewComponent
    {
        private readonly IBlogService _blogService;

        public FooterLastBlogsViewComponent(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var blogs = await _blogService.GetLast3BlogsAsync();
            return View("Default", blogs);
        }
    }
}
