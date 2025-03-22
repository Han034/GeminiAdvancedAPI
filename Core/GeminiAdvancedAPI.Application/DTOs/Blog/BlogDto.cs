using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.DTOs.Blog
{
    public class BlogDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorId { get; set; } // string olarak, UserName veya Email göstermek için.
        public string AuthorUserName { get; set; } // veya AuthorEmail, hangisini kullanacaksanız.
        public string AuthorFullName { get; set; } // Ad Soyad.
        public DateTime PublishedDate { get; set; }
        public bool IsPublished { get; set; }
        public string? ImageUrl { get; set; }
        public List<string> CategoryNames { get; set; }
        public List<string> TagNames { get; set; }
        public int CommentCount { get; set; } // Yorum sayısı.
    }
}
