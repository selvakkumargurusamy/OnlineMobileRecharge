namespace MobileRecharge.UnitTests.Service;
public class RechargeTransactionServiceTests
{
    private readonly Mock<IRechargeTransactionRepository> _transactionRepository;
    private readonly Mock<IBeneficiaryRepository> _beneficiaryRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IOptions<AppSettings>> _appSettings;
    private readonly Mock<IHttpService> _httpService;

    public RechargeTransactionServiceTests()
    {
        _transactionRepository = new Mock<IRechargeTransactionRepository>();
        _beneficiaryRepository = new Mock<IBeneficiaryRepository>();
        _mapper = new Mock<IMapper>();
        _appSettings = new Mock<IOptions<AppSettings>>();
        _httpService = new Mock<IHttpService>();
    }

    #region Process Recharge
    [Theory, AutoData]
    public async Task ProcessRechargeAsync_ReturnSucessResult_WhenValidInput(RechargeDto rechargeRequest)
    {
        rechargeRequest.Amount = 100;

        //arrange
        var beneficieryEntity = GetBeneficiarie();
        _beneficiaryRepository.Setup(x => x.GetBeneficiaryByIdAsync(rechargeRequest.BeneficiaryId)).ReturnsAsync(beneficieryEntity);
        _transactionRepository.Setup(x => x.GetBeneficiaryRechargedAmountForCurrentMonth(rechargeRequest.BeneficiaryId)).ReturnsAsync(100);
        _transactionRepository.Setup(x => x.GetUserRechargedAmountForCurrentMonth(beneficieryEntity.UserId)).ReturnsAsync(500);
        _httpService.Setup(x => x.GetAsync<int>(It.IsAny<string>())).ReturnsAsync(500);
        _httpService.Setup(x => x.PostAsync<PaymentDto, bool>(It.IsAny<string>(), It.IsAny<PaymentDto>())).ReturnsAsync(true);
        _transactionRepository.Setup(x => x.AddRechargeTransactionAsync(It.IsAny<RechargeTransaction>())).ReturnsAsync(1);
        _appSettings.Setup(x => x.Value).Returns(GetAppSetting());

        var rechargeService = new RechargeTransactionService(_transactionRepository.Object, _mapper.Object, _beneficiaryRepository.Object,
            _appSettings.Object, _httpService.Object);

        //act
        var actual = await rechargeService.ProcessRecharge(rechargeRequest);

        //assert
        Assert.True(actual);
    }

    [Theory, AutoData]
    public async Task ProcessRechargeAsync_ReturnException_WhenBeneficiaryNotExist(RechargeDto rechargeRequest)
    {
        rechargeRequest.Amount = 100;

        //arrange
        _appSettings.Setup(x => x.Value).Returns(GetAppSetting());
        _beneficiaryRepository.Setup(x => x.GetBeneficiaryByIdAsync(rechargeRequest.BeneficiaryId)).ReturnsAsync((Beneficiarie?)null);
        

        var rechargeService = new RechargeTransactionService(_transactionRepository.Object, _mapper.Object, _beneficiaryRepository.Object,
            _appSettings.Object, _httpService.Object);

        //act           
        Exception exception = await Assert.ThrowsAsync<Exception>(() => rechargeService.ProcessRecharge(rechargeRequest));

        //assert
        Assert.Contains("Beneficiary Not Found", exception.Message);
    }

    [Theory, AutoData]
    public async Task ProcessRechargeAsync_ReturnException_WhenBeneficiaryRechargeLimitReached(RechargeDto rechargeRequest)
    {
        rechargeRequest.Amount = 100;

        //arrange
        _appSettings.Setup(x => x.Value).Returns(GetAppSetting());
        var beneficieryEntity = GetBeneficiarie();
        _beneficiaryRepository.Setup(x => x.GetBeneficiaryByIdAsync(rechargeRequest.BeneficiaryId)).ReturnsAsync(beneficieryEntity);
        _transactionRepository.Setup(x => x.GetBeneficiaryRechargedAmountForCurrentMonth(rechargeRequest.BeneficiaryId)).ReturnsAsync(2000);            

        var rechargeService = new RechargeTransactionService(_transactionRepository.Object, _mapper.Object, _beneficiaryRepository.Object,
            _appSettings.Object, _httpService.Object);

        //act           
        Exception exception = await Assert.ThrowsAsync<Exception>(() => rechargeService.ProcessRecharge(rechargeRequest));
        
        //assert
        Assert.Contains("Benificiary maximum allowed recharge limit exceed for this month", exception.Message);
    }

    [Theory, AutoData]
    public async Task ProcessRechargeAsync_ReturnException_WhenUserRechargeLimitReached(RechargeDto rechargeRequest)
    {
        rechargeRequest.Amount = 100;

        //arrange
        _appSettings.Setup(x => x.Value).Returns(GetAppSetting());
        var beneficieryEntity = GetBeneficiarie();
        _beneficiaryRepository.Setup(x => x.GetBeneficiaryByIdAsync(rechargeRequest.BeneficiaryId)).ReturnsAsync(beneficieryEntity);
        _transactionRepository.Setup(x => x.GetBeneficiaryRechargedAmountForCurrentMonth(rechargeRequest.BeneficiaryId)).ReturnsAsync(100);
        _transactionRepository.Setup(x => x.GetUserRechargedAmountForCurrentMonth(beneficieryEntity.UserId)).ReturnsAsync(5000);
        
        var rechargeService = new RechargeTransactionService(_transactionRepository.Object, _mapper.Object, _beneficiaryRepository.Object,
            _appSettings.Object, _httpService.Object);

        //act           
        Exception exception = await Assert.ThrowsAsync<Exception>(() => rechargeService.ProcessRecharge(rechargeRequest));

        //assert
        Assert.Contains("User maximum allowed recharge limit exceed for this month", exception.Message);
    }

