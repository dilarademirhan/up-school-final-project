using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.OrderEvents.Commands.Queries
{
    public class OrderEventGetByOrderIdQueryHandler : IRequestHandler<OrderEventGetByOrderIdQuery,
        List<OrderEventGetByOrderIdDto>>
    {
        IApplicationDbContext _applicationDbContext;

        public OrderEventGetByOrderIdQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<List<OrderEventGetByOrderIdDto>> Handle(OrderEventGetByOrderIdQuery request, CancellationToken cancellationToken)
        {
            
            var orderEvents = await _applicationDbContext.OrderEvents
                .Where(oe => oe.OrderId == request.OrderId)
                .ToListAsync(cancellationToken);

            var orderEventGetByOrderIdDtos = orderEvents.Select(oe => new OrderEventGetByOrderIdDto
            {
                OrderId = oe.OrderId,
                Status = oe.Status,
                CreatedOn = oe.CreatedOn
            }).ToList();

            return orderEventGetByOrderIdDtos;
        }
    }
}
