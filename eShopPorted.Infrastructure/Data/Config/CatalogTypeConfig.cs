using eShopPorted.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopPorted.Infrastructure.Data.Config
{
    /// <summary>
    /// Entity Framework configuration for CatalogType
    /// </summary>
    public class CatalogTypeConfig : IEntityTypeConfiguration<CatalogType>
    {
        public void Configure(EntityTypeBuilder<CatalogType> builder)
        {
            builder.ToTable("CatalogType");

            builder.Property(ct => ct.Type)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
