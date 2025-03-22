using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using GeminiAdvancedAPI.Domain.Entities.Blog;
using GeminiAdvancedAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Persistence.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly ApplicationDbContext _context;

        public BlogRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Blog>> GetBlogsWithCategoriesAndTagsAsync()
        {
            return await _context.Blogs
                .Include(b => b.BlogCategories)
                .ThenInclude(bc => bc.Category)
                .Include(b => b.Tags)
                .ToListAsync();
        }

        public async Task<Blog> AddAsync(Blog blog)
        {
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync(); // DbContext üzerinden kaydet
            return blog;
        }

        public async Task DeleteAsync(Guid id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog != null)
            {
                _context.Blogs.Remove(blog);
            }
            await _context.SaveChangesAsync(); // DEĞİŞİKLİKLERİ KAYDET
        }

        public async Task<IEnumerable<Blog>> GetAllAsync()
        {
            return await _context.Blogs
                .Include(b => b.Author)  // Yazar bilgisini dahil et.
                .Include(p => p.BlogCategories).ThenInclude(pc => pc.Category)
                .Include(x => x.Tags)
                .Include(p => p.Comments)
                .ToListAsync();
        }

        public async Task<Blog> GetByIdAsync(Guid id)
        {
            return await _context.Blogs
                .Include(b => b.Author)  // Yazar bilgisini dahil et.
                .Include(p => p.BlogCategories).ThenInclude(pc => pc.Category)
                .Include(x => x.Tags)
                .Include(b => b.Comments) //Yorumlar
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task UpdateAsync(Blog blog)
        {

            _context.Entry(blog).State = EntityState.Modified; // Bu satır önemli
            await _context.SaveChangesAsync(); // DEĞİŞİKLİKLERİ KAYDET

        }

        public async Task<IEnumerable<Blog>> GetBlogsByCategoryAsync(int categoryId)
        {
            return await _context.Blogs
                .Include(b => b.BlogCategories) // BlogCategory'leri dahil et
                .ThenInclude(bc => bc.Category) // Category'leri dahil et
                .Where(b => b.BlogCategories.Any(bc => bc.CategoryId == categoryId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Blog>> GetBlogsByTagAsync(int tagId)
        {
            return await _context.Blogs
                .Include(b => b.Tags)
                .Where(b => b.Tags.Any(t => t.Id == tagId))
                .ToListAsync();
        }

        public async Task<int> GetTotalBlogCountAsync()
        {
            return await _context.Blogs.CountAsync();
        }

        public async Task<IEnumerable<Blog>> GetPagedBlogsAsync(int page, int pageSize)
        {
            return await _context.Blogs
                    .Include(b => b.Author)  // Yazar bilgisini dahil et.
                    .Include(p => p.BlogCategories).ThenInclude(pc => pc.Category)
                    .Include(x => x.Tags)
                    .Include(p => p.Comments)
                    .OrderByDescending(b => b.PublishedDate)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
        }
    }
}
