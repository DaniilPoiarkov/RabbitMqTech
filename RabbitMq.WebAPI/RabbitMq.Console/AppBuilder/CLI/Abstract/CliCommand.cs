using Newtonsoft.Json;
using RabbitMq.Console.TestModels;

namespace RabbitMq.Console.AppBuilder.CLI.Abstract
{
    internal abstract class CliCommand : ICliCommand
    {
        public abstract string ControllerName { get; }

        public abstract string Description { get; }

        public abstract Task Execute(string[] args, ConsoleApplication app);

        protected static async Task<bool> HandleResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return true;

            string error = "Unknown";

            var errorMessage = JsonConvert.DeserializeObject<ErrorMessage>(
                await response.Content.ReadAsStringAsync());

            if (errorMessage != null && !string.IsNullOrEmpty(errorMessage.Error))
                error = errorMessage.Error;

            System.Console.WriteLine("Status code: " + (int)response.StatusCode +
                "\nError: " + error);

            return false;
        }

        protected static bool ValidateBody<TBody>(TBody? value)
        {
            if (value is null)
            {
                System.Console.WriteLine("Error: Cannot deserialize body");
                return false;
            }

            return true;
        }
    }
}
