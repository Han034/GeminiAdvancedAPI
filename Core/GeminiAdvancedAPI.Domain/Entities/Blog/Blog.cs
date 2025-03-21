using GeminiAdvancedAPI.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Domain.Entities.Blog
{
    public class Blog : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid AuthorId { get; set; } // User tablosuna foreign key. string, çünkü AppUser'da Id string.
        public DateTime PublishedDate { get; set; }
        public bool IsPublished { get; set; }
        public string? ImageUrl { get; set; } // İsteğe bağlı görsel

        // İlişkiler
        public virtual AppUser Author { get; set; } // Yazar
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<BlogCategory> BlogCategories { get; set; } = new HashSet<BlogCategory>();
        public virtual ICollection<Tag> Tags { get; set; }


        // İsteğe bağlı diğer özellikler
        // public int ReadCount { get; set; } // Okunma sayısı
        // public string Summary { get; set; } // Özet
    }
}
