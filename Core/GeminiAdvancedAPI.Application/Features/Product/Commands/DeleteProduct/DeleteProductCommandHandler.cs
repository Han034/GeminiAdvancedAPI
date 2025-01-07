using GeminiAdvancedAPI.Application.Exceptions;
using GeminiAdvancedAPI.Application.Interfaces;
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
		private readonly IUnitOfWork _unitOfWork;
		public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
		{
			var product = await _unitOfWork.Products.GetByIdAsync(request.Id);

			if (product == null)
			{
				throw new NotFoundException($"Product with id {request.Id} not found");
			}

			await _unitOfWork.Products.DeleteAsync(product);
			await _unitOfWork.SaveChangesAsync();
			return Unit.Value;
		}
	}
}
