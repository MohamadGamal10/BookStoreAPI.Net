using BooKStore.Models;
using BooKStore.Repositories;

namespace BooKStore.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IRepository<Book> Books { get; }
        IAuthorRepository Authors { get; }
        //IRepository<Author> Authors { get; }
        Task<int> CompleteAsync();
    }
}
