

using MobileRecharge.Domain.Models;

namespace MobileRecharge.Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //DbInitializer.Initialize(this);
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Beneficiarie> Beneficiaries { get; set; } = null!;
        public DbSet<UserBalance> UserBalances { get; set; } = null!;
        public DbSet<RechargeTransaction> RechargeTransactions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasKey(e => e.Id);
            modelBuilder.Entity<User>().Property(e => e.Id).ValueGeneratedOnAdd();

            var user = new User
            {
                Id = 1,
                FirstName = "Selva",
                LastName = "Kumar",
                Email = "selva@gmail.com",
                IsVerified = true
            };
            modelBuilder.Entity<User>().HasData(user);

            modelBuilder.Entity<Beneficiarie>().HasKey(e => e.Id);
            modelBuilder.Entity<Beneficiarie>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Beneficiarie>().HasData(
                            new Beneficiarie
                            {
                                Id = 1,
                                UserId = 1,
                                NickName = "Selva",
                                PhoneNumber = "+971-70-39380474"
                            },
                            new Beneficiarie
                            {
                                Id = 2,
                                UserId = 1,
                                NickName = "Kumar",
                                PhoneNumber = "+971-70-30666969"
                            }
                );

            modelBuilder.Entity<UserBalance>().HasKey(e => e.Id);
            modelBuilder.Entity<UserBalance>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<UserBalance>().HasData(
                            new UserBalance
                            {
                                Id = 1,
                                UserId = 1,
                                Balance = 5000
                            }
           );

            modelBuilder.Entity<RechargeTransaction>()
                .HasOne<User>(s => s.User)
                .WithMany(s => s.RechargeTransactions)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
