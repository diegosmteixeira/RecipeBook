using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RecipeBook.Application.UseCases.Connection.GenerateQRCode;

namespace RecipeBook.API.WebSockets;

[Authorize(Policy = "UserLogged")]
public class Connection : Hub
{
    private readonly IGenerateQRCodeUseCase _generateQRCode;

    public Connection(IGenerateQRCodeUseCase generateQRCode)
    {
        _generateQRCode = generateQRCode;
    }

    public async Task GetQRCode()
    {
        var qrCode = await _generateQRCode.Execute();

        await Clients.Caller.SendAsync("QR Code", qrCode);
    }
}
