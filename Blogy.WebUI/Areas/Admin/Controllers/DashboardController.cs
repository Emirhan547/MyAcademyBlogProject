using Blogy.Business.Services.DashboardServices;
using Blogy.WebUI.Const;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{Roles.Admin}")]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var statistics = await _dashboardService.GetStatisticsAsync();
            return View(statistics);
        }

        [HttpGet("api/dashboard/chart-data")]
        public async Task<IActionResult> GetChartData()
        {
            var statistics = await _dashboardService.GetStatisticsAsync();

            return Json(new
            {
                monthlyData = statistics.MonthlyStats.Select(m => new
                {
                    month = m.Month,
                    count = m.BlogCount
                }),
                categoryData = statistics.CategoriesStats.Select(c => new
                {
                    name = c.CategoryName,
                    value = c.BlogCount
                })
            });
        }
    }
}