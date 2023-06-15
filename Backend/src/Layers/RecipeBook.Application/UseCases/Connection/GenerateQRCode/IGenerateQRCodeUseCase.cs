namespace RecipeBook.Application.UseCases.Connection.GenerateQRCode;
public interface IGenerateQRCodeUseCase
{
    Task<(byte[] qrCode, string idUser)> Execute();
}