using Domain.Enums;
using MediatR;

namespace Application.Features.OrderEvents.Commands.Queries
{
    public class OrderEventGetByOrderIdQuery : IRequest<List<OrderEventGetByOrderIdDto>>
    {
        public Guid OrderId { get; set; }
    }
}
