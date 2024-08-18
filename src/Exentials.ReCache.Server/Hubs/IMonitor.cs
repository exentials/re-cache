namespace Exentials.ReCache.Server.Hubs;

public interface IMonitor
{
    Task ReceiveLog(string message);
}
