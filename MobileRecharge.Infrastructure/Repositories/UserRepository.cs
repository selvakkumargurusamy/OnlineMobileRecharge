namespace MobileRecharge.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DbSet<User> _users;
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext dbContext)
    {
        _context = dbContext;
        _users = dbContext.Users;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _users.ToListAsync();
    }

    public async Task<User?> GetUsersAsync(int id)
    {
        return await _users.FindAsync(id);
    }

    public async Task<IEnumerable<Beneficiarie>> GetUserBeneficiariesAsync(int id)
    {
        var users = await _users.Include(i => i.Beneficiaries).Where(i => i.Id == id).FirstOrDefaultAsync()!;

        return users!.Beneficiaries;
    }

    public async Task<int> CreateUserAsync(User user)
    {
        try
        {
            await _users.AddAsync(user);
            _context.SaveChanges();

            return user.Id;
        }
        catch (Exception)
        {
            return 0;
        }
    }

    public async Task<bool> UpdateUserAsync(int id, User user)
    {
        var userEntity = await _users.FindAsync(id);
        if (userEntity == null)
        {
            return false;
        }
        
        userEntity.FirstName = user.FirstName;
        userEntity.LastName = user.LastName;
        userEntity.Email = user.Email;
        userEntity.IsVerified = user.IsVerified;

        var result = _context.SaveChanges();

        return result > 0;
    }
}
