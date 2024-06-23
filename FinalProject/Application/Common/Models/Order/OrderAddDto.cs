using Domain.Enums;

namespace Application.Common.Models.Order
{
    public class OrderAddDto
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public string Email { get; set; }
        public int RequestedAmount { get; set; }
        public int TotalFoundAmount { get; set; }
        public ProductCrawlType ProductCrawlType { get; set; }
    }
}
