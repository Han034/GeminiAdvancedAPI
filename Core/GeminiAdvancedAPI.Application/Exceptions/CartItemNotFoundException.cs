using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Exceptions
{
    public class CartItemNotFoundException : Exception
    {
        public CartItemNotFoundException(string message) : base(message) { }
        public CartItemNotFoundException(Guid cartItemId) : base($"Cart item with id {cartItemId} not found.") { }

    }
}
