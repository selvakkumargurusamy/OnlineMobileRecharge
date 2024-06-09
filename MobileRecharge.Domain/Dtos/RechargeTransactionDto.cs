namespace MobileRecharge.Domain.Dtos;

public class RechargeTransactionDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BeneficiaryId { get; set; }
    public DateTime Date { get; set; }
    public int TopUpAmount { get; set; }
    public int ServiceCharge { get; set; }
    public int TotalAmount { get; set; }
    public string? BankTransactionId { get; set; }
    public bool IsSuccess { get; set; }
    public string? ErrorDetails { get; set; }
    public virtual UserDto User { get; set; } = null!;
    public virtual BeneficiaryDto Beneficiary { get; set; } = null!;
}
