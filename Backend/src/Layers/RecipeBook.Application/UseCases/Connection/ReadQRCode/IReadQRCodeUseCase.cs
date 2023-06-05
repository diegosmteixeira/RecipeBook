using RecipeBook.Communication.Response;

namespace RecipeBook.Application.UseCases.Connection.ReadQRCode;
public interface IReadQRCodeUseCase
{
    Task<(ResponseUserConnectionJson userToConnect, string idUserWhichGenerateQRCode)> Execute(string connectionCode);
}
