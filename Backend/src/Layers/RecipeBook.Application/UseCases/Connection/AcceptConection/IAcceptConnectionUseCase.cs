namespace RecipeBook.Application.UseCases.Connection.AcceptConection;
public interface IAcceptConnectionUseCase
{
    Task<string> Execute(string connectedWithUserId);
}
