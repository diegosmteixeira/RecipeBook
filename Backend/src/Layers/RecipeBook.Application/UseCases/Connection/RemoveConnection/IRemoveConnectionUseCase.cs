namespace RecipeBook.Application.UseCases.Connection.RemoveConnection;
public interface IRemoveConnectionUseCase
{
    Task Execute(long userIdToRemove);
}
