using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.DTOs.Identity
{
    public class UserProfileDto
    {
        public Guid Id { get; set; } // Kullanıcı ID'si
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; } // Opsiyonel telefon numarası
        public string? ProfilePictureUrl { get; set; } // Profil resmi URL'si (opsiyonel)
                                                       // Ekleyebileceğiniz diğer alanlar:
                                                       // public string? Address { get; set; }
                                                       // public DateTime? BirthDate { get; set; }
                                                       // public string? Bio { get; set; } // Kullanıcı hakkında kısa bir açıklama
    }
}
