using AutoMapper;
using BooKStore.Dtos.Book;
using BooKStore.HTTP;
using BooKStore.Interfaces;
using BooKStore.Models;

namespace BooKStore.Services
{
    public class BookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<BookDto>>> GetAllBooks()
        {
            var books = await _unitOfWork.Books.GetAllAsync();

            if (!books.Any())
                return Result<IEnumerable<BookDto>>.Fail("No books found");

            var booksDto = _mapper.Map<IEnumerable<BookDto>>(books);

            return Result<IEnumerable<BookDto>>.Ok(booksDto, "Books retrieved successfully");
        }

        public async Task<Result<BookDto>> GetBookById(int id)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id);

            if (book == null)
                return Result<BookDto>.Fail("Book not found");

            var bookDto = _mapper.Map<BookDto>(book);

            return Result<BookDto>.Ok(bookDto);
        }

        public async Task<Result<BookDto>> AddBook(CreateBookDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                return Result<BookDto>.Fail("Title is required");

            if (dto.AuthorId == null || dto.AuthorId == 0)
                return Result<BookDto>.Fail("Author is required");

            var author = await _unitOfWork.Authors.GetByIdAsync(dto.AuthorId);
            if (author == null)
            {
                return Result<BookDto>.Fail("Author not found");
            }

            var book = new Book
            {
                Title = dto.Title,
                AuthorId = dto.AuthorId,
                publishedDate = dto.publishedDate
            };
            await _unitOfWork.Books.AddAsync(book);
            await _unitOfWork.CompleteAsync();

            var bookDto = _mapper.Map<BookDto>(book);

            return Result<BookDto>.Ok(bookDto, "Book created successfully");
        }

        public async Task<Result<BookDto>> UpdateBook(int id, UpdateBookDto dto)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id);

            if (book == null)
                return Result<BookDto>.Fail("Book not found");

            if (string.IsNullOrWhiteSpace(dto.Title))
                return Result<BookDto>.Fail("Title is required");

            if (dto.AuthorId == null)
                return Result<BookDto>.Fail("Author is required");

            book.Title = dto.Title;
            book.AuthorId = dto.AuthorId;

            _unitOfWork.Books.Update(book);
            await _unitOfWork.CompleteAsync();

            var bookDto = _mapper.Map<BookDto>(book);

            return Result<BookDto>.Ok(bookDto, "Book Updated successfully");
        }
        public async Task<Result<BookDto>> DeleteBook(int id)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id);

            if (book == null)
                return Result<BookDto>.Fail("Book not found");

            _unitOfWork.Books.Delete(book);
            await _unitOfWork.CompleteAsync();

            var bookDto = _mapper.Map<BookDto>(book);
            return Result<BookDto>.Ok(bookDto, "Book deleted successfully");
        }



    }
}
