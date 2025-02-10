using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using GeminiAdvancedAPI.Domain.Entities;
using GeminiAdvancedAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Persistence.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<Product> GetAll()
        {
            return _dbContext.Set<Product>();
        }
        public async Task<IQueryable<Product>> GetAllAsync() // IProductRepository içerisindeki bu metodu implemente et.
        {
            return await Task.FromResult(GetAll());
        }
    }
}
