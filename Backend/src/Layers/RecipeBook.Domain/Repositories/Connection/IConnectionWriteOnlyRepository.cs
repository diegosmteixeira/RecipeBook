namespace RecipeBook.Domain.Repositories.Connection;
public interface IConnectionWriteOnlyRepository
{
    Task Register(Entities.Connection connection);
}
