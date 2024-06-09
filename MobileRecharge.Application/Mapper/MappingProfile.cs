

namespace MobileRecharge.Application.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<Beneficiarie, BeneficiaryDto>().ReverseMap();
    }
}
