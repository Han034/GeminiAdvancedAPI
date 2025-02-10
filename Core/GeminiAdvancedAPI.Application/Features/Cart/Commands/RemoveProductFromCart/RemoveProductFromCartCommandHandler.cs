using GeminiAdvancedAPI.Application.Interfaces;
using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace GeminiAdvancedAPI.Application.Features.Cart.Commands.RemoveProductFromCart
{
    public class RemoveProductFromCartCommandHandler : IRequestHandler<RemoveProductFromCartCommand>
    {
        private readonly ICartRepository _cartRepository; // IUnitOfWork yerine
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RemoveProductFromCartCommandHandler(ICartRepository cartRepository, IHttpContextAccessor httpContextAccessor)
        {
            _cartRepository = cartRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Handle(RemoveProductFromCartCommand request, CancellationToken cancellationToken)
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

            await _cartRepository.RemoveItemFromCartAsync(userId, request.ProductId, request.Quantity); //Doğrudan
        }
    }
}
