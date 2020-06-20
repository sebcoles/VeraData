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
            .ForMember(dest => dest.PreScanErrors, opt => opt.MapFrom(src => ConvertErrors(src.file_issue)))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.name))
            .ForMember(dest => dest.Size, opt => opt.MapFrom(src => CreateKilobytes(src.size)))
            .ForMember(dest => dest.Added, opt => opt.MapFrom(src => ConvertFiles(src.sourcefile.@new)))
            .ForMember(dest => dest.Modified, opt => opt.MapFrom(src => ConvertFiles(src.sourcefile.modified)))
            .ForMember(dest => dest.Removed, opt => opt.MapFrom(src => ConvertFiles(src.sourcefile.removed)))
            .ForMember(dest => dest.Hash, opt => opt.MapFrom(src => src.checksum));
        }

        public double CreateKilobytes(string size)
        {
            var measure = size.Substring(size.Length-2,2).ToLower();
            var numbers = Int32.Parse(size.Substring(0, size.Length - 2));
            
            if(measure == "mb")            
                return numbers * 1024;

            if (measure == "kb")
                return numbers;

            throw new ArgumentException(size);
        }

        private PreScanError[] ConvertErrors(FileIssueType[] issues)
        {
            return issues.Select(x => new PreScanError
            {
                Filename = x.filename,
                Error = x.details
            }).ToArray();
        }

        private ModuleFile[] ConvertFiles(FileType[] files)
        {
            return files.Select(x => new ModuleFile
            {
                Path = x.path
            }).ToArray();
        }
    }
}
