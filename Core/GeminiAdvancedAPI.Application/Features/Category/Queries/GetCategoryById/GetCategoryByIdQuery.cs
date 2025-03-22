using GeminiAdvancedAPI.Application.Features.Category.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Category.Queries.GetCategoryById
{
    public record GetCategoryByIdQuery(int Id) : IRequest<CategoryDto>; // CategoryDto dönecek

}
