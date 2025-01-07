using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Product.Commands.CreateProduct
{
	public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
	{
		private readonly IProductRepository _productRepository;

		public CreateProductCommandHandler(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
		{
			var product = new Domain.Entities.Product
			{
				Name = request.Name,
				Description = request.Description,
				Price = request.Price,
				Stock = request.Stock,
				CreatedDate = DateTime.Now
			};

			await _productRepository.AddAsync(product);
			return product.Id;
		}
	}
}
