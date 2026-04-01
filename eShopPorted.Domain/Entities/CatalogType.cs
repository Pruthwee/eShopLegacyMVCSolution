namespace eShopPorted.Domain.Entities;

/// <summary>
/// Represents a catalog type/category
/// </summary>
public class CatalogType
{
    public int Id { get; set; }
    
    public string Type { get; set; } = string.Empty;
}
