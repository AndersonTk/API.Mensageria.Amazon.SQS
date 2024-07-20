using Application.Consumers.Common;
using Application.MediatR.UseCases;
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

public class CategoryDeleteConsumer : DeleteConsumerBase<Category, CategoryDeleteContract, DeleteCategoryCommand, bool>
{
    public CategoryDeleteConsumer(IMediator mediator, IMapper mapper) : base(mediator, mapper)
    {
    }
}