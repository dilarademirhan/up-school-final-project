using Application.Features.Products.Commands.Add;
using Application.Features.Products.Queries.GetByOrderId;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  //  [Authorize]
    public class ProductsController : ApiControllerBase
    {
        [HttpPost("Add")]
        public async Task<IActionResult> AddAsync(ProductAddCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("GetByOrderId")]
        public async Task<IActionResult> GetByOrderIdAsync(ProductGetByOrderIdQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
    }

}
