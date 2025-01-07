using AutoMapper;
using GeminiAdvancedAPI.Application.Interfaces;
using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using MediatR;


namespace GeminiAdvancedAPI.Application.Features.Product.Commands.CreateProduct
{
	public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
		{
			var product = _mapper.Map<Domain.Entities.Product>(request);
			product.CreatedDate = DateTime.Now;
			await _unitOfWork.Products.AddAsync(product);
			await _unitOfWork.SaveChangesAsync();
			return product.Id;
		}
	}
}
