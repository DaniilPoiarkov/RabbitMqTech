using RabbitMq.Broker.Hubs;
using RabbitMq.WebAPI;
using RabbitMq.WebAPI.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegisterServices(builder.Configuration);

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

app.MapHub<NotificationHub>("/notifications");

app.UseCors(cors => cors
    .AllowAnyMethod()
    .AllowAnyOrigin()
    .AllowAnyHeader());

app.UseHttpsRedirection();

app
    .UseAuthentication()
    .UseAuthorization()
    .UseMiddleware<GetUserParametersMiddleware>();

app.MapControllers();

app.Run();
