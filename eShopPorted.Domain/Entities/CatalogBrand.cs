namespace eShopPorted.Domain.Entities
{
    /// <summary>
    /// Represents a catalog brand in the eShop domain
    /// </summary>
    public class CatalogBrand
    {
        public int Id { get; set; }
        public string Brand { get; set; } = string.Empty;
    }
}
