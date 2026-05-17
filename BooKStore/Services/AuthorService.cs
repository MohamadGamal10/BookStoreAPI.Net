using AutoMapper;
using BooKStore.Dtos.Author;
using BooKStore.Dtos.Book;
using BooKStore.HTTP;
using BooKStore.Interfaces;
using BooKStore.Models;

namespace BooKStore.Services
{
    public class AuthorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuthorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<AuthorDto>>> GetAllAuthors()
        {
            var authors = await _unitOfWork.Authors.GetAllAsync();

            if (!authors.Any())
                return Result<IEnumerable<AuthorDto>>.Fail("No authors found");

            var authorsDto =  _mapper.Map<List<AuthorDto>>(authors);
            return Result<IEnumerable<AuthorDto>>.Ok(authorsDto);
        }
        public async Task<Result<AuthorDto>> GetAuthorById(int id)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id);

            if (author == null)
                return Result<AuthorDto>.Fail("Author not found");

            var authorDto =  _mapper.Map<AuthorDto>(author);
            return Result<AuthorDto>.Ok(authorDto);
        }
        public async Task<Result<AuthorDto>> CreateAuthor(CreateAuthorDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return Result<AuthorDto>.Fail("Name is required");

            var books = dto.Books != null
                ? _mapper.Map<List<Book>>(dto.Books)
                : new List<Book>();
            var author = new Author
            {
                Name = dto.Name,
                BirthDate = dto.BirthDate,
                Books = books
            };

            await _unitOfWork.Authors.AddAsync(author);
            await _unitOfWork.CompleteAsync();

            var authorDto =  _mapper.Map<AuthorDto>(author);
            return Result<AuthorDto>.Ok(authorDto);
        }
    }
}
