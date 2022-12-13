using Microsoft.AspNetCore.SignalR;

namespace Exentials.ReCache.Server.Hubs
{
	public class MonitorHub : Hub<IMonitor>
	{
		public async Task SendLog(string message)
			=> await Clients.All.ReceiveLog(message);
	}
}
