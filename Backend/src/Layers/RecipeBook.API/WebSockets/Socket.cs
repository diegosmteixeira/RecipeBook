﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RecipeBook.Application.UseCases.Connection.GenerateQRCode;
using RecipeBook.Application.UseCases.Connection.ReadQRCode;
using RecipeBook.Communication.Response;
using RecipeBook.Exception;
using RecipeBook.Exception.ExceptionsBase;

namespace RecipeBook.API.WebSockets;

[Authorize(Policy = "UserLogged")]
public class Socket : Hub
{
    private readonly Broadcaster _broadcaster;
    private readonly IGenerateQRCodeUseCase _generateQRCode;
    private readonly IReadQRCodeUseCase _readQRCode;
    private readonly IHubContext<Socket> _hubContext;

    public Socket(IGenerateQRCodeUseCase generateQRCode, 
                  IHubContext<Socket> hubContext, 
                  IReadQRCodeUseCase readQRCode)
    {
        _broadcaster = Broadcaster.Instance;
        _generateQRCode = generateQRCode;
        _hubContext = hubContext;
        _readQRCode = readQRCode;
    }

    public async Task GetQRCode()
    {
        (var qrCode, var idUser ) = await _generateQRCode.Execute();

        _broadcaster.InitializeConnection(_hubContext, idUser, Context.ConnectionId);

        await Clients.Caller.SendAsync("QR Code", qrCode);
    }

    public async Task ReadQRCode(string connectionCode)
    {
        try
        {
            (var userToConnect, string idUserWhichGenerateQRCode) = await _readQRCode.Execute(connectionCode);

            var connectionId = _broadcaster.GetUserConnectionId(idUserWhichGenerateQRCode);

            await Clients.Client("").SendAsync("ResultReadQRCode", userToConnect);

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
