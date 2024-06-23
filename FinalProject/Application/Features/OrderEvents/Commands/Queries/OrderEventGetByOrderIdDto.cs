using Domain.Enums;

namespace Application.Features.OrderEvents.Commands.Queries
{
    public class OrderEventGetByOrderIdDto
    {
        public Guid OrderId { get; set; }
        public OrderStatus Status { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
    }
}
