using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Product.Commands.DeleteProduct
{
	public record DeleteProductCommand(Guid Id) : IRequest<Unit>;

}
