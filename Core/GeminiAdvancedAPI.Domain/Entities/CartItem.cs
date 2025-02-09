using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Domain.Entities
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; } // Ürünün o anki fiyatı (indirimler vs. olabilir)
        public virtual Cart Cart { get; set; }
        public virtual Product Product { get; set; }

    }
}
