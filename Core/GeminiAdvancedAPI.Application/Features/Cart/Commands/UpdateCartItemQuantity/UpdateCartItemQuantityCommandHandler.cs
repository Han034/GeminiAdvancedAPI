using GeminiAdvancedAPI.Application.Interfaces;
using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Cart.Commands.UpdateCartItemQuantity
{
    public class UpdateCartItemQuantityCommandHandler : IRequestHandler<UpdateCartItemQuantityCommand>
    {
        private readonly ICartRepository _cartRepository; // IUnitOfWork yerine
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateCartItemQuantityCommandHandler(ICartRepository cartRepository, IHttpContextAccessor httpContextAccessor)
        {
            _cartRepository = cartRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Handle(UpdateCartItemQuantityCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.Identity.Name;
            if (string.IsNullOrEmpty(userId))
            {
                if (_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("MyShoppingCart", out string cartId))
                {
                    userId = cartId;
                }
                else
                {
                    throw new UnauthorizedAccessException("User not authenticated."); //veya başka bir exception
                }
            }
            await _cartRepository.UpdateCartItemQuantityAsync(userId, request.ProductId, request.Quantity); //Doğrudan
        }
    }
}
