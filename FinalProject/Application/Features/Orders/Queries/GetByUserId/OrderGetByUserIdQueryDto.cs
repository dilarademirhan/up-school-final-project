using Domain.Entities;
using Domain.Enums;

namespace Application.Features.Orders.Queries.GetByUserId
{
    public class OrderGetByUserIdQueryDto
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public int RequestedAmount { get; set; }
        public int TotalFoundAmount { get; set; }
        public ProductCrawlType ProductCrawlType { get; set; }
        public ICollection<OrderEvent> OrderEvents { get; set; }
        public ICollection<Product> Products { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}
