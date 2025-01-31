using GeminiAdvancedAPI.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Persistence.EntityConfigurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("Users"); // Tablo adını AspNetUsers yerine Users olarak belirliyoruz.

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd(); // Guid'in veritabanı tarafından oluşturulmasını sağlıyoruz (isteğe bağlı).

            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(50);

            // Diğer IdentityUser property'leri için özel bir konfigürasyona ihtiyacınız yoksa,
            // burada tanımlamanıza gerek yok. EF Core, standart konfigurasyonu uygulayacaktır.
        }
    }
}
