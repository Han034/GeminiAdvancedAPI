using GeminiAdvancedAPI.Application.Exceptions;
using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Product.Commands.DeleteProduct
{
	public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
	{
		private readonly IProductRepository _productRepository;

		public DeleteProductCommandHandler(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
		{
			var product = await _productRepository.GetByIdAsync(request.Id);

			if (product == null)
			{
				throw new NotFoundException($"Product with id {request.Id} not found");
			}

			await _productRepository.DeleteAsync(product);
			return Unit.Value;
		}
	}
}
