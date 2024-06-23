using Application.Common.Models.SeleniumLog;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs
{
    public class SeleniumLogHub : Hub
    {
        public async Task SendLogNotificationAsync(SeleniumLogDto log)
        {
            await Clients.AllExcept(Context.ConnectionId).SendAsync("NewSeleniumLogAdded", log);
        }
    }
}
