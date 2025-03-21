using GeminiAdvancedAPI.Domain.Entities.Blog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Domain.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        public string? Description { get; set; } // İsteğe bağlı açıklama
        public virtual ICollection<BlogCategory> BlogCategories { get; set; } = new HashSet<BlogCategory>();
    }
}
