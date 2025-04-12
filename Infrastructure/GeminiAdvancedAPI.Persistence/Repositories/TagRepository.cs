using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using GeminiAdvancedAPI.Domain.Entities;
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
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationDbContext _context;
        public TagRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Tag> AddAsync(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync(); // DbContext üzerinden kaydet
            return tag;
        }

        public async Task DeleteAsync(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag is not null)
            {
                _context.Tags.Remove(tag);
            }
            await _context.SaveChangesAsync(); // DEĞİŞİKLİKLERİ KAYDET
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<Tag> GetByIdAsync(int id)
        {
            return await _context.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(Tag tag)
        {
            _context.Entry(tag).State = EntityState.Modified;
            await _context.SaveChangesAsync(); // DEĞİŞİKLİKLERİ KAYDET
        }
        public async Task<bool> TagExistsAsync(string tagName)
        {
            return await _context.Tags.AnyAsync(t => t.Name == tagName);
        }
        public async Task<Tag> GetTagByNameAsync(string tagName)
        {
            return await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
        }

        public IQueryable<Category> GetAll()
        {
            return _context.Categories;
        }
    }
}
