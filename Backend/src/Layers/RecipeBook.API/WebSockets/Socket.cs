using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RecipeBook.Application.UseCases.Connection.GenerateQRCode;

namespace RecipeBook.API.WebSockets;

[Authorize(Policy = "UserLogged")]
public class Socket : Hub
{
    private readonly Broadcaster _broadcaster;
    private readonly IGenerateQRCodeUseCase _generateQRCode;
    private readonly IHubContext<Socket> _hubContext;

    public Socket(IGenerateQRCodeUseCase generateQRCode, IHubContext<Socket> hubContext)
    {
        _broadcaster = Broadcaster.Instance;
        _generateQRCode = generateQRCode;
        _hubContext = hubContext;
    }

    public async Task GetQRCode()
    {
        var qrCode = await _generateQRCode.Execute();

        _broadcaster.InitializeConnection(_hubContext, Context.ConnectionId);

        await Clients.Caller.SendAsync("QR Code", qrCode);
    }
}
