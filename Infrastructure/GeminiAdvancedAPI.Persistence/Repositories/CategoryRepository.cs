using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using GeminiAdvancedAPI.Domain.Entities;
using GeminiAdvancedAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Category> AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync(); // DbContext üzerinden kaydet
            return category;
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category is not null)
            {
                _context.Categories.Remove(category);
            }
            await _context.SaveChangesAsync(); // DEĞİŞİKLİKLERİ KAYDET
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync(); // DEĞİŞİKLİKLERİ KAYDET

        }
        public async Task<bool> CategoryExistsAsync(string categoryName)
        {
            return await _context.Categories.AnyAsync(c => c.Name == categoryName);
        }
    }
}
