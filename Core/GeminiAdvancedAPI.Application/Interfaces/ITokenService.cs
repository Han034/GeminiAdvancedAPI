using GeminiAdvancedAPI.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user, IList<string> roles);
    }
}
