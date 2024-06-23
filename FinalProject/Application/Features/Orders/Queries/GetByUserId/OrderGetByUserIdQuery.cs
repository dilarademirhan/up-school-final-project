using MediatR;

namespace Application.Features.Orders.Queries.GetByUserId
{
    public class OrderGetByUserIdQuery : IRequest<List<OrderGetByUserIdQueryDto>>
    {
        public string? UserId { get; set; }

        public OrderGetByUserIdQuery(string? userId)
        {
            UserId = userId;
        }
    }
}
