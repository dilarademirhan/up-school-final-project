using Application.Features.Orders.Commands.Add;
using Application.Common.Models.Order;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Products.Queries;
using Microsoft.AspNetCore.Authorization;
using Application.Features.Orders.Queries;
using Application.Features.Orders.Queries.GetByUserId;
using Application.Features.Orders.Commands.Update;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ApiControllerBase
    {
        [HttpPost("Add")]
        public async Task<IActionResult> AddAsync(OrderAddCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("GetByUserId")]
        public async Task<IActionResult> GetByUserIdAsync(OrderGetByUserIdQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpPost("UpdateById")]
        public async Task<IActionResult> UpdateByIdAsync(OrderUpdateCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
