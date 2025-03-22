using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Category.Commands.DeleteCategory
{
    public record DeleteCategoryCommand([Required] int Id) : IRequest; // Silinecek kategorinin ID'si

}
