using RecipeBook.Infrastructure.Repository.RepositoryAccess;
using TestsUtilities.Entities;

namespace WebApi.Test;

public class ContextSeedInMemory
{
    public static (RecipeBook.Domain.Entities.User user, string password) Seed(RecipeBookContext context)
    {
        (var user, var password) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);

        context.Users.Add(user);
        context.Recipes.Add(recipe);

        context.SaveChanges();

        return (user, password);
    }

    public static (RecipeBook.Domain.Entities.User user, string password) SeedUserWithoutAnyRecipe(RecipeBookContext context)
    {
        (var user, var password) = UserBuilder.BuildUser2();

        context.Users.Add(user);

        context.SaveChanges();

        return (user, password);
    }
}