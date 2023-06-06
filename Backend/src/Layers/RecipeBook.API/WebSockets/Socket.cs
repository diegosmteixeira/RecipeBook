using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RecipeBook.Application.UseCases.Connection.AcceptConection;
using RecipeBook.Application.UseCases.Connection.GenerateQRCode;
using RecipeBook.Application.UseCases.Connection.ReadQRCode;
using RecipeBook.Application.UseCases.Connection.RefuseConnection;
using RecipeBook.Domain.Entities;
using RecipeBook.Exception;
using RecipeBook.Exception.ExceptionsBase;

namespace RecipeBook.API.WebSockets;

[Authorize(Policy = "UserLogged")]
public class Socket : Hub
{
    private readonly Broadcaster _broadcaster;
    private readonly IGenerateQRCodeUseCase _generateQRCode;
    private readonly IReadQRCodeUseCase _readQRCode;
    private readonly IRefuseConnectionUseCase _refuseConnection;
    private readonly IAcceptConnectionUseCase _acceptConnection;
    private readonly IHubContext<Socket> _hubContext;

    public Socket(IRefuseConnectionUseCase refuseConnection,
                  IAcceptConnectionUseCase acceptConnection,
                  IGenerateQRCodeUseCase generateQRCode,
                  IHubContext<Socket> hubContext,
                  IReadQRCodeUseCase readQRCode)
    {
        _refuseConnection = refuseConnection;
        _acceptConnection = acceptConnection;
        _broadcaster = Broadcaster.Instance;
        _generateQRCode = generateQRCode;
        _hubContext = hubContext;
        _readQRCode = readQRCode;
    }

    public async Task GetQRCode()
    {
        try
        {
            (var qrCode, var idUser) = await _generateQRCode.Execute();

            _broadcaster.InitializeConnection(_hubContext, idUser, Context.ConnectionId);

            await Clients.Caller.SendAsync("QR Code", qrCode);

        }
        catch (RecipeBookException ex)
        {
            await Clients.Caller.SendAsync("Error", ex.Message);
        }
        catch
        {
            await Clients.Caller.SendAsync("Error", ResourceErrorMessages.UNKNOWN_ERROR);
        }
    }

    public async Task ReadQRCode(string connectionCode)
    {
        try
        {
            (var userToConnect, string idUserWhichGenerateQRCode) = await _readQRCode.Execute(connectionCode);

            var connectionId = _broadcaster.GetUserConnectionId(idUserWhichGenerateQRCode);

            _broadcaster.ResetExpirationTime(connectionId);
            _broadcaster.SetConnectionIdUserQRCodeReader(idUserWhichGenerateQRCode, Context.ConnectionId);

            await Clients.Client(connectionId).SendAsync("ResultReadQRCode", userToConnect);

        }
        catch (RecipeBookException ex)
        {
            await Clients.Caller.SendAsync("Error", ex.Message);
        }
        catch
        {
            await Clients.Caller.SendAsync("Error", ResourceErrorMessages.UNKNOWN_ERROR);
        }
    }

    public async Task RefuseConnection()
    {
        try
        {
            var generatorConnectionId = Context.ConnectionId;

            var userId = await _refuseConnection.Execute();

            var connectionIdUserQRCodeReader = _broadcaster.Remove(generatorConnectionId, userId);

            await Clients.Client(connectionIdUserQRCodeReader).SendAsync("OnConnectionRefused");
        }
        catch (RecipeBookException ex)
        {
            await Clients.Caller.SendAsync("Error", ex.Message);
        }
        catch
        {
            await Clients.Caller.SendAsync("Error", ResourceErrorMessages.UNKNOWN_ERROR);
        }
    }

    public async Task AcceptConnection(string connectedWithUserId)
    {
        try
        {
            var userId = await _acceptConnection.Execute(connectedWithUserId);

            var generatorConnectionId = Context.ConnectionId;

            var connectionIdUserQRCodeReader = _broadcaster.Remove(generatorConnectionId, userId);

            await Clients.Client(connectionIdUserQRCodeReader).SendAsync("OnConnectionAccepted");
        }
        catch (RecipeBookException ex)
        {
            await Clients.Caller.SendAsync("Error", ex.Message);
        }
        catch
        {
            await Clients.Caller.SendAsync("Error", ResourceErrorMessages.UNKNOWN_ERROR);
        }
    }
}
