using RabbitMq.Common.DTOs;

namespace RabbitMq.Console.Abstract
{
    public interface ICurrentUserService
    {
        public UserDto? CurrentUser { get; }
        void SetCurrentUser(UserDto currentUser);
        Task SetCurrentUserFromRequest();
        Task SetUpUserData();
        Task Login();
    }
}
