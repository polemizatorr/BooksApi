using BooksApi.Dto;
using BooksApi.Enums;

namespace BooksApi.Interface
{
    public interface IAuthorsService
    {
        public Task<IEnumerable<Author>> GetAllAuthorsAsync();
        public Task<IEnumerable<Author>> GetOderedAuthorsAsync(SortDirection sortDirection);
    }
}
