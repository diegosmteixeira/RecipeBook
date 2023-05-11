namespace RecipeBook.Domain.Repositories;

public interface IUnitOfWork
{
    Task Commit();
}
