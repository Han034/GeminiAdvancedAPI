using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Tag.Commands.UpdateTag
{
    public record UpdateTagCommand([Required] int Id, [Required] string Name) : IRequest; // Geriye bir şey dönmeyecek

}
