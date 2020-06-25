using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VeracodeService.Models;
using VeraData.DataAccess.Models;

namespace VeraData.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<FlawType, Category>()
           .ForMember(dest => dest.Id, opt => opt.Ignore())
           .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.categoryname))
           .ForMember(dest => dest.VeracodeId, opt => opt.MapFrom(src => src.categoryid));
        }
    }
}
