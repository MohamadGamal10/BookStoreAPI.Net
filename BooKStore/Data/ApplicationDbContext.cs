using BooKStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BooKStore.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer("Server=.;Database=BookStore;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true",
                options => options.EnableRetryOnFailure());
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }


    }
}
