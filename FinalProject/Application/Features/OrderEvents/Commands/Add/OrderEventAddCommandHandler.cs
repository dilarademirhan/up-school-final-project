using Application.Common.Interfaces;
using Application.Common.Models.Email;
using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.Features.OrderEvents.Commands.Add
{
    public class OrderEventAddCommandHandler : IRequestHandler<OrderEventAddCommand, Response<Guid>>
    {
        IApplicationDbContext _applicationDbContext;
        private readonly IEmailService _emailService;

        public OrderEventAddCommandHandler(IApplicationDbContext applicationDbContext, IEmailService emailService)
        {
            _applicationDbContext = applicationDbContext;
            _emailService = emailService;
        }

        public async Task<Response<Guid>> Handle(OrderEventAddCommand request, CancellationToken cancellationToken)
        {
            var orderEvent = new OrderEvent()
            {
                Id = Guid.NewGuid(),
                OrderId = request.OrderId,
                Status = request.Status,
                CreatedOn = DateTime.Now,
            };

            await _applicationDbContext.OrderEvents.AddAsync(orderEvent);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            _emailService.SendEmailInformation(new SendEmailInformationDto()
            {
                Email = request.Email,
                OrderStatus = request.Status
            });
            return new Response<Guid>("OrderEvent added");

        }
    }
}
