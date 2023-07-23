using AutoMapper;
using FinalProject.Domain.Entities;
//using FinalProject.Domain.Data;
using FinalProject.Domain.Dtos;
using FinalProject.Domain.Utilities;
using FinalProject.Persistence.EntityFramework.Contexts;
using FinalProject.WebApi.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Principal;

namespace FinalProject.WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly FinalProjectDbContext _dbContext;
        private readonly IHubContext<OrdersHub> _ordersHubContext;

        public OrdersController(IMapper mapper, FinalProjectDbContext dbContext, IHubContext<OrdersHub> ordersHubContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _ordersHubContext = ordersHubContext;
        }


        [HttpGet]
        public IActionResult GetAll(bool isAscending, ProductCrawlType productCrawlType)
        {
            IQueryable<Order> ordersQuery = _dbContext.Orders.AsQueryable();
                ordersQuery = ordersQuery.Where(x =>
                    x.ProductCrawlType == productCrawlType);

            var orders = ordersQuery.ToList();
            var orderDtos = orders.Select(order => OrderDto.MapFromOrder(order)).ToList();

            return Ok(orderDtos);
        }


        [HttpPost]
        public async Task<IActionResult> AddAsync(OrderAddDto orderAddDto, CancellationToken cancellationToken)
        {

            var order = new Order()
            {
                Id = Guid.NewGuid(),
                RequestedAmount = orderAddDto.RequestedAmount,
                TotalAmount = orderAddDto.TotalAmount,
                ProductCrawlType = orderAddDto.ProductCrawlType,
                CreatedOn = DateTimeOffset.Now,
            };

            await _dbContext.Orders.AddAsync(order, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var orderDto = OrderDto.MapFromOrder(order);

            await _ordersHubContext.Clients.All.SendAsync(SignalRMethodKeys.Orders.Added, orderDto, cancellationToken);
 
            return Ok(orderDto);
        }


        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var order = _dbContext.Orders.FirstOrDefault(x => x.Id == id);

            if (order is null) return NotFound("The selected order was not found.");

            _dbContext.Orders.Remove(order);
            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
