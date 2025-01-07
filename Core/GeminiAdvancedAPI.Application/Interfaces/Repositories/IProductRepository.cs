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
		// Product'a özel metodlar buraya eklenebilir.
		// Örneğin:
		// Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId);
	}
}
