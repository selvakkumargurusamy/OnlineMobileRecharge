using MobileRecharge.Domain.Models;

namespace MobileRecharge.Application.Services;

public class BeneficiaryService : IBeneficiaryService
{
    private readonly IBeneficiaryRepository _beneficiaryRepo;
    private readonly AppSettings _appSettings;
    private readonly IMapper _mapper;

    public BeneficiaryService(IBeneficiaryRepository repository, IOptions<AppSettings> appSettings, IMapper mapper)
    {
        _beneficiaryRepo = repository;
        _appSettings = appSettings.Value;
        _mapper = mapper;

    }

    public async Task<IEnumerable<BeneficiaryDto>> GetAllBeneficiariesAsync(int userId)
    {
        var list = await _beneficiaryRepo.GetAllBeneficiariesAsync(userId);
        return _mapper.Map<IEnumerable<BeneficiaryDto>>(list);
    }

    public async Task<BeneficiaryDto?> GetBeneficiaryByIdAsync(int id)
    {
        var entity = await _beneficiaryRepo.GetBeneficiaryByIdAsync(id);
        return _mapper.Map<BeneficiaryDto>(entity);
    }

    public async Task<int> AddBeneficiaryAsync(int userId, BeneficiaryDto beneficiaryDto)
    {
        var count = await _beneficiaryRepo.GetBeneficiariesCountAsync(userId);

        if (count >= _appSettings.MaxBeneficiaryPerUser)
            throw new Exception("Max beneficiary reached");

        var beneficiary = _mapper.Map<Beneficiarie>(beneficiaryDto);
        beneficiary.UserId = userId;
        return await _beneficiaryRepo.AddBeneficiaryAsync(userId, beneficiary);
    }

    public async Task<bool> UpdateBeneficiaryAsync(int id, BeneficiaryDto beneficiaryDto)
    {
        var beneficiary = _mapper.Map<Beneficiarie>(beneficiaryDto);
        return await _beneficiaryRepo.UpdateBeneficiaryAsync(id, beneficiary);
    }

    public async Task<bool> DeleteBeneficiaryAsync(int id)
    {
        return await _beneficiaryRepo.DeleteBeneficiaryAsync(id);
    }
}
