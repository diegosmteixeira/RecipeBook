using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Application.Services.Cryptography;
using RecipeBook.Application.Services.LoggedUser;
using RecipeBook.Application.Services.Token;
using RecipeBook.Application.UseCases.Recipe.Register;
using RecipeBook.Application.UseCases.User.ChangePassword;
using RecipeBook.Application.UseCases.User.Login;
using RecipeBook.Application.UseCases.User.Register;

namespace RecipeBook.Application;

public static class Bootstraper
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddAdditionalPassword(services, configuration);
        AddHashIds(services, configuration);
        AddToken(services, configuration);
        AddUseCase(services);
        AddUserLogged(services);
    }

    private static void AddUserLogged(IServiceCollection services)
    {
        services.AddScoped<IUserLogged, UserLogged>();
    }

    private static void AddAdditionalPassword(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetRequiredSection("Configurations:Password:AdditionalPassword");

        services.AddScoped(option => new Encryption(section.Value));
    }

    private static void AddToken(this IServiceCollection services, IConfiguration configuration)
    {
        var sectionLifetime = configuration.GetRequiredSection("Configurations:Jwt:TokenLifetimeMinutes");
        var sectionTokenSecret = configuration.GetRequiredSection("Configurations:Jwt:TokenSecret");
        services.AddScoped(option => new TokenConfigurator(int.Parse(sectionLifetime.Value), sectionTokenSecret.Value));
    }

    private static void AddHashIds(IServiceCollection services, IConfiguration configuration)
    {
        var salt = configuration.GetRequiredSection("Configurations:HashIds:Salt");

        services.AddHashids(setup =>
        {
            setup.Salt = salt.Value;
            setup.MinHashLength = 3;
        });
    }

    private static void AddUseCase(IServiceCollection services)
    {
        services.AddScoped<IUserRegisterUseCase, UserRegisterUseCase>()
                .AddScoped<IUserLoginUseCase, UserLoginUseCase>()
                .AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>()
                .AddScoped<IRegisterRecipeUseCase, RegisterRecipeUseCase>();
    }
}
