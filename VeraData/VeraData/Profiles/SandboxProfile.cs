using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VeracodeService.Models;
using VeraData.DataAccess.Models;

namespace VeraData.Profiles
{
    public class SandboxProfile : Profile
    {
        public SandboxProfile()
        {
            CreateMap<SandboxType, Sandbox>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.VeracodeId, opt => opt.MapFrom(src => src.sandbox_id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.sandbox_name));
        }
    }
}
