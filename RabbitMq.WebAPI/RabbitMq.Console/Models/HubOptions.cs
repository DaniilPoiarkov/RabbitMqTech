
namespace RabbitMq.Console.Models
{
    public class HubOptions
    {
        public string Url { get; set; }

        public HubOptions(string url)
        {
            Url = url;
        }
    }
}
