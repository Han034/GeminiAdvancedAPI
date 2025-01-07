using AutoMapper;
using GeminiAdvancedAPI.Application.Exceptions;
using GeminiAdvancedAPI.Application.Interfaces;
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
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
		{
			var product = await _unitOfWork.Products.GetByIdAsync(request.Id);

			if (product == null)
			{
				throw new NotFoundException($"Product with id {request.Id} not found");
			}

			_mapper.Map(request, product);

			product.UpdatedDate = DateTime.Now;

			await _unitOfWork.Products.UpdateAsync(product);
			await _unitOfWork.SaveChangesAsync();
			return Unit.Value;
		}
	}
}
