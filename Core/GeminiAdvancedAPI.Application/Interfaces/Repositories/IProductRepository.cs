using GeminiAdvancedAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Interfaces.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        IQueryable<Product> GetAll(); // Zaten vardı
        Task<IQueryable<Product>> GetAllAsync(); // İsteğe bağlı eklenebilir, ama GetAll() ile aynı işi yapar
    }
}
