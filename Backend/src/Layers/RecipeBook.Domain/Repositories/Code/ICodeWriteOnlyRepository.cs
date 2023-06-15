namespace RecipeBook.Domain.Repositories.Code;
public interface ICodeWriteOnlyRepository
{
    Task Register(Entities.Code code);
    Task Delete(long userId);
}
