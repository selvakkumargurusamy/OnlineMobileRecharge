namespace MobileRecharge.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UserController : Controller
{
    private IUserService _userService;
    private AppSettings _appSettings;
    private IMediator _mediator;

    public UserController(IUserService accountService, IOptions<AppSettings> appSettings, IMediator mediator)
    {
        _userService = accountService;
        _appSettings = appSettings.Value;
        _mediator = mediator;
    }

    [HttpGet]
    [Route("")]
    [ProducesResponseType(typeof(IEnumerable<UserDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetAllUsers()
    {
        var query = new GetAllUsersQuery();
        var result = await _mediator.Send(query); 
        return result.Any() ? Ok(result) : NotFound();
    }


    [HttpGet]
    [Route("{{userId}}")]
    [ProducesResponseType(typeof(IEnumerable<BeneficiaryDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetUserBeneficiaries(int userId)
    {
        var query = new GetUserBeneficiariesQueries(userId);
        var result = await _mediator.Send(query);
        return result.Any() ? Ok(result) : NotFound();
    }

    [HttpGet]
    [Route("recharge-option")]
    public IEnumerable<int> GetRechargeOption()
    {
        return _appSettings.AllowedRechargePlans.ToList();
    }
}
