using Microsoft.AspNetCore.Mvc;
using RabbitMq.Common.DTOs;
using RabbitMq.Common.DTOs.AuxiliaryModels;
using RabbitMq.Identity.Abstract;
using RabbitMq.WebAPI.Filters;

namespace RabbitMq.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ModelValidation]
    [LastTimeAction]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegister model, CancellationToken token) => 
            Ok(new AccessToken() { Token = await _service.Register(model, token) });

        [HttpPut]
        public async Task<IActionResult> Login([FromBody] UserLogin credentials, CancellationToken token) => 
            Login(await _service.Login(credentials, token));

        [HttpGet]
        public IActionResult Login(UserDto user) => 
            Ok(new AccessToken() { Token = _service.GetToken(user) });

        [HttpPut("reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model, CancellationToken tokem)
        {
            await _service.ResetPassword(model, tokem);
            return NoContent();
        }
    }
}
