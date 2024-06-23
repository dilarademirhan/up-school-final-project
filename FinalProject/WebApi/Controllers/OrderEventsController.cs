using Application.Features.OrderEvents.Commands.Add;
using Application.Features.OrderEvents.Commands.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderEventsController : ApiControllerBase
    {
        [HttpPost("Add")]
        public async Task<IActionResult> AddAsync(OrderEventAddCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("GetByOrderId")]
        public async Task<IActionResult> GetByOrderIdAsync(OrderEventGetByOrderIdQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
    }
}
