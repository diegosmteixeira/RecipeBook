using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace RecipeBook.API.WebSockets;

public class Broadcaster
{
    private readonly static Lazy<Broadcaster> _instance = new Lazy<Broadcaster>(() => new Broadcaster());
    public static Broadcaster Instance { get { return _instance.Value; } }

    private ConcurrentDictionary<string, Connection> _dictionary { get; set; }

    public Broadcaster()
    {
        _dictionary = new ConcurrentDictionary<string, Connection>();
    }

    public void InitializeConnection(IHubContext<Socket> hubContext, string connectionId)
    {
        var connect = new Connection(hubContext, connectionId);

        _dictionary.TryAdd(connectionId, connect);

        connect.InitializeTimer(CallBackExpiredTime);
    }

    private void CallBackExpiredTime(string connectionId)
    {
        _dictionary.TryRemove(connectionId, out _);
    }
}
