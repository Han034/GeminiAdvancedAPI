using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Product.Queries.GetProducts
{
    public class GetProductsCountQueryHandler : IRequestHandler<GetProductsCountQuery, int>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsCountQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<int> Handle(GetProductsCountQuery request, CancellationToken cancellationToken)
        {
            return await _productRepository.GetAll().CountAsync();
        }
    }
}
