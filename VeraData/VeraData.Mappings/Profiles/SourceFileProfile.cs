using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VeracodeService.Models;
using VeraData.DataAccess.Models;

namespace VeraData.Mappings.Profiles
{
    public class SourceFileProfile : Profile
    {
        public SourceFileProfile()
        {
            CreateMap<FlawType, SourceFile>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.sourcefile))
            .ForMember(dest => dest.Path, opt => opt.MapFrom(src => src.sourcefilepath));
        }
    }
}
