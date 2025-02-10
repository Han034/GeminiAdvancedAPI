using AutoMapper;
using AutoMapper.QueryableExtensions;
using GeminiAdvancedAPI.Application.DTOs.PagedResult;
using GeminiAdvancedAPI.Application.Features.Product.Dtos;
using GeminiAdvancedAPI.Application.Interfaces;
using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GeminiAdvancedAPI.Application.Features.Product.Queries.GetProducts
{
    public record GetProductsPagedQuery(int PageNumber, int PageSize) : IRequest<PagedResult<ProductDto>>;

}
