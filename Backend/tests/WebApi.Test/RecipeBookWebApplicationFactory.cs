using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Infrastructure.Repository.RepositoryAccess;

namespace WebApi.Test;

public class RecipeBookWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var describer = services.SingleOrDefault(d => d.ServiceType == typeof(RecipeBookContext));

                if (describer != null)
                    services.Remove(describer);

                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
                services.AddDbContext<RecipeBookContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });

                var serviceProvider = services.BuildServiceProvider();

                using var scope = serviceProvider.CreateScope();
                var scopeService = scope.ServiceProvider;
                var database = scopeService.GetRequiredService<RecipeBookContext>();

                database.Database.EnsureDeleted();
            });
    }

}
