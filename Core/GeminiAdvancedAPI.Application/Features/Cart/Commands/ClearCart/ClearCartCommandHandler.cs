using GeminiAdvancedAPI.Application.Interfaces;
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
        private readonly IUnitOfWork _unitOfWork;
        public ClearCartCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _unitOfWork.Carts.GetByUserIdAsync(request.UserId);
            if (cart == null)
            {
                return; // Sepet yoksa bir şey yapma (veya hata fırlatılabilir)
            }

            cart.CartItems.Clear(); // Sepetteki tüm öğeleri temizle

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
