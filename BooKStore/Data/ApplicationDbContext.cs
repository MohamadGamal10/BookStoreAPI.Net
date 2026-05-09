using BooKStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BooKStore.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }


    }
}
