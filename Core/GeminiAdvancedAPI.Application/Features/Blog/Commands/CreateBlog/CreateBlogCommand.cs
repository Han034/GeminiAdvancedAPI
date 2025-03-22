using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Validasyon attributeları için
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Blog.Commands.CreateBlog
{
    public record CreateBlogCommand(
    [Required] string Title, // Boş olamaz
    [Required] string Content, // Boş olamaz
    Guid AuthorId,
    DateTime PublishedDate,
    bool IsPublished,
    string? ImageUrl,
    List<int> CategoryIds,
    List<int> TagIds
) : IRequest<Guid>; // Yeni blog'un ID'sini dönecek
}
