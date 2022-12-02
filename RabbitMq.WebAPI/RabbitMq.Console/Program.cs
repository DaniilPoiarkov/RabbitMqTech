using RabbitMq.Console.Abstract;
using RabbitMq.Console.AppBuilder;
using RabbitMq.Console.AppBuilder.CLI.Implementations;
using RabbitMq.Console.Services;

var builder = new ConsoleApplicationBuilder
{
    Title = "Console RabbitMqTech Client",
    ForegroundColor = ConsoleColor.Cyan
};

builder.ConfigureHttpClient(client =>
{
    client.BaseAddress = new("https://localhost:7036");
});

builder.Commands
    .AddCommandTransient<IHttpClientService, HttpClientService>();

builder
    .AddCliCommand<HelpCommand>()
    .AddCliCommand<ClearConsoleCommand>()
    .AddCliCommand<ExitCommand>()
    .AddCliCommand<UserCommand>()
    .AddCliCommand<QueueCommand>()
    .AddCliCommand<NotificationCommand>();

var app = builder.Build();

await app.Run();
