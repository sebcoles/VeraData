using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VeracodeService.Models;
using VeraData.DataAccess.Models;

namespace VeraData.Profiles
{
    public class PreScanMessageProfile : Profile
    {
        public PreScanMessageProfile()
        {
            CreateMap<FileIssueType, PreScanMessage>()
           .ForMember(dest => dest.Id, opt => opt.Ignore())
           .ForMember(dest => dest.Filename, opt => opt.MapFrom(src => src.filename))
           .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.details));
        }
    }
}
