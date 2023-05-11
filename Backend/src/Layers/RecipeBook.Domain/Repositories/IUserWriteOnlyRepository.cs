using RecipeBook.Domain.Entities;

namespace RecipeBook.Domain.Repositories;

public interface IUserWriteOnlyRepository
{
    Task Add(User user);
}
