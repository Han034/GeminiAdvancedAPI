using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using GeminiAdvancedAPI.Application.Interfaces;
using GeminiAdvancedAPI.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using GeminiAdvancedAPI.Application.Exceptions;
using AutoMapper;
using GeminiAdvancedAPI.Application.DTOs;

namespace GeminiAdvancedAPI.Application.Features.Cart.Commands
{
    public class AddProductToCartCommandHandler : IRequestHandler<AddProductToCartCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor; // Ekledik
        private readonly ICartRepository _cartRepository;

        public AddProductToCartCommandHandler(ICartRepository cartRepository, IProductRepository productRepository, IHttpContextAccessor httpContextAccessor)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Handle(AddProductToCartCommand request, CancellationToken cancellationToken)
        {
            //Doğrudan userId, command nesnesinden geliyor.
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found."); // Veya uygun bir exception
            }

            CartDto cartDto = await _cartRepository.GetByUserIdAsync(request.UserId);

            if (cartDto == null)
            {
                cartDto = new CartDto { UserId = request.UserId, CartItems = new List<CartItemDto>() };
            }

            var cartItem = cartDto.CartItems.FirstOrDefault(ci => ci.ProductId == request.ProductId);

            if (cartItem == null)
            {
                cartItem = new CartItemDto
                {
                    ProductId = request.ProductId,
                    ProductName = product.Name, // Ürün adını DTO'ya ekliyoruz.
                    Quantity = request.Quantity,
                    Price = product.Price
                };
                if (cartDto.CartItems == null)
                {
                    cartDto.CartItems = new List<CartItemDto>(); // Eğer CartItems null ise, yeni bir liste oluştur
                }
                cartDto.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += request.Quantity;
            }

            await _cartRepository.AddOrUpdateCartAsync(cartDto);
        }
    }
}
