using Domain.Interfaces;
using DotNetEnv;
using Infra.Data.Context;
using Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Consumer.Test.Common;
public class UnitOfWorkFixture
{
    public readonly ICategoryRepository Categories;
    public readonly IProductRepository Products;

    private static DbContextOptions<ApplicationDbContext> DbContext { get; set; }
    private static ApplicationDbContext ApplicationDbContext {  get; set; }
    static UnitOfWorkFixture()
    {
        var assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var solutionDirectory = Directory.GetParent(assemblyDirectory!)!.Parent!.Parent!.Parent!.Parent!.FullName;
        var envFilePath = Path.Combine(solutionDirectory, ".env");

        Env.Load(envFilePath);
        var connectionString = Env.GetString("DB_CONNECTION_DEV");

        DbContext = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(connectionString).Options;

        ApplicationDbContext = new ApplicationDbContext(DbContext);
    }

    public UnitOfWorkFixture()
    {
        //var context = new ApplicationDbContext(DbContext);
        Categories = new CategoryRepository(ApplicationDbContext);
        Products = new ProductRepository(ApplicationDbContext);
    }

    public async Task SaveChangesAsync()
        => await ApplicationDbContext.SaveChangesAsync();
}
