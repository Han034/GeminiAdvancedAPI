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
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Content).IsRequired();
            builder.Property(c => c.AuthorId).IsRequired();
            builder.Property(c => c.BlogId).IsRequired();
            builder.Property(c => c.CreatedDate).IsRequired();

            // İlişkiler
            builder.HasOne(c => c.Author).WithMany().HasForeignKey(c => c.AuthorId).OnDelete(DeleteBehavior.NoAction); // User silindiğinde yorumlar silinmesin
            builder.HasOne(c => c.Blog).WithMany(b => b.Comments).HasForeignKey(c => c.BlogId).OnDelete(DeleteBehavior.Cascade);  // Blog silindiğinde yorumlar silinsin
            builder.HasOne(c => c.ParentComment).WithMany(pc => pc.ChildComments).HasForeignKey(c => c.ParentCommentId).OnDelete(DeleteBehavior.NoAction); // NoAction olarak değiştir.

            // Parent comment ilişkisi (isteğe bağlı)
            // Parent comment ilişkisi (isteğe bağlı)
            builder.HasOne(c => c.ParentComment)
                .WithMany(pc => pc.ChildComments)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.NoAction); // Bunu ekleyin/güncelleyin
            // builder.HasOne(c => c.ParentComment).WithMany(pc => pc.ChildComments).HasForeignKey(c => c.ParentCommentId);
        }
    }
}
