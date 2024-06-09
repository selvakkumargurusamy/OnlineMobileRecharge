namespace MobileRecharge.Application.Interfaces;

public interface IBeneficiaryService
{
    Task<IEnumerable<BeneficiaryDto>> GetAllBeneficiariesAsync(int userId);
    Task<BeneficiaryDto?> GetBeneficiaryByIdAsync(int id);
    Task<int> AddBeneficiaryAsync(int userId, BeneficiaryDto beneficiary);
    Task<bool> UpdateBeneficiaryAsync(int id, BeneficiaryDto beneficiary);
    Task<bool> DeleteBeneficiaryAsync(int id);
}
