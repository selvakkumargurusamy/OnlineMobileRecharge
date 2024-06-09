namespace MobileRecharge.Application.Command;

public class DeleteBeneficiaryCommand : IRequest<bool>
{
    public int Id;

    public DeleteBeneficiaryCommand(int Id)
    {
        this.Id = Id;
    }
}
