namespace MobileRecharge.Domain.Models;

public class UserBalance
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int UserId { get; set; }

    public int Balance { get; set; }

    // Navigational Properties
    //public virtual int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
