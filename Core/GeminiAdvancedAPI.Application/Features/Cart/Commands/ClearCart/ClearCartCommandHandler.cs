using GeminiAdvancedAPI.Application.Interfaces;
using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Cart.Commands.ClearCart
{
    public class ClearCartCommandHandler : IRequestHandler<ClearCartCommand>
    {
        private readonly ICartRepository _cartRepository; // IUnitOfWork yerine ICartRepository

        public ClearCartCommandHandler(ICartRepository cartRepository) // Constructor'da ICartRepository alın
        {
            _cartRepository = cartRepository;
        }

        public async Task Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            await _cartRepository.ClearCartAsync(request.UserId); // Doğrudan repository metodunu çağırın
        }
    }
}
