using AutoMapper;
using MRCase.Application.Data.Dtos;
using MRCase.Application.Pagination;
using MRCase.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRCase.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ImportDataTRDto, Datum>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Date, opt => opt.MapFrom(im => im.dc_Zaman))
                .ForMember(d => d.Event, opt => opt.MapFrom(im => im.dc_Olay))
                .ForMember(d => d.Category, opt => opt.MapFrom(im => im.dc_Kategori));

            CreateMap<ImportDataITDto, Datum>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Date, opt => opt.MapFrom(im => im.dc_Orario))
                .ForMember(d => d.Event, opt => opt.MapFrom(im => im.dc_Evento))
                .ForMember(d => d.Category, opt => opt.MapFrom(im => im.dc_Categoria));

            CreateMap<Datum, DatumResponseDto>();
        }
    }
}
