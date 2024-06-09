namespace MobileRecharge.Domain.Dtos;

public class BeneficiaryDto
{
    public int Id { get; set; }

    [MaxLength(20)]
    public string NickName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;
}
