using BooKStore.Models;

namespace BooKStore.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IRepository<Book> Books { get; }
        IRepository<Author> Authors { get; }
        Task<int> CompleteAsync();
    }
}
