using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.DTOs.Blog
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string AuthorId { get; set; } //veya username
        public string AuthorUserName { get; set; } // Kullanıcı adı.
        public Guid BlogId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? ParentCommentId { get; set; } // Cevaplanan yorumun ID'si
        public string? ParentCommentContent { get; set; }
        //public List<CommentDto> ChildComments { get; set; }  // Cevapları/Alt Yorumlar. (Gerekirse)
    }
}
