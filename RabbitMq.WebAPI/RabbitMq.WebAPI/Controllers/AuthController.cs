using Microsoft.AspNetCore.Mvc;
using RabbitMq.Common.DTOs;
using RabbitMq.Common.DTOs.AuxiliaryModels;
using RabbitMq.Identity.Abstract;

namespace RabbitMq.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegister model)
        {
            return Ok(await _service.Register(model));
        }

        [HttpPut]
        public async Task<IActionResult> Login([FromBody] UserLogin credentials)
        {
            return Login(await _service.Login(credentials));
        }

        [HttpGet]
        public IActionResult Login(UserDto user)
        {
            return Ok(_service.GetToken(user));
        }
    }
}
