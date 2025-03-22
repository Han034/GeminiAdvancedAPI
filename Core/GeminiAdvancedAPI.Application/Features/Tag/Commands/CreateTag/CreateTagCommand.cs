using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Tag.Commands.CreateTag
{
    public record CreateTagCommand([Required] string Name) : IRequest<int>; // TagId (int) dönecek

}
