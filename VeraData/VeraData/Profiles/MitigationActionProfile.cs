using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using VeracodeService.Models;
using VeraData.DataAccess.Models;

namespace VeraData.Profiles
{
    public class MitigationActionProfile : Profile
    {
        public MitigationActionProfile()
        {
            CreateMap<MitigationActionType, MitigationAction>()
           .ForMember(dest => dest.Id, opt => opt.Ignore())
           .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.desc))
           .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.comment))
           .ForMember(dest => dest.Date, opt => opt.MapFrom(src => GetDate(src.date)))
           .ForMember(dest => dest.Reviewer, opt => opt.MapFrom(src => src.reviewer));
        }

        public DateTime GetDate(string date)
        {
            //2020-03-23T10:49:30-04:00
            var substring = date.Substring(0, 19);
            var pattern = "yyyy-MM-dd:hh:mm:ss";
            DateTime parsedDate;
            DateTime.TryParseExact(substring, pattern, null, DateTimeStyles.None, out parsedDate);
            return parsedDate;
        }
    }
}
