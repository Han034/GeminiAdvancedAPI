using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Features.Blog.DTOs
{
    public class BlogDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorId { get; set; } //veya username
        public string AuthorUserName { get; set; }
        public string AuthorFullName { get; set; }
        public DateTime PublishedDate { get; set; }
        public bool IsPublished { get; set; }
        public string? ImageUrl { get; set; }
        public List<string> CategoryNames { get; set; } = new();
        public List<string> TagNames { get; set; } = new();
        public int CommentCount { get; set; }
    }
}
