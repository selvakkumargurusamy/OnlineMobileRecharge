using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Api.Model
{
    public class UserBalance
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int Balance { get; set; }
    }
}