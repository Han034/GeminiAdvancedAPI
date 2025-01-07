using AutoMapper;
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
		private readonly IMapper _mapper;
		public GetProductQueryHandler(IProductRepository productRepository, IMapper mapper)
		{
			_productRepository = productRepository;
			_mapper = mapper;
		}

		public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
		{
			var product = await _productRepository.GetByIdAsync(request.Id);
			return _mapper.Map<ProductDto>(product);
		}
	}
}
