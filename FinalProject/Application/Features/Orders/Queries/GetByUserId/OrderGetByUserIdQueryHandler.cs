using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Queries.GetByUserId
{
    public class OrderGetByUserIdQueryHandler : IRequestHandler<OrderGetByUserIdQuery, List<OrderGetByUserIdQueryDto>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public OrderGetByUserIdQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<List<OrderGetByUserIdQueryDto>> Handle(OrderGetByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _applicationDbContext.Orders
                .Where(x => x.CreatedByUserId == request.UserId)
                .Select(order => new OrderGetByUserIdQueryDto
                {
                    Id = order.Id,
                    UserId = order.CreatedByUserId,
                    RequestedAmount = order.RequestedAmount,
                    TotalFoundAmount = order.TotalFoundAmount,
                    ProductCrawlType = order.ProductCrawlType,
                    OrderEvents = order.OrderEvents,
                    Products = order.Products,
                    CreatedOn = order.CreatedOn
                }).ToListAsync(cancellationToken);

            return orders;
        }
    }
}
