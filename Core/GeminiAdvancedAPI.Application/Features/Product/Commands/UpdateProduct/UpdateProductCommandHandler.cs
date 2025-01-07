using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Product.Commands.UpdateProduct
{
	public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
	{
		private readonly IProductRepository _productRepository;

		public UpdateProductCommandHandler(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
		{
			var product = await _productRepository.GetByIdAsync(request.Id);

			if (product == null)
			{
				throw new Exception("Product not found"); // İleride daha anlamlı bir exception fırlatılabilir.
			}

			product.Name = request.Name;
			product.Description = request.Description;
			product.Price = request.Price;
			product.Stock = request.Stock;
			product.UpdatedDate = DateTime.Now;

			await _productRepository.UpdateAsync(product);
			return Unit.Value;
		}
	}
}
