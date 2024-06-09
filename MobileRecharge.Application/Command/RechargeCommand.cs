namespace MobileRecharge.Application.Command;

public class RechargeCommand : IRequest<bool>
{
    public RechargeDto Request;

    public RechargeCommand(RechargeDto rechargeDto)
    {
        Request = rechargeDto;
    }
}
