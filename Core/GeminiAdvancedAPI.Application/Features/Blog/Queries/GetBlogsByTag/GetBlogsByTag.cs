using GeminiAdvancedAPI.Application.Features.Blog.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Blog.Queries.GetBlogsByTag
{
    public record GetBlogsByTagQuery(int TagId) : IRequest<List<BlogDto>>; // BlogDto listesi dönecek

}
