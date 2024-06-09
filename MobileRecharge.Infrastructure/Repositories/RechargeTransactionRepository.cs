namespace MobileRecharge.Infrastructure.Repositories;

public class RechargeTransactionRepository : IRechargeTransactionRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<RechargeTransaction> _rechargeTransactions;

    public RechargeTransactionRepository(AppDbContext context)
    {
        _context = context;
        _rechargeTransactions = _context.RechargeTransactions;
    }

    public async Task<int> AddRechargeTransactionAsync(RechargeTransaction transaction)
    {
        if (transaction == null)
        {
            throw new ArgumentNullException(nameof(transaction));
        }

        await _rechargeTransactions.AddAsync(transaction);
        await _context.SaveChangesAsync();

        return transaction.Id;
    }

    public async Task<IEnumerable<RechargeTransaction>> GetAllRechargeTransactionsForUserAsync(int userId)
    {
        return await _rechargeTransactions.Include(a => a.User).Include(a => a.Beneficiary).Where(i => i.UserId == userId).ToListAsync();
    }

    public async Task<RechargeTransaction?> GetRechargeTransactionByIdAsync(int transactionId)
    {
        return await _rechargeTransactions.FindAsync(transactionId);
    }

    public async Task<int> GetUserRechargedAmountForCurrentMonth(int userId)
    {
       return await _rechargeTransactions.Where(a => a.UserId == userId && a.Date.Month == DateTime.Now.Month).SumAsync(a => a.TotalAmount);
    }

    public async Task<int> GetBeneficiaryRechargedAmountForCurrentMonth(int beneficiaryId)
    {
        return await _rechargeTransactions.Where(a => a.BeneficiaryId == beneficiaryId && a.Date.Month == DateTime.Now.Month).SumAsync(a => a.TotalAmount);
    }
}
