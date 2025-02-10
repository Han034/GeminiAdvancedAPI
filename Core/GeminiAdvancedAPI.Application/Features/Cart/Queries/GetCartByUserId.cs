using AutoMapper;
using GeminiAdvancedAPI.Application.DTOs;
using GeminiAdvancedAPI.Application.Interfaces;
using MediatR;


namespace GeminiAdvancedAPI.Application.Features.Cart.Queries
{
    public record GetCartByUserIdQuery(string UserId) : IRequest<CartDto>;

}
