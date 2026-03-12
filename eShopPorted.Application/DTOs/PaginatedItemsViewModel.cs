namespace eShopPorted.Application.DTOs
{
    /// <summary>
    /// View model for paginated items
    /// </summary>
    /// <typeparam name="TEntity">The type of entity being paginated</typeparam>
    public class PaginatedItemsViewModel<TEntity> where TEntity : class
    {
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public long Count { get; private set; }
        public IEnumerable<TEntity> Data { get; private set; }

        public PaginatedItemsViewModel(int pageIndex, int pageSize, long count, IEnumerable<TEntity> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }
    }
}
