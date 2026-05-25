using BooKStore.Models;

namespace BooKStore.Interfaces
{
    public interface IAuthorRepository: IRepository<Author>
    {
        Task<Author?> GetAuthorWithBooksAsync(int id);
        Task<Book?> CreateBookForAuthorAsync(int authorId, Book book);
        Task<Book?> GetBookWithBookIdAndAuthorIdAsync(int authorId, int bookId);
        public void updateBookWithBookIdAndAuthorId(Book book);
        public void deleteBookWithBookIdAndAuthorId(Book book);
    }
}
