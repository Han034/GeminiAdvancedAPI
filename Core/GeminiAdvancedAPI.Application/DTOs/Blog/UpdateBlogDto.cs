using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.DTOs.Blog
{
    public class UpdateBlogDto
    {
        public Guid Id { get; set; } //Güncellenecek blog id'si
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishedDate { get; set; }
        public bool IsPublished { get; set; }
        public string? ImageUrl { get; set; }
        public List<int> CategoryIds { get; set; } = new();
        public List<int> TagIds { get; set; } = new();
    }
}
