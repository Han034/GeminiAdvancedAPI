using AutoMapper;
using GeminiAdvancedAPI.Application.Exceptions;
using GeminiAdvancedAPI.Application.Features.Tag.DTOs;
using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Tag.Queries.GetTagById
{
    public class GetTagByIdQueryHandler : IRequestHandler<GetTagByIdQuery, TagDto>
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public GetTagByIdQueryHandler(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }
        public async Task<TagDto> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
        {
            var tag = await _tagRepository.GetByIdAsync(request.Id);
            if (tag == null)
            {
                throw new NotFoundException(nameof(Tag), request.Id);
            }
            return _mapper.Map<TagDto>(tag);
        }
    }
}
