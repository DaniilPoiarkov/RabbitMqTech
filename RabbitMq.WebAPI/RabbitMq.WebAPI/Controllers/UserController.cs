using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RabbitMq.Common.Parameters;
using RabbitMq.Services.MediatoR.Requests;

namespace RabbitMq.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserParameters _userParameters;

        public UserController(UserParameters parameters, IMediator mediator)
        {
            _mediator = mediator;
            _userParameters = parameters;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserById(int id, CancellationToken cancellationToken) =>
             Ok(await _mediator.Send(new GetUserByIdRequest() { UserId = id }, cancellationToken));

        [HttpGet("email")]
        public async Task<IActionResult> GetUserByEmail(string email, CancellationToken cancellationToken) =>
            Ok(await _mediator.Send(new GetUserByEmailRequest() { UserEmail = email }, cancellationToken));

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken) =>
            Ok(await _mediator.Send(new GetUserByIdRequest() { UserId = _userParameters.UserId }, cancellationToken));

        [HttpPut]
        public async Task<IActionResult> SetConnectionId(string connectionId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new SetConnectionIdRequest() 
            { 
                ConnectionId = connectionId,
                UserId = _userParameters.UserId
            }, cancellationToken);

            return NoContent();
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken) =>
            Ok(await _mediator.Send(new GetAllUsersRequest(), cancellationToken));
    }
}
