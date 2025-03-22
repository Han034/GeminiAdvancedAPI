using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.DTOs.Blog.Comment
{
    public class CreateCommentDto
    {
        public string Content { get; set; }
        public Guid? ParentCommentId { get; set; } // Cevaplanan yorumun ID'si.  NULL ise, ana yorumdur.
    }
}
