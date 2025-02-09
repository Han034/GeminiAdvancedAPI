using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Domain.Entities
{
    public class Cart : BaseEntity
    {
        public string UserId { get; set; } // Sepetin sahibi olan kullanıcı
        public virtual ICollection<CartItem> CartItems { get; set; }

        // İsteğe bağlı: Sepet ile ilgili ek bilgiler (ör. oluşturulma tarihi)
        public DateTime CreatedDate { get; set; }
    }
}
