using Blogy.Business.DTOs.DashboardDtos;
using Blogy.DataAccess.Repositories.BlogRepositories;
using Blogy.DataAccess.Repositories.CategoryRepositories;
using Blogy.DataAccess.Repositories.CommentRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogy.Business.Services.DashboardServices
{
   
    public class DashboardService : IDashboardService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICommentRepository _commentRepository;

        public DashboardService(IBlogRepository blogRepository,
            ICategoryRepository categoryRepository,
            ICommentRepository commentRepository)
        {
            _blogRepository = blogRepository;
            _categoryRepository = categoryRepository;
            _commentRepository = commentRepository;
        }

        public async Task<DashboardStatisticsDto> GetStatisticsAsync()
        {
            var blogs = await _blogRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllAsync();
            var comments = await _commentRepository.GetAllAsync();

            var categoryStats = categories.Select(c => new CategoryStatisticsDto
            {
                CategoryName = c.Name,
                BlogCount = blogs.Count(b => b.CategoryId == c.Id)
            }).ToList();

            var monthlyStats = blogs
                .GroupBy(b => b.CreatedDate.ToString("MMMM yyyy"))
                .Select(g => new MonthlyBlogStatisticsDto
                {
                    Month = g.Key,
                    BlogCount = g.Count()
                })
                .OrderBy(m => m.Month)
                .ToList();

            return new DashboardStatisticsDto
            {
                TotalBlogs = blogs.Count,
                TotalCategories = categories.Count,
                TotalComments = comments.Count,
                TotalUsers = 0, // UserManager'dan çekilecek
                AvgCommentsPerBlog = blogs.Count > 0
                    ? (decimal)comments.Count / blogs.Count
                    : 0,
                CategoriesStats = categoryStats,
                MonthlyStats = monthlyStats
            };
        }
    }
}
