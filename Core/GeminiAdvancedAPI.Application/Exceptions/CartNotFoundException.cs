using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Exceptions
{
    public class CartNotFoundException : Exception
    {
        public CartNotFoundException(string userId) : base($"Cart for user with id {userId} not found.") { }

    }
}
