using RabbitMq.Broker.Hubs;
using RabbitMq.WebAPI;
using RabbitMq.WebAPI.DependencyInjection;
using RabbitMq.WebAPI.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ApplyServiceInstallers(
    builder.Configuration,
    typeof(IServiceInstaller).Assembly);

builder.Logging.ClearProviders();

var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Logging.AddSerilog(logger);
builder.Services.AddSingleton<Serilog.ILogger>(logger);

var app = builder.Build();

app.UseMiddleware<ExceptionHandler>(logger);

// This should be commented for integration tests
// Otherwise it will throw following exception:
// System.InvalidOperationException : Relational-specific methods can only be used when the context is using a relational database provider.
// Posible Solution is to use separate Relational-DB for tests. At this moment InMemoryDatabase has been used

//app.ApplyPendingMigrations();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(cors => cors
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
    .WithOrigins(
        "http://localhost:4200",
        "http://localhost:3000"));

app
    .UseHttpsRedirection()
    .UseRouting();

app
    .UseAuthentication()
    .UseAuthorization()
    .UseMiddleware<GetUserParametersMiddleware>();

app.UseEndpoints(cfg =>
{
    cfg.MapControllers();
    cfg.MapHub<NotificationHub>("/notifications");
    cfg.MapHub<SendReminderHub>("/reminder");
});

app.StartQuartzJobs();

app.Run();

// Necessary for integration tests
public partial class Program
{

}
