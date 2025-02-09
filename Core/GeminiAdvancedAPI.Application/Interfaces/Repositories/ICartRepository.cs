using GeminiAdvancedAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Interfaces.Repositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart> GetByUserIdAsync(string userId);
        Task<int> GetCartItemCountAsync(string userId);
        // Sepete özel diğer metotlar buraya eklenebilir (örneğin, GetCartItemCount)

    }
}
