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

namespace GeminiAdvancedAPI.Application.Features.Cart.Commands
{
    public class AddProductToCartCommandHandler : IRequestHandler<AddProductToCartCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor; // Ekledik

        public AddProductToCartCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository, IHttpContextAccessor httpContextAccessor) // Parametre olarak ekledik
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor; // Field'a atadık
        }

        public async Task Handle(AddProductToCartCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.Identity.Name;
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User not authenticated."); // Bunu da özel bir exception'a çevirebilirsiniz
            }

            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                throw new ProductNotFoundException(request.ProductId); // Artık ProductNotFoundException fırlatıyoruz
            }

            var cart = await _unitOfWork.Carts.GetByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Domain.Entities.Cart { UserId = userId, CartItems = new List<CartItem>() };
                await _unitOfWork.Carts.AddAsync(cart);
                await _unitOfWork.SaveChangesAsync();
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == request.ProductId); // Artık null reference hatası almayacaksınız
            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                    Price = product.Price // Ürünün fiyatını burada alıyoruz
                };
                cart.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += request.Quantity;
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
