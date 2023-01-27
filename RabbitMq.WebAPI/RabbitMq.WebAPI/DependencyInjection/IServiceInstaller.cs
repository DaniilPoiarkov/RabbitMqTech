namespace RabbitMq.WebAPI.DependencyInjection;

public interface IServiceInstaller
{
    void InstallService(IServiceCollection services, IConfiguration configuration);
}
