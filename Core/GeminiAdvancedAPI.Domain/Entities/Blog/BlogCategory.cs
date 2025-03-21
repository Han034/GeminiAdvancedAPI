using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Domain.Entities.Blog
{
    public class BlogCategory
    {
        [Key]
        public Guid BlogId { get; set; }
        public virtual Blog Blog { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
