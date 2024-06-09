namespace MobileRecharge.UnitTests.Controller
{
    public class BeneficiaryControllerTests
    {
        private readonly Mock<IMediator> mockMediator;

        public BeneficiaryControllerTests()
        {
            mockMediator = new Mock<IMediator>();
        }

        #region Get All
        [Theory, AutoData]
        public void GetAll_ReturnSuccessResult_WhenDataExists(int userId)
        {
            //arrange
            var response = GetAllBeneficiary();
            mockMediator.Setup(x => x.Send(It.IsAny<GetAllBeneficiariesByUserQueries>(), default)).ReturnsAsync(response);
            var controller = new BeneficiaryController(mockMediator.Object);

            //act
            var actionResult = controller.GetAll(userId);
            var result = actionResult.Result as OkObjectResult;
            var actual = result?.Value as List<BeneficiaryDto>;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(actual);
            Assert.True(actual.Count > 0);
        }

        [Fact]
        public void GetAll_ReturnNotFoundResult_WhenDataNotExists()
        {
            //arrange
            mockMediator.Setup(x => x.Send(It.IsAny<GetAllBeneficiariesByUserQueries>(), default)).ReturnsAsync(new List<BeneficiaryDto>());
            var controller = new BeneficiaryController(mockMediator.Object);

            //act
            var actionResult = controller.GetAll(0);
            var result = actionResult.Result as NotFoundResult;


            //assert        
            Assert.IsType<NotFoundResult>(result);
        }

        private static IEnumerable<BeneficiaryDto> GetAllBeneficiary()
        {
            var data = new Faker<BeneficiaryDto>()
                            .RuleFor(e => e.Id, f => f.Random.Int(1, 100));

            return data.Generate(2);
        }
        #endregion

        #region Create
        [Fact]
        public void Create_ReturnSucessResult_WhenValidInput()
        {
            //arrange
            mockMediator.Setup(x => x.Send(It.IsAny<AddBeneficiaryCommand>(), default)).ReturnsAsync(1);
            var controller = new BeneficiaryController(mockMediator.Object);

            //act
            var request = GetRequest();
            var actionResult = controller.Create(1, request);
            var result = actionResult.Result as OkObjectResult;
            var actual = result?.Value as object;

            //assert        
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(1, actual);
        }

        [Fact]
        public void Create_ReturnNotFoundResult_WhenInValidInput()
        {
            //arrange
            mockMediator.Setup(x => x.Send(It.IsAny<AddBeneficiaryCommand>(), default)).ReturnsAsync(0);
            var controller = new BeneficiaryController(mockMediator.Object);

            //act

            var request = GetRequest();
            var actionResult = controller.Create(1, request);
            var result = actionResult.Result as NotFoundResult;

            //assert        
            Assert.IsType<NotFoundResult>(result);
        }

        private static BeneficiaryDto GetRequest()
        {
            var data = new Faker<BeneficiaryDto>()
                            .RuleFor(e => e.Id, f => f.Random.Int(1, 100));

            return data.Generate();
        }
        #endregion
    }
}