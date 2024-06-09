namespace MobileRecharge.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class RechargeTransactionController : Controller
{
    private readonly IRechargeTransactionService _rechargeService;
    private readonly AppSettings _appSettings;
    private IMediator _mediator;

    public RechargeTransactionController(IRechargeTransactionService rechargeService, IOptions<AppSettings> appSettings, IMediator mediator)
    {
        _rechargeService = rechargeService;
        _appSettings = appSettings.Value;
        _mediator = mediator;
    }

    [HttpPost]
    [Route("recharge")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Recharge([FromBody] RechargeDto recharge)
    {
        try
        {
            var plans = _appSettings.AllowedRechargePlans;
            if (!plans.Any(i => i == recharge.Amount))
            {
                return BadRequest($"Enter a valid recharge amount. Available Plans: {string.Join(",", plans)}");
            }

            var query = new RechargeCommand(recharge);
            var result = await _mediator.Send(query);

            if (!result)
                throw new Exception("Something went wrong!");

            return result ? Ok(result) : NotFound();     
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Internal Server Error", ErrorCode = 500, Details = ex.Message });
        }


    }

}
