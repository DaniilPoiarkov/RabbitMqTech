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
    .AddCommandTransient<IHttpClientService, HttpClientService>()

    .AddCommandSingleton(new HubConnectionService("https://localhost:7036/notifications"))
    .AddCommandSingleton<CurrentUserService>();

builder
    .AddCliCommand<HelpCommand>()
    .AddCliCommand<ClearConsoleCommand>()
    .AddCliCommand<ExitCommand>()
    .AddCliCommand<AuthCommand>()
    .AddCliCommand<UserCommand>()
    .AddCliCommand<QueueCommand>()
    .AddCliCommand<NotificationCommand>();

builder.Use(context =>
{
    context.Args = context.Args
        .Select(args => args.ToLower())
        .ToArray();

    return context;
});

var app = builder.Build();

await app.Run();
