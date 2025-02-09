using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Cart.Commands
{
    public record AddProductToCartCommand(string UserId, Guid ProductId, int Quantity) : IRequest; //record olarak kalabilir

}
