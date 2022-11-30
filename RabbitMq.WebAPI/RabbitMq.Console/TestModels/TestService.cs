
namespace RabbitMq.Console.TestModels
{
    public class TestService : ITestService
    {
        private readonly Guid _guid;
        public TestService()
        {
            _guid = Guid.NewGuid();
        }

        public Guid RandomGuid => _guid;
    }
}
