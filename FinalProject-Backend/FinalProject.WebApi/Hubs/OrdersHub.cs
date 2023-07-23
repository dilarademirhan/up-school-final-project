using FinalProject.Domain.Dtos;
using Microsoft.AspNetCore.SignalR;

namespace FinalProject.WebApi.Hubs
{
    public class OrdersHub : Hub
    {
        public async Task SendLogNotificationAsync(SeleniumLogDto log)
        {
            await Clients.AllExcept(Context.ConnectionId).SendAsync("NewSeleniumLogAdded", log);

        }
    }
}
