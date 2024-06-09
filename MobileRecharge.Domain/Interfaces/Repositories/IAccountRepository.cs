using MobileRecharge.Domain.Models;

namespace MobileRecharge.Domain.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        public Task<UserBalance> GetAccount(int id);

        public Task<UserBalance> GetAccountWithBeneficiaries(int id);

        public Task<bool> UpdateAccount(int id, UserBalance account);

        public Task<bool> DeleteAccount(int id);

    }
}
