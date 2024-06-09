

namespace MobileRecharge.Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            DbInitializer.Initialize(this);
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserBalance> UserBalances { get; set; } = null!;
        public DbSet<Beneficiarie> Beneficiaries { get; set; } = null!;
        public DbSet<RechargeTransaction> RechargeTransactions { get; set; } = null!;

    }
}
