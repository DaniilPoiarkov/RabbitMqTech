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

app.ApplyPendingMigrations();

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
});

app.Run();
