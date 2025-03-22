using AutoMapper;
using GeminiAdvancedAPI.Application.Features.Blog.DTOs;
using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Blog.Queries.GetBlogsByCategory
{
    public class GetBlogsByCategoryQueryHandler : IRequestHandler<GetBlogsByCategoryQuery, List<BlogDto>>
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;

        public GetBlogsByCategoryQueryHandler(IBlogRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        public async Task<List<BlogDto>> Handle(GetBlogsByCategoryQuery request, CancellationToken cancellationToken)
        {
            var blogs = await _blogRepository.GetBlogsByCategoryAsync(request.CategoryId);
            return _mapper.Map<List<BlogDto>>(blogs);
        }
    }
}
