using RecipeBook.Infrastructure.Repository.RepositoryAccess;
using TestsUtilities.Entities;

namespace WebApi.Test;

public class ContextSeedInMemory
{
    public static (RecipeBook.Domain.Entities.User user, string password) Seed(RecipeBookContext context)
    {
        (var user, var password) = UserBuilder.Build();

        context.Users.Add(user);

        context.SaveChanges();

        return (user, password);
    }
}
