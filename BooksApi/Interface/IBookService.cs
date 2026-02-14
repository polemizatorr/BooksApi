using BooksApi.Dto;
using BooksApi.Enums;

namespace BooksApi.Interface
{
    public interface IBookService
    {
        public Task<IEnumerable<Book>> GetAllBooksAsync();
        public Task<Book?> GetBookById(string id);
        public Task<IEnumerable<Book>> GetBooksByAuthorAsync(string authorName);
        public Task<IEnumerable<Book>> GetOderedBooksAsync(BookSortFields field, SortDirection sortDirection);
        public Task<IEnumerable<Book>> GetFilteredBooksAsync(BookFilterFields field, string filterValue);
    }
}
