using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VeracodeService.Models;
using VeraData.DataAccess;

namespace VeraData.Mappings.Profiles
{
    public class FlawProfile : Profile
    {
        public FlawProfile()
        {
            CreateMap<FlawType, Flaw>()
            .ForMember(dest => dest.VeracodeId, opt => opt.MapFrom(src => src.issueid))
            .ForMember(dest => dest.LineNumber, opt => opt.MapFrom(src => src.line))
            .ForMember(dest => dest.PrototypeFunction, opt => opt.MapFrom(src => src.functionprototype))
            .ForMember(dest => dest.Function, opt => opt.MapFrom(src => src.procedure_name))
            .ForMember(dest => dest.RemediationStatus, opt => opt.MapFrom(src => src.remediation_status))
            .ForMember(dest => dest.SeverityId, opt => opt.MapFrom(src => src.severity))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.categoryid))
            .ForMember(dest => dest.CweId, opt => opt.MapFrom(src => src.cweid))
            .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.count));
        }
    }
}
