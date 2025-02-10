using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.DTOs
{
    public class RemoveProductFromCartRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
