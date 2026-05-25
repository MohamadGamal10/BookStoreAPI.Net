using BooKStore.Data;
using BooKStore.Interfaces;
using BooKStore.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BooKStore.Repositories
{
    public class AuthorRepository : MainRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(ApplicationDbContext context) : base(context)
        {

        }
        public async Task<Author?> GetAuthorWithBooksAsync(int id)
        {
            try
            {
                return await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error fetching author with books for ID {AuthorId}", id);
                return null;
            }
        }
        public async Task<Book?> CreateBookForAuthorAsync(int authorId, Book book)
        {
            try
            {
                 await _context.Books
                .AddAsync(book);
                return book;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating book for Author ID {AuthorId}", authorId);
                return null;
            }
        }

        public async Task<Book?> GetBookWithBookIdAndAuthorIdAsync(int authorId, int bookId)
        {
            try
            {
                return await _context.Books
           .FirstOrDefaultAsync(b => b.Id == bookId && b.AuthorId == authorId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error fetching book ID {BookId} for Author ID {AuthorId}", bookId, authorId);
                return null;
            }
        }

        public void updateBookWithBookIdAndAuthorId(Book book)
        {
            try
            {
                _context.Books.Update(book);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating book ID {BookId} for Author ID {AuthorId}", book.Id, book.AuthorId);
            }
        }

        public void deleteBookWithBookIdAndAuthorId(Book book)
        {
            try
            {
                _context.Books.Remove(book);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error deleting book ID {BookId} for Author ID {AuthorId}", book.Id, book.AuthorId);
            }
        }

    }
}
