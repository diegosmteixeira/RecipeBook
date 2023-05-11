using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Domain.Extension;
using System.Reflection;

namespace RecipeBook.Infrastructure
{
    public static class Bootstrapper
    {
        public static void AddRepository(this IServiceCollection service, IConfiguration configuration)
        {
            AddFluentMigrator(service, configuration);
        }

        public static void AddFluentMigrator(IServiceCollection service, IConfiguration configuration)
        {


            service.AddFluentMigratorCore().ConfigureRunner(c => c.AddMySql5()
                .WithGlobalConnectionString(configuration.GetConnection())
                .ScanIn(Assembly.Load("RecipeBook.Infrastructure"))
                .For.All());
        }
    }
}
