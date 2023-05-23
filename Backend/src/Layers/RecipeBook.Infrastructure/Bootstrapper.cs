using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Domain.Extension;
using RecipeBook.Domain.Repositories;
using RecipeBook.Infrastructure.Repository;
using RecipeBook.Infrastructure.Repository.RepositoryAccess;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RecipeBook.Domain.Repositories.User;

namespace RecipeBook.Infrastructure;

public static class Bootstrapper
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configurationManager)
    {
        AddFluentMigrator(services, configurationManager);

        AddContext(services, configurationManager);
        AddUnityOfWork(services);
        AddRepositories(services);
    }

    private static void AddContext(IServiceCollection services, IConfiguration configurationManager)
    {
        _ = bool.TryParse(configurationManager.GetSection("Configurations:DatabaseInMemory").Value, out bool databaseInMemory);

        if (!databaseInMemory)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));
            var connectionString = configurationManager.GetConnection();

            services.AddDbContext<RecipeBookContext>(dbContextOptions =>
            {
                dbContextOptions.UseMySql(connectionString, serverVersion);
            });
        }

    }

    private static void AddUnityOfWork(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>()
                .AddScoped<IUserReadOnlyRepository, UserRepository>()
                .AddScoped<IUserUpdateOnlyRepository, UserRepository>();
    }

    public static void AddFluentMigrator(IServiceCollection service, IConfiguration configurationManager)
    {
        _ = bool.TryParse(configurationManager.GetSection("Configurations:DatabaseInMemory").Value, out bool databaseInMemory);

        if (!databaseInMemory)
        {
            service.AddFluentMigratorCore().ConfigureRunner(c => c.AddMySql5()
                .WithGlobalConnectionString(configurationManager.GetConnection())
                .ScanIn(Assembly.Load("RecipeBook.Infrastructure"))
                .For.All());
        }
    }
}
