using GeminiAdvancedAPI.Application.Interfaces;
using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Cart.Queries
{
    public class GetCartItemCountQueryHandler : IRequestHandler<GetCartItemCountQuery, int>
    {
        private readonly ICartRepository _cartRepository; // IUnitOfWork yerine

        public GetCartItemCountQueryHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public async Task<int> Handle(GetCartItemCountQuery request, CancellationToken cancellationToken)
        {
            return await _cartRepository.GetCartItemCountAsync(request.UserId);
        }
    }
}
