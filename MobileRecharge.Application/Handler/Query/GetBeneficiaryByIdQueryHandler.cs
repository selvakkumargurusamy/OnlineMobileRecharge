namespace MobileRecharge.Application.Handler.Query;

public class GetBeneficiaryByIdQueryHandler : IRequestHandler<GetBeneficiaryByIdQuery, BeneficiaryDto?>
{

    private readonly IBeneficiaryService _beneficiaryService;
    public GetBeneficiaryByIdQueryHandler(IBeneficiaryService beneficiaryService)
    {
        _beneficiaryService = beneficiaryService;
    }
    public async Task<BeneficiaryDto?> Handle(GetBeneficiaryByIdQuery request, CancellationToken cancellationToken)
    {
        var response = await _beneficiaryService.GetBeneficiaryByIdAsync(request.id);
        return response;
    }
}
