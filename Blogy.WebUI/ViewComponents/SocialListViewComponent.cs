using Blogy.DataAccess.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blogy.WebUI.ViewComponents
{
    public class SocialListViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public SocialListViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var socials = await _context.Socials.AsNoTracking().ToListAsync();
            return View("Default", socials);
        }
    }
}
