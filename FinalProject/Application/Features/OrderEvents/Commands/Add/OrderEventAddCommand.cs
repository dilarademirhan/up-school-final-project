using Domain.Common;
using Domain.Enums;
using MediatR;

namespace Application.Features.OrderEvents.Commands.Add
{
    public class OrderEventAddCommand : IRequest<Response<Guid>>
    {
        public Guid OrderId { get; set; }
        public string Email { get; set; }
        public OrderStatus Status { get; set; }

        public OrderEventAddCommand(Guid orderId, string email, OrderStatus status)
        {
            OrderId = orderId;
            Email = email; 
            Status = status;
        }
    }
}
