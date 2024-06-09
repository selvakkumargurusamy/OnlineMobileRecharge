namespace MobileRecharge.Application.Handler.Query;

public class GetAllBeneficiariesByUserQueriesHandler : IRequestHandler<GetAllBeneficiariesByUserQueries, IEnumerable<BeneficiaryDto>>
{

    private readonly IBeneficiaryService _beneficiaryService;
    public GetAllBeneficiariesByUserQueriesHandler(IBeneficiaryService beneficiaryService)
    {
        _beneficiaryService = beneficiaryService;
    }
    public async Task<IEnumerable<BeneficiaryDto>> Handle(GetAllBeneficiariesByUserQueries request, CancellationToken cancellationToken)
    {
        var response = await _beneficiaryService.GetAllBeneficiariesAsync(request.UserId);
        return response;
    }
}
