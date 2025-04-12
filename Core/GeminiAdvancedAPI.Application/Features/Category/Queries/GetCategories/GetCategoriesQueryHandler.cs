using AutoMapper;
using GeminiAdvancedAPI.Application.Features.Category.DTOs;
using GeminiAdvancedAPI.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Category.Queries.GetCategories
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public GetCategoriesQueryHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var result = await _categoryRepository.GetAllAsync();

            return _mapper.Map<List<CategoryDto>>(result);
        }
        //public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        //{
        //    var categories = await _categoryRepository.GetAllAsync();
        //    // return _mapper.Map<List<CategoryDto>>(categories); // AutoMapper'ı geçici olarak devre dışı bırak

        //    // Manuel mapping
        //    var categoryDtos = categories.Select(c => new CategoryDto
        //    {
        //        Id = c.Id,
        //        Name = c.Name,
        //        Description = c.Description
        //    }).ToList();

        //    return categoryDtos;
        //}
    }
}
