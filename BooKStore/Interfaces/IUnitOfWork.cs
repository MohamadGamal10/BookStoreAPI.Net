using BooKStore.Models;

namespace BooKStore.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IRepository<Book> Books { get; }
        Task<int> CompleteAsync();
    }
}
