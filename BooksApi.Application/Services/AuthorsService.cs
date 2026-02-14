using BooksApi.Dto;
using BooksApi.Enums;
using BooksApi.Interface;
using Microsoft.Extensions.Caching.Memory;

namespace BooksApi.Services
{
    public class AuthorsService : IAuthorsService
    {
        private readonly IBooksApiClient _client;
        private readonly IMemoryCache _cache;

        private readonly string _cacheKey = "AuthorsCache";
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(10);

        public AuthorsService(IBooksApiClient client, IMemoryCache cache)
        {
            _client = client;
            _cache = cache;
        }
        public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
        {
            return await GetAuthorsDataAsync();
        }
        public async Task<IEnumerable<Author>> GetOderedAuthorsAsync(SortDirection sortDirection)
        {
            var authors = await GetAuthorsDataAsync();

            authors = sortDirection switch
            {
                SortDirection.Ascending => authors.OrderBy(a => a.Name),
                SortDirection.Descending => authors.OrderByDescending(a => a.Name),
                _ => authors
            };

            return authors;
        }

        private async Task<IEnumerable<Author>> GetAuthorsAsync()
        {
            var authorsResponse = await _client.GetAuthorsAsync();

            var authors = authorsResponse.Select(a => new Author
            {
                Id = a.slug,
                Name = a.name
            }).ToList();

            return authors;
        }

        private async Task<IEnumerable<Author>> GetAuthorsDataAsync()
        {
            if (_cache.TryGetValue(_cacheKey, out List<Author> cachedAuthors))
            {
                return cachedAuthors;
            }

            var authors = await GetAuthorsAsync();

            _cache.Set(_cacheKey, authors, _cacheDuration);

            return authors;
        }
    }
}
