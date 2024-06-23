using Domain.Enums;

namespace Application.Common.Models.Order
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public int RequestedAmount { get; set; }
        public int TotalFoundAmount { get; set; }
        public ProductCrawlType ProductCrawlType { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}
