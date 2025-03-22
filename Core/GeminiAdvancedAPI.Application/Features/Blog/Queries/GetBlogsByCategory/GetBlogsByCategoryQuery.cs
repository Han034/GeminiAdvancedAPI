using GeminiAdvancedAPI.Application.Features.Blog.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Blog.Queries.GetBlogsByCategory
{
    public record GetBlogsByCategoryQuery(int CategoryId) : IRequest<List<BlogDto>>; // BlogDto listesi dönecek

}
