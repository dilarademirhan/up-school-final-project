using Application.Common.Interfaces;
using Application.Common.Models.Email;
using Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Commands.Update
{
    public class OrderUpdateCommandHandler : IRequestHandler<OrderUpdateCommand, Response<Guid>>
    {
        IApplicationDbContext _applicationDbContext;

        public OrderUpdateCommandHandler(IApplicationDbContext applicationDbContext, IEmailService emailService)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Response<Guid>> Handle(OrderUpdateCommand request, CancellationToken cancellationToken)
        {
            var order = await _applicationDbContext.Orders
                .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

            if (order == null)
            {
                throw new ApplicationException("Order not found");

            }

            order.TotalFoundAmount = request.TotalFoundAmount;
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return new Response<Guid>("Order updated.");
        }
    }
}
