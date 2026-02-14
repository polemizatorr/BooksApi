using BooksApi.Dto;
using BooksApi.Enums;
using BooksApi.Interface;
using BooksApi.Pagination;
using BooksApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BooksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorsService _authorService;

        public AuthorsController(IAuthorsService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAuthors(int page = 1, int pageSize = 20)
        {
            var authors = await _authorService.GetAllAuthorsAsync();
            var pagedResult = PaginationExtensions.ToPagedResult(authors, page, pageSize);
            return Ok(pagedResult);
        }

        [HttpGet("sortedByName")]
        public async Task<ActionResult<IEnumerable<Book>>> GetOrderedAuthors(SortDirection sortDirection = SortDirection.Ascending, int page = 1, int pageSize = 20)
        {
            var authors = await _authorService.GetOderedAuthorsAsync(sortDirection);
            var pagedResult = PaginationExtensions.ToPagedResult(authors, page, pageSize);
            return Ok(pagedResult);
        }

    }
}
