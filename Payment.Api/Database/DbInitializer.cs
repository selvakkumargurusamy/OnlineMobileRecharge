using Payment.Api.Model;

namespace Payment.Api.Database;

// Infrastructure/Data/DbInitializer.cs
public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.EnsureCreated(); // Ensure that the database is created

        // Check if there is already data in the database
        if (context.UserBalances.Any())
        {
            return; // Database has been seeded
        }

        // Seed initial data
        var userBalance =
           new UserBalance { UserId = 1,Balance = 5000 };


        context.UserBalances.Add(userBalance);

        context.SaveChanges();
    }
}
