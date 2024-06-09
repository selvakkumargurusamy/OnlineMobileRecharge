namespace MobileRecharge.Application.Handler.Query
{
    public class GetUserBeneficiariesQueriesHandler : IRequestHandler<GetUserBeneficiariesQueries, IEnumerable<BeneficiaryDto>>
    {

        private readonly IUserService _userService;
        public GetUserBeneficiariesQueriesHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IEnumerable<BeneficiaryDto>> Handle(GetUserBeneficiariesQueries request, CancellationToken cancellationToken)
        {
            var response = await _userService.GetUserBeneficiariesAsync(request.UserId);
            return response;
        }
    }
}
