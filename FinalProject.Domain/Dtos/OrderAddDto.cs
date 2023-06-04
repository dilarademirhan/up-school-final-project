using FinalProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.Dtos
{
    public class OrderAddDto
    {
        public Guid Id { get; set; }
        public int RequestedAmount { get; set; }
        public int TotalAmount { get; set; }
        public ProductCrawlType ProductCrawlType { get; set; }
        public ICollection<OrderEvent> OrderEvents { get; set; }
        public ICollection<Product> Products { get; set; }
        public DateTimeOffset CreatedOn { get; set; }

    }
}
