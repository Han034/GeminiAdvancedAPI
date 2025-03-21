using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Domain.Entities.Identity
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [AllowNull]
        public string? RefreshToken { get; set; }
        [AllowNull]
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public string? PhoneNumber { get; set; } // Opsiyonel telefon numarası
        public string? ProfilePictureUrl { get; set; } // ? işareti, nullable olduğunu belirtir

    }
}
