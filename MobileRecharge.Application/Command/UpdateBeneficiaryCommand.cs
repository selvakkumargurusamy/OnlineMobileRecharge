namespace MobileRecharge.Application.Command;

public class UpdateBeneficiaryCommand : IRequest<bool>
{
    public BeneficiaryDto Request;

    public int Id;

    public UpdateBeneficiaryCommand(int Id, BeneficiaryDto beneficiaryDto)
    {
        this.Id = Id;
        Request = beneficiaryDto;
    }
}
