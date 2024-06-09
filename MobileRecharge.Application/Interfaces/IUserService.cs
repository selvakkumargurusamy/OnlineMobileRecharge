namespace MobileRecharge.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();

    Task<UserDto?> GetUsersAsync(int id);

    Task<IEnumerable<BeneficiaryDto>> GetUserBeneficiariesAsync(int userId);

    Task<int> CreateUserAsync(UserDto user);

    Task<bool> UpdateUserAsync(int id, UserDto user);
}