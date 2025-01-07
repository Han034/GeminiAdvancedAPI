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

		// Product'a özel metotların implementasyonları buraya eklenebilir.
		// Örneğin:
		// public async Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId)
		// {
		//     return await _dbContext.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
		// }
	}
}
