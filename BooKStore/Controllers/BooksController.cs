using BooKStore.Dtos.Book;
using BooKStore.HTTP.Responses;
using BooKStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace BooKStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _bookService.GetAllBooks();
            return ApiResponse.ToResponse(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _bookService.GetBookById(id);
            return ApiResponse.ToResponse(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateBookDto dto)
        {
            var result = await _bookService.AddBook(dto);

            return ApiResponse.ToCreatedResponse(
                result,
                nameof(GetById),
                new { id = result.Data?.Id },
                this
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateBookDto dto)
        {
            var result = await _bookService.UpdateBook(id, dto);
            return ApiResponse.ToResponse(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _bookService.DeleteBook(id);
            return ApiResponse.ToResponse(result);
        }
    }
}