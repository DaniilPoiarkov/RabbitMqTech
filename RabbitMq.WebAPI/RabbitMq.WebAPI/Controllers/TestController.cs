using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RabbitMq.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public TestController() { }

        [HttpGet]
        public IActionResult Test() => 
            Ok("Test");
    }
}
