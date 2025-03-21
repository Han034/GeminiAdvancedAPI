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
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("Tags");
            //builder.HasKey(t => t.Id); //Key zaten tanımlanmıs
            builder.Property(t => t.Name).IsRequired().HasMaxLength(50);
        }
    }
}
