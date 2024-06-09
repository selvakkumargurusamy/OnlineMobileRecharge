namespace MobileRecharge.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsersAsync();

    Task<User?> GetUsersAsync(int id);

    Task<IEnumerable<Beneficiarie>> GetUserBeneficiariesAsync(int userId);

    Task<int> CreateUserAsync(User user);

    Task<bool> UpdateUserAsync(int id, User user);
}
