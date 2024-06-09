namespace MobileRecharge.Domain.Models;

public class Beneficiarie
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(20)]
    public string NickName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;       

    public int UserId { get; set; }

    public bool IsActive { get; set; } = true;

    [InverseProperty("Beneficiaries")]
    public virtual User User { get; set; } = null!;

}