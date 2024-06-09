using Microsoft.EntityFrameworkCore;
using Payment.Api.Model;

namespace Payment.Api.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            DbInitializer.Initialize(this);
        }
        public DbSet<UserBalance> UserBalances { get; set; } = null!;

    }
}
