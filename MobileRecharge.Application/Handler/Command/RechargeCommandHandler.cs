namespace MobileRecharge.Application.Handler.Command;

public class RechargeCommandHandler : IRequestHandler<RechargeCommand, bool>
{
    private readonly IRechargeTransactionService _rechargeTransactionService;
    public RechargeCommandHandler(IRechargeTransactionService rechargeTransactionService)
    {
        _rechargeTransactionService = rechargeTransactionService;
    }

    public async Task<bool> Handle(RechargeCommand rechargeCommand, CancellationToken cancellationToken)
    {
        return await _rechargeTransactionService.ProcessRecharge(rechargeCommand.Request);
    }
}
