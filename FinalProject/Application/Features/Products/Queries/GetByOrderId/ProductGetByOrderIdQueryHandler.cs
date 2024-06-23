using Application.Common.Interfaces;
using Application.Features.Products.Queries.GetByOrderId;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Queries.GetByOrderId
{
    public class ProductGetByOrderIdQueryHandler : IRequestHandler<ProductGetByOrderIdQuery, List<ProductGetByOrderIdDto>>
    {
        IApplicationDbContext _applicationDbContext;

        public ProductGetByOrderIdQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<List<ProductGetByOrderIdDto>> Handle(ProductGetByOrderIdQuery request, CancellationToken cancellationToken)
        {
            var products = await _applicationDbContext.Products
                .Where(p => p.OrderId == request.OrderId)
                .Select(p => new ProductGetByOrderIdDto
                {
                    Name = p.Name,
                    Picture = p.Picture,
                    IsOnSale = p.IsOnSale,
                    Price = p.IsOnSale ? p.SalePrice : p.Price,
                    OrderId = p.OrderId
                })
                .ToListAsync(cancellationToken);

            return products;
        }
    }
}
