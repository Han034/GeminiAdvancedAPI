using GeminiAdvancedAPI.Application.Features.Blog.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Blog.Queries.GetBlogById
{
    public record GetBlogByIdQuery(Guid Id) : IRequest<BlogDto>; // BlogDto dönecek
}
