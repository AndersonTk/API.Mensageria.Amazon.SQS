using Domain.Contracts;
using FluentValidation;
using RS = Resources.Common;

namespace Application.MediatR.UseCases;

public class ProductValidation : AbstractValidator<ProductContract>
{
    public ProductValidation()
    {
        RuleFor(a => a.Id).NotEmpty();
        RuleFor(a => a.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage(RS.DATA_ANOTATION_REQUIRED.Replace("{0}", RS.PRODUCT_LBL_NAME))
            .Length(1, 255).WithMessage($"{RS.PRODUCT_LBL_NAME} tem que ter entre 1 a 255 caractêres");
    }
}
