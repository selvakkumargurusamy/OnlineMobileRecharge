namespace MobileRecharge.Application.Handler.Command;

public class UpdateBeneficiaryCommandHandler : IRequestHandler<UpdateBeneficiaryCommand, bool>
{
    private readonly IBeneficiaryService _beneficiaryService;
    public UpdateBeneficiaryCommandHandler(IBeneficiaryService beneficiaryService)
    {
        _beneficiaryService = beneficiaryService;
    }

    public async Task<bool> Handle(UpdateBeneficiaryCommand updateBeneficiaryCommand, CancellationToken cancellationToken)
    {
        return await _beneficiaryService.UpdateBeneficiaryAsync(updateBeneficiaryCommand.Id, updateBeneficiaryCommand.Request);
    }
}
