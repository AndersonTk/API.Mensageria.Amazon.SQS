using Application.DTOs;
using Application.MediatR.UseCases;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities;

namespace Application.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductOrchestratorCommand, ProductContract>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(a => a.Id == Guid.Empty ? Guid.NewGuid() : a.Id))
            .ReverseMap();

        CreateMap<Product, ProductContract>().ReverseMap();
        CreateMap<Product, ProductDto>()
            .ForMember(a => a.Category, opt => opt.MapFrom(a => a.Category.Name))
            .ReverseMap();
    }
}
