namespace MobileRecharge.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class BeneficiaryController : Controller
{
    private IMediator _mediator;

    public BeneficiaryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    
    [Route("user/{userId}/getAll")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BeneficiaryDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<IActionResult> GetAll(int userId)
    {

        var query = new GetAllBeneficiariesByUserQueries(userId);
        var result = await _mediator.Send(query);
        return result.Any() ? Ok(result) : NoContent();
    }

    [Route("{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(BeneficiaryDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<IActionResult> Get(int id)
    {
        var query = new GetBeneficiaryByIdQuery(id);
        var result = await _mediator.Send(query);
        return result != null ? Ok(result) : NoContent();
    }

    [Route("user/{userId}/create")]
    [HttpPost]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Create(int userId, [FromBody] BeneficiaryDto beneficiary)
    {
        var query = new AddBeneficiaryCommand(userId, beneficiary);
        var result = await _mediator.Send(query);
        return result > 0 ? Ok(result) : NotFound();
    }

    [Route("{id}")]
    [HttpPut]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] BeneficiaryDto beneficiary)
    {
        var query = new UpdateBeneficiaryCommand(id, beneficiary);
        var result = await _mediator.Send(query);
        return result ? Ok(result) : NotFound();
    }

    [Route("{id}/delete")]
    [HttpPut]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var query = new DeleteBeneficiaryCommand(id);
        var result = await _mediator.Send(query);
        return result ? Ok(result) : NotFound();
    }
}
