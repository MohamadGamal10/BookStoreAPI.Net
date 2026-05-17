using BooKStore.Dtos.Author;
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
    }
}
