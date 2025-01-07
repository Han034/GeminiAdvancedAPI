using GeminiAdvancedAPI.Application.Features.Product.Dtos;
using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Product.Queries.GetProduct
{
	public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
	{
		private readonly IProductRepository _productRepository;

		public GetProductQueryHandler(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
		{
			var product = await _productRepository.GetByIdAsync(request.Id);

			// Burada AutoMapper kullanılabilir (ileride eklenecek)
			return new ProductDto
			{
				Id = product.Id,
				Name = product.Name,
				Description = product.Description,
				Price = product.Price,
				Stock = product.Stock
			};
		}
	}
}
