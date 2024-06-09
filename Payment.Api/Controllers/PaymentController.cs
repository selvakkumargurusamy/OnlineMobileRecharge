using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payment.Api.Database;
using Payment.Api.Dto;
using Payment.Api.Model;

namespace Payment.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly DbSet<UserBalance> _userBalances;
        private readonly AppDbContext _dbContext;

        public PaymentController(ILogger<PaymentController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _userBalances = dbContext.UserBalances;
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("balance/{userId}")]
        public async Task<int> GetBalance(int userId)
        {
            return (await _userBalances.FindAsync(userId))?.Balance ?? 0;
        }

        [HttpPost]
        [Route("recharge")]
        public async Task<bool> Recharge([FromBody] PaymentDto paymentDto)
        {
            var userBalance = await _userBalances.FindAsync(paymentDto.UserId);

            if (userBalance == null)
            {
                return false;
            }
            userBalance.Balance -= paymentDto.Amount;

            _dbContext.SaveChanges();

            return true;
        }
    }
}