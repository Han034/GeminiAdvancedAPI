using GeminiAdvancedAPI.Application.Features.Product.Dtos;
using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Product.Queries.GetProducts
{
	public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
	{
		private readonly IProductRepository _productRepository;

		public GetProductsQueryHandler(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
		{
			var products = await _productRepository.GetAllAsync();

			// Burada AutoMapper kullanılabilir (ileride eklenecek)
			return products.Select(p => new ProductDto
			{
				Id = p.Id,
				Name = p.Name,
				Description = p.Description,
				Price = p.Price,
				Stock = p.Stock
			}).ToList();
		}
	}
}
