using BooksApi.Dto;
using BooksApi.Entities;

namespace BooksApi.Interface
{
    public interface IBooksApiClient
    {
        Task<IEnumerable<BookResponse>> GetBooksAsync();

        Task<IEnumerable<AuthorResponse>> GetAuthorsAsync();
    }
}
