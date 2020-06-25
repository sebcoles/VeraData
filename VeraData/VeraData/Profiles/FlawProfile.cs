using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VeracodeService.Models;
using VeraData.DataAccess;

namespace VeraData.Profiles
{
    public class FlawProfile : Profile
    {
        public FlawProfile()
        {
            CreateMap<FlawType, Flaw>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.VeracodeId, opt => opt.MapFrom(src => ParseInt(src.issueid)))
            .ForMember(dest => dest.LineNumber, opt => opt.MapFrom(src => ParseInt(src.line)))
            .ForMember(dest => dest.PrototypeFunction, opt => opt.MapFrom(src => src.functionprototype))
            .ForMember(dest => dest.Function, opt => opt.MapFrom(src => src.procedure_name))
            .ForMember(dest => dest.RemediationStatus, opt => opt.MapFrom(src => src.remediation_status))
            .ForMember(dest => dest.SeverityId, opt => opt.MapFrom(src => ParseInt(src.severity)+1))
            .ForMember(dest => dest.Count, opt => opt.MapFrom(src => ParseInt(src.count)))
            .ForMember(dest => dest.VeracodeCategoryId, opt => opt.MapFrom(src => ParseInt(src.categoryid)))
            .ForMember(dest => dest.VeracodeCweId, opt => opt.MapFrom(src => ParseInt(src.cweid)))
            .ForMember(dest => dest.Severity, opt => opt.Ignore())
            .ForMember(dest => dest.Cwe, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore());
        }

        public int ParseInt(string value)
        {
            return Int32.Parse(value);
        }
    }
}
