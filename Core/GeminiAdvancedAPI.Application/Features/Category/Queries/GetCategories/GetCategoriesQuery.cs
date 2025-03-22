using GeminiAdvancedAPI.Application.Features.Category.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Category.Queries.GetCategories
{
    public record GetCategoriesQuery : IRequest<List<CategoryDto>>; // CategoryDto listesi dönecek

}
