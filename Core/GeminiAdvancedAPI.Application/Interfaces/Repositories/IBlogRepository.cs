using GeminiAdvancedAPI.Domain.Entities.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Interfaces.Repositories
{
    public interface IBlogRepository
    {
        Task<IEnumerable<Blog>> GetAllAsync();
        Task<Blog> GetByIdAsync(Guid id);
        Task<Blog> AddAsync(Blog blog);
        Task UpdateAsync(Blog blog);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Blog>> GetBlogsByCategoryAsync(int categoryId);
        Task<IEnumerable<Blog>> GetBlogsByTagAsync(int tagId);
        Task<IEnumerable<Blog>> GetBlogsWithCategoriesAndTagsAsync();
        Task<int> GetTotalBlogCountAsync();
        Task<IEnumerable<Blog>> GetPagedBlogsAsync(int page, int pageSize);

    }
}
