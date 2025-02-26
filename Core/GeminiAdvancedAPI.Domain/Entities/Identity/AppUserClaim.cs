using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Domain.Entities.Identity
{
    public class AppUserClaim
    {
        public int Id { get; set; } // PK
        public Guid UserId { get; set; } // Hangi kullanıcıya ait olduğu
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public virtual AppUser User { get; set; } // Navigation property
    }
}