    [Theory, AutoData]
    public async Task ProcessRechargeAsync_ReturnException_WhenInsufficientBalance(RechargeDto rechargeRequest)
    {
        rechargeRequest.Amount = 100;

        //arrange
        _appSettings.Setup(x => x.Value).Returns(GetAppSetting());
        var beneficieryEntity = GetBeneficiarie();
        _beneficiaryRepository.Setup(x => x.GetBeneficiaryByIdAsync(rechargeRequest.BeneficiaryId)).ReturnsAsync(beneficieryEntity);
        _transactionRepository.Setup(x => x.GetBeneficiaryRechargedAmountForCurrentMonth(rechargeRequest.BeneficiaryId)).ReturnsAsync(100);
        _transactionRepository.Setup(x => x.GetUserRechargedAmountForCurrentMonth(beneficieryEntity.UserId)).ReturnsAsync(500);
        _httpService.Setup(x => x.GetAsync<int>(It.IsAny<string>())).ReturnsAsync(0);
       
        var rechargeService = new RechargeTransactionService(_transactionRepository.Object, _mapper.Object, _beneficiaryRepository.Object,
            _appSettings.Object, _httpService.Object);

        //act           
        Exception exception = await Assert.ThrowsAsync<Exception>(() => rechargeService.ProcessRecharge(rechargeRequest));

        //assert
        Assert.Contains("Insufficient balance", exception.Message);
    }

    [Theory, AutoData]
    public async Task ProcessRechargeAsync_ReturnException_WhenPaymentFailed(RechargeDto rechargeRequest)
    {
        rechargeRequest.Amount = 100;

        //arrange
        _appSettings.Setup(x => x.Value).Returns(GetAppSetting());
        var beneficieryEntity = GetBeneficiarie();
        _beneficiaryRepository.Setup(x => x.GetBeneficiaryByIdAsync(rechargeRequest.BeneficiaryId)).ReturnsAsync(beneficieryEntity);
        _transactionRepository.Setup(x => x.GetBeneficiaryRechargedAmountForCurrentMonth(rechargeRequest.BeneficiaryId)).ReturnsAsync(100);
        _transactionRepository.Setup(x => x.GetUserRechargedAmountForCurrentMonth(beneficieryEntity.UserId)).ReturnsAsync(500);
        _httpService.Setup(x => x.GetAsync<int>(It.IsAny<string>())).ReturnsAsync(5000);
        _httpService.Setup(x => x.PostAsync<PaymentDto, bool>(It.IsAny<string>(), It.IsAny<PaymentDto>())).ReturnsAsync(false);
        
        var rechargeService = new RechargeTransactionService(_transactionRepository.Object, _mapper.Object, _beneficiaryRepository.Object,
            _appSettings.Object, _httpService.Object);

        //act           
        Exception exception = await Assert.ThrowsAsync<Exception>(() => rechargeService.ProcessRecharge(rechargeRequest));

        //assert
        Assert.Contains("Payment Failed", exception.Message);
    }

    [Theory, AutoData]
    public async Task ProcessRechargeAsync_ReturnFalse_WhenTransactionFailed(RechargeDto rechargeRequest)
    {
        rechargeRequest.Amount = 100;

        //arrange
        _appSettings.Setup(x => x.Value).Returns(GetAppSetting());
        var beneficieryEntity = GetBeneficiarie();
        _beneficiaryRepository.Setup(x => x.GetBeneficiaryByIdAsync(rechargeRequest.BeneficiaryId)).ReturnsAsync(beneficieryEntity);
        _transactionRepository.Setup(x => x.GetBeneficiaryRechargedAmountForCurrentMonth(rechargeRequest.BeneficiaryId)).ReturnsAsync(100);
        _transactionRepository.Setup(x => x.GetUserRechargedAmountForCurrentMonth(beneficieryEntity.UserId)).ReturnsAsync(500);
        _httpService.Setup(x => x.GetAsync<int>(It.IsAny<string>())).ReturnsAsync(5000);
        _httpService.Setup(x => x.PostAsync<PaymentDto, bool>(It.IsAny<string>(), It.IsAny<PaymentDto>())).ReturnsAsync(true);
        _transactionRepository.Setup(x => x.AddRechargeTransactionAsync(It.IsAny<RechargeTransaction>())).ReturnsAsync(0);


        var rechargeService = new RechargeTransactionService(_transactionRepository.Object, _mapper.Object, _beneficiaryRepository.Object,
            _appSettings.Object, _httpService.Object);

        //act           
        var actual = await rechargeService.ProcessRecharge(rechargeRequest);

        //assert
        Assert.False(actual);
    }

    private AppSettings GetAppSetting()
    {
        return new AppSettings()
        {
            AllowedRechargePlans = new List<int>() { 5, 10, 20, 30, 50, 75, 100 }.ToArray(),
            MaxBeneficiaryPerUser = 5,
            MaxRechargePerMonthForUser = 1500,
            MaxRechargePerMonthPerUnVerifiedUser = 1000,
            MaxRechargePerMonthPerVerifiedUser = 500
        };
    }
    private static Beneficiarie GetBeneficiarie()
    {
        var data = new Faker<Beneficiarie>()
                        .RuleFor(e => e.Id, f => f.Random.Int(1, 100))
                        .RuleFor(e => e.UserId, f => f.Random.Int(1, 100))
                        .RuleFor(e => e.User, GetUser());

        return data.Generate();
    }

    private static User GetUser()
    {
        var data = new Faker<User>()
                        .RuleFor(e => e.Id, f => f.Random.Int(1, 100))
                        .RuleFor(e => e.IsVerified, true);

        return data.Generate();
    }
    #endregion       
}