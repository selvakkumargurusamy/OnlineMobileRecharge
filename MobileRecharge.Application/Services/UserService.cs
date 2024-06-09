namespace MobileRecharge.Application.Services;

public class UserService : IUserService
{

    private readonly IUserRepository _userRepos;
    private readonly IMapper _mapper;
    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepos = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepos.GetAllUsersAsync();
        return _mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto?> GetUsersAsync(int userId)
    {
        var user = await _userRepos.GetUsersAsync(userId);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<IEnumerable<BeneficiaryDto>> GetUserBeneficiariesAsync(int userId)
    {
        var beneficiaries = await _userRepos.GetUserBeneficiariesAsync(userId);
        return _mapper.Map<IEnumerable<Beneficiarie>, IEnumerable<BeneficiaryDto>>(beneficiaries);
    }

    public Task<int> CreateUserAsync(UserDto userDto)
    {
        var user = _mapper.Map<User>(userDto);
        return _userRepos.CreateUserAsync(user);
    }

    public Task<bool> UpdateUserAsync(int id, UserDto userDto)
    {
        var user = _mapper.Map<User>(userDto);
        return _userRepos.UpdateUserAsync(id, user);
    }
}
