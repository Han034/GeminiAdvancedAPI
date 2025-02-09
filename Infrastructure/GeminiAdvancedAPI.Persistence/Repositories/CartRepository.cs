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
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Cart> GetByUserIdAsync(string userId)
        {
            return await _dbContext.Carts
                .Include(c => c.CartItems) // Sepet öğelerini de dahil et (eager loading)
                .ThenInclude(ci => ci.Product) // İsteğe bağlı: Her bir sepet öğesinin Product bilgisini de dahil et
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<int> GetCartItemCountAsync(string userId)
        {
            var cart = await _dbContext.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return 0;
            }

            return cart.CartItems.Sum(item => item.Quantity);
        }
    }
}
