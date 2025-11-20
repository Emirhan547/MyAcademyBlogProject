using Blogy.DataAccess.Context;
using Blogy.DataAccess.Repositories.GenericRepositories;
using Blogy.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blogy.DataAccess.Repositories.BlogRepositories
{
    public class BlogRepository : GenericRepository<Blog>, IBlogRepository
    {
        public BlogRepository(AppDbContext context) : base(context)
        {
        }


        public async Task<List<Blog>> GetBlogsWithCategoriesAsync()
        {
            return await _table
               .AsNoTracking()
               .Include(x => x.Category)
               .ToListAsync();
        }

        public async Task<List<Blog>> GetBlogsByCategoryIdWithCategoryAsync(int categoryId)
        {
            return await _table
                .AsNoTracking()
                .Where(x => x.CategoryId == categoryId)
                .Include(x => x.Category)
                .ToListAsync();
        }

        public async Task<List<Blog>> GetLast3BlogsAsync()
        {
            return await _table.OrderByDescending(x => x.Id).Take(3).ToListAsync();
        }
    }
}