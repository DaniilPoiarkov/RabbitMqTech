using MediatR;
using RabbitMq.Services.Abstract;
using RabbitMq.Services.MediatoR.User.Requests;

namespace RabbitMq.Services.MediatoR.User.Handlers
{
    public class UpdateAvatarUrlHandler : IRequestHandler<UpdateAvatarUrlRequest, Unit>
    {
        private readonly IUserService _service;
        public UpdateAvatarUrlHandler(IUserService service)
        {
            _service = service;
        }

        public async Task<Unit> Handle(UpdateAvatarUrlRequest request, CancellationToken cancellationToken)
        {
            await _service.UpdateAvatar(request.UserId, request.AvatarUrl ?? "", cancellationToken);
            return Unit.Value;
        }
    }
}
