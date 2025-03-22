using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Blog.Commands.UpdateBlog
{
    public record UpdateBlogCommand(
    [Required] Guid Id,
    [Required] string Title,
    [Required] string Content,
    DateTime PublishedDate,
    bool IsPublished,
    string? ImageUrl,
    List<int> CategoryIds,
    List<int> TagIds
) : IRequest; // Void gibi - bir şey dönmeyecek
}
