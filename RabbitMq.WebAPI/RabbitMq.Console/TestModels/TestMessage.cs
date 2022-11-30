
namespace RabbitMq.Console.TestModels
{
    internal class TestMessage
    {
        public string? Message { get; set; }
        public TestMessage(string? message)
        {
            Message = message;
        }

        public override string ToString() => $"TestMessage: Message: {Message}";
    }
}
