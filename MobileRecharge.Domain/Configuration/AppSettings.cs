namespace MobileRecharge.Domain.Configuration;

public class AppSettings
{
    public int MaxBeneficiaryPerUser { get; set; }
    public int MaxRechargePerMonthForUser { get; set; }
    public int MaxRechargePerMonthPerVerifiedUser { get; set; }
    public int MaxRechargePerMonthPerUnVerifiedUser { get; set; }
    public int[] AllowedRechargePlans { get; set; } = Array.Empty<int>();

}
