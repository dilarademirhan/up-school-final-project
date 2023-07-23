using FinalProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.Dtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public int RequestedAmount { get; set; }
        public int TotalAmount { get; set; }
        public ProductCrawlType ProductCrawlType { get; set; }
        public ICollection<OrderEvent> OrderEvents { get; set; }
        public ICollection<Product> Products { get; set; }
        public DateTimeOffset CreatedOn { get; set; }

        public static OrderDto MapFromOrder(Order order)
        {
            return new OrderDto()
            {
                Id = order.Id,
                // Title = order.Title,
                // UserName = order.UserName,
                // Password = order.Password,
                // Url = order.Url,
                // ShowPassword = false,
                CreatedOn = order.CreatedOn,
                // IsFavourite = order.IsFavourite
            };
        }
    }
}
