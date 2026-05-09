using BooKStore.Data;
using BooKStore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BooKStore.Repositories
{
    public class MainRepository<T> : IRepository<T> where T : class
    {
        protected ApplicationDbContext _context;

        public MainRepository(ApplicationDbContext context) 
        {
            _context = context;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _context.Set<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in GetAllAsync");
                return Enumerable.Empty<T>();
            }

        }

        public async Task<T?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<T>().FindAsync(id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in GetByIdAsync");
                return null;
            }
        }

        public async Task AddAsync(T entity)
        {
            try
            {
                await _context.Set<T>().AddAsync(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in AddAsync");
            }
        }


        public void Update(T entity)
        {
            try
            {
                _context.Set<T>().Update(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in Update");
            }
        }

        public void Delete(T entity)
        {
            try
            {
                _context.Set<T>().Remove(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in Delete");
            }
        }

    }
}
