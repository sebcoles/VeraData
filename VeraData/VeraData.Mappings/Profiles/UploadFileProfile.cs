using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VeracodeService.Models;
using VeraData.DataAccess.Models;

namespace VeraData.Mappings.Profiles
{
    public class UploadFileProfile : Profile
    {
        public UploadFileProfile()
        {
            CreateMap<FileListFileType, UploadFile>()
            .ForMember(dest => dest.VeracodeId, opt => opt.MapFrom(src => src.file_id))
            .ForMember(dest => dest.Hash, opt => opt.MapFrom(src => src.file_md5))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.file_name));
        }
    }
}
