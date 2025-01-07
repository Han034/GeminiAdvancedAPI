using AutoMapper;
using GeminiAdvancedAPI.Application.Exceptions;
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
		private readonly IMapper _mapper;

		public UpdateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
		{
			_productRepository = productRepository;
			_mapper = mapper;
		}

		public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
		{
			var product = await _productRepository.GetByIdAsync(request.Id);

			if (product == null)
			{
				throw new NotFoundException($"Product with id {request.Id} not found");
			}

			_mapper.Map(request, product);

			product.UpdatedDate = DateTime.Now;

			await _productRepository.UpdateAsync(product);
			return Unit.Value;
		}
	}
}
