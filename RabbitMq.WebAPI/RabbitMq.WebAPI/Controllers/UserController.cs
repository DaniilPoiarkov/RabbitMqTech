using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RabbitMq.Common.Parameters;
using RabbitMq.Services.Abstract;

namespace RabbitMq.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly UserParameters _userParameters;

        public UserController(IUserService service, UserParameters parameters)
        {
            _service = service;
            _userParameters = parameters;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserById(int id, CancellationToken cancellationToken) =>
             Ok(await _service.GetUserById(id, cancellationToken));

        [HttpGet("email")]
        public async Task<IActionResult> GetUserByEmail(string email, CancellationToken cancellationToken) =>
            Ok(await _service.GetUserByEmail(email, cancellationToken));

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken) =>
            Ok(await _service.GetUserById(_userParameters.UserId, cancellationToken));

        [HttpPut]
        public async Task<IActionResult> SetConnectionId(string connectionId, CancellationToken cancellationToken)
        {
            await _service.SetConnectionId(connectionId, _userParameters.UserId, cancellationToken);
            return NoContent();
        }
    }
}
