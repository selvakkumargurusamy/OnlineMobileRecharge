namespace MobileRecharge.Domain.Models;

public class RechargeTransaction
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BeneficiaryId { get; set; }
    public DateTime Date { get; set; }
    public int RechargeAmount { get; set; }
    public int ServiceCharge { get; set; }
    public int TotalAmount { get; set; }
    public string? BankTransactionId { get; set; }
    public bool IsSuccess { get; set; }
    public string? ErrorDetails { get; set; }
    public virtual User User { get; set; } = null!;
    public virtual Beneficiarie Beneficiary { get; set; } = null!;
}
