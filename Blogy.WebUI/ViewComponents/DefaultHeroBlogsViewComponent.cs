using Blogy.Business.Services.BlogServices;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.ViewComponents
{
    public class DefaultHeroBlogsViewComponent : ViewComponent
    {
        private readonly IBlogService _blogService;

        public DefaultHeroBlogsViewComponent(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // En son eklenen 4–6 blogu çekip banner'a yayacağız
            var blogs = await _blogService.GetLast3BlogsAsync();
            return View("Default", blogs);
        }
    }
}
