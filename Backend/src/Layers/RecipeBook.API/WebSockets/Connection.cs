using Microsoft.AspNetCore.SignalR;
using System.Timers;

namespace RecipeBook.API.WebSockets;

public class Connection
{
    private readonly IHubContext<Socket> _hubContext;
    private readonly string UserQRCodeOwnerConnectionId;
    private string ConnectionIdUserQRCodeReader;
    private Action<string> _callBackExpiredTime;
    private System.Timers.Timer _timer { get; set; }
    private short timeLeftSeconds { get; set; }

    public Connection(IHubContext<Socket> hubContext, string userQRCodeOwnerConnectionId)
    {
        _hubContext = hubContext;
        UserQRCodeOwnerConnectionId = userQRCodeOwnerConnectionId;
    }

    public void InitializeTimer(Action<string> callBackExpiredTime)
    {
        _callBackExpiredTime = callBackExpiredTime;

        StartTimer();
    }

    public void TimerReset()
    {
        StopTimer();
        StartTimer();
    }

    public void StopTimer()
    {
        _timer?.Stop();
        _timer?.Dispose();
        _timer = null;
    }

    public void SetConnectionIdUserQRCodeReader(string connectionId)
    {
        ConnectionIdUserQRCodeReader = connectionId;
    }

    public string UserQRCodeReader()
    {
        return ConnectionIdUserQRCodeReader;
    }

    public void StartTimer()
    {
        timeLeftSeconds = 60;
        _timer = new System.Timers.Timer(1000)
        {
            Enabled = false
        };
        _timer.Elapsed += ElapsedTimer;
        _timer.Enabled = true;
    }

    private async void ElapsedTimer(object sender, ElapsedEventArgs e)
    {
        if (timeLeftSeconds >= 0)
            await _hubContext.Clients.Client(UserQRCodeOwnerConnectionId).SendAsync("SetTimeLeft", timeLeftSeconds--);
        else
        {
            StopTimer();
            _callBackExpiredTime(UserQRCodeOwnerConnectionId);
        }
    }
}