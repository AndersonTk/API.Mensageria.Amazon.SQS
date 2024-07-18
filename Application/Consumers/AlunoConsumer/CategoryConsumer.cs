using Application.Consumers.Common;
using Application.Requests;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using MediatR;

namespace Application.Consumers;

public class CategoryConsumer : ConsumerBase<Category, CategoryContract, CategoryOrchestratorCommand, Category>
{
    public CategoryConsumer(IMediator mediator, IMapper mapper) : base(mediator, mapper)
    {
    }
}