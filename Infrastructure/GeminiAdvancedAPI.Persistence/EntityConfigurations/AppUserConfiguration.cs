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
            builder.ToTable("AspNetUsers"); // Tablo adını AspNetUsers yerine Users olarak belirliyoruz (isteğe bağlı).

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd(); // Opsiyonel: Guid'in veritabanı tarafından oluşturulmasını istiyorsak

            // Diğer property'ler için konfigürasyon (isteğe bağlı):
            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Email).HasMaxLength(256); // IdentityUser'dan gelen property
            builder.Property(x => x.NormalizedEmail).HasMaxLength(256); // IdentityUser'dan gelen property
            builder.Property(x => x.UserName).HasMaxLength(256); // IdentityUser'dan gelen property
            builder.Property(x => x.NormalizedUserName).HasMaxLength(256); // IdentityUser'dan gelen property
            builder.Property(x => x.PhoneNumber).HasMaxLength(50); // Kendi eklediğimiz property

            //İlişkiler (eğer varsa ve daha önce tanımlamadıysanız)
            //Örneğin, UserClaims tablosuyla ilişki:
            builder.HasMany<AppUserClaim>().WithOne(x => x.User).HasForeignKey(x => x.UserId).IsRequired();

            // Diğer ilişkiler...
        }
    }
}
