using AutoMapper;
using AutoMapper.QueryableExtensions;
using GeminiAdvancedAPI.Application.Exceptions;
using GeminiAdvancedAPI.Application.Features.Product.Dtos;
using GeminiAdvancedAPI.Application.Features.Product.Queries.GetProducts;
using GeminiAdvancedAPI.Application.Interfaces;
using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Product.Queries.GetProduct
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);

            if (product == null)
            {
                // Ürün bulunamadıysa NotFoundException fırlat
                throw new NotFoundException(nameof(Product), request.Id);
            }

            // Bulunan ürünü ProductDto'ya map'le ve döndür
            return _mapper.Map<ProductDto>(product);
        }
    }
}
