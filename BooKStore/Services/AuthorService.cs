using AutoMapper;
using BooKStore.Dtos.Author;
using BooKStore.Dtos.Book;
using BooKStore.HTTP;
using BooKStore.Interfaces;
using BooKStore.Models;
using static System.Reflection.Metadata.BlobBuilder;

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
                return Result<IEnumerable<AuthorDto>>.Fail("Authors not found");

            var authorsDto = _mapper.Map<List<AuthorDto>>(authors);
            return Result<IEnumerable<AuthorDto>>.Ok(authorsDto, "Books retrieved successfully!");
        }
        public async Task<Result<AuthorDto>> GetAuthorById(int id)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id);

            if (author == null)
                return Result<AuthorDto>.Fail("Author not found");

            var authorDto = _mapper.Map<AuthorDto>(author);
            return Result<AuthorDto>.Ok(authorDto, "Book retrieved successfully!");
        }
        public async Task<Result<AuthorDto>> CreateAuthor(CreateAuthorDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return Result<AuthorDto>.Fail("Name is required");

            if (dto.BirthDate > DateOnly.FromDateTime(DateTime.Now))
                return Result<AuthorDto>.Fail("Birth date cannot be in the future");


            var author = new Author
            {
                Name = dto.Name,
                BirthDate = dto.BirthDate,
            };

            await _unitOfWork.Authors.AddAsync(author);
            await _unitOfWork.CompleteAsync();

            var authorDto = _mapper.Map<AuthorDto>(author);
            return Result<AuthorDto>.Ok(authorDto, "Author created successfully!");
        }

        public async Task<Result<AuthorDto>> UpdateAuthor(int id, UpdateAuthorDto dto)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id);

            if (author == null)
                return Result<AuthorDto>.Fail("Author not found");

            _mapper.Map(dto, author);
            await _unitOfWork.CompleteAsync();

            var authorDto = _mapper.Map<AuthorDto>(author);
            return Result<AuthorDto>.Ok(authorDto, "Author Updated successfully!");
        }

        public async Task<Result<AuthorDto>> DeleteAuthor(int id)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id);
            if (author == null)
                return Result<AuthorDto>.Fail("Author not found");
            _unitOfWork.Authors.Delete(author);
            await _unitOfWork.CompleteAsync();
            return Result<AuthorDto>.Ok(_mapper.Map<AuthorDto>(author), "Author deleted successfully!");
        }

        public async Task<Result<IEnumerable<BookDto>>> GetBooksByAuthorId(int authorId)
        {
            var author = await _unitOfWork.Authors.GetAuthorWithBooksAsync(authorId);
            if (author == null)
                return Result<IEnumerable<BookDto>>.Fail("Author not found");
            var booksDto = _mapper.Map<IEnumerable<BookDto>>(author.Books);
            return Result<IEnumerable<BookDto>>.Ok(booksDto, "Books for specific Author retrieved successfully!");
        }

        public async Task<Result<BookDto>> createBookForAuthor(int authorId, BookForSpecificAuthorDto dto)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(authorId);
            if (author == null)
            {
                return Result<BookDto>.Fail("Author not found");
            }

            var book = _mapper.Map<Book>(dto);
            book.AuthorId = authorId;
            await _unitOfWork.Books.AddAsync(book);
            await _unitOfWork.CompleteAsync();

            return Result<BookDto>.Ok(_mapper.Map<BookDto>(book), "Book for specific Author created successfully!");
        }

        public async Task<Result<BookDto>> GetBookByAuthorId(int authorId, int bookId)
        {
            var BookByAuthor = await _unitOfWork.Authors.GetBookWithBookIdAndAuthorIdAsync(authorId, bookId);
            if (BookByAuthor == null)
                return Result<BookDto>.Fail("Book for specific Author not found");

            var author = await _unitOfWork.Authors.GetByIdAsync(authorId);
            if (author == null)
                return Result<BookDto>.Fail("Author not found");

            var bookDto = _mapper.Map<BookDto>(BookByAuthor);
            return Result<BookDto>.Ok(bookDto, "Book for specific Author retrieved successfully!");
        }

        public async Task<Result<BookDto>> updateBookByAuthorIdServ(int authorId, int bookId, BookForSpecificAuthorDto dto)
        {
            var BookByAuthor = await _unitOfWork.Authors.GetBookWithBookIdAndAuthorIdAsync(authorId, bookId);
            if (BookByAuthor == null)
                return Result<BookDto>.Fail("Book for specific Author not found");

            var author = await _unitOfWork.Authors.GetByIdAsync(authorId);
            if (author == null)
                return Result<BookDto>.Fail("Author not found");

            //var book = _mapper.Map<Book>(dto);
            BookByAuthor.AuthorId = authorId;
            BookByAuthor.Id = bookId;
            BookByAuthor.Title = dto.Title;
            BookByAuthor.publishedDate = dto.publishedDate;


            _unitOfWork.Authors.updateBookWithBookIdAndAuthorId(BookByAuthor);
            await _unitOfWork.CompleteAsync();

            return Result<BookDto>.Ok(_mapper.Map<BookDto>(BookByAuthor), "Book for specific Author updated successfully!");

        }

        public async Task<Result<BookDto>> deleteBookByAuthorIdServ(int authorId, int bookId)
        {
            var BookByAuthor = await _unitOfWork.Authors.GetBookWithBookIdAndAuthorIdAsync(authorId, bookId);
            if (BookByAuthor == null)
                return Result<BookDto>.Fail("Book for specific Author not found");

            var author = await _unitOfWork.Authors.GetByIdAsync(authorId);
            if (author == null)
                return Result<BookDto>.Fail("Author not found");

            _unitOfWork.Authors.deleteBookWithBookIdAndAuthorId(BookByAuthor);
            //_mapper.Map(dto, author);
            await _unitOfWork.CompleteAsync();

            return Result<BookDto>.Ok(_mapper.Map<BookDto>(BookByAuthor), "Book for specific Author deleted successfully!");
        }

    }
}
