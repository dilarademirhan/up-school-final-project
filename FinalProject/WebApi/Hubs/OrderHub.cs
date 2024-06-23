using Application.Common.Models.Order;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs
{
    public class OrderHub : Hub
    {
        public async Task AddOrder(OrderAddDto orderAddDto)
        {
            await Clients.AllExcept(Context.ConnectionId).SendAsync("NewOrderAdded", orderAddDto);
        }
    }
}
