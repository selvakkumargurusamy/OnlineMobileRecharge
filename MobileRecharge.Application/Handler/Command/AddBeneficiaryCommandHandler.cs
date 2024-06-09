namespace MobileRecharge.Application.Handler.Command;

public class AddBeneficiaryCommandHandler : IRequestHandler<AddBeneficiaryCommand, int>
{
    private readonly IBeneficiaryService _beneficiaryService;
    public AddBeneficiaryCommandHandler(IBeneficiaryService beneficiaryService)
    {
        _beneficiaryService = beneficiaryService;
    }

    public async Task<int> Handle(AddBeneficiaryCommand addBeneficiaryCommand, CancellationToken cancellationToken)
    {
        return await _beneficiaryService.AddBeneficiaryAsync(addBeneficiaryCommand.UserId, addBeneficiaryCommand.Request);
    }
}
