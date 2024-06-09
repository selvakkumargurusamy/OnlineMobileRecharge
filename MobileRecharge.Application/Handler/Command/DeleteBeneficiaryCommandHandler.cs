namespace MobileRecharge.Application.Handler.Command;

public class DeleteBeneficiaryCommandHandler : IRequestHandler<DeleteBeneficiaryCommand, bool>
{
    private readonly IBeneficiaryService _beneficiaryService;
    public DeleteBeneficiaryCommandHandler(IBeneficiaryService beneficiaryService)
    {
        _beneficiaryService = beneficiaryService;
    }

    public async Task<bool> Handle(DeleteBeneficiaryCommand deleteBeneficiaryCommand, CancellationToken cancellationToken)
    {
        return await _beneficiaryService.DeleteBeneficiaryAsync(deleteBeneficiaryCommand.Id);
    }
}
