using GeminiAdvancedAPI.Domain.Entities.Blog;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Persistence.EntityConfigurations
{
    public class BlogCategoryConfiguration : IEntityTypeConfiguration<BlogCategory>
    {
        public void Configure(EntityTypeBuilder<BlogCategory> builder)
        {
            builder.ToTable("BlogCategories"); // Tablo adı

            // İlişkiler
            builder.HasOne(bc => bc.Blog) // BlogCategory, bir Blog'a aittir.
                .WithMany(b => b.BlogCategories) // Blog'un birden fazla BlogCategory'si olabilir.
                .HasForeignKey(bc => bc.BlogId);  // Foreign Key

            builder.HasOne(bc => bc.Category)  // BlogCategory, bir Category'e aittir.
                .WithMany(c => c.BlogCategories) // Category'nin birden fazla BlogCategory'si olabilir.
                .HasForeignKey(bc => bc.CategoryId);   // Foreign Key
        }
    }
}
