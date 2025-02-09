using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Cart.Commands.UpdateCartItemQuantity
{
    public record UpdateCartItemQuantityCommand(string UserId, Guid ProductId, int Quantity) : IRequest;

}
