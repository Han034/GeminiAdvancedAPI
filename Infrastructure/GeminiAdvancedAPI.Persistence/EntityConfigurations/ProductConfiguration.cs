using GeminiAdvancedAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Persistence.EntityConfigurations
{
	public class ProductConfiguration : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
			builder.ToTable("Products"); // Tablo adı

			builder.HasKey(p => p.Id); // Primary Key

			builder.Property(p => p.Name)
				.IsRequired()
				.HasMaxLength(255); // Zorunlu ve maksimum 255 karakter

			builder.Property(p => p.Description)
				.IsRequired(false); // Opsiyonel

			builder.Property(p => p.Price)
				.IsRequired()
				.HasColumnType("decimal(18,2)"); // Zorunlu ve ondalık sayı (ör. 1234.56)

			builder.Property(p => p.Stock)
				.IsRequired();

			builder.Property(p => p.CreatedDate).IsRequired();
			builder.Property(p => p.UpdatedDate).IsRequired(false);
		}
	}
}
