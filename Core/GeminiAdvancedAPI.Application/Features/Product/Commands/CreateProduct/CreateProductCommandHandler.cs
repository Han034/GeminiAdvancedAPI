using AutoMapper;
using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using MediatR;


namespace GeminiAdvancedAPI.Application.Features.Product.Commands.CreateProduct
{
	public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
	{
		private readonly IProductRepository _productRepository;
		private readonly IMapper _mapper;
		public CreateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
		{
			_productRepository = productRepository;
			_mapper = mapper;
		}

		public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
		{
			var product = _mapper.Map<Domain.Entities.Product>(request);
			product.CreatedDate = DateTime.Now;
			await _productRepository.AddAsync(product);
			return product.Id;
		}
	}
}
