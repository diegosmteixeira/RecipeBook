using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Application.Services.Cryptography;
using RecipeBook.Application.Services.Token;
using RecipeBook.Application.UseCases.User.Register;

namespace RecipeBook.Application.Services;

public static class Bootstraper
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddAdditionalPassword(services, configuration);
        AddToken(services, configuration);


        services.AddScoped<IUserRegisterUseCase, UserRegisterUseCase>();
    }

    private static void AddAdditionalPassword(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetRequiredSection("Config:AdditionalPassword");

        services.AddScoped(option => new Encryption(section.Value));
    }

    private static void AddToken(this IServiceCollection services, IConfiguration configuration)
    {
        var sectionLifetime = configuration.GetRequiredSection("Config:TokenLifetime");
        var sectionTokenSecret = configuration.GetRequiredSection("Config:TokenSecret");
        services.AddScoped(option => new TokenConfigurator(int.Parse(sectionLifetime.Value), sectionTokenSecret.Value));
    }


}
