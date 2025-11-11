using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogy.Business.DTOs.DashboardDtos
{
    public class DashboardStatisticsDto
    {
        public int TotalBlogs { get; set; }
        public int TotalCategories { get; set; }
        public int TotalComments { get; set; }
        public int TotalUsers { get; set; }
        public decimal AvgCommentsPerBlog { get; set; }
        public List<CategoryStatisticsDto> CategoriesStats { get; set; }
        public List<MonthlyBlogStatisticsDto> MonthlyStats { get; set; }
    }
}
