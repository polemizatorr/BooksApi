using BooksApi.Dto;
using BooksApi.Enums;
using BooksApi.Interface;
using BooksApi.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace BooksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _booksService;

        public BooksController(IBookService booksService)
        {
            _booksService = booksService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks(int page = 1, int pageSize = 20)
        {
            var books = await _booksService.GetAllBooksAsync();
            var pagedResult = PaginationExtensions.ToPagedResult(books, page, pageSize);
            return Ok(pagedResult);
        }

        [HttpGet("ByAuthor")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksByAuthor([FromQuery] string authorName, int page = 1, int pageSize = 20)
        {
            var books = await _booksService.GetBooksByAuthorAsync(authorName);
            var pagedResult = PaginationExtensions.ToPagedResult(books, page, pageSize);
            return Ok(pagedResult);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBookById(string id)
        {
            var book = await _booksService.GetBookById(id);
            return book is not null ? Ok(book) : NotFound();
        }

        [HttpGet("Sorted")]
        public async Task<ActionResult<IEnumerable<Book>>> GetOrderedBooks(BookSortFields field, SortDirection sortDirection, int page = 1, int pageSize = 20)
        {
            var books = await _booksService.GetOderedBooksAsync(field, sortDirection);
            var pagedResult = PaginationExtensions.ToPagedResult(books, page, pageSize);
            return Ok(pagedResult);
        }

        [HttpGet("Filtered")]
        public async Task<ActionResult<IEnumerable<Book>>> GetFilteredBooks(BookFilterFields field, string filterValue, int page = 1, int pageSize = 20)
        {
            var books = await _booksService.GetFilteredBooksAsync(field, filterValue);
            var pagedResult = PaginationExtensions.ToPagedResult(books, page, pageSize);
            return Ok(pagedResult);
        }

    }
}
