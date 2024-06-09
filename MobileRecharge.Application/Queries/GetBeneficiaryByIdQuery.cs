namespace MobileRecharge.Application.Queries;

public class GetBeneficiaryByIdQuery : IRequest<BeneficiaryDto?>
{
    public int id;
    public GetBeneficiaryByIdQuery(int id)
    {
        this.id = id;
    }
   
}
