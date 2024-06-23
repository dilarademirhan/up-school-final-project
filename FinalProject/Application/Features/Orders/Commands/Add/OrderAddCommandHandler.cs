using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.Features.Orders.Commands.Add
{
    public class OrderAddCommandHandler : IRequestHandler<OrderAddCommand, Response<Guid>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public OrderAddCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Response<Guid>> Handle(OrderAddCommand request, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                Id = request.Id,
                RequestedAmount = request.RequestedAmount,
                ProductCrawlType = request.ProductCrawlType,
                TotalFoundAmount = request.TotalFoundAmount,
                CreatedOn = DateTimeOffset.UtcNow,
                CreatedByUserId = request.UserId,
                IsDeleted = false
            };

            await _applicationDbContext.Orders.AddAsync(order, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return new Response<Guid>("Order added");
        }



    }
}
