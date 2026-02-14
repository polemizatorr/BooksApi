using BooksApi.Dto;
using BooksApi.Entities;
using BooksApi.Interface;

namespace BooksApi.ApiClient
{
    public class BooksApiClient : IBooksApiClient
    {
        private readonly HttpClient _httpClient;

        public BooksApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<AuthorResponse>> GetAuthorsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<AuthorResponse>>("authors/");

            return response ?? new List<AuthorResponse>();
        }

        public async Task<IEnumerable<BookResponse>> GetBooksAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<BookResponse>>("books/");

            return response ?? new List<BookResponse>();
        }
    }
}
