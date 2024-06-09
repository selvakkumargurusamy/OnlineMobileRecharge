namespace MobileRecharge.Application.Command;

public class AddBeneficiaryCommand : IRequest<int>
{
    public BeneficiaryDto Request;

    public int UserId;

    public AddBeneficiaryCommand(int userId, BeneficiaryDto beneficiaryDto)
    {
        UserId = userId;
        Request = beneficiaryDto;
    }
}
