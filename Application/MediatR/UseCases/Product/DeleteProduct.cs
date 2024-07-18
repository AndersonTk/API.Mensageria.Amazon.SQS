using Domain.Interfaces.Base;
using MediatR;

namespace Application.MediatR.UseCases;
public class DeleteProduct : IRequest<bool>
{
    public Guid Id { get; set; }
}

public class DeleteProductHandler : IRequestHandler<DeleteProduct, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<bool> Handle(DeleteProduct request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var product = await _unitOfWork.Product.GetByIdAsync(request.Id);
            var category = await _unitOfWork.Category.GetByIdAsync(product.CategoryId);
            var category2 = await _unitOfWork.Category.GetByIdAsync(Guid.Parse("1c7ca513-6da9-4427-8a29-0008788e1a40"));
            await _unitOfWork.Category.DeleteAsync(category2);
            await _unitOfWork.Product.DeleteAsync(product);
            await _unitOfWork.Category.DeleteAsync(category);
            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitTransactionAsync();
            return true;
        }
        catch (Exception)
        {

            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
