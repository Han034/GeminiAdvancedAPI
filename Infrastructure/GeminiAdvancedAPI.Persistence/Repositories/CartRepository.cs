using GeminiAdvancedAPI.Application.DTOs;
using GeminiAdvancedAPI.Application.Interfaces;
using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using GeminiAdvancedAPI.Domain.Entities;
using GeminiAdvancedAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Persistence.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly IDistributedCache _cache;
        private readonly string _cartKeyPrefix = "cart:"; // Redis key'leri için ön ek

        public CartRepository(IDistributedCache cache) // Sadece IDistributedCache
        {
            _cache = cache;
        }

        private string GetCartKey(string userId) => $"{_cartKeyPrefix}{userId}";

        public async Task<CartDto> GetByUserIdAsync(string userId)
        {
            var cartKey = GetCartKey(userId);
            var cachedCart = await _cache.GetStringAsync(cartKey);
            return cachedCart == null ? null : JsonSerializer.Deserialize<CartDto>(cachedCart);
        }

        public async Task AddOrUpdateCartAsync(CartDto cartDto)
        {
            var cartKey = GetCartKey(cartDto.UserId);
            var serializedCart = JsonSerializer.Serialize(cartDto);
            await _cache.SetStringAsync(cartKey, serializedCart, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7) // Örnek: 7 gün sonra expire olsun
            });
        }

        public async Task DeleteCartAsync(string userId)
        {
            var cartKey = GetCartKey(userId);
            await _cache.RemoveAsync(cartKey);
        }
        public async Task<int> GetCartItemCountAsync(string userId)
        {
            var cartDto = await GetByUserIdAsync(userId);
            if (cartDto == null)
            {
                return 0;
            }

            return cartDto.CartItems.Sum(item => item.Quantity);
        }
        public async Task RemoveItemFromCartAsync(string userId, Guid productId, int quantity)
        {
            var cart = await GetByUserIdAsync(userId);

            if (cart == null)
            {
                throw new KeyNotFoundException("Cart Not Found");
            }
            var cartItem = cart.CartItems.FirstOrDefault(x => x.ProductId == productId);

            if (cartItem == null)
            {
                throw new KeyNotFoundException("CartItem Not Found");
            }

            if (quantity >= cartItem.Quantity)
            {
                cart.CartItems.Remove(cartItem);
            }
            else
            {
                cartItem.Quantity -= quantity;
            }
            await AddOrUpdateCartAsync(cart);

        }
        public async Task UpdateCartItemQuantityAsync(string userId, Guid productId, int quantity)
        {
            var cart = await GetByUserIdAsync(userId);

            if (cart == null)
            {
                throw new KeyNotFoundException("Cart not found.");
            }

            var cartItem = cart.CartItems.FirstOrDefault(x => x.ProductId == productId);

            if (cartItem == null)
            {
                throw new KeyNotFoundException("Product not found in cart.");
            }

            cartItem.Quantity = quantity;

            if (cartItem.Quantity <= 0)
            {
                cart.CartItems.Remove(cartItem);
            }

            await AddOrUpdateCartAsync(cart);
        }

        public async Task ClearCartAsync(string userId)
        {
            var cartKey = GetCartKey(userId); // Redis key'ini al
            await _cache.RemoveAsync(cartKey); // Redis'ten sepeti sil
        }
    }
}
