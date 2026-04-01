using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using eShopPorted.Domain.Entities;

namespace eShopPorted.Infrastructure.Configuration;

/// <summary>
/// Entity configuration for CatalogType
/// </summary>
public class CatalogTypeConfig : IEntityTypeConfiguration<CatalogType>
{
    public void Configure(EntityTypeBuilder<CatalogType> builder)
    {
        builder.ToTable("CatalogType");

        builder.Property(ct => ct.Id)
            .UseHiLo("catalog_type_hilo")
            .IsRequired();

        builder.Property(ct => ct.Type)
            .IsRequired()
            .HasMaxLength(100);
    }
}
