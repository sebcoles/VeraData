using AutoMapper;
using VeracodeService;
using VeracodeService.Models;
using VeraData.DataAccess.Models;

namespace VeraData.Profiles
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<AppType, Application>()
            .ForMember(dest => dest.VeracodeId, opt => opt.MapFrom(src => src.app_id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.app_name));
        }
    }
}
