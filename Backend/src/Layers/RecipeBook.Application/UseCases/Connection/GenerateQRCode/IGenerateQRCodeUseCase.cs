namespace RecipeBook.Application.UseCases.Connection.GenerateQRCode;
public interface IGenerateQRCodeUseCase
{
    Task<string> Execute();
}
