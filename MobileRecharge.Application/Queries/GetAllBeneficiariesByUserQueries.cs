namespace MobileRecharge.Application.Queries;

public class GetAllBeneficiariesByUserQueries : IRequest<IEnumerable<BeneficiaryDto>>
{
    public int UserId;
    public GetAllBeneficiariesByUserQueries(int userId)
    {
        this.UserId = userId;
    }
   
}
