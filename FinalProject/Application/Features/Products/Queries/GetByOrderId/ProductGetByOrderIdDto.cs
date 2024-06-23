namespace Application.Features.Products.Queries.GetByOrderId
{
    public class ProductGetByOrderIdDto
    {
        public Guid OrderId { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public bool IsOnSale { get; set; }
        public decimal Price { get; set; }
    }
}
