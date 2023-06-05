namespace RecipeBook.Domain.Repositories.Code;
public interface ICodeReadOnlyRepository
{
    Task<Domain.Entities.Code> RecoverEntityCode(string code);
}
