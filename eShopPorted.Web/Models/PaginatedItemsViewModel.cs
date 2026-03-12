namespace eShopPorted.Web.Models;

public class PaginatedItemsViewModel<TEntity>
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public long Count { get; set; }
    public IEnumerable<TEntity> Data { get; set; } = new List<TEntity>();

    public int ActualPage => PageIndex;
    public int ItemsPerPage => PageSize;
    public long TotalItems => Count;
    public int TotalPages => (int)Math.Ceiling((decimal)Count / PageSize);
}
