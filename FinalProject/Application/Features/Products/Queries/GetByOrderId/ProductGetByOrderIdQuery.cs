using MediatR;

namespace Application.Features.Products.Queries.GetByOrderId
{
    public class ProductGetByOrderIdQuery : IRequest<List<ProductGetByOrderIdDto>>
    {
        public Guid OrderId { get; set; }

        public ProductGetByOrderIdQuery(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
