namespace BooksApi.Pagination
{
    public static class PaginationExtensions
    {
        public static PagedResult<T> ToPagedResult<T>(
            this IEnumerable<T> source,
            int page,
            int pageSize)
        {
            if (page <= 0)
                page = 1;

            if (pageSize <= 0)
                pageSize = 10;

            var totalCount = source.Count();

            var items = source
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<T>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = items
            };
        }
    }

}
