using BooksApi.Dto;
using BooksApi.Enums;
using BooksApi.Interface;
using Microsoft.Extensions.Caching.Memory;
using System.Globalization;

namespace BooksApi.Services
{
    public class BooksService : IBookService
    {
        private readonly IBooksApiClient _client;
        private readonly IMemoryCache _cache;

        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(10);
        private readonly string _cacheKey = "BooksCache";


        public BooksService(IBooksApiClient client, IMemoryCache cache)
        {
            _client = client;
            _cache = cache;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await GetBooksDataAsync();
        }

        public async Task<Book?> GetBookById(string id)
        {
            var books = await GetBooksDataAsync();
            var result = books.FirstOrDefault(b => b.Id == id);

            return result ?? null;
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorAsync(string authorName)
        {
            var books = await GetBooksDataAsync();
            var result = books.Where(b => b.Author.Name != null && b.Author.Name.Equals(authorName, StringComparison.OrdinalIgnoreCase)).ToList();

            return result;
        }

        public async Task<IEnumerable<Book>> GetFilteredBooksAsync(BookFilterFields field, string filterValue)
        {
            if (string.IsNullOrWhiteSpace(filterValue))
                return await GetBooksDataAsync();

            var booksResponse = await GetBooksDataAsync();
            filterValue = filterValue.Trim().ToLowerInvariant();

            Func<Book, bool> predicate = field switch
            {
                BookFilterFields.Kind => b => !string.IsNullOrEmpty(b.Kind) && b.Kind.ToLowerInvariant().Contains(filterValue),
                BookFilterFields.Genre => b => !string.IsNullOrEmpty(b.Genre) && b.Genre.ToLowerInvariant().Contains(filterValue),
                BookFilterFields.Epoch => b => !string.IsNullOrEmpty(b.Epoch) && b.Epoch.ToLowerInvariant().Contains(filterValue),
                _ => b => true
            };

            var filtered = booksResponse.Where(predicate);
            return filtered;
        }

        public async Task<IEnumerable<Book>> GetOderedBooksAsync(BookSortFields field, SortDirection sortDirection)
        {
            var booksResponse = await GetBooksDataAsync();
            var comparer = StringComparer.Create(new CultureInfo("pl-PL"), ignoreCase: true);

            Func<Book, string> keySelector = field switch
            {
                BookSortFields.Title => b => b.Title,
                BookSortFields.Author => b => b.Author?.Name ?? string.Empty,
                _ => b => b.Title
            };

            var ordered = sortDirection switch
            {
                SortDirection.Ascending => booksResponse.OrderBy(keySelector, comparer),
                SortDirection.Descending => booksResponse.OrderByDescending(keySelector, comparer),
                _ => booksResponse
            };

            return ordered;
        }

        private async Task<IEnumerable<Book>> GetBooksAsync()
        {
            var booksResponse = await _client.GetBooksAsync();
            var authorsResponse = await _client.GetAuthorsAsync();

            var authorsByName = authorsResponse.ToDictionary(a => a.name.TrimEnd().TrimStart(), a => a);

            var books = booksResponse.Select(book =>
            {
                authorsByName.TryGetValue(book.author.TrimEnd().TrimStart(), out var author);

                return new Book
                {
                    Id = book.slug,
                    Cover = book.cover,
                    Kind = book.kind,
                    Genre = book.genre,
                    Epoch = book.epoch,
                    Url = book.href,
                    Thumbnail = book.simple_thumb,
                    Title = book.title,
                    Author = author == null
                        // if not exactly matching book author name with author name display name from the book
                        ? new Author 
                        {
                            Id = book.author ?? string.Empty,
                            Name = book.author ?? string.Empty
                        }
                        : new Author
                        {
                            Id = author.slug,
                            Name = author.name
                        }
                };
            }).ToList();

            return books;
        }

        private async Task<IEnumerable<Book>> GetBooksDataAsync()
        {
            if (_cache.TryGetValue(_cacheKey, out List<Book> cachedBooks))
            {
                return cachedBooks;
            }

            var books = await GetBooksAsync();

            _cache.Set(_cacheKey, books, _cacheDuration);

            return books;
        }
    }
}
