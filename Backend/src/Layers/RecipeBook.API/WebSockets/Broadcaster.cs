using Microsoft.AspNetCore.SignalR;
using RecipeBook.Exception;
using RecipeBook.Exception.ExceptionsBase;
using System.Collections.Concurrent;

namespace RecipeBook.API.WebSockets;

public class Broadcaster
{
    private readonly static Lazy<Broadcaster> _instance = new Lazy<Broadcaster>(() => new Broadcaster());
    public static Broadcaster Instance { get { return _instance.Value; } }

    private ConcurrentDictionary<string, object> _dictionary { get; set; }

    public Broadcaster()
    {
        _dictionary = new ConcurrentDictionary<string, object>();
    }

    public void InitializeConnection(IHubContext<Socket> hubContext,
                                     string idUserWhichGenerateQRCode, 
                                     string connectionId)
    {
        var connect = new Connection(hubContext, connectionId);

        _dictionary.TryAdd(connectionId, connect);
        _dictionary.TryAdd(idUserWhichGenerateQRCode, connectionId);

        connect.InitializeTimer(CallBackExpiredTime);
    }

    private void CallBackExpiredTime(string connectionId)
    {
        _dictionary.TryRemove(connectionId, out _);
    }

    public string GetUserConnectionId(string userId) 
    {
        if (!_dictionary.TryGetValue(userId, out var connectionId)) 
        {
            throw new RecipeBookException(ResourceErrorMessages.USER_NOT_FOUND);
        }

        return connectionId.ToString();
    }

    public void ResetExpirationTime(string connectionId)
    {
        _dictionary.TryGetValue(connectionId, out var objectConnection);

        var connection = objectConnection as Connection;

        connection.TimerReset();
    }

    public void SetConnectionIdUserQRCodeReader(string idUserWhichGenerateQRCode, string connectionIdUserQRCodeReader)
    {
        var connectionId = GetUserConnectionId(idUserWhichGenerateQRCode);

        _dictionary.TryGetValue(connectionId, out var objectConnection);

        var connection = objectConnection as Connection;

        connection.SetConnectionIdUserQRCodeReader(connectionIdUserQRCodeReader);
    }

    public string Remove(string connectionId, string userId)
    {
        _dictionary.TryGetValue(connectionId, out var objectConnection);

        var connection = objectConnection as Connection;

        connection.StopTimer();

        _dictionary.TryRemove(connectionId, out _);
        _dictionary.TryRemove(userId, out _);

        return connection.UserQRCodeReader();
    }
}
