using AutoMapper;
using MyCleanArchitectureApp.Application.DTOs;
using MyCleanArchitectureApp.Core.Entities;

namespace MyCleanArchitectureApp.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<State, StateDto>().ReverseMap();
        }
    }
}
