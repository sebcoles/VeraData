using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VeracodeService.Models;
using VeraData.DataAccess.Models;

namespace VeraData.Profiles
{
    public class ModuleProfile : Profile
    {
        public ModuleProfile()
        {
            CreateMap<ModuleType, Module>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.VeracodeId, opt => opt.MapFrom(src => src.id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.name))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.status))
            .ForMember(dest => dest.Platform, opt => opt.MapFrom(src => src.platform))
            .ForMember(dest => dest.HasFatalErrors, opt => opt.MapFrom(src => src.has_fatal_errors))
            .ForMember(dest => dest.Size, opt => opt.MapFrom(src => CreateKilobytes(src.size)))
            .ForMember(dest => dest.Hash, opt => opt.MapFrom(src => src.checksum))
            .ForMember(dest => dest.EntryPoint, opt => opt.Ignore());
        }

        public double CreateKilobytes(string size)
        {
            var measure = size.Substring(size.Length - 2, 2).ToLower();
            var numbers = Int32.Parse(size.Substring(0, size.Length - 2));

            if (measure == "mb")
                return numbers * 1024;

            if (measure == "kb")
                return numbers;

            throw new ArgumentException(size);

        }
    }
}
