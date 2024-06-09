namespace MobileRecharge.Application.Services;

public class RechargeTransactionService : IRechargeTransactionService
{
    private readonly IRechargeTransactionRepository _transactionRepository;
    private readonly IBeneficiaryRepository _beneficiaryRepository;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;
    private readonly IHttpService _httpService;

    public RechargeTransactionService(IRechargeTransactionRepository transactionRepository,
        IMapper mapper,
        IBeneficiaryRepository beneficiaryRepository,
        IOptions<AppSettings> appSettings,
        IHttpService httpService)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
        _beneficiaryRepository = beneficiaryRepository;
        _appSettings = appSettings.Value;
        _httpService = httpService;
    }

    public async Task<IEnumerable<RechargeTransactionDto>> GetAllRechargeTransactionsForUser(int userId)
    {
        var transactionList = await _transactionRepository.GetAllRechargeTransactionsForUserAsync(userId);
        return _mapper.Map<IEnumerable<RechargeTransactionDto>>(transactionList);
    }

    public async Task<RechargeTransactionDto> GetRechargeTransactionByIdAsync(int transactionId)
    {
        var entity = await _transactionRepository.GetRechargeTransactionByIdAsync(transactionId);
        return _mapper.Map<RechargeTransactionDto>(entity);
    }

    public async Task<bool> ProcessRecharge(RechargeDto rechargeRequest)
    {
        var beneficiaryEntity = await _beneficiaryRepository.GetBeneficiaryByIdAsync(rechargeRequest.BeneficiaryId);
        if (beneficiaryEntity == null)
            throw new Exception($"Beneficiary Not Found. Id : {rechargeRequest.BeneficiaryId}");

        await EnsureBenificiaryHasLimitToRecharge(beneficiaryEntity, rechargeRequest);

        await EnsureUserHasLimitToRecharge(beneficiaryEntity, rechargeRequest);

        await EnsureUserHasBalanceToRecharge(beneficiaryEntity, rechargeRequest);

        await DoPayment(beneficiaryEntity.UserId, GetTotalRechargeAmount(rechargeRequest));

        return await AddRechargeTransaction(beneficiaryEntity, rechargeRequest);
    }

    private async Task<bool> AddRechargeTransaction(Beneficiarie beneficiaryEntity, RechargeDto rechargeRequest)
    {
        var rechargeTransaction = new RechargeTransaction()
        {
            BankTransactionId = Guid.NewGuid().ToString(),
            BeneficiaryId = beneficiaryEntity.Id,
            Date = DateTime.Now,
            IsSuccess = true,
            ServiceCharge = 1,
            RechargeAmount = rechargeRequest.Amount,
            TotalAmount = GetTotalRechargeAmount(rechargeRequest),
            UserId = beneficiaryEntity.UserId
        };
        var result = await _transactionRepository.AddRechargeTransactionAsync(rechargeTransaction);
        return result > 0;
    }

    private async Task<bool> DoPayment(int userId, int totalRechargeAmount)
    {
        var paymentResponseSuccess = await _httpService.PostAsync<PaymentDto, bool>("Payment/recharge", new PaymentDto() { UserId = userId, Amount = totalRechargeAmount });

        if (!paymentResponseSuccess)
            throw new Exception($"Payment Failed");

        return true;
    }

    private async Task<bool> EnsureBenificiaryHasLimitToRecharge(Beneficiarie beneficiaryEntity, RechargeDto rechargeRequest)
    {
        var benificiaryTotalRechargedAmountForThisMonth = await _transactionRepository.GetBeneficiaryRechargedAmountForCurrentMonth(rechargeRequest.BeneficiaryId);

        var totalBenificiaryRechargedAmount = benificiaryTotalRechargedAmountForThisMonth + rechargeRequest.Amount;

        int benificiaryMaxLimitForRechargeCurrentMonth = GetBeneficiaryMaxLimitForRechargePerMonth(beneficiaryEntity);

        if (totalBenificiaryRechargedAmount > benificiaryMaxLimitForRechargeCurrentMonth)
            throw new Exception($"Benificiary maximum allowed recharge limit exceed for this month. Limit: {benificiaryMaxLimitForRechargeCurrentMonth}, Beneficiary: {beneficiaryEntity.NickName}, Total Topup: {benificiaryTotalRechargedAmountForThisMonth},  Requested TopUp: {rechargeRequest.Amount}.");

        return true;
    }

    private async Task<bool> EnsureUserHasLimitToRecharge(Beneficiarie beneficiaryEntity, RechargeDto rechargeRequest)
    {
        var userTotalRechargedAmountForThisMonth = await _transactionRepository.GetUserRechargedAmountForCurrentMonth(beneficiaryEntity.UserId);

        var totalUserRechargedAmount = userTotalRechargedAmountForThisMonth + rechargeRequest.Amount;

        int userMaxLimitForRechargePerMonth = GetUserMaxLimitForRechargePerMonth();

        if (totalUserRechargedAmount > userMaxLimitForRechargePerMonth)
            throw new Exception($"User maximum allowed recharge limit exceed for this month. Limit: {userMaxLimitForRechargePerMonth}, Beneficiary: {beneficiaryEntity.NickName}, Total Topup: {userTotalRechargedAmountForThisMonth},  Requested TopUp: {rechargeRequest.Amount}.");

        return true;
    }

    private async Task<bool> EnsureUserHasBalanceToRecharge(Beneficiarie beneficiaryEntity, RechargeDto rechargeRequest)
    {
        int availableBalace = await _httpService.GetAsync<int>(string.Format("Payment/balance/{0}", beneficiaryEntity.UserId));

        int totalRechargeAmount = GetTotalRechargeAmount(rechargeRequest);

        if (totalRechargeAmount > availableBalace)
            throw new Exception($"Insufficient balance. Requested TopUp: {rechargeRequest.Amount}, Available Balance: {availableBalace}");

        return true;
    }

    private int GetTotalRechargeAmount(RechargeDto rechargeRequest)
    {
        int serviceCharge = 1;
        int totalRechargeAmount = rechargeRequest.Amount + serviceCharge; // Including Service charge AED 1
        return totalRechargeAmount;
    }

    private int GetBeneficiaryMaxLimitForRechargePerMonth(Beneficiarie beneficiaryDetails)
    {
        return beneficiaryDetails.User.IsVerified ?
                 _appSettings.MaxRechargePerMonthPerVerifiedUser :
                 _appSettings.MaxRechargePerMonthPerUnVerifiedUser;
    }

    private int GetUserMaxLimitForRechargePerMonth()
    {
        return _appSettings.MaxRechargePerMonthForUser;
    }
}

public class PaymentDto
{
    public int UserId { get; set; }
    public int Amount { get; set; }
}