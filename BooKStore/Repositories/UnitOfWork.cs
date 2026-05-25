using BooKStore.Data;
using BooKStore.Interfaces;
using BooKStore.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BooKStore.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IRepository<Book> Books { get; private set; }
        public IAuthorRepository Authors { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Books = new MainRepository<Book>(_context);
            Authors = new AuthorRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in CompleteAsync");
                throw;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
