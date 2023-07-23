using FinalProject.Domain.Dtos;
using FinalProject.WebApi.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace FinalProject.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeleniumLogsController : ControllerBase
    {
        private readonly IHubContext<SeleniumLogHub> _seleniumLogHubContext;

        public SeleniumLogsController(IHubContext<SeleniumLogHub> seleniumLogHubContext)
        {
            _seleniumLogHubContext = seleniumLogHubContext;
        }

        [HttpPost]
        public async Task<IActionResult> SendLogNotificationAsync(SendLogNotificationApiDto logNotificationApiDto)
        {
            await _seleniumLogHubContext.Clients.AllExcept(logNotificationApiDto.ConnectionId).SendAsync("NewLogAdded",
                 logNotificationApiDto.Log);
            return Ok();
        }
    }
}
