using GeminiAdvancedAPI.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Cart.Commands.UpdateCartItemQuantity
{
    public class UpdateCartItemQuantityCommandHandler : IRequestHandler<UpdateCartItemQuantityCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCartItemQuantityCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateCartItemQuantityCommand request, CancellationToken cancellationToken)
        {
            var cart = await _unitOfWork.Carts.GetByUserIdAsync(request.UserId);
            if (cart == null)
            {
                throw new KeyNotFoundException("Cart not found."); // Veya uygun bir exception
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == request.ProductId);
            if (cartItem == null)
            {
                throw new KeyNotFoundException("Product not found in cart."); // Veya uygun bir exception
            }

            cartItem.Quantity = request.Quantity;

            if (cartItem.Quantity <= 0)
            {
                // Eğer miktar 0 veya daha az ise, ürünü sepetten sil
                cart.CartItems.Remove(cartItem);
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
