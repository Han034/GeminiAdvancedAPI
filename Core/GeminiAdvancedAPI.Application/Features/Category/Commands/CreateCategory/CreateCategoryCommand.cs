using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Category.Commands.CreateCategory
{
    public record CreateCategoryCommand([Required] string Name, string? Description) : IRequest<int>; // CategoryId (int) dönecek

}
