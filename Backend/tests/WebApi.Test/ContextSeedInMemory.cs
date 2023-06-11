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

    public static (RecipeBook.Domain.Entities.User user, string password) SeedUserWithConnection(RecipeBookContext context)
    {
        (var user, var password) = UserBuilder.BuildUserWithConnection();

        context.Users.Add(user);

        var userConnectionsList = ConnectionBuilder.Build();

        for(var index = 1; index <= userConnectionsList.Count; index++)
        {
            var connectionWithUser = userConnectionsList[index - 1];

            context.Connections.Add(new RecipeBook.Domain.Entities.Connection()
            {
                Id = index,
                UserId = user.Id,  // only do an where condition
                ConnectedWithUser = connectionWithUser // need to pass an user, because a JOIN will be performed
            });
        }

        context.SaveChanges();

        return (user, password);


    }
}