using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VeracodeService.Models;
using VeraData.DataAccess.Models;

namespace VeraData.Profiles
{
    public class CweProfile : Profile
    {
        public CweProfile()
        {
            CreateMap<FlawType, Cwe>()
           .ForMember(dest => dest.Id, opt => opt.Ignore())
           .ForMember(dest => dest.RemediationEffort, opt => opt.MapFrom(src => Int32.Parse(src.remediationeffort)))
           .ForMember(dest => dest.Exploitability, opt => opt.MapFrom(src => Int32.Parse(src.exploitLevel)))
           .ForMember(dest => dest.Pci, opt => opt.MapFrom(src => src.pcirelated))
           .ForMember(dest => dest.PolicyImpact, opt => opt.MapFrom(src => src.affects_policy_compliance))
           .ForMember(dest => dest.VeracodeId, opt => opt.MapFrom(src => src.cweid));
        }
    }
}
