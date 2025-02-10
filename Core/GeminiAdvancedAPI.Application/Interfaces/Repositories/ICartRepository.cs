using GeminiAdvancedAPI.Application.DTOs;
using GeminiAdvancedAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Interfaces.Repositories
{
    public interface ICartRepository // IRepository<Cart>'tan türemesine GEREK YOK
    {
        Task<CartDto> GetByUserIdAsync(string userId);
        Task AddOrUpdateCartAsync(CartDto cartDto); // Sepeti kaydetmek/güncellemek için
        Task DeleteCartAsync(string userId); // Sepeti silmek için
        Task<int> GetCartItemCountAsync(string userId);
        Task RemoveItemFromCartAsync(string userId, Guid productId, int quantity);
        Task UpdateCartItemQuantityAsync(string userId, Guid productId, int quantity);
        Task ClearCartAsync(string userId);

        // İsteğe bağlı: Sepete özel başka metotlar eklenebilir
    }
}
