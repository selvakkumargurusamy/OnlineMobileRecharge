namespace MobileRecharge.Infrastructure.Database;

// Infrastructure/Data/DbInitializer.cs
public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.EnsureCreated(); // Ensure that the database is created

        // Check if there is already data in the database
        if (context.Users.Any())
        {
            return; // Database has been seeded
        }

        // Seed initial data
        var user =
           new User { FirstName = "Selva", LastName = "Kumar", Email = "selva@gmail.com", IsVerified = true };

        var beneficary1 = new Beneficiarie { User = user, NickName = "Selva", PhoneNumber = "+971-70-39380474" };
        var beneficary2 = new Beneficiarie { User = user, NickName = "Kumar", PhoneNumber = "+971-70-30666969" };

        var userBalance = new UserBalance { User = user, Balance = 5000 };

        context.Users.Add(user);
        context.Beneficiaries.Add(beneficary1);
        context.Beneficiaries.Add(beneficary2);
        context.UserBalances.Add(userBalance);

        context.SaveChanges();
    }
}
