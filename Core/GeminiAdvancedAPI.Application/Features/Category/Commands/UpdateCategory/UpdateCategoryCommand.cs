using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Category.Commands.UpdateCategory
{
    public record UpdateCategoryCommand([Required] int Id, [Required] string Name, string? Description) : IRequest; // Geriye bir şey dönmeyecek (void gibi)

}
