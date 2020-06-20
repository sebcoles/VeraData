using AutoMapper;
using VeracodeService.Models;
using VeraData.DataAccess;

namespace VeraData.Profiles
{
    public class ScanProfile : Profile
    {
        public ScanProfile()
        {
            CreateMap<BuildInfoBuildType, Scan>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.VeracodeId, opt => opt.MapFrom(src => src.build_id))
            .ForMember(dest => dest.ScanStart, opt => opt.MapFrom(src => src.launch_date))
            .ForMember(dest => dest.Submitter, opt => opt.MapFrom(src => src.submitter))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.version))
            .ForMember(dest => dest.ScanStatus, opt => opt.MapFrom(src => src.analysis_unit[0].status))
            .ForMember(dest => dest.ScanEnd, opt => opt.MapFrom(src => src.analysis_unit[0].published_date))
            .ForMember(dest => dest.ScanType, opt => opt.MapFrom(src => src.analysis_unit[0].analysis_type));
        }
    }
}
