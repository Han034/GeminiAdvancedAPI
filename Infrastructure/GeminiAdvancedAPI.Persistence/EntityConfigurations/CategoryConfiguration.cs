using GeminiAdvancedAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Persistence.EntityConfigurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            //builder.HasKey(c => c.Id); //Key zaten tanımlanmıs
            builder.Property(c => c.Name).IsRequired().HasMaxLength(50);
            builder.Property(c => c.Description).HasMaxLength(200); // İsteğe bağlı
        }
    }
}
