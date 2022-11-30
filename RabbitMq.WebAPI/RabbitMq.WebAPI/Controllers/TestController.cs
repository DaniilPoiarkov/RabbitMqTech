using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RabbitMq.Common.Entities.Notifications;
using RabbitMq.Services.Abstract;

namespace RabbitMq.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestController : ControllerBase
    {
        public TestController() { }

        [HttpGet]
        public IActionResult Test() => 
            Ok("Test");
    }
}
