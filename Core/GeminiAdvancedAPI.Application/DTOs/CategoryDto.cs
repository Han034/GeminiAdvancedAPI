using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
    public class UpdateCategoryRequestDto
    {
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }
        // ID BURADA YOK!
    }
}
