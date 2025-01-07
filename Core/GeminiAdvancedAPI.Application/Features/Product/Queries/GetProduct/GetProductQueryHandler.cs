using AutoMapper;
using GeminiAdvancedAPI.Application.Features.Product.Dtos;
using GeminiAdvancedAPI.Application.Interfaces;
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
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GetProductQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
		{
			var product = await _unitOfWork.Products.GetByIdAsync(request.Id);
			return _mapper.Map<ProductDto>(product);
		}
	}
}
