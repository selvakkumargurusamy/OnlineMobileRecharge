namespace MobileRecharge.UnitTests.Service;
public class BeneficiaryServiceTests
{
    private readonly Mock<IBeneficiaryRepository> _beneficiaryRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IOptions<AppSettings>> _appSettings;

    public BeneficiaryServiceTests()
    {
        _beneficiaryRepository = new Mock<IBeneficiaryRepository>();
        _mapper = new Mock<IMapper>();
        _appSettings = new Mock<IOptions<AppSettings>>();
    }

    #region Process Recharge
    [Theory, AutoData]
    public async Task AddBeneficiaryAsync_ReturnSucessResult_WhenValidInput(int userId, BeneficiaryDto beneficiaryDto)
    {
        //arrange
        var beneficieryEntity = GetBeneficiarie();
        _beneficiaryRepository.Setup(x => x.AddBeneficiaryAsync(userId, beneficieryEntity)).ReturnsAsync(1);
        _appSettings.Setup(x => x.Value).Returns(GetAppSetting());
        _mapper.Setup(x => x.Map<Beneficiarie>(beneficiaryDto)).Returns(beneficieryEntity);

        var beneficiaryService = new BeneficiaryService(_beneficiaryRepository.Object, _appSettings.Object, _mapper.Object);

        //act
        var actual = await beneficiaryService.AddBeneficiaryAsync(userId, beneficiaryDto);

        //assert
        Assert.True(actual > 0);
    }

    [Theory, AutoData]
    public async Task AddBeneficiaryAsync_ReturnException_WhenBeneficiaryAddLimitReached(int userId, BeneficiaryDto beneficiaryDto)
    {       

        //arrange
        var beneficieryEntity = GetBeneficiarie();
        _beneficiaryRepository.Setup(x => x.GetBeneficiariesCountAsync(userId)).ReturnsAsync(5);
        _beneficiaryRepository.Setup(x => x.AddBeneficiaryAsync(userId, beneficieryEntity)).ReturnsAsync(1);
        _appSettings.Setup(x => x.Value).Returns(GetAppSetting());
        _mapper.Setup(x => x.Map<Beneficiarie>(beneficiaryDto)).Returns(beneficieryEntity);

        var beneficiaryService = new BeneficiaryService(_beneficiaryRepository.Object, _appSettings.Object, _mapper.Object);

        //act           
        Exception exception = await Assert.ThrowsAsync<Exception>(() => beneficiaryService.AddBeneficiaryAsync(userId, beneficiaryDto));

        //assert
        Assert.Contains("Max beneficiary reached", exception.Message);
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