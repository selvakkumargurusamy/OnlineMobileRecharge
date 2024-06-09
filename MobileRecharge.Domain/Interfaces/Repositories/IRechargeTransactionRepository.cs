namespace MobileRecharge.Domain.Interfaces.Repositories;

public interface IRechargeTransactionRepository
{
    public Task<int> AddRechargeTransactionAsync(RechargeTransaction transaction);

    public Task<RechargeTransaction?> GetRechargeTransactionByIdAsync(int transactionId);

    public Task<IEnumerable<RechargeTransaction>> GetAllRechargeTransactionsForUserAsync(int userId);

    public Task<int> GetUserRechargedAmountForCurrentMonth(int userId);

    public Task<int> GetBeneficiaryRechargedAmountForCurrentMonth(int beneficiaryId);

}
