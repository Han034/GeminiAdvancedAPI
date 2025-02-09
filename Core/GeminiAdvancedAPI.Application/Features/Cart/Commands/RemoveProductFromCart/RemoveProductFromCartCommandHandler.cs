using GeminiAdvancedAPI.Application.Interfaces;
using MediatR;

namespace GeminiAdvancedAPI.Application.Features.Cart.Commands.RemoveProductFromCart
{
    public class RemoveProductFromCartCommandHandler : IRequestHandler<RemoveProductFromCartCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveProductFromCartCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(RemoveProductFromCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _unitOfWork.Carts.GetByUserIdAsync(request.UserId);
            if (cart == null)
            {
                throw new KeyNotFoundException("Cart not found."); //veya uygun exception
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == request.ProductId);
            if (cartItem == null)
            {
                throw new KeyNotFoundException("Product not found in cart."); //veya uygun exception
            }

            if (request.Quantity >= cartItem.Quantity)
            {
                // Tüm miktarı veya daha fazlasını sil
                cart.CartItems.Remove(cartItem);
            }
            else
            {
                // Belirtilen miktarı azalt
                cartItem.Quantity -= request.Quantity;
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
