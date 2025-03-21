using GeminiAdvancedAPI.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Domain.Entities.Blog
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Content { get; set; }

        public Guid AuthorId { get; set; } // Yorumu yapan kullanıcı
        public Guid BlogId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // İlişkiler
        public virtual AppUser Author { get; set; }
        public virtual Blog Blog { get; set; }

        //İsteğe bağlı: Üst yorum (parent comment)
        public Guid? ParentCommentId { get; set; }
        public virtual Comment ParentComment { get; set; }
        public virtual ICollection<Comment> ChildComments { get; set; }
    }

}
