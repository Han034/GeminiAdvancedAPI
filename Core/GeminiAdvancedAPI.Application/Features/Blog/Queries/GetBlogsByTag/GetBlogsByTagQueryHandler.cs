using AutoMapper;
using GeminiAdvancedAPI.Application.Features.Blog.DTOs;
using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Blog.Queries.GetBlogsByTag
{
    public class GetBlogsByTagQueryHandler : IRequestHandler<GetBlogsByTagQuery, List<BlogDto>>
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;

        public GetBlogsByTagQueryHandler(IBlogRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        public async Task<List<BlogDto>> Handle(GetBlogsByTagQuery request, CancellationToken cancellationToken)
        {
            var blogs = await _blogRepository.GetBlogsByTagAsync(request.TagId);
            return _mapper.Map<List<BlogDto>>(blogs);
        }
    }
}
