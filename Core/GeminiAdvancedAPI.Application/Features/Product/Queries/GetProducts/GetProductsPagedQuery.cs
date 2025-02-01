using GeminiAdvancedAPI.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GeminiAdvancedAPI.Application.Features.Product.Queries.GetProducts
{
    public record GetProductsPagedQuery(int PageNumber, int PageSize) : IRequest<List<Domain.Entities.Product>>;

    public class GetProductsPagedQueryHandler : IRequestHandler<GetProductsPagedQuery, List<Domain.Entities.Product>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProductsPagedQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Domain.Entities.Product>> Handle(GetProductsPagedQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Products.GetAllAsync()
                .ContinueWith(task => task.Result
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList(), cancellationToken);
        }
    }
}
