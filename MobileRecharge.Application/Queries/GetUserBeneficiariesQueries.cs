namespace MobileRecharge.Application.Queries;

public class GetUserBeneficiariesQueries : IRequest<IEnumerable<BeneficiaryDto>>
{
    public int UserId;
    public GetUserBeneficiariesQueries(int userId)
    {
        this.UserId = userId;
    }
   
}
