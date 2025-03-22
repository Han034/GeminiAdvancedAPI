using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Tag.Commands.DeleteTag
{
    public record DeleteTagCommand([Required] int Id) : IRequest; // Silinecek tag ID'si

}
