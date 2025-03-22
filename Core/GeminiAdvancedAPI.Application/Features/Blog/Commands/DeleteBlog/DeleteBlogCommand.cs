using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Blog.Commands.DeleteBlog
{
    public record DeleteBlogCommand([Required] Guid Id) : IRequest; // Silinecek blog ID'si
}
