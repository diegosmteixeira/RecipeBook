using RecipeBook.Communication.Response;

namespace RecipeBook.Application.UseCases.Connection.RecoverConnection;
public interface IRecoverConnectionUseCase
{
    Task<IList<ResponseUserConnectedWithJson>> Execute();
}
