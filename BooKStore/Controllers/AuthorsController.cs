using BooKStore.Dtos.Author;
using BooKStore.Dtos.Book;
using BooKStore.HTTP.Responses;
using BooKStore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BooKStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly AuthorService _authorService;

        public AuthorsController(AuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _authorService.GetAllAuthors();
            return ApiResponse.ToResponse(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _authorService.GetAuthorById(id);
            return ApiResponse.ToResponse(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add(CreateAuthorDto dto)
        {
            var result = await _authorService.CreateAuthor(dto);
            return ApiResponse.ToCreatedResponse(
                result,
                nameof(GetById),
                new { id = result.Data?.Id },
                this
                );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateAuthorDto dto)
        {
            var result = await _authorService.UpdateAuthor(id, dto);
            return ApiResponse.ToResponse(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _authorService.DeleteAuthor(id);
            return ApiResponse.ToResponse(result);
        }

        [HttpGet("{authorId}/books")]
        public async Task<IActionResult> GetBooksByAuthorId(int authorId)
        {
            var result = await _authorService.GetBooksByAuthorId(authorId);
            return ApiResponse.ToResponse(result);
        }

        [HttpPost("{authorId}/books")]
        public async Task<IActionResult> CreateBookForSpecificAuthor(int authorId, BookForSpecificAuthorDto dto)
        {
            var result = await _authorService.createBookForAuthor(authorId, dto);
            return ApiResponse.ToResponse(result);
        }

        [HttpGet("{authorId}/books/{bookId}")]
        public async Task<IActionResult> GetBookByAuthorId(int authorId, int bookId)
        {
            var result = await _authorService.GetBookByAuthorId(authorId, bookId);
            return ApiResponse.ToResponse(result);
        }

        [HttpPut("{authorId}/books/{bookId}")]
        public async Task<IActionResult> upadateBookByAuthorId(int authorId, int bookId, BookForSpecificAuthorDto updateDto)
        {
            var result = await _authorService.updateBookByAuthorIdServ(authorId, bookId, updateDto);
            return ApiResponse.ToResponse(result);
        }

        [HttpDelete("{authorId}/books/{bookId}")]
        public async Task<IActionResult> deleteBookByAuthorId(int authorId, int bookId) 
        {
            var result = await _authorService.deleteBookByAuthorIdServ(authorId, bookId);
            return ApiResponse.ToResponse(result);
        }

    }
}
