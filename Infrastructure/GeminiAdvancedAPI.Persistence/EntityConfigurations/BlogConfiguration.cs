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
    public class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.ToTable("Blogs");
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Title).IsRequired().HasMaxLength(200);
            builder.Property(b => b.Content).IsRequired();
            builder.Property(b => b.AuthorId).IsRequired();
            builder.Property(b => b.PublishedDate).IsRequired();
            builder.Property(b => b.IsPublished).IsRequired();
            builder.Property(b => b.ImageUrl).HasMaxLength(255); // İsteğe bağlı

            // İlişkiler
            builder.HasOne(b => b.Author).WithMany().HasForeignKey(b => b.AuthorId); // User ile ilişki (one-to-many)
            builder.HasMany(b => b.Comments).WithOne(c => c.Blog).HasForeignKey(c => c.BlogId); // Comment ile ilişki (one-to-many)
            builder.HasMany(x => x.Tags)
            .WithMany(y => y.Blogs)
            .UsingEntity<Dictionary<string, object>>(
                "BlogTag",
                x => x.HasOne<Tag>().WithMany().HasForeignKey("TagId"),
                x => x.HasOne<Blog>().WithMany().HasForeignKey("BlogId"),
                x =>
                {
                    x.Property<Guid>("BlogId");
                    x.Property<int>("TagId");
                    x.HasKey("BlogId", "TagId");
                }
            );
        }
    }
}
