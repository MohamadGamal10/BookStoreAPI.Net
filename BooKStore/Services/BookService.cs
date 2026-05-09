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

        public async Task<Result<IEnumerable<Book>>> GetAllBooks()
        {
            var books = await _unitOfWork.Books.GetAllAsync();

            if (!books.Any())
                return Result<IEnumerable<Book>>.Fail("No books found");

            return Result<IEnumerable<Book>>.Ok(books, "Books retrieved successfully");
        }

        public async Task<Result<Book>> GetBookById(int id)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id);

            if (book == null)
                return Result<Book>.Fail("Book not found");

            return Result<Book>.Ok(book);
        }

        public async Task<Result<Book>> AddBook(CreateBookDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                return Result<Book>.Fail("Title is required");

            if (string.IsNullOrWhiteSpace(dto.Author))
                return Result<Book>.Fail("Author is required");

            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author
            };
            await _unitOfWork.Books.AddAsync(book);
            await _unitOfWork.CompleteAsync();

            return Result<Book>.Ok(book, "Book created successfully");
        }

        public async Task<Result<Book>> UpdateBook(int id, UpdateBookDto dto)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id);

            if (book == null)
                return Result<Book>.Fail("Book not found");

            if (string.IsNullOrWhiteSpace(dto.Title))
                return Result<Book>.Fail("Title is required");

            if (string.IsNullOrWhiteSpace(dto.Author))
                return Result<Book>.Fail("Author is required");

            book.Title = dto.Title;
            book.Author = dto.Author;

            _unitOfWork.Books.Update(book);
            await _unitOfWork.CompleteAsync();

            return Result<Book>.Ok(book, "Book Updated successfully");
        }
        public async Task<Result<Book>> DeleteBook(int id)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id);

            if (book == null)
                return Result<Book>.Fail("Book not found");

            _unitOfWork.Books.Delete(book);
            await _unitOfWork.CompleteAsync();

            return Result<Book>.Ok(book, "Book deleted successfully");
        }



    }
}
