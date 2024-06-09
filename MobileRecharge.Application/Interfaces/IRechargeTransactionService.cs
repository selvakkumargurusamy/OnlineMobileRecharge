namespace MobileRecharge.Application.Interfaces;

public interface IRechargeTransactionService
{
    Task<bool> ProcessRecharge(RechargeDto rechargeRequest);

}