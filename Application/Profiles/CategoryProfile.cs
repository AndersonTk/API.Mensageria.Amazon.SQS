using Application.DTOs;
using Application.MediatR.UseCases;
using Application.Requests;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities;

namespace Application.Profiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CategoryOrchestratorCommand, CategoryContract>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(a => a.Id == Guid.Empty ? Guid.NewGuid() : a.Id))
            .ForMember(dest => dest.CreateUser, opt => opt.MapFrom(a => a.CreateUser))
            .ReverseMap();        
        
        CreateMap<DeleteCategoryCommand, CategoryDeleteContract>()
            .ReverseMap();

        CreateMap<Category, CategoryContract>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(a => a.Id))
            .ReverseMap();
        
        CreateMap<Category, CategoryDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(a => a.Id))
            .ReverseMap();

        CreateMap<Category, CategoryDeleteContract>()
            .ReverseMap();
    }
}