using GeminiAdvancedAPI.Domain.Entities;
using GeminiAdvancedAPI.Domain.Entities.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Interfaces.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync();
        Task<Tag> GetByIdAsync(int id);
        Task<Tag> AddAsync(Tag tag);
        Task UpdateAsync(Tag tag);
        Task DeleteAsync(int id);
        Task<bool> TagExistsAsync(string tagName);
        Task<Tag> GetTagByNameAsync(string tagName); //Tag name ile getirme.
        IQueryable<Category> GetAll();

    }
}
