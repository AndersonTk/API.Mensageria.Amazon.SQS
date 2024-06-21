using Application.Requests;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities;

namespace Application.Profiles;

public class AlunoProfile : Profile
{
    public AlunoProfile()
    {
        CreateMap<AlunoRequest, AlunoContract>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(a => a.Id == Guid.Empty ? Guid.NewGuid() : a.Id))
            .ForMember(dest => dest.CreateUser, opt => opt.MapFrom(a => "Anderson Pinheiro"))
            .ReverseMap();

        CreateMap<Aluno, AlunoContract>().ReverseMap();
    }
}
