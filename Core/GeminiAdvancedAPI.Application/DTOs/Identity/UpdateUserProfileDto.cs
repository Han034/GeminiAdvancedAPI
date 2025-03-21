using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.DTOs.Identity
{
    public class UpdateUserProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public IFormFile? ProfilePicture { get; set; } // Profil resmi için (opsiyonel)
        // Diğer güncellenebilir alanlar
    }
}
