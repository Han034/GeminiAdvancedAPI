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
    public class AppUserClaimConfiguration : IEntityTypeConfiguration<AppUserClaim>
    {
        public void Configure(EntityTypeBuilder<AppUserClaim> builder)
        {
            builder.ToTable("UserClaims");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ClaimType).IsRequired().HasMaxLength(256);
            builder.Property(x => x.ClaimValue).IsRequired().HasMaxLength(256);

        }
    }
}
