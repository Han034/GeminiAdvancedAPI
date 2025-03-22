using AutoMapper;
using GeminiAdvancedAPI.Application.Exceptions;
using GeminiAdvancedAPI.Application.Features.Blog.DTOs;
using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Blog.Queries.GetBlogById
{
    public class GetBlogByIdQueryHandler : IRequestHandler<GetBlogByIdQuery, BlogDto>
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;

        public GetBlogByIdQueryHandler(IBlogRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        public async Task<BlogDto> Handle(GetBlogByIdQuery request, CancellationToken cancellationToken)
        {
            var blog = await _blogRepository.GetByIdAsync(request.Id);

            if (blog == null)
            {
                throw new NotFoundException(nameof(Blog), request.Id); // Daha açıklayıcı exception
            }

            return _mapper.Map<BlogDto>(blog);
        }
    }
}
