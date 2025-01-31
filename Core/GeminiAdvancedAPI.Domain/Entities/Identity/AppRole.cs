using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Domain.Entities.Identity
{
    public class AppRole : IdentityRole<Guid>
    {
        // Rol ile ilgili ek özellikler buraya eklenebilir.
    }
}
