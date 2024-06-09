namespace MobileRecharge.Domain.Interfaces.Repositories;
public interface IBeneficiaryRepository
{
    Task<IEnumerable<Beneficiarie>> GetAllBeneficiariesAsync(int userId);
    Task<Beneficiarie?> GetBeneficiaryByIdAsync(int id);
    Task<int> GetBeneficiariesCountAsync(int userId);
    Task<int> AddBeneficiaryAsync(int userId, Beneficiarie beneficiary);
    Task<bool> UpdateBeneficiaryAsync(int id, Beneficiarie beneficiary);
    Task<bool> DeleteBeneficiaryAsync(int id);
}
