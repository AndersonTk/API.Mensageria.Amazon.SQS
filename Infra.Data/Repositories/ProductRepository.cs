using Domain.Entities;
using Domain.Interfaces;
using Infra.Data.Context;
using Infra.Data.Repositories.Common;

namespace Infra.Data.Repositories;
public class ProductRepository : Repository<Product>, IProductCategory
{
    public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
    { }
}
